using System;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    /// <summary>
    /// A general <see cref="ContextMenu"/> for usage on a <see cref="PropertyGrid"/>.
    /// </summary>
    public class GeneralPropertyGridContextMenu : ContextMenu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralPropertyGridContextMenu"/> class.
        /// </summary>
        public GeneralPropertyGridContextMenu() : this(true, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralPropertyGridContextMenu"/> class.
        /// </summary>
        /// <param name="reset">If true, the Reset menu item will be added.</param>
        /// <param name="viewOriginal">If true, the View Original menu item will be added.</param>
        public GeneralPropertyGridContextMenu(bool reset, bool viewOriginal)
        {
            if (reset)
                MenuItems.Add(new MenuItem("Reset", ClickMenuItem_Reset, Shortcut.CtrlShiftZ));

            if (viewOriginal)
                MenuItems.Add(new MenuItem("View Original", ClickMenuItem_ViewOriginal, Shortcut.CtrlShiftX));
        }

        /// <summary>
        /// Handles the Click event of the Reset menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void ClickMenuItem_Reset(object sender, EventArgs e)
        {
            // Get the PropertyGrid that created the menu
            var pg = TryGetMenuSourceControl(sender) as PropertyGrid;
            if (pg == null)
                return;

            // Ensure we have the references that we need
            if (pg.SelectedObject == null || pg.SelectedGridItem == null || pg.SelectedGridItem.PropertyDescriptor == null)
                return;

            // Reset the value
            pg.SelectedGridItem.PropertyDescriptor.ResetValue(pg.SelectedObject);

            // Re-select to force the text to update
            pg.SelectedGridItem = pg.SelectedGridItem;
        }

        /// <summary>
        /// Handles the Click event of the Reset menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void ClickMenuItem_ViewOriginal(object sender, EventArgs e)
        {
            // Get the PropertyGrid that created the menu
            var pg = TryGetMenuSourceControl(sender) as PropertyGrid;
            if (pg == null)
                return;

            // Ensure we have the references that we need
            if (pg.SelectedObject == null || pg.SelectedGridItem == null || pg.SelectedGridItem.PropertyDescriptor == null)
                return;

            var asAdvPropDesc = pg.SelectedGridItem.PropertyDescriptor as AdvancedPropertyDescriptor;
            if (asAdvPropDesc == null)
                return;

            // Show the original value
            MessageBox.Show(string.Format("Original value for `{0}`: {1}", asAdvPropDesc.DisplayName,
                                          asAdvPropDesc.OriginalValue ?? "[NULL]"));
        }

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