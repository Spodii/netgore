using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Editor;

namespace DemoGame.Editor.UITypeEditors
{
    public partial class CharacterTemplateAndAmountListForm : Form
    {
        readonly List<MutablePair<CharacterTemplateID, ushort>> _list;

        CharacterTemplateID? _selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateAndAmountListForm"/> class.
        /// </summary>
        /// <param name="list">The list of <see cref="CharacterTemplateID"/>s and amounts.</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public CharacterTemplateAndAmountListForm(List<MutablePair<CharacterTemplateID, ushort>> list)
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
        /// Gets or sets if the <see cref="CharacterTemplateID"/>s must be distinct. Default value is true.
        /// </summary>
        public bool RequireDistinct { get; set; }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DesignMode)
                return;

            _list.Clear();
            _list.AddRange(lstItems.Items.OfType<MutablePair<CharacterTemplateID, ushort>>());

            base.OnClosing(e);
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate
            if (!_selectedItem.HasValue)
            {
                MessageBox.Show("You must select an item to add first.");
                return;
            }

            if (RequireDistinct)
            {
                if (
                    lstItems.Items.Cast<MutablePair<CharacterTemplateID, ushort>>().Any(
                        x => _selectedItem.HasValue && x.Key == _selectedItem.Value))
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
            var newItem = new MutablePair<CharacterTemplateID, ushort>(_selectedItem.Value, amount);
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
        /// Handles the Click event of the btnBrowse control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var f = new CharacterTemplateUITypeEditorForm(null))
            {
                // If we require distinct, skip items we already have in the list
                if (RequireDistinct)
                {
                    var listItems = lstItems.Items.OfType<MutablePair<CharacterTemplateID, ushort>>().ToImmutable();
                    f.SkipItems = (x => listItems.Any(y => y.Key == x.ID));
                }

                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                var item = f.SelectedItem;
                _selectedItem = item.ID;
                txtItem.Text = item.ID + " [" + item.Name + "]";
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the lstItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void lstItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                txtAmount_Leave(sender, null);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected item
            var sel = lstItems.SelectedItem as MutablePair<CharacterTemplateID, ushort>;
            if (sel == null)
                return;

            txtAmount.Text = sel.Value.ToString();
        }

        /// <summary>
        /// Handles the Leave event of the txtAmount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtAmount_Leave(object sender, EventArgs e)
        {
            // Get the selected item
            var sel = lstItems.SelectedItem as MutablePair<CharacterTemplateID, ushort>;
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
    }
}