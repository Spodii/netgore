using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.AI;
using NetGore.Db;
using NetGore.Features.Emoticons;
using NetGore.Features.Shops;
using NetGore.Features.Skills;
using NetGore.Network;
using NetGore.NPCChat;
using NetGore.Stats;

namespace DemoGame.Server
{
    /// <summary>
    /// The server representation of a single Character that can be either player-controller or computer-controller.
    /// </summary>
    public abstract class Character : CharacterEntity, IGetTime, IRespawnable, ICharacterTable, IUpdateableMapReference
    {
        /// <summary>
        /// Delegate for handling an event from a <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> the event took place on.</param>
        public delegate void CharacterEventHandler(Character character);

        /// <summary>
        /// Delegate for handling an event from a <see cref="Character"/>.
        /// </summary>
        /// <param name="attacker">The <see cref="Character"/> that did the attacking.</param>
        /// <param name="attacked">The <see cref="Character"/> that was attacked.</param>
        /// <param name="damage">The amount of damage inflicted on the <paramref name="attacked"/> by
        /// the <paramref name="attacker"/>.</param>
        public delegate void CharacterAttackCharacterEventHandler(Character attacker, Character attacked, int damage);

        /// <summary>
        /// Delegate for handling an event from a <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> the event took place on.</param>
        /// <param name="map">The map related to the event.</param>
        public delegate void CharacterMapEventHandler(Character character, Map map);

        /// <summary>
        /// Delegate for handling an event from a <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> the event took place on.</param>
        /// <param name="oldMap">The old map.</param>
        /// <param name="newMap">The new map.</param>
        public delegate void CharacterChangeMapEventHandler(Character character, Map oldMap, Map newMap);

        /// <summary>
        /// Delegate for handling an event from a <see cref="Character"/>.
        /// </summary>
        /// <param name="killed">The <see cref="Character"/> that was killed.</param>
        /// <param name="killer">The <see cref="Character"/> that killed the <paramref name="killed"/>.</param>
        public delegate void CharacterKillEventHandler(Character killed, Character killer);

        /// <summary>
        /// Delegate for handling an event from a <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> the event took place on.</param>
        /// <param name="item">The <see cref="ItemEntity"/> related to the event.</param>
        public delegate void CharacterItemEventHandler(Character character, ItemEntity item);

        /// <summary>
        /// Delegate for handling an event from a <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> the event took place on.</param>
        /// <param name="oldExp">The old amount of experience.</param>
        /// <param name="exp">The new amount of experience.</param>
        public delegate void CharacterExpEventHandler(Character character, int oldExp, int exp);

        /// <summary>
        /// Delegate for handling an event from a <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> the event took place on.</param>
        /// <param name="oldLevel">The old level.</param>
        /// <param name="level">The new level.</param>
        public delegate void CharacterLevelEventHandler(Character character, byte oldLevel, byte level);

        /// <summary>
        /// Delegate for handling an event from a <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> the event took place on.</param>
        /// <param name="oldCash">The old amount of cash.</param>
        /// <param name="cash">The new amount of cash.</param>
        public delegate void CharacterCashEventHandler(Character character, int oldCash, int cash);

        /// <summary>
        /// Delegate for handling an event from a <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> the event took place on.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public delegate void CharacterChangeSPEventHandler(Character character, SPValueType oldValue, SPValueType newValue);

        /// <summary>
        /// Delegate for handling an event from a <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> the event took place on.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public delegate void CharacterStatPointsEventHandler(Character character, int oldValue, int newValue);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Amount of time the character must wait between attacks
        /// </summary>
        const int _attackTimeout = 500;

        /// <summary>
        /// How frequently the SP is recovered in milliseconds.
        /// </summary>
        const int _spRecoveryRate = 3000;

        static readonly AllianceManager _allianceManager = AllianceManager.Instance;
        static readonly CharacterTemplateManager _characterTemplateManager = CharacterTemplateManager.Instance;

        /// <summary>
        /// Random number generator for Characters
        /// </summary>
        static readonly Random _rand = new Random();

        static readonly ShopManager _shopManager = ShopManager.Instance;

        readonly CharacterStatsBase _baseStats;
        readonly CharacterEquipped _equipped;
        readonly CharacterInventory _inventory;
        readonly bool _isPersistent;
        readonly CharacterStatsBase _modStats;
        readonly CharacterSPSynchronizer _spSync;
        readonly CharacterStatusEffects _statusEffects;
        readonly World _world;

