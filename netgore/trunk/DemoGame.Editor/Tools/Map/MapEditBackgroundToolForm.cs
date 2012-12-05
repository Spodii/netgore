using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor;
using WeifenLuo.WinFormsUI.Docking;
using NetGore.Graphics;

namespace DemoGame.Editor.Tools
{
    public partial class MapEditBackgroundToolForm : DockContent
    {
        EditorMap _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapEditBackgroundToolForm"/> class.
        /// </summary>
        public MapEditBackgroundToolForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the <see cref="EditorMap"/> being edited.
        /// </summary>
        public EditorMap Map
        {
            get { return _map; }
            set
            {
                if (Map == value)
                    return;

                var oldValue = Map;
                _map = value;

                OnMapChanged(oldValue, value);
            }
        }

        protected virtual void OnMapChanged(EditorMap oldValue, EditorMap newValue)
        {
            Text = string.Format("BG Images (Map: {0})", newValue != null ? newValue.ToString() : "None");
            RebuildList();
        }

        /// <summary>
        /// Rebuilds the list of <see cref="BackgroundImage"/>s.
        /// </summary>
        public void RebuildList()
        {
            var selected = lstBGs.SelectedItem;

            try
            {
                lstBGs.BeginUpdate();

                // Clear the list
                lstBGs.Items.Clear();

                if (Map != null)
                {
                    // Re-add the items
                    var items = Map.BackgroundImages.Cast<object>().ToArray();
                    if (items.Length > 0)
                        lstBGs.Items.AddRange(items);

                    // Re-set the selected item
                    if (selected != null && lstBGs.Items.Contains(selected))
                        lstBGs.SelectedItem = selected;
                }
            }
            finally
            {
                lstBGs.EndUpdate();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDelete_Click(object sender, EventArgs e)
        {
            var map = Map;
            var selected = lstBGs.SelectedItem as BackgroundImage;
            if (selected == null || map == null)
                return;

            // Confirm deletion
            const string confirmMsg = "Are you sure you wish to delete the BackgroundImage `{0}`?";
            if (MessageBox.Show(string.Format(confirmMsg, selected.Name), "Delete BackgroundImage?", MessageBoxButtons.YesNo) ==
                DialogResult.No)
                return;

            // Delete
            var wasDeleted = map.RemoveBackgroundImage(selected);
            Debug.Assert(wasDeleted);

            // Update list
            lstBGs.RemoveItemAndReselect(selected);
            RebuildList();
        }

        /// <summary>
        /// Handles the Click event of the btnNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnNew_Click(object sender, EventArgs e)
        {
            var map = Map;
            if (map == null)
                return;

            // Create new BackgroundImage
            var bg = new BackgroundLayer(map, map);
            map.AddBackgroundImage(bg);

            // Update list
            RebuildList();
        }

        /// <summary>
        /// Handles the SelectedValueChanged event of the lstBGs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstBGs_SelectedValueChanged(object sender, EventArgs e)
        {
            var selected = lstBGs.SelectedItem;
            if (selected == null)
                return;

            pg.SelectedObject = selected;
        }

        /// <summary>
        /// Handles the PropertyValueChanged event of the pg control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PropertyValueChangedEventArgs"/> instance containing the event data.</param>
        void pg_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var selected = lstBGs.SelectedItem;
            if (selected != pg.SelectedObject)
                return;

            lstBGs.RefreshItemAt(lstBGs.SelectedIndex);
        }
    }
}