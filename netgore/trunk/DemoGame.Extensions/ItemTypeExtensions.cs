using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;

namespace DemoGame.Extensions
{
    /// <summary>
    /// Extensions for the DemoGame.ItemType.
    /// </summary>
    public static class ItemTypeExtensions
    {
        /// <summary>
        /// Checks if a specified ItemType value is defined by the ItemType enum.
        /// </summary>
        /// <param name="itemType">ItemType value to check.</param>
        /// <returns>True if the <paramref name="itemType"/> is defined in the ItemType enum, else false.</returns>
        public static bool IsDefined(this ItemType itemType)
        {
            return Enum.IsDefined(typeof(ItemType), itemType);
        }
    }
}