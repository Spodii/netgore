using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using NetGore;

namespace DemoGame.Editor.UITypeEditors
{
    public partial class SkillTypeListForm : Form
    {
        readonly List<SkillType> _list;

        SkillType? _selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillTypeListForm"/> class.
        /// </summary>
        /// <param name="list">The list of <see cref="SkillType"/>s and amounts.</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public SkillTypeListForm(List<SkillType> list)
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
        /// Gets or sets if the <see cref="SkillType"/>s must be distinct. Default value is true.
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
            _list.AddRange(lstItems.Items.OfType<SkillType>());

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
                if (lstItems.Items.OfType<SkillType>().Any(x => _selectedItem.HasValue && x == _selectedItem.Value))
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
            using (var f = new SkillTypeUITypeEditorForm(null))
            {
                // If we require distinct, skip items we already have in the list
                if (RequireDistinct)
                {
                    var listItems = lstItems.Items.OfType<SkillType>().ToImmutable();
                    f.SkipItems = (x => listItems.Any(y => y == x));
                }

                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                var item = f.SelectedItem;
                _selectedItem = item;

                txtItem.Text = item.ToString();
            }
        }
    }
}