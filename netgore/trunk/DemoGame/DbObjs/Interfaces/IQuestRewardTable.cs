using System;
using System.Linq;

namespace DemoGame.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `quest_reward`.
    /// </summary>
    public interface IQuestRewardTable
    {
        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        Int32 ID { get; }

        /// <summary>
        /// Gets the value of the database column `quest_id`.
        /// </summary>
        Int32 QuestId { get; }

        /// <summary>
        /// Gets the value of the database column `type`.
        /// </summary>
        Byte Type { get; }

        /// <summary>
        /// Gets the value of the database column `value`.
        /// </summary>
        String Value { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IQuestRewardTable DeepCopy();
    }
}