using System;
using System.Linq;
using DemoGame.Client;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Graphics;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// Implementation of <see cref="Character"/> specifically for the editor.
    /// </summary>
    public class EditorCharacter : Character
    {
        readonly CharacterID _characterID;
        readonly ICharacterTable _table;

        // TODO: Class is currently unused because displaying persistent Characters in the editor is not yet supported

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorCharacter"/> class.
        /// </summary>
        /// <param name="table">The <see cref="ICharacterTable"/> describing the character.</param>
        /// <param name="map">The <see cref="Map"/> to place the character on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="table" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="map" /> is <c>null</c>.</exception>
        public EditorCharacter(ICharacterTable table, EditorMap map)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (map == null)
                throw new ArgumentNullException("map");

            _table = table;
            _characterID = table.ID;

            var dbController = DbControllerBase.GetInstance();

            var charInfo = dbController.GetQuery<SelectCharacterByIDQuery>().Execute(_characterID);
            BodyID = charInfo.BodyID;
            Teleport(new Vector2(charInfo.RespawnX, charInfo.RespawnY));
            Resize(BodyInfo.Size);
            Name = charInfo.Name;

            Initialize(map, SkeletonManager.Create(ContentPaths.Build));
        }

        public CharacterID CharacterID
        {
            get { return _characterID; }
        }

        public ICharacterTable CharacterTable
        {
            get { return _table; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("[{0}] {1}", CharacterID, Name);
        }

        /// <summary>
        /// Perform pre-collision velocity and position updating.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="deltaTime">The amount of that that has elapsed time since last update.</param>
        public override void UpdateVelocity(IMap map, int deltaTime)
        {
            SetVelocityRaw(Vector2.Zero);
        }
    }
}