using System;
using System.Linq;
using NetGore.Features.Guilds;

namespace DemoGame.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `guild_member`.
    /// </summary>
    public interface IGuildMemberTable
    {
        /// <summary>
        /// Gets the value of the database column `character_id`.
        /// </summary>
        CharacterID CharacterID { get; }

        /// <summary>
        /// Gets the value of the database column `guild_id`.
        /// </summary>
        GuildID GuildID { get; }

        /// <summary>
        /// Gets the value of the database column `joined`.
        /// </summary>
        DateTime Joined { get; }

        /// <summary>
        /// Gets the value of the database column `rank`.
        /// </summary>
        GuildRank Rank { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IGuildMemberTable DeepCopy();
    }
}