using System;
using System.Linq;
using NetGore;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `map_spawn`.
    /// </summary>
    public interface IMapSpawnTable
    {
        /// <summary>
        /// Gets the value of the database column `amount`.
        /// </summary>
        Byte Amount { get; }

        /// <summary>
        /// Gets the value of the database column `character_template_id`.
        /// </summary>
        CharacterTemplateID CharacterTemplateID { get; }

        /// <summary>
        /// Gets the value of the database column `height`.
        /// </summary>
        ushort? Height { get; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        MapSpawnValuesID ID { get; }

        /// <summary>
        /// Gets the value of the database column `map_id`.
        /// </summary>
        MapIndex MapID { get; }

        /// <summary>
        /// Gets the value of the database column `width`.
        /// </summary>
        ushort? Width { get; }

        /// <summary>
        /// Gets the value of the database column `x`.
        /// </summary>
        ushort? X { get; }

        /// <summary>
        /// Gets the value of the database column `y`.
        /// </summary>
        ushort? Y { get; }
    }
}