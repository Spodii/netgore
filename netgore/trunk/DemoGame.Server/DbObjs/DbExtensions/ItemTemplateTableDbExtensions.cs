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

This file was generated on (UTC): 3/30/2010 12:13:03 AM
********************************************************************/

using System;
using System.Data;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class ItemTemplateTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class ItemTemplateTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this IItemTemplateTable source, DbParameterValues paramValues)
        {
            paramValues["@description"] = source.Description;
            paramValues["@equipped_body"] = source.EquippedBody;
            paramValues["@graphic"] = (UInt16)source.Graphic;
            paramValues["@height"] = source.Height;
            paramValues["@hp"] = (Int16)source.HP;
            paramValues["@id"] = (UInt16)source.ID;
            paramValues["@mp"] = (Int16)source.MP;
            paramValues["@name"] = source.Name;
            paramValues["@range"] = source.Range;
            paramValues["@stat_agi"] = (Int16)source.GetStat(StatType.Agi);
            paramValues["@stat_defence"] = (Int16)source.GetStat(StatType.Defence);
            paramValues["@stat_int"] = (Int16)source.GetStat(StatType.Int);
            paramValues["@stat_maxhit"] = (Int16)source.GetStat(StatType.MaxHit);
            paramValues["@stat_maxhp"] = (Int16)source.GetStat(StatType.MaxHP);
            paramValues["@stat_maxmp"] = (Int16)source.GetStat(StatType.MaxMP);
            paramValues["@stat_minhit"] = (Int16)source.GetStat(StatType.MinHit);
            paramValues["@stat_req_agi"] = (Int16)source.GetReqStat(StatType.Agi);
            paramValues["@stat_req_int"] = (Int16)source.GetReqStat(StatType.Int);
            paramValues["@stat_req_str"] = (Int16)source.GetReqStat(StatType.Str);
            paramValues["@stat_str"] = (Int16)source.GetStat(StatType.Str);
            paramValues["@type"] = (Byte)source.Type;
            paramValues["@value"] = source.Value;
            paramValues["@weapon_type"] = (Byte)source.WeaponType;
            paramValues["@width"] = source.Width;
        }

        /// <summary>
        /// Checks if this <see cref="IItemTemplateTable"/> contains the same values as an<paramref name="other"/> <see cref="IItemTemplateTable"/>.
        /// </summary>
        /// <param name="other">The <see cref="IItemTemplateTable"/> to compare the values to.</param>
        /// <returns>
        /// True if this <see cref="IItemTemplateTable"/> contains the same values as the <paramref name="<paramref name="other"/>"/>; <paramref name="other"/>wise false.
        /// </returns>
        public static Boolean HasSameValues(this IItemTemplateTable source, IItemTemplateTable other)
        {
            return Equals(source.Description, other.Description) && Equals(source.EquippedBody, other.EquippedBody) &&
                   Equals(source.Graphic, other.Graphic) && Equals(source.Height, other.Height) && Equals(source.HP, other.HP) &&
                   Equals(source.ID, other.ID) && Equals(source.MP, other.MP) && Equals(source.Name, other.Name) &&
                   Equals(source.Range, other.Range) && Equals(source.GetStat(StatType.Agi), other.GetStat(StatType.Agi)) &&
                   Equals(source.GetStat(StatType.Defence), other.GetStat(StatType.Defence)) &&
                   Equals(source.GetStat(StatType.Int), other.GetStat(StatType.Int)) &&
                   Equals(source.GetStat(StatType.MaxHit), other.GetStat(StatType.MaxHit)) &&
                   Equals(source.GetStat(StatType.MaxHP), other.GetStat(StatType.MaxHP)) &&
                   Equals(source.GetStat(StatType.MaxMP), other.GetStat(StatType.MaxMP)) &&
                   Equals(source.GetStat(StatType.MinHit), other.GetStat(StatType.MinHit)) &&
                   Equals(source.GetReqStat(StatType.Agi), other.GetReqStat(StatType.Agi)) &&
                   Equals(source.GetReqStat(StatType.Int), other.GetReqStat(StatType.Int)) &&
                   Equals(source.GetReqStat(StatType.Str), other.GetReqStat(StatType.Str)) &&
                   Equals(source.GetStat(StatType.Str), other.GetStat(StatType.Str)) && Equals(source.Type, other.Type) &&
                   Equals(source.Value, other.Value) && Equals(source.WeaponType, other.WeaponType) &&
                   Equals(source.Width, other.Width);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this ItemTemplateTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("description");

            source.Description = dataReader.GetString(i);

            i = dataReader.GetOrdinal("equipped_body");

            source.EquippedBody = (dataReader.IsDBNull(i) ? null : dataReader.GetString(i));

            i = dataReader.GetOrdinal("graphic");

            source.Graphic = (GrhIndex)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("height");

            source.Height = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("hp");

            source.HP = dataReader.GetInt16(i);

            i = dataReader.GetOrdinal("id");

            source.ID = (ItemTemplateID)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("mp");

            source.MP = dataReader.GetInt16(i);

            i = dataReader.GetOrdinal("name");

            source.Name = dataReader.GetString(i);

            i = dataReader.GetOrdinal("range");

            source.Range = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("stat_agi");

            source.SetStat(StatType.Agi, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_defence");

            source.SetStat(StatType.Defence, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_int");

            source.SetStat(StatType.Int, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_maxhit");

            source.SetStat(StatType.MaxHit, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_maxhp");

            source.SetStat(StatType.MaxHP, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_maxmp");

            source.SetStat(StatType.MaxMP, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_minhit");

            source.SetStat(StatType.MinHit, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_req_agi");

            source.SetReqStat(StatType.Agi, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_req_int");

            source.SetReqStat(StatType.Int, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_req_str");

            source.SetReqStat(StatType.Str, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_str");

            source.SetStat(StatType.Str, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("type");

            source.Type = (ItemType)dataReader.GetByte(i);

            i = dataReader.GetOrdinal("value");

            source.Value = dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("weapon_type");

            source.WeaponType = (WeaponType)dataReader.GetByte(i);

            i = dataReader.GetOrdinal("width");

            source.Width = dataReader.GetByte(i);
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
        public static void TryCopyValues(this IItemTemplateTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@description":
                        paramValues[i] = source.Description;
                        break;

                    case "@equipped_body":
                        paramValues[i] = source.EquippedBody;
                        break;

                    case "@graphic":
                        paramValues[i] = (UInt16)source.Graphic;
                        break;

                    case "@height":
                        paramValues[i] = source.Height;
                        break;

                    case "@hp":
                        paramValues[i] = (Int16)source.HP;
                        break;

                    case "@id":
                        paramValues[i] = (UInt16)source.ID;
                        break;

                    case "@mp":
                        paramValues[i] = (Int16)source.MP;
                        break;

                    case "@name":
                        paramValues[i] = source.Name;
                        break;

                    case "@range":
                        paramValues[i] = source.Range;
                        break;

                    case "@stat_agi":
                        paramValues[i] = (Int16)source.GetStat(StatType.Agi);
                        break;

                    case "@stat_defence":
                        paramValues[i] = (Int16)source.GetStat(StatType.Defence);
                        break;

                    case "@stat_int":
                        paramValues[i] = (Int16)source.GetStat(StatType.Int);
                        break;

                    case "@stat_maxhit":
                        paramValues[i] = (Int16)source.GetStat(StatType.MaxHit);
                        break;

                    case "@stat_maxhp":
                        paramValues[i] = (Int16)source.GetStat(StatType.MaxHP);
                        break;

                    case "@stat_maxmp":
                        paramValues[i] = (Int16)source.GetStat(StatType.MaxMP);
                        break;

                    case "@stat_minhit":
                        paramValues[i] = (Int16)source.GetStat(StatType.MinHit);
                        break;

                    case "@stat_req_agi":
                        paramValues[i] = (Int16)source.GetReqStat(StatType.Agi);
                        break;

                    case "@stat_req_int":
                        paramValues[i] = (Int16)source.GetReqStat(StatType.Int);
                        break;

                    case "@stat_req_str":
                        paramValues[i] = (Int16)source.GetReqStat(StatType.Str);
                        break;

                    case "@stat_str":
                        paramValues[i] = (Int16)source.GetStat(StatType.Str);
                        break;

                    case "@type":
                        paramValues[i] = (Byte)source.Type;
                        break;

                    case "@value":
                        paramValues[i] = source.Value;
                        break;

                    case "@weapon_type":
                        paramValues[i] = (Byte)source.WeaponType;
                        break;

                    case "@width":
                        paramValues[i] = source.Width;
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
        public static void TryReadValues(this ItemTemplateTable source, IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "description":
                        source.Description = dataReader.GetString(i);
                        break;

                    case "equipped_body":
                        source.EquippedBody = (dataReader.IsDBNull(i) ? null : dataReader.GetString(i));
                        break;

                    case "graphic":
                        source.Graphic = (GrhIndex)dataReader.GetUInt16(i);
                        break;

                    case "height":
                        source.Height = dataReader.GetByte(i);
                        break;

                    case "hp":
                        source.HP = dataReader.GetInt16(i);
                        break;

                    case "id":
                        source.ID = (ItemTemplateID)dataReader.GetUInt16(i);
                        break;

                    case "mp":
                        source.MP = dataReader.GetInt16(i);
                        break;

                    case "name":
                        source.Name = dataReader.GetString(i);
                        break;

                    case "range":
                        source.Range = dataReader.GetUInt16(i);
                        break;

                    case "stat_agi":
                        source.SetStat(StatType.Agi, dataReader.GetInt16(i));
                        break;

                    case "stat_defence":
                        source.SetStat(StatType.Defence, dataReader.GetInt16(i));
                        break;

                    case "stat_int":
                        source.SetStat(StatType.Int, dataReader.GetInt16(i));
                        break;

                    case "stat_maxhit":
                        source.SetStat(StatType.MaxHit, dataReader.GetInt16(i));
                        break;

                    case "stat_maxhp":
                        source.SetStat(StatType.MaxHP, dataReader.GetInt16(i));
                        break;

                    case "stat_maxmp":
                        source.SetStat(StatType.MaxMP, dataReader.GetInt16(i));
                        break;

                    case "stat_minhit":
                        source.SetStat(StatType.MinHit, dataReader.GetInt16(i));
                        break;

                    case "stat_req_agi":
                        source.SetReqStat(StatType.Agi, dataReader.GetInt16(i));
                        break;

                    case "stat_req_int":
                        source.SetReqStat(StatType.Int, dataReader.GetInt16(i));
                        break;

                    case "stat_req_str":
                        source.SetReqStat(StatType.Str, dataReader.GetInt16(i));
                        break;

                    case "stat_str":
                        source.SetStat(StatType.Str, dataReader.GetInt16(i));
                        break;

                    case "type":
                        source.Type = (ItemType)dataReader.GetByte(i);
                        break;

                    case "value":
                        source.Value = dataReader.GetInt32(i);
                        break;

                    case "weapon_type":
                        source.WeaponType = (WeaponType)dataReader.GetByte(i);
                        break;

                    case "width":
                        source.Width = dataReader.GetByte(i);
                        break;
                }
            }
        }
    }
}