using System.Text;
using System.Text.RegularExpressions;

namespace EduNest.Helpers
{
    public static class NestNameHelper
    {
        // Matches "-yyyy-MM-dd-HH-mm-ss" at the end
        private static readonly Regex TsSuffix = new Regex("-\\d{4}-\\d{2}-\\d{2}-\\d{2}-\\d{2}-\\d{2}$", RegexOptions.Compiled);

        // Remove invalid filename chars and dash from the user-typed name
        public static string SanitizeUserName(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var invalid = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(input.Length);
            foreach (var ch in input)
            {
                if (ch == '-') continue; // do not allow dash in name
                if (invalid.Contains(ch)) continue;
                sb.Append(ch);
            }
            // trim spaces at ends, collapse inner multiple spaces
            return Regex.Replace(sb.ToString().Trim(), "\\s{2,}", " ");
        }

        // Build the canonical folder name: "<name>-<course>-yyyy-MM-dd-HH-mm-ss"
        public static string BuildFolderName(string cleanName, string courseCode, DateTime dt)
        {
            var ts = dt.ToString("yyyy-MM-dd-HH-mm-ss");
            return $"{cleanName}-{courseCode}-{ts}";
        }

        // Try to parse "<name>-<course>-yyyy-MM-dd-HH-mm-ss"
        // Important: we split at the FIRST dash before the timestamp,
        // because the name has no dashes and the course may have dashes.
        public static bool TryParseNameAndCourse(string folderName, out string name, out string course)
        {
            name = string.Empty;
            course = string.Empty;

            var m = TsSuffix.Match(folderName);
            if (!m.Success) return false;

            var beforeTs = folderName.Substring(0, m.Index); // "<name>-<course>"
            var firstDash = beforeTs.IndexOf('-');
            if (firstDash <= 0 || firstDash >= beforeTs.Length - 1) return false;

            name = beforeTs.Substring(0, firstDash).Trim();
            course = beforeTs.Substring(firstDash + 1).Trim();

            // Basic course code validation: must not contain invalid filename chars
            if (course.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return false;

            return true;
        }

        // For UI: "name (COURSE)" or fallback to raw folderName
        public static string FormatDisplay(string folderName)
        {
            if (TryParseNameAndCourse(folderName, out var name, out var course))
                return $"{name} ({course})";
            return folderName;
        }

        // Returns just the course code, or null if not parseable
        public static string CourseCodeCheck(string folderName)
        {
            return TryParseNameAndCourse(folderName, out _, out var course) ? course : null;
        }
    }
}
