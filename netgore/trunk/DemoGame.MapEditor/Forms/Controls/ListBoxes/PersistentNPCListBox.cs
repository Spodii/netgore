using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;
using NetGore.Graphics;

// NOTE: Don't worry, I'll get around to this at some point. -Spodi

namespace DemoGame.MapEditor
{
    /// <summary>
    /// ListBox specifically for Persistent NPCs.
    /// </summary>
    public class PersistentNPCListBox : ListBox
    {
        IDbController _dbController;
        Map _map;

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            if (PropertyGrid == null)
                return;

            PersistentNPCListBoxItem selected = SelectedItem as PersistentNPCListBoxItem;
            if (selected == null)
                return;

            //if (PropertyGrid.SelectedObject != selected.Value)
            //    PropertyGrid.SelectedObject = selected.Value;
        }

        /// <summary>
        /// Gets or sets the PropertyGrid to display the property values for the selected NPC in this PersistentNPCListBox.
        /// </summary>
        [Description("The PropertyGrid to display the property values for the selected NPC in this PersistentNPCListBox.")]
        public PropertyGrid PropertyGrid { get; set; }

        /// <summary>
        /// Loads and populates the collection for the given map.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> to use to communicate with the database.</param>
        /// <param name="map">The map.</param>
        public void Load(IDbController dbController, Map map)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _map = map;
            _dbController = dbController;
        }

        /// <summary>
        /// Gets the <see cref="IDbController"/> used to communicate with the database.
        /// </summary>
        public IDbController DbController { get { return _dbController; } }

        /// <summary>
        /// Gets the current map.
        /// </summary>
        public Map Map { get { return _map; } }

        /// <summary>
        /// Populates the list with the persistent NPCs.
        /// </summary>
        void PopulateItems()
        {
            // Get the values from the database
            var persistentNPCIDs = DbController.GetQuery<SelectPersistentMapNPCsQuery>().Execute(Map.Index);
            foreach (var characterID in persistentNPCIDs)
            {
                var c = new Character();
                // TODO: $$$$$ c.Initialize(Map, SkeletonManager
            }

            Items.Clear();
        }

        class PersistentNPCListBoxItem
        {
            /*
            public readonly NPC Value;

            public PersistentNPCListBoxItem(MapSpawnValues v)
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
            */
        }
    }
}