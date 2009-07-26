using System;
using System.ComponentModel;
using System.Windows.Forms;
using DemoGame.Server;

namespace DemoGame.MapEditor
{
    /// <summary>
    /// ListBox specifically for Persistent NPCs.
    /// </summary>
    public class PersistentNPCListBox : ListBox
    {
        DBController _dbController;
        MapBase _map;

        /// <summary>
        /// Gets or sets the PropertyGrid to display the property values for the selected item in this PersistentNPCListBox.
        /// </summary>
        [Description("The PropertyGrid to display the property values for the selected item in this PersistentNPCListBox.")]
        public PropertyGrid PropertyGrid { get; set; }

        public void SetMap(DBController dbController, MapBase map)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _map = map;
            _dbController = dbController;
        }
    }
}