using System.Net.NetworkInformation;
using Windows.Security.Credentials.UI;

namespace EduNest.Helpers.Password
{
    // Public static helper to manage forgot-password tokens and sending verification emails
    public static class ForgotPassword
    {
        // Five always-valid tokens (random letters and numbers)
        public static readonly List<string> Tokens = new List<string>
        {
            "ah1nU289dhbs222bWeud",
            "Qx8mZ74pLk2s90vRty6b",
            "nV7cB3s9Ew1pT5zYq4Hr",
            "m9Kp2S8dL1xF6uV0aZr3",
            "zA2b4C6d8E1f0G3h5Jk7"
        };

        private static readonly Random _rnd = new Random();

        // Pick a random token from the list
        public static string PickRandomToken()
        {
            return Tokens[_rnd.Next(Tokens.Count)];
        }

        // Convenience method to check whether an input matches one of the master tokens
        public static bool IsMasterToken(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            return Tokens.Contains(input);
        }

        // Convenience wrapper to request Windows Hello verification from .NET code
        public static async Task<(bool Success, bool Cancelled, string? Error)> VerifyWithWindowsHelloAsync(string message = "Verify using Windows Hello")
        {
            var availability = await UserConsentVerifier.CheckAvailabilityAsync();
            if (availability != UserConsentVerifierAvailability.Available)
                return (false, false, $"Windows Hello not available: {availability}");

            var result = await UserConsentVerifier.RequestVerificationAsync(message);
            if (result == UserConsentVerificationResult.Verified)
                return (true, false, null);

            if (result == UserConsentVerificationResult.Canceled)
                return (false, true, "Verification canceled by user.");

            return (false, false, $"Verification failed: {result}");
        }

        // SMTP/token sending removed. Use Windows Hello verification or implement a secure server-side
        // workflow to send one-time tokens. This helper intentionally does not include any hard-coded
        // accounts or SMTP credentials.

        // Synchronous check for internet connectivity using ICMP ping to a reliable host
        // Falls back to NetworkInterface.GetIsNetworkAvailable when ping is not permitted.
        public static bool IsOnline(int timeoutMs = 1500)
        {
            try
            {
                using var pinger = new Ping();
                var reply = pinger.Send("8.8.8.8", timeoutMs);
                if (reply != null && reply.Status == IPStatus.Success)
                    return true;
            }
            catch
            {
                // Ignore and try fallback
            }

            try
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
            catch
            {
                return false;
            }
        }
    }
}
