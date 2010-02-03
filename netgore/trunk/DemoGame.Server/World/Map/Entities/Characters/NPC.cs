using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.NPCChat;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.AI;
using NetGore.Features.Shops;
using NetGore.NPCChat;
using NetGore.Stats;

namespace DemoGame.Server
{
    /// <summary>
    /// A non-player character
    /// </summary>
    public class NPC : Character
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        IAI _ai;
        NPCChatDialogBase _chatDialog;

        ushort _giveCash;
        ushort _giveExp;
        ushort _respawnSecs;

        /// <summary>
        /// The game time at which the NPC will respawn.
        /// </summary>
        int _respawnTime = 0;

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

            Alliance = AllianceManager["monster"];

            Load(characterID);

            if (log.IsInfoEnabled)
                log.InfoFormat("Created persistent NPC `{0}` from CharacterID `{1}`.", this, characterID);

            LoadPersistentNPCTemplateInfo();
        }

        /// <summary>
        /// NPC constructor.
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

            ICharacterTemplateTable v = template.TemplateTable;

            // Set the rest of the template stuff
            Load(template);
            _respawnSecs = v.Respawn;
            _giveExp = v.GiveExp;
            _giveCash = v.GiveCash;

            RespawnMapIndex = map.Index;
            RespawnPosition = position;

            LoadSpawnItems();

            // Done loading
            SetAsLoaded();

            if (log.IsInfoEnabled)
                log.InfoFormat("Created NPC instance from template `{0}`.", template);

            // Spawn
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Teleport(position);
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
            ChangeMap(map);
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
        // ReSharper disable MemberCanBeMadeStatic.Global
        public bool WillRespawn // ReSharper restore MemberCanBeMadeStatic.Global
        {
            get { return RespawnMapIndex.HasValue; }
        }

        /// <summary>
        /// When overridden in the derived class, checks if enough time has elapesd since the Character died
        /// for them to be able to respawn.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        /// <returns>True if enough time has elapsed; otherwise false.</returns>
        protected override bool CheckRespawnElapsedTime(int currentTime)
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
        protected override CharacterStatsBase CreateStats(StatCollectionType statCollectionType)
        {
            return new NPCStats(statCollectionType);
        }

        /// <summary>
        /// When overridden in the derived class, handles additional loading stuff.
        /// </summary>
        /// <param name="v">The ICharacterTable containing the database values for this Character.</param>
        protected override void HandleAdditionalLoading(ICharacterTable v)
        {
            base.HandleAdditionalLoading(v);

            if (v.ChatDialog.HasValue)
                _chatDialog = NPCChatManager.GetDialog(v.ChatDialog.Value);

            if (v.ShopID.HasValue)
            {
                if (!ShopManager.TryGetValue(v.ShopID.Value, out _shop))
                {
                    const string errmsg = "Failed to load shop with ID `{0}` for NPC `{1}`. Setting shop as null.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, v.ShopID.Value, this);
                    Debug.Fail(string.Format(errmsg, v.ShopID.Value, this));
                    _shop = null;
                }
            }
        }

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected override void HandleUpdate(IMap imap, float deltaTime)
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
                    Inventory.RemoveAt(item.Key, false);
                    DropItem(item.Value);
                }
            }

            // Remove equipment
            foreach (var item in Equipped)
            {
                Equipped.RemoveAt(item.Key);
            }

            // Check to respawn
            if (WillRespawn)
            {
                // Start the respawn sequence
                IsAlive = false;
                _respawnTime = GetTime() + (RespawnSecs * 1000);

                LoadSpawnItems();

                ChangeMap(null);
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

            CharacterTemplate template = CharacterTemplateManager[CharacterTemplateID.Value];
            if (template == null)
                return;

            var v = template.TemplateTable;

            _giveCash = v.GiveCash;
            _giveExp = v.GiveExp;
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

            CharacterTemplate template = CharacterTemplateManager[templateID.Value];
            if (template == null)
                return;

            var spawnInventory = template.Inventory;
            var spawnEquipment = template.Equipment;

            // Create the items
            if (spawnInventory != null)
            {
                foreach (CharacterTemplateInventoryItem inventoryItem in spawnInventory)
                {
                    ItemEntity item = inventoryItem.CreateInstance();
                    if (item == null)
                        continue;

                    ItemEntity extraItems = Inventory.Add(item);
                    if (extraItems != null)
                        extraItems.Dispose();
                }
            }

            if (spawnEquipment != null)
            {
                foreach (CharacterTemplateEquipmentItem equippedItem in spawnEquipment)
                {
                    ItemEntity item = equippedItem.CreateInstance();
                    if (item == null)
                        continue;

                    if (!Equipped.Equip(item))
                        item.Dispose();
                }
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
            var newAI = AIFactory.Instance.Create(aiID, this);
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
            var newAI = AIFactory.Instance.Create(aiName, this);
            if (newAI == null)
            {
                _ai = null;
                return false;
            }

            Debug.Assert(newAI.Actor == this);
            _ai = newAI;

            return true;
        }
    }
}