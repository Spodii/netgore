using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Base class for a guild manager.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IGuild"/>.</typeparam>
    public abstract class GuildManagerBase<T> : IGuildManager<T> where T : class, IGuild
    {
        static readonly GuildSettings _guildSettings = GuildSettings.Instance;

        /// <summary>
        /// Object used to lock on the guild creation.
        /// </summary>
        readonly object _createSync = new object();

        /// <summary>
        /// The collection of guilds, indexed by their <see cref="GuildID"/>.
        /// </summary>
        readonly TSDictionary<GuildID, T> _guilds = new TSDictionary<GuildID, T>();

        /// <summary>
        /// Object used to lock when accessing the <see cref="_guilds"/> collection.
        /// </summary>
        readonly object _guildsSync = new object();

        /// <summary>
        /// Loads all the guild information into the guild manager. This should only be called once, preferably in
        /// the object's constructor.
        /// </summary>
        protected void Initialize()
        {
            var guilds = LoadGuilds();

            lock (_guildsSync)
            {
                Debug.Assert(_guilds.IsEmpty(), "Expected the guilds collection to be empty...");

                foreach (var guild in guilds)
                {
                    Debug.Assert(!_guilds.ContainsKey(guild.ID), string.Format("Duplicate guild ID found for ID `{0}`.", guild.ID));
                    _guilds.Add(guild.ID, guild);
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets if the <paramref name="name"/> is an available guild name.
        /// </summary>
        /// <param name="name">The guild name to check if available.</param>
        /// <returns>True if the <paramref name="name"/> is available; otherwise false.</returns>
        protected abstract bool InternalIsNameAvailable(string name);

        /// <summary>
        /// When overridden in the derived class, gets if the <paramref name="tag"/> is an available guild tag.
        /// </summary>
        /// <param name="tag">The guild tag to check if available.</param>
        /// <returns>True if the <paramref name="tag"/> is available; otherwise false.</returns>
        protected abstract bool InternalIsTagAvailable(string tag);

        /// <summary>
        /// When overridden in the derived class, logs an event from a guild.
        /// </summary>
        /// <param name="eventCreator">The guild member that created the event.</param>
        /// <param name="guildEvent">The type of event that took place.</param>
        /// <param name="eventTarget">Optionally contains the other guild member that the event involves. This member
        /// may or may not actually be in the guild at this point.</param>
        /// <param name="arg0">The optional first argument string.</param>
        /// <param name="arg1">The optional second argument string.</param>
        /// <param name="arg2">The optional third argument string.</param>
        protected abstract void InternalLogEvent(IGuildMember eventCreator, GuildEvents guildEvent, IGuildMember eventTarget,
                                                 string arg0, string arg1, string arg2);

        /// <summary>
        /// Tries to create a new guild.
        /// </summary>
        /// <param name="creator">The one trying to create the guild.</param>
        /// <param name="name">The name of the guild to create.</param>
        /// <param name="tag">The tag for the guild to create.</param>
        /// <returns>The created guild instance if successfully created, or null if the guild could not
        /// be created.</returns>
        protected abstract T InternalTryCreateGuild(IGuildMember creator, string name, string tag);

        /// <summary>
        /// Gets if the <paramref name="creator"/> is allowed to create a guild.
        /// </summary>
        /// <param name="creator">The one trying to create the guild.</param>
        /// <returns>True if the <paramref name="creator"/> is allowed to create a guild; otherwise false.</returns>
        protected virtual bool IsValidGuildCreator(IGuildMember creator)
        {
            if (creator.Guild != null)
                return false;

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, gets all of the guilds loaded from the database.
        /// </summary>
        /// <returns>All of the guilds loaded from the database.</returns>
        protected abstract IEnumerable<T> LoadGuilds();

        #region IGuildManager<T> Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var guild in this.Select(x => x.Value))
            {
                guild.Dispose();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<GuildID, T>> GetEnumerator()
        {
            return _guilds.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the guild with the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the guild to get.</param>
        /// <returns>The guild with the specified <paramref name="id"/>, or null if the guild does not exist in this
        /// guild manager.</returns>
        public T GetGuild(GuildID id)
        {
            T ret;

            lock (_guildsSync)
            {
                if (!_guilds.TryGetValue(id, out ret))
                    ret = null;
            }

            return ret;
        }

        /// <summary>
        /// Gets the guild with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the guild to get.</param>
        /// <returns>The guild with the specified <paramref name="name"/>, or null if the guild does not exist in this
        /// guild manager.</returns>
        public T GetGuild(string name)
        {
            if (!_guildSettings.IsValidName(name))
                return null;

            lock (_guildsSync)
            {
                return _guilds.Values.FirstOrDefault(x => StringComparer.OrdinalIgnoreCase.Equals(name, x.Name));
            }
        }

        /// <summary>
        /// Gets if the <paramref name="name"/> is an available guild name.
        /// </summary>
        /// <param name="name">The guild name to check if available.</param>
        /// <returns>True if the <paramref name="name"/> is available; otherwise false.</returns>
        public bool IsNameAvailable(string name)
        {
            if (!_guildSettings.IsValidName(name))
                return false;

            return InternalIsNameAvailable(name);
        }

        /// <summary>
        /// Gets if the <paramref name="tag"/> is an available guild tag.
        /// </summary>
        /// <param name="tag">The guild tag to check if available.</param>
        /// <returns>True if the <paramref name="tag"/> is available; otherwise false.</returns>
        public bool IsTagAvailable(string tag)
        {
            if (!_guildSettings.IsValidTag(tag))
                return false;

            return InternalIsTagAvailable(tag);
        }

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
        public void LogEvent(IGuildMember eventCreator, GuildEvents guildEvent, IGuildMember eventTarget, string arg0, string arg1,
                             string arg2)
        {
            if (eventCreator == null)
                throw new ArgumentNullException("eventCreator");

            Debug.Assert(EnumHelper<GuildEvents>.IsDefined(guildEvent));

            InternalLogEvent(eventCreator, guildEvent, eventTarget, arg0, arg1, arg2);
        }

        /// <summary>
        /// Tries to create a new guild.
        /// </summary>
        /// <param name="creator">The <see cref="IGuildMember"/> describing the object trying to create the guild.</param>
        /// <param name="name">The name of the guild to create.</param>
        /// <param name="tag">The tag for the guild to create.</param>
        /// <returns>The created guild instance if successfully created, or null if the guild could not
        /// be created.</returns>
        public T TryCreateGuild(IGuildMember creator, string name, string tag)
        {
            // Check for a valid creator
            if (!IsValidGuildCreator(creator))
                return null;

            T ret;

            // Lock to ensure that we don't have to worry about race conditions since its not like guilds are made
            // often anyways for this to be a performance hit
            lock (_createSync)
            {
                // Check that the name and tag are even available
                if (!IsNameAvailable(name))
                    return null;

                if (!IsTagAvailable(tag))
                    return null;

                // Let the derived class try to create the instance
                ret = InternalTryCreateGuild(creator, name, tag);
            }

            // Check that the guild was created successfully
            if (ret != null)
            {
                // Add the creator as the founder of the guild
                creator.GuildRank = _guildSettings.HighestRank;
                creator.Guild = ret;

                // Add the guild to the dictionary
                lock (_guildsSync)
                {
                    Debug.Assert(!_guilds.ContainsKey(ret.ID));
                    _guilds.Add(ret.ID, ret);
                }
            }

            return ret;
        }

        #endregion
    }
}