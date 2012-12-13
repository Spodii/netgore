using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using NetGore;

namespace DemoGame.Editor.UITypeEditors
{
    public partial class CharacterTemplateEquippedItemListForm : Form
    {
        readonly List<CharacterTemplateEquippedItem> _list;

        ItemTemplateID? _selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateAndAmountListForm"/> class.
        /// </summary>
        /// <param name="list">The list of <see cref="CharacterTemplateID"/>s and amounts.</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public CharacterTemplateEquippedItemListForm(List<CharacterTemplateEquippedItem> list)
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
            _list.AddRange(lstItems.Items.OfType<CharacterTemplateEquippedItem>());

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
                if (lstItems.Items.Cast<CharacterTemplateEquippedItem>().Any(x => _selectedItem.HasValue && x.ID == _selectedItem.Value))
                {
                    MessageBox.Show("That item is already in the list.");
                    _selectedItem = null;
                    return;
                }
            }

            // Add
            var newItem = new CharacterTemplateEquippedItem(_selectedItem.Value, ItemChance.FromPercent(1.0f));
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
            using (var f = new ItemTemplateUITypeEditorForm(null))
            {
                // If we require distinct, skip items we already have in the list
                if (RequireDistinct)
                {
                    var listItems = lstItems.Items.OfType<CharacterTemplateEquippedItem>().ToImmutable();
                    f.SkipItems = (x => listItems.Any(y => y.ID == x.ID));
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
                txtChance_Leave(sender, null);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstItems.SelectedItem == null)
                return;

            // Get the selected item
            if (!(lstItems.SelectedItem is CharacterTemplateEquippedItem))
            {
                Debug.Fail("Was expecting type " + typeof(CharacterTemplateEquippedItem));
                return;
            }

            var sel = (CharacterTemplateEquippedItem)lstItems.SelectedItem;

            txtChance.Text = Math.Round(sel.Chance.Percentage * 100f, 4).ToString();
        }

        /// <summary>
        /// Handles the Leave event of the txtAmount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtChance_Leave(object sender, EventArgs e)
        {
            if (lstItems.SelectedItem == null)
                return;

            // Get the selected item
            if (!(lstItems.SelectedItem is CharacterTemplateEquippedItem))
            {
                Debug.Fail("Was expecting type " + typeof(CharacterTemplateEquippedItem));
                return;
            }

            var sel = (CharacterTemplateEquippedItem)lstItems.SelectedItem;

            // Parse the new amount
            float chancePct;
            if (!float.TryParse(txtChance.Text, out chancePct))
                return;

            ItemChance newItemChance = ItemChance.FromPercent(chancePct / 100f);

            // Check that the amount changed
            if (sel.Chance == newItemChance)
                return;

            // Set the new amount
            sel.Chance = newItemChance;

            // Force the text to refresh
            lstItems.Items[lstItems.SelectedIndex] = sel;
        }
    }
}