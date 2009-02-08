using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Platyform;
using Platyform.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// A non-player character
    /// </summary>
    public class NPC : Character
    {
        /// <summary>
        /// NPC's AI module
        /// </summary>
        readonly AIBase _ai;

        readonly List<NPCDrop> _drops;
        readonly NPCInventory _inventory;
        readonly NPCStats _stats;

        ushort _giveCash;
        ushort _giveExp;

        ushort _respawnSecs;

        /// <summary>
        /// The game time at which the NPC will respawn (IsAlive must be false)
        /// </summary>
        int _spawnTime = 0;

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
        /// Gets the Character's Inventory.
        /// </summary>
        public override Inventory Inventory
        {
            get { return _inventory; }
        }

        /// <summary>
        /// Gets or sets (protected) the amount of time it takes (in milliseconds) for the NPC to respawn
        /// </summary>
        public ushort RespawnSecs
        {
            get { return _respawnSecs; }
            protected set { _respawnSecs = value; }
        }

        public override CharacterStatsBase Stats
        {
            get { return _stats; }
        }

        /// <summary>
        /// NPC constructor
        /// </summary>
        /// <param name="parent">World that the NPC belongs to.</param>
        /// <param name="template">NPCTemplate used to create the NPC.</param>
        public NPC(World parent, NPCTemplate template) : base(parent)
        {
            if (template == null)
                throw new ArgumentNullException("template");
            if (parent == null)
                throw new ArgumentNullException("parent");

            // Set up the inventory
            _inventory = new NPCInventory(this);

            // Set up the NPC
            Name = template.Name;
            Alliance = template.Alliance;
            BodyInfo = GameData.Body(template.BodyIndex);
            CB = new CollisionBox(BodyInfo.Width, BodyInfo.Height);

            // Create the AI
            if (!string.IsNullOrEmpty(template.AIName))
                _ai = AIBase.CreateInstance(template.AIName, this);

            // Create and copy over the stats
            _stats = new NPCStats(this);
            _stats.CopyStatValuesFrom(template.Stats, false);

            _stats[StatType.HP] = _stats[StatType.MaxHP];
            _stats[StatType.MP] = _stats[StatType.MaxMP];

            _stats.GetStat(StatType.HP).OnChange += HP_OnChange;
            _stats.GetStat(StatType.MP).OnChange += MP_OnChange;

            // Set the rest of the template stuff
            _respawnSecs = template.RespawnSecs;
            _giveExp = template.GiveExp;
            _giveCash = template.GiveCash;
            _drops = template.Drops.ToList();

            // Done loading
            SetAsLoaded();
        }

        public override PacketWriter GetCreationData()
        {
            // We do not notify about dead NPCs
            if (!IsAlive)
                return null;

            return ServerPacket.CreateNPC(MapCharIndex, Name, Position, BodyInfo.Index);
        }

        /// <summary>
        /// Gives an item to the Character.
        /// </summary>
        /// <param name="item">Item to give to the character.</param>
        /// <returns>The remainder of the item that failed to be added to the inventory, or null if all of the
        /// item was added.</returns>
        public override ItemEntity GiveItem(ItemEntity item)
        {
            return _inventory.Add(item);
        }

        void HP_OnChange(IStat stat)
        {
            int hp = Stats[StatType.HP];
            int maxHP = Stats[StatType.MaxHP];

            if (hp > maxHP)
                Stats[StatType.HP] = maxHP;
            else if (hp <= 0)
                Kill();
        }

        /// <summary>
        /// Kills the NPC, visually removing them from the map and starting their respawn timer
        /// </summary>
        public override void Kill()
        {
            if (!IsAlive)
            {
                Debug.Fail("Attempted to kill a dead NPC.");
                return;
            }
            if (Map == null)
            {
                Debug.Fail("Attempted to kill a NPC not on a map.");
                return;
            }

            // Drop items
            foreach (NPCDrop drop in _drops)
            {
                byte amount = drop.GetDropAmount();
                if (amount > 0)
                    DropItem(drop.ItemTemplate, amount);
            }

            // Remove the NPC from the clients (must be called before IsAlive is set to false!)
            using (PacketWriter removalData = GetRemovalData())
            {
                Map.Send(removalData);
            }

            // Start the respawn sequence
            IsAlive = false;
            _spawnTime = GetTime() + (RespawnSecs * 1000);
        }

        /// <summary>
        /// Handles what happens when the NPC is killed
        /// </summary>
        /// <param name="source">Entity that killed the NPC</param>
        protected override void KilledBy(Entity source)
        {
            // Killed by a user
            User user = source as User;
            if (user != null)
                user.GiveKillReward(GiveExp, GiveCash);
        }

        void MP_OnChange(IStat stat)
        {
            int mp = Stats[StatType.MP];
            int maxMP = Stats[StatType.MaxMP];

            if (mp > maxMP)
                Stats[StatType.MP] = maxMP;
            else if (mp < 0)
                Stats[StatType.MP] = 0;
        }

        /// <summary>
        /// Spawns the NPC on the map
        /// </summary>
        void Spawn()
        {
            // NPC must already be alive
            if (IsAlive)
            {
                Debug.Fail("Attempted to spawn a living NPC.");
                return;
            }
            if (Map == null)
            {
                Debug.Fail("Attempted to spawn a NPC with no map.");
                return;
            }

            // Restore the NPC's stats
            Stats[StatType.HP] = Stats[StatType.MaxHP];
            Stats[StatType.MP] = Stats[StatType.MaxMP];

            // Set the NPC's new location
            Teleport(new Vector2(560f, 490f));

            // Set the NPC as alive
            IsAlive = true;

            // Create the NPC on the clients (must be called after IsAlive is set to true!)
            using (PacketWriter pw = GetCreationData())
            {
                Map.Send(pw);
            }
        }

        /// <summary>
        /// Updates the NPC
        /// </summary>
        public override void Update(IMap imap, float deltaTime)
        {
            // Make sure there are users on the map
            if (Map.Users.Count == 0)
                return;

            // Check for spawning if dead
            if (!IsAlive)
            {
                if (_spawnTime > GetTime())
                    return; // Still more time until the NPC spawns

                Spawn();
            }

            // Update the AI
            if (_ai != null)
                _ai.Update();

            // Perform the base update of the character
            base.Update(imap, deltaTime);
        }
    }
}