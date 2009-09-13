using System;
using System.Linq;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `log_account_activity`.
    /// </summary>
    public interface ILogAccountActivityTable
    {
        /// <summary>
        /// Gets the value of the database column `account_id`.
        /// </summary>
        Int32 AccountId { get; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        Int32 ID { get; }

        /// <summary>
        /// Gets the value of the database column `ip`.
        /// </summary>
        UInt32 Ip { get; }

        /// <summary>
        /// Gets the value of the database column `time_login`.
        /// </summary>
        DateTime TimeLogin { get; }

        /// <summary>
        /// Gets the value of the database column `time_logout`.
        /// </summary>
        DateTime? TimeLogout { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        ILogAccountActivityTable DeepCopy();
    }
}