﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DemoGame.EditorTools;
using NetGore;
using NetGore.EditorTools;

namespace DemoGame.EditorTools
{
    public partial class ItemTemplateAndAmountListForm : Form
    {
        readonly List<MutablePair<ItemTemplateID, byte>> _list;

        ItemTemplateID? _selectedItem;

        /// <summary>
        /// Gets or sets if the <see cref="ItemTemplateID"/>s must be distinct. Default value is true.
        /// </summary>
        public bool RequireDistinct { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemTemplateAndAmountListForm"/> class.
        /// </summary>
        /// <param name="list">The list of <see cref="ItemTemplateID"/>s and amounts.</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is null.</exception>
        public ItemTemplateAndAmountListForm(List<MutablePair<ItemTemplateID, byte>> list)
        {
            if (DesignMode)
                return;

            if (list == null)
                throw new ArgumentNullException("list");

            _list = list;

            RequireDistinct = true;

            InitializeComponent();

            lstItems.Items.AddRange(list.Cast<object>().ToArray());
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DesignMode)
                return;

            _list.Clear();
            _list.AddRange(lstItems.Items.OfType<MutablePair<ItemTemplateID, byte>>());

            base.OnClosing(e);
        }

        /// <summary>
        /// Handles the Click event of the btnBrowse control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var f = new ItemTemplateUITypeEditorForm(null))
            {
                // If we require distinct, skip items we already have in the list
                if (RequireDistinct)
                {
                    var listItems = lstItems.Items.OfType<MutablePair<ItemTemplateID, byte>>().ToImmutable();
                    f.SkipItems = (x => !listItems.Any(y => y.Key == x.ID));
                }

                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                var item = f.SelectedItem;
                txtItem.Text = item.ID + " [" + item.Name + "]";
            }
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate
            if (!_selectedItem.HasValue)
            {
                MessageBox.Show("You must select an item to add first.");
                return;
            }

            if (RequireDistinct)
            {
                if (lstItems.Items.OfType<MutablePair<ItemTemplateID, byte>>().Any(x => x.Key == _selectedItem.Value))
                {
                    MessageBox.Show("That item is already in the list.");
                    _selectedItem = null;
                    return;
                }
            }

            byte amount;
            if (!byte.TryParse(txtAmount.Text, out amount))
            {
                MessageBox.Show("Invalid amount specified.");
                return;
            }

            // Add
            var newItem = new MutablePair<ItemTemplateID, byte>(_selectedItem.Value, amount);
            lstItems.Items.Add(newItem);

            if (RequireDistinct)
            {
                _selectedItem = null;
                txtItem.Text = string.Empty;
            }

            // Select the new item
            lstItems.SelectedItem = newItem;
        }

        /// <summary>
        /// Handles the Leave event of the txtAmount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtAmount_Leave(object sender, EventArgs e)
        {
            // Get the selected item
            var sel = lstItems.SelectedItem as MutablePair<ItemTemplateID, byte>;
            if (sel == null)
                return;

            // Parse the new amount
            byte amount;
            if (!byte.TryParse(txtAmount.Text, out amount))
                return;

            // Check that the amount changed
            if (sel.Value == amount)
                return;

            // Set the new amount
            sel.Value = amount;
            
            // Force the text to refresh
            lstItems.Items[lstItems.SelectedIndex] = sel;
        }

        /// <summary>
        /// Handles the KeyDown event of the lstItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void lstItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                txtAmount_Leave(sender, null);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected item
            var sel = lstItems.SelectedItem as MutablePair<ItemTemplateID, byte>;
            if (sel == null)
                return;

            txtAmount.Text = sel.Value.ToString();
        }
    }
}