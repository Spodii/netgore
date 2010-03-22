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

This file was generated on (UTC): 3/22/2010 2:25:30 AM
********************************************************************/

using System;
using System.Data;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class GameConstantTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class GameConstantTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this IGameConstantTable source, DbParameterValues paramValues)
        {
            paramValues["@max_account_name_length"] = source.MaxAccountNameLength;
            paramValues["@max_account_password_length"] = source.MaxAccountPasswordLength;
            paramValues["@max_characters_per_account"] = source.MaxCharactersPerAccount;
            paramValues["@max_character_name_length"] = source.MaxCharacterNameLength;
            paramValues["@max_inventory_size"] = source.MaxInventorySize;
            paramValues["@max_shop_items"] = source.MaxShopItems;
            paramValues["@max_status_effect_power"] = source.MaxStatusEffectPower;
            paramValues["@min_account_name_length"] = source.MinAccountNameLength;
            paramValues["@min_account_password_length"] = source.MinAccountPasswordLength;
            paramValues["@min_character_name_length"] = source.MinCharacterNameLength;
            paramValues["@screen_height"] = source.ScreenHeight;
            paramValues["@screen_width"] = source.ScreenWidth;
            paramValues["@server_ip"] = source.ServerIp;
            paramValues["@server_ping_port"] = source.ServerPingPort;
            paramValues["@server_tcp_port"] = source.ServerTcpPort;
            paramValues["@world_physics_update_rate"] = source.WorldPhysicsUpdateRate;
        }

        /// <summary>
        /// Checks if this <see cref="IGameConstantTable"/> contains the same values as an<paramref name="other"/> <see cref="IGameConstantTable"/>.
        /// </summary>
        /// <param name="other">The <see cref="IGameConstantTable"/> to compare the values to.</param>
        /// <returns>
        /// True if this <see cref="IGameConstantTable"/> contains the same values as the <paramref name="<paramref name="other"/>"/>; <paramref name="other"/>wise false.
        /// </returns>
        public static Boolean HasSameValues(this IGameConstantTable source, IGameConstantTable other)
        {
            return Equals(source.MaxAccountNameLength, other.MaxAccountNameLength) &&
                   Equals(source.MaxAccountPasswordLength, other.MaxAccountPasswordLength) &&
                   Equals(source.MaxCharactersPerAccount, other.MaxCharactersPerAccount) &&
                   Equals(source.MaxCharacterNameLength, other.MaxCharacterNameLength) &&
                   Equals(source.MaxInventorySize, other.MaxInventorySize) && Equals(source.MaxShopItems, other.MaxShopItems) &&
                   Equals(source.MaxStatusEffectPower, other.MaxStatusEffectPower) &&
                   Equals(source.MinAccountNameLength, other.MinAccountNameLength) &&
                   Equals(source.MinAccountPasswordLength, other.MinAccountPasswordLength) &&
                   Equals(source.MinCharacterNameLength, other.MinCharacterNameLength) &&
                   Equals(source.ScreenHeight, other.ScreenHeight) && Equals(source.ScreenWidth, other.ScreenWidth) &&
                   Equals(source.ServerIp, other.ServerIp) && Equals(source.ServerPingPort, other.ServerPingPort) &&
                   Equals(source.ServerTcpPort, other.ServerTcpPort) &&
                   Equals(source.WorldPhysicsUpdateRate, other.WorldPhysicsUpdateRate);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this GameConstantTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("max_account_name_length");

            source.MaxAccountNameLength = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("max_account_password_length");

            source.MaxAccountPasswordLength = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("max_characters_per_account");

            source.MaxCharactersPerAccount = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("max_character_name_length");

            source.MaxCharacterNameLength = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("max_inventory_size");

            source.MaxInventorySize = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("max_shop_items");

            source.MaxShopItems = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("max_status_effect_power");

            source.MaxStatusEffectPower = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("min_account_name_length");

            source.MinAccountNameLength = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("min_account_password_length");

            source.MinAccountPasswordLength = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("min_character_name_length");

            source.MinCharacterNameLength = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("screen_height");

            source.ScreenHeight = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("screen_width");

            source.ScreenWidth = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("server_ip");

            source.ServerIp = dataReader.GetString(i);

            i = dataReader.GetOrdinal("server_ping_port");

            source.ServerPingPort = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("server_tcp_port");

            source.ServerTcpPort = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("world_physics_update_rate");

            source.WorldPhysicsUpdateRate = dataReader.GetUInt16(i);
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The key must already exist in the DbParameterValues
        /// for the value to be copied over. If any of the keys in the DbParameterValues do not
        /// match one of the column names, or if there is no field for a key, then it will be
        /// ignored. Because of this, it is important to be careful when using this method
        /// since columns or keys can be skipped without any indication.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void TryCopyValues(this IGameConstantTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@max_account_name_length":
                        paramValues[i] = source.MaxAccountNameLength;
                        break;

                    case "@max_account_password_length":
                        paramValues[i] = source.MaxAccountPasswordLength;
                        break;

                    case "@max_characters_per_account":
                        paramValues[i] = source.MaxCharactersPerAccount;
                        break;

                    case "@max_character_name_length":
                        paramValues[i] = source.MaxCharacterNameLength;
                        break;

                    case "@max_inventory_size":
                        paramValues[i] = source.MaxInventorySize;
                        break;

                    case "@max_shop_items":
                        paramValues[i] = source.MaxShopItems;
                        break;

                    case "@max_status_effect_power":
                        paramValues[i] = source.MaxStatusEffectPower;
                        break;

                    case "@min_account_name_length":
                        paramValues[i] = source.MinAccountNameLength;
                        break;

                    case "@min_account_password_length":
                        paramValues[i] = source.MinAccountPasswordLength;
                        break;

                    case "@min_character_name_length":
                        paramValues[i] = source.MinCharacterNameLength;
                        break;

                    case "@screen_height":
                        paramValues[i] = source.ScreenHeight;
                        break;

                    case "@screen_width":
                        paramValues[i] = source.ScreenWidth;
                        break;

                    case "@server_ip":
                        paramValues[i] = source.ServerIp;
                        break;

                    case "@server_ping_port":
                        paramValues[i] = source.ServerPingPort;
                        break;

                    case "@server_tcp_port":
                        paramValues[i] = source.ServerTcpPort;
                        break;

                    case "@world_physics_update_rate":
                        paramValues[i] = source.WorldPhysicsUpdateRate;
                        break;
                }
            }
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. Unlike ReadValues(), this method not only doesn't require
        /// all values to be in the IDataReader, but also does not require the values in
        /// the IDataReader to be a defined field for the table this class represents.
        /// Because of this, you need to be careful when using this method because values
        /// can easily be skipped without any indication.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void TryReadValues(this GameConstantTable source, IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "max_account_name_length":
                        source.MaxAccountNameLength = dataReader.GetByte(i);
                        break;

                    case "max_account_password_length":
                        source.MaxAccountPasswordLength = dataReader.GetByte(i);
                        break;

                    case "max_characters_per_account":
                        source.MaxCharactersPerAccount = dataReader.GetByte(i);
                        break;

                    case "max_character_name_length":
                        source.MaxCharacterNameLength = dataReader.GetByte(i);
                        break;

                    case "max_inventory_size":
                        source.MaxInventorySize = dataReader.GetByte(i);
                        break;

                    case "max_shop_items":
                        source.MaxShopItems = dataReader.GetByte(i);
                        break;

                    case "max_status_effect_power":
                        source.MaxStatusEffectPower = dataReader.GetUInt16(i);
                        break;

                    case "min_account_name_length":
                        source.MinAccountNameLength = dataReader.GetByte(i);
                        break;

                    case "min_account_password_length":
                        source.MinAccountPasswordLength = dataReader.GetByte(i);
                        break;

                    case "min_character_name_length":
                        source.MinCharacterNameLength = dataReader.GetByte(i);
                        break;

                    case "screen_height":
                        source.ScreenHeight = dataReader.GetUInt16(i);
                        break;

                    case "screen_width":
                        source.ScreenWidth = dataReader.GetUInt16(i);
                        break;

                    case "server_ip":
                        source.ServerIp = dataReader.GetString(i);
                        break;

                    case "server_ping_port":
                        source.ServerPingPort = dataReader.GetUInt16(i);
                        break;

                    case "server_tcp_port":
                        source.ServerTcpPort = dataReader.GetUInt16(i);
                        break;

                    case "world_physics_update_rate":
                        source.WorldPhysicsUpdateRate = dataReader.GetUInt16(i);
                        break;
                }
            }
        }
    }
}