using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Handles Character events.
    /// </summary>
    /// <param name="character">The Character that the event took place on.</param>
    public delegate void CharacterEventHandler(Character character);

    /// <summary>
    /// Handles Character attack events.
    /// </summary>
    /// <param name="attacker">The Character that did the attacking.</param>
    /// <param name="attacked">The Character that was attacked.</param>
    /// <param name="damage">Amount of damage that was inflicted.</param>
    public delegate void CharacterAttackCharacterEventHandler(Character attacker, Character attacked, int damage);

    /// <summary>
    /// Handles Character Map events.
    /// </summary>
    /// <param name="character">The Character that the event took place on.</param>
    /// <param name="map">The Map related to the event.</param>
    public delegate void CharacterMapEventHandler(Character character, Map map);

    /// <summary>
    /// Handles Character killing events.
    /// </summary>
    /// <param name="killed">The Character that was killed.</param>
    /// <param name="killer">The Character that did the killing.</param>
    public delegate void CharacterKillEventHandler(Character killed, Character killer);

    /// <summary>
    /// Handles Character item events.
    /// </summary>
    /// <param name="character">The Character that the event took place on.</param>
    /// <param name="item">The ItemEntity related to the event.</param>
    public delegate void CharacterItemEventHandler(Character character, ItemEntity item);

    /// <summary>
    /// A game character
    /// </summary>
    public abstract class Character : CharacterEntity, IGetTime
    {
        /// <summary>
        /// Amount of time the character must wait between attacks
        /// </summary>
        const int _attackTimeout = 500;

        /// <summary>
        /// Random number generator for Characters
        /// </summary>
        static readonly Random _rand = new Random();

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly World _world;

        /// <summary>
        /// Character's alliance.
        /// </summary>
        Alliance _alliance;

        /// <summary>
        /// Gets if this Character's existant is persistent. If true, the Character's state will be synchronized
        /// into the database. If false, the Character's state will only exist in the Character object and cannot
        /// be restored after object is lost (such as a server reset).
        /// </summary>
        public bool IsPersistent { get { return _isPersistent; } }

        readonly bool _isPersistent;

        /// <summary>
        /// If the character is alive or not.
        /// </summary>
        bool _isAlive = false;

        bool _isLoaded = false;

        /// <summary>
        /// Time at which the character last performed an attack.
        /// </summary>
        int _lastAttackTime;

        /// <summary>
        /// Map the character is currently on.
        /// </summary>
        Map _map;

        /// <summary>
        /// Name of the character.
        /// </summary>
        string _name;

        /// <summary>
        /// Notifies listeners when the Character performs an attack. The attack does not have to actually hit
        /// anything for this event to be raised. This will be raised before <see cref="OnAttackCharacter"/>.
        /// </summary>
        public event CharacterEventHandler OnAttack;

        /// <summary>
        /// Notifies listeners when this Character has successfully attacked another Character.
        /// </summary>
        public event CharacterAttackCharacterEventHandler OnAttackCharacter;

        /// <summary>
        /// Notifies listeners when this Character has been attacked by another Character.
        /// </summary>
        public event CharacterAttackCharacterEventHandler OnAttackedByCharacter;

        /// <summary>
        /// Notifies listeners when this Character has killed another Character.
        /// </summary>
        public event CharacterKillEventHandler OnKillCharacter;

        /// <summary>
        /// Notifies listeners when this Character has dropped an item.
        /// </summary>
        public event CharacterItemEventHandler OnDropItem;

        /// <summary>
        /// Notifies listeners when this Character has received an item.
        /// </summary>
        public event CharacterItemEventHandler OnGetItem; // TODO: Implement. Difficulty implementing is due to the inventory system making a deep copy of things. Should probably add some events to the InventoryBase.

        /// <summary>
        /// Notifies listeners when this Character uses an item.
        /// </summary>
        public event CharacterItemEventHandler OnUseItem;

        /// <summary>
        /// Notifies listeners when this Character has been killed in any way, no matter who did it or how it happened.
        /// </summary>
        public event CharacterEventHandler OnKilled;

        /// <summary>
        /// Notifies listeners when this Character has been killed by another Character.
        /// </summary>
        public event CharacterKillEventHandler OnKilledByCharacter;

        /// <summary>
        /// Gets a random number generator to be used for Characters.
        /// </summary>
        protected static Random Rand
        {
            get { return _rand; }
        }

        /// <summary>
        /// Gets or sets (protected) the Character's alliance.
        /// </summary>
        public Alliance Alliance
        {
            get { return _alliance; }
            protected set { _alliance = value; }
        }

        /// <summary>
        /// Gets the Character's Inventory.
        /// </summary>
        public abstract Inventory Inventory { get; }


        /// <summary>
        /// Gets the Character's equipped items.
        /// </summary>
        public abstract CharacterEquipped Equipped { get; }

        /// <summary>
        /// Gets or sets (protected) if the Character is currently alive.
        /// </summary>
        public bool IsAlive
        {
            get { return _isAlive; }
            protected set { _isAlive = value; }
        }

        /// <summary>
        /// Gets if the Character has been loaded. If this is false, it is assumed
        /// that changes to stats are because the Character is being loaded, and not that their stats have
        /// changed, and thus will be handled differently.
        /// </summary>
        protected bool IsLoaded
        {
            get { return _isLoaded; }
        }

        /// <summary>
        /// Gets or sets the map the character is currently on.
        /// </summary>
        public Map Map
        {
            get { return _map; }
            internal set { _map = value; }
        }

        /// <summary>
        /// Gets or sets the name of the character.
        /// </summary>
        public override string Name
        {
            get { return _name; }
            set
            {
                // Check that the name is valid
                // TODO: Need to find a good way to handle validating the User and NPC names individually
                /*
                if (!GameData.IsValidCharName(value))
                {
                    const string errmsg = "Attempted to give Character `{0}` an invalid name `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, _name, value);
                    Debug.Fail(string.Format(errmsg, _name, value));
                    return;
                }
                */

                // Set the new name
                _name = value;
            }
        }

        /// <summary>
        /// Gets the CharacterStatsBase used for this Character's stats.
        /// </summary>
        public abstract CharacterStatsBase Stats { get; }

        /// <summary>
        /// Gets the world the NPC belongs to.
        /// </summary>
        public World World
        {
            get { return _world; }
        }

        protected Character()
        {
            throw new MethodAccessException("Character's empty constructor should never be used on the server since the " +
                                            "server never needs to deserialize a DynamicEntity.");
        }


        public DBController DBController
        {
            get { return World.Server.DBController; }
        }

        CharacterID _id;

        public CharacterID ID
        {
            get
            {
                return _id;
            }
        }

        void InternalLoad(SelectCharacterQueryValues characterValues)
        {
            Name = characterValues.Name;
            _id = characterValues.ID;

            BodyInfo = GameData.Body(characterValues.BodyIndex);
            CB = new CollisionBox(characterValues.Position, BodyInfo.Width, BodyInfo.Height);

            // Set the Map and, if a User, add them to the World
            Map m = World.GetMap(characterValues.MapIndex);
            // TODO: We can recover when a NPC's map is invalid at least...
            if (m == null)
                throw new Exception(string.Format("Unable to get Map with index `{0}`.", characterValues.MapIndex));

            User asUser = this as User;
            if (asUser != null)
                World.AddUser(asUser);

            // HACK: Ugh... horrible place to set the RespawnMap
            if (this is NPC)
                ((NPC)this).RespawnMap = m;

            ChangeMap(m);

            // Load the Character's items
            Inventory.Load();
            Equipped.Load();

            // Mark the Character as loaded
            SetAsLoaded();
        }


        /// <summary>
        /// Lets us know if we have saved the Character since they have been updated. Used to ensure saves aren't
        /// called back-to-back without any values changing in-between.
        /// </summary>
        bool _saved = false;

        /// <summary>
        /// Saves the Character's information. If IsPersistent is false, this will do nothing, but will not
        /// raise any Exceptions.
        /// </summary>
        public void Save()
        {
            if (!IsPersistent)
                return;

            // Do not save if the user is already saved
            if (_saved)
                return;

            // Set the user as saved
            _saved = true;

            // Execute the user save query
            DBController.GetQuery<UpdateCharacterQuery>().Execute(this);

            if (log.IsInfoEnabled)
                log.InfoFormat("Saved Character `{0}`.", this);
        }

        public override string ToString()
        {
            return string.Format("{0} [ID: {1}, Type: {2}]", Name, ID, GetType().Name);
        }

        protected void Load(CharacterID characterID)
        {
            var values = DBController.GetQuery<SelectCharacterByIDQuery>().Execute(characterID, Stats);
            InternalLoad(values);
        }

        protected void Load(string characterName)
        {
            var values = DBController.GetQuery<SelectCharacterQuery>().Execute(characterName, Stats);
            InternalLoad(values);
        }

        /// <summary>
        /// Character constructor.
        /// </summary>
        /// <param name="world">World that the character belongs to.</param>
        /// <param name="isPersistent">If the Character's state is persistent. If true, Load() MUST be called
        /// at some point during the Character's constructor!</param>
        protected Character(World world, bool isPersistent)
        {
            _world = world;
            _isPersistent = isPersistent;
        }

        /// <summary>
        /// Makes the character perform an attack.
        /// </summary>
        public void Attack()
        {
            int currTime = GetTime();

            // Ensure enough time has elapsed since the last attack
            if (currTime - _lastAttackTime <= _attackTimeout)
                return;

            // Update the last attack time to now
            _lastAttackTime = currTime;

            if (OnAttack != null)
                OnAttack(this);

            // Inform the map that the user has performed an attack
            using (PacketWriter charAttack = ServerPacket.CharAttack(MapEntityIndex))
            {
                Map.SendToArea(Position, charAttack);
            }

            // Damage all hit characters
            Rectangle hitRect = BodyInfo.GetHitRect(this, BodyInfo.PunchRect);
            List<Character> hitChars = Map.GetEntities<Character>(hitRect);
            foreach (Character c in hitChars)
            {
                Attack(c);
            }
        }

        /// <summary>
        /// Tries to attacks a specific target Character. The attack can fail if the target is an invalid
        /// Character for this Character to attack.
        /// </summary>
        /// <param name="target">Character to attack</param>
        void Attack(Character target)
        {
            if (target == null)
            {
                Debug.Fail("target is null.");
                return;
            }

            // Only attack living characters
            if (!target.IsAlive)
                return;

            // Don't attack self
            if (target == this)
                return;

            // Check that the alliance allows the character to attack the target
            if (!Alliance.CanAttack(target.Alliance))
                return;

            // Get the damage
            int damage = GetAttackDamage(target);

            // Apply the damage to the target
            target.Damage(this, damage);

            // Raise attack events
            if (OnAttackCharacter != null)
                OnAttackCharacter(this, target, damage);

            if (target.OnAttackedByCharacter != null)
                target.OnAttackedByCharacter(this, target, damage);
        }

        /// <summary>
        /// Changes the Character's map.
        /// </summary>
        /// <param name="newMap">New map to place the Character on.</param>
        public void ChangeMap(Map newMap)
        {
            if (Map == newMap)
            {
                Debug.Fail("Character is already on this map.");
                return;
            }

            // Remove the Character from the last map
            if (Map != null)
                Map.RemoveEntity(this);

            _map = null;

            // Set the Character's new map
            if (newMap != null)
                newMap.AddEntity(this);
        }

        /// <summary>
        /// Applies damage to the Character.
        /// </summary>
        /// <param name="source">Entity the damage came from. Can be null.</param>
        /// <param name="damage">Amount of damage to apply to the Character. Does not include damage reduction
        /// from defense or any other kind of damage alterations since these are calculated here.</param>
        public virtual void Damage(Entity source, int damage)
        {
            // Apply damage
            using (PacketWriter pw = ServerPacket.CharDamage(MapEntityIndex, damage))
            {
                Map.SendToArea(Position, pw);
            }
            Stats[StatType.HP] -= damage;

            // Check if the character died
            if (Stats[StatType.HP] <= 0)
            {
                if (source != null)
                {
                    var sourceCharacter = source as Character;
                    if (sourceCharacter != null)
                    {
                        if (sourceCharacter.OnKillCharacter != null)
                            sourceCharacter.OnKillCharacter(this, sourceCharacter);

                        if (OnKilledByCharacter != null)
                            OnKilledByCharacter(this, sourceCharacter);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the Character to the World's DisposeStack, allowing for the Dispose to
        /// happen later. It is strongly recommended you use this over Dispose to avoid
        /// InvalidOperation exceptions from the Lists containing the Character.
        /// </summary>
        public void DelayedDispose()
        {
            Stack<IDisposable> stack = Map.World.DisposeStack;
            if (!stack.Contains(this))
                stack.Push(this);
        }

        /// <summary>
        /// Makes the Character drop an existing item. This does NOT remove the ItemEntity from the Character in any
        /// way. Be sure to remove the ItemEntity from the Character first if needed.
        /// </summary>
        /// <param name="item">ItemEntity to drop.</param>
        public void DropItem(ItemEntity item)
        {
            Vector2 dropPos = GetDropPos();
            item.Position = dropPos;

            // Add the item to the map
            Map.AddEntity(item);

            if (OnDropItem != null)
                OnDropItem(this, item);
        }

        /// <summary>
        /// Makes the Character drop an item. Does not modify the item requested to drop at all or anything,
        /// so if you want to also remove the item, such as with dropping an item from the Inventory,
        /// this will not take care of that.
        /// </summary>
        /// <param name="itemTemplate">ItemTemplate for the item to drop.</param>
        /// <param name="amount">Amount of the item to drop.</param>
        protected void DropItem(ItemTemplate itemTemplate, byte amount)
        {
            Vector2 dropPos = GetDropPos();

            // Create the item on the map
            var droppedItem = Map.CreateItem(itemTemplate, dropPos, amount);
            
            if (OnDropItem != null)
                OnDropItem(this, droppedItem);
        }

        /// <summary>
        /// Gets the amount of damage for a normal attack.
        /// </summary>
        /// <param name="target">Character being attacked.</param>
        /// <returns>The amount of damage to inflict for a normal attack.</returns>
        public int GetAttackDamage(Character target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            int damage = Rand.Next(Stats[StatType.MinHit], Stats[StatType.MaxHit]);

            // Apply the defence, and ensure the damage is in a valid range
            int defence;
            if (!target.Stats.TryGetStatValue(StatType.Defence, out defence))
                defence = 0;

            damage -= defence / 2;

            if (damage < 1)
                damage = 1;

            return damage;
        }

        Vector2 GetDropPos()
        {
            const int _dropRange = 32;

            // Get the center point of the Character
            Vector2 dropPos = Position + (Size / 2);

            // Move the X point randomly dropRange pixels in either direction
            dropPos.X += -_dropRange + Rand.Next(_dropRange * 2);

            return dropPos;
        }

        /// <summary>
        /// Gets the map interface used by the Character, primarily for when referencing by the CharacterEntity.
        /// Can return null if the Character is not on a map, and null returns should be supported.
        /// </summary>
        /// <returns>Map interface used by the Character. Can be null.</returns>
        protected override IMap GetIMap()
        {
            return Map;
        }

        /// <summary>
        /// Gives an item to the Character to be placed in their Inventory.
        /// </summary>
        /// <param name="item">Item to give to the character.</param>
        /// <returns>The remainder of the item that failed to be added to the inventory, or null if all of the
        /// item was added.</returns>
        public abstract ItemEntity GiveItem(ItemEntity item);

        /// <summary>
        /// Handles when the Character's HP changes.
        /// </summary>
        /// <param name="stat">Stat that changed.</param>
        void HP_OnChange(IStat stat)
        {
            int hp = Stats[StatType.HP];
            int maxHP = Stats[StatType.MaxHP];

            if (hp > maxHP)
            {
                // Keep the HP in a valid range
                Stats[StatType.HP] = maxHP;
            }
            else if (hp <= 0)
            {
                // No more HP, no more living
                Kill();
            }
        }

        /// <summary>
        /// Checks if a username is valid
        /// </summary>
        /// <param name="name">Name of the character</param>
        /// <returns>True if the name is valid, else false</returns>
        public static bool IsValidName(string name)
        {
            return GameData.IsValidCharName(name);
        }

        /// <summary>
        /// Checks if a password is valid
        /// </summary>
        /// <param name="name">Password for the character</param>
        /// <returns>True if the name is valid, else false</returns>
        public static bool IsValidPassword(string name)
        {
            return GameData.IsValidCharName(name);
        }

        /// <summary>
        /// Makes the Character jump (CanJump must be true)
        /// </summary>
        public void Jump()
        {
            if (!CanJump)
                return;

            SetVelocity(Velocity + new Vector2(0.0f, -0.48f));
        }

        /// <summary>
        /// When overridden in the derived class, implements the Character being killed. This 
        /// doesn't actually care how the Character was killed, it just takes the appropriate
        /// actions to kill them.
        /// </summary>
        public virtual void Kill()
        {
            if (OnKilled != null)
                OnKilled(this);
        }

        /// <summary>
        /// Starts moving the character to the left
        /// </summary>
        public void MoveLeft()
        {
            if (IsMovingLeft)
                return;

            SetVelocity(Velocity + new Vector2(-0.18f, 0.0f));
        }

        /// <summary>
        /// Starts moving the character to the right
        /// </summary>
        public void MoveRight()
        {
            if (IsMovingRight)
                return;

            SetVelocity(Velocity + new Vector2(0.18f, 0.0f));
        }

        /// <summary>
        /// Handles when the Character's MP changes.
        /// </summary>
        /// <param name="stat">Stat that changed.</param>
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
        /// Sets the Character to being loaded. Must be called after the Character has been loaded.
        /// </summary>
        protected void SetAsLoaded()
        {
            Debug.Assert(!_isLoaded, "SetAsLoaded() has already been called on this Character.");
            _isLoaded = true;

            // Hook some event listeners
            Stats.GetStat(StatType.HP).OnChange += HP_OnChange;
            Stats.GetStat(StatType.MP).OnChange += MP_OnChange;
        }

        /// <summary>
        /// Stops all of the character's horizontal movement.
        /// </summary>
        public override void StopMoving()
        {
            if (!IsMoving)
                return;

            base.StopMoving();
        }

        /// <summary>
        /// Teleports the character to a new position and informs clients in the area of
        /// interest that the character has teleported.
        /// </summary>
        /// <param name="position">Position to teleport to.</param>
        public override void Teleport(Vector2 position)
        {
            if (Map == null && IsAlive && IsLoaded)
            {
                const string errmsg = "Attempted to teleport a Character `{0}` while their map was null.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            base.Teleport(position);
        }

        /// <summary>
        /// Updates the character and their state.
        /// </summary>
        public override void Update(IMap imap, float deltaTime)
        {
            Debug.Assert(imap == Map, "Character.Update()'s imap is, for whatever reason, not equal to the set Map.");

            // Update shouldn't be called on disposed Characters since they shouldn't be referenced
            // by the Map anymore
            if (IsDisposed)
            {
                const string errmsg = "Updated called on disposed Character `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, Name);
                Debug.Fail(string.Format(errmsg, Name));
                return;
            }

            // Set the Character as not saved
            _saved = false;

            base.Update(imap, deltaTime);
        }

        protected override void HandleDispose()
        {
            // Make sure the Character was saved
            Save();

            base.HandleDispose();
        }

        bool UseEquipment(ItemEntity item, InventorySlot? inventorySlot)
        {
            // Only allow users to equip items
            var user = this as User;
            if (user == null)
                return false;

            if (!inventorySlot.HasValue)
            {
                // Equip an item not from the inventory
                return user.Equipped.Equip(item);
            }
            else
            {
                // Equip an item from the inventory
                return user.Equip(inventorySlot.Value);
            }
        }

        /// <summary>
        /// Makes the Character use an item.
        /// </summary>
        /// <param name="item">Item to use.</param>
        /// <param name="inventorySlot">Inventory slot of the item being used, or null if not used from the inventory.</param>
        /// <returns>True if the item was successfully used, else false.</returns>
        public bool UseItem(ItemEntity item, InventorySlot? inventorySlot)
        {
            // Check for a valid amount
            if (item.Amount <= 0)
            {
                const string errmsg = "Attempted to use item `{0}`, but the amount was invalid.";
                Debug.Fail(string.Format(errmsg, item));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, item);
                return false;
            }

            // Use the item based on the item's type
            bool wasUsed;
            switch (item.Type)
            {
                case ItemType.Unusable:
                    wasUsed = false;
                    break;

                case ItemType.UseOnce:
                    wasUsed = UseItemUseOnce(item);
                    break;

                case ItemType.Weapon:
                case ItemType.Helmet:
                case ItemType.Body:
                    wasUsed = UseEquipment(item, inventorySlot);
                    break;

                default:
                    // Unhandled item type
                    const string errmsg = "Attempted to use item `{0}`, but it contains invalid or unhandled ItemType `{1}`.";
                    Debug.Fail(string.Format(errmsg, item, item.Type));
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, item, item.Type);

                    wasUsed = false;
                    break;
            }

            if (wasUsed && OnUseItem != null)
                OnUseItem(this, item);

            return wasUsed;
        }

        bool UseItemUseOnce(ItemEntity item)
        {
            IEnumerable<IStat> useBonuses = item.Stats.Where(stat => stat.Value != 0);
            foreach (IStat stat in useBonuses)
            {
                Stats[stat.StatType] += stat.Value;
            }

            return true;
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public int GetTime()
        {
            return World.GetTime();
        }

        #endregion
    }
}