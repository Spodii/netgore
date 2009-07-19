using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// A non-player character
    /// </summary>
    public class NPC : Character
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        AIBase _ai;

        ushort _giveCash;
        ushort _giveExp;
        ushort _respawnSecs;

        /// <summary>
        /// The game time at which the NPC will respawn.
        /// </summary>
        int _respawnTime = 0;

        /// <summary>
        /// Gets the NPC's AI. Can be null.
        /// </summary>
        public AIBase AI
        {
            get { return _ai; }
        }

        public ushort GiveCash
        {
            get { return _giveCash; }
            protected set { _giveCash = value; }
        }

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
        /// Gets if this NPC will respawn after dieing.
        /// </summary>
// ReSharper disable MemberCanBeMadeStatic.Global
        public bool WillRespawn // ReSharper restore MemberCanBeMadeStatic.Global
        {
            get { return RespawnMapIndex.HasValue; }
        }

        [Obsolete("Do not use this empty constructor on the Server!")]
        public NPC()
        {
        }

        public NPC(World parent, CharacterID characterID) : base(parent, true)
        {
            // HACK: This whole constructor is uber hax
            if (parent == null)
                throw new ArgumentNullException("parent");

            Alliance = AllianceManager.GetAlliance("monster");

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
        public NPC(World parent, CharacterTemplate template, Map map, Vector2 position) : base(parent, false)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (template == null)
                throw new ArgumentNullException("template");

            // Create the AI
            if (!string.IsNullOrEmpty(template.AIName))
                SetAI(template.AIName);

            // Set the rest of the template stuff
            Load(template);
            _respawnSecs = template.RespawnSecs;
            _giveExp = template.GiveExp;
            _giveCash = template.GiveCash;

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
        /// When overridden in the derived class, checks if enough time has elapesd since the Character died
        /// for them to be able to respawn.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        /// <returns>True if enough time has elapsed; otherwise false.</returns>
        protected override bool CheckRespawnElapsedTime(int currentTime)
        {
            return currentTime > _respawnTime;
        }

        protected override CharacterEquipped CreateEquipped()
        {
            return new NPCEquipped(this);
        }

        protected override CharacterInventory CreateInventory()
        {
            return new NPCInventory(this);
        }

        protected override CharacterStatsBase CreateStats(StatCollectionType statCollectionType)
        {
            return new NPCStats(this, statCollectionType);
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
                    Inventory.RemoveAt(item.Key);
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
            if (!TemplateID.HasValue)
                return;

            CharacterTemplate template = CharacterTemplateManager.GetTemplate(TemplateID.Value);
            if (template == null)
                return;

            _giveCash = template.GiveCash;
            _giveExp = template.GiveExp;
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
            var templateID = TemplateID;
            if (!templateID.HasValue)
                return;

            CharacterTemplate template = CharacterTemplateManager.GetTemplate(templateID.Value);
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
        /// Sets the NPC's AI.
        /// </summary>
        /// <param name="aiName">Name of the AI to change to. Use a null or empty string to set the AI to null.</param>
        public void SetAI(string aiName)
        {
            if (string.IsNullOrEmpty(aiName))
                _ai = null;
            else
            {
                AIBase newAI;
                try
                {
                    newAI = AIFactory.Create(aiName, this);
                }
                catch (KeyNotFoundException)
                {
                    const string errmsg = "Failed to change to AI to `{0}` for NPC `{1}` - AI not found.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, aiName, this);
                    Debug.Fail(string.Format(errmsg, aiName, this));
                    return;
                }

                Debug.Assert(newAI.Actor == this);
                _ai = newAI;
            }
        }

        /// <summary>
        /// Updates the NPC
        /// </summary>
        public override void Update(IMap imap, float deltaTime)
        {
            // Check for spawning if dead
            if (!IsAlive)
                return;

            // Update the AI
            AIBase ai = AI;
            if (ai != null)
                ai.Update();

            // Perform the base update of the character
            base.Update(imap, deltaTime);
        }
    }
}