using System;
using System.Linq;
using DemoGame.Client;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Graphics;
using NetGore.IO;
using SFML.Graphics;

namespace DemoGame.Editor
{
    public class MapEditorCharacter : Character
    {
        readonly CharacterID _characterID;
        readonly ICharacterTable _table;

        public MapEditorCharacter(ICharacterTable table, Map map)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (map == null)
                throw new ArgumentNullException("map");

            // ReSharper disable DoNotCallOverridableMethodsInConstructor

            _table = table;
            _characterID = table.ID;

            var dbController = DbControllerBase.GetInstance();

            var charInfo = dbController.GetQuery<SelectCharacterByIDQuery>().Execute(_characterID);
            BodyID = charInfo.BodyID;
            Teleport(new Vector2(charInfo.RespawnX, charInfo.RespawnY));
            Resize(BodyInfo.Size);
            Name = charInfo.Name;

            Initialize(map, SkeletonManager.Create(ContentPaths.Build));

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
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
        /// <param name="deltaTime">The amount of that that has elapsed time since last update.</param>
        public override void UpdateVelocity(int deltaTime)
        {
            SetVelocityRaw(Vector2.Zero);
        }
    }
}