using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore.Db;

// NOTE: Don't worry, I'll get around to this at some point. -Spodi

namespace DemoGame.MapEditor
{
    /// <summary>
    /// ListBox specifically for Persistent NPCs.
    /// </summary>
    public class PersistentNPCListBox : ListBox
    {
        IDbController _dbController;
        MapBase _map;

        /// <summary>
        /// Gets or sets the PropertyGrid to display the property values for the selected characterID in this PersistentNPCListBox.
        /// </summary>
        [Description("The PropertyGrid to display the property values for the selected characterID in this PersistentNPCListBox.")
        ]
        public PropertyGrid PropertyGrid { get; set; }

        public PersistentNPCListBox()
        {
            SelectedIndexChanged += HandleSelectedIndexChanged;
        }

        void HandleSelectedIndexChanged(object sender, EventArgs e)
        {
            if (PropertyGrid == null)
                return;

            PersistentNPCListBoxItem selected = SelectedItem as PersistentNPCListBoxItem;
            if (selected == null)
                return;

            //if (PropertyGrid.SelectedObject != selected.Value)
            //    PropertyGrid.SelectedObject = selected.Value;
        }

        public void SetMap(IDbController dbController, MapBase map)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _map = map;
            _dbController = dbController;
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