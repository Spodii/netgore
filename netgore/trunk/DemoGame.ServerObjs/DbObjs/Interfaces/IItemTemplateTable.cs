using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `item_template`.
    /// </summary>
    public interface IItemTemplateTable
    {
        /// <summary>
        /// Gets the value of the database column `description`.
        /// </summary>
        String Description { get; }

        /// <summary>
        /// Gets the value of the database column `graphic`.
        /// </summary>
        UInt16 Graphic { get; }

        /// <summary>
        /// Gets the value of the database column `height`.
        /// </summary>
        Byte Height { get; }

        /// <summary>
        /// Gets the value of the database column `hp`.
        /// </summary>
        UInt16 HP { get; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        ItemTemplateID ID { get; }

        /// <summary>
        /// Gets the value of the database column `mp`.
        /// </summary>
        UInt16 MP { get; }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        String Name { get; }

        IEnumerable<KeyValuePair<StatType, Int32>> ReqStats { get; }
        IEnumerable<KeyValuePair<StatType, Int32>> Stats { get; }

        /// <summary>
        /// Gets the value of the database column `type`.
        /// </summary>
        Byte Type { get; }

        /// <summary>
        /// Gets the value of the database column `value`.
        /// </summary>
        Int32 Value { get; }

        /// <summary>
        /// Gets the value of the database column `width`.
        /// </summary>
        Byte Width { get; }

        /// <summary>
        /// Gets the value of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        Int32 GetReqStat(StatType key);

        /// <summary>
        /// Gets the value of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        Int32 GetStat(StatType key);

        /// <summary>
        /// Gets the <paramref name="value"/> of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <param name="value">The value to assign to the column with the corresponding <paramref name="key"/>.</param>
        void SetReqStat(StatType key, Int32 value);

        /// <summary>
        /// Gets the <paramref name="value"/> of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <param name="value">The value to assign to the column with the corresponding <paramref name="key"/>.</param>
        void SetStat(StatType key, Int32 value);
    }
}