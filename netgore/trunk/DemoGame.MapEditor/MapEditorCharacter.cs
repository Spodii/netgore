using System.Linq;
using DemoGame.Client;
using DemoGame.Server.Queries;
using Microsoft.Xna.Framework;
using NetGore.Db;
using NetGore.Graphics;
using NetGore.IO;

namespace DemoGame.MapEditor
{
    public class MapEditorCharacter : Character
    {
        readonly CharacterID _characterID;

        public MapEditorCharacter(CharacterID characterID, Map map)
        {
            _characterID = characterID;

            var dbController = DbControllerBase.GetInstance();

            var charInfo = dbController.GetQuery<SelectCharacterByIDQuery>().Execute(characterID);
            BodyInfoIndex = charInfo.BodyID;
            Teleport(new Vector2(charInfo.RespawnX, charInfo.RespawnY));
            Resize(BodyInfo.Size);
            Name = charInfo.Name;

            Initialize(map, SkeletonManager.Create(ContentPaths.Build));
        }

        public CharacterID CharacterID
        {
            get { return _characterID; }
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