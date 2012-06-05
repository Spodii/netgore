using System;
using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Extension methods for the <see cref="IGuildManager"/>.
    /// </summary>
    public static class IGuildManagerExtensions
    {
        /// <summary>
        /// Logs an event from a guild.
        /// </summary>
        /// <param name="guildManager">The <see cref="IGuildManager"/>.</param>
        /// <param name="eventCreator">The guild member that created the event.</param>
        /// <param name="guildEvent">The type of event that took place.</param>
        /// <param name="eventTarget">Optionally contains the other guild member that the event involves. This member
        /// may or may not actually be in the guild at this point.</param>
        /// <param name="arg0">The optional first argument string.</param>
        /// <param name="arg1">The optional second argument string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="eventCreator"/> is null.</exception>
        public static void LogEvent(this IGuildManager guildManager, IGuildMember eventCreator, GuildEvents guildEvent,
                                    IGuildMember eventTarget, string arg0, string arg1)
        {
            guildManager.LogEvent(eventCreator, guildEvent, eventTarget, arg0, arg1, null);
        }

        /// <summary>
        /// Logs an event from a guild.
        /// </summary>
        /// <param name="guildManager">The <see cref="IGuildManager"/>.</param>
        /// <param name="eventCreator">The guild member that created the event.</param>
        /// <param name="guildEvent">The type of event that took place.</param>
        /// <param name="eventTarget">Optionally contains the other guild member that the event involves. This member
        /// may or may not actually be in the guild at this point.</param>
        /// <param name="arg0">The optional first argument string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="eventCreator"/> is null.</exception>
        public static void LogEvent(this IGuildManager guildManager, IGuildMember eventCreator, GuildEvents guildEvent,
                                    IGuildMember eventTarget, string arg0)
        {
            guildManager.LogEvent(eventCreator, guildEvent, eventTarget, arg0, null, null);
        }

        /// <summary>
        /// Logs an event from a guild.
        /// </summary>
        /// <param name="guildManager">The <see cref="IGuildManager"/>.</param>
        /// <param name="eventCreator">The guild member that created the event.</param>
        /// <param name="guildEvent">The type of event that took place.</param>
        /// <param name="eventTarget">Optionally contains the other guild member that the event involves. This member
        /// may or may not actually be in the guild at this point.</param>
        /// <exception cref="ArgumentNullException"><paramref name="eventCreator"/> is null.</exception>
        public static void LogEvent(this IGuildManager guildManager, IGuildMember eventCreator, GuildEvents guildEvent,
                                    IGuildMember eventTarget)
        {
            guildManager.LogEvent(eventCreator, guildEvent, eventTarget, null, null, null);
        }

        /// <summary>
        /// Logs an event from a guild.
        /// </summary>
        /// <param name="guildManager">The <see cref="IGuildManager"/>.</param>
        /// <param name="eventCreator">The guild member that created the event.</param>
        /// <param name="guildEvent">The type of event that took place.</param>
        /// <exception cref="ArgumentNullException"><paramref name="eventCreator"/> is null.</exception>
        public static void LogEvent(this IGuildManager guildManager, IGuildMember eventCreator, GuildEvents guildEvent)
        {
            guildManager.LogEvent(eventCreator, guildEvent, null, null, null, null);
        }
    }
}