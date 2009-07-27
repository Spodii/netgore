using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using DbLinq.Data.Linq;
using DbLinq.MySql;
using DbLinq.Vendor;

namespace DemoGame.Db
{
    public class DemoGameDb : DataContext
    {
        public Table<Alliance> Alliance
        {
            get { return GetTable<Alliance>(); }
        }

        public Table<AllianceAttackable> AllianceAttackable
        {
            get { return GetTable<AllianceAttackable>(); }
        }

        public Table<AllianceHostile> AllianceHostile
        {
            get { return GetTable<AllianceHostile>(); }
        }

        public Table<Character> Character
        {
            get { return GetTable<Character>(); }
        }

        public Table<CharacterEquipped> CharacterEquipped
        {
            get { return GetTable<CharacterEquipped>(); }
        }

        public Table<CharacterInventory> CharacterInventory
        {
            get { return GetTable<CharacterInventory>(); }
        }

        public Table<CharacterTemplate> CharacterTemplate
        {
            get { return GetTable<CharacterTemplate>(); }
        }

        public Table<CharacterTemplateEquipped> CharacterTemplateEquipped
        {
            get { return GetTable<CharacterTemplateEquipped>(); }
        }

        public Table<CharacterTemplateInventory> CharacterTemplateInventory
        {
            get { return GetTable<CharacterTemplateInventory>(); }
        }

        public Table<Item> Item
        {
            get { return GetTable<Item>(); }
        }

        public Table<ItemTemplate> ItemTemplate
        {
            get { return GetTable<ItemTemplate>(); }
        }

        public Table<Map> Map
        {
            get { return GetTable<Map>(); }
        }

        public Table<MapSpawn> MapSpawn
        {
            get { return GetTable<MapSpawn>(); }
        }

        public DemoGameDb(IDbConnection connection) : base(connection, new MySqlVendor())
        {
        }

        public DemoGameDb(IDbConnection connection, IVendor vendor)
            : base(connection, vendor)
        {
        }
    }

    [Table(Name = "demogame.alliance")]
    public class Alliance : INotifyPropertyChanged
    {
        EntitySet<AllianceAttackable> _allianceAttackable;
        EntitySet<AllianceAttackable> _allianceAttackable1;
        EntitySet<AllianceHostile> _allianceHostile;
        EntitySet<AllianceHostile> _allianceHostile1;
        EntitySet<CharacterTemplate> _characterTemplate;
        byte _id;

        string _name;

        [Association(Storage = "_allianceAttackable", OtherKey = "AttackableID", ThisKey = "ID",
            Name = "alliance_attackable_ibfk_3")]
        [DebuggerNonUserCode]
        public EntitySet<AllianceAttackable> AllianceAttackable
        {
            get { return _allianceAttackable; }
            set { _allianceAttackable = value; }
        }

        [Association(Storage = "_allianceAttackable1", OtherKey = "AllianceID", ThisKey = "ID",
            Name = "alliance_attackable_ibfk_4")]
        [DebuggerNonUserCode]
        public EntitySet<AllianceAttackable> AllianceAttackable1
        {
            get { return _allianceAttackable1; }
            set { _allianceAttackable1 = value; }
        }

        [Association(Storage = "_allianceHostile", OtherKey = "HostileID", ThisKey = "ID", Name = "alliance_hostile_ibfk_3")]
        [DebuggerNonUserCode]
        public EntitySet<AllianceHostile> AllianceHostile
        {
            get { return _allianceHostile; }
            set { _allianceHostile = value; }
        }

        [Association(Storage = "_allianceHostile1", OtherKey = "AllianceID", ThisKey = "ID", Name = "alliance_hostile_ibfk_4")]
        [DebuggerNonUserCode]
        public EntitySet<AllianceHostile> AllianceHostile1
        {
            get { return _allianceHostile1; }
            set { _allianceHostile1 = value; }
        }

