using System;
using System.Linq;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `server_time`.
    /// </summary>
    public interface IServerTimeTable
    {
        /// <summary>
        /// Gets the value of the database column `server_time`.
        /// </summary>
        DateTime ServerTime { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IServerTimeTable DeepCopy();
    }
}