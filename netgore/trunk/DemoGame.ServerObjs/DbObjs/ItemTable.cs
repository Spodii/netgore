using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `item`.
    /// </summary>
    public interface IItemTable
    {
        /// <summary>
        /// Gets the value for the database column `agi`.
        /// </summary>
        UInt16 Agi { get; }

        /// <summary>
        /// Gets the value for the database column `amount`.
        /// </summary>
        Byte Amount { get; }

        /// <summary>
        /// Gets the value for the database column `armor`.
        /// </summary>
        UInt16 Armor { get; }

        /// <summary>
        /// Gets the value for the database column `bra`.
        /// </summary>
        UInt16 Bra { get; }

        /// <summary>
        /// Gets the value for the database column `defence`.
        /// </summary>
        UInt16 Defence { get; }

        /// <summary>
        /// Gets the value for the database column `description`.
        /// </summary>
        String Description { get; }

        /// <summary>
        /// Gets the value for the database column `dex`.
        /// </summary>
        UInt16 Dex { get; }

        /// <summary>
        /// Gets the value for the database column `evade`.
        /// </summary>
        UInt16 Evade { get; }

        /// <summary>
        /// Gets the value for the database column `graphic`.
        /// </summary>
        UInt16 Graphic { get; }

        /// <summary>
        /// Gets the value for the database column `height`.
        /// </summary>
        Byte Height { get; }

        /// <summary>
        /// Gets the value for the database column `hp`.
        /// </summary>
        UInt16 Hp { get; }

        /// <summary>
        /// Gets the value for the database column `id`.
        /// </summary>
        UInt32 Id { get; }

        /// <summary>
        /// Gets the value for the database column `imm`.
        /// </summary>
        UInt16 Imm { get; }

        /// <summary>
        /// Gets the value for the database column `int`.
        /// </summary>
        UInt16 Int { get; }

        /// <summary>
        /// Gets the value for the database column `maxhit`.
        /// </summary>
        UInt16 Maxhit { get; }

        /// <summary>
        /// Gets the value for the database column `maxhp`.
        /// </summary>
        UInt16 Maxhp { get; }

        /// <summary>
        /// Gets the value for the database column `maxmp`.
        /// </summary>
        UInt16 Maxmp { get; }

        /// <summary>
        /// Gets the value for the database column `minhit`.
        /// </summary>
        UInt16 Minhit { get; }

        /// <summary>
        /// Gets the value for the database column `mp`.
        /// </summary>
        UInt16 Mp { get; }

        /// <summary>
        /// Gets the value for the database column `name`.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the value for the database column `perc`.
        /// </summary>
        UInt16 Perc { get; }

        /// <summary>
        /// Gets the value for the database column `reqacc`.
        /// </summary>
        Byte Reqacc { get; }

        /// <summary>
        /// Gets the value for the database column `reqagi`.
        /// </summary>
        Byte Reqagi { get; }

        /// <summary>
        /// Gets the value for the database column `reqarmor`.
        /// </summary>
        Byte Reqarmor { get; }

        /// <summary>
        /// Gets the value for the database column `reqbra`.
        /// </summary>
        Byte Reqbra { get; }

        /// <summary>
        /// Gets the value for the database column `reqdex`.
        /// </summary>
        Byte Reqdex { get; }

        /// <summary>
        /// Gets the value for the database column `reqevade`.
        /// </summary>
        Byte Reqevade { get; }

        /// <summary>
        /// Gets the value for the database column `reqimm`.
        /// </summary>
        Byte Reqimm { get; }

        /// <summary>
        /// Gets the value for the database column `reqint`.
        /// </summary>
        Byte Reqint { get; }

        /// <summary>
        /// Gets the value for the database column `type`.
        /// </summary>
        Byte Type { get; }

        /// <summary>
        /// Gets the value for the database column `value`.
        /// </summary>
        Int32 Value { get; }

        /// <summary>
        /// Gets the value for the database column `width`.
        /// </summary>
        Byte Width { get; }
    }

    /// <summary>
    /// Provides a strongly-typed structure for the database table `item`.
    /// </summary>
    public class ItemTable : IItemTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 32;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "item";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
                                              {
                                                  "agi", "amount", "armor", "bra", "defence", "description", "dex", "evade",
                                                  "graphic", "height", "hp", "id", "imm", "int", "maxhit", "maxhp", "maxmp",
                                                  "minhit", "mp", "name", "perc", "reqacc", "reqagi", "reqarmor", "reqbra", "reqdex"
                                                  , "reqevade", "reqimm", "reqint", "type", "value", "width"
                                              };

        /// <summary>
        /// The field that maps onto the database column `agi`.
        /// </summary>
        UInt16 _agi;

        /// <summary>
        /// The field that maps onto the database column `amount`.
        /// </summary>
        Byte _amount;

        /// <summary>
        /// The field that maps onto the database column `armor`.
        /// </summary>
        UInt16 _armor;

        /// <summary>
        /// The field that maps onto the database column `bra`.
        /// </summary>
        UInt16 _bra;

        /// <summary>
        /// The field that maps onto the database column `defence`.
        /// </summary>
        UInt16 _defence;

        /// <summary>
        /// The field that maps onto the database column `description`.
        /// </summary>
        String _description;

        /// <summary>
        /// The field that maps onto the database column `dex`.
        /// </summary>
        UInt16 _dex;

        /// <summary>
        /// The field that maps onto the database column `evade`.
        /// </summary>
        UInt16 _evade;

        /// <summary>
        /// The field that maps onto the database column `graphic`.
        /// </summary>
        UInt16 _graphic;

        /// <summary>
        /// The field that maps onto the database column `height`.
        /// </summary>
        Byte _height;

        /// <summary>
        /// The field that maps onto the database column `hp`.
        /// </summary>
        UInt16 _hp;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        UInt32 _id;

        /// <summary>
        /// The field that maps onto the database column `imm`.
        /// </summary>
        UInt16 _imm;

        /// <summary>
        /// The field that maps onto the database column `int`.
        /// </summary>
        UInt16 _int;

        /// <summary>
        /// The field that maps onto the database column `maxhit`.
        /// </summary>
        UInt16 _maxhit;

        /// <summary>
        /// The field that maps onto the database column `maxhp`.
        /// </summary>
        UInt16 _maxhp;

        /// <summary>
        /// The field that maps onto the database column `maxmp`.
        /// </summary>
        UInt16 _maxmp;

        /// <summary>
        /// The field that maps onto the database column `minhit`.
        /// </summary>
        UInt16 _minhit;

        /// <summary>
        /// The field that maps onto the database column `mp`.
        /// </summary>
        UInt16 _mp;

        /// <summary>
        /// The field that maps onto the database column `name`.
        /// </summary>
        String _name;

        /// <summary>
        /// The field that maps onto the database column `perc`.
        /// </summary>
        UInt16 _perc;

        /// <summary>
        /// The field that maps onto the database column `reqacc`.
        /// </summary>
        Byte _reqacc;

        /// <summary>
        /// The field that maps onto the database column `reqagi`.
        /// </summary>
        Byte _reqagi;

        /// <summary>
        /// The field that maps onto the database column `reqarmor`.
        /// </summary>
        Byte _reqarmor;

        /// <summary>
        /// The field that maps onto the database column `reqbra`.
        /// </summary>
        Byte _reqbra;

        /// <summary>
        /// The field that maps onto the database column `reqdex`.
        /// </summary>
        Byte _reqdex;

        /// <summary>
        /// The field that maps onto the database column `reqevade`.
        /// </summary>
        Byte _reqevade;

        /// <summary>
        /// The field that maps onto the database column `reqimm`.
        /// </summary>
        Byte _reqimm;

        /// <summary>
        /// The field that maps onto the database column `reqint`.
        /// </summary>
        Byte _reqint;

        /// <summary>
        /// The field that maps onto the database column `type`.
        /// </summary>
        Byte _type;

        /// <summary>
        /// The field that maps onto the database column `value`.
        /// </summary>
        Int32 _value;

        /// <summary>
        /// The field that maps onto the database column `width`.
        /// </summary>
        Byte _width;

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
        }

        /// <summary>
        /// ItemTable constructor.
        /// </summary>
        public ItemTable()
        {
        }

        /// <summary>
        /// ItemTable constructor.
        /// </summary>
        /// <param name="agi">The initial value for the corresponding property.</param>
        /// <param name="amount">The initial value for the corresponding property.</param>
        /// <param name="armor">The initial value for the corresponding property.</param>
        /// <param name="bra">The initial value for the corresponding property.</param>
        /// <param name="defence">The initial value for the corresponding property.</param>
        /// <param name="description">The initial value for the corresponding property.</param>
        /// <param name="dex">The initial value for the corresponding property.</param>
        /// <param name="evade">The initial value for the corresponding property.</param>
        /// <param name="graphic">The initial value for the corresponding property.</param>
        /// <param name="height">The initial value for the corresponding property.</param>
        /// <param name="hp">The initial value for the corresponding property.</param>
        /// <param name="id">The initial value for the corresponding property.</param>
        /// <param name="imm">The initial value for the corresponding property.</param>
        /// <param name="int">The initial value for the corresponding property.</param>
        /// <param name="maxhit">The initial value for the corresponding property.</param>
        /// <param name="maxhp">The initial value for the corresponding property.</param>
        /// <param name="maxmp">The initial value for the corresponding property.</param>
        /// <param name="minhit">The initial value for the corresponding property.</param>
        /// <param name="mp">The initial value for the corresponding property.</param>
        /// <param name="name">The initial value for the corresponding property.</param>
        /// <param name="perc">The initial value for the corresponding property.</param>
        /// <param name="reqacc">The initial value for the corresponding property.</param>
        /// <param name="reqagi">The initial value for the corresponding property.</param>
        /// <param name="reqarmor">The initial value for the corresponding property.</param>
        /// <param name="reqbra">The initial value for the corresponding property.</param>
        /// <param name="reqdex">The initial value for the corresponding property.</param>
        /// <param name="reqevade">The initial value for the corresponding property.</param>
        /// <param name="reqimm">The initial value for the corresponding property.</param>
        /// <param name="reqint">The initial value for the corresponding property.</param>
        /// <param name="type">The initial value for the corresponding property.</param>
        /// <param name="value">The initial value for the corresponding property.</param>
        /// <param name="width">The initial value for the corresponding property.</param>
        public ItemTable(UInt16 @agi, Byte @amount, UInt16 @armor, UInt16 @bra, UInt16 @defence, String @description, UInt16 @dex,
                         UInt16 @evade, UInt16 @graphic, Byte @height, UInt16 @hp, UInt32 @id, UInt16 @imm, UInt16 @int,
                         UInt16 @maxhit, UInt16 @maxhp, UInt16 @maxmp, UInt16 @minhit, UInt16 @mp, String @name, UInt16 @perc,
                         Byte @reqacc, Byte @reqagi, Byte @reqarmor, Byte @reqbra, Byte @reqdex, Byte @reqevade, Byte @reqimm,
                         Byte @reqint, Byte @type, Int32 @value, Byte @width)
        {
            Agi = @agi;
            Amount = @amount;
            Armor = @armor;
            Bra = @bra;
            Defence = @defence;
            Description = @description;
            Dex = @dex;
            Evade = @evade;
            Graphic = @graphic;
            Height = @height;
            Hp = @hp;
            Id = @id;
            Imm = @imm;
            Int = @int;
            Maxhit = @maxhit;
            Maxhp = @maxhp;
            Maxmp = @maxmp;
            Minhit = @minhit;
            Mp = @mp;
            Name = @name;
            Perc = @perc;
            Reqacc = @reqacc;
            Reqagi = @reqagi;
            Reqarmor = @reqarmor;
            Reqbra = @reqbra;
            Reqdex = @reqdex;
            Reqevade = @reqevade;
            Reqimm = @reqimm;
            Reqint = @reqint;
            Type = @type;
            Value = @value;
            Width = @width;
        }

        /// <summary>
        /// ItemTable constructor.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
        public ItemTable(IDataReader dataReader)
        {
            ReadValues(dataReader);
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(IItemTable source, IDictionary<String, Object> dic)
        {
            dic["@agi"] = source.Agi;
            dic["@amount"] = source.Amount;
            dic["@armor"] = source.Armor;
            dic["@bra"] = source.Bra;
            dic["@defence"] = source.Defence;
            dic["@description"] = source.Description;
            dic["@dex"] = source.Dex;
            dic["@evade"] = source.Evade;
            dic["@graphic"] = source.Graphic;
            dic["@height"] = source.Height;
            dic["@hp"] = source.Hp;
            dic["@id"] = source.Id;
            dic["@imm"] = source.Imm;
            dic["@int"] = source.Int;
            dic["@maxhit"] = source.Maxhit;
            dic["@maxhp"] = source.Maxhp;
            dic["@maxmp"] = source.Maxmp;
            dic["@minhit"] = source.Minhit;
            dic["@mp"] = source.Mp;
            dic["@name"] = source.Name;
            dic["@perc"] = source.Perc;
            dic["@reqacc"] = source.Reqacc;
            dic["@reqagi"] = source.Reqagi;
            dic["@reqarmor"] = source.Reqarmor;
            dic["@reqbra"] = source.Reqbra;
            dic["@reqdex"] = source.Reqdex;
            dic["@reqevade"] = source.Reqevade;
            dic["@reqimm"] = source.Reqimm;
            dic["@reqint"] = source.Reqint;
            dic["@type"] = source.Type;
            dic["@value"] = source.Value;
            dic["@width"] = source.Width;
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(IItemTable source, DbParameterValues paramValues)
        {
            paramValues["@agi"] = source.Agi;
            paramValues["@amount"] = source.Amount;
            paramValues["@armor"] = source.Armor;
            paramValues["@bra"] = source.Bra;
            paramValues["@defence"] = source.Defence;
            paramValues["@description"] = source.Description;
            paramValues["@dex"] = source.Dex;
            paramValues["@evade"] = source.Evade;
            paramValues["@graphic"] = source.Graphic;
            paramValues["@height"] = source.Height;
            paramValues["@hp"] = source.Hp;
            paramValues["@id"] = source.Id;
            paramValues["@imm"] = source.Imm;
            paramValues["@int"] = source.Int;
            paramValues["@maxhit"] = source.Maxhit;
            paramValues["@maxhp"] = source.Maxhp;
            paramValues["@maxmp"] = source.Maxmp;
            paramValues["@minhit"] = source.Minhit;
            paramValues["@mp"] = source.Mp;
            paramValues["@name"] = source.Name;
            paramValues["@perc"] = source.Perc;
            paramValues["@reqacc"] = source.Reqacc;
            paramValues["@reqagi"] = source.Reqagi;
            paramValues["@reqarmor"] = source.Reqarmor;
            paramValues["@reqbra"] = source.Reqbra;
            paramValues["@reqdex"] = source.Reqdex;
            paramValues["@reqevade"] = source.Reqevade;
            paramValues["@reqimm"] = source.Reqimm;
            paramValues["@reqint"] = source.Reqint;
            paramValues["@type"] = source.Type;
            paramValues["@value"] = source.Value;
            paramValues["@width"] = source.Width;
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public void CopyValues(IDictionary<String, Object> dic)
        {
            CopyValues(this, dic);
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public void CopyValues(DbParameterValues paramValues)
        {
            CopyValues(this, paramValues);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void ReadValues(IDataReader dataReader)
        {
            Agi = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("agi"));
            Amount = (Byte)dataReader.GetValue(dataReader.GetOrdinal("amount"));
            Armor = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("armor"));
            Bra = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("bra"));
            Defence = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("defence"));
            Description = (String)dataReader.GetValue(dataReader.GetOrdinal("description"));
            Dex = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("dex"));
            Evade = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("evade"));
            Graphic = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("graphic"));
            Height = (Byte)dataReader.GetValue(dataReader.GetOrdinal("height"));
            Hp = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("hp"));
            Id = (UInt32)dataReader.GetValue(dataReader.GetOrdinal("id"));
            Imm = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("imm"));
            Int = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("int"));
            Maxhit = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("maxhit"));
            Maxhp = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("maxhp"));
            Maxmp = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("maxmp"));
            Minhit = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("minhit"));
            Mp = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("mp"));
            Name = (String)dataReader.GetValue(dataReader.GetOrdinal("name"));
            Perc = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("perc"));
            Reqacc = (Byte)dataReader.GetValue(dataReader.GetOrdinal("reqacc"));
            Reqagi = (Byte)dataReader.GetValue(dataReader.GetOrdinal("reqagi"));
            Reqarmor = (Byte)dataReader.GetValue(dataReader.GetOrdinal("reqarmor"));
            Reqbra = (Byte)dataReader.GetValue(dataReader.GetOrdinal("reqbra"));
            Reqdex = (Byte)dataReader.GetValue(dataReader.GetOrdinal("reqdex"));
            Reqevade = (Byte)dataReader.GetValue(dataReader.GetOrdinal("reqevade"));
            Reqimm = (Byte)dataReader.GetValue(dataReader.GetOrdinal("reqimm"));
            Reqint = (Byte)dataReader.GetValue(dataReader.GetOrdinal("reqint"));
            Type = (Byte)dataReader.GetValue(dataReader.GetOrdinal("type"));
            Value = (Int32)dataReader.GetValue(dataReader.GetOrdinal("value"));
            Width = (Byte)dataReader.GetValue(dataReader.GetOrdinal("width"));
        }

        #region IItemTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `agi`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Agi
        {
            get { return _agi; }
            set { _agi = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `amount`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `armor`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Armor
        {
            get { return _armor; }
            set { _armor = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `bra`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Bra
        {
            get { return _bra; }
            set { _bra = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `defence`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Defence
        {
            get { return _defence; }
            set { _defence = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `description`.
        /// The underlying database type is `varchar(255)`.
        /// </summary>
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `dex`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Dex
        {
            get { return _dex; }
            set { _dex = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `evade`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Evade
        {
            get { return _evade; }
            set { _evade = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `graphic`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Graphic
        {
            get { return _graphic; }
            set { _graphic = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `height`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `16`.
        /// </summary>
        public Byte Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `hp`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Hp
        {
            get { return _hp; }
            set { _hp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(10) unsigned`.
        /// </summary>
        public UInt32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `imm`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Imm
        {
            get { return _imm; }
            set { _imm = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `int`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Int
        {
            get { return _int; }
            set { _int = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `maxhit`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Maxhit
        {
            get { return _maxhit; }
            set { _maxhit = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `maxhp`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Maxhp
        {
            get { return _maxhp; }
            set { _maxhp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `maxmp`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Maxmp
        {
            get { return _maxmp; }
            set { _maxmp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `minhit`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Minhit
        {
            get { return _minhit; }
            set { _minhit = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `mp`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Mp
        {
            get { return _mp; }
            set { _mp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `name`.
        /// The underlying database type is `varchar(255)`.
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `perc`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 Perc
        {
            get { return _perc; }
            set { _perc = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `reqacc`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
        /// </summary>
        public Byte Reqacc
        {
            get { return _reqacc; }
            set { _reqacc = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `reqagi`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
        /// </summary>
        public Byte Reqagi
        {
            get { return _reqagi; }
            set { _reqagi = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `reqarmor`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
        /// </summary>
        public Byte Reqarmor
        {
            get { return _reqarmor; }
            set { _reqarmor = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `reqbra`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
        /// </summary>
        public Byte Reqbra
        {
            get { return _reqbra; }
            set { _reqbra = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `reqdex`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
        /// </summary>
        public Byte Reqdex
        {
            get { return _reqdex; }
            set { _reqdex = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `reqevade`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
        /// </summary>
        public Byte Reqevade
        {
            get { return _reqevade; }
            set { _reqevade = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `reqimm`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
        /// </summary>
        public Byte Reqimm
        {
            get { return _reqimm; }
            set { _reqimm = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `reqint`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
        /// </summary>
        public Byte Reqint
        {
            get { return _reqint; }
            set { _reqint = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `type`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
        /// </summary>
        public Byte Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `value`.
        /// The underlying database type is `int(11)` with the default value of `0`.
        /// </summary>
        public Int32 Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `width`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `16`.
        /// </summary>
        public Byte Width
        {
            get { return _width; }
            set { _width = value; }
        }

        #endregion
    }
}