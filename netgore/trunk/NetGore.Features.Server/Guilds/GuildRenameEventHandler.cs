using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Handles an event from a guild related to a guild member changing the name or tag of the guild.
    /// </summary>
    /// <param name="guild">The guild the event is related to.</param>
    /// <param name="invoker">The guild member that invoked the event.</param>
    /// <param name="oldName">The old name or tag.</param>
    /// <param name="newName">The new name or tag.</param>
    public delegate void GuildRenameEventHandler(IGuild guild, IGuildMember invoker, string oldName, string newName);
}