using System;
using System.Linq;
using NetGore.Features.Guilds;

namespace DemoGame.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `guild`.
    /// </summary>
    public interface IGuildTable
    {
        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        GuildID ID { get; }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the value of the database column `tag`.
        /// </summary>
        String Tag { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IGuildTable DeepCopy();
    }
}