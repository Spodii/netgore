using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// Helper methods for menus.
    /// </summary>
    public static class MenuHelper
    {
        /// <summary>
        /// Tries to get the <see cref="Control"/> that a menu item originates from.
        /// </summary>
        /// <param name="sender">The menu item.</param>
        /// <returns>The <see cref="Control"/> that the <paramref name="sender"/> menu item was invoked on;
        /// otherwise null.</returns>
        public static Control TryGetMenuSourceControl(object sender)
        {
            if (sender == null)
                return null;

            if (sender is ToolStripItem)
                return TryGetMenuSourceControl(((ToolStripItem)sender).Owner);

            if (sender is MenuItem)
                return TryGetMenuSourceControl(((MenuItem)sender).Parent);

            if (sender is ContextMenuStrip)
                return ((ContextMenuStrip)sender).SourceControl;

            if (sender is ContextMenu)
                return ((ContextMenu)sender).SourceControl;

            return null;
        }
    }
}