public static class SearchHelper
{
    public static void HandleTextChanged(System.Windows.Forms.Timer searchTimer)
    {
        if (searchTimer == null) return;
        searchTimer.Stop();
        searchTimer.Start(); // restart debounce
    }

    public static void HandleKeyPress(KeyPressEventArgs e)
    {
        if (e == null) return;

        // Allow control keys (like Backspace, Enter, etc.)
        if (char.IsControl(e.KeyChar))
            return;

        char[] invalidChars = Path.GetInvalidFileNameChars();
        if (invalidChars.Contains(e.KeyChar))
        {
            e.Handled = true; // block invalid character
            System.Media.SystemSounds.Beep.Play();
        }
    }
}
