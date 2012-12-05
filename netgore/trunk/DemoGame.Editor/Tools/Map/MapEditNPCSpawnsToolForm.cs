using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Server;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Editor;
using WeifenLuo.WinFormsUI.Docking;
using NetGore;

namespace DemoGame.Editor.Tools
{
    public partial class MapEditNPCSpawnsToolForm : DockContent
    {
        EditorMap _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapEditNPCSpawnsToolForm"/> class.
        /// </summary>
        public MapEditNPCSpawnsToolForm()
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

        /// <summary>
        /// Gets the MapSpawnValues shown in the form.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MapSpawnValues> GetMapSpawns()
        {
            return lstSpawns.Items.OfType<MapSpawnValues>();
        }

        protected virtual void OnMapChanged(EditorMap oldValue, EditorMap newValue)
        {
            Text = string.Format("NPC Spawns (Map: {0})", newValue != null ? newValue.ToString() : "None");
            lstSpawns.Map = newValue;
        }

        /// <summary>
        /// Handles the Click event of the <see cref="btnDelete"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDelete_Click(object sender, EventArgs e)
        {
            var map = Map;
            var selected = lstSpawns.SelectedItem as IMapSpawnTable;
            if (selected == null || map == null)
                return;

            // Confirm deletion
            const string confirmMsg = "Are you sure you wish to delete the selected NPC spawn?";
            if (MessageBox.Show(confirmMsg, "Delete NPC spawn?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Delete
            var q = DbControllerBase.GetInstance().GetQuery<DeleteMapSpawnQuery>();
            q.Execute(selected.ID);

            // Update list
            lstSpawns.RemoveItemAndReselect(selected);
            lstSpawns.ReloadSpawns();
        }

        /// <summary>
        /// Handles the Click event of the <see cref="btnNew"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnNew_Click(object sender, EventArgs e)
        {
            var map = Map;
            if (map == null)
                return;

            // Create new spawn
            var charID = CharacterTemplateManager.Instance.First().TemplateTable.ID;
            var value = new MapSpawnTable { MapID = map.ID, Amount = 1, CharacterTemplateID = charID };

            var q = DbControllerBase.GetInstance().GetQuery<InsertMapSpawnQuery>();
            q.Execute(value);

            // Update list
            lstSpawns.ReloadSpawns();
        }

        /// <summary>
        /// Handles the SelectedValueChanged event of the <see cref="lstSpawns"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstSpawns_SelectedValueChanged(object sender, EventArgs e)
        {
            var selected = lstSpawns.SelectedItem;
            if (selected == null)
                return;

            pg.SelectedObject = selected;
        }

        /// <summary>
        /// Handles the PropertyValueChanged event of the <see cref="pg"/> control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PropertyValueChangedEventArgs"/> instance containing the event data.</param>
        void pg_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var selected = lstSpawns.SelectedItem;
            if (selected != pg.SelectedObject)
                return;

            lstSpawns.RefreshItemAt(lstSpawns.SelectedIndex);
        }

        private void lstSpawns_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Focus on the center of the selected item
            var selected = lstSpawns.SelectedItem as MapSpawnValues;
            if (selected == null)
                return;

            var spawnArea = selected.SpawnArea;

            if (!spawnArea.X.HasValue && !spawnArea.Y.HasValue && 
                !spawnArea.Width.HasValue && !spawnArea.Height.HasValue)
                return;

            var map = Map;
            if (map == null)
                return;

            var rect = spawnArea.ToRectangle(map);
            map.Camera.CenterOn(rect.Center.ToVector2());
        }
    }
}