using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Handles an event from a guild.
    /// </summary>
    /// <param name="guild">The guild the event is related to.</param>
    public delegate void GuildEventHandler(IGuild guild);
}