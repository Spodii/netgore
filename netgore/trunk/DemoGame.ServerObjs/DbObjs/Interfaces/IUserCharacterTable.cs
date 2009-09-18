using System;
using System.Linq;
using NetGore;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `user_character`.
    /// </summary>
    public interface IUserCharacterTable
    {
        /// <summary>
        /// Gets the value of the database column `acc`.
        /// </summary>
        Byte Acc { get; }

        /// <summary>
        /// Gets the value of the database column `account_id`.
        /// </summary>
        AccountID? AccountID { get; }

        /// <summary>
        /// Gets the value of the database column `agi`.
        /// </summary>
        Byte Agi { get; }

        /// <summary>
        /// Gets the value of the database column `armor`.
        /// </summary>
        Byte Armor { get; }

        /// <summary>
        /// Gets the value of the database column `body_id`.
        /// </summary>
        BodyIndex BodyID { get; }

        /// <summary>
        /// Gets the value of the database column `bra`.
        /// </summary>
        Byte Bra { get; }

        /// <summary>
        /// Gets the value of the database column `cash`.
        /// </summary>
        UInt32 Cash { get; }

        /// <summary>
        /// Gets the value of the database column `character_template_id`.
        /// </summary>
        CharacterTemplateID? CharacterTemplateID { get; }

        /// <summary>
        /// Gets the value of the database column `chat_dialog`.
        /// </summary>
        ushort? ChatDialog { get; }

        /// <summary>
        /// Gets the value of the database column `defence`.
        /// </summary>
        Byte Defence { get; }

        /// <summary>
        /// Gets the value of the database column `dex`.
        /// </summary>
        Byte Dex { get; }

        /// <summary>
        /// Gets the value of the database column `evade`.
        /// </summary>
        Byte Evade { get; }

        /// <summary>
        /// Gets the value of the database column `exp`.
        /// </summary>
        UInt32 Exp { get; }

        /// <summary>
        /// Gets the value of the database column `hp`.
        /// </summary>
        SPValueType HP { get; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        Int32 ID { get; }

        /// <summary>
        /// Gets the value of the database column `imm`.
        /// </summary>
        Byte Imm { get; }

        /// <summary>
        /// Gets the value of the database column `int`.
        /// </summary>
        Byte Int { get; }

        /// <summary>
        /// Gets the value of the database column `level`.
        /// </summary>
        Byte Level { get; }

        /// <summary>
        /// Gets the value of the database column `map_id`.
        /// </summary>
        MapIndex MapID { get; }

        /// <summary>
        /// Gets the value of the database column `maxhit`.
        /// </summary>
        Byte MaxHit { get; }

        /// <summary>
        /// Gets the value of the database column `maxhp`.
        /// </summary>
        Int16 MaxHP { get; }

        /// <summary>
        /// Gets the value of the database column `maxmp`.
        /// </summary>
        Int16 MaxMP { get; }

        /// <summary>
        /// Gets the value of the database column `minhit`.
        /// </summary>
        Byte MinHit { get; }

        /// <summary>
        /// Gets the value of the database column `mp`.
        /// </summary>
        SPValueType MP { get; }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the value of the database column `perc`.
        /// </summary>
        Byte Perc { get; }

        /// <summary>
        /// Gets the value of the database column `recov`.
        /// </summary>
        Byte Recov { get; }

        /// <summary>
        /// Gets the value of the database column `regen`.
        /// </summary>
        Byte Regen { get; }

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

        /// <summary>
        /// Gets the value of the database column `str`.
        /// </summary>
        Byte Str { get; }

        /// <summary>
        /// Gets the value of the database column `tact`.
        /// </summary>
        Byte Tact { get; }

        /// <summary>
        /// Gets the value of the database column `ws`.
        /// </summary>
        Byte WS { get; }

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
        IUserCharacterTable DeepCopy();
    }
}