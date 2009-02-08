using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Platyform.Extensions;

namespace DemoGame
{
    /// <summary>
    /// Enum containing all of the different types of items.
    /// </summary>
    public enum ItemType : byte
    {
        // Each value is explicitly assigned to ensure the order does not change since these values are used
        // directly in the database.

        /// <summary>
        /// An item that can not be used at all in any way (ie items just for selling).
        /// </summary>
        Unusable = 0,

        /// <summary>
        /// An item that can be used, but only once (ie consumables).
        /// </summary>
        UseOnce = 1,

        /// <summary>
        /// A hand-held weapon.
        /// </summary>
        Weapon = 2,

        /// <summary>
        /// A helmet or other head-gear.
        /// </summary>
        Helmet = 3,

        /// <summary>
        /// Body armor or similar full-body gear.
        /// </summary>
        Body = 4
    }

    public static class ItemTypeExtensions
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static IEnumerable<EquipmentSlot> GetPossibleSlots(this ItemType itemType)
        {
            // FUTURE: Ugly hack - would be better if this was all defined better (attributes at least maybe)

            switch (itemType)
            {
                case ItemType.Body:
                    return new EquipmentSlot[] { EquipmentSlot.Body };

                case ItemType.Helmet:
                    return new EquipmentSlot[] { EquipmentSlot.Head };

                case ItemType.Weapon:
                    return new EquipmentSlot[] { EquipmentSlot.RightHand };

                case ItemType.Unusable:
                    return null;

                default:
                    const string errmsg = "Specified itemType `{0}` is not handled.";

                    Debug.Fail(string.Format(errmsg, itemType));
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, itemType);

                    return null;
            }
        }
    }
}