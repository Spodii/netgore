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

This file was generated on (UTC): 5/17/2010 11:46:58 PM
********************************************************************/

using System;
using System.Data;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Db;
using NetGore.Features.Shops;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class WorldStatsUserShoppingTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class WorldStatsUserShoppingTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this IWorldStatsUserShoppingTable source, DbParameterValues paramValues)
        {
            paramValues["@amount"] = source.Amount;
            paramValues["@character_id"] = (Int32)source.CharacterID;
            paramValues["@cost"] = source.Cost;
            paramValues["@item_template_id"] = (ushort?)source.ItemTemplateID;
            paramValues["@map_id"] = (ushort?)source.MapID;
            paramValues["@sale_type"] = source.SaleType;
            paramValues["@shop_id"] = (UInt16)source.ShopID;
            paramValues["@when"] = source.When;
            paramValues["@x"] = source.X;
            paramValues["@y"] = source.Y;
        }

        /// <summary>
        /// Checks if this <see cref="IWorldStatsUserShoppingTable"/> contains the same values as another <see cref="IWorldStatsUserShoppingTable"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IWorldStatsUserShoppingTable"/>.</param>
        /// <param name="otherItem">The <see cref="IWorldStatsUserShoppingTable"/> to compare the values to.</param>
        /// <returns>
        /// True if this <see cref="IWorldStatsUserShoppingTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
        /// </returns>
        public static Boolean HasSameValues(this IWorldStatsUserShoppingTable source, IWorldStatsUserShoppingTable otherItem)
        {
            return Equals(source.Amount, otherItem.Amount) && Equals(source.CharacterID, otherItem.CharacterID) &&
                   Equals(source.Cost, otherItem.Cost) && Equals(source.ItemTemplateID, otherItem.ItemTemplateID) &&
                   Equals(source.MapID, otherItem.MapID) && Equals(source.SaleType, otherItem.SaleType) &&
                   Equals(source.ShopID, otherItem.ShopID) && Equals(source.When, otherItem.When) && Equals(source.X, otherItem.X) &&
                   Equals(source.Y, otherItem.Y);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this WorldStatsUserShoppingTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("amount");

            source.Amount = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("character_id");

            source.CharacterID = (CharacterID)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("cost");

            source.Cost = dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("item_template_id");

            source.ItemTemplateID = (Nullable<ItemTemplateID>)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("map_id");

            source.MapID = (Nullable<MapID>)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("sale_type");

            source.SaleType = dataReader.GetSByte(i);

            i = dataReader.GetOrdinal("shop_id");

            source.ShopID = (ShopID)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("when");

            source.When = dataReader.GetDateTime(i);

            i = dataReader.GetOrdinal("x");

            source.X = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("y");

            source.Y = dataReader.GetUInt16(i);
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
        public static void TryCopyValues(this IWorldStatsUserShoppingTable source, DbParameterValues paramValues)
        {
            for (var i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@amount":
                        paramValues[i] = source.Amount;
                        break;

                    case "@character_id":
                        paramValues[i] = (Int32)source.CharacterID;
                        break;

                    case "@cost":
                        paramValues[i] = source.Cost;
                        break;

                    case "@item_template_id":
                        paramValues[i] = (ushort?)source.ItemTemplateID;
                        break;

                    case "@map_id":
                        paramValues[i] = (ushort?)source.MapID;
                        break;

                    case "@sale_type":
                        paramValues[i] = source.SaleType;
                        break;

                    case "@shop_id":
                        paramValues[i] = (UInt16)source.ShopID;
                        break;

                    case "@when":
                        paramValues[i] = source.When;
                        break;

                    case "@x":
                        paramValues[i] = source.X;
                        break;

                    case "@y":
                        paramValues[i] = source.Y;
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
        public static void TryReadValues(this WorldStatsUserShoppingTable source, IDataReader dataReader)
        {
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "amount":
                        source.Amount = dataReader.GetByte(i);
                        break;

                    case "character_id":
                        source.CharacterID = (CharacterID)dataReader.GetInt32(i);
                        break;

                    case "cost":
                        source.Cost = dataReader.GetInt32(i);
                        break;

                    case "item_template_id":
                        source.ItemTemplateID =
                            (Nullable<ItemTemplateID>)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "map_id":
                        source.MapID = (Nullable<MapID>)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "sale_type":
                        source.SaleType = dataReader.GetSByte(i);
                        break;

                    case "shop_id":
                        source.ShopID = (ShopID)dataReader.GetUInt16(i);
                        break;

                    case "when":
                        source.When = dataReader.GetDateTime(i);
                        break;

                    case "x":
                        source.X = dataReader.GetUInt16(i);
                        break;

                    case "y":
                        source.Y = dataReader.GetUInt16(i);
                        break;
                }
            }
        }
    }
}