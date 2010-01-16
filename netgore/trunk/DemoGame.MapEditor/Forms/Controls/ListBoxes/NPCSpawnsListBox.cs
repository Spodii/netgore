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
    public class NPCSpawnsListBox : ListBox, IMapBoundControl
    {
        MapBase _map;

        public NPCSpawnsListBox()
        {
            SelectedIndexChanged += HandleSelectedIndexChanged;
        }

        public MapBase Map
        {
            get { return _map; }
            set
            {
                if (Map == value)
                    return;

                _map = value;

                ReloadSpawns();
            }
        }

        /// <summary>
        /// Gets or sets the PropertyGrid to display the property values for the selected NPC in this NPCSpawnsListBox.
        /// </summary>
        [Description("The PropertyGrid to display the property values for the selected NPC in this NPCSpawnsListBox.")]
        public PropertyGrid PropertyGrid { get; set; }

        public void AddNewItem()
        {
            if (Map == null)
            {
                MessageBox.Show("The map must be set before a new spawn can be created!");
                return;
            }

            MapSpawnValues newSpawn = new MapSpawnValues(DbControllerBase.GetInstance(), Map.Index, new CharacterTemplateID(1));
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

            if (Map == null)
                return;

            var spawnInfo = MapSpawnValues.Load(DbControllerBase.GetInstance(), Map.Index);
            var asArray = spawnInfo.Select(x => new NPCSpawnsListBoxItem(x)).ToArray();
            Items.AddRange(asArray);
        }

        #region IMapBoundControl Members

        /// <summary>
        /// Gets or sets the current <see cref="IMapBoundControl.IMap"/>.
        /// </summary>
        IMap IMapBoundControl.IMap
        {
            get { return Map; }
            set { Map = (MapBase)value; }
        }

        #endregion

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