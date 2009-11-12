using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Server;
using NetGore;
using NetGore.Db;
using NetGore.EditorTools;

namespace DemoGame.MapEditor
{
    /// <summary>
    /// A ListBox specifically for the MapSpawnValues on a Map.
    /// </summary>
    public class NPCSpawnsListBox : ListBox
    {
        IDbController _dbController;
        MapBase _map;

        /// <summary>
        /// Gets or sets the PropertyGrid to display the property values for the selected NPC in this NPCSpawnsListBox.
        /// </summary>
        [Description("The PropertyGrid to display the property values for the selected NPC in this NPCSpawnsListBox.")]
        public PropertyGrid PropertyGrid { get; set; }

        public NPCSpawnsListBox()
        {
            SelectedIndexChanged += HandleSelectedIndexChanged;
        }

        public void AddNewItem()
        {
            if (_map == null)
            {
                MessageBox.Show("The map must be set before a new spawn can be created!");
                return;
            }

            MapSpawnValues newSpawn = new MapSpawnValues(_dbController, _map.Index, new CharacterTemplateID(1));
            NPCSpawnsListBoxItem newItem = new NPCSpawnsListBoxItem(newSpawn);

            this.AddItemAndReselect(newItem);

            SelectedItem = newItem;
        }

        public void DeleteItem(object item)
        {
            if (item == null)
                return;

            NPCSpawnsListBoxItem listBoxItem = SelectedItem as NPCSpawnsListBoxItem;
            if (listBoxItem == null)
                return;

            MapSpawnValues spawnItem = listBoxItem.Value;
            if (spawnItem == null)
                return;

            if (!Items.Contains(listBoxItem))
            {
                MessageBox.Show("Failed to find NPC `{0}` in the ListBox.");
                return;
            }

            this.RemoveItemAndReselect(listBoxItem);

            spawnItem.Delete();
        }

        public void DeleteSelectedItem()
        {
            if (SelectedItem == null)
                return;

            DeleteItem(SelectedItem);
        }

        public IEnumerable<MapSpawnValues> GetMapSpawnValues()
        {
            foreach (NPCSpawnsListBoxItem item in Items.OfType<NPCSpawnsListBoxItem>())
            {
                yield return item.Value;
            }
        }

        void HandleSelectedIndexChanged(object sender, EventArgs e)
        {
            if (PropertyGrid == null)
                return;

            NPCSpawnsListBoxItem selected = SelectedItem as NPCSpawnsListBoxItem;
            if (selected == null)
                return;

            if (PropertyGrid.SelectedObject != selected.Value)
                PropertyGrid.SelectedObject = selected.Value;
        }

        void ReloadSpawns()
        {
            Items.Clear();

            var spawnInfo = MapSpawnValues.Load(_dbController, _map.Index);
            var asArray = spawnInfo.Select(x => new NPCSpawnsListBoxItem(x)).ToArray();
            Items.AddRange(asArray);
        }

        public void SetMap(IDbController dbController, MapBase map)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _map = map;
            _dbController = dbController;

            ReloadSpawns();
        }

        class NPCSpawnsListBoxItem
        {
            public readonly MapSpawnValues Value;

            public NPCSpawnsListBoxItem(MapSpawnValues v)
            {
                Value = v;
            }

            public override string ToString()
            {
                return string.Format("Char ID: {0}  Count: {1}  Region: {2}", Value.CharacterTemplateID, Value.SpawnAmount,
                                     Value.SpawnArea);
            }

            public static implicit operator MapSpawnValues(NPCSpawnsListBoxItem v)
            {
                return v.Value;
            }
        }
    }
}