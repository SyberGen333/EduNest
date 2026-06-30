using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace EduNest.Helpers.Main
{
    public static class ZipperBeforeClose
    {
        /// <summary>
        /// Creates EduNest.zip and deletes folder. Also creates a timestamped backup.
        /// </summary>
        public static async Task ZipAndDeleteAsync(string sourceFolder, string zipPath, frmProgressBar dlg)
        {
            if (!Directory.Exists(sourceFolder))
                return;

            dlg.UpdateStatus("Preparing backup...", 5);

            // 1. Create the main EduNest.zip
            dlg.UpdateStatus("Compressing EduNest folder...", 15);

            string temp = zipPath + ".tmp";

            if (File.Exists(temp)) File.Delete(temp);
            if (File.Exists(zipPath)) File.Delete(zipPath);

            await Task.Run(() =>
            {
                ZipFile.CreateFromDirectory(sourceFolder, temp, CompressionLevel.Optimal, includeBaseDirectory: true);
            });

            dlg.UpdateStatus("Finalizing compression...", 60);

            File.Move(temp, zipPath, true);

            // 2. Create timestamped backup: EduNestBackup-YYYY-MM-DD-hh-mm-ss.zip
            string docs = Path.GetDirectoryName(zipPath);
            string backupName = "EduNestBackup-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".zip";
            string backupPath = Path.Combine(docs, backupName);

            dlg.UpdateStatus("Creating auto-backup...", 75);

            File.Copy(zipPath, backupPath, true);

            // 3. Keep only 3 newest backups
            dlg.UpdateStatus("Cleaning old backups...", 90);

            var backups = Directory.GetFiles(docs, "EduNestBackup-*.zip")
                                   .OrderByDescending(f => f)
                                   .ToList();

            if (backups.Count > 3)
            {
                foreach (var old in backups.Skip(3))
                    File.Delete(old);
            }

            dlg.UpdateStatus("Backup complete!", 100);
        }
    }
}
