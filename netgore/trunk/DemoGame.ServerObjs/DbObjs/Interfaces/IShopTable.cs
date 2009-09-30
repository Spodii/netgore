using System;
using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `shop`.
    /// </summary>
    public interface IShopTable
    {
        /// <summary>
        /// Gets the value of the database column `can_buy`.
        /// </summary>
        Boolean CanBuy { get; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        ShopID ID { get; }

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
        IShopTable DeepCopy();
    }
}