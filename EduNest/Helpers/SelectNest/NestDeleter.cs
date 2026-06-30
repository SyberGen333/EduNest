using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace EduNest.Helpers.SelectNest
{
    internal static class NestDeleter
    {
        // Deletes the directory. Uses owner.Invoke to display message boxes on the UI thread when necessary.
        // Returns true when deletion succeeded, false otherwise.
        public static async Task<bool> DeleteNestAsync(Form owner, string fullPath)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (string.IsNullOrEmpty(fullPath)) throw new ArgumentNullException(nameof(fullPath));

            return await Task.Run(() =>
            {
                try
                {
                    try
                    {
                        FileSystem.DeleteDirectory(fullPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                    catch (Exception recycleEx) when (recycleEx is UnauthorizedAccessException || recycleEx is IOException)
                    {
                        // Try clearing read-only attributes and retry recycle
                        try
                        {
                            ClearReadOnlyAttributesRecursive(fullPath);
                            FileSystem.DeleteDirectory(fullPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        }
                        catch (Exception retryEx) when (retryEx is UnauthorizedAccessException || retryEx is IOException)
                        {
                            // Ask user whether to permanently delete after failure to recycle
                            var perma = (DialogResult)owner.Invoke(new Func<DialogResult>(() =>
                                MessageBox.Show(owner, "Could not move to Recycle Bin (permission or in-use). Try permanent delete?\r\n\r\n" + retryEx.Message, "Delete permanently?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                            ));
                            if (perma != DialogResult.Yes)
                                return false;

                            try
                            {
                                // ensure attributes cleared then delete
                                ClearReadOnlyAttributesRecursive(fullPath);
                                Directory.Delete(fullPath, true);
                            }
                            catch (Exception finalEx)
                            {
                                owner.Invoke(new Action(() => MessageBox.Show(owner, "Permanent delete failed: \r\n" + finalEx.Message + "\r\n\r\nTry closing any apps using files in the folder or run the app as Administrator.", "Delete failed", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                                return false;
                            }
                        }
                    }
                    catch (Exception recycleEx)
                    {
                        // Non-IO/permission exception when trying to recycle
                        owner.Invoke(new Action(() => MessageBox.Show(owner, "Delete failed: \r\n" + recycleEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    owner.Invoke(new Action(() => MessageBox.Show(owner, "Failed to delete nest:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                    return false;
                }
            }).ConfigureAwait(false);
        }

        // Helper: clear ReadOnly attribute for all files and directories under path
        private static void ClearReadOnlyAttributesRecursive(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) return;

            // directories
            foreach (var dir in Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories).Concat(new[] { path }))
            {
                try
                {
                    var attrs = File.GetAttributes(dir);
                    if ((attrs & FileAttributes.ReadOnly) != 0)
                    {
                        attrs &= ~FileAttributes.ReadOnly;
                        File.SetAttributes(dir, attrs);
                    }
                }
                catch { /* ignore individual failures */ }
            }

            // files
            foreach (var file in Directory.GetFiles(path, "*", System.IO.SearchOption.AllDirectories))
            {
                try
                {
                    var attrs = File.GetAttributes(file);
                    if ((attrs & FileAttributes.ReadOnly) != 0)
                    {
                        attrs &= ~FileAttributes.ReadOnly;
                        File.SetAttributes(file, attrs);
                    }
                }
                catch { /* ignore individual failures */ }
            }
        }
    }
}
