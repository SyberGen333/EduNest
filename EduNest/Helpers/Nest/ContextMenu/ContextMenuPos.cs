namespace EduNest.Helpers.Nest.ContextMenu
{
    public class ContextMenuPos
    {
        public static void AtBottomRight(ContextMenuStrip menu)
        {
            if (menu == null) return;

            // Use current cursor position
            Point cursorPosition = Cursor.Position;

            // Get screen bounds where cursor is
            Screen screen = Screen.FromPoint(cursorPosition);
            Rectangle workingArea = screen.WorkingArea;

            int x = cursorPosition.X + 1;
            int y = cursorPosition.Y + 1;

            Size menuSize = menu.GetPreferredSize(Size.Empty);

            // Flip vertically if it would overflow bottom
            if (y + menuSize.Height > workingArea.Bottom)
                y -= menuSize.Height;

            // Flip horizontally if it would overflow right
            if (x + menuSize.Width > workingArea.Right)
                x -= menuSize.Width;

            menu.Show(new Point(x, y));
        }

    }
}
