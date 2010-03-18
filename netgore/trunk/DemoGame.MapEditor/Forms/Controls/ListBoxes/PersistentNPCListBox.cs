using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using Microsoft.Xna.Framework;
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
        [Browsable(false)]
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
        /// Gets or sets the <see cref="PropertyGrid"/> to display the selected item on.
        /// </summary>
        public PropertyGrid PropertyGrid { get; set; }

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
            var getCharacterQuery = dbController.GetQuery<SelectCharacterByIDQuery>();
            var persistentNPCIDs = dbController.GetQuery<SelectPersistentMapNPCsByRespawnMapQuery>().Execute(Map.ID);
            var addedChars = new List<MapEditorCharacter>();

            foreach (var characterID in persistentNPCIDs)
            {
                // Get the actual character
                var table = getCharacterQuery.Execute(characterID);

                // Create the character
                var c = new MapEditorCharacter(table, Map);
                addedChars.Add(c);
            }

            var listItems = addedChars.Select(x => new ListItem(this, x));
            Items.AddRange(listItems.OrderBy(x => x.CharacterID).ToArray());
        }

        protected override void OnSelectedIndexChanged(System.EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            var pg = PropertyGrid;
            if (pg == null)
                return;

            var item = SelectedItem as ListItem;
            if (item == null)
                return;

            if (pg.SelectedObject != item)
                pg.SelectedObject = item;
        }

        /// <summary>
        /// Gets an item in this <see cref="ListBox"/> as a <see cref="MapEditorCharacter"/>.
        /// </summary>
        /// <param name="listItem">The item from this <see cref="ListBox"/>.</param>
        /// <returns>The <paramref name="listItem"/> as a <see cref="MapEditorCharacter"/>, or null if of
        /// an invalid type.</returns>
        public static MapEditorCharacter ItemAsMapEditorCharacter(object listItem)
        {
            var asListItem = listItem as ListItem;
            if (asListItem == null)
                return null;

            return asListItem.Character;
        }

        /// <summary>
        /// Gets an item in this <see cref="ListBox"/> as an <see cref="ICharacterTable"/>.
        /// </summary>
        /// <param name="listItem">The item from this <see cref="ListBox"/>.</param>
        /// <returns>The <paramref name="listItem"/> as an <see cref="ICharacterTable"/>, or null if of
        /// an invalid type.</returns>
        public static ICharacterTable ItemAsICharacterTable(object listItem)
        {
            var asMapEditorCharacter = ItemAsMapEditorCharacter(listItem);
            if (asMapEditorCharacter == null)
                return null;

            return asMapEditorCharacter.CharacterTable;
        }

        class ListItem
        {
            readonly PersistentNPCListBox _parent;

            MapEditorCharacter _character;

            /// <summary>
            /// Initializes a new instance of the <see cref="ListItem"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="character">The character.</param>
            public ListItem(PersistentNPCListBox parent, MapEditorCharacter character)
            {
                _parent = parent;
                _character = character;
            }

            /// <summary>
            /// Gets the parent <see cref="PersistentNPCListBox"/> control.
            /// </summary>
            [Browsable(false)]
            public PersistentNPCListBox Parent { get { return _parent; } }

            /// <summary>
            /// Gets the <see cref="MapEditorCharacter"/>.
            /// </summary>
            [Browsable(false)]
            public MapEditorCharacter Character
            {
                get { return _character; }
            }

            /// <summary>
            /// Gets or sets the character's position.
            /// </summary>
            [Browsable(true)]
            [Description("The world position of the character.")]
            public Vector2 Position
            {
                get { return _character.Position; }
                set
                {
                    if (_character.Position == value)
                        return;

                    _character.Teleport(value);
                }
            }

            /// <summary>
            /// Gets or sets the <see cref="CharacterID"/> for the character.
            /// </summary>
            [Browsable(true)]
            [Description("The ID of the character.")]
            public CharacterID CharacterID
            {
                get
                {
                    return _character.CharacterID;
                }
                set
                {
                    // Ensure there is a new value before going through all this work
                    if (_character.CharacterID == value)
                        return;

                    // Get the character's data
                    var dbController = DbControllerBase.GetInstance();
                    var selectCharQuery = dbController.GetQuery<SelectCharacterByIDQuery>();
                    var charTable = selectCharQuery.Execute(value);

                    if (charTable == null)
                        return;

                    // Create the new character
                    MapEditorCharacter newChar = new MapEditorCharacter(charTable, Parent.Map);
                    _character = newChar;
                }
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
            set { Map = (Map)value; }
        }

        #endregion
    }
}