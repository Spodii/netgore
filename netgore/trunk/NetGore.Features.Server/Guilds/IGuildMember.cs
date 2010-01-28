using System;
using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Interface for an object that is allowed to be part of a guild.
    /// </summary>
    public interface IGuildMember
    {
        /// <summary>
        /// Gets or sets the guild member's current guild. Will be null if they are not part of any guilds.
        /// This value should only be set by the <see cref="IGuildManager"/>. When the value is changed,
        /// <see cref="IGuild.RemoveOnlineMember"/> should be called for the old value (if not null) and
        /// <see cref="IGuild.AddOnlineMember"/> should be called for the new value (if not null).
        /// </summary>
        IGuild Guild { get; set; }

        /// <summary>
        /// Gets or sets the guild member's ranking in the guild. Only valid if in a guild.
        /// This value should only be set by the <see cref="IGuildManager"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is greater than the maximum
        /// rank value.</exception>
        GuildRank GuildRank { get; set; }

        /// <summary>
        /// Gets an ID that can be used to distinguish this <see cref="IGuildMember"/> from any other
        /// <see cref="IGuildMember"/> instance.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Gets the unique name of the guild member.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Saves the guild member's information.
        /// </summary>
        void SaveGuildInformation();

        /// <summary>
        /// Notifies the guild member that they have been invited to join a guild.
        /// </summary>
        /// <param name="inviter">The guild member that invited them into the guild.</param>
        /// <param name="guild">The guild they are being invited to join.</param>
        void SendGuildInvite(IGuildMember inviter, IGuild guild);
    }
}