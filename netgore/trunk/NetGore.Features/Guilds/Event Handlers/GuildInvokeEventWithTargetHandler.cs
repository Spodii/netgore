using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Handles when a guild member invokes an event.
    /// </summary>
    /// <param name="invoker">The guild member that invoked the event.</param>
    /// <param name="target">The optional guild member the event involves.</param>
    public delegate void GuildInvokeEventWithTargetHandler(IGuildMember invoker, IGuildMember target);
}