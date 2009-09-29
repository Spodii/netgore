using System;
using System.Linq;
using DemoGame;
using NetGore;
using NetGore.RPGComponents;

namespace DemoGame.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `map`.
    /// </summary>
    public interface IMapTable
    {
        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        MapIndex ID { get; }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IMapTable DeepCopy();
    }
}