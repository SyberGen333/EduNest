using System;
using System.IO;
using System.IO.Compression;

namespace EduNest.Helpers.Main
{
    public static class UnzipperBeforeOpen
    {
        /// <summary>
        /// Handles loading EduNest folder from EduNest.zip if needed.
        /// </summary>
        public static void LoadEduNestOnStartup()
        {
            string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string rootFolder = Path.Combine(docs, "EduNest");
            string zipPath = Path.Combine(docs, "EduNest.zip");

            bool folderExists = Directory.Exists(rootFolder);
            bool zipExists = File.Exists(zipPath);

            // CASE 1: Folder exists and NOT empty → ignore ZIP
            if (folderExists && Directory.EnumerateFileSystemEntries(rootFolder).Any())
            {
                NormalizeEduNestNaming(rootFolder);
                return;
            }

            // CASE 2: Folder exists but empty → load ZIP
            if (folderExists && !Directory.EnumerateFileSystemEntries(rootFolder).Any())
            {
                if (zipExists)
                {
                    ExtractZipWithProgress(rootFolder, zipPath);
                    NormalizeEduNestNaming(rootFolder);
                }
                return;
            }

            // CASE 3: No folder but ZIP exists
            if (!folderExists && zipExists)
            {
                Directory.CreateDirectory(rootFolder);
                ExtractZipWithProgress(rootFolder, zipPath);
                NormalizeEduNestNaming(rootFolder);
                return;
            }

            // CASE 4: No folder + No ZIP → fresh empty folder
            if (!folderExists)
                Directory.CreateDirectory(rootFolder);
        }

        private static void ExtractZipWithProgress(string outputFolder, string zipPath)
        {
            using (var p = new frmProgressBar("Loading EduNest..."))
            {
                p.Show();
                p.UpdateStatus("Extracting...", 10);

                try
                {
                    ZipFile.ExtractToDirectory(zipPath, outputFolder, true);
                    p.UpdateStatus("Completed", 100);
                }
                catch
                {
                    Directory.CreateDirectory(outputFolder);
                    p.SetCompleted("Failed to load ZIP, created fresh folder instead.");
                }
            }
        }

        /// <summary>
        /// Fixes incorrect extracted folder like EduNest/Edunest → EduNest
        /// </summary>
        private static void NormalizeEduNestNaming(string rootFolder)
        {
            string wrong = Path.Combine(Path.GetDirectoryName(rootFolder), "Edunest");

            if (Directory.Exists(wrong) && !Directory.Exists(rootFolder))
            {
                Directory.Move(wrong, rootFolder);
            }
        }
    }
}
