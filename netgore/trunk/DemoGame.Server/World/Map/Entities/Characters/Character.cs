using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.AI;
using NetGore.Db;
using NetGore.Features.NPCChat;
using NetGore.Features.Shops;
using NetGore.Features.Skills;
using NetGore.Graphics.Extensions;
using NetGore.Network;
using NetGore.Stats;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// The server representation of a single Character that can be either player-controller or computer-controller.
    /// </summary>
    public abstract partial class Character : CharacterEntity, IGetTime, IRespawnable, ICharacterTable, IUpdateableMapReference, IServerSaveable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// How frequently the SP is recovered in milliseconds.
        /// </summary>
        const int _spRecoveryRate = 3000;

        static readonly AllianceManager _allianceManager = AllianceManager.Instance;
        static readonly CharacterTemplateManager _characterTemplateManager = CharacterTemplateManager.Instance;

        /// <summary>
        /// Random number generator for Characters
        /// </summary>
        static readonly SafeRandom _rand = new SafeRandom();

        static readonly ShopManager _shopManager = ShopManager.Instance;
        static readonly SkillManager _skillManager = SkillManager.Instance;

        readonly StatCollection<StatType> _baseStats;
        readonly CharacterEquipped _equipped;
        readonly CharacterInventory _inventory;
        readonly bool _isPersistent;
        readonly StatCollection<StatType> _modStats;
        readonly CharacterSkillCaster _skillCaster;
        readonly CharacterSPSynchronizer _spSync;
        readonly CharacterStatusEffects _statusEffects;
        readonly World _world;

        int _attackTimeout = 500;

        int _cash;
        int _exp;
        SPValueType _hp;
        CharacterID _id;

        bool _isLoaded = false;
        KnownSkillsCollection _knownSkills;

        short _level;

        /// <summary>
        /// Map the character is currently on.
        /// </summary>
        Map _map;

        ushort _moveSpeed;

        /// <summary>
        /// Contains the cached value of the <see cref="Character.MoveSpeed"/>'s real movement velocity. This value
        /// is automatically updated whenever the <see cref="Character.MoveSpeed"/> property changes.
        /// </summary>
        float _moveSpeedVelocityCache;

        SPValueType _mp;

        /// <summary>
        /// Name of the character.
        /// </summary>
        string _name;

        /// <summary>
        /// The time at which the character will be allowed to attack again. That is, they cannot attack until the game time
        /// surpasses this time.
        /// </summary>
        TickCount _nextAttackTime;

        int _nextLevelExp;

        /// <summary>
        /// Lets us know if we have saved the Character since they have been updated. Used to ensure saves aren't
        /// called back-to-back without any values changing in-between.
        /// </summary>
        bool _saved = false;

        /// <summary>
        /// The time that the SP will next be recovered.
        /// </summary>
        int _spRecoverTime;

        int _statPoints;

        CharacterTemplateID? _templateID;
        bool _updateModStats = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Character"/> class.
        /// </summary>
        /// <param name="world">World that the character belongs to.</param>
        /// <param name="isPersistent">If the Character's state is persistent. If true, Load() MUST be called
        /// at some point during the Character's constructor!</param>
        protected Character(World world, bool isPersistent) : base(Vector2.Zero, Vector2.One)
        {
            IsAlive = false;
            _skillCaster = new CharacterSkillCaster(this);

            _world = world;
            _isPersistent = isPersistent;

            if (IsPersistent)
                _statusEffects = new PersistentCharacterStatusEffects(this);
            else
                _statusEffects = new NonPersistentCharacterStatusEffects(this);

            _statusEffects.Added -= StatusEffects_HandleOnAdd;
            _statusEffects.Added += StatusEffects_HandleOnAdd;
            _statusEffects.Removed -= StatusEffects_HandleOnRemove;
            _statusEffects.Removed += StatusEffects_HandleOnRemove;

            _baseStats = CreateStats(StatCollectionType.Base);
            _modStats = CreateStats(StatCollectionType.Modified);
            _spSync = CreateSPSynchronizer();
            _inventory = CreateInventory();
            _equipped = CreateEquipped();

            // Set up the listeners for when the stat collections change
            BaseStats.StatChanged -= BaseStatChangedHandler;
            BaseStats.StatChanged += BaseStatChangedHandler;
            ModStats.StatChanged -= ModStatChangedHandler;
            ModStats.StatChanged += ModStatChangedHandler;

            // Set up the listeners for when the equipped items change
            _equipped.Equipped -= EquippedHandler;
            _equipped.Equipped += EquippedHandler;
            _equipped.Unequipped -= UnequippedHandler;
            _equipped.Unequipped += UnequippedHandler;
        }

        /// <summary>
        /// Notifies listeners when the Character performs an attack. The attack does not have to actually hit
        /// anything for this event to be raised. This will be raised before <see cref="AttackedCharacter"/>.
        /// </summary>
        public event TypedEventHandler<Character> Attacked;

        /// <summary>
        /// Notifies listeners when this Character has been attacked by another Character.
        /// </summary>
        public event TypedEventHandler<Character, CharacterAttackEventArgs> AttackedByCharacter;

        /// <summary>
        /// Notifies listeners when this Character has successfully attacked another Character.
        /// </summary>
        public event TypedEventHandler<Character, CharacterAttackEventArgs> AttackedCharacter;

        /// <summary>
        /// Notifies listeners when this <see cref="Character"/>'s cash value has changed.
        /// </summary>
        public event TypedEventHandler<Character, ValueChangedEventArgs<int>> CashChanged;

        /// <summary>
        /// Notifies listeners when this Character has dropped an item.
        /// </summary>
        public event TypedEventHandler<Character, EventArgs<ItemEntity>> DroppedItem;

        /// <summary>
        /// Notifies listeners when this <see cref="Character"/>'s exp value has changed.
        /// </summary>
        public event TypedEventHandler<Character, ValueChangedEventArgs<int>> ExpChanged;

        /// <summary>
        /// Notifies listeners when this <see cref="Character"/>'s HP value has changed.
        /// </summary>
        public event TypedEventHandler<Character, ValueChangedEventArgs<SPValueType>> HPChanged;

        /// <summary>
        /// Notifies listeners when this Character has been killed in any way, no matter who did it or how it happened.
        /// </summary>
        public event TypedEventHandler<Character> Killed;

        /// <summary>
        /// Notifies listeners when this Character has been killed by another Character.
        /// </summary>
        public event TypedEventHandler<Character, CharacterKillEventArgs> KilledByCharacter;

        /// <summary>
        /// Notifies listeners when this Character has killed another Character.
        /// </summary>
        public event TypedEventHandler<Character, CharacterKillEventArgs> KilledCharacter;

        /// <summary>
        /// Notifies listeners when this <see cref="Character"/>'s level value has changed.
        /// </summary>
        public event TypedEventHandler<Character, ValueChangedEventArgs<short>> LevelChanged;

        /// <summary>
        /// Notifies listeners when this <see cref="Character"/>'s MP value has changed.
        /// </summary>
        public event TypedEventHandler<Character, ValueChangedEventArgs<SPValueType>> MPChanged;

        /// <summary>
        /// Notifies listeners when this <see cref="Character"/>'s map has changed.
        /// </summary>
        public event TypedEventHandler<Character, ValueChangedEventArgs<Map>> MapChanged;

        /// <summary>
        /// Notifies listeners when this <see cref="Character"/>'s StatPoints value has changed.
        /// </summary>
        public event TypedEventHandler<Character, ValueChangedEventArgs<int>> StatPointsChanged;

        /// <summary>
        /// Notifies listeners when the Character's TemplateID has changed.
        /// </summary>
        public event TypedEventHandler<Character> TemplateIDChanged;

        /// <summary>
        /// Notifies listeners when this Character uses an item.
        /// </summary>
        public event TypedEventHandler<Character, EventArgs<ItemEntity>> UsedItem;

        /// <summary>
        /// When overridden in the derived class, gets the Character's AI. Can be null if they have no AI.
        /// </summary>
        public abstract IAI AI { get; }

        /// <summary>
        /// Gets or sets (protected) the Character's alliance.
        /// </summary>
        public Alliance Alliance { get; protected set; }

        /// <summary>
        /// Gets the <see cref="AllianceManager"/> instance to be used by the <see cref="Character"/>s.
        /// </summary>
        protected static AllianceManager AllianceManager
        {
            get { return _allianceManager; }
        }

        /// <summary>
        /// Gets the amount of time in milliseconds the <see cref="Character"/> must wait between attacks.
        /// </summary>
        public int AttackTimeout
        {
            get { return _attackTimeout; }
        }

        /// <summary>
        /// Gets the <see cref="Character"/>'s base stats collection.
        /// </summary>
        public StatCollection<StatType> BaseStats
        {
            get { return _baseStats; }
        }

        /// <summary>
        /// Gets the <see cref="CharacterTemplateManager"/> instance to be used by the <see cref="Character"/>s.
        /// </summary>
        protected static CharacterTemplateManager CharacterTemplateManager
        {
            get { return _characterTemplateManager; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the chat dialog for this Character.
        /// </summary>
        public abstract NPCChatDialogBase ChatDialog { get; }

        public IDbController DbController
        {
            get { return World.Server.DbController; }
        }

        /// <summary>
        /// Gets the Character's equipped items.
        /// </summary>
        public CharacterEquipped Equipped
        {
            get { return _equipped; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if the CharacterEntity has a chat dialog. Do not use the setter
        /// on this property.
        /// </summary>
        /// <exception cref="NotSupportedException">This value should never be set directly in the Server.</exception>
        public override bool HasChatDialog
        {
            get { return ChatDialog != null; }
            protected set { throw new NotSupportedException("This value should never be set directly in the Server."); }
        }

        /// <summary>
        /// When overridden in the derived class, gets if the CharacterEntity has a shop. Do not use the setter
        /// on this property.
        /// </summary>
        /// <exception cref="NotSupportedException">This value should never be set directly in the Server.</exception>
        public override bool HasShop
        {
            get { return Shop != null; }
            protected set { throw new NotSupportedException("This value should never be set directly in the Server."); }
        }

        /// <summary>
        /// Gets the Character's Inventory.
        /// </summary>
        public CharacterInventory Inventory
        {
            get { return _inventory; }
        }

        /// <summary>
        /// Gets or sets (protected) if the Character is currently alive.
        /// </summary>
        public bool IsAlive { get; protected set; }

        /// <summary>
        /// Gets if the Character has been loaded. If this is false, the Character has either not been loaded or is
        /// currently in the proccess of loading.
        /// </summary>
        public bool IsLoaded
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

        /// <summary>
        /// Gets the collection of skills known by this <see cref="Character"/>.
        /// </summary>
        public KnownSkillsCollection KnownSkills
        {
            get { return _knownSkills; }
        }

        /// <summary>
        /// Gets the Map the Character is currently on. This value can be null if the Character is not currently
        /// on a Map.
        /// </summary>
        public Map Map
        {
            get { return _map; }
        }

        /// <summary>
        /// Gets the <see cref="Character"/>'s mod stats collection.
        /// </summary>
        public StatCollection<StatType> ModStats
        {
            get { return _modStats; }
        }

        /// <summary>
        /// Gets a random number generator to be used for Characters.
        /// </summary>
        protected static Random Rand
        {
            get { return _rand; }
        }

        /// <summary>
        /// Gets or sets the index of the Map the Character will respawn on.
        /// </summary>
        public MapID? RespawnMapID { get; set; }

        /// <summary>
        /// Gets or sets where the Character will respawn in their respawn map. Only valid if the
        /// <see cref="Character.RespawnMapID"/> is set to a non-null value.
        /// </summary>
        public Vector2 RespawnPosition { get; set; }

        /// <summary>
        /// When overridden in the derived class, gets this <see cref="Character"/>'s shop.
        /// </summary>
        public abstract IShop<ShopItem> Shop { get; }

        /// <summary>
        /// Gets the <see cref="ShopManager"/> instance to be used by the <see cref="Character"/>s.
        /// </summary>
        protected static ShopManager ShopManager
        {
            get { return _shopManager; }
        }

        public CharacterStatusEffects StatusEffects
        {
            get { return _statusEffects; }
        }

        /// <summary>
        /// Gets the weapon to use for attacking. Cannot be null.
        /// </summary>
        public ItemEntity Weapon
        {
            get
            {
                var weapon = _equipped[EquipmentSlot.RightHand];

                if (weapon == null)
                    return World.UnarmedWeapon;

                return weapon;
            }
        }

        /// <summary>
        /// Gets the world the Character is in.
        /// </summary>
        public World World
        {
            get { return _world; }
        }

        /// <summary>
        /// When overridden in the derived class, lets the Character handle being given items through GiveItem().
        /// </summary>
        /// <param name="item">The <see cref="ItemEntity"/> the Character was given.</param>
        /// <param name="amount">The amount of the <paramref name="item"/> the Character was given. Will be greater
        /// than 0.</param>
        protected virtual void AfterGiveItem(ItemEntity item, byte amount)
        {
        }

        /// <summary>
        /// Attacks the <paramref name="target"/> using the equipped weapon.
        /// </summary>
        /// <param name="target">The <see cref="Character"/> to attack. If null, the target will be selected
        /// automatically if applicable.</param>
        public void Attack(Character target)
        {
            Attack(target, Weapon);
        }

        /// <summary>
        /// Attacks the <paramref name="target"/> using the given <paramref name="weapon"/>.
        /// </summary>
        /// <param name="target">The <see cref="Character"/> to attack. If null, the target will be selected
        /// automatically if applicable.</param>
        /// <param name="weapon">The weapon to use for attacking. If null, will be treated as an unarmed melee attack.</param>
        public void Attack(Character target, ItemEntity weapon)
        {
            if (!IsAlive)
            {
                const string errmsg = "`{0}` tried to attack `{1}` with weapon `{2}` while dead.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, target, weapon);
                return;
            }

            var currTime = GetTime();

            // Don't allow attacking while casting a skill
            if (_skillCaster.IsCastingSkill)
                return;

            // Check that enough time has elapsed since the last attack
            if (_nextAttackTime > currTime)
                return;

            // If no weapon is specified, use the unarmed weapon
            if (weapon == null)
                weapon = Weapon;

            // Abort if using an unknown weapon type
            if (weapon.WeaponType == WeaponType.Unknown)
            {
                TrySend(GameMessage.CannotAttackWithWeapon, ServerMessageType.GUI, weapon.Name);
                return;
            }

            // Check if a target was given to us
            if (target != null)
            {
                // Check for a valid target
                if (!target.IsAlive)
                {
                    const string errmsg = "`{0}` tried to attack target `{1}`, but the target is not alive.";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, this, target);
                    Debug.Fail(string.Format(errmsg, this, target));
                    return;
                }

                if (target.Map != Map)
                {
                    const string errmsg = "`{0}` tried to attack target `{1}`, but the target is on a different map.";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, this, target);
                    Debug.Fail(string.Format(errmsg, this, target));
                    return;
                }

                if (!Alliance.CanAttack(target.Alliance))
                {
                    TrySend(GameMessage.CannotAttackAllianceConflict, ServerMessageType.GUI, target.Name);
                    return;
                }

                if (this.GetDistance(target) > weapon.Range)
                {
                    TrySend(GameMessage.CannotAttackTooFarAway, ServerMessageType.GUI);
                    return;
                }
            }

            // Call the appropriate attack method
            switch (weapon.WeaponType)
            {
                case WeaponType.Melee:
                    AttackMelee(weapon, target);
                    break;

                case WeaponType.Projectile:
                case WeaponType.Ranged:
                    AttackRanged(weapon, target);
                    break;

                default:
                    const string errmsg = "No attack support defined for WeaponType `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, weapon.WeaponType);
                    Debug.Fail(string.Format(errmsg, weapon.WeaponType));
                    break;
            }

            // Update the last attack time to now
            _nextAttackTime = (TickCount)(currTime + _attackTimeout);
        }

        /// <summary>
        /// Performs the actual attacking of a specific character. This should only be called by
        /// <see cref="AttackMelee"/> or <see cref="AttackRanged"/>.
        /// </summary>
        /// <param name="target">Character to attack.</param>
        void AttackApplyReal(Character target)
        {
            // Get the damage
            var damage = GetAttackDamage(target);

            // Apply the damage to the target
            target.Damage(this, damage);

            // Raise attack events
            OnAttackedCharacter(target, damage);
            if (AttackedCharacter != null)
                AttackedCharacter.Raise(this, new CharacterAttackEventArgs(target, damage));

            target.OnAttackedByCharacter(this, damage);
            if (target.AttackedByCharacter != null)
                target.AttackedByCharacter.Raise(this, new CharacterAttackEventArgs(target, damage));
        }

        /// <summary>
        /// Handles attacking with a melee weapon.
        /// </summary>
        /// <param name="weapon">The weapon to attack with. Cannot be null.</param>
        /// <param name="target">The target to attack. Can be null.</param>
        void AttackMelee(IItemTable weapon, Character target)
        {
            if (weapon == null)
            {
                Debug.Fail("Weapon should not be null...");
                return;
            }

            OnAttacked();
            if (Attacked != null)
                Attacked.Raise(this, EventArgs.Empty);

            // If melee and no target was defined, try to find one automatically
            if (target == null)
            {
                var hitArea = GameData.GetMeleeAttackArea(this, weapon.Range);
                target = Map.Spatial.Get<Character>(hitArea, x => x != this && x.IsAlive && Alliance.CanAttack(x.Alliance));
            }

            // Display the attack
            var targetID = (target != null ? target.MapEntityIndex : (MapEntityIndex?)null);
            using (var charAttack = ServerPacket.CharAttack(MapEntityIndex, targetID, weapon.ActionDisplayID))
            {
                Map.SendToArea(this, charAttack, ServerMessageType.MapEffect);
            }

            // Check that we managed to find a target
            if (target == null)
                return;

            // We found a target, so attack it
            AttackApplyReal(target);
        }

        /// <summary>
        /// Handles attacking with a ranged weapon.
        /// </summary>
        /// <param name="weapon">The weapon to attack with. Cannot be null.</param>
        /// <param name="target">The target to attack. Can be null.</param>
        void AttackRanged(ItemEntity weapon, Character target)
        {
            if (weapon == null)
            {
                Debug.Fail("Weapon should not be null...");
                return;
            }

            // We can't do anything with ranged attacks if no target is given
            if (target == null)
            {
                TrySend(GameMessage.CannotAttackNeedTarget, ServerMessageType.GUI);
                return;
            }

            Ray2D ray = new Ray2D(this, Position, target.Position, Map.Spatial);

            Vector2 rayCollideWall;

            // FUTURE: Use to create some sort of wasted ammo on a wall or something.  e.g. Grenade item explodes on walls.
            bool hasHitWall = ray.Intersects<WallEntity>(out rayCollideWall);

            if (hasHitWall)
            {
                TrySend(GameMessage.CannotAttackNotInSight, ServerMessageType.GUI);
                return;
            }

            List<ISpatial> rayCollideCharacters;

            // Use IntersectsMany here if you want to damage all characters in the attack path
            bool hasHitCharacter = ray.Intersects<Character>(out rayCollideCharacters);

            if (hasHitCharacter)
            {
                var ammoUsed = false;

                // Check for the needed ammo
                switch (weapon.WeaponType)
                {
                    case WeaponType.Projectile:
                        // Grab projectile ammo out of the inventory first if possible to avoid having to constantly reload
                        var invAmmo = Inventory.FirstOrDefault(x => weapon.CanStack(x.Value));
                        if (invAmmo.Value != null)
                            Inventory.DecreaseItemAmount(invAmmo.Key);
                        else
                            weapon.Destroy();

                        ammoUsed = true;
                        break;

                    case WeaponType.Ranged:
                        // By default, guns won't use ammo. But if you want to require guns to use ammo, you can do so here
                        ammoUsed = true;
                        break;
                }

                if (!ammoUsed)
                    return;

                foreach (var character in rayCollideCharacters)
                {
                    var c = character as Character;

                    if (!Alliance.CanAttack(c.Alliance))
                        continue;

                    // Attack
                    using (var charAttack = ServerPacket.CharAttack(MapEntityIndex, c.MapEntityIndex, weapon.ActionDisplayID))
                    {
                        Map.SendToArea(this, charAttack, ServerMessageType.MapEffect);
                    }

                    OnAttacked();

                    if (Attacked != null)
                        Attacked.Raise(this, EventArgs.Empty);

                    AttackApplyReal(c);
                }
            }
        }

        /// <summary>
        /// Handles when the <see cref="Character"/>'s base stats change.
        /// </summary>
        /// <param name="sender">The <see cref="IStatCollection{TStatType}"/> the event came from.</param>
        /// <param name="e">The <see cref="StatCollectionStatChangedEventArgs{StatType}"/> instance containing the event data.</param>
        void BaseStatChangedHandler(IStatCollection<StatType> sender, StatCollectionStatChangedEventArgs<StatType> e)
        {
            _updateModStats = true;

            OnBaseStatChanged(e.StatType, e.OldValue, e.NewValue);
        }

        /// <summary>
        /// When overridden in the derived class, checks if enough time has elapesd since the Character died
        /// for them to be able to respawn.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        /// <returns>True if enough time has elapsed; otherwise false.</returns>
        protected abstract bool CheckRespawnElapsedTime(TickCount currentTime);

        /// <summary>
        /// When overridden in the derived class, creates the CharacterEquipped for this Character.
        /// </summary>
        /// <returns>The CharacterEquipped for this Character.</returns>
        protected abstract CharacterEquipped CreateEquipped();

        /// <summary>
        /// When overridden in the derived class, creates the CharacterInventory for this Character.
        /// </summary>
        /// <returns>The CharacterInventory for this Character.</returns>
        protected abstract CharacterInventory CreateInventory();

        /// <summary>
        /// When overridden in the derived class, creates the CharacterSPSynchronizer for this Character.
        /// </summary>
        /// <returns>The CharacterSPSynchronizer for this Character.</returns>
        protected virtual CharacterSPSynchronizer CreateSPSynchronizer()
        {
            return new CharacterSPSynchronizer(this);
        }

        /// <summary>
        /// When overridden in the derived class, creates the CharacterStatsBase for this Character.
        /// </summary>
        /// <param name="statCollectionType">The type of <see cref="StatCollectionType"/> to create.</param>
        /// <returns>The CharacterStatsBase for this Character.</returns>
        protected abstract StatCollection<StatType> CreateStats(StatCollectionType statCollectionType);

        /// <summary>
        /// Applies damage to the Character.
        /// </summary>
        /// <param name="source">Entity the damage came from. Can be null.</param>
        /// <param name="damage">Amount of damage to apply to the Character. Does not include damage reduction
        /// from defense or any other kind of damage alterations since these are calculated here.</param>
        public virtual void Damage(Entity source, int damage)
        {
            if (!IsAlive)
            {
                const string errmsg = "`{0}` tried to damage dead character `{1}` (for {2} damage).";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, source, this, damage);
                return;
            }

            // Apply damage
            using (var pw = ServerPacket.CharDamage(MapEntityIndex, damage))
            {
                Map.SendToArea(this, pw, ServerMessageType.MapEffect);
            }

            var newHP = HP - damage;
            if (newHP < 0)
                newHP = 0;

            HP = newHP;

            // Check if the character died
            if (newHP <= 0)
            {
                if (source != null)
                {
                    var sourceCharacter = source as Character;
                    if (sourceCharacter != null)
                    {
                        sourceCharacter.OnKilledCharacter(this);
                        if (sourceCharacter.KilledCharacter != null)
                            sourceCharacter.KilledCharacter.Raise(this, new CharacterKillEventArgs(sourceCharacter));

                        OnKilledByCharacter(sourceCharacter);
                        if (KilledByCharacter != null)
                            KilledByCharacter.Raise(this, new CharacterKillEventArgs(sourceCharacter));
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
            World.DelayedDispose(this);
        }

        /// <summary>
        /// Makes the Character drop an existing item. This does NOT remove the ItemEntity from the Character in any
        /// way. Be sure to remove the ItemEntity from the Character first if needed.
        /// </summary>
        /// <param name="item">ItemEntity to drop.</param>
        public void DropItem(ItemEntity item)
        {
            if (item == null)
            {
                const string errmsg = "`{0}` tried to drop a null item.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return;
            }

            if (item.IsDisposed)
            {
                const string errmsg = "`{0}` tried to drop disposed item `{1}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, item);
                return;
            }

            if (!IsAlive)
            {
                const string errmsg = "`{0}` tried to drop item `{1}` while dead.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this, item);
                return;
            }

            var dropPos = GetDropPos();
            item.Position = dropPos;

            // Add the item to the map
            Map.AddEntity(item);

            OnDroppedItem(item);

            if (DroppedItem != null)
                DroppedItem.Raise(this, EventArgsHelper.Create(item));
        }

        /// <summary>
        /// Makes the Character drop an item. Does not modify the item requested to drop at all or anything,
        /// so if you want to also remove the item, such as with dropping an item from the Inventory,
        /// this will not take care of that.
        /// </summary>
        /// <param name="itemTemplate">ItemTemplate for the item to drop.</param>
        /// <param name="amount">Amount of the item to drop.</param>
        protected void DropItem(IItemTemplateTable itemTemplate, byte amount)
        {
            if (itemTemplate == null)
            {
                const string errmsg = "`{0}` tried to drop null item template.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return;
            }

            var dropPos = GetDropPos();

            // Create the item on the map
            var droppedItem = Map.CreateItem(itemTemplate, dropPos, amount);

            OnDroppedItem(droppedItem);

            if (DroppedItem != null)
                DroppedItem.Raise(this, EventArgsHelper.Create(droppedItem));
        }

        /// <summary>
        /// Makes the <see cref="Character"/> use an <see cref="Emoticon"/>.
        /// </summary>
        /// <param name="emoticon">The emoticon to use.</param>
        public void Emote(Emoticon emoticon)
        {
            using (var pw = ServerPacket.Emote(MapEntityIndex, emoticon))
            {
                Map.SendToArea(this, pw, ServerMessageType.MapEffect);
            }
        }

        /// <summary>
        /// Attempts to equip an item from the User's inventory.
        /// </summary>
        /// <param name="inventorySlot">Index of the slot containing the item to equip.</param>
        /// <returns>True if the item was successfully equipped, else false.</returns>
        public bool Equip(InventorySlot inventorySlot)
        {
            if (!IsAlive)
            {
                const string errmsg = "`{0}` tried to equip inventory slot `{1}` but they are dead.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this, inventorySlot);
                return false;
            }

            // Get the item from the inventory
            var item = Inventory[inventorySlot];

            // Do not try to equip null items
            if (item == null)
                return false;

            // If there is more than one of the item, split it first
            if (item.Amount > 1)
            {
                // Split the existing item into two parts
                item = item.Split(1);
                if (item == null)
                {
                    Debug.Fail("Failed to split item for some reason.");
                    return false;
                }
            }
            else
            {
                // We are using all (1) of the item, so remove it from the inventory
                Inventory.RemoveAt(inventorySlot, false);
            }

            // Try to equip the item
            var successful = Equipped.Equip(item);

            // If failed to equip, give the item back to the User
            if (!successful)
            {
                var remainder = TryGiveItem(item);
                if (remainder != null)
                {
                    Debug.Fail("What the hell just happened? Failed to equip the item, and failed to add back to inventory?");
                    DropItem(remainder);
                }
            }

            return successful;
        }

        /// <summary>
        /// Handles when the <see cref="Character"/> equips an item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EquippedEventArgs{ItemEntity}"/> instance containing the event data.</param>
        void EquippedHandler(EquippedBase<ItemEntity> sender, EquippedEventArgs<ItemEntity> e)
        {
            _updateModStats = true;

            OnEquipped(e.Item, e.Slot);
        }

        /// <summary>
        /// Gets the amount of damage for a normal attack.
        /// </summary>
        /// <param name="target">Character being attacked.</param>
        /// <returns>The amount of damage to inflict for a normal attack.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target" /> is <c>null</c>.</exception>
        public int GetAttackDamage(Character target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            int minHit = ModStats[StatType.MinHit];
            int maxHit = ModStats[StatType.MaxHit];

            if (minHit > maxHit)
                maxHit = minHit;

            var damage = Rand.Next(minHit, maxHit);

            // Apply the defence, and ensure the damage is in a valid range
            int defence = target.ModStats[StatType.Defence];

            damage -= defence / 2;

            if (damage < 1)
                damage = 1;

            return damage;
        }

        Vector2 GetDropPos()
        {
            const int _dropRange = 32;

            // Get the center point of the Character
            var dropPos = Position + (Size / 2);

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
        /// When overridden in the derived class, gets the <see cref="MapID"/> that this <see cref="Character"/>
        /// will use for when loading.
        /// </summary>
        /// <returns>The ID of the map to load this <see cref="Character"/> on.</returns>
        protected abstract MapID GetLoadMap();

        /// <summary>
        /// When overridden in the derived class, gets the position that this <see cref="Character"/>
        /// will use for when loading.
        /// </summary>
        /// <returns>The position to load this <see cref="Character"/> at.</returns>
        protected abstract Vector2 GetLoadPosition();

        /// <summary>
        /// Gives an item to the Character to be placed in their Inventory.
        /// </summary>
        /// <param name="item">Item to give to the character.</param>
        /// <returns>The amount of the <paramref name="item"/> that was successfully given to the <see cref="Character"/>.</returns>
        public virtual int GiveItem(ItemEntity item)
        {
            if (item == null)
            {
                Debug.Fail("Item is null.");
                return 0;
            }

            Debug.Assert(item.Amount != 0, "Invalid item amount.");

            var amountAdded = _inventory.Add(item);
            Debug.Assert(amountAdded >= byte.MinValue && amountAdded <= byte.MaxValue);

            if (amountAdded > 0)
                AfterGiveItem(item, (byte)amountAdded);

            return amountAdded;
        }

        protected virtual void GiveKillReward(int exp, int cash)
        {
            Exp += exp;
            Cash += cash;
        }

        /// <summary>
        /// When overridden in the derived class, handles additional loading stuff.
        /// </summary>
        /// <param name="v">The ICharacterTable containing the database values for this Character.</param>
        protected virtual void HandleAdditionalLoading(ICharacterTable v)
        {
        }

        /// <summary>
        /// Performs the actual disposing of the Entity. This is called by the base Entity class when
        /// a request has been made to dispose of the Entity. This is guarenteed to only be called once.
        /// All classes that override this method should be sure to call base.DisposeHandler() after
        /// handling what it needs to dispose.
        /// </summary>
        /// <param name="disposeManaged">When true, <see cref="IDisposable.Dispose"/> was explicitly called and managed resources need to be
        /// disposed. When false, managed resources do not need to be disposed since this object was garbage-collected.</param>
        protected override void HandleDispose(bool disposeManaged)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Disposing character `{0}`.", this);

            // Make sure the Character was saved
            Save();

            // Dispose of disposable stuff
            if (Equipped != null)
                Equipped.Dispose();

            if (Inventory != null)
                Inventory.Dispose();

            if (StatusEffects != null)
                StatusEffects.Dispose();

            base.HandleDispose(disposeManaged);
        }

        /// <summary>
        /// Handles when no legal position could be found for this <see cref="Character"/>.
        /// This will usually occur when performing a teleport into an area that is completely blocked off, and no near-by
        /// position can be found. Moving a <see cref="Character"/> too far from the original position can result in them
        /// going somewhere that they are not supposed to, so it is best to send them to a predefined location.
        /// </summary>
        /// <param name="position">The position that the <see cref="Character"/> tried to go to, but failed to.</param>
        /// <returns>The position to warp the <see cref="Character"/> to.</returns>
        protected virtual Vector2 HandleNoLegalPositionFound(Vector2 position)
        {
            if (IsPersistent)
            {
                // Persistent Characters get sent to their loading position
                const string errmsg = "Character `{0}` is persistent, so they are being set back to their respawn position.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this);

                var mapID = GetLoadMap();
                var map = World.GetMap(mapID);
                var pos = GetLoadPosition();

                Teleport(map, pos);
                return pos;
            }
            else
            {
                // Non-persistent characters are destroyed
                const string errmsg = "Character `{0}` is not persistent, so they are being disposed.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this);

                DelayedDispose();
                return position;
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional steps to be taken when saving.
        /// </summary>
        protected virtual void HandleSave()
        {
        }

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected override void HandleUpdate(IMap imap, int deltaTime)
        {
            // Update shouldn't be called on disposed Characters since they shouldn't be referenced
            // by the Map anymore
            if (IsDisposed)
            {
                const string errmsg = "Update called on disposed Character `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, Name);
                return;
            }

            // Set the Character as not saved
            _saved = false;

            if (_updateModStats)
                UpdateModStats();

            UpdateSPRecovery();
            StatusEffects.Update();
            _skillCaster.Update();

            base.HandleUpdate(imap, deltaTime);

            _spSync.Synchronize();
        }

        /// <summary>
        /// When overridden in the derived class, implements the Character being killed. This 
        /// doesn't actually care how the Character was killed, it just takes the appropriate
        /// actions to kill them.
        /// </summary>
        public virtual void Kill()
        {
            if (!IsAlive)
            {
                const string errmsg = "Tried to kill `{0}` but they are already dead.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                return;
            }

            // Ensure movement has stopped
            StopMoving();

            // Raise the kill events
            OnKilled();

            if (Killed != null)
                Killed.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Makes the Character's level increase. Does not alter the experience in any way since it is assume that,
        /// when this is called, the Character already has enough experience for the next level.
        /// </summary>
        protected virtual void LevelUp()
        {
            Level++;
            StatPoints += 5;
        }

        protected void Load(CharacterID characterID)
        {
            var values = DbController.GetQuery<SelectCharacterByIDQuery>().Execute(characterID);
            LoadFromQueryValues(values);
        }

        protected void Load(CharacterTemplate template)
        {
            var v = template.TemplateTable;

            Name = v.Name;
            Alliance = _allianceManager[v.AllianceID];
            BodyInfo = BodyInfoManager.Instance.GetBody(v.BodyID);
            CharacterTemplateID = v.ID;
            Resize(BodyInfo.Size);

            _level = v.Level;
            MoveSpeed = v.MoveSpeed;
            _exp = v.Exp;
            _statPoints = v.StatPoints;

            // Create the AI
            if (v.AIID.HasValue)
            {
                if (!SetAI(v.AIID.Value))
                {
                    const string errmsg = "Failed to set Character `{0}`'s AI to ID `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, v.AIID.Value);
                    Debug.Fail(string.Format(errmsg, this, v.AIID.Value));
                    RemoveAI();
                }
            }

            // Set the base stats
            BaseStats.CopyValuesFrom(v.Stats);

            // Set known skills
            if (_knownSkills == null)
                _knownSkills = new KnownSkillsCollection(this);

            KnownSkills.SetValues(template.KnownSkills);
        }

        void LoadFromQueryValues(ICharacterTable v)
        {
            Name = v.Name;
            _id = v.ID;
            _templateID = v.CharacterTemplateID;
            MoveSpeed = v.MoveSpeed;

            BodyInfo = BodyInfoManager.Instance.GetBody(v.BodyID);

            Teleport(new Vector2(v.LoadX, v.LoadY));
            Resize(BodyInfo.Size);

            ((PersistentCharacterStatusEffects)StatusEffects).Load();

            // Set the character information
            _level = v.Level;
            _exp = v.Exp;
            _cash = v.Cash;
            _hp = v.HP;
            _mp = v.MP;
            RespawnMapID = v.RespawnMapID;
            RespawnPosition = new Vector2(v.RespawnX, v.RespawnY);
            StatPoints = v.StatPoints;

            _knownSkills = new KnownSkillsCollection(this);

            // Create the AI
            if (v.AIID.HasValue)
            {
                if (!SetAI(v.AIID.Value))
                {
                    const string errmsg = "Failed to set Character `{0}`'s AI to ID `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, v.AIID.Value);
                    Debug.Fail(string.Format(errmsg, this, v.AIID.Value));
                    RemoveAI();
                }
            }

            // Load the stats
            BaseStats.CopyValuesFrom(v.Stats);

            // Load the Character's items
            Inventory.Load();
            Equipped.Load();

            // Update the mod stats
            UpdateModStats();

            // Additional loading
            HandleAdditionalLoading(v);

            // Set the map
            var m = World.GetMap(v.LoadMapID);
            if (m != null)
                Teleport(m, Position);
            else
                ((IRespawnable)this).Respawn();

            // Mark the Character as loaded
            SetAsLoaded();

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded Character `{0}`.", Name);
        }

        /// <summary>
        /// Handles when the <see cref="Character"/>'s mod stats change.
        /// </summary>
        /// <param name="sender">The <see cref="IStatCollection{TStatType}"/> the event came from.</param>
        /// <param name="e">The <see cref="StatCollectionStatChangedEventArgs{StatType}"/> instance containing the event data.</param>
        void ModStatChangedHandler(IStatCollection<StatType> sender, StatCollectionStatChangedEventArgs<StatType> e)
        {
            _updateModStats = true;

            // Ensure the HP and MP are valid
            switch (e.StatType)
            {
                case StatType.MaxMP:
                    if (MP > e.NewValue)
                        MP = (int)e.NewValue;
                    break;

                case StatType.MaxHP:
                    if (HP > e.NewValue)
                        HP = (int)e.NewValue;
                    break;
            }

            // Update the attack rate
            if (e.StatType == StatType.Agi)
                _attackTimeout = Math.Max(GameData.AttackTimeoutMin, GameData.AttackTimeoutDefault - ModStats[StatType.Agi] * 2);

            OnModStatChanged(e.StatType, e.OldValue, e.NewValue);
        }

        /// <summary>
        /// Starts moving the character to the left.
        /// </summary>
        public void MoveLeft()
        {
            if (!IsAlive)
                return;

            if (IsMovingLeft)
                return;

            if (_skillCaster.IsCastingSkill)
                return;

            SetHeading(Direction.West);

            SetVelocity(new Vector2(-_moveSpeedVelocityCache, Velocity.Y));
        }

        /// <summary>
        /// Starts moving the character to the right.
        /// </summary>
        public void MoveRight()
        {
            if (!IsAlive)
                return;

            if (IsMovingRight)
                return;

            if (_skillCaster.IsCastingSkill)
                return;

            SetHeading(Direction.East);

            SetVelocity(new Vector2(_moveSpeedVelocityCache, Velocity.Y));
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.Attacked"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        protected virtual void OnAttacked()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.AttackedByCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="attacker">The <see cref="Character"/> that attacked us.</param>
        /// <param name="damage">The amount of damage inflicted on this <see cref="Character"/>.</param>
        protected virtual void OnAttackedByCharacter(Character attacker, int damage)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.AttackedCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="attacked">The <see cref="Character"/> that was attacked.</param>
        /// <param name="damage">The amount of damage inflicted on the <paramref name="attacked"/> by
        /// the this <see cref="Character"/>.</param>
        protected virtual void OnAttackedCharacter(Character attacked, int damage)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when a base stat
        /// changes. Use this overload instead of adding an event listener to the base stat collection when possible.
        /// </summary>
        /// <param name="statType">The type of the stat that changed.</param>
        /// <param name="oldValue">The old value of the stat.</param>
        /// <param name="newValue">The new value of the stat.</param>
        protected virtual void OnBaseStatChanged(StatType statType, StatValueType oldValue, StatValueType newValue)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.CashChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldCash">The old cash.</param>
        /// <param name="cash">The cash.</param>
        protected virtual void OnCashChanged(int oldCash, int cash)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.DroppedItem"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="item">The item that was dropped.</param>
        protected virtual void OnDroppedItem(ItemEntity item)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of
        /// when the <see cref="Character"/> equips an item.
        /// Use this overload instead of adding an event listener to the corresponding event when possible.
        /// </summary>
        /// <param name="item">The <see cref="ItemEntity"/> that was equipped.</param>
        /// <param name="slot">The slot the <paramref name="item"/> was equipped in.</param>
        protected virtual void OnEquipped(ItemEntity item, EquipmentSlot slot)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.ExpChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldExp">The old exp.</param>
        /// <param name="exp">The exp.</param>
        protected virtual void OnExpChanged(int oldExp, int exp)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.HPChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnHPChanged(SPValueType oldValue, SPValueType newValue)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.Killed"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        protected virtual void OnKilled()
        {
            // Increment EventCounters
            foreach (var item in Inventory.Items)
            {
                var template = item.ItemTemplateID;
                if (template.HasValue)
                    EventCounterManager.ItemTemplate.Increment(template.Value, ItemTemplateEventCounterType.CharacterDeaths);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.KilledByCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="killer">The <see cref="Character"/> that killed this <see cref="Character"/>.</param>
        protected virtual void OnKilledByCharacter(Character killer)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.KilledCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="killed">The <see cref="Character"/> that this <see cref="Character"/> killed.</param>
        protected virtual void OnKilledCharacter(Character killed)
        {
            // Increment EventCounter
            var w = Weapon;
            if (w != null)
            {
                var template = w.ItemTemplateID;
                if (template.HasValue)
                    EventCounterManager.ItemTemplate.Increment(template.Value, ItemTemplateEventCounterType.CharactersKilled);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.LevelChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnLevelChanged(short oldValue, short newValue)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.MPChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnMPChanged(SPValueType oldValue, SPValueType newValue)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.MapChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldMap">The old map.</param>
        /// <param name="newMap">The new map.</param>
        protected virtual void OnMapChanged(Map oldMap, Map newMap)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when a mod stat
        /// changes. Use this overload instead of adding an event listener to the mod stat collection when possible.
        /// </summary>
        /// <param name="statType">The type of the stat that changed.</param>
        /// <param name="oldValue">The old value of the stat.</param>
        /// <param name="newValue">The new value of the stat.</param>
        protected virtual void OnModStatChanged(StatType statType, StatValueType oldValue, StatValueType newValue)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.StatPointsChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnStatPointsChanged(int oldValue, int newValue)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.TemplateIDChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        protected virtual void OnTemplateIDChanged()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of
        /// when the <see cref="Character"/> unequips an item.
        /// Use this overload instead of adding an event listener to the corresponding event when possible.
        /// </summary>
        /// <param name="item">The <see cref="ItemEntity"/> that was unequipped.</param>
        /// <param name="slot">The slot the <paramref name="item"/> was unequipped in.</param>
        protected virtual void OnUnequipped(ItemEntity item, EquipmentSlot slot)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.KilledByCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="item">The item that was used.</param>
        protected virtual void OnUsedItem(ItemEntity item)
        {
            if (item.Type == ItemType.UseOnce && item.ItemTemplateID.HasValue)
            {
                WorldStatsTracker.Instance.AddCountConsumeItem((int)item.ItemTemplateID.Value);
                EventCounterManager.ItemTemplate.Increment(item.ItemTemplateID.Value, ItemTemplateEventCounterType.Consume);
            }
        }

        /// <summary>
        /// Makes the Character raise their base Stat of the corresponding type by one point, assuming they have enough
        /// points available to raise the Stat, and lowers the amount of spendable points accordingly.
        /// </summary>
        /// <param name="st">StatType of the stat to raise.</param>
        public void RaiseStat(StatType st)
        {
            if (!IsAlive)
                return;

            var cost = GameData.StatCost(BaseStats[st]);

            if (StatPoints < cost)
            {
                const string errmsg = "Character `{0}` tried to raise stat `{1}`, but only has `{2}` of `{3}` needed points.";
                Debug.Fail(string.Format(errmsg, this, st, StatPoints, cost));
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, st, StatPoints, cost);
                return;
            }

            BaseStats[st]++;
            StatPoints -= cost;
        }

        /// <summary>
        /// Removes the Character's AI.
        /// </summary>
        public abstract void RemoveAI();

        /// <summary>
        /// Saves the persistent Character's information. If IsPersistent is false, this will do nothing.
        /// </summary>
        public void Save()
        {
            if (!IsPersistent)
                return;

            // Do not save if the character is already saved
            if (_saved)
                return;

            // Set the character as saved
            _saved = true;

            // Execute the user save query
            DbController.GetQuery<UpdateCharacterQuery>().Execute(this);

            HandleSave();

            if (log.IsInfoEnabled)
                log.InfoFormat("Saved Character `{0}`.", this);
        }

        /// <summary>
        /// Attempts to set the Character's AI.
        /// </summary>
        /// <param name="aiID">The ID of the new AI to use.</param>
        /// <returns>True if the AI was successfully set; otherwise false.</returns>
        public abstract bool SetAI(AIID aiID);

        /// <summary>
        /// Attempts to set the Character's AI.
        /// </summary>
        /// <param name="aiName">The name of the new AI to use.</param>
        /// <returns>True if the AI was successfully set; otherwise false.</returns>
        public abstract bool SetAI(string aiName);

        /// <summary>
        /// Sets the Character to being loaded. Must be called after the Character has been loaded.
        /// </summary>
        protected void SetAsLoaded()
        {
            Debug.Assert(!IsLoaded, "SetAsLoaded() has already been called on this Character.");
            _isLoaded = true;

            _nextLevelExp = GameData.LevelCost(_level);
        }

        /// <summary>
        /// Handles when an ActiveStatusEffect is added to this Character's StatusEffects.
        /// </summary>
        /// <param name="characterStatusEffects">The CharacterStatusEffects the event took place on.</param>
        /// <param name="activeStatusEffect">The ActiveStatusEffect that was added.</param>
        protected virtual void StatusEffects_HandleOnAdd(CharacterStatusEffects characterStatusEffects,
                                                         EventArgs<ActiveStatusEffect> activeStatusEffect)
        {
            _updateModStats = true;
        }

        /// <summary>
        /// Handles when an ActiveStatusEffect is removed from this Character's StatusEffects.
        /// </summary>
        /// <param name="characterStatusEffects">The CharacterStatusEffects the event took place on.</param>
        /// <param name="activeStatusEffect">The ActiveStatusEffect that was removed.</param>
        protected virtual void StatusEffects_HandleOnRemove(CharacterStatusEffects characterStatusEffects,
                                                            EventArgs<ActiveStatusEffect> activeStatusEffect)
        {
            _updateModStats = true;
        }

        /// <summary>
        /// Stop the character's controllable movement. Any forces acting upon the character, such as gravity, will
        /// not be affected.
        /// </summary>
        public override void StopMoving()
        {
            if (!IsMoving)
                return;

            base.StopMoving();
        }

        /// <summary>
        /// Synchronizes this Character's paperdoll information to the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The <see cref="User"/> to synchronize to.</param>
        public void SynchronizePaperdollTo(User user)
        {
            Equipped.SynchronizePaperdollTo(user);
        }

        /// <summary>
        /// Synchronizes this Character's stat points (SP) to the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The <see cref="User"/> to synchronize to.</param>
        public void SynchronizeSPTo(User user)
        {
            _spSync.ForceSynchronizeTo(user);
        }

        /// <summary>
        /// Teleports the character to a new position and informs clients in the area of
        /// interest that the character has teleported.
        /// </summary>
        /// <param name="newMap">The new map to teleport to.</param>
        /// <param name="position">Position to teleport to.</param>
        public void Teleport(MapBase newMap, Vector2 position)
        {
            if (newMap != Map)
            {
                // Take the character off the old map, teleport, then put them on the new map
                if (Map != null)
                    Map.RemoveEntity(this);

                _map = null;

                Teleport(position);

                if (newMap != null)
                {
                    newMap.AddEntity(this);
                    _spSync.ForceSynchronize();
                }
            }
            else
            {
                // Just teleport since the map didn't change
                Teleport(position);
            }

            Debug.Assert(Map == newMap);
        }

        /// <summary>
        /// Teleports the character to a new position and informs clients in the area of
        /// interest that the character has teleported.
        /// </summary>
        /// <param name="position">Position to teleport to.</param>
        protected override void Teleport(Vector2 position)
        {
            // Make sure the position we teleport to is valid
            var validPosition = ValidatePosition(position);

            base.Teleport(validPosition);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} [{1}; {2}]", Name, ID, GetType().Name);
        }

        /// <summary>
        /// Gives an item to the Character to be placed in their Inventory.
        /// </summary>
        /// <param name="item">Item to give to the character.</param>
        /// <returns>The remainder of the item that failed to be added to the inventory, or null if all of the
        /// item was added.</returns>
        public virtual ItemEntity TryGiveItem(ItemEntity item)
        {
            if (item == null)
            {
                Debug.Fail("Item is null.");
                return null;
            }

            Debug.Assert(item.Amount != 0, "Invalid item amount.");

            // Add as much of the item to the inventory as we can
            int startAmount = item.Amount;
            ItemEntity remainder = _inventory.TryAdd(item);

            // Check how much was added
            int amountAdded = (startAmount - (remainder != null ? (int)remainder.Amount : 0));
            Debug.Assert(amountAdded >= 0 && amountAdded <= byte.MaxValue);

            amountAdded = amountAdded.Clamp(byte.MinValue, byte.MaxValue);

            if (amountAdded > 0)
                AfterGiveItem(item, (byte)amountAdded);

            // Return the remainder
            return remainder;
        }

        /// <summary>
        /// Tries to send data to the <see cref="Character"/> if they implement <see cref="INetworkSender"/>.
        /// </summary>
        /// <param name="gameMessage">The game message.</param>
        /// <param name="messageType">The type of message.</param>
        void TrySend(GameMessage gameMessage, ServerMessageType messageType)
        {
            var comm = this as INetworkSender;
            if (comm != null)
                comm.Send(gameMessage, messageType);
        }

        /// <summary>
        /// Tries to send data to the <see cref="Character"/> if they implement <see cref="INetworkSender"/>.
        /// </summary>
        /// <param name="gameMessage">The game message.</param>
        /// <param name="messageType">The type of message.</param>
        /// <param name="parameters">The message parameters.</param>
        void TrySend(GameMessage gameMessage, ServerMessageType messageType, params object[] parameters)
        {
            var comm = this as INetworkSender;
            if (comm != null)
                comm.Send(gameMessage, messageType, parameters);
        }

        /// <summary>
        /// Handles when the <see cref="Character"/> unequips an item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EquippedEventArgs{ItemEntity}"/> instance containing the event data.</param>
        void UnequippedHandler(EquippedBase<ItemEntity> sender, EquippedEventArgs<ItemEntity> e)
        {
            _updateModStats = true;

            OnUnequipped(e.Item, e.Slot);
        }

        /// <summary>
        /// Updates the character's mod stats.
        /// </summary>
        protected void UpdateModStats()
        {
            _updateModStats = false;

            // Update all the mod stats
            for (var i = 0; i <= EnumHelper<StatType>.MaxValue; i++)
            {
                var statType = (StatType)i;
                ModStats[statType] = ModStatHelper.Calculate(BaseStats, statType, Equipped, StatusEffects);
            }

            if (log.IsDebugEnabled)
                log.DebugFormat("Updated mod stats for character `{0}`.", this);
        }

        /// <summary>
        /// Updates the Character's status points recovery.
        /// </summary>
        void UpdateSPRecovery()
        {
            var time = GetTime();

            // Check that enough time has elapsed
            if (_spRecoverTime > time)
                return;

            // Set the new recovery time
            _spRecoverTime = (int)time + _spRecoveryRate;

            // Recover
            HP += 1 + ModStats[StatType.Str] / 2;
            MP += 1 + ModStats[StatType.Int] / 2;
        }

        /// <summary>
        /// Makes the Character use an equipment item.
        /// </summary>
        /// <param name="item">Item to be equipped.</param>
        /// <param name="inventorySlot">If the item is from the inventory, the inventory slot that the item is in.</param>
        /// <returns>True if the item was successfully equipped; otherwise false.</returns>
        bool UseEquipment(ItemEntity item, InventorySlot? inventorySlot)
        {
            if (!inventorySlot.HasValue)
            {
                // Equip an item not from the inventory
                return Equipped.Equip(item);
            }
            else
            {
                // Equip an item from the inventory
                return Equip(inventorySlot.Value);
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
            if (!IsAlive)
            {
                const string errmsg = "`{0}` tried to use item `{1}` while dead.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, item);
                return false;
            }

            // Check for a valid amount
            if (item.Amount <= 0)
            {
                const string errmsg = "`{0}` attempted to use item `{1}`, but the amount was invalid.";
                Debug.Fail(string.Format(errmsg, this, item));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, item);
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
                    const string errmsg =
                        "`{0}` attempted to use item `{1}`, but it contains invalid or unhandled ItemType `{2}`.";
                    Debug.Fail(string.Format(errmsg, this, item, item.Type));
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, item, item.Type);

                    wasUsed = false;
                    break;
            }

            if (wasUsed)
            {
                OnUsedItem(item);
                if (UsedItem != null)
                    UsedItem.Raise(this, EventArgsHelper.Create(item));
            }

            return wasUsed;
        }

        /// <summary>
        /// Uses a use-once item.
        /// </summary>
        /// <param name="item">The item to be used.</param>
        /// <returns>True if the item was used; otherwise false.</returns>
        bool UseItemUseOnce(ItemEntity item)
        {
            var useBonuses = item.BaseStats.Where(stat => stat.Value != 0);
            foreach (var stat in useBonuses)
            {
                BaseStats[stat.StatType] += stat.Value;
            }

            if (item.HP != 0)
                HP += item.HP;
            if (item.MP != 0)
                MP += item.MP;

            if (item.ActionDisplayID.HasValue)
            {
                using (var pw = ServerPacket.CreateActionDisplayAtEntity(item.ActionDisplayID.Value, MapEntityIndex))
                {
                    Map.Send(pw, ServerMessageType.MapEffect);
                }
            }

            return true;
        }

        /// <summary>
        /// A flag that determines whether this character can take damage or not
        /// </summary>
        public bool Immortal
        {
            get;
            set;
        }


        /// <summary>
        /// Makes the Character use a skill.
        /// </summary>
        /// <param name="skillType">The type of skill to use.</param>
        /// <param name="target">The target to use the skill on. Can be null.</param>
        public void UseSkill(SkillType skillType, Character target)
        {
            var skill = _skillManager.GetSkill(skillType);
            UseSkill(skill, target);
        }

        /// <summary>
        /// Makes the Character use a skill.
        /// </summary>
        /// <param name="skillType">The type of skill to use.</param>
        public void UseSkill(SkillType skillType)
        {
            var skill = _skillManager.GetSkill(skillType);
            UseSkill(skill, null);
        }

        /// <summary>
        /// Makes the Character use a skill.
        /// </summary>
        /// <param name="skill">The skill to use.</param>
        /// <param name="target">The target to use the skill on. Can be null.</param>
        public void UseSkill(ISkill<SkillType, StatType, Character> skill, Character target)
        {
            // Check for a valid skill
            if (skill == null)
            {
                const string errmsg = "Character `{0}` tried to use a null skill in UseSkill.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            _skillCaster.TryStartCastingSkill(skill, target);
        }

        /// <summary>
        /// Validates the given <paramref name="position"/> by attempting to make it a legal position if it is not
        /// one already.
        /// </summary>
        /// <param name="position">The position to validate.</param>
        /// <returns>If the <paramref name="position"/> was already valid, or no valid position was found, contains
        /// the same value as the <paramref name="position"/>; otherwise, contains the corrected valid position.</returns>
        Vector2 ValidatePosition(Vector2 position)
        {
            if (Map == null)
                return position;

            var tempRect = new Rectangle(position.X, position.Y, Size.X, Size.Y);

            Vector2 closestLegalPosition;
            bool isClosestPositionValid;
            if (!Map.IsValidPlacementPosition(tempRect, out closestLegalPosition, out isClosestPositionValid))
            {
                if (isClosestPositionValid)
                {
                    // Near-by legal position found
                    return closestLegalPosition;
                }
                else
                {
                    // No legal position could be found
                    return HandleNoLegalPositionFound(position);
                }
            }

            return position;
        }

        #region ICharacterTable Members

        /// <summary>
        /// Gets the value of the database column `ai_id`.
        /// </summary>
        AIID? ICharacterTable.AIID
        {
            get
            {
                var ai = AI;
                if (ai == null)
                    return null;

                return ai.ID;
            }
        }

        /// <summary>
        /// Gets the value of the database column `body_id`.
        /// </summary>
        BodyID ICharacterTable.BodyID
        {
            get { return BodyInfo.ID; }
        }

        /// <summary>
        /// Gets or sets the amount of Cash the Character has on hand.
        /// </summary>
        public int Cash
        {
            get { return _cash; }
            set
            {
                if (_cash == value)
                    return;

                var oldValue = _cash;
                _cash = value;

                OnCashChanged(oldValue, _cash);
                if (CashChanged != null)
                    CashChanged.Raise(this, ValueChangedEventArgs.Create(oldValue, _cash));
            }
        }

        /// <summary>
        /// Gets or sets the ID of the CharacterTemplate that this Character was created from.
        /// This will not alter the Character in any way except for functions that make use of the
        /// CharacterTemplateID, such as Equipment and Inventory Items a NPC spawns with.
        /// </summary>
        public CharacterTemplateID? CharacterTemplateID
        {
            get { return _templateID; }
            set
            {
                if (_templateID == value)
                    return;

                _templateID = value;

                OnTemplateIDChanged();
                if (TemplateIDChanged != null)
                    TemplateIDChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the value of the database column `chat_dialog`.
        /// </summary>
        NPCChatDialogID? ICharacterTable.ChatDialog
        {
            get
            {
                if (ChatDialog == null)
                    return null;

                return ChatDialog.ID;
            }
        }

        /// <summary>
        /// Gets or sets the amount of experience the Character has.
        /// </summary>
        public int Exp
        {
            get { return _exp; }
            set
            {
                if (_exp == value)
                    return;

                var oldValue = _exp;
                _exp = value;

                OnExpChanged(oldValue, _exp);
                if (ExpChanged != null)
                    ExpChanged.Raise(this, ValueChangedEventArgs.Create(oldValue, _exp));

                // Check if this change in experience has made the Character level
                while (_exp >= _nextLevelExp)
                {
                    LevelUp();
                }
            }
        }

        public SPValueType HP
        {
            get { return _hp; }
            set
            {

                // Prevent this character from taking any damage if they're immortal
                if(Immortal)
                    return;
                
                // Get the new value, ensuring it is in a valid range
                int max = ModStats[StatType.MaxHP];
                SPValueType newValue;
                if (value > max)
                    newValue = max;
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

                OnHPChanged(oldValue, _hp);

                if (HPChanged != null)
                    HPChanged.Raise(this, ValueChangedEventArgs.Create(oldValue, _hp));

                if (_hp <= 0)
                    Kill();
            }
        }

        public CharacterID ID
        {
            get { return _id; }
        }

        public short Level
        {
            get { return _level; }
            private set
            {
                if (_level == value)
                    return;

                var oldValue = _level;
                _level = value;
                _nextLevelExp = GameData.LevelCost(_level);

                OnLevelChanged(oldValue, _level);
                if (LevelChanged != null)
                    LevelChanged.Raise(this, ValueChangedEventArgs.Create(oldValue, _level));
            }
        }

        /// <summary>
        /// Gets the value of the database column `map_id`.
        /// </summary>
        MapID ICharacterTable.LoadMapID
        {
            get { return Map.ID; }
        }

        /// <summary>
        /// Gets the value of the database column `x`.
        /// </summary>
        ushort ICharacterTable.LoadX
        {
            get { return (ushort)GetLoadPosition().X; }
        }

        /// <summary>
        /// Gets the value of the database column `y`.
        /// </summary>
        ushort ICharacterTable.LoadY
        {
            get { return (ushort)GetLoadPosition().Y; }
        }

        /// <summary>
        /// Gets or sets the value of the database column `mp`.
        /// </summary>
        public SPValueType MP
        {
            get { return _mp; }
            set
            {
                // Get the new value, ensuring it is in a valid range
                int max = ModStats[StatType.MaxMP];
                SPValueType newValue;
                if (value > max)
                    newValue = max;
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

                OnMPChanged(oldValue, _mp);
                if (MPChanged != null)
                    MPChanged.Raise(this, ValueChangedEventArgs.Create(oldValue, _mp));
            }
        }

        /// <summary>
        /// Gets or sets the value of the database column `move_speed`.
        /// </summary>
        public ushort MoveSpeed
        {
            get { return _moveSpeed; }
            set
            {
                _moveSpeed = value;
                _moveSpeedVelocityCache = GameData.MovementSpeedToVelocity(_moveSpeed);
            }
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
                if (!GameData.CharacterName.IsValid(value))
                {
                    const string errmsg = "Attempted to give Character `{0}` an invalid name `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, value);
                    Debug.Fail(string.Format(errmsg, this, value));
                    return;
                }

                // Set the new name
                _name = value;
            }
        }

        /// <summary>
        /// Gets the value of the database column `respawn_map`.
        /// </summary>
        MapID? ICharacterTable.RespawnMapID
        {
            get { return RespawnMapID; }
        }

        /// <summary>
        /// Gets the value of the database column `respawn_x`.
        /// </summary>
        float ICharacterTable.RespawnX
        {
            get { return RespawnPosition.X; }
        }

        /// <summary>
        /// Gets the value of the database column `respawn_y`.
        /// </summary>
        float ICharacterTable.RespawnY
        {
            get { return RespawnPosition.Y; }
        }

        /// <summary>
        /// Gets the value of the database column `shop_id`.
        /// </summary>
        ShopID? ICharacterTable.ShopID
        {
            get
            {
                var v = Shop;
                if (v == null)
                    return null;
                return v.ID;
            }
        }

        /// <summary>
        /// Gets the number of points the Character can spend on stats.
        /// </summary>
        public int StatPoints
        {
            get { return _statPoints; }
            private set
            {
                if (_statPoints == value)
                    return;

                var oldValue = _statPoints;
                _statPoints = value;

                OnStatPointsChanged(oldValue, _statPoints);
                if (StatPointsChanged != null)
                    StatPointsChanged.Raise(this, ValueChangedEventArgs.Create(oldValue, _statPoints));
            }
        }

        /// <summary>
        /// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
        /// key is the collection's key and the value is the value for that corresponding key.
        /// </summary>
        IEnumerable<KeyValuePair<StatType, int>> ICharacterTable.Stats
        {
            get { return BaseStats.Select(x => new KeyValuePair<StatType, int>(x.StatType, x.Value)); }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        ICharacterTable ICharacterTable.DeepCopy()
        {
            return new CharacterTable(this);
        }

        /// <summary>
        /// Gets the value of the database column in the column collection `Stat`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        int ICharacterTable.GetStat(StatType key)
        {
            return BaseStats[key];
        }

        #endregion

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public TickCount GetTime()
        {
            return World.GetTime();
        }

        #endregion

        #region IRespawnable Members

        /// <summary>
        /// Gets the DynamicEntity to respawn (typically just "this").
        /// </summary>
        DynamicEntity IRespawnable.DynamicEntity
        {
            get { return this; }
        }

        /// <summary>
        /// Checks if the IRespawnable is ready to be respawned. If the object is already spawned, this should
        /// still return true.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>True if the IRespawnable is ready to respawn, else false.</returns>
        bool IRespawnable.ReadyToRespawn(TickCount currentTime)
        {
            return IsAlive || CheckRespawnElapsedTime(currentTime);
        }

        /// <summary>
        /// Handles respawning the DynamicEntity. This must also take care of setting the Map.
        /// </summary>
        void IRespawnable.Respawn()
        {
            UpdateModStats();

            // Restore the Character's stats
            HP = (int)ModStats[StatType.MaxHP];
            MP = (int)ModStats[StatType.MaxMP];

            // Set the Character's new location
            Teleport(RespawnPosition);

            // Set the map
            if (!RespawnMapID.HasValue)
            {
                // If the respawn map is invalid, there is nothing we can do to spawn it, so dispose of it
                if (log.IsInfoEnabled)
                    log.InfoFormat("Disposing Character `{0}` since they had no respawn map.", this);

                Debug.Assert(!IsDisposed);
                DelayedDispose();
            }
            else
            {
                var respawnMap = World.GetMap(RespawnMapID.Value);
                if (respawnMap == null)
                {
                    const string errmsg = "Tried to respawn `{0}` but the map they tried to respawn on (ID `{1}`) returned null!";
                    if (log.IsFatalEnabled)
                        log.FatalFormat(errmsg, this, RespawnMapID.Value);
                    Debug.Fail(string.Format(errmsg, this, RespawnMapID.Value));

                    DelayedDispose();
                }
                else
                {
                    // Set the Character as alive
                    IsAlive = true;

                    // Move the Character to their respawn map
                    Teleport(respawnMap, Position);
                }
            }
        }

        #endregion

        #region IServerSaveable Members

        /// <summary>
        /// Saves the state of this object and all <see cref="IServerSaveable"/> objects under it to the database.
        /// </summary>
        void IServerSaveable.ServerSave()
        {
            Save();
        }

        #endregion

        #region IUpdateableMapReference Members

        /// <summary>
        /// Gets or sets the map that the <see cref="IUpdateableMapReference"/> is on. This should only be set
        /// by the map that the object was added to.
        /// </summary>
        IMap IUpdateableMapReference.Map
        {
            get { return Map; }
            set
            {
                if (_map == value)
                    return;

                var oldMap = _map;

                _map = (Map)value;

                OnMapChanged(oldMap, _map);
                if (MapChanged != null)
                    MapChanged.Raise(this, ValueChangedEventArgs.Create(oldMap, _map));
            }
        }

        #endregion
    }
}