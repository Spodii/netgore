using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using log4net;
using NetGore.Features.NPCChat;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// Represents an <see cref="NPC"/> which was summoned. Thralled NPCs are not intended to respawn and they should not
    /// provide interaction and services like <see cref="NPC"/>s do. This means no quests, interactive chat, shopping, etc.
    /// </summary>
    public class ThralledNPC : NPC
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ThralledNPC(World parent, CharacterTemplate template, Map map, Vector2 position)
            : base(parent, template, map, position)
        {
            // This NPC should never respawn. Once it's dead, that should be it!
            RespawnMapID = null;
            RespawnPosition = Vector2.Zero;

            if (log.IsDebugEnabled)
                log.DebugFormat("Created ThralledNPC `{0}` on map `{1}` at `{2}` with template `{3}`.", this, Map, Position,
                    template);
        }

        /// <summary>
        /// Gets the NPC's chat dialog if they have one, or null if they don't.
        /// Always returns null for a <see cref="ThralledNPC"/>.
        /// </summary>
        public override NPCChatDialogBase ChatDialog
        {
            get { return null; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if the CharacterEntity has a chat dialog. Do not use the setter
        /// on this property.
        /// Always returns false for a <see cref="ThralledNPC"/>.
        /// </summary>
        public override bool HasChatDialog
        {
            get { return false; }
            protected set { }
        }

        /// <summary>
        /// When overridden in the derived class, gets if the CharacterEntity has a shop. Do not use the setter
        /// on this property.
        /// Always returns false for a <see cref="ThralledNPC"/>.
        /// </summary>
        public override bool HasShop
        {
            get { return false; }
            protected set { }
        }

        /// <summary>
        /// When overridden in the derived class, gets this <see cref="Character"/>'s shop.
        /// Always returns null for a <see cref="ThralledNPC"/>.
        /// </summary>
        public override IShop<ShopItem> Shop
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the quests that this <see cref="NPC"/> should provide.
        /// </summary>
        /// <param name="charTemplate">The <see cref="CharacterTemplate"/> that this <see cref="NPC"/> was loaded from.</param>
        /// <returns>
        /// The quests that this <see cref="NPC"/> should provide. Return an empty or null collection to make
        /// this <see cref="NPC"/> not provide any quests.
        /// </returns>
        protected override IEnumerable<IQuest<User>> GetProvidedQuests(CharacterTemplate charTemplate)
        {
            // Never use quests for a thralled NPC
            return null;
        }

        /// <summary>
        /// When overridden in the derived class, handles additional loading stuff.
        /// </summary>
        /// <param name="v">The ICharacterTable containing the database values for this Character.</param>
        protected override void HandleAdditionalLoading(ICharacterTable v)
        {
            // Do not perform additional loading, since the additional loading is just for special NPC things like
            // shops and chat, which are things we do not want in a thralled NPC
        }
    }
}