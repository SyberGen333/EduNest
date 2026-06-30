using Windows.Security.Credentials.UI;

namespace EduNest.Helpers.Password
{
    public static class WindowsHelloHelper
    {
        public static async Task<(bool Success, string? Error)> RequestWindowsHelloAsync(string message = "Verify using Windows Hello")
        {
            var availability = await UserConsentVerifier.CheckAvailabilityAsync();
            if (availability != UserConsentVerifierAvailability.Available)
                return (false, $"Windows Hello not available: {availability}");

            var result = await UserConsentVerifier.RequestVerificationAsync(message);
            if (result == UserConsentVerificationResult.Verified)
                return (true, null);

            return (false, $"Verification failed: {result}");
        }
    }
}
