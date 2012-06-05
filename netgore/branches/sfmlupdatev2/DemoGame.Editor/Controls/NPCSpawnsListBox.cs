using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Server;
using DemoGame.Server.Queries;
using NetGore;
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
        /// Initializes a new instance of the <see cref="NPCSpawnsListBox"/> class.
        /// </summary>
        public NPCSpawnsListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

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

        public void AddNewItem()
        {
            if (Map == null)
            {
                MessageBox.Show("The map must be set before a new spawn can be created!");
                return;
            }

            var charID = CharacterTemplateManager.Instance.First().TemplateTable.ID;
            var newSpawn = new MapSpawnValues(DbControllerBase.GetInstance(), Map.ID, charID);

            this.AddItemAndReselect(newSpawn);
        }

        public bool DeleteItem(object item)
        {
            return DeleteItem(item as MapSpawnValues);
        }

        public bool DeleteItem(MapSpawnValues item)
        {
            if (item == null)
                return false;

            if (!Items.Contains(item))
                return false;

            var q = DbControllerBase.GetInstance().GetQuery<DeleteMapSpawnQuery>();
            q.Execute(item.ID);

            ReloadSpawns();

            return true;
        }

        public void DeleteSelectedItem()
        {
            if (SelectedItem == null)
                return;

            DeleteItem(SelectedItem);
        }

        static string GetDrawString(MapSpawnValues x)
        {
            return string.Format("Char ID: {0}  Count: {1}  Region: {2}", x.CharacterTemplateID, x.SpawnAmount, x.SpawnArea);
        }

        public IEnumerable<MapSpawnValues> GetMapSpawnValues()
        {
            return Items.OfType<MapSpawnValues>().ToImmutable();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data. </param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DesignMode || !ControlHelper.DrawListItem<MapSpawnValues>(Items, e, x => GetDrawString(x)))
                base.OnDrawItem(e);
        }

        /// <summary>
        /// Reloads the spawns in this list. This shouldn't need to be called manually unless the list is updated externally.
        /// However, calling this when the list hasn't changed will do no harm.
        /// </summary>
        public void ReloadSpawns()
        {
            var selected = SelectedItem;

            if (Map == null)
            {
                Items.Clear();
                return;
            }

            // Grab the latest values
            var spawnInfo = MapSpawnValues.Load(DbControllerBase.GetInstance(), Map.ID);
            var asArray = spawnInfo.OrderBy(x => x.ID).Cast<object>().ToArray();

            // Check if we are already up-to-date
            if (asArray.ContainSameElements(Items.Cast<object>().ToImmutable()))
                return;

            // Update the list
            try
            {
                BeginUpdate();

                Items.Clear();

                if (asArray.Length > 0)
                {
                    Items.AddRange(asArray);

                    if (selected != null && Items.Contains(selected))
                        SelectedItem = selected;
                }
            }
            finally
            {
                EndUpdate();
            }
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
    }
}