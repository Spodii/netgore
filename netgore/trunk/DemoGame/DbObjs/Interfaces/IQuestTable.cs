using System;
using System.Linq;
using NetGore.Features.Quests;

namespace DemoGame.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `quest`.
    /// </summary>
    public interface IQuestTable
    {
        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        QuestID ID { get; }

        /// <summary>
        /// Gets the value of the database column `repeatable`.
        /// </summary>
        Boolean Repeatable { get; }

        /// <summary>
        /// Gets the value of the database column `reward_cash`.
        /// </summary>
        Int32 RewardCash { get; }

        /// <summary>
        /// Gets the value of the database column `reward_exp`.
        /// </summary>
        Int32 RewardExp { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IQuestTable DeepCopy();
    }
}