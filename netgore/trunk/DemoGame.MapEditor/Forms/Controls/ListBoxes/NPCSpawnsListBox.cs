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

        /// <summary>
        /// Gets or sets the map used to populate this list box.
        /// </summary>
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

        public MapSpawnValues SelectedItemReal
        {
            get { return ((NPCSpawnsListBoxItem)SelectedItem).Value; }
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