        /// <summary>
        /// The account ID of this Character, or null if they don't have an account. Normally, a User should always have
        /// an account ID, and an NPC should never have one.
        /// </summary>
        AccountID? _accountID;

        /// <summary>
        /// Character's alliance.
        /// </summary>
        Alliance _alliance;

        int _cash;
        int _exp;
        SPValueType _hp;
        CharacterID _id;

        /// <summary>
        /// Gets the weapon to use for attacking. Cannot be null.
        /// </summary>
        public ItemEntity Weapon { get { 
            var weapon = _equipped[EquipmentSlot.RightHand];

            if (weapon == null)
                return World.UnarmedWeapon;

            return weapon;
        } }

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

        readonly CharacterSkillCaster _skillCaster;

        CharacterTemplateID? _templateID;

        /// <summary>
        /// Notifies listeners when the Character performs an attack. The attack does not have to actually hit
        /// anything for this event to be raised. This will be raised before <see cref="AttackedCharacter"/>.
        /// </summary>
        public event CharacterEventHandler Attacked;

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.Attacked"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        protected virtual void OnAttacked()
        {
        }

        /// <summary>
        /// Notifies listeners when this Character has successfully attacked another Character.
        /// </summary>
        public event CharacterAttackCharacterEventHandler AttackedCharacter;

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
        /// Notifies listeners when this Character has been attacked by another Character.
        /// </summary>
        public event CharacterAttackCharacterEventHandler AttackedByCharacter;

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
        /// Notifies listeners when this <see cref="Character"/>'s cash value has changed.
        /// </summary>
        public event CharacterCashEventHandler CashChanged;

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
        /// Notifies listeners when this <see cref="Character"/>'s exp value has changed.
        /// </summary>
        public event CharacterExpEventHandler ExpChanged;

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
        /// Notifies listeners when this <see cref="Character"/>'s HP value has changed.
        /// </summary>
        public event CharacterChangeSPEventHandler HPChanged;

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
        /// Notifies listeners when this <see cref="Character"/>'s level value has changed.
        /// </summary>
        public event CharacterLevelEventHandler LevelChanged;

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.LevelChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnLevelChanged(byte oldValue, byte newValue)
        {
        }

        /// <summary>
        /// Notifies listeners when this <see cref="Character"/>'s MP value has changed.
        /// </summary>
        public event CharacterChangeSPEventHandler MPChanged;

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
        /// Notifies listeners when this <see cref="Character"/>'s StatPoints value has changed.
        /// </summary>
        public event CharacterStatPointsEventHandler StatPointsChanged;

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
        /// Notifies listeners when the Character's TemplateID has changed.
        /// </summary>
        public event CharacterEventHandler TemplateIDChanged;

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.TemplateIDChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        protected virtual void OnTemplateIDChanged()
        {
        }

        /// <summary>
        /// Notifies listeners when this Character has dropped an item.
        /// </summary>
        public event CharacterItemEventHandler DroppedItem;

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
        /// Notifies listeners when this Character has received an item.
        /// </summary>
        public event CharacterItemEventHandler GotItem;

        // TODO: Implement OnGetItem. Difficulty implementing is due to the inventory system making a deep copy of things. Should probably add some events to the InventoryBase.

        /// <summary>
        /// Notifies listeners when this Character has killed another Character.
        /// </summary>
        public event CharacterKillEventHandler KilledCharacter;

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.KilledCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="killed">The <see cref="Character"/> that this <see cref="Character"/> killed.</param>
        protected virtual void OnKilledCharacter(Character killed)
        {
        }

        /// <summary>
        /// Notifies listeners when this Character has been killed in any way, no matter who did it or how it happened.
        /// </summary>
        public event CharacterEventHandler Killed;

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.Killed"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        protected virtual void OnKilled()
        {
        }

        /// <summary>
        /// Notifies listeners when this Character has been killed by another Character.
        /// </summary>
        public event CharacterKillEventHandler KilledByCharacter;

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
        /// Notifies listeners when this Character uses an item.
        /// </summary>
        public event CharacterItemEventHandler UsedItem;

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.KilledByCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="item">The item that was used.</param>
        protected virtual void OnUsedItem(ItemEntity item)
        {
        }

