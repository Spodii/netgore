using System;
using System.Linq;
using NetGore;
using NetGore.AI;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `npc_character`.
    /// </summary>
    public interface INpcCharacterTable
    {
        /// <summary>
        /// Gets the value of the database column `account_id`.
        /// </summary>
        AccountID? AccountID { get; }

        /// <summary>
        /// Gets the value of the database column `ai_id`.
        /// </summary>
        AIID? AIID { get; }

        /// <summary>
        /// Gets the value of the database column `body_id`.
        /// </summary>
        BodyIndex BodyID { get; }

        /// <summary>
        /// Gets the value of the database column `cash`.
        /// </summary>
        Int32 Cash { get; }

        /// <summary>
        /// Gets the value of the database column `character_template_id`.
        /// </summary>
        CharacterTemplateID? CharacterTemplateID { get; }

        /// <summary>
        /// Gets the value of the database column `chat_dialog`.
        /// </summary>
        ushort? ChatDialog { get; }

        /// <summary>
        /// Gets the value of the database column `exp`.
        /// </summary>
        Int32 Exp { get; }

        /// <summary>
        /// Gets the value of the database column `hp`.
        /// </summary>
        SPValueType HP { get; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        Int32 ID { get; }

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
        SPValueType MP { get; }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        String Name { get; }

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
        /// Gets the value of the database column `shop_id`.
        /// </summary>
        ShopID? ShopID { get; }

        /// <summary>
        /// Gets the value of the database column `stat_agi`.
        /// </summary>
        Int16 StatAgi { get; }

        /// <summary>
        /// Gets the value of the database column `stat_defence`.
        /// </summary>
        Int16 StatDefence { get; }

        /// <summary>
        /// Gets the value of the database column `stat_int`.
        /// </summary>
        Int16 StatInt { get; }

        /// <summary>
        /// Gets the value of the database column `stat_maxhit`.
        /// </summary>
        Int16 StatMaxhit { get; }

        /// <summary>
        /// Gets the value of the database column `stat_maxhp`.
        /// </summary>
        Int16 StatMaxhp { get; }

        /// <summary>
        /// Gets the value of the database column `stat_maxmp`.
        /// </summary>
        Int16 StatMaxmp { get; }

        /// <summary>
        /// Gets the value of the database column `stat_minhit`.
        /// </summary>
        Int16 StatMinhit { get; }

        /// <summary>
        /// Gets the value of the database column `statpoints`.
        /// </summary>
        Int32 StatPoints { get; }

        /// <summary>
        /// Gets the value of the database column `stat_str`.
        /// </summary>
        Int16 StatStr { get; }

        /// <summary>
        /// Gets the value of the database column `x`.
        /// </summary>
        Single X { get; }

        /// <summary>
        /// Gets the value of the database column `y`.
        /// </summary>
        Single Y { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        INpcCharacterTable DeepCopy();
    }
}