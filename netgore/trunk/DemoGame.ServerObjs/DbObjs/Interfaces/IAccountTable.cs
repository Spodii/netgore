using System;
using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `account`.
    /// </summary>
    public interface IAccountTable
    {
        /// <summary>
        /// Gets the value of the database column `current_ip`.
        /// </summary>
        uint? CurrentIp { get; }

        /// <summary>
        /// Gets the value of the database column `email`.
        /// </summary>
        String Email { get; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        AccountID ID { get; }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the value of the database column `password`.
        /// </summary>
        String Password { get; }

        /// <summary>
        /// Gets the value of the database column `time_created`.
        /// </summary>
        DateTime TimeCreated { get; }

        /// <summary>
        /// Gets the value of the database column `time_last_login`.
        /// </summary>
        DateTime TimeLastLogin { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IAccountTable DeepCopy();
    }
}