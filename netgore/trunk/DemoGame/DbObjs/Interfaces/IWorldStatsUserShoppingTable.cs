/********************************************************************
                   DO NOT MANUALLY EDIT THIS FILE!

This file was automatically generated using the DbClassCreator
program. The only time you should ever alter this file is if you are
using an automated code formatter. The DbClassCreator will overwrite
this file every time it is run, so all manual changes will be lost.
If there is something in this file that you wish to change, you should
be able to do it through the DbClassCreator arguments.

Make sure that you re-run the DbClassCreator every time you alter your
game's database.

For more information on the DbClassCreator, please see:
    http://www.netgore.com/wiki/dbclasscreator.html

This file was generated on (UTC): 5/11/2010 11:46:42 PM
********************************************************************/

using System;
using System.Linq;
using NetGore;
using NetGore.Features.Shops;

namespace DemoGame.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `world_stats_user_shopping`.
    /// </summary>
    public interface IWorldStatsUserShoppingTable
    {
        /// <summary>
        /// Gets the value of the database column `amount`.
        /// </summary>
        Byte Amount { get; }

        /// <summary>
        /// Gets the value of the database column `character_id`.
        /// </summary>
        CharacterID CharacterID { get; }

        /// <summary>
        /// Gets the value of the database column `cost`.
        /// </summary>
        Int32 Cost { get; }

        /// <summary>
        /// Gets the value of the database column `item_template_id`.
        /// </summary>
        ItemTemplateID? ItemTemplateID { get; }

        /// <summary>
        /// Gets the value of the database column `map_id`.
        /// </summary>
        MapID MapID { get; }

        /// <summary>
        /// Gets the value of the database column `sale_type`.
        /// </summary>
        SByte SaleType { get; }

        /// <summary>
        /// Gets the value of the database column `shop_id`.
        /// </summary>
        ShopID ShopID { get; }

        /// <summary>
        /// Gets the value of the database column `when`.
        /// </summary>
        DateTime When { get; }

        /// <summary>
        /// Gets the value of the database column `x`.
        /// </summary>
        UInt16 X { get; }

        /// <summary>
        /// Gets the value of the database column `y`.
        /// </summary>
        UInt16 Y { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IWorldStatsUserShoppingTable DeepCopy();
    }
}