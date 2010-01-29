using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Handles an event from a guild related to a guild member.
    /// </summary>
    /// <param name="guild">The guild the event is related to.</param>
    /// <param name="guildMember">The guild member the event is related to.</param>
    public delegate void GuildMemberEventHandler(IGuild guild, IGuildMember guildMember);
}