        [Association(Storage = "_characterTemplate", OtherKey = "AllianceID", ThisKey = "ID", Name = "character_template_ibfk_2")]
        [DebuggerNonUserCode]
        public EntitySet<CharacterTemplate> CharacterTemplate
        {
            get { return _characterTemplate; }
            set { _characterTemplate = value; }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_id", Name = "id", DbType = "tinyint(3) unsigned", IsPrimaryKey = true, CanBeNull = false)]
        public byte ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_name", Name = "name", DbType = "varchar(255)", CanBeNull = false)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public Alliance()
        {
            _allianceAttackable = new EntitySet<AllianceAttackable>(AllianceAttackable_Attach, AllianceAttackable_Detach);
            _allianceAttackable1 = new EntitySet<AllianceAttackable>(AllianceAttackable1_Attach, AllianceAttackable1_Detach);
            _allianceHostile = new EntitySet<AllianceHostile>(AllianceHostile_Attach, AllianceHostile_Detach);
            _allianceHostile1 = new EntitySet<AllianceHostile>(AllianceHostile1_Attach, AllianceHostile1_Detach);
            _characterTemplate = new EntitySet<CharacterTemplate>(CharacterTemplate_Attach, CharacterTemplate_Detach);
        }

        void AllianceAttackable_Attach(AllianceAttackable entity)
        {
            entity.Alliance = this;
        }

        void AllianceAttackable_Detach(AllianceAttackable entity)
        {
            entity.Alliance = null;
        }

        void AllianceAttackable1_Attach(AllianceAttackable entity)
        {
            entity.Alliance1 = this;
        }

        void AllianceAttackable1_Detach(AllianceAttackable entity)
        {
            entity.Alliance1 = null;
        }

        void AllianceHostile_Attach(AllianceHostile entity)
        {
            entity.Alliance = this;
        }

        void AllianceHostile_Detach(AllianceHostile entity)
        {
            entity.Alliance = null;
        }

        void AllianceHostile1_Attach(AllianceHostile entity)
        {
            entity.Alliance1 = this;
        }

        void AllianceHostile1_Detach(AllianceHostile entity)
        {
            entity.Alliance1 = null;
        }

        void CharacterTemplate_Attach(CharacterTemplate entity)
        {
            entity.Alliance = this;
        }

        void CharacterTemplate_Detach(CharacterTemplate entity)
        {
            entity.Alliance = null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.alliance_attackable")]
    public class AllianceAttackable : INotifyPropertyChanged
    {
        EntityRef<Alliance> _alliance;

        EntityRef<Alliance> _alliance1;
        byte _allianceID;
        byte _attackableID;

        [Association(Storage = "_alliance", OtherKey = "ID", ThisKey = "AttackableID", Name = "alliance_attackable_ibfk_3",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public Alliance Alliance
        {
            get { return _alliance.Entity; }
            set
            {
                if (value != _alliance.Entity)
                {
                    if (_alliance.Entity != null)
                    {
                        Alliance previousAlliance = _alliance.Entity;
                        _alliance.Entity = null;
                        previousAlliance.AllianceAttackable.Remove(this);
                    }
                    _alliance.Entity = value;
                    if (value != null)
                    {
                        value.AllianceAttackable.Add(this);
                        _attackableID = value.ID;
                    }
                    else
                        _attackableID = default(byte);
                }
            }
        }

        [Association(Storage = "_alliance1", OtherKey = "ID", ThisKey = "AllianceID", Name = "alliance_attackable_ibfk_4",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public Alliance Alliance1
        {
            get { return _alliance1.Entity; }
            set
            {
                if (value != _alliance1.Entity)
                {
                    if (_alliance1.Entity != null)
                    {
                        Alliance previousAlliance = _alliance1.Entity;
                        _alliance1.Entity = null;
                        previousAlliance.AllianceAttackable1.Remove(this);
                    }
                    _alliance1.Entity = value;
                    if (value != null)
                    {
                        value.AllianceAttackable1.Add(this);
                        _allianceID = value.ID;
                    }
                    else
                        _allianceID = default(byte);
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_allianceID", Name = "alliance_id", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte AllianceID
        {
            get { return _allianceID; }
            set
            {
                if (value != _allianceID)
                {
                    _allianceID = value;
                    OnPropertyChanged("AllianceID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_attackableID", Name = "attackable_id", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte AttackableID
        {
            get { return _attackableID; }
            set
            {
                if (value != _attackableID)
                {
                    _attackableID = value;
                    OnPropertyChanged("AttackableID");
                }
            }
        }

        public AllianceAttackable()
        {
            _alliance = new EntityRef<Alliance>();
            _alliance1 = new EntityRef<Alliance>();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.alliance_hostile")]
    public class AllianceHostile : INotifyPropertyChanged
    {
        EntityRef<Alliance> _alliance;

        EntityRef<Alliance> _alliance1;
        byte _allianceID;
        byte _hostileID;

        [Association(Storage = "_alliance", OtherKey = "ID", ThisKey = "HostileID", Name = "alliance_hostile_ibfk_3",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public Alliance Alliance
        {
            get { return _alliance.Entity; }
            set
            {
                if (value != _alliance.Entity)
                {
                    if (_alliance.Entity != null)
                    {
                        Alliance previousAlliance = _alliance.Entity;
                        _alliance.Entity = null;
                        previousAlliance.AllianceHostile.Remove(this);
                    }
                    _alliance.Entity = value;
                    if (value != null)
                    {
                        value.AllianceHostile.Add(this);
                        _hostileID = value.ID;
                    }
                    else
                        _hostileID = default(byte);
                }
            }
        }

        [Association(Storage = "_alliance1", OtherKey = "ID", ThisKey = "AllianceID", Name = "alliance_hostile_ibfk_4",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public Alliance Alliance1
        {
            get { return _alliance1.Entity; }
            set
            {
                if (value != _alliance1.Entity)
                {
                    if (_alliance1.Entity != null)
                    {
                        Alliance previousAlliance = _alliance1.Entity;
                        _alliance1.Entity = null;
                        previousAlliance.AllianceHostile1.Remove(this);
                    }
                    _alliance1.Entity = value;
                    if (value != null)
                    {
                        value.AllianceHostile1.Add(this);
                        _allianceID = value.ID;
                    }
                    else
                        _allianceID = default(byte);
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_allianceID", Name = "alliance_id", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte AllianceID
        {
            get { return _allianceID; }
            set
            {
                if (value != _allianceID)
                {
                    _allianceID = value;
                    OnPropertyChanged("AllianceID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_hostileID", Name = "hostile_id", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte HostileID
        {
            get { return _hostileID; }
            set
            {
                if (value != _hostileID)
                {
                    _hostileID = value;
                    OnPropertyChanged("HostileID");
                }
            }
        }

        public AllianceHostile()
        {
            _alliance = new EntityRef<Alliance>();
            _alliance1 = new EntityRef<Alliance>();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.character")]
    public class Character : INotifyPropertyChanged
    {
        byte _acC;

        byte _agI;

        byte _armor;

        ushort _body;

        byte _bra;

        uint _cash;
        EntitySet<CharacterEquipped> _characterEquipped;

        EntitySet<CharacterInventory> _characterInventory;
        EntityRef<CharacterTemplate> _characterTemplate;

        byte _defence;

        byte _deX;

        byte _evade;

        uint _exP;

        ushort _hp;

        uint _id;

        byte _imM;

        byte _int;

        byte _level;

        ushort _map;
        EntityRef<Map> _mapMap;

        EntityRef<Map> _mapMap1;

        byte _maXhIt;

        ushort _maXhP;

        ushort _maXmP;

        byte _miNHit;

        ushort _mp;

        string _name;

        string _password;

        byte _perC;

        byte _recOv;

        byte _reGen;

        ushort? _reSpawnMap;

        float _reSpawnX;

        float _reSpawnY;

        uint _statPoints;

        byte _str;

        byte _tact;

        ushort? _templateID;

        byte _ws;

        float _x;

        float _y;

        [DebuggerNonUserCode]
        [Column(Storage = "_acC", Name = "acc", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte AcC
        {
            get { return _acC; }
            set
            {
                if (value != _acC)
                {
                    _acC = value;
                    OnPropertyChanged("AcC");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_agI", Name = "agi", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte AgI
        {
            get { return _agI; }
            set
            {
                if (value != _agI)
                {
                    _agI = value;
                    OnPropertyChanged("AgI");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_armor", Name = "armor", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Armor
        {
            get { return _armor; }
            set
            {
                if (value != _armor)
                {
                    _armor = value;
                    OnPropertyChanged("Armor");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_body", Name = "body", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Body
        {
            get { return _body; }
            set
            {
                if (value != _body)
                {
                    _body = value;
                    OnPropertyChanged("Body");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_bra", Name = "bra", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Bra
        {
            get { return _bra; }
            set
            {
                if (value != _bra)
                {
                    _bra = value;
                    OnPropertyChanged("Bra");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_cash", Name = "cash", DbType = "int unsigned", CanBeNull = false)]
        public uint Cash
        {
            get { return _cash; }
            set
            {
                if (value != _cash)
                {
                    _cash = value;
                    OnPropertyChanged("Cash");
                }
            }
        }

        [Association(Storage = "_characterEquipped", OtherKey = "CharacterID", ThisKey = "ID", Name = "character_equipped_ibfk_4")
        ]
        [DebuggerNonUserCode]
        public EntitySet<CharacterEquipped> CharacterEquipped
        {
            get { return _characterEquipped; }
            set { _characterEquipped = value; }
        }

        [Association(Storage = "_characterInventory", OtherKey = "CharacterID", ThisKey = "ID",
            Name = "character_inventory_ibfk_4")]
        [DebuggerNonUserCode]
        public EntitySet<CharacterInventory> CharacterInventory
        {
            get { return _characterInventory; }
            set { _characterInventory = value; }
        }

        [Association(Storage = "_characterTemplate", OtherKey = "ID", ThisKey = "TemplateID", Name = "character_ibfk_1",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public CharacterTemplate CharacterTemplate
        {
            get { return _characterTemplate.Entity; }
            set
            {
                if (value != _characterTemplate.Entity)
                {
                    if (_characterTemplate.Entity != null)
                    {
                        CharacterTemplate previousCharacterTemplate = _characterTemplate.Entity;
                        _characterTemplate.Entity = null;
                        previousCharacterTemplate.Character.Remove(this);
                    }
                    _characterTemplate.Entity = value;
                    if (value != null)
                    {
                        value.Character.Add(this);
                        _templateID = value.ID;
                    }
                    else
                        _templateID = null;
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_defence", Name = "defence", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Defence
        {
            get { return _defence; }
            set
            {
                if (value != _defence)
                {
                    _defence = value;
                    OnPropertyChanged("Defence");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_deX", Name = "dex", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte DEx
        {
            get { return _deX; }
            set
            {
                if (value != _deX)
                {
                    _deX = value;
                    OnPropertyChanged("DEx");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_evade", Name = "evade", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Evade
        {
            get { return _evade; }
            set
            {
                if (value != _evade)
                {
                    _evade = value;
                    OnPropertyChanged("Evade");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_exP", Name = "exp", DbType = "int unsigned", CanBeNull = false)]
        public uint ExP
        {
            get { return _exP; }
            set
            {
                if (value != _exP)
                {
                    _exP = value;
                    OnPropertyChanged("ExP");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_hp", Name = "hp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Hp
        {
            get { return _hp; }
            set
            {
                if (value != _hp)
                {
                    _hp = value;
                    OnPropertyChanged("Hp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_id", Name = "id", DbType = "int unsigned", IsPrimaryKey = true, CanBeNull = false)]
        public uint ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_imM", Name = "imm", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte IMm
        {
            get { return _imM; }
            set
            {
                if (value != _imM)
                {
                    _imM = value;
                    OnPropertyChanged("IMm");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_int", Name = "`int`", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Int
        {
            get { return _int; }
            set
            {
                if (value != _int)
                {
                    _int = value;
                    OnPropertyChanged("Int");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_level", Name = "level", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Level
        {
            get { return _level; }
            set
            {
                if (value != _level)
                {
                    _level = value;
                    OnPropertyChanged("Level");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_map", Name = "map", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Map
        {
            get { return _map; }
            set
            {
                if (value != _map)
                {
                    _map = value;
                    OnPropertyChanged("Map");
                }
            }
        }

        [Association(Storage = "_mapMap", OtherKey = "ID", ThisKey = "Map", Name = "character_ibfk_2", IsForeignKey = true)]
        [DebuggerNonUserCode]
        public Map MapMap
        {
            get { return _mapMap.Entity; }
            set
            {
                if (value != _mapMap.Entity)
                {
                    if (_mapMap.Entity != null)
                    {
                        Map previousMap = _mapMap.Entity;
                        _mapMap.Entity = null;
                        previousMap.Character.Remove(this);
                    }
                    _mapMap.Entity = value;
                    if (value != null)
                    {
                        value.Character.Add(this);
                        _map = value.ID;
                    }
                    else
                        _map = default(ushort);
                }
            }
        }

        [Association(Storage = "_mapMap1", OtherKey = "ID", ThisKey = "ReSpawnMap", Name = "character_ibfk_3", IsForeignKey = true
            )]
        [DebuggerNonUserCode]
        public Map MapMap1
        {
            get { return _mapMap1.Entity; }
            set
            {
                if (value != _mapMap1.Entity)
                {
                    if (_mapMap1.Entity != null)
                    {
                        Map previousMap = _mapMap1.Entity;
                        _mapMap1.Entity = null;
                        previousMap.Character1.Remove(this);
                    }
                    _mapMap1.Entity = value;
                    if (value != null)
                    {
                        value.Character1.Add(this);
                        _reSpawnMap = value.ID;
                    }
                    else
                        _reSpawnMap = null;
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXhIt", Name = "maxhit", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte MaXHit
        {
            get { return _maXhIt; }
            set
            {
                if (value != _maXhIt)
                {
                    _maXhIt = value;
                    OnPropertyChanged("MaXHit");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXhP", Name = "maxhp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MaXHp
        {
            get { return _maXhP; }
            set
            {
                if (value != _maXhP)
                {
                    _maXhP = value;
                    OnPropertyChanged("MaXHp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXmP", Name = "maxmp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MaXMp
        {
            get { return _maXmP; }
            set
            {
                if (value != _maXmP)
                {
                    _maXmP = value;
                    OnPropertyChanged("MaXMp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_miNHit", Name = "minhit", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte MInHit
        {
            get { return _miNHit; }
            set
            {
                if (value != _miNHit)
                {
                    _miNHit = value;
                    OnPropertyChanged("MInHit");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_mp", Name = "mp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Mp
        {
            get { return _mp; }
            set
            {
                if (value != _mp)
                {
                    _mp = value;
                    OnPropertyChanged("Mp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_name", Name = "name", DbType = "varchar(50)", CanBeNull = false)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_password", Name = "password", DbType = "varchar(50)", CanBeNull = false)]
        public string Password
        {
            get { return _password; }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_perC", Name = "perc", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte PerC
        {
            get { return _perC; }
            set
            {
                if (value != _perC)
                {
                    _perC = value;
                    OnPropertyChanged("PerC");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_recOv", Name = "recov", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte RecOV
        {
            get { return _recOv; }
            set
            {
                if (value != _recOv)
                {
                    _recOv = value;
                    OnPropertyChanged("RecOV");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reGen", Name = "regen", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReGen
        {
            get { return _reGen; }
            set
            {
                if (value != _reGen)
                {
                    _reGen = value;
                    OnPropertyChanged("ReGen");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reSpawnMap", Name = "respawn_map", DbType = "smallint(5) unsigned")]
        public ushort? ReSpawnMap
        {
            get { return _reSpawnMap; }
            set
            {
                if (value != _reSpawnMap)
                {
                    _reSpawnMap = value;
                    OnPropertyChanged("ReSpawnMap");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reSpawnX", Name = "respawn_x", DbType = "float", CanBeNull = false)]
        public float ReSpawnX
        {
            get { return _reSpawnX; }
            set
            {
                if (value != _reSpawnX)
                {
                    _reSpawnX = value;
                    OnPropertyChanged("ReSpawnX");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reSpawnY", Name = "respawn_y", DbType = "float", CanBeNull = false)]
        public float ReSpawnY
        {
            get { return _reSpawnY; }
            set
            {
                if (value != _reSpawnY)
                {
                    _reSpawnY = value;
                    OnPropertyChanged("ReSpawnY");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_statPoints", Name = "statpoints", DbType = "int unsigned", CanBeNull = false)]
        public uint StatPoints
        {
            get { return _statPoints; }
            set
            {
                if (value != _statPoints)
                {
                    _statPoints = value;
                    OnPropertyChanged("StatPoints");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_str", Name = "str", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte STR
        {
            get { return _str; }
            set
            {
                if (value != _str)
                {
                    _str = value;
                    OnPropertyChanged("STR");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_tact", Name = "tact", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Tact
        {
            get { return _tact; }
            set
            {
                if (value != _tact)
                {
                    _tact = value;
                    OnPropertyChanged("Tact");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_templateID", Name = "template_id", DbType = "smallint(5) unsigned")]
        public ushort? TemplateID
        {
            get { return _templateID; }
            set
            {
                if (value != _templateID)
                {
                    _templateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_ws", Name = "ws", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte WS
        {
            get { return _ws; }
            set
            {
                if (value != _ws)
                {
                    _ws = value;
                    OnPropertyChanged("WS");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_x", Name = "x", DbType = "float", CanBeNull = false)]
        public float X
        {
            get { return _x; }
            set
            {
                if (value != _x)
                {
                    _x = value;
                    OnPropertyChanged("X");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_y", Name = "y", DbType = "float", CanBeNull = false)]
        public float Y
        {
            get { return _y; }
            set
            {
                if (value != _y)
                {
                    _y = value;
                    OnPropertyChanged("Y");
                }
            }
        }

        public Character()
        {
            _characterEquipped = new EntitySet<CharacterEquipped>(CharacterEquipped_Attach, CharacterEquipped_Detach);
            _characterInventory = new EntitySet<CharacterInventory>(CharacterInventory_Attach, CharacterInventory_Detach);
            _characterTemplate = new EntityRef<CharacterTemplate>();
            _mapMap = new EntityRef<Map>();
            _mapMap1 = new EntityRef<Map>();
        }

        void CharacterEquipped_Attach(CharacterEquipped entity)
        {
            entity.Character = this;
        }

        void CharacterEquipped_Detach(CharacterEquipped entity)
        {
            entity.Character = null;
        }

        void CharacterInventory_Attach(CharacterInventory entity)
        {
            entity.Character = this;
        }

        void CharacterInventory_Detach(CharacterInventory entity)
        {
            entity.Character = null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.character_equipped")]
    public class CharacterEquipped : INotifyPropertyChanged
    {
        EntityRef<Character> _character;
        uint _characterID;
        EntityRef<Item> _item;

        uint _itemID;

        byte _slot;

        [Association(Storage = "_character", OtherKey = "ID", ThisKey = "CharacterID", Name = "character_equipped_ibfk_4",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public Character Character
        {
            get { return _character.Entity; }
            set
            {
                if (value != _character.Entity)
                {
                    if (_character.Entity != null)
                    {
                        Character previousCharacter = _character.Entity;
                        _character.Entity = null;
                        previousCharacter.CharacterEquipped.Remove(this);
                    }
                    _character.Entity = value;
                    if (value != null)
                    {
                        value.CharacterEquipped.Add(this);
                        _characterID = value.ID;
                    }
                    else
                        _characterID = default(uint);
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_characterID", Name = "character_id", DbType = "int unsigned", IsPrimaryKey = true, CanBeNull = false)]
        public uint CharacterID
        {
            get { return _characterID; }
            set
            {
                if (value != _characterID)
                {
                    _characterID = value;
                    OnPropertyChanged("CharacterID");
                }
            }
        }

        [Association(Storage = "_item", OtherKey = "ID", ThisKey = "ItemID", Name = "character_equipped_ibfk_3",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public Item Item
        {
            get { return _item.Entity; }
            set
            {
                if (value != _item.Entity)
                {
                    if (_item.Entity != null)
                    {
                        Item previousItem = _item.Entity;
                        _item.Entity = null;
                        previousItem.CharacterEquipped.Remove(this);
                    }
                    _item.Entity = value;
                    if (value != null)
                    {
                        value.CharacterEquipped.Add(this);
                        _itemID = value.ID;
                    }
                    else
                        _itemID = default(uint);
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_itemID", Name = "item_id", DbType = "int unsigned", IsPrimaryKey = true, CanBeNull = false)]
        public uint ItemID
        {
            get { return _itemID; }
            set
            {
                if (value != _itemID)
                {
                    _itemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_slot", Name = "slot", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Slot
        {
            get { return _slot; }
            set
            {
                if (value != _slot)
                {
                    _slot = value;
                    OnPropertyChanged("Slot");
                }
            }
        }

        public CharacterEquipped()
        {
            _item = new EntityRef<Item>();
            _character = new EntityRef<Character>();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.character_inventory")]
    public class CharacterInventory : INotifyPropertyChanged
    {
        EntityRef<Character> _character;
        uint _characterID;
        EntityRef<Item> _item;
        uint _itemID;

        [Association(Storage = "_character", OtherKey = "ID", ThisKey = "CharacterID", Name = "character_inventory_ibfk_4",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public Character Character
        {
            get { return _character.Entity; }
            set
            {
                if (value != _character.Entity)
                {
                    if (_character.Entity != null)
                    {
                        Character previousCharacter = _character.Entity;
                        _character.Entity = null;
                        previousCharacter.CharacterInventory.Remove(this);
                    }
                    _character.Entity = value;
                    if (value != null)
                    {
                        value.CharacterInventory.Add(this);
                        _characterID = value.ID;
                    }
                    else
                        _characterID = default(uint);
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_characterID", Name = "character_id", DbType = "int unsigned", IsPrimaryKey = true, CanBeNull = false)]
        public uint CharacterID
        {
            get { return _characterID; }
            set
            {
                if (value != _characterID)
                {
                    _characterID = value;
                    OnPropertyChanged("CharacterID");
                }
            }
        }

        [Association(Storage = "_item", OtherKey = "ID", ThisKey = "ItemID", Name = "character_inventory_ibfk_3",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public Item Item
        {
            get { return _item.Entity; }
            set
            {
                if (value != _item.Entity)
                {
                    if (_item.Entity != null)
                    {
                        Item previousItem = _item.Entity;
                        _item.Entity = null;
                        previousItem.CharacterInventory.Remove(this);
                    }
                    _item.Entity = value;
                    if (value != null)
                    {
                        value.CharacterInventory.Add(this);
                        _itemID = value.ID;
                    }
                    else
                        _itemID = default(uint);
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_itemID", Name = "item_id", DbType = "int unsigned", IsPrimaryKey = true, CanBeNull = false)]
        public uint ItemID
        {
            get { return _itemID; }
            set
            {
                if (value != _itemID)
                {
                    _itemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        public CharacterInventory()
        {
            _item = new EntityRef<Item>();
            _character = new EntityRef<Character>();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.character_template")]
    public class CharacterTemplate : INotifyPropertyChanged
    {
        byte _acC;

        byte _agI;

        string _ai;
        EntityRef<Alliance> _alliance;

        byte _allianceID;

        byte _armor;

        ushort _body;

        byte _bra;
        EntitySet<Character> _character;

        EntitySet<CharacterTemplateEquipped> _characterTemplateEquipped;

        EntitySet<CharacterTemplateInventory> _characterTemplateInventory;

        byte _defence;

        byte _deX;

        byte _evade;

        uint _exP;

        ushort _giveCash;

        ushort _giveExP;

        ushort _id;

        byte _imM;

        byte _int;

        byte _level;
        EntitySet<MapSpawn> _mapSpawn;

        byte _maXhIt;

        ushort _maXhP;

        ushort _maXmP;

        byte _miNHit;

        string _name;

        byte _perC;

        byte _recOv;

        byte _reGen;

        ushort _reSpawn;

        uint _statPoints;

        byte _str;

        byte _tact;

        byte _ws;

        [DebuggerNonUserCode]
        [Column(Storage = "_acC", Name = "acc", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte AcC
        {
            get { return _acC; }
            set
            {
                if (value != _acC)
                {
                    _acC = value;
                    OnPropertyChanged("AcC");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_agI", Name = "agi", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte AgI
        {
            get { return _agI; }
            set
            {
                if (value != _agI)
                {
                    _agI = value;
                    OnPropertyChanged("AgI");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_ai", Name = "ai", DbType = "varchar(255)", CanBeNull = false)]
        public string Ai
        {
            get { return _ai; }
            set
            {
                if (value != _ai)
                {
                    _ai = value;
                    OnPropertyChanged("Ai");
                }
            }
        }

        [Association(Storage = "_alliance", OtherKey = "ID", ThisKey = "AllianceID", Name = "character_template_ibfk_2",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public Alliance Alliance
        {
            get { return _alliance.Entity; }
            set
            {
                if (value != _alliance.Entity)
                {
                    if (_alliance.Entity != null)
                    {
                        Alliance previousAlliance = _alliance.Entity;
                        _alliance.Entity = null;
                        previousAlliance.CharacterTemplate.Remove(this);
                    }
                    _alliance.Entity = value;
                    if (value != null)
                    {
                        value.CharacterTemplate.Add(this);
                        _allianceID = value.ID;
                    }
                    else
                        _allianceID = default(byte);
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_allianceID", Name = "alliance_id", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte AllianceID
        {
            get { return _allianceID; }
            set
            {
                if (value != _allianceID)
                {
                    _allianceID = value;
                    OnPropertyChanged("AllianceID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_armor", Name = "armor", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Armor
        {
            get { return _armor; }
            set
            {
                if (value != _armor)
                {
                    _armor = value;
                    OnPropertyChanged("Armor");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_body", Name = "body", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Body
        {
            get { return _body; }
            set
            {
                if (value != _body)
                {
                    _body = value;
                    OnPropertyChanged("Body");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_bra", Name = "bra", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Bra
        {
            get { return _bra; }
            set
            {
                if (value != _bra)
                {
                    _bra = value;
                    OnPropertyChanged("Bra");
                }
            }
        }

        [Association(Storage = "_character", OtherKey = "TemplateID", ThisKey = "ID", Name = "character_ibfk_1")]
        [DebuggerNonUserCode]
        public EntitySet<Character> Character
        {
            get { return _character; }
            set { _character = value; }
        }

        [Association(Storage = "_characterTemplateEquipped", OtherKey = "CharacterID", ThisKey = "ID",
            Name = "character_template_equipped_ibfk_4")]
        [DebuggerNonUserCode]
        public EntitySet<CharacterTemplateEquipped> CharacterTemplateEquipped
        {
            get { return _characterTemplateEquipped; }
            set { _characterTemplateEquipped = value; }
        }

        [Association(Storage = "_characterTemplateInventory", OtherKey = "CharacterID", ThisKey = "ID",
            Name = "character_template_inventory_ibfk_1")]
        [DebuggerNonUserCode]
        public EntitySet<CharacterTemplateInventory> CharacterTemplateInventory
        {
            get { return _characterTemplateInventory; }
            set { _characterTemplateInventory = value; }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_defence", Name = "defence", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Defence
        {
            get { return _defence; }
            set
            {
                if (value != _defence)
                {
                    _defence = value;
                    OnPropertyChanged("Defence");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_deX", Name = "dex", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte DEx
        {
            get { return _deX; }
            set
            {
                if (value != _deX)
                {
                    _deX = value;
                    OnPropertyChanged("DEx");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_evade", Name = "evade", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Evade
        {
            get { return _evade; }
            set
            {
                if (value != _evade)
                {
                    _evade = value;
                    OnPropertyChanged("Evade");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_exP", Name = "exp", DbType = "int unsigned", CanBeNull = false)]
        public uint ExP
        {
            get { return _exP; }
            set
            {
                if (value != _exP)
                {
                    _exP = value;
                    OnPropertyChanged("ExP");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_giveCash", Name = "give_cash", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort GiveCash
        {
            get { return _giveCash; }
            set
            {
                if (value != _giveCash)
                {
                    _giveCash = value;
                    OnPropertyChanged("GiveCash");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_giveExP", Name = "give_exp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort GiveExP
        {
            get { return _giveExP; }
            set
            {
                if (value != _giveExP)
                {
                    _giveExP = value;
                    OnPropertyChanged("GiveExP");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_id", Name = "id", DbType = "smallint(5) unsigned", IsPrimaryKey = true, CanBeNull = false)]
        public ushort ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_imM", Name = "imm", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte IMm
        {
            get { return _imM; }
            set
            {
                if (value != _imM)
                {
                    _imM = value;
                    OnPropertyChanged("IMm");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_int", Name = "`int`", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Int
        {
            get { return _int; }
            set
            {
                if (value != _int)
                {
                    _int = value;
                    OnPropertyChanged("Int");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_level", Name = "level", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Level
        {
            get { return _level; }
            set
            {
                if (value != _level)
                {
                    _level = value;
                    OnPropertyChanged("Level");
                }
            }
        }

        [Association(Storage = "_mapSpawn", OtherKey = "CharacterID", ThisKey = "ID", Name = "map_spawn_ibfk_1")]
        [DebuggerNonUserCode]
        public EntitySet<MapSpawn> MapSpawn
        {
            get { return _mapSpawn; }
            set { _mapSpawn = value; }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXhIt", Name = "maxhit", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte MaXHit
        {
            get { return _maXhIt; }
            set
            {
                if (value != _maXhIt)
                {
                    _maXhIt = value;
                    OnPropertyChanged("MaXHit");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXhP", Name = "maxhp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MaXHp
        {
            get { return _maXhP; }
            set
            {
                if (value != _maXhP)
                {
                    _maXhP = value;
                    OnPropertyChanged("MaXHp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXmP", Name = "maxmp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MaXMp
        {
            get { return _maXmP; }
            set
            {
                if (value != _maXmP)
                {
                    _maXmP = value;
                    OnPropertyChanged("MaXMp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_miNHit", Name = "minhit", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte MInHit
        {
            get { return _miNHit; }
            set
            {
                if (value != _miNHit)
                {
                    _miNHit = value;
                    OnPropertyChanged("MInHit");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_name", Name = "name", DbType = "varchar(50)", CanBeNull = false)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_perC", Name = "perc", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte PerC
        {
            get { return _perC; }
            set
            {
                if (value != _perC)
                {
                    _perC = value;
                    OnPropertyChanged("PerC");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_recOv", Name = "recov", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte RecOV
        {
            get { return _recOv; }
            set
            {
                if (value != _recOv)
                {
                    _recOv = value;
                    OnPropertyChanged("RecOV");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reGen", Name = "regen", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReGen
        {
            get { return _reGen; }
            set
            {
                if (value != _reGen)
                {
                    _reGen = value;
                    OnPropertyChanged("ReGen");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reSpawn", Name = "respawn", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort ReSpawn
        {
            get { return _reSpawn; }
            set
            {
                if (value != _reSpawn)
                {
                    _reSpawn = value;
                    OnPropertyChanged("ReSpawn");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_statPoints", Name = "statpoints", DbType = "int unsigned", CanBeNull = false)]
        public uint StatPoints
        {
            get { return _statPoints; }
            set
            {
                if (value != _statPoints)
                {
                    _statPoints = value;
                    OnPropertyChanged("StatPoints");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_str", Name = "str", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte STR
        {
            get { return _str; }
            set
            {
                if (value != _str)
                {
                    _str = value;
                    OnPropertyChanged("STR");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_tact", Name = "tact", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Tact
        {
            get { return _tact; }
            set
            {
                if (value != _tact)
                {
                    _tact = value;
                    OnPropertyChanged("Tact");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_ws", Name = "ws", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte WS
        {
            get { return _ws; }
            set
            {
                if (value != _ws)
                {
                    _ws = value;
                    OnPropertyChanged("WS");
                }
            }
        }

        public CharacterTemplate()
        {
            _character = new EntitySet<Character>(Character_Attach, Character_Detach);
            _characterTemplateEquipped = new EntitySet<CharacterTemplateEquipped>(CharacterTemplateEquipped_Attach,
                                                                                  CharacterTemplateEquipped_Detach);
            _characterTemplateInventory = new EntitySet<CharacterTemplateInventory>(CharacterTemplateInventory_Attach,
                                                                                    CharacterTemplateInventory_Detach);
            _mapSpawn = new EntitySet<MapSpawn>(MapSpawn_Attach, MapSpawn_Detach);
            _alliance = new EntityRef<Alliance>();
        }

        void Character_Attach(Character entity)
        {
            entity.CharacterTemplate = this;
        }

        void Character_Detach(Character entity)
        {
            entity.CharacterTemplate = null;
        }

        void CharacterTemplateEquipped_Attach(CharacterTemplateEquipped entity)
        {
            entity.CharacterTemplate = this;
        }

        void CharacterTemplateEquipped_Detach(CharacterTemplateEquipped entity)
        {
            entity.CharacterTemplate = null;
        }

        void CharacterTemplateInventory_Attach(CharacterTemplateInventory entity)
        {
            entity.CharacterTemplate = this;
        }

        void CharacterTemplateInventory_Detach(CharacterTemplateInventory entity)
        {
            entity.CharacterTemplate = null;
        }

        void MapSpawn_Attach(MapSpawn entity)
        {
            entity.CharacterTemplate = this;
        }

        void MapSpawn_Detach(MapSpawn entity)
        {
            entity.CharacterTemplate = null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.character_template_equipped")]
    public class CharacterTemplateEquipped : INotifyPropertyChanged
    {
        ushort _chance;

        ushort _characterID;

        EntityRef<CharacterTemplate> _characterTemplate;
        ushort _itemID;
        EntityRef<ItemTemplate> _itemTemplate;

        [DebuggerNonUserCode]
        [Column(Storage = "_chance", Name = "chance", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Chance
        {
            get { return _chance; }
            set
            {
                if (value != _chance)
                {
                    _chance = value;
                    OnPropertyChanged("Chance");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_characterID", Name = "character_id", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort CharacterID
        {
            get { return _characterID; }
            set
            {
                if (value != _characterID)
                {
                    _characterID = value;
                    OnPropertyChanged("CharacterID");
                }
            }
        }

        [Association(Storage = "_characterTemplate", OtherKey = "ID", ThisKey = "CharacterID",
            Name = "character_template_equipped_ibfk_4", IsForeignKey = true)]
        [DebuggerNonUserCode]
        public CharacterTemplate CharacterTemplate
        {
            get { return _characterTemplate.Entity; }
            set
            {
                if (value != _characterTemplate.Entity)
                {
                    if (_characterTemplate.Entity != null)
                    {
                        CharacterTemplate previousCharacterTemplate = _characterTemplate.Entity;
                        _characterTemplate.Entity = null;
                        previousCharacterTemplate.CharacterTemplateEquipped.Remove(this);
                    }
                    _characterTemplate.Entity = value;
                    if (value != null)
                    {
                        value.CharacterTemplateEquipped.Add(this);
                        _characterID = value.ID;
                    }
                    else
                        _characterID = default(ushort);
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_itemID", Name = "item_id", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort ItemID
        {
            get { return _itemID; }
            set
            {
                if (value != _itemID)
                {
                    _itemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        [Association(Storage = "_itemTemplate", OtherKey = "ID", ThisKey = "ItemID", Name = "character_template_equipped_ibfk_3",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public ItemTemplate ItemTemplate
        {
            get { return _itemTemplate.Entity; }
            set
            {
                if (value != _itemTemplate.Entity)
                {
                    if (_itemTemplate.Entity != null)
                    {
                        ItemTemplate previousItemTemplate = _itemTemplate.Entity;
                        _itemTemplate.Entity = null;
                        previousItemTemplate.CharacterTemplateEquipped.Remove(this);
                    }
                    _itemTemplate.Entity = value;
                    if (value != null)
                    {
                        value.CharacterTemplateEquipped.Add(this);
                        _itemID = value.ID;
                    }
                    else
                        _itemID = default(ushort);
                }
            }
        }

        public CharacterTemplateEquipped()
        {
            _itemTemplate = new EntityRef<ItemTemplate>();
            _characterTemplate = new EntityRef<CharacterTemplate>();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.character_template_inventory")]
    public class CharacterTemplateInventory : INotifyPropertyChanged
    {
        ushort _chance;

        ushort _characterID;
        EntityRef<CharacterTemplate> _characterTemplate;

        ushort _itemID;
        EntityRef<ItemTemplate> _itemTemplate;

        byte _maX;

        byte _miN;

        [DebuggerNonUserCode]
        [Column(Storage = "_chance", Name = "chance", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Chance
        {
            get { return _chance; }
            set
            {
                if (value != _chance)
                {
                    _chance = value;
                    OnPropertyChanged("Chance");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_characterID", Name = "character_id", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort CharacterID
        {
            get { return _characterID; }
            set
            {
                if (value != _characterID)
                {
                    _characterID = value;
                    OnPropertyChanged("CharacterID");
                }
            }
        }

        [Association(Storage = "_characterTemplate", OtherKey = "ID", ThisKey = "CharacterID",
            Name = "character_template_inventory_ibfk_1", IsForeignKey = true)]
        [DebuggerNonUserCode]
        public CharacterTemplate CharacterTemplate
        {
            get { return _characterTemplate.Entity; }
            set
            {
                if (value != _characterTemplate.Entity)
                {
                    if (_characterTemplate.Entity != null)
                    {
                        CharacterTemplate previousCharacterTemplate = _characterTemplate.Entity;
                        _characterTemplate.Entity = null;
                        previousCharacterTemplate.CharacterTemplateInventory.Remove(this);
                    }
                    _characterTemplate.Entity = value;
                    if (value != null)
                    {
                        value.CharacterTemplateInventory.Add(this);
                        _characterID = value.ID;
                    }
                    else
                        _characterID = default(ushort);
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_itemID", Name = "item_id", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort ItemID
        {
            get { return _itemID; }
            set
            {
                if (value != _itemID)
                {
                    _itemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        [Association(Storage = "_itemTemplate", OtherKey = "ID", ThisKey = "ItemID", Name = "character_template_inventory_ibfk_2",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public ItemTemplate ItemTemplate
        {
            get { return _itemTemplate.Entity; }
            set
            {
                if (value != _itemTemplate.Entity)
                {
                    if (_itemTemplate.Entity != null)
                    {
                        ItemTemplate previousItemTemplate = _itemTemplate.Entity;
                        _itemTemplate.Entity = null;
                        previousItemTemplate.CharacterTemplateInventory.Remove(this);
                    }
                    _itemTemplate.Entity = value;
                    if (value != null)
                    {
                        value.CharacterTemplateInventory.Add(this);
                        _itemID = value.ID;
                    }
                    else
                        _itemID = default(ushort);
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maX", Name = "max", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte MaX
        {
            get { return _maX; }
            set
            {
                if (value != _maX)
                {
                    _maX = value;
                    OnPropertyChanged("MaX");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_miN", Name = "min", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte MIn
        {
            get { return _miN; }
            set
            {
                if (value != _miN)
                {
                    _miN = value;
                    OnPropertyChanged("MIn");
                }
            }
        }

        public CharacterTemplateInventory()
        {
            _characterTemplate = new EntityRef<CharacterTemplate>();
            _itemTemplate = new EntityRef<ItemTemplate>();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.item")]
    public class Item : INotifyPropertyChanged
    {
        ushort _agI;

        byte _amount;

        ushort _armor;

        ushort _bra;
        EntitySet<CharacterEquipped> _characterEquipped;

        EntitySet<CharacterInventory> _characterInventory;

        ushort _defence;

        string _description;

        ushort _deX;

        ushort _evade;

        ushort _graphic;

        byte _height;

        ushort _hp;

        uint _id;

        ushort _imM;

        ushort _int;

        ushort _maXhIt;

        ushort _maXhP;

        ushort _maXmP;

        ushort _miNHit;

        ushort _mp;

        string _name;

        ushort _perC;

        byte _reQaCC;

        byte _reQaGI;

        byte _reQaRmor;

        byte _reQbRa;

        byte _reQdeX;

        byte _reQeVade;

        byte _reQimM;

        byte _reQiNt;

        byte _type;

        int _value;

        byte _width;

        [DebuggerNonUserCode]
        [Column(Storage = "_agI", Name = "agi", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort AgI
        {
            get { return _agI; }
            set
            {
                if (value != _agI)
                {
                    _agI = value;
                    OnPropertyChanged("AgI");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_amount", Name = "amount", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Amount
        {
            get { return _amount; }
            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_armor", Name = "armor", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Armor
        {
            get { return _armor; }
            set
            {
                if (value != _armor)
                {
                    _armor = value;
                    OnPropertyChanged("Armor");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_bra", Name = "bra", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Bra
        {
            get { return _bra; }
            set
            {
                if (value != _bra)
                {
                    _bra = value;
                    OnPropertyChanged("Bra");
                }
            }
        }

        [Association(Storage = "_characterEquipped", OtherKey = "ItemID", ThisKey = "ID", Name = "character_equipped_ibfk_3")]
        [DebuggerNonUserCode]
        public EntitySet<CharacterEquipped> CharacterEquipped
        {
            get { return _characterEquipped; }
            set { _characterEquipped = value; }
        }

        [Association(Storage = "_characterInventory", OtherKey = "ItemID", ThisKey = "ID", Name = "character_inventory_ibfk_3")]
        [DebuggerNonUserCode]
        public EntitySet<CharacterInventory> CharacterInventory
        {
            get { return _characterInventory; }
            set { _characterInventory = value; }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_defence", Name = "defence", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Defence
        {
            get { return _defence; }
            set
            {
                if (value != _defence)
                {
                    _defence = value;
                    OnPropertyChanged("Defence");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_description", Name = "description", DbType = "varchar(255)", CanBeNull = false)]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_deX", Name = "dex", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort DEx
        {
            get { return _deX; }
            set
            {
                if (value != _deX)
                {
                    _deX = value;
                    OnPropertyChanged("DEx");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_evade", Name = "evade", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Evade
        {
            get { return _evade; }
            set
            {
                if (value != _evade)
                {
                    _evade = value;
                    OnPropertyChanged("Evade");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_graphic", Name = "graphic", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Graphic
        {
            get { return _graphic; }
            set
            {
                if (value != _graphic)
                {
                    _graphic = value;
                    OnPropertyChanged("Graphic");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_height", Name = "height", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Height
        {
            get { return _height; }
            set
            {
                if (value != _height)
                {
                    _height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_hp", Name = "hp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Hp
        {
            get { return _hp; }
            set
            {
                if (value != _hp)
                {
                    _hp = value;
                    OnPropertyChanged("Hp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_id", Name = "id", DbType = "int unsigned", IsPrimaryKey = true, CanBeNull = false)]
        public uint ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_imM", Name = "imm", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort IMm
        {
            get { return _imM; }
            set
            {
                if (value != _imM)
                {
                    _imM = value;
                    OnPropertyChanged("IMm");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_int", Name = "`int`", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Int
        {
            get { return _int; }
            set
            {
                if (value != _int)
                {
                    _int = value;
                    OnPropertyChanged("Int");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXhIt", Name = "maxhit", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MaXHit
        {
            get { return _maXhIt; }
            set
            {
                if (value != _maXhIt)
                {
                    _maXhIt = value;
                    OnPropertyChanged("MaXHit");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXhP", Name = "maxhp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MaXHp
        {
            get { return _maXhP; }
            set
            {
                if (value != _maXhP)
                {
                    _maXhP = value;
                    OnPropertyChanged("MaXHp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXmP", Name = "maxmp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MaXMp
        {
            get { return _maXmP; }
            set
            {
                if (value != _maXmP)
                {
                    _maXmP = value;
                    OnPropertyChanged("MaXMp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_miNHit", Name = "minhit", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MInHit
        {
            get { return _miNHit; }
            set
            {
                if (value != _miNHit)
                {
                    _miNHit = value;
                    OnPropertyChanged("MInHit");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_mp", Name = "mp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Mp
        {
            get { return _mp; }
            set
            {
                if (value != _mp)
                {
                    _mp = value;
                    OnPropertyChanged("Mp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_name", Name = "name", DbType = "varchar(255)", CanBeNull = false)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_perC", Name = "perc", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort PerC
        {
            get { return _perC; }
            set
            {
                if (value != _perC)
                {
                    _perC = value;
                    OnPropertyChanged("PerC");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQaCC", Name = "reqacc", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQAcC
        {
            get { return _reQaCC; }
            set
            {
                if (value != _reQaCC)
                {
                    _reQaCC = value;
                    OnPropertyChanged("ReQAcC");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQaGI", Name = "reqagi", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQAgI
        {
            get { return _reQaGI; }
            set
            {
                if (value != _reQaGI)
                {
                    _reQaGI = value;
                    OnPropertyChanged("ReQAgI");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQaRmor", Name = "reqarmor", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQArmor
        {
            get { return _reQaRmor; }
            set
            {
                if (value != _reQaRmor)
                {
                    _reQaRmor = value;
                    OnPropertyChanged("ReQArmor");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQbRa", Name = "reqbra", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQbRa
        {
            get { return _reQbRa; }
            set
            {
                if (value != _reQbRa)
                {
                    _reQbRa = value;
                    OnPropertyChanged("ReQbRa");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQdeX", Name = "reqdex", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQDEx
        {
            get { return _reQdeX; }
            set
            {
                if (value != _reQdeX)
                {
                    _reQdeX = value;
                    OnPropertyChanged("ReQDEx");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQeVade", Name = "reqevade", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQEvade
        {
            get { return _reQeVade; }
            set
            {
                if (value != _reQeVade)
                {
                    _reQeVade = value;
                    OnPropertyChanged("ReQEvade");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQimM", Name = "reqimm", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQIMm
        {
            get { return _reQimM; }
            set
            {
                if (value != _reQimM)
                {
                    _reQimM = value;
                    OnPropertyChanged("ReQIMm");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQiNt", Name = "reqint", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQInt
        {
            get { return _reQiNt; }
            set
            {
                if (value != _reQiNt)
                {
                    _reQiNt = value;
                    OnPropertyChanged("ReQInt");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_type", Name = "type", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_value", Name = "value", DbType = "int", CanBeNull = false)]
        public int Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_width", Name = "width", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Width
        {
            get { return _width; }
            set
            {
                if (value != _width)
                {
                    _width = value;
                    OnPropertyChanged("Width");
                }
            }
        }

        public Item()
        {
            _characterEquipped = new EntitySet<CharacterEquipped>(CharacterEquipped_Attach, CharacterEquipped_Detach);
            _characterInventory = new EntitySet<CharacterInventory>(CharacterInventory_Attach, CharacterInventory_Detach);
        }

        void CharacterEquipped_Attach(CharacterEquipped entity)
        {
            entity.Item = this;
        }

        void CharacterEquipped_Detach(CharacterEquipped entity)
        {
            entity.Item = null;
        }

        void CharacterInventory_Attach(CharacterInventory entity)
        {
            entity.Item = this;
        }

        void CharacterInventory_Detach(CharacterInventory entity)
        {
            entity.Item = null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.item_template")]
    public class ItemTemplate : INotifyPropertyChanged
    {
        ushort _agI;

        ushort _armor;

        ushort _bra;
        EntitySet<CharacterTemplateEquipped> _characterTemplateEquipped;

        EntitySet<CharacterTemplateInventory> _characterTemplateInventory;

        ushort _defence;

        string _description;

        ushort _deX;

        ushort _evade;

        ushort _graphic;

        byte _height;

        ushort _hp;

        ushort _id;

        ushort _imM;

        ushort _int;

        ushort _maXhIt;

        ushort _maXhP;

        ushort _maXmP;

        ushort _miNHit;

        ushort _mp;

        string _name;

        ushort _perC;

        byte _reQaCC;

        byte _reQaGI;

        byte _reQaRmor;

        byte _reQbRa;

        byte _reQdeX;

        byte _reQeVade;

        byte _reQimM;

        byte _reQiNt;

        byte _type;

        int _value;

        byte _width;

        [DebuggerNonUserCode]
        [Column(Storage = "_agI", Name = "agi", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort AgI
        {
            get { return _agI; }
            set
            {
                if (value != _agI)
                {
                    _agI = value;
                    OnPropertyChanged("AgI");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_armor", Name = "armor", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Armor
        {
            get { return _armor; }
            set
            {
                if (value != _armor)
                {
                    _armor = value;
                    OnPropertyChanged("Armor");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_bra", Name = "bra", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Bra
        {
            get { return _bra; }
            set
            {
                if (value != _bra)
                {
                    _bra = value;
                    OnPropertyChanged("Bra");
                }
            }
        }

        [Association(Storage = "_characterTemplateEquipped", OtherKey = "ItemID", ThisKey = "ID",
            Name = "character_template_equipped_ibfk_3")]
        [DebuggerNonUserCode]
        public EntitySet<CharacterTemplateEquipped> CharacterTemplateEquipped
        {
            get { return _characterTemplateEquipped; }
            set { _characterTemplateEquipped = value; }
        }

        [Association(Storage = "_characterTemplateInventory", OtherKey = "ItemID", ThisKey = "ID",
            Name = "character_template_inventory_ibfk_2")]
        [DebuggerNonUserCode]
        public EntitySet<CharacterTemplateInventory> CharacterTemplateInventory
        {
            get { return _characterTemplateInventory; }
            set { _characterTemplateInventory = value; }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_defence", Name = "defence", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Defence
        {
            get { return _defence; }
            set
            {
                if (value != _defence)
                {
                    _defence = value;
                    OnPropertyChanged("Defence");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_description", Name = "description", DbType = "varchar(255)", CanBeNull = false)]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_deX", Name = "dex", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort DEx
        {
            get { return _deX; }
            set
            {
                if (value != _deX)
                {
                    _deX = value;
                    OnPropertyChanged("DEx");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_evade", Name = "evade", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Evade
        {
            get { return _evade; }
            set
            {
                if (value != _evade)
                {
                    _evade = value;
                    OnPropertyChanged("Evade");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_graphic", Name = "graphic", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Graphic
        {
            get { return _graphic; }
            set
            {
                if (value != _graphic)
                {
                    _graphic = value;
                    OnPropertyChanged("Graphic");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_height", Name = "height", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Height
        {
            get { return _height; }
            set
            {
                if (value != _height)
                {
                    _height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_hp", Name = "hp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Hp
        {
            get { return _hp; }
            set
            {
                if (value != _hp)
                {
                    _hp = value;
                    OnPropertyChanged("Hp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_id", Name = "id", DbType = "smallint(5) unsigned", IsPrimaryKey = true, CanBeNull = false)]
        public ushort ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_imM", Name = "imm", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort IMm
        {
            get { return _imM; }
            set
            {
                if (value != _imM)
                {
                    _imM = value;
                    OnPropertyChanged("IMm");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_int", Name = "`int`", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Int
        {
            get { return _int; }
            set
            {
                if (value != _int)
                {
                    _int = value;
                    OnPropertyChanged("Int");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXhIt", Name = "maxhit", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MaXHit
        {
            get { return _maXhIt; }
            set
            {
                if (value != _maXhIt)
                {
                    _maXhIt = value;
                    OnPropertyChanged("MaXHit");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXhP", Name = "maxhp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MaXHp
        {
            get { return _maXhP; }
            set
            {
                if (value != _maXhP)
                {
                    _maXhP = value;
                    OnPropertyChanged("MaXHp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_maXmP", Name = "maxmp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MaXMp
        {
            get { return _maXmP; }
            set
            {
                if (value != _maXmP)
                {
                    _maXmP = value;
                    OnPropertyChanged("MaXMp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_miNHit", Name = "minhit", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MInHit
        {
            get { return _miNHit; }
            set
            {
                if (value != _miNHit)
                {
                    _miNHit = value;
                    OnPropertyChanged("MInHit");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_mp", Name = "mp", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort Mp
        {
            get { return _mp; }
            set
            {
                if (value != _mp)
                {
                    _mp = value;
                    OnPropertyChanged("Mp");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_name", Name = "name", DbType = "varchar(255)", CanBeNull = false)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_perC", Name = "perc", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort PerC
        {
            get { return _perC; }
            set
            {
                if (value != _perC)
                {
                    _perC = value;
                    OnPropertyChanged("PerC");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQaCC", Name = "reqacc", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQAcC
        {
            get { return _reQaCC; }
            set
            {
                if (value != _reQaCC)
                {
                    _reQaCC = value;
                    OnPropertyChanged("ReQAcC");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQaGI", Name = "reqagi", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQAgI
        {
            get { return _reQaGI; }
            set
            {
                if (value != _reQaGI)
                {
                    _reQaGI = value;
                    OnPropertyChanged("ReQAgI");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQaRmor", Name = "reqarmor", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQArmor
        {
            get { return _reQaRmor; }
            set
            {
                if (value != _reQaRmor)
                {
                    _reQaRmor = value;
                    OnPropertyChanged("ReQArmor");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQbRa", Name = "reqbra", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQbRa
        {
            get { return _reQbRa; }
            set
            {
                if (value != _reQbRa)
                {
                    _reQbRa = value;
                    OnPropertyChanged("ReQbRa");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQdeX", Name = "reqdex", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQDEx
        {
            get { return _reQdeX; }
            set
            {
                if (value != _reQdeX)
                {
                    _reQdeX = value;
                    OnPropertyChanged("ReQDEx");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQeVade", Name = "reqevade", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQEvade
        {
            get { return _reQeVade; }
            set
            {
                if (value != _reQeVade)
                {
                    _reQeVade = value;
                    OnPropertyChanged("ReQEvade");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQimM", Name = "reqimm", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQIMm
        {
            get { return _reQimM; }
            set
            {
                if (value != _reQimM)
                {
                    _reQimM = value;
                    OnPropertyChanged("ReQIMm");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_reQiNt", Name = "reqint", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte ReQInt
        {
            get { return _reQiNt; }
            set
            {
                if (value != _reQiNt)
                {
                    _reQiNt = value;
                    OnPropertyChanged("ReQInt");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_type", Name = "type", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_value", Name = "value", DbType = "int", CanBeNull = false)]
        public int Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_width", Name = "width", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Width
        {
            get { return _width; }
            set
            {
                if (value != _width)
                {
                    _width = value;
                    OnPropertyChanged("Width");
                }
            }
        }

        public ItemTemplate()
        {
            _characterTemplateEquipped = new EntitySet<CharacterTemplateEquipped>(CharacterTemplateEquipped_Attach,
                                                                                  CharacterTemplateEquipped_Detach);
            _characterTemplateInventory = new EntitySet<CharacterTemplateInventory>(CharacterTemplateInventory_Attach,
                                                                                    CharacterTemplateInventory_Detach);
        }

        void CharacterTemplateEquipped_Attach(CharacterTemplateEquipped entity)
        {
            entity.ItemTemplate = this;
        }

        void CharacterTemplateEquipped_Detach(CharacterTemplateEquipped entity)
        {
            entity.ItemTemplate = null;
        }

        void CharacterTemplateInventory_Attach(CharacterTemplateInventory entity)
        {
            entity.ItemTemplate = this;
        }

        void CharacterTemplateInventory_Detach(CharacterTemplateInventory entity)
        {
            entity.ItemTemplate = null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.map")]
    public class Map : INotifyPropertyChanged
    {
        EntitySet<Character> _character;

        EntitySet<Character> _character1;
        ushort _id;
        string _name;

        [Association(Storage = "_character", OtherKey = "Map", ThisKey = "ID", Name = "character_ibfk_2")]
        [DebuggerNonUserCode]
        public EntitySet<Character> Character
        {
            get { return _character; }
            set { _character = value; }
        }

        [Association(Storage = "_character1", OtherKey = "ReSpawnMap", ThisKey = "ID", Name = "character_ibfk_3")]
        [DebuggerNonUserCode]
        public EntitySet<Character> Character1
        {
            get { return _character1; }
            set { _character1 = value; }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_id", Name = "id", DbType = "smallint(5) unsigned", IsPrimaryKey = true, CanBeNull = false)]
        public ushort ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_name", Name = "name", DbType = "varchar(255)", CanBeNull = false)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public Map()
        {
            _character = new EntitySet<Character>(Character_Attach, Character_Detach);
            _character1 = new EntitySet<Character>(Character1_Attach, Character1_Detach);
        }

        void Character_Attach(Character entity)
        {
            entity.MapMap = this;
        }

        void Character_Detach(Character entity)
        {
            entity.MapMap = null;
        }

        void Character1_Attach(Character entity)
        {
            entity.MapMap1 = this;
        }

        void Character1_Detach(Character entity)
        {
            entity.MapMap1 = null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "demogame.map_spawn")]
    public class MapSpawn : INotifyPropertyChanged
    {
        byte _amount;

        ushort _characterID;
        EntityRef<CharacterTemplate> _characterTemplate;

        ushort? _height;

        int _id;

        ushort _mapID;

        ushort? _width;

        ushort? _x;

        ushort? _y;

        [DebuggerNonUserCode]
        [Column(Storage = "_amount", Name = "amount", DbType = "tinyint(3) unsigned", CanBeNull = false)]
        public byte Amount
        {
            get { return _amount; }
            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_characterID", Name = "character_id", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort CharacterID
        {
            get { return _characterID; }
            set
            {
                if (value != _characterID)
                {
                    _characterID = value;
                    OnPropertyChanged("CharacterID");
                }
            }
        }

        [Association(Storage = "_characterTemplate", OtherKey = "ID", ThisKey = "CharacterID", Name = "map_spawn_ibfk_1",
            IsForeignKey = true)]
        [DebuggerNonUserCode]
        public CharacterTemplate CharacterTemplate
        {
            get { return _characterTemplate.Entity; }
            set
            {
                if (value != _characterTemplate.Entity)
                {
                    if (_characterTemplate.Entity != null)
                    {
                        CharacterTemplate previousCharacterTemplate = _characterTemplate.Entity;
                        _characterTemplate.Entity = null;
                        previousCharacterTemplate.MapSpawn.Remove(this);
                    }
                    _characterTemplate.Entity = value;
                    if (value != null)
                    {
                        value.MapSpawn.Add(this);
                        _characterID = value.ID;
                    }
                    else
                        _characterID = default(ushort);
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_height", Name = "height", DbType = "smallint(5) unsigned")]
        public ushort? Height
        {
            get { return _height; }
            set
            {
                if (value != _height)
                {
                    _height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_id", Name = "id", DbType = "int", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_mapID", Name = "map_id", DbType = "smallint(5) unsigned", CanBeNull = false)]
        public ushort MapID
        {
            get { return _mapID; }
            set
            {
                if (value != _mapID)
                {
                    _mapID = value;
                    OnPropertyChanged("MapID");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_width", Name = "width", DbType = "smallint(5) unsigned")]
        public ushort? Width
        {
            get { return _width; }
            set
            {
                if (value != _width)
                {
                    _width = value;
                    OnPropertyChanged("Width");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_x", Name = "x", DbType = "smallint(5) unsigned")]
        public ushort? X
        {
            get { return _x; }
            set
            {
                if (value != _x)
                {
                    _x = value;
                    OnPropertyChanged("X");
                }
            }
        }

        [DebuggerNonUserCode]
        [Column(Storage = "_y", Name = "y", DbType = "smallint(5) unsigned")]
        public ushort? Y
        {
            get { return _y; }
            set
            {
                if (value != _y)
                {
                    _y = value;
                    OnPropertyChanged("Y");
                }
            }
        }

        public MapSpawn()
        {
            _characterTemplate = new EntityRef<CharacterTemplate>();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}