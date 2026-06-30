using System.Text.RegularExpressions;

public static class NestHelpers
{
    // Attempts to split folderName into (namePart, courseCode) given the timestamp suffix pattern.
    // knownCourseCodes is optional; if provided, we try to match one of them (longest-first).
    public static (string namePart, string courseCode) CourseCodeCheck(string folderName, IEnumerable<string> knownCourseCodes = null)
    {
        if (string.IsNullOrEmpty(folderName))
            return (folderName ?? string.Empty, string.Empty);

        // timestamp pattern at the end: -yyyy-MM-dd-HH-mm-ss
        var tsPattern = "-\\d{4}-\\d{2}-\\d{2}-\\d{2}-\\d{2}-\\d{2}$";
        var match = Regex.Match(folderName, tsPattern);
        if (!match.Success)
        {
            // Can't find timestamp — fallback: try to split by last dash
            var lastDashFallback = folderName.LastIndexOf('-');
            if (lastDashFallback >= 0)
            {
                var namePart = folderName.Substring(0, lastDashFallback).Trim();
                var courseCode = folderName.Substring(lastDashFallback + 1).Trim();
                return (namePart, courseCode);
            }
            return (folderName, string.Empty);
        }

        var beforeTs = folderName.Substring(0, match.Index); // e.g. "MyName-BSCS-OM"

        // 1) If we have known course codes, try to match longest first.
        if (knownCourseCodes != null)
        {
            var codes = knownCourseCodes
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(c => c.Length);

            foreach (var code in codes)
            {
                // check both with and without a preceding dash (robust)
                if (beforeTs.EndsWith("-" + code, StringComparison.OrdinalIgnoreCase))
                {
                    var namePart = beforeTs.Substring(0, beforeTs.Length - (code.Length + 1)).Trim();
                    return (namePart, code);
                }
                if (beforeTs.EndsWith(code, StringComparison.OrdinalIgnoreCase))
                {
                    var namePart = beforeTs.Substring(0, beforeTs.Length - code.Length).Trim();
                    // strip trailing dash if present
                    if (namePart.EndsWith("-")) namePart = namePart.Substring(0, namePart.Length - 1).Trim();
                    return (namePart, code);
                }
            }
        }

        // 2) Heuristic fallback: split by '-' and try combinations from the end.
        // We consider the last 1..3 segments as candidate course codes (join with '-').
        var parts = beforeTs.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim()).ToArray();

        if (parts.Length == 0)
            return (string.Empty, string.Empty);
        if (parts.Length == 1)
            return (string.Empty, parts[0]);

        // build candidates: prefer the longest (more segments) that matches a course-like pattern
        // course-like pattern: uppercase letters/digits and maybe separated by '-' (e.g., BSCS-OM)
        var courseTokenPattern = new Regex(@"^[A-Z0-9]+(?:-[A-Z0-9]+)*$", RegexOptions.IgnoreCase);

        for (int take = Math.Min(3, parts.Length - 1); take >= 1; take--)
        {
            // take last 'take' parts as candidate course code
            var candidateParts = parts.Skip(parts.Length - take).Take(take);
            var candidate = string.Join("-", candidateParts);

            if (courseTokenPattern.IsMatch(candidate))
            {
                // name part is everything before those candidate parts
                var namePart = string.Join("-", parts.Take(parts.Length - take)).Trim();
                return (namePart, candidate);
            }
        }

        // Final fallback: assume last segment is course code
        var last = parts.Last();
        var nameFallback = string.Join("-", parts.Take(parts.Length - 1)).Trim();
        return (nameFallback, last);
    }
}