        /// <summary>
        /// Notifies listeners when this <see cref="Character"/>'s map has changed.
        /// </summary>
        public event CharacterChangeMapEventHandler MapChanged;

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
        /// Initializes a new instance of the <see cref="Character"/> class.
        /// </summary>
        /// <param name="world">World that the character belongs to.</param>
        /// <param name="isPersistent">If the Character's state is persistent. If true, Load() MUST be called
        /// at some point during the Character's constructor!</param>
        protected Character(World world, bool isPersistent) : base(Vector2.Zero, Vector2.One)
        {
            _skillCaster = new CharacterSkillCaster(this);

            _world = world;
            _isPersistent = isPersistent;

            if (IsPersistent)
                _statusEffects = new PersistentCharacterStatusEffects(this);
            else
                _statusEffects = new NonPersistentCharacterStatusEffects(this);

            _statusEffects.Added += StatusEffects_HandleOnAdd;
            _statusEffects.Removed += StatusEffects_HandleOnRemove;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _baseStats = CreateStats(StatCollectionType.Base);
            _modStats = CreateStats(StatCollectionType.Modified);
            _spSync = CreateSPSynchronizer();
            _inventory = CreateInventory();
            _equipped = CreateEquipped();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            // Set up the listeners for when the stat collections change
            IStatCollectionStatEventHandler<StatType> statChangedHandler = delegate { _updateModStats = true; };
            BaseStats.StatChanged += statChangedHandler;
            ModStats.StatChanged += statChangedHandler;

            // Set up the listeners for when the equipped items change
            EquippedEventHandler<ItemEntity> equippedChangeHandler = delegate { _updateModStats = true; };
            _equipped.Equipped += equippedChangeHandler;
            _equipped.Unequipped += equippedChangeHandler;

            // Listen to when the max HP and MP change to make sure the current HP and MP stay in a valid range
            ModStats.GetStat(StatType.MaxHP).Changed += delegate(IStat<StatType> stat)
                                                        {
                                                            if (HP > stat.Value)
                                                                HP = stat.Value;
                                                        };

            ModStats.GetStat(StatType.MaxMP).Changed += delegate(IStat<StatType> stat)
                                                        {
                                                            if (MP > stat.Value)
                                                                MP = stat.Value;
                                                        };
        }

        bool _updateModStats = true;

        /// <summary>
        /// When overridden in the derived class, gets the Character's AI. Can be null if they have no AI.
        /// </summary>
        public abstract IAI AI { get; }

        /// <summary>
        /// Gets or sets (protected) the Character's alliance.
        /// </summary>
        public Alliance Alliance
        {
            get { return _alliance; }
            protected set { _alliance = value; }
        }

        /// <summary>
        /// Gets the <see cref="AllianceManager"/> instance to be used by the <see cref="Character"/>s.
        /// </summary>
        protected static AllianceManager AllianceManager
        {
            get { return _allianceManager; }
        }

        public CharacterStatsBase BaseStats
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
        public override bool HasChatDialog
        {
            get { return ChatDialog != null; }
            protected set { throw new NotSupportedException("This value should never be set directly in the Server."); }
        }

        /// <summary>
        /// When overridden in the derived class, gets if the CharacterEntity has a shop. Do not use the setter
        /// on this property.
        /// </summary>
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
        public bool IsAlive
        {
            get { return _isAlive; }
            protected set { _isAlive = value; }
        }

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
        /// Gets the Map the Character is currently on. This value can be null if the Character is not currently
        /// on a Map.
        /// </summary>
        public Map Map
        {
            get { return _map; }
        }

        public CharacterStatsBase ModStats
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
        public MapIndex? RespawnMapIndex { get; set; }

        /// <summary>
        /// Gets or sets where the Character will respawn in their respawn map. Only valid if the
        /// RespawnMapIndex is set to a non-null value.
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

        public void Attack(Character target)
        {
            Attack(Weapon, target);
        }

