using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Enum of the different engine-defined guild events.
    /// </summary>
    public enum GuildEvents : byte
    {
        Rename = 0,
        Kick = 1,
        Invite = 2,
        Leave = 3,
        Promote = 4,
        Demote = 5
    }
}