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
using NetGore;

namespace DemoGame.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `item`.
    /// </summary>
    public interface IItemTable
    {
        /// <summary>
        /// Gets the value of the database column `amount`.
        /// </summary>
        Byte Amount { get; }

        /// <summary>
        /// Gets the value of the database column `description`.
        /// </summary>
        String Description { get; }

        /// <summary>
        /// Gets the value of the database column `equipped_body`.
        /// </summary>
        String EquippedBody { get; }

        /// <summary>
        /// Gets the value of the database column `graphic`.
        /// </summary>
        GrhIndex Graphic { get; }

        /// <summary>
        /// Gets the value of the database column `hp`.
        /// </summary>
        SPValueType HP { get; }

        /// <summary>
        /// Gets the value of the database column `height`.
        /// </summary>
        Byte Height { get; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        ItemID ID { get; }

        /// <summary>
        /// Gets the value of the database column `item_template_id`.
        /// </summary>
        ItemTemplateID? ItemTemplateID { get; }

        /// <summary>
        /// Gets the value of the database column `mp`.
        /// </summary>
        SPValueType MP { get; }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the value of the database column `range`.
        /// </summary>
        UInt16 Range { get; }

        /// <summary>
        /// Gets an IEnumerable of KeyValuePairs containing the values in the `ReqStat` collection. The
        /// key is the collection's key and the value is the value for that corresponding key.
        /// </summary>
        IEnumerable<KeyValuePair<StatType, Int32>> ReqStats { get; }

        /// <summary>
        /// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
        /// key is the collection's key and the value is the value for that corresponding key.
        /// </summary>
        IEnumerable<KeyValuePair<StatType, Int32>> Stats { get; }

        /// <summary>
        /// Gets the value of the database column `type`.
        /// </summary>
        ItemType Type { get; }

        /// <summary>
        /// Gets the value of the database column `value`.
        /// </summary>
        Int32 Value { get; }

        /// <summary>
        /// Gets the value of the database column `weapon_type`.
        /// </summary>
        WeaponType WeaponType { get; }

        /// <summary>
        /// Gets the value of the database column `width`.
        /// </summary>
        Byte Width { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IItemTable DeepCopy();

        /// <summary>
        /// Gets the value of the database column in the column collection `ReqStat`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        Int32 GetReqStat(StatType key);

        /// <summary>
        /// Gets the value of the database column in the column collection `Stat`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        Int32 GetStat(StatType key);
    }
}