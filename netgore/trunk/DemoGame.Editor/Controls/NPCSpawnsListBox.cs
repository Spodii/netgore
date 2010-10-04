using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Server;
using NetGore.Db;
using NetGore.Editor;
using NetGore.Editor.WinForms;
using NetGore.World;

namespace DemoGame.Editor
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
        [Browsable(false)]
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

        public MapSpawnValues SelectedItemReal
        {
            get
            {
                var item = SelectedItem as NPCSpawnsListBoxItem;
                if (item == null)
                    return null;

                return item.Value;
            }
        }

        public void AddNewItem()
        {
            if (Map == null)
            {
                MessageBox.Show("The map must be set before a new spawn can be created!");
                return;
            }

            var newSpawn = new MapSpawnValues(DbControllerBase.GetInstance(), Map.ID, new CharacterTemplateID(1));
            var newItem = new NPCSpawnsListBoxItem(newSpawn);

            this.AddItemAndReselect(newItem);

            SelectedItem = newItem;
        }

        public void DeleteItem(object item)
        {
            if (item == null)
                return;

            var listBoxItem = SelectedItem as NPCSpawnsListBoxItem;
            if (listBoxItem == null)
                return;

            var spawnItem = listBoxItem.Value;
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
            return Items.OfType<NPCSpawnsListBoxItem>().Select(item => item.Value);
        }

        void ReloadSpawns()
        {
            Items.Clear();

            if (Map == null)
                return;

            var spawnInfo = MapSpawnValues.Load(DbControllerBase.GetInstance(), Map.ID);
            var asArray = spawnInfo.Select(x => new NPCSpawnsListBoxItem(x)).ToArray();
            Items.AddRange(asArray);
        }

        #region IMapBoundControl Members

        /// <summary>
        /// Gets or sets the current <see cref="IMapBoundControl.IMap"/>.
        /// </summary>
        [Browsable(false)]
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