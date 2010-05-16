/********************************************************************
                   DO NOT MANUALLY EDIT THIS FILE!

This file was automatically generated using the DbClassCreator
program. The only time you should ever alter this file is if you are
using an automated code formatter. The DbClassCreator will overwrite
this file every time it is run, so all manual changes will be lost.
If there is something in this file that you wish to change, you should
be able to do it through the DbClassCreator arguments.

Make sure that you re-run the DbClassCreator every time you alter your
game's database.

For more information on the DbClassCreator, please see:
    http://www.netgore.com/wiki/dbclasscreator.html

This file was generated on (UTC): 5/16/2010 7:45:19 PM
********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.IO;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `game_constant`.
    /// </summary>
    public class GameConstantTable : IGameConstantTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 16;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "game_constant";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        {
            "max_account_name_length", "max_account_password_length", "max_characters_per_account", "max_character_name_length",
            "max_inventory_size", "max_shop_items", "max_status_effect_power", "min_account_name_length",
            "min_account_password_length", "min_character_name_length", "screen_height", "screen_width", "server_ip",
            "server_ping_port", "server_tcp_port", "world_physics_update_rate"
        };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        {
            "max_account_name_length", "max_account_password_length", "max_characters_per_account", "max_character_name_length",
            "max_inventory_size", "max_shop_items", "max_status_effect_power", "min_account_name_length",
            "min_account_password_length", "min_character_name_length", "screen_height", "screen_width", "server_ip",
            "server_ping_port", "server_tcp_port", "world_physics_update_rate"
        };

        /// <summary>
        /// The field that maps onto the database column `max_account_name_length`.
        /// </summary>
        Byte _maxAccountNameLength;

        /// <summary>
        /// The field that maps onto the database column `max_account_password_length`.
        /// </summary>
        Byte _maxAccountPasswordLength;

        /// <summary>
        /// The field that maps onto the database column `max_character_name_length`.
        /// </summary>
        Byte _maxCharacterNameLength;

        /// <summary>
        /// The field that maps onto the database column `max_characters_per_account`.
        /// </summary>
        Byte _maxCharactersPerAccount;

        /// <summary>
        /// The field that maps onto the database column `max_inventory_size`.
        /// </summary>
        Byte _maxInventorySize;

        /// <summary>
        /// The field that maps onto the database column `max_shop_items`.
        /// </summary>
        Byte _maxShopItems;

        /// <summary>
        /// The field that maps onto the database column `max_status_effect_power`.
        /// </summary>
        UInt16 _maxStatusEffectPower;

        /// <summary>
        /// The field that maps onto the database column `min_account_name_length`.
        /// </summary>
        Byte _minAccountNameLength;

        /// <summary>
        /// The field that maps onto the database column `min_account_password_length`.
        /// </summary>
        Byte _minAccountPasswordLength;

        /// <summary>
        /// The field that maps onto the database column `min_character_name_length`.
        /// </summary>
        Byte _minCharacterNameLength;

        /// <summary>
        /// The field that maps onto the database column `screen_height`.
        /// </summary>
        UInt16 _screenHeight;

        /// <summary>
        /// The field that maps onto the database column `screen_width`.
        /// </summary>
        UInt16 _screenWidth;

        /// <summary>
        /// The field that maps onto the database column `server_ip`.
        /// </summary>
        String _serverIp;

        /// <summary>
        /// The field that maps onto the database column `server_ping_port`.
        /// </summary>
        UInt16 _serverPingPort;

        /// <summary>
        /// The field that maps onto the database column `server_tcp_port`.
        /// </summary>
        UInt16 _serverTcpPort;

        /// <summary>
        /// The field that maps onto the database column `world_physics_update_rate`.
        /// </summary>
        UInt16 _worldPhysicsUpdateRate;

        /// <summary>
        /// GameConstantTable constructor.
        /// </summary>
        public GameConstantTable()
        {
        }

        /// <summary>
        /// GameConstantTable constructor.
        /// </summary>
        /// <param name="maxAccountNameLength">The initial value for the corresponding property.</param>
        /// <param name="maxAccountPasswordLength">The initial value for the corresponding property.</param>
        /// <param name="maxCharactersPerAccount">The initial value for the corresponding property.</param>
        /// <param name="maxCharacterNameLength">The initial value for the corresponding property.</param>
        /// <param name="maxInventorySize">The initial value for the corresponding property.</param>
        /// <param name="maxShopItems">The initial value for the corresponding property.</param>
        /// <param name="maxStatusEffectPower">The initial value for the corresponding property.</param>
        /// <param name="minAccountNameLength">The initial value for the corresponding property.</param>
        /// <param name="minAccountPasswordLength">The initial value for the corresponding property.</param>
        /// <param name="minCharacterNameLength">The initial value for the corresponding property.</param>
        /// <param name="screenHeight">The initial value for the corresponding property.</param>
        /// <param name="screenWidth">The initial value for the corresponding property.</param>
        /// <param name="serverIp">The initial value for the corresponding property.</param>
        /// <param name="serverPingPort">The initial value for the corresponding property.</param>
        /// <param name="serverTcpPort">The initial value for the corresponding property.</param>
        /// <param name="worldPhysicsUpdateRate">The initial value for the corresponding property.</param>
        public GameConstantTable(Byte @maxAccountNameLength, Byte @maxAccountPasswordLength, Byte @maxCharactersPerAccount,
                                 Byte @maxCharacterNameLength, Byte @maxInventorySize, Byte @maxShopItems,
                                 UInt16 @maxStatusEffectPower, Byte @minAccountNameLength, Byte @minAccountPasswordLength,
                                 Byte @minCharacterNameLength, UInt16 @screenHeight, UInt16 @screenWidth, String @serverIp,
                                 UInt16 @serverPingPort, UInt16 @serverTcpPort, UInt16 @worldPhysicsUpdateRate)
        {
            MaxAccountNameLength = @maxAccountNameLength;
            MaxAccountPasswordLength = @maxAccountPasswordLength;
            MaxCharactersPerAccount = @maxCharactersPerAccount;
            MaxCharacterNameLength = @maxCharacterNameLength;
            MaxInventorySize = @maxInventorySize;
            MaxShopItems = @maxShopItems;
            MaxStatusEffectPower = @maxStatusEffectPower;
            MinAccountNameLength = @minAccountNameLength;
            MinAccountPasswordLength = @minAccountPasswordLength;
            MinCharacterNameLength = @minCharacterNameLength;
            ScreenHeight = @screenHeight;
            ScreenWidth = @screenWidth;
            ServerIp = @serverIp;
            ServerPingPort = @serverPingPort;
            ServerTcpPort = @serverTcpPort;
            WorldPhysicsUpdateRate = @worldPhysicsUpdateRate;
        }

        /// <summary>
        /// GameConstantTable constructor.
        /// </summary>
        /// <param name="source">IGameConstantTable to copy the initial values from.</param>
        public GameConstantTable(IGameConstantTable source)
        {
            CopyValuesFrom(source);
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public static IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
        /// </summary>
        public static IEnumerable<String> DbKeyColumns
        {
            get { return _dbColumnsKeys; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
        /// </summary>
        public static IEnumerable<String> DbNonKeyColumns
        {
            get { return _dbColumnsNonKey; }
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(IGameConstantTable source, IDictionary<String, Object> dic)
        {
            dic["@max_account_name_length"] = source.MaxAccountNameLength;
            dic["@max_account_password_length"] = source.MaxAccountPasswordLength;
            dic["@max_characters_per_account"] = source.MaxCharactersPerAccount;
            dic["@max_character_name_length"] = source.MaxCharacterNameLength;
            dic["@max_inventory_size"] = source.MaxInventorySize;
            dic["@max_shop_items"] = source.MaxShopItems;
            dic["@max_status_effect_power"] = source.MaxStatusEffectPower;
            dic["@min_account_name_length"] = source.MinAccountNameLength;
            dic["@min_account_password_length"] = source.MinAccountPasswordLength;
            dic["@min_character_name_length"] = source.MinCharacterNameLength;
            dic["@screen_height"] = source.ScreenHeight;
            dic["@screen_width"] = source.ScreenWidth;
            dic["@server_ip"] = source.ServerIp;
            dic["@server_ping_port"] = source.ServerPingPort;
            dic["@server_tcp_port"] = source.ServerTcpPort;
            dic["@world_physics_update_rate"] = source.WorldPhysicsUpdateRate;
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public void CopyValues(IDictionary<String, Object> dic)
        {
            CopyValues(this, dic);
        }

        /// <summary>
        /// Copies the values from the given <paramref name="source"/> into this GameConstantTable.
        /// </summary>
        /// <param name="source">The IGameConstantTable to copy the values from.</param>
        public void CopyValuesFrom(IGameConstantTable source)
        {
            MaxAccountNameLength = source.MaxAccountNameLength;
            MaxAccountPasswordLength = source.MaxAccountPasswordLength;
            MaxCharactersPerAccount = source.MaxCharactersPerAccount;
            MaxCharacterNameLength = source.MaxCharacterNameLength;
            MaxInventorySize = source.MaxInventorySize;
            MaxShopItems = source.MaxShopItems;
            MaxStatusEffectPower = source.MaxStatusEffectPower;
            MinAccountNameLength = source.MinAccountNameLength;
            MinAccountPasswordLength = source.MinAccountPasswordLength;
            MinCharacterNameLength = source.MinCharacterNameLength;
            ScreenHeight = source.ScreenHeight;
            ScreenWidth = source.ScreenWidth;
            ServerIp = source.ServerIp;
            ServerPingPort = source.ServerPingPort;
            ServerTcpPort = source.ServerTcpPort;
            WorldPhysicsUpdateRate = source.WorldPhysicsUpdateRate;
        }

        /// <summary>
        /// Gets the data for the database column that this table represents.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the data for.</param>
        /// <returns>
        /// The data for the database column with the name <paramref name="columnName"/>.
        /// </returns>
        public static ColumnMetadata GetColumnData(String columnName)
        {
            switch (columnName)
            {
                case "max_account_name_length":
                    return new ColumnMetadata("max_account_name_length", "", "tinyint(3) unsigned", null, typeof(Byte), false,
                                              false, false);

                case "max_account_password_length":
                    return new ColumnMetadata("max_account_password_length", "", "tinyint(3) unsigned", null, typeof(Byte), false,
                                              false, false);

                case "max_characters_per_account":
                    return new ColumnMetadata("max_characters_per_account", "", "tinyint(3) unsigned", null, typeof(Byte), false,
                                              false, false);

                case "max_character_name_length":
                    return new ColumnMetadata("max_character_name_length", "", "tinyint(3) unsigned", null, typeof(Byte), false,
                                              false, false);

                case "max_inventory_size":
                    return new ColumnMetadata("max_inventory_size", "", "tinyint(3) unsigned", null, typeof(Byte), false, false,
                                              false);

                case "max_shop_items":
                    return new ColumnMetadata("max_shop_items", "", "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                case "max_status_effect_power":
                    return new ColumnMetadata("max_status_effect_power", "", "smallint(5) unsigned", null, typeof(UInt16), false,
                                              false, false);

                case "min_account_name_length":
                    return new ColumnMetadata("min_account_name_length", "", "tinyint(3) unsigned", null, typeof(Byte), false,
                                              false, false);

                case "min_account_password_length":
                    return new ColumnMetadata("min_account_password_length", "", "tinyint(3) unsigned", null, typeof(Byte), false,
                                              false, false);

                case "min_character_name_length":
                    return new ColumnMetadata("min_character_name_length", "", "tinyint(3) unsigned", null, typeof(Byte), false,
                                              false, false);

                case "screen_height":
                    return new ColumnMetadata("screen_height", "", "smallint(5) unsigned", null, typeof(UInt16), false, false,
                                              false);

                case "screen_width":
                    return new ColumnMetadata("screen_width", "", "smallint(5) unsigned", null, typeof(UInt16), false, false,
                                              false);

                case "server_ip":
                    return new ColumnMetadata("server_ip", "", "varchar(150)", null, typeof(String), false, false, false);

                case "server_ping_port":
                    return new ColumnMetadata("server_ping_port", "", "smallint(5) unsigned", null, typeof(UInt16), false, false,
                                              false);

                case "server_tcp_port":
                    return new ColumnMetadata("server_tcp_port", "", "smallint(5) unsigned", null, typeof(UInt16), false, false,
                                              false);

                case "world_physics_update_rate":
                    return new ColumnMetadata("world_physics_update_rate", "", "smallint(5) unsigned", null, typeof(UInt16), false,
                                              false, false);

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Gets the value of a column by the database column's name.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the value for.</param>
        /// <returns>
        /// The value of the column with the name <paramref name="columnName"/>.
        /// </returns>
        public Object GetValue(String columnName)
        {
            switch (columnName)
            {
                case "max_account_name_length":
                    return MaxAccountNameLength;

                case "max_account_password_length":
                    return MaxAccountPasswordLength;

                case "max_characters_per_account":
                    return MaxCharactersPerAccount;

                case "max_character_name_length":
                    return MaxCharacterNameLength;

                case "max_inventory_size":
                    return MaxInventorySize;

                case "max_shop_items":
                    return MaxShopItems;

                case "max_status_effect_power":
                    return MaxStatusEffectPower;

                case "min_account_name_length":
                    return MinAccountNameLength;

                case "min_account_password_length":
                    return MinAccountPasswordLength;

                case "min_character_name_length":
                    return MinCharacterNameLength;

                case "screen_height":
                    return ScreenHeight;

                case "screen_width":
                    return ScreenWidth;

                case "server_ip":
                    return ServerIp;

                case "server_ping_port":
                    return ServerPingPort;

                case "server_tcp_port":
                    return ServerTcpPort;

                case "world_physics_update_rate":
                    return WorldPhysicsUpdateRate;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Sets the <paramref name="value"/> of a column by the database column's name.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
        /// <param name="value">Value to assign to the column.</param>
        public void SetValue(String columnName, Object value)
        {
            switch (columnName)
            {
                case "max_account_name_length":
                    MaxAccountNameLength = (Byte)value;
                    break;

                case "max_account_password_length":
                    MaxAccountPasswordLength = (Byte)value;
                    break;

                case "max_characters_per_account":
                    MaxCharactersPerAccount = (Byte)value;
                    break;

                case "max_character_name_length":
                    MaxCharacterNameLength = (Byte)value;
                    break;

                case "max_inventory_size":
                    MaxInventorySize = (Byte)value;
                    break;

                case "max_shop_items":
                    MaxShopItems = (Byte)value;
                    break;

                case "max_status_effect_power":
                    MaxStatusEffectPower = (UInt16)value;
                    break;

                case "min_account_name_length":
                    MinAccountNameLength = (Byte)value;
                    break;

                case "min_account_password_length":
                    MinAccountPasswordLength = (Byte)value;
                    break;

                case "min_character_name_length":
                    MinCharacterNameLength = (Byte)value;
                    break;

                case "screen_height":
                    ScreenHeight = (UInt16)value;
                    break;

                case "screen_width":
                    ScreenWidth = (UInt16)value;
                    break;

                case "server_ip":
                    ServerIp = (String)value;
                    break;

                case "server_ping_port":
                    ServerPingPort = (UInt16)value;
                    break;

                case "server_tcp_port":
                    ServerTcpPort = (UInt16)value;
                    break;

                case "world_physics_update_rate":
                    WorldPhysicsUpdateRate = (UInt16)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IGameConstantTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `max_account_name_length`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        [SyncValue]
        public Byte MaxAccountNameLength
        {
            get { return _maxAccountNameLength; }
            set { _maxAccountNameLength = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `max_account_password_length`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        [SyncValue]
        public Byte MaxAccountPasswordLength
        {
            get { return _maxAccountPasswordLength; }
            set { _maxAccountPasswordLength = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `max_character_name_length`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        [SyncValue]
        public Byte MaxCharacterNameLength
        {
            get { return _maxCharacterNameLength; }
            set { _maxCharacterNameLength = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `max_characters_per_account`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        [SyncValue]
        public Byte MaxCharactersPerAccount
        {
            get { return _maxCharactersPerAccount; }
            set { _maxCharactersPerAccount = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `max_inventory_size`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        [SyncValue]
        public Byte MaxInventorySize
        {
            get { return _maxInventorySize; }
            set { _maxInventorySize = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `max_shop_items`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        [SyncValue]
        public Byte MaxShopItems
        {
            get { return _maxShopItems; }
            set { _maxShopItems = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `max_status_effect_power`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt16 MaxStatusEffectPower
        {
            get { return _maxStatusEffectPower; }
            set { _maxStatusEffectPower = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `min_account_name_length`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        [SyncValue]
        public Byte MinAccountNameLength
        {
            get { return _minAccountNameLength; }
            set { _minAccountNameLength = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `min_account_password_length`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        [SyncValue]
        public Byte MinAccountPasswordLength
        {
            get { return _minAccountPasswordLength; }
            set { _minAccountPasswordLength = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `min_character_name_length`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        [SyncValue]
        public Byte MinCharacterNameLength
        {
            get { return _minCharacterNameLength; }
            set { _minCharacterNameLength = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `screen_height`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt16 ScreenHeight
        {
            get { return _screenHeight; }
            set { _screenHeight = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `screen_width`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt16 ScreenWidth
        {
            get { return _screenWidth; }
            set { _screenWidth = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `server_ip`.
        /// The underlying database type is `varchar(150)`.
        /// </summary>
        [SyncValue]
        public String ServerIp
        {
            get { return _serverIp; }
            set { _serverIp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `server_ping_port`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt16 ServerPingPort
        {
            get { return _serverPingPort; }
            set { _serverPingPort = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `server_tcp_port`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt16 ServerTcpPort
        {
            get { return _serverTcpPort; }
            set { _serverTcpPort = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `world_physics_update_rate`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt16 WorldPhysicsUpdateRate
        {
            get { return _worldPhysicsUpdateRate; }
            set { _worldPhysicsUpdateRate = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IGameConstantTable DeepCopy()
        {
            return new GameConstantTable(this);
        }

        #endregion

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}