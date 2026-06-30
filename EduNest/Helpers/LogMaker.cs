using System;
using System.IO;
using System.Text;

namespace EduNest.Helpers
{
    /// <summary>
    /// Simple file logger that writes timestamped actions to daily log files.
    /// Files are created under the application's "logs" subfolder as "log-YYYY-MM-DD.txt".
    /// Safe for concurrent calls within the same process.
    /// </summary>
    internal static class LogMaker
    {
        private static readonly object _sync = new object();

        /// <summary>
        /// Append a timestamped action line to today's log file.
        /// This method swallows exceptions to avoid affecting the host application.
        /// </summary>
        /// <param name="action">A short description of the action to log.</param>
        public static void Log(string action)
        {
            try
            {
                // Respect user setting: if logging disabled, do nothing.
                if (!EduNest.Settings.Default.DoLogAdding)
                    return;

                if (string.IsNullOrWhiteSpace(action)) return;

                var baseDir = AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory;
                var logsDir = Path.Combine(baseDir, "logs");
                Directory.CreateDirectory(logsDir);

                var fileName = $"log-{DateTime.Now:yyyy-MM-dd}.txt";
                var filePath = Path.Combine(logsDir, fileName);

                var line = $"[{DateTime.Now:yyyy-MM-dd-HH-mm-ss}] {action}";

                lock (_sync)
                {
                    File.AppendAllText(filePath, line + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch
            {
                // Intentionally swallow exceptions — logging must not break app execution.
            }
        }

        /// <summary>
        /// Convenience helper to log exceptions with optional context.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="context">Optional context message.</param>
        public static void LogException(Exception ex, string context = null)
        {
            if (ex == null) return;
            var ctx = string.IsNullOrWhiteSpace(context) ? "Exception" : context;
            var message = $"{ctx}: {ex.GetType().Name}: {ex.Message}";
            Log(message);
            // Optionally include stack trace on the next line (keeps file readable)
            if (!string.IsNullOrWhiteSpace(ex.StackTrace))
            {
                Log($"StackTrace: {ex.StackTrace.Replace(Environment.NewLine, " | ")}");
            }
        }
    }
}
