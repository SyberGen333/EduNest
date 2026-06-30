namespace EduNest.Helpers
{
    // Small helper to get/set LastNestOpened in user settings
    public static class SettingsHelper
    {
        private static string GetFullNestsBasePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EduNest", "Nests");
        }

        public static string GetSavedLastNestPath()
        {
            var val = EduNest.Settings.Default.LastNestOpened;
            if (string.IsNullOrWhiteSpace(val)) return null;

            // If value is a folder name (no path separators), assume it's relative to base nests folder
            if (!val.Contains(Path.DirectorySeparatorChar) && !val.Contains(Path.AltDirectorySeparatorChar))
            {
                var candidate = Path.Combine(GetFullNestsBasePath(), val);
                if (Directory.Exists(candidate)) return candidate;
                return null;
            }

            // If it's a path, verify existence
            if (Directory.Exists(val)) return val;

            return null;
        }

        public static void SaveLastNest(string folderPathOrName)
        {
            // store just the folder name if it's under the default Nests folder, otherwise store full path
            if (string.IsNullOrWhiteSpace(folderPathOrName)) return;

            string baseNests = GetFullNestsBasePath();

            try
            {
                if (folderPathOrName.StartsWith(baseNests, StringComparison.OrdinalIgnoreCase))
                {
                    // save relative folder name
                    var name = Path.GetFileName(folderPathOrName.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                    EduNest.Settings.Default.LastNestOpened = name;
                }
                else
                {
                    EduNest.Settings.Default.LastNestOpened = folderPathOrName;
                }

                EduNest.Settings.Default.Save();
            }
            catch
            {
                // ignore failures silently
            }
        }
    }
}