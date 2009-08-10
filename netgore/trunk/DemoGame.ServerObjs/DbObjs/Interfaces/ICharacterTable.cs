using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `character`.
    /// </summary>
    public interface ICharacterTable
    {
        /// <summary>
        /// Gets the value of the database column `body_id`.
        /// </summary>
        BodyIndex BodyID { get; }

        /// <summary>
        /// Gets the value of the database column `cash`.
        /// </summary>
        UInt32 Cash { get; }

        /// <summary>
        /// Gets the value of the database column `character_template_id`.
        /// </summary>
        CharacterTemplateID? CharacterTemplateID { get; }

        /// <summary>
        /// Gets the value of the database column `exp`.
        /// </summary>
        UInt32 Exp { get; }

        /// <summary>
        /// Gets the value of the database column `hp`.
        /// </summary>
        Int16 HP { get; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        CharacterID ID { get; }

        /// <summary>
        /// Gets the value of the database column `level`.
        /// </summary>
        Byte Level { get; }

        /// <summary>
        /// Gets the value of the database column `map_id`.
        /// </summary>
        MapIndex MapID { get; }

        /// <summary>
        /// Gets the value of the database column `mp`.
        /// </summary>
        Int16 MP { get; }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the value of the database column `password`.
        /// </summary>
        String Password { get; }

        /// <summary>
        /// Gets the value of the database column `respawn_map`.
        /// </summary>
        MapIndex? RespawnMap { get; }

        /// <summary>
        /// Gets the value of the database column `respawn_x`.
        /// </summary>
        Single RespawnX { get; }

        /// <summary>
        /// Gets the value of the database column `respawn_y`.
        /// </summary>
        Single RespawnY { get; }

        /// <summary>
        /// Gets the value of the database column `statpoints`.
        /// </summary>
        UInt32 StatPoints { get; }

        IEnumerable<KeyValuePair<StatType, Int32>> Stats { get; }

        /// <summary>
        /// Gets the value of the database column `x`.
        /// </summary>
        Single X { get; }

        /// <summary>
        /// Gets the value of the database column `y`.
        /// </summary>
        Single Y { get; }

        /// <summary>
        /// Gets the value of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        Int32 GetStat(StatType key);

        /// <summary>
        /// Gets the <paramref name="value"/> of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <param name="value">The value to assign to the column with the corresponding <paramref name="key"/>.</param>
        void SetStat(StatType key, Int32 value);
    }
}