        /// <summary>
        /// Attacks the <paramref name="target"/> using the given <paramref name="weapon"/>.
        /// </summary>
        /// <param name="weapon">The weapon to use for attacking. If null, will be treated as an unarmed melee attack.</param>
        /// <param name="target">The <see cref="Character"/> to attack. If null, the target will be selected
        /// automatically if applicable.</param>
        public void Attack(ItemEntity weapon, Character target)
        {
            var currTime = GetTime();

            // Don't allow attacking while casting a skill
            if (_skillCaster.IsCastingSkill)
                return;

            // Check that enough time has elapsed since the last attack
            if (_lastAttackTime + _attackTimeout > currTime)
                return;

            // If no weapon is specified, use the unarmed weapon
            if (weapon == null)
                weapon = Weapon;

            // Abort if using an unknown weapon type
            if (weapon.WeaponType == WeaponType.Unknown)
            {
                // TODO: Send a message to the client: "You cannot attack using a {0}."
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

                if (this.GetDistance(target) > weapon.Range)
                {
                    // TODO: Send a message to the client: "Cannot attack {0} because they are too far away."
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
                case WeaponType.Gun:
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
            _lastAttackTime = currTime;
        }

        /// <summary>
        /// Handles attacking with a melee weapon.
        /// </summary>
        /// <param name="weapon">The weapon to attack with.</param>
        /// <param name="target">The target to attack. Can be null.</param>
        void AttackMelee(ItemEntity weapon, Character target)
        {
            // We will show the melee attack animation no matter what, so just send it now
            using (PacketWriter charAttack = ServerPacket.CharAttack(MapEntityIndex))
            {
                Map.SendToArea(this, charAttack);
            }

            OnAttacked();
            if (Attacked != null)
                Attacked(this);

            // If melee and no target was defined, try to find one automatically
            if (target == null)
            {
                var hitArea = GameData.GetMeleeAttackArea(this, weapon.Range);
                target = Map.Spatial.Get<Character>(hitArea, x => x != this && x.IsAlive && Alliance.CanAttack(x.Alliance));
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
        /// <param name="weapon">The weapon to attack with.</param>
        /// <param name="target">The target to attack. Can be null.</param>
        void AttackRanged(ItemEntity weapon, Character target)
        {
            // We can't do anything with ranged attacks if no target is given
            if (target == null)
            {
                // TODO: Send a message to the client: "You must select a target to attack first."
                return;
            }

            bool ammoUsed = false;

            // Check for the needed ammo
            switch (weapon.WeaponType)
            {
                case WeaponType.Projectile:
                    // Grab projectile ammo out of the inventory first if possible to avoid having to constantly reload
                    var invAmmo = Inventory.FirstOrDefault(x => weapon.CanStack(x.Value));
                    if (invAmmo.Value != null)
                        Inventory.DecreaseItemAmount(invAmmo.Key);
                    else
                        weapon.Dispose();

                    ammoUsed = true;
                    break;

                case WeaponType.Gun:
                    // By default, guns won't use ammo. But if you want to require guns to use ammo, you can do so here
                    ammoUsed = true;
                    break;
            }

            if (!ammoUsed)
                return;

            // Attack
            using (PacketWriter charAttack = ServerPacket.CharAttack(MapEntityIndex))
            {
                Map.SendToArea(this, charAttack);
            }

            OnAttacked();
            if (Attacked != null)
                Attacked(this);

            AttackApplyReal(target);
        }

        /// <summary>
        /// Performs the actual attacking of a specific character. This should only be called by
        /// <see cref="AttackMelee"/> or <see cref="AttackRanged"/>.
        /// </summary>
        /// <param name="target">Character to attack.</param>
        void AttackApplyReal(Character target)
        {
            // Get the damage
            int damage = GetAttackDamage(target);

            // Apply the damage to the target
            target.Damage(this, damage);

            // Raise attack events
            OnAttackedCharacter(target, damage);
            if (AttackedCharacter != null)
                AttackedCharacter(this, target, damage);

            target.OnAttackedByCharacter(this, damage);
            if (target.AttackedByCharacter != null)
                target.AttackedByCharacter(this, target, damage);
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

            Teleport(Position);

            _spSync.ForceSynchronize();
        }

        /// <summary>
        /// When overridden in the derived class, checks if enough time has elapesd since the Character died
        /// for them to be able to respawn.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        /// <returns>True if enough time has elapsed; otherwise false.</returns>
        protected abstract bool CheckRespawnElapsedTime(int currentTime);

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
        protected abstract CharacterStatsBase CreateStats(StatCollectionType statCollectionType);

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
                Map.SendToArea(this, pw);
            }

            int newHP = HP - damage;
            if (newHP < 0)
                newHP = 0;

            HP = newHP;

            // Check if the character died
            if (HP <= 0)
            {
                if (source != null)
                {
                    Character sourceCharacter = source as Character;
                    if (sourceCharacter != null)
                    {
                        sourceCharacter.OnKilledCharacter(this);
                        if (sourceCharacter.KilledCharacter != null)
                            sourceCharacter.KilledCharacter(this, sourceCharacter);

                        OnKilledByCharacter(sourceCharacter);
                        if (KilledByCharacter != null)
                            KilledByCharacter(this, sourceCharacter);
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
            var stack = World.DisposeStack;
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

            OnDroppedItem(item);
            if (DroppedItem != null)
                DroppedItem(this, item);
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
            Vector2 dropPos = GetDropPos();

            // Create the item on the map
            ItemEntity droppedItem = Map.CreateItem(itemTemplate, dropPos, amount);

            OnDroppedItem(droppedItem);
            if (DroppedItem != null)
                DroppedItem(this, droppedItem);
        }

        /// <summary>
        /// Makes the <see cref="Character"/> use an <see cref="Emoticon"/>.
        /// </summary>
        /// <param name="emoticon">The emoticon to use.</param>
        public void Emote(Emoticon emoticon)
        {
            using (var pw = ServerPacket.Emote(MapEntityIndex, emoticon))
            {
                Map.SendToArea(this, pw);
            }
        }

        /// <summary>
        /// Attempts to equip an item from the User's inventory.
        /// </summary>
        /// <param name="inventorySlot">Index of the slot containing the item to equip.</param>
        /// <returns>True if the item was successfully equipped, else false.</returns>
        public bool Equip(InventorySlot inventorySlot)
        {
            // Get the item from the inventory
            ItemEntity item = Inventory[inventorySlot];

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
            bool successful = Equipped.Equip(item);

            // If failed to equip, give the item back to the User
            if (!successful)
            {
                ItemEntity remainder = GiveItem(item);
                if (remainder != null)
                {
                    Debug.Fail("What the hell just happened? Failed to equip the item, and failed to add back to inventory?");
                    DropItem(remainder);
                }
            }

            return successful;
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

            int minHit = ModStats[StatType.MinHit];
            int maxHit = ModStats[StatType.MaxHit];

            if (minHit > maxHit)
                maxHit = minHit;

            int damage = Rand.Next(minHit, maxHit);

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
        public virtual ItemEntity GiveItem(ItemEntity item)
        {
            if (item == null)
            {
                Debug.Fail("Item is null.");
                return null;
            }

            Debug.Assert(item.Amount != 0, "Invalid item amount.");

            // Add as much of the item to the inventory as we can
            byte startAmount = item.Amount;
            ItemEntity remainder = _inventory.Add(item);

            // Check how much was added
            byte amountAdded;
            if (remainder == null)
                amountAdded = startAmount;
            else
            {
                Debug.Assert(startAmount >= item.Amount, "Somehow the startAmount is less than the current amount of items.");
                Debug.Assert(startAmount - item.Amount >= 0, "Negative item amount given...?");
                amountAdded = (byte)(startAmount - item.Amount);
            }

            if (amountAdded > 0)
                AfterGiveItem(item, amountAdded);

            // Return the remainder
            return remainder;
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
        protected override void HandleDispose()
        {
            // Make sure the Character was saved
            Save();

            // Dispose of disposable stuff
            if (Equipped != null)
                Equipped.Dispose();

            if (Inventory != null)
                Inventory.Dispose();

            if (StatusEffects != null)
                StatusEffects.Dispose();

            base.HandleDispose();
        }

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected override void HandleUpdate(IMap imap, float deltaTime)
        {
            Debug.Assert(imap == Map, "Character.Update()'s imap is, for whatever reason, not equal to the set Map.");

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
        /// Checks if the given string is a valid string for a character name.
        /// </summary>
        /// <param name="s">The string to test.</param>
        /// <returns>True if <paramref name="s"/> is a valid string for a character name; otherwise false.</returns>
        public static bool IsValidName(string s)
        {
            return GameData.CharacterName.IsValid(s);
        }

#if !TOPDOWN
        /// <summary>
        /// Makes the Character jump if <see cref="Character.CanJump"/> is true. If <see cref="Character.CanJump"/> is false,
        /// this will do nothing.
        /// </summary>
        public void Jump()
        {
            if (!CanJump)
                return;

            if (_skillCaster.IsCastingSkill)
                return;

            SetVelocity(Velocity + new Vector2(0.0f, -0.48f));
        }
#endif

        /// <summary>
        /// When overridden in the derived class, implements the Character being killed. This 
        /// doesn't actually care how the Character was killed, it just takes the appropriate
        /// actions to kill them.
        /// </summary>
        public virtual void Kill()
        {
            OnKilled();
            if (Killed != null)
                Killed(this);
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
            ICharacterTable values = DbController.GetQuery<SelectCharacterByIDQuery>().Execute(characterID);
            LoadFromQueryValues(values);
        }

        protected void Load(string characterName)
        {
            ICharacterTable values = DbController.GetQuery<SelectCharacterQuery>().Execute(characterName);
            LoadFromQueryValues(values);
        }

        protected void Load(CharacterTemplate template)
        {
            ICharacterTemplateTable v = template.TemplateTable;

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

            BaseStats.CopyValuesFrom(v.Stats, false);
        }

        void LoadFromQueryValues(ICharacterTable v)
        {
            Name = v.Name;
            _id = v.ID;
            _templateID = v.CharacterTemplateID;
            _accountID = v.AccountID;
            MoveSpeed = v.MoveSpeed;

            BodyInfo = BodyInfoManager.Instance.GetBody(v.BodyID);

            Teleport(new Vector2(v.X, v.Y));
            Resize(BodyInfo.Size);

            ((PersistentCharacterStatusEffects)StatusEffects).Load();

            // Set the character information
            _level = v.Level;
            _exp = v.Exp;
            _cash = v.Cash;
            _hp = new SPValueType(v.HP);
            _mp = new SPValueType(v.MP);
            RespawnMapIndex = v.RespawnMap;
            RespawnPosition = new Vector2(v.RespawnX, v.RespawnY);
            StatPoints = v.StatPoints;

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
            _baseStats.CopyValuesFrom(v.Stats, false);

            // Set the Map and, if a User, add them to the World
            Map m = World.GetMap(v.MapID);
            // TODO: We can recover when a NPC's map is invalid at least... See bug: http://netgore.com/bugs/view.php?id=103
            if (m == null)
                throw new Exception(string.Format("Unable to get Map with index `{0}`.", v.MapID));

            // Load the Character's items
            Inventory.Load();
            Equipped.Load();

            // Update the mod stats
            UpdateModStats();

            // Additional loading
            HandleAdditionalLoading(v);

            // Set the map
            ChangeMap(m);

            // Mark the Character as loaded
            SetAsLoaded();

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded Character `{0}`.", Name);
        }

#if TOPDOWN
    /// <summary>
    /// Starts moving the character down.
    /// </summary>
        public void MoveDown()
        {
            if (IsMovingDown)
                return;

            if (_skillCaster.IsCastingSkill)
                return;

            SetVelocity(new Vector2(Velocity.X, _moveSpeedVelocityCache));
        }
#endif

        /// <summary>
        /// Starts moving the character to the left.
        /// </summary>
        public void MoveLeft()
        {
            if (IsMovingLeft)
                return;

            if (_skillCaster.IsCastingSkill)
                return;

            SetVelocity(new Vector2(-_moveSpeedVelocityCache, Velocity.Y));
        }

        /// <summary>
        /// Starts moving the character to the right.
        /// </summary>
        public void MoveRight()
        {
            if (IsMovingRight)
                return;

            if (_skillCaster.IsCastingSkill)
                return;

            SetVelocity(new Vector2(_moveSpeedVelocityCache, Velocity.Y));
        }

#if TOPDOWN
    /// <summary>
    /// Starts moving the character up.
    /// </summary>
        public void MoveUp()
        {
            if (IsMovingUp)
                return;

            if (_skillCaster.IsCastingSkill)
                return;

            SetVelocity(new Vector2(Velocity.X, -_moveSpeedVelocityCache));
        }
#endif

        /// <summary>
        /// Makes the Character raise their base Stat of the corresponding type by one point, assuming they have enough
        /// points available to raise the Stat, and lowers the amount of spendable points accordingly.
        /// </summary>
        /// <param name="st">StatType of the stat to raise.</param>
        public void RaiseStat(StatType st)
        {
            int cost = GameData.StatCost(BaseStats[st]);

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
        /// When overridden in the derived class, allows for additional steps to be taken when saving.
        /// </summary>
        protected virtual void HandleSave()
        {
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
                                                         ActiveStatusEffect activeStatusEffect)
        {
            _updateModStats = true;
        }

        /// <summary>
        /// Handles when an ActiveStatusEffect is removed from this Character's StatusEffects.
        /// </summary>
        /// <param name="characterStatusEffects">The CharacterStatusEffects the event took place on.</param>
        /// <param name="activeStatusEffect">The ActiveStatusEffect that was removed.</param>
        protected virtual void StatusEffects_HandleOnRemove(CharacterStatusEffects characterStatusEffects,
                                                            ActiveStatusEffect activeStatusEffect)
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
        /// Synchronizes this Character's SP to the specified <paramref name="user"/>.
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
        public void Teleport(Map newMap, Vector2 position)
        {
            if (newMap == null)
                throw new ArgumentNullException("newMap");

            if (newMap != Map)
            {
                if (Map != null)
                    Map.RemoveEntity(this);

                _map = null;

                newMap.AddEntity(this);
            }

            Teleport(position);
        }

        /// <summary>
        /// Teleports the character to a new position and informs clients in the area of
        /// interest that the character has teleported.
        /// </summary>
        /// <param name="position">Position to teleport to.</param>
        public override void Teleport(Vector2 position)
        {
            if (Map == null)
            {
                if (IsAlive && IsLoaded)
                {
                    // If the map is null, but they are alive and loaded, we have a problem...
                    const string errmsg = "Attempted to teleport a Character `{0}` while their map was null.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this);
                    Debug.Fail(string.Format(errmsg, this));
                }
            }
            else
            {
                // Make sure the position we teleport to is valid
                position = ValidatePosition(position);
            }

            base.Teleport(position);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} [ID: {1}, Type: {2}]", Name, ID, GetType().Name);
        }

        /// <summary>
        /// Updates the character's mod stats.
        /// </summary>
        protected void UpdateModStats()
        {
            _updateModStats = false;

            // FUTURE: This is called every goddamn Update(). That is WAY too much...
            foreach (var modStat in ModStats)
            {
                modStat.Value = ModStatHelper<StatType>.Calculate(BaseStats, modStat.StatType, Equipped, StatusEffects);
            }

            if (log.IsDebugEnabled)
                log.DebugFormat("Updated mod stats for character `{0}`.", this);
        }

        /// <summary>
        /// Updates the Character's status points recovery.
        /// </summary>
        void UpdateSPRecovery()
        {
            int time = GetTime();

            // Check that enough time has elapsed
            if (_spRecoverTime > time)
                return;

            // Set the new recovery time
            _spRecoverTime += _spRecoveryRate;

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

            if (wasUsed)
            {
                OnUsedItem(item);
                if (UsedItem != null)
                    UsedItem(this, item);
            }

            return wasUsed;
        }

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

            return true;
        }

        /// <summary>
        /// Makes the Character use a skill.
        /// </summary>
        /// <param name="skillType">The type of skill to use.</param>
        /// <param name="target">The target to use the skill on. Can be null.</param>
        public void UseSkill(SkillType skillType, Character target)
        {
            UseSkill(_skillManager.GetSkill(skillType), target);
        }

        static readonly SkillManager _skillManager = SkillManager.Instance;

        /// <summary>
        /// Makes the Character use a skill.
        /// </summary>
        /// <param name="skillType">The type of skill to use.</param>
        public void UseSkill(SkillType skillType)
        {
            UseSkill(_skillManager.GetSkill(skillType), null);
        }

        /// <summary>
        /// Makes the Character use a skill.
        /// </summary>
        /// <param name="skill">The skill to use.</param>
        /// <param name="target">The target to use the skill on. Can be null.</param>
        public void UseSkill(ISkill<SkillType, StatType, Character> skill, Character target)
        {
            if (skill == null)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("Character `{0}` tried to use a null skill in UseSkill.", this);
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

            var tempRect = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y);

            Vector2 closestLegalPosition;
            bool isClosestPositionValid;
            if (!Map.IsValidPlacementPosition(tempRect, out closestLegalPosition, out isClosestPositionValid))
            {
                if (isClosestPositionValid)
                    return closestLegalPosition;
                else
                {
                    // TODO: Could not find a valid position for the Character. Need to do SOMETHING here...
                }
            }

            return position;
        }

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
                    MapChanged(this, oldMap, _map);
            }
        }

        #region ICharacterTable Members

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

        /// <summary>
        /// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
        /// key is the collection's key and the value is the value for that corresponding key.
        /// </summary>
        IEnumerable<KeyValuePair<StatType, int>> ICharacterTable.Stats
        {
            get { return BaseStats.ToKeyValuePairs(); }
        }

        /// <summary>
        /// Gets the value of the database column `account_id`.
        /// </summary>
        AccountID? ICharacterTable.AccountID
        {
            get { return _accountID; }
        }

        /// <summary>
        /// Gets the value of the database column `body_id`.
        /// </summary>
        BodyIndex ICharacterTable.BodyID
        {
            get { return BodyInfo.Index; }
        }

        /// <summary>
        /// Gets the amount of Cash the Character has on hand.
        /// </summary>
        public int Cash
        {
            get { return _cash; }
            protected set
            {
                if (_cash == value)
                    return;

                int oldValue = _cash;
                _cash = value;

                OnCashChanged(oldValue, _cash);
                if (CashChanged != null)
                    CashChanged(this, oldValue, _cash);
            }
        }

        /// <summary>
        /// Gets the value of the database column `chat_dialog`.
        /// </summary>
        ushort? ICharacterTable.ChatDialog
        {
            get
            {
                if (ChatDialog == null)
                    return null;

                return ChatDialog.Index;
            }
        }

        /// <summary>
        /// Gets the amount of experience the Character has.
        /// </summary>
        public int Exp
        {
            get { return _exp; }
            private set
            {
                if (_exp == value)
                    return;

                int oldValue = _exp;
                _exp = value;

                OnExpChanged(oldValue, _exp);
                if (ExpChanged != null)
                    ExpChanged(this, oldValue, _exp);

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
                SPValueType oldValue = _hp;
                if (newValue == oldValue)
                    return;

                // Apply new value
                _hp = newValue;

                OnHPChanged(oldValue, _hp);
                if (HPChanged != null)
                    HPChanged(this, oldValue, _hp);

                if (_hp <= 0)
                    Kill();
            }
        }

        public CharacterID ID
        {
            get { return _id; }
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

                OnLevelChanged(oldValue, _level);
                if (LevelChanged != null)
                    LevelChanged(this, oldValue, _level);
            }
        }

        /// <summary>
        /// Gets the value of the database column `map_id`.
        /// </summary>
        MapIndex ICharacterTable.MapID
        {
            get { return Map.Index; }
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
                SPValueType oldValue = _mp;
                if (newValue == oldValue)
                    return;

                // Apply new value
                _mp = newValue;

                OnMPChanged(oldValue, _mp);
                if (MPChanged != null)
                    MPChanged(this, oldValue, _mp);
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
        /// Gets the value of the database column `respawn_map`.
        /// </summary>
        MapIndex? ICharacterTable.RespawnMap
        {
            get { return RespawnMapIndex; }
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

                int oldValue = _statPoints;
                _statPoints = value;

                OnStatPointsChanged(oldValue, _statPoints);
                if (StatPointsChanged != null)
                    StatPointsChanged(this, oldValue, _statPoints);
            }
        }

        /// <summary>
        /// Gets the value of the database column `x`.
        /// </summary>
        float ICharacterTable.X
        {
            get { return Position.X; }
        }

        /// <summary>
        /// Gets the value of the database column `y`.
        /// </summary>
        float ICharacterTable.Y
        {
            get { return Position.Y; }
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
                    TemplateIDChanged(this);
            }
        }

        #endregion

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

        #region IRespawnable Members

        /// <summary>
        /// Checks if the IRespawnable is ready to be respawned. If the object is already spawned, this should
        /// still return true.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>True if the IRespawnable is ready to respawn, else false.</returns>
        bool IRespawnable.ReadyToRespawn(int currentTime)
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
            HP = ModStats[StatType.MaxHP];
            MP = ModStats[StatType.MaxMP];

            // Set the Character's new location
            Teleport(RespawnPosition);

            // Set the Character as alive
            IsAlive = true;

            // Set the map
            if (!RespawnMapIndex.HasValue)
            {
                // If the respawn map is invalid, there is nothing we can do to spawn it, so dispose of it
                if (log.IsInfoEnabled)
                    log.InfoFormat("Disposing Character `{0}` since they had no respawn map.");

                Debug.Assert(!IsDisposed);
                DelayedDispose();
            }
            else
            {
                Map respawnMap = World.GetMap(RespawnMapIndex.Value);
                if (respawnMap == null)
                {
                    // TODO: Invalid respawn map? If a User, log them out. If a NPC... Dispose of it? In either case, a log.Fatal() is needed.
                    Debug.Fail("...");
                }
                else
                {
                    // Move the Character to their respawn map
                    ChangeMap(respawnMap);
                }
            }
        }

        /// <summary>
        /// Gets the DynamicEntity to respawn (typically just "this").
        /// </summary>
        DynamicEntity IRespawnable.DynamicEntity
        {
            get { return this; }
        }

        #endregion
    }
}