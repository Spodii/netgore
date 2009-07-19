using System;
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

    public delegate void CharacterExpEventHandler(Character character, uint oldExp, uint exp);

    public delegate void CharacterLevelEventHandler(Character character, byte oldLevel, byte level);

    public delegate void CharacterCashEventHandler(Character character, uint oldCash, uint cash);

    public delegate void CharacterChangeSPEventHandler(Character character, SPValueType oldValue, SPValueType newValue);

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
        readonly CharacterStatsBase _baseStats;
        readonly bool _isPersistent;
        readonly CharacterStatsBase _modStats;

        readonly World _world;

        /// <summary>
        /// Character's alliance.
        /// </summary>
        Alliance _alliance;

        uint _cash;
        uint _exp;
        uint _expSpent;
        CharacterID _id;

        /// <summary>
        /// If the character is alive or not.
        /// </summary>
        bool _isAlive = false;

        bool _isLoaded = false;

        /// <summary>
        /// Time at which the character last performed an attack.
        /// </summary>
        int _lastAttackTime;

        byte _level;

        /// <summary>
        /// Map the character is currently on.
        /// </summary>
        Map _map;

        /// <summary>
        /// Name of the character.
        /// </summary>
        string _name;

        uint _nextLevelExp;

        SPValueType _hp;

        public SPValueType HP
        {
            get
            {
                return _hp;
            }
            set
            {
                // Get the new value, ensuring it is in a valid range
                var max = ModStats[StatType.MaxHP];
                SPValueType newValue;
                if (value > max)
                    newValue = (SPValueType)max;
                else if (value < 0)
                    newValue = 0;
                else
                    newValue = value;

                // Check that the value has changed
                var oldValue = _hp;
                if (newValue == oldValue)
                    return;

                // Apply new value
                _hp = newValue;

                if (OnChangeHP != null)
                    OnChangeHP(this, oldValue, _hp);

                if (_hp <= 0)
                    Kill();
            }
        }

        SPValueType _mp;

        public SPValueType MP
        {
            get { return _mp; }
            set
            {
                // Get the new value, ensuring it is in a valid range
                var max = ModStats[StatType.MaxMP];
                SPValueType newValue;
                if (value > max)
                    newValue = (SPValueType)max;
                else if (value < 0)
                    newValue = 0;
                else
                    newValue = value;

                // Check that the value has changed
                var oldValue = _mp;
                if (newValue == oldValue)
                    return;

                // Apply new value
                _mp = newValue;

                if (OnChangeMP != null)
                    OnChangeMP(this, oldValue, _mp);
            }
        }

        public event CharacterChangeSPEventHandler OnChangeHP;

        public event CharacterChangeSPEventHandler OnChangeMP;

        /// <summary>
        /// Lets us know if we have saved the Character since they have been updated. Used to ensure saves aren't
        /// called back-to-back without any values changing in-between.
        /// </summary>
        bool _saved = false;

        CharacterTemplateID? _templateID;

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

        public event CharacterCashEventHandler OnChangeCash;
        public event CharacterExpEventHandler OnChangeExp;
        public event CharacterExpEventHandler OnChangeExpSpent;
        public event CharacterLevelEventHandler OnChangeLevel;

        /// <summary>
        /// Notifies listeners when the Character's TemplateID has changed.
        /// </summary>
        public event CharacterEventHandler OnChangeTemplateID;

        /// <summary>
        /// Notifies listeners when this Character has dropped an item.
        /// </summary>
        public event CharacterItemEventHandler OnDropItem;

        /// <summary>
        /// Notifies listeners when this Character has received an item.
        /// </summary>
        public event CharacterItemEventHandler OnGetItem;

        // TODO: Implement OnGetItem. Difficulty implementing is due to the inventory system making a deep copy of things. Should probably add some events to the InventoryBase.

        /// <summary>
        /// Notifies listeners when this Character has killed another Character.
        /// </summary>
        public event CharacterKillEventHandler OnKillCharacter;

        /// <summary>
        /// Notifies listeners when this Character has been killed in any way, no matter who did it or how it happened.
        /// </summary>
        public event CharacterEventHandler OnKilled;

        /// <summary>
        /// Notifies listeners when this Character has been killed by another Character.
        /// </summary>
        public event CharacterKillEventHandler OnKilledByCharacter;

        /// <summary>
        /// Notifies listeners when this Character uses an item.
        /// </summary>
        public event CharacterItemEventHandler OnUseItem;

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

        public CharacterStatsBase BaseStats
        {
            get { return _baseStats; }
        }

        public uint Cash
        {
            get { return _cash; }
            private set
            {
                if (_cash == value)
                    return;

                uint oldValue = _cash;
                _cash = value;

                if (OnChangeCash != null)
                    OnChangeCash(this, oldValue, _cash);
            }
        }

        public DBController DBController
        {
            get { return World.Server.DBController; }
        }

        /// <summary>
        /// Gets the Character's equipped items.
        /// </summary>
        public abstract CharacterEquipped Equipped { get; }

        public uint Exp
        {
            get { return _exp; }
            private set
            {
                if (_exp == value)
                    return;

                uint oldValue = _exp;
                _exp = value;

                if (OnChangeExp != null)
                    OnChangeExp(this, oldValue, _exp);
            }
        }

        public uint ExpSpent
        {
            get { return _expSpent; }
            private set
            {
                if (_expSpent == value)
                    return;

                uint oldValue = _expSpent;
                _expSpent = value;

                if (OnChangeExpSpent != null)
                    OnChangeExpSpent(this, oldValue, _expSpent);
            }
        }

        public CharacterID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the Character's Inventory.
        /// </summary>
        public abstract CharacterInventory Inventory { get; }

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
        /// Gets if this Character's existant is persistent. If true, the Character's state will be synchronized
        /// into the database. If false, the Character's state will only exist in the Character object and cannot
        /// be restored after object is lost (such as a server reset).
        /// </summary>
        public bool IsPersistent
        {
            get { return _isPersistent; }
        }

        public byte Level
        {
            get { return _level; }
            private set
            {
                if (_level == value)
                    return;

                byte oldValue = _level;
                _level = value;
                _nextLevelExp = GameData.LevelCost(_level);

                if (OnChangeLevel != null)
                    OnChangeLevel(this, oldValue, _level);
            }
        }

        /// <summary>
        /// Gets or sets the map the character is currently on.
        /// </summary>
        public Map Map
        {
            get { return _map; }
            internal set { _map = value; }
        }

        public CharacterStatsBase ModStats
        {
            get { return _modStats; }
        }

        readonly CharacterSPSynchronizer _spSync;

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

        public uint StatPoints
        {
            get { return Exp - ExpSpent; }
        }

        /// <summary>
        /// Gets or sets the ID of the CharacterTemplate that this Character was created from.
        /// This will not alter the Character in any way except for functions that make use of the
        /// CharacterTemplateID, such as Equipment and Inventory Items a NPC spawns with.
        /// </summary>
        public CharacterTemplateID? TemplateID
        {
            get { return _templateID; }
            set
            {
                if (_templateID == value)
                    return;

                _templateID = value;

                if (OnChangeTemplateID != null)
                    OnChangeTemplateID(this);
            }
        }

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

