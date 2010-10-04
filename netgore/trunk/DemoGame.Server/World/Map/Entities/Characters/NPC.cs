using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.NPCChat;
using log4net;
using NetGore;
using NetGore.AI;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.IO;
using NetGore.Network;
using NetGore.NPCChat;
using NetGore.Stats;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// A non-player character
    /// </summary>
    public class NPC : Character, IQuestProvider<User>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly IAIFactory<Character> _aiFactory = AIFactory.Instance;

        /// <summary>
        /// Cache of an empty enumerable of quests.
        /// </summary>
        static readonly IEnumerable<IQuest<User>> _emptyQuests = Enumerable.Empty<IQuest<User>>();

        static readonly NPCChatManagerBase _npcChatManager = NPCChatManager.Instance;

        IAI _ai;
        NPCChatDialogBase _chatDialog;
        ushort _giveCash;
        ushort _giveExp;
        IEnumerable<IQuest<User>> _quests;
        ushort _respawnSecs;

        /// <summary>
        /// The game time at which the NPC will respawn.
        /// </summary>
        TickCount _respawnTime = 0;

        IShop<ShopItem> _shop;

        /// <summary>
        /// Initializes a new instance of the <see cref="NPC"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="characterID">The character ID.</param>
        public NPC(World parent, CharacterID characterID) : base(parent, true)
        {
            // HACK: This whole constructor is uber hax
            if (parent == null)
                throw new ArgumentNullException("parent");

            Alliance = AllianceManager[new AllianceID(1)];

            IsAlive = true;

            Load(characterID);

            if (log.IsInfoEnabled)
                log.InfoFormat("Created persistent NPC `{0}` from CharacterID `{1}`.", this, characterID);

            LoadPersistentNPCTemplateInfo();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NPC"/> class.
        /// </summary>
        /// <param name="parent">World that the NPC belongs to.</param>
        /// <param name="template">NPCTemplate used to create the NPC.</param>
        /// <param name="map">The map.</param>
        /// <param name="position">The position.</param>
        public NPC(World parent, CharacterTemplate template, Map map, Vector2 position) : base(parent, false)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (template == null)
                throw new ArgumentNullException("template");

            var v = template.TemplateTable;

            // Set the rest of the template stuff
            Load(template);
            _respawnSecs = v.Respawn;
            _giveExp = v.GiveExp;
            _giveCash = v.GiveCash;
            _quests = GetProvidedQuests(template) ?? _emptyQuests;
            _chatDialog = v.ChatDialog.HasValue ? _npcChatManager[v.ChatDialog.Value] : null;
            SetShopFromID(v.ShopID);

            // Set the respawn positions only if the map we set them on is not instanced
            if (!map.IsInstanced)
            {
                RespawnMapID = map.ID;
                RespawnPosition = position;
            }
            else
            {
                RespawnMapID = null;
                RespawnPosition = Vector2.Zero;
            }

            LoadSpawnItems();

            // Done loading
            SetAsLoaded();

            if (log.IsDebugEnabled)
                log.DebugFormat("Created NPC instance from template `{0}`.", template);

            // Spawn
            ChangeMap(map, position);

            ((IRespawnable)this).Respawn();
        }

        /// <summary>
        /// When overridden in the derived class, gets the Character's AI. Can be null if they have no AI.
        /// </summary>
        public override IAI AI
        {
            get { return _ai; }
        }

        /// <summary>
        /// Gets the NPC's chat dialog if they have one, or null if they don't.
        /// </summary>
        public override NPCChatDialogBase ChatDialog
        {
            get { return _chatDialog; }
        }

        /// <summary>
        /// Gets the amount of cash that the NPC gives upon being killed.
        /// </summary>
        public ushort GiveCash
        {
            get { return _giveCash; }
            protected set { _giveCash = value; }
        }

        /// <summary>
        /// Gets the amount of experience that the NPC gives upon being killed.
        /// </summary>
        public ushort GiveExp
        {
            get { return _giveExp; }
            protected set { _giveExp = value; }
        }

        /// <summary>
        /// Gets or sets (protected) the amount of time it takes (in milliseconds) for the NPC to respawn.
        /// </summary>
        public ushort RespawnSecs
        {
            get { return _respawnSecs; }
            protected set { _respawnSecs = value; }
        }

        /// <summary>
        /// When overridden in the derived class, gets this <see cref="Character"/>'s shop.
        /// </summary>
        public override IShop<ShopItem> Shop
        {
            get { return _shop; }
        }

        /// <summary>
        /// Gets if this NPC will respawn after dieing.
        /// </summary>
        public bool WillRespawn
        {
            get { return RespawnMapID.HasValue; }
        }

        /// <summary>
        /// When overridden in the derived class, handles post creation-serialization processing. This method is invoked
        /// immediately after the <see cref="DynamicEntity"/>'s creation values have been serialized.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> that was used to serialize the values.</param>
        protected override void AfterSendCreated(IValueWriter writer)
        {
            base.AfterSendCreated(writer);

            var pw = writer as PacketWriter;
            if (pw != null && !Quests.IsEmpty())
                ServerPacket.SetProvidedQuests(pw, MapEntityIndex, Quests.Select(x => x.QuestID).ToImmutable());
        }

        /// <summary>
        /// When overridden in the derived class, checks if enough time has elapesd since the Character died
        /// for them to be able to respawn.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        /// <returns>True if enough time has elapsed; otherwise false.</returns>
        protected override bool CheckRespawnElapsedTime(TickCount currentTime)
        {
            return currentTime > _respawnTime;
        }

        /// <summary>
        /// When overridden in the derived class, creates the CharacterEquipped for this Character.
        /// </summary>
        /// <returns>
        /// The CharacterEquipped for this Character.
        /// </returns>
        protected override CharacterEquipped CreateEquipped()
        {
            return new NPCEquipped(this);
        }

        /// <summary>
        /// When overridden in the derived class, creates the CharacterInventory for this Character.
        /// </summary>
        /// <returns>
        /// The CharacterInventory for this Character.
        /// </returns>
        protected override CharacterInventory CreateInventory()
        {
            return new NPCInventory(this);
        }

        /// <summary>
        /// When overridden in the derived class, creates the CharacterStatsBase for this Character.
        /// </summary>
        /// <param name="statCollectionType">The type of <see cref="StatCollectionType"/> to create.</param>
        /// <returns>
        /// The CharacterStatsBase for this Character.
        /// </returns>
        protected override StatCollection<StatType> CreateStats(StatCollectionType statCollectionType)
        {
            return new StatCollection<StatType>(statCollectionType);
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="MapID"/> that this <see cref="Character"/>
        /// will use for when loading.
        /// </summary>
        /// <returns>The ID of the map to load this <see cref="Character"/> on.</returns>
        protected override MapID GetLoadMap()
        {
            // Use the current position if a non-instanced map to resume where right where the NPC left off, or if an
            // instanced or invalid map, use the respawn position
            if (Map == null || Map.IsInstanced)
            {
                if (RespawnMapID.HasValue)
                    return RespawnMapID.Value;

                const string errmsg =
                    "NPC `{0}` could not get a valid load position. Using ServerSettings.InvalidPersistentNPCLoad... instead.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));

                return ServerConfig.InvalidPersistentNPCLoadMap;
            }

            return Map.ID;
        }

        /// <summary>
        /// When overridden in the derived class, gets the position that this <see cref="Character"/>
        /// will use for when loading.
        /// </summary>
        /// <returns>The position to load this <see cref="Character"/> at.</returns>
        protected override Vector2 GetLoadPosition()
        {
            // Use the current position if a non-instanced map to resume where right where the NPC left off, or if an
            // instanced or invalid map, use the respawn position
            if (Map == null || Map.IsInstanced)
            {
                if (RespawnMapID.HasValue)
                    return RespawnPosition;

                const string errmsg =
                    "NPC `{0}` could not get a valid load position. Using ServerSettings.InvalidPersistentNPCLoad... instead.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));

                return ServerConfig.InvalidPersistentNPCLoadPosition;
            }

            return Position;
        }

        /// <summary>
        /// Gets the quests that this <see cref="NPC"/> should provide.
        /// </summary>
        /// <param name="charTemplate">The <see cref="CharacterTemplate"/> that this <see cref="NPC"/> was loaded from.</param>
        /// <returns>The quests that this <see cref="NPC"/> should provide. Return an empty or null collection to make
        /// this <see cref="NPC"/> not provide any quests.</returns>
        protected virtual IEnumerable<IQuest<User>> GetProvidedQuests(CharacterTemplate charTemplate)
        {
            if (charTemplate == null)
                return _emptyQuests;

            return charTemplate.Quests;
        }

        /// <summary>
        /// When overridden in the derived class, handles additional loading stuff.
        /// </summary>
        /// <param name="v">The ICharacterTable containing the database values for this Character.</param>
        protected override void HandleAdditionalLoading(ICharacterTable v)
        {
            base.HandleAdditionalLoading(v);

            _chatDialog = v.ChatDialog.HasValue ? _npcChatManager[v.ChatDialog.Value] : null;
            SetShopFromID(v.ShopID);
        }

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected override void HandleUpdate(IMap imap, int deltaTime)
        {
            // Check for spawning if dead
            if (!IsAlive)
                return;

            // Update the AI
            var ai = AI;
            if (ai != null)
                ai.Update();

            // Perform the base update of the character
            base.HandleUpdate(imap, deltaTime);
        }

        /// <summary>
        /// When overridden in the derived class, implements the Character being killed. This 
        /// doesn't actually care how the Character was killed, it just takes the appropriate
        /// actions to kill them.
        /// </summary>
        public override void Kill()
        {
            base.Kill();

            if (!IsAlive)
            {
                const string errmsg = "Attempted to kill dead NPC `{0}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            // Drop items
            if (Map == null)
            {
                const string errmsg = "Attempted to kill NPC `{0}` with a null map.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
            }
            else
            {
                foreach (var item in Inventory)
                {
                    DropItem(item.Value);
                }

                Inventory.RemoveAll(false);
            }

            // Remove equipment
            Equipped.RemoveAll(true);

            // Check to respawn
            if (WillRespawn)
            {
                // Start the respawn sequence
                IsAlive = false;
                _respawnTime = (TickCount)(GetTime() + (RespawnSecs * 1000));

                LoadSpawnItems();

                ChangeMap(null, Vector2.Zero);
                World.AddToRespawn(this);
            }
            else
            {
                // No respawning, so just dispose
                Debug.Assert(!IsDisposed);
                DelayedDispose();
            }
        }

        /// <summary>
        /// Loads additional information for a persistent NPC from their CharacterTemplate, if they have one.
        /// </summary>
        void LoadPersistentNPCTemplateInfo()
        {
            if (!CharacterTemplateID.HasValue)
                return;

            var template = CharacterTemplateManager[CharacterTemplateID.Value];
            if (template == null)
                return;

            var v = template.TemplateTable;

            _giveCash = v.GiveCash;
            _giveExp = v.GiveExp;
            _quests = GetProvidedQuests(template) ?? _emptyQuests;

            if (v.ChatDialog.HasValue)
                _chatDialog = _npcChatManager[v.ChatDialog.Value];
            else
                _chatDialog = null;
        }

        /// <summary>
        /// Reloads the Inventory and Equipment items the NPC spawns with.
        /// </summary>
        void LoadSpawnItems()
        {
            // All items remaining in the inventory or equipment should NOT be referenced!
            // Items that were dropped should have been removed when dropping
            Inventory.RemoveAll(true);
            Equipped.RemoveAll(true);

            // Grab the respawn items from the template
            var templateID = CharacterTemplateID;
            if (!templateID.HasValue)
                return;

            var template = CharacterTemplateManager[templateID.Value];
            if (template == null)
                return;

            var spawnInventory = template.Inventory;
            var spawnEquipment = template.Equipment;

            // Create the items
            if (spawnInventory != null)
            {
                foreach (var inventoryItem in spawnInventory)
                {
                    var item = inventoryItem.CreateInstance();
                    if (item == null)
                        continue;

                    var extraItems = Inventory.Add(item);
                    if (extraItems != null)
                        extraItems.Dispose();
                }
            }

            if (spawnEquipment != null)
            {
                foreach (var equippedItem in spawnEquipment)
                {
                    var item = equippedItem.CreateInstance();
                    if (item == null)
                        continue;

                    if (!Equipped.Equip(item))
                        item.Dispose();
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.KilledCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="killed">The <see cref="Character"/> that this <see cref="Character"/> killed.</param>
        protected override void OnKilledCharacter(Character killed)
        {
            base.OnKilledCharacter(killed);

            var killedUser = killed as User;
            if (killedUser != null)
            {
                WorldStatsTracker.Instance.AddNPCKillUser(this, killedUser);
                if (CharacterTemplateID.HasValue)
                    WorldStatsTracker.Instance.AddCountNPCKillUser((int)CharacterTemplateID.Value, (int)killedUser.ID);
            }
        }

        /// <summary>
        /// Removes the Character's AI.
        /// </summary>
        public override void RemoveAI()
        {
            _ai = null;
        }

        /// <summary>
        /// Attempts to set the Character's AI.
        /// </summary>
        /// <param name="aiID">The ID of the new AI to use.</param>
        /// <returns>
        /// True if the AI was successfully set; otherwise false.
        /// </returns>
        public override bool SetAI(AIID aiID)
        {
            var newAI = _aiFactory.Create(aiID, this);
            if (newAI == null)
            {
                _ai = null;
                return false;
            }

            Debug.Assert(newAI.Actor == this);
            _ai = newAI;

            return true;
        }

        /// <summary>
        /// Attempts to set the Character's AI.
        /// </summary>
        /// <param name="aiName">The name of the new AI to use.</param>
        /// <returns>
        /// True if the AI was successfully set; otherwise false.
        /// </returns>
        public override bool SetAI(string aiName)
        {
            var newAI = _aiFactory.Create(aiName, this);
            if (newAI == null)
            {
                _ai = null;
                return false;
            }

            Debug.Assert(newAI.Actor == this);
            _ai = newAI;

            return true;
        }

        void SetShopFromID(ShopID? shopID)
        {
            if (!shopID.HasValue)
            {
                _shop = null;
                return;
            }

            if (!ShopManager.TryGetValue(shopID.Value, out _shop))
            {
                const string errmsg = "Failed to load shop with ID `{0}` for NPC `{1}`. Setting shop as null.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, shopID.Value, this);
                Debug.Fail(string.Format(errmsg, shopID.Value, this));
                _shop = null;
            }
        }

        #region IQuestProvider<User> Members

        /// <summary>
        /// Gets the quests that this quest provider provides.
        /// </summary>
        public IEnumerable<IQuest<User>> Quests
        {
            get { return _quests; }
        }

        #endregion
    }
}