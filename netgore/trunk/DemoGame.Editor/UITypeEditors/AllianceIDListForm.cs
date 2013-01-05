using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Server;
using NetGore;
using NetGore.Editor;

namespace DemoGame.Editor.UITypeEditors
{
    public partial class AllianceIDListForm : Form
    {
        readonly List<AllianceID> _list;

        AllianceID? _selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceIDListForm"/> class.
        /// </summary>
        /// <param name="list">The list of <see cref="AllianceID"/>s and amounts.</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public AllianceIDListForm(List<AllianceID> list)
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
        /// Gets or sets if the <see cref="ItemTemplateID"/>s must be distinct. Default value is true.
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
            _list.AddRange(lstItems.Items.OfType<AllianceID>());

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
                if (lstItems.Items.OfType<AllianceID>().Any(x => _selectedItem.HasValue && x == _selectedItem.Value))
                {
                    MessageBox.Show("That item is already in the list.");
                    _selectedItem = null;
                    return;
                }
            }

            // Add
            lstItems.Items.Add(_selectedItem.Value);

            // Select the new item
            lstItems.SelectedItem = _selectedItem.Value;

            if (RequireDistinct)
            {
                _selectedItem = null;
                txtItem.Text = string.Empty;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnBrowse control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var f = new AllianceUITypeEditorForm(null))
            {
                // If we require distinct, skip items we already have in the list
                if (RequireDistinct)
                {
                    var listItems = lstItems.Items.OfType<AllianceID>().ToImmutable();
                    f.SkipItems = (x => listItems.Any(y => y == x.ID));
                }

                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                var item = f.SelectedItem;
                _selectedItem = item.ID;

                txtItem.Text = item.ID + " [" + item.Name + "]";
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedItem == null)
                return;

            var selID = (AllianceID)lstItems.SelectedItem;

            Alliance sel = AllianceManager.Instance[selID];
            if (sel == null)
                return;
 
            string msg = string.Format("Are you sure you wish to remove the alliance `{0}` from the list?", sel);
            if (MessageBox.Show(msg, "Remove alliance from list?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            lstItems.RemoveItemAtAndReselect(lstItems.SelectedIndex);
        }
    }
}