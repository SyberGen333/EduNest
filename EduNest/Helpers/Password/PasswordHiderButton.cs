namespace EduNest.Helpers.Password
{
    internal class PasswordHiderButton
    {
        private readonly TextBox _textBox;
        private readonly Label _label;
        private bool _isHidden = true;

        public PasswordHiderButton(TextBox textBox, Label label)
        {
            _textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
            _label = label ?? throw new ArgumentNullException(nameof(label));

            // Initialize label appearance
            _label.BackColor = Color.White;
            _label.Cursor = Cursors.Hand;
            _label.TextAlign = ContentAlignment.MiddleCenter;

            // Force start hidden regardless of textbox default
            _isHidden = true;
            _textBox.UseSystemPasswordChar = true;
            UpdateLabel();

            _label.Click += Label_Click;

            // Keep label vertically centered if font or size changes
            _textBox.FontChanged += (s, e) => AlignLabel();
            _textBox.SizeChanged += (s, e) => AlignLabel();
            _label.SizeChanged += (s, e) => AlignLabel();

            AlignLabel();
        }

        private void Label_Click(object? sender, EventArgs e)
        {
            _isHidden = !_isHidden;
            _textBox.UseSystemPasswordChar = _isHidden;
            UpdateLabel();

            // focus back to textbox so typing continues
            _textBox.Focus();
            _textBox.SelectionStart = _textBox.Text.Length;
        }

        private void UpdateLabel()
        {
            _label.Text = _isHidden ? "🔒" : "🔓";
            // Small padding to give breathing room visually
            _label.Padding = new Padding(6, 0, 6, 0);
        }

        private void AlignLabel()
        {
            // Ensure label height matches textbox height and is vertically aligned
            try
            {
                _label.Height = _textBox.Height;
                // Place label at right edge of containing panel (designer should dock it), but ensure font alignment
                _label.Top = _textBox.Top;
            }
            catch
            {
                // ignore alignment errors in designer mode
            }
        }
    }
}
