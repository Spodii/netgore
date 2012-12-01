using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.UI
{
    /// <summary>
    /// A <see cref="Form"/> for showing a list of items for a <see cref="UITypeEditor"/>.
    /// </summary>
    /// <typeparam name="T">The type of listed item.</typeparam>
    public abstract partial class UITypeEditorListForm<T> : Form
    {
        readonly TextFilterContainer _filter = new TextFilterContainer();

        T[] _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="UITypeEditorListForm{T}"/> class.
        /// </summary>
        protected UITypeEditorListForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets all of the available items to choose from.
        /// </summary>
        public IEnumerable<T> AvailableItems
        {
            get { return lstItems.Items.OfType<T>(); }
        }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        public T SelectedItem
        {
            get { return (T)lstItems.SelectedItem; }
        }

        /// <summary>
        /// Gets or sets a <see cref="Func{T,TResult}"/> used to determine what items in the list to skip adding.
        /// </summary>
        public Func<T, bool> SkipItems { get; set; }

        /// <summary>
        /// When overridden in the derived class, draws the <paramref name="item"/>.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawItemEventArgs"/> instance containing the event data.</param>
        /// <param name="item">The item being drawn.</param>
        protected virtual void DrawListItem(DrawItemEventArgs e, T item)
        {
            ControlHelper.DrawItem(e, GetItemDisplayString(item));
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>The string to display for the <paramref name="item"/>.</returns>
        protected abstract string GetItemDisplayString(T item);

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected abstract IEnumerable<T> GetListItems();

        /// <summary>
        /// When overridden in the derived class, allows for handling when the return item has been selected.
        /// </summary>
        /// <param name="item">The item that was selected.</param>
        protected virtual void HandleItemSelected(T item)
        {
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
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Don't try to display items when in design mode
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            _filter.FilterChanged -= _filter_FilterChanged;
            _filter.FilterChanged += _filter_FilterChanged;

            // Set up the search bar
            cmbFilterType.Items.AddRange(TextFilter.GetFilterNames.Cast<object>().ToArray());
            cmbFilterType.SelectedItem = "Text";
            splitContainer1.Panel1Collapsed = true;

            // Set the draw handler
            lstItems.DrawItemHandler = x => DrawListItem(x, (T)lstItems.Items[x.Index]);

            // Set the items
            lstItems.Items.Clear();
            var itemsTemp = GetListItems();

            if (itemsTemp != null)
            {
                if (SkipItems != null)
                    itemsTemp = itemsTemp.Where(x => !SkipItems(x));

                _items = itemsTemp.ToArray();

                lstItems.Items.AddRange(_items.Cast<object>().ToArray());
            }
            else
                _items = null;

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
        /// Handles the KeyDown event of the UITypeEditorListForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void UITypeEditorListForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && e.Modifiers == Keys.Control)
                splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
        }

        /// <summary>
        /// Handles when the filter changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _filter_FilterChanged(TextFilterContainer sender, EventArgs e)
        {
            // Store the currently selected item
            var selected = lstItems.SelectedItem;

            // Clear the list
            lstItems.Items.Clear();

            // Display the filtered list
            var filteredItems = _filter.Filter.FilterItems(_items, GetItemDisplayString);
            lstItems.Items.AddRange(filteredItems.Cast<object>().ToArray());

            // Restore the selected item
            lstItems.SelectedItem = selected;

            // If the previously selected item is not available in the filtered list, just select the first item available
            if (lstItems.SelectedIndex < 0 && lstItems.Items.Count > 0)
                lstItems.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the Click event of the btnApplyFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnApplyFilter_Click(object sender, EventArgs e)
        {
            // Try to get the selected filter name
            var s = cmbFilterType.SelectedItem == null ? null : cmbFilterType.SelectedItem.ToString();

            // Try to change the filter
            bool success;
            if (string.IsNullOrEmpty(s))
                success = _filter.TryChangeFilter(txtFilter.Text);
            else
                success = _filter.TryChangeFilter(txtFilter.Text, s);

            if (!success)
            {
                MessageBox.Show("Invalid filter string specified.");
                return;
            }
        }

        /// <summary>
        /// Handles the DoubleClick event of the lstItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstItems_DoubleClick(object sender, EventArgs e)
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