// ReSharper disable DoNotCallOverridableMethodsInConstructor
            _baseStats = CreateStats(StatCollectionType.Base);
            _modStats = CreateStats(StatCollectionType.Modified);
            _spSync = CreateSPSynchronizer();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        protected virtual CharacterSPSynchronizer CreateSPSynchronizer()
        {
            return new CharacterSPSynchronizer(this);
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
            var hitChars = Map.GetEntities<Character>(hitRect);
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

        protected abstract CharacterStatsBase CreateStats(StatCollectionType statCollectionType);

        /// <summary>
        /// Applies damage to the Character.
        /// </summary>
        /// <param name="source">Entity the damage came from. Can be null.</param>
        /// <param name="damage">Amount of damage to apply to the Character. Does not include damage reduction
        /// from defense or any other kind of damage alterations since these are calculated here.</param>
        public virtual void Damage(Entity source, int damage)
        {
            // TODO: Handle having Map == null

            // Apply damage
            using (PacketWriter pw = ServerPacket.CharDamage(MapEntityIndex, damage))
            {
                Map.SendToArea(Position, pw);
            }
            HP -= (SPValueType)damage;

            // Check if the character died
            if (HP <= 0)
            {
                if (source != null)
                {
                    Character sourceCharacter = source as Character;
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
            var stack = Map.World.DisposeStack;
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
            ItemEntity droppedItem = Map.CreateItem(itemTemplate, dropPos, amount);

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

            int damage = Rand.Next(ModStats[StatType.MinHit], ModStats[StatType.MaxHit]);

            // Apply the defence, and ensure the damage is in a valid range
            int defence;
            if (!target.ModStats.TryGetStatValue(StatType.Defence, out defence))
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

        protected virtual void GiveKillReward(uint exp, uint cash)
        {
            Exp += exp;
            Cash += cash;
        }

        protected override void HandleDispose()
        {
            // Make sure the Character was saved
            Save();

            // Dispose of disposable stuff
            if (Equipped != null)
                Equipped.Dispose();

            if (Inventory != null)
                Inventory.Dispose();

            base.HandleDispose();
        }

        void InternalLoad(SelectCharacterQueryValues v)
        {
            Name = v.Name;
            _id = v.ID;
            _templateID = v.TemplateID;

            BodyInfo = GameData.Body(v.BodyIndex);
            CB = new CollisionBox(v.Position, BodyInfo.Width, BodyInfo.Height);

            // Set the character information
            _level = v.Level;
            _expSpent = v.ExpSpent;
            _exp = v.Exp;
            _cash = v.Cash;
            _hp = v.HP;
            _mp = v.MP;

            // Load the stats
            _baseStats.CopyStatValuesFrom(v.Stats, true);

            // Set the Map and, if a User, add them to the World
            Map m = World.GetMap(v.MapIndex);
            // TODO: We can recover when a NPC's map is invalid at least...
            if (m == null)
                throw new Exception(string.Format("Unable to get Map with index `{0}`.", v.MapIndex));

            User asUser = this as User;
            if (asUser != null)
                World.AddUser(asUser);

            // HACK: The Respawn Map should be coming from the SelectCharacterQueryValues
            if (this is NPC)
                ((NPC)this).RespawnMap = m;

            ChangeMap(m);

            // Load the Character's items
            Inventory.Load();
            Equipped.Load();

            // Update the mod stats
            UpdateModStats();

            // Mark the Character as loaded
            SetAsLoaded();
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

        protected void Load(CharacterID characterID)
        {
            SelectCharacterQueryValues values = DBController.GetQuery<SelectCharacterByIDQuery>().Execute(characterID);
            InternalLoad(values);
        }

        protected void Load(string characterName)
        {
            SelectCharacterQueryValues values = DBController.GetQuery<SelectCharacterQuery>().Execute(characterName);
            InternalLoad(values);
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
        /// Makes the Character raise their base Stat of the corresponding type by one point, assuming they have enough
        /// points available to raise the Stat, and lowers the amount of spendable points accordingly.
        /// </summary>
        /// <param name="st">StatType of the stat to raise.</param>
        public void RaiseStat(StatType st)
        {
            int cost = GameData.StatCost(BaseStats[st]);

            if (StatPoints <= cost)
            {
                const string errmsg = "User `{0}` tried to raise stat `{1}`, but only has {2} of {3} points needed.";
                Debug.Fail(string.Format(errmsg, this, st, StatPoints, cost));
                return;
            }

            BaseStats[st]++;
            _expSpent += (uint)cost;
        }

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

        /// <summary>
        /// Sets the Character to being loaded. Must be called after the Character has been loaded.
        /// </summary>
        protected void SetAsLoaded()
        {
            Debug.Assert(!_isLoaded, "SetAsLoaded() has already been called on this Character.");
            _isLoaded = true;
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

        public override string ToString()
        {
            return string.Format("{0} [ID: {1}, Type: {2}]", Name, ID, GetType().Name);
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

            UpdateModStats();

            base.Update(imap, deltaTime);

            _spSync.Synchronize();
        }

        protected void UpdateModStats()
        {
            // FUTURE: This is called every goddamn Update(). That is WAY too much...
            foreach (IStat modStat in ModStats)
            {
                modStat.Value = CharacterModStatCalculator.Calculate(BaseStats, modStat.StatType, Equipped);
            }
        }

        bool UseEquipment(ItemEntity item, InventorySlot? inventorySlot)
        {
            // Only allow users to equip items
            User user = this as User;
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
            var useBonuses = item.BaseStats.Where(stat => stat.Value != 0);
            foreach (IStat stat in useBonuses)
            {
                BaseStats[stat.StatType] += stat.Value;
            }

            if (item.HP != 0)
                HP += item.HP;
            if (item.MP != 0)
                MP += item.MP;

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