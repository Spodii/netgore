using System;
using System.Linq;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `account_ips`.
    /// </summary>
    public interface IAccountIpsTable
    {
        /// <summary>
        /// Gets the value of the database column `account_id`.
        /// </summary>
        AccountID AccountID { get; }

        /// <summary>
        /// Gets the value of the database column `ip`.
        /// </summary>
        UInt32 Ip { get; }

        /// <summary>
        /// Gets the value of the database column `time`.
        /// </summary>
        DateTime Time { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IAccountIpsTable DeepCopy();
    }
}