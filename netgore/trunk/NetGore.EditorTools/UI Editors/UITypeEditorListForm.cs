using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    /// <summary>
    /// A <see cref="Form"/> for showing a list of items for a <see cref="UITypeEditor"/>.
    /// </summary>
    /// <typeparam name="T">The type of listed item.</typeparam>
    public partial class UITypeEditorListForm<T> : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UITypeEditorListForm{T}"/> class.
        /// </summary>
        public UITypeEditorListForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        public T SelectedItem { get { return (T)lstItems.SelectedItem; } }

        /// <summary>
        /// When overridden in the derived class, draws the <paramref name="item"/>.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawItemEventArgs"/> instance containing the event data.</param>
        /// <param name="item">The item being drawn.</param>
        protected virtual void DrawListItem(DrawItemEventArgs e, T item)
        {
            e.DrawBackground();
            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(item.ToString(), e.Font, brush, e.Bounds);
            }

            if (e.State == DrawItemState.Selected)
                e.DrawFocusRectangle();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Don't try to display items when in design mode
            if (DesignMode)
                return;

            // Set the draw handler
            lstItems.DrawItemHandler = x => DrawListItem(x, (T)lstItems.Items[x.Index]);

            // Set the items
            lstItems.Items.Clear();
            var items = GetListItems();
            if (items != null)
                lstItems.Items.AddRange(items.Cast<object>().ToArray());

            // Set the default selected item
            lstItems.SelectedItem = SetDefaultSelectedItem(lstItems.Items.Cast<T>());
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected virtual T SetDefaultSelectedItem(IEnumerable<T> items)
        {
            return items.FirstOrDefault();
        }

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="item"/> is valid to be
        /// used as the returned item.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        /// <returns>If the given <paramref name="item"/> is valid to be used as the returned item.</returns>
        protected virtual bool IsItemValid(T item)
        {
            return true;
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the return item has been selected.
        /// </summary>
        /// <param name="item">The item that was selected.</param>
        protected virtual void HandleItemSelected(T item)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected virtual IEnumerable<T> GetListItems()
        {
            return null;
        }

        /// <summary>
        /// Handles the DoubleClick event of the lstItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lstItems_DoubleClick(object sender, EventArgs e)
        {
            // Check for a valid selection range
            if (lstItems.SelectedIndex < 0 || lstItems.SelectedIndex >= lstItems.Items.Count)
                return;

            // Get the item
            var item = (T)lstItems.SelectedItem;

            // Check for a valid item
            if (!IsItemValid(item))
                return;

            HandleItemSelected(item);

            DialogResult = DialogResult.OK;
        }
    }
}
