using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Interface for an object that manages the guilds.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IGuild"/>.</typeparam>
    public interface IGuildManager<T> : IGuildManager, IEnumerable<KeyValuePair<GuildID, T>> where T : class, IGuild
    {
        /// <summary>
        /// Gets the guild with the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the guild to get.</param>
        /// <returns>The guild with the specified <paramref name="id"/>, or null if the guild does not exist in this
        /// guild manager.</returns>
        T GetGuild(GuildID id);

        /// <summary>
        /// Gets the guild with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the guild to get.</param>
        /// <returns>The guild with the specified <paramref name="name"/>, or null if the guild does not exist in this
        /// guild manager.</returns>
        T GetGuild(string name);

        /// <summary>
        /// Tries to create a new guild.
        /// </summary>
        /// <param name="creator">The <see cref="IGuildMember"/> describing the object trying to create the guild.</param>
        /// <param name="name">The name of the guild to create.</param>
        /// <param name="tag">The tag for the guild to create.</param>
        /// <returns>The created guild instance if successfully created, or null if the guild could not
        /// be created.</returns>
        T TryCreateGuild(IGuildMember creator, string name, string tag);
    }

    /// <summary>
    /// Interface for an object that manages the guilds.
    /// </summary>
    public interface IGuildManager : IDisposable
    {
        /// <summary>
        /// Gets if the <paramref name="name"/> is an available guild name.
        /// </summary>
        /// <param name="name">The guild name to check if available.</param>
        /// <returns>True if the <paramref name="name"/> is available; otherwise false.</returns>
        bool IsNameAvailable(string name);

        /// <summary>
        /// Gets if the <paramref name="tag"/> is an available guild tag.
        /// </summary>
        /// <param name="tag">The guild tag to check if available.</param>
        /// <returns>True if the <paramref name="tag"/> is available; otherwise false.</returns>
        bool IsTagAvailable(string tag);

        /// <summary>
        /// Logs an event from a guild.
        /// </summary>
        /// <param name="eventCreator">The guild member that created the event.</param>
        /// <param name="guildEvent">The type of event that took place.</param>
        /// <param name="eventTarget">Optionally contains the other guild member that the event involves. This member
        /// may or may not actually be in the guild at this point.</param>
        /// <param name="arg0">The optional first argument string.</param>
        /// <param name="arg1">The optional second argument string.</param>
        /// <param name="arg2">The optional third argument string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="eventCreator"/> is null.</exception>
        void LogEvent(IGuildMember eventCreator, GuildEvents guildEvent, IGuildMember eventTarget, string arg0, string arg1,
                      string arg2);
    }
}