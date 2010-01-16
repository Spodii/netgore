using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;
using NetGore.EditorTools;

namespace DemoGame.MapEditor
{
    /// <summary>
    /// ListBox specifically for Persistent NPCs.
    /// </summary>
    public class PersistentNPCListBox : ListBox, IMapBoundControl
    {
        Map _map;

        /// <summary>
        /// Gets or sets the current map.
        /// </summary>
        public Map Map
        {
            get { return _map; }
            set
            {
                if (Map == value)
                    return;

                _map = value;

                PopulateItems();
            }
        }

        /// <summary>
        /// Populates the list with the persistent NPCs.
        /// </summary>
        void PopulateItems()
        {
            Items.Clear();
            if (Map == null)
                return;

            // Get the indicies of the characters set to spawn on this map from the database
            var dbController = DbControllerBase.GetInstance();
            var persistentNPCIDs = dbController.GetQuery<SelectPersistentMapNPCsByRespawnMapQuery>().Execute(Map.Index);
            var addedChars = new List<MapEditorCharacter>();
            foreach (var characterID in persistentNPCIDs)
            {
                // Create the character
                var c = new MapEditorCharacter(characterID, Map);
                addedChars.Add(c);
            }

            Items.AddRange(addedChars.OrderBy(x => x.CharacterID).ToArray());
        }

        #region IMapBoundControl Members

        /// <summary>
        /// Gets or sets the current <see cref="IMapBoundControl.IMap"/>.
        /// </summary>
        IMap IMapBoundControl.IMap
        {
            get { return Map; }
            set { Map = (Map)value; }
        }

        #endregion
    }
}