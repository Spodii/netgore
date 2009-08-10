using System;
using System.Linq;
using NetGore;

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
    }
}