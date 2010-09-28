using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// Represents an <see cref="NPC"/> which was summoned. Thralled NPCs are not intended to respawn and they should not
    /// provide interaction and services like <see cref="NPC"/>s do. This means no quests, interactive chat, shopping, etc.
    /// </summary>
    public class ThralledNPC : NPC
    {
        public ThralledNPC(World parent, CharacterTemplate template, Map map, Vector2 position)
            : base(parent, template, map, position)
        {
            // This NPC should never respawn. Once it's dead, that should be it!
            RespawnMapID = null;
            RespawnPosition = Vector2.Zero;
        }

        /// <summary>
        /// When overridden in the derived class, handles additional loading stuff.
        /// </summary>
        /// <param name="v">The ICharacterTable containing the database values for this Character.</param>
        protected override void HandleAdditionalLoading(DemoGame.DbObjs.ICharacterTable v)
        {
            // Do not perform additional loading, since the additional loading is just for special NPC things like
            // shops and chat, which are things we do not want in a thralled NPC
        }

        /// <summary>
        /// When overridden in the derived class, gets if the CharacterEntity has a chat dialog. Do not use the setter
        /// on this property.
        /// Always returns false for a <see cref="ThralledNPC"/>.
        /// </summary>
        public override bool HasChatDialog
        {
            get
            {
                return false;
            }
            protected set
            {
            }
        }

        /// <summary>
        /// Cache of an empty enumerable of quests.
        /// </summary>
        static readonly IEnumerable<NetGore.Features.Quests.IQuest<User>> _emptyQuests = Enumerable.Empty<NetGore.Features.Quests.IQuest<User>>();

        /// <summary>
        /// Gets the quests that this quest provider provides.
        /// Always returns an empty collection for a <see cref="ThralledNPC"/>.
        /// </summary>
        public override IEnumerable<NetGore.Features.Quests.IQuest<User>> Quests
        {
            get
            {
                return _emptyQuests;
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets if the CharacterEntity has a shop. Do not use the setter
        /// on this property.
        /// Always returns false for a <see cref="ThralledNPC"/>.
        /// </summary>
        public override bool HasShop
        {
            get
            {
                return false;
            }
            protected set
            {
            }
        }

        /// <summary>
        /// Gets the NPC's chat dialog if they have one, or null if they don't.
        /// Always returns null for a <see cref="ThralledNPC"/>.
        /// </summary>
        public override NetGore.NPCChat.NPCChatDialogBase ChatDialog
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets this <see cref="Character"/>'s shop.
        /// Always returns null for a <see cref="ThralledNPC"/>.
        /// </summary>
        public override NetGore.Features.Shops.IShop<ShopItem> Shop
        {
            get
            {
                return null;
            }
        }
    }
}