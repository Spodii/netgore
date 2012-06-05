using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Base class for performing actions on an <see cref="IGuildMember"/> using just their name. This object abstracts
    /// whether a <see cref="IGuildMember"/> object is coming from memory or has to be loaded from the database.
    /// </summary>
    public abstract class GuildMemberPerformerBase
    {
        readonly ObjectPool<TemporaryGuildMember> _pool;
        readonly Action<IGuildMember> _saveHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberPerformerBase"/> class.
        /// </summary>
        /// <param name="saveHandler">Delegate describing how to implement the
        /// <see cref="IGuildMember.SaveGuildInformation"/> method on the <see cref="IGuildMember"/> when the
        /// <see cref="IGuildMember"/> object reference has to be constructed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="saveHandler" /> is <c>null</c>.</exception>
        protected GuildMemberPerformerBase(Action<IGuildMember> saveHandler)
        {
            if (saveHandler == null)
                throw new ArgumentNullException("saveHandler");

            _pool = new ObjectPool<TemporaryGuildMember>(x => new TemporaryGuildMember(this), null, x => x.Clear(), true);
            _saveHandler = saveHandler;
        }

        /// <summary>
        /// Gets the <see cref="IGuildMember"/> with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="IGuildMember"/>.</param>
        /// <param name="mustFree">If true, the returned <see cref="IGuildMember"/> is a
        /// <see cref="TemporaryGuildMember"/> and must be freed.</param>
        /// <returns>The <see cref="IGuildMember"/> for the guild member with the given <paramref name="name"/>,
        /// or null if the guild member could not be loaded or does not exist.</returns>
        IGuildMember Acquire(string name, out bool mustFree)
        {
            IGuildMember guildMember;

            // Acquire existing reference
            if (TryGetGuildMember(name, out guildMember))
            {
                mustFree = false;
                return guildMember;
            }

            mustFree = true;

            // Try to load from storage
            TemporaryGuildMemberPoolValues values;
            if (!TryLoadGuildMember(name, out values))
                return null;

            var ret = _pool.Acquire();
            ret.Initialize(ref values);

            return ret;
        }

        /// <summary>
        /// Performs an action on a guild member with the given name.
        /// </summary>
        /// <param name="name">The name of the guild member to perform the action on.</param>
        /// <param name="action">The action to perform on the <see cref="IGuildMember"/>. It is vital that the
        /// <see cref="IGuildMember"/> parameter in the <see cref="Action{T}"/> is not referenced outside
        /// of the scope of the delegate. If the <paramref name="name"/> did not yield a valid <see cref="IGuildMember"/>
        /// then a null <see cref="IGuildMember"/> will be passed to this action.</param>
        public void Perform(string name, Action<IGuildMember> action)
        {
            bool mustFree;

            // Get the guild member
            var guildMember = Acquire(name, out mustFree);

            // Perform the action even if the object is null
            action(guildMember);

            // Free the object back into the pool if needed
            if (mustFree && guildMember != null)
                _pool.Free((TemporaryGuildMember)guildMember);
        }

        /// <summary>
        /// When overridden in the derived class, tries to acquire the guild member with the given <paramref name="name"/>
        /// who's object already exists in memory.
        /// </summary>
        /// <param name="name">The name of the <see cref="IGuildMember"/> to get.</param>
        /// <param name="guildMember">When this method returns true, contains the <see cref="IGuildMember"/>
        /// for the guild member with the given <paramref name="name"/>.</param>
        /// <returns>True if the <see cref="IGuildMember"/> with the given <paramref name="name"/> was successfully
        /// loaded; otherwise false.</returns>
        protected abstract bool TryGetGuildMember(string name, out IGuildMember guildMember);

        /// <summary>
        /// When overridden in the derived class, tries to load the guild member with the given <paramref name="name"/>
        /// from an external data source (such as the database). This will only be called when
        /// <see cref="GuildMemberPerformerBase.TryGetGuildMember"/> fails.
        /// </summary>
        /// <param name="name">The name of the <see cref="IGuildMember"/> to get.</param>
        /// <param name="values">When this method returns true, contains the needed loaded values for the
        /// <see cref="IGuildMember"/>.</param>
        /// <returns>True if the <see cref="IGuildMember"/> with the given <paramref name="name"/> was successfully
        /// loaded; otherwise false.</returns>
        protected abstract bool TryLoadGuildMember(string name, out TemporaryGuildMemberPoolValues values);

        /// <summary>
        /// An implementation of <see cref="IGuildMember"/> that is intended for temporary usage only. The primary purpose
        /// of this class is to be able to create a <see cref="IGuildMember"/> object for a guild member who's
        /// object instance does not exist in memory already (such as an offline user). A reference to this object
        /// should never be held for a prolonged period.
        /// </summary>
        class TemporaryGuildMember : IGuildMember, IPoolable
        {
            readonly GuildMemberPerformerBase _pool;

            IGuild _guild;
            int _id;
            string _name;
            GuildRank _rank;

            /// <summary>
            /// Initializes a new instance of the <see cref="TemporaryGuildMember"/> class.
            /// </summary>
            internal TemporaryGuildMember(GuildMemberPerformerBase pool)
            {
                _pool = pool;
            }

            /// <summary>
            /// Clears the loaded values.
            /// </summary>
            public void Clear()
            {
                // Clear any object references to ensure we don't end up preventing any garbage collection
                _guild = null;
            }

            /// <summary>
            /// Initializes the <see cref="TemporaryGuildMember"/>'s values.
            /// </summary>
            /// <param name="values">The values to use.</param>
            public void Initialize(ref TemporaryGuildMemberPoolValues values)
            {
                _name = values.Name;
                _id = values.ID;
                _guild = values.Guild;
                _rank = values.Rank;
            }

            #region IGuildMember Members

            /// <summary>
            /// Gets or sets the guild member's current guild. Will be null if they are not part of any guilds.
            /// This value should only be set by the <see cref="IGuildManager"/>. When the value is changed,
            /// <see cref="IGuild.RemoveOnlineMember"/> should be called for the old value (if not null) and
            /// <see cref="IGuild.AddOnlineMember"/> should be called for the new value (if not null).
            /// </summary>
            public IGuild Guild
            {
                get { return _guild; }
                set { _guild = value; }
            }

            /// <summary>
            /// Gets or sets the guild member's ranking in the guild. Only valid if in a guild.
            /// This value should only be set by the <see cref="IGuildManager"/>.
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is greater than the maximum
            /// rank value.</exception>
            public GuildRank GuildRank
            {
                get { return _rank; }
                set { _rank = value; }
            }

            /// <summary>
            /// Gets an ID that can be used to distinguish this <see cref="IGuildMember"/> from any other
            /// <see cref="IGuildMember"/> instance.
            /// </summary>
            public int ID
            {
                get { return _id; }
            }

            /// <summary>
            /// Gets the unique name of the guild member.
            /// </summary>
            public string Name
            {
                get { return _name; }
            }

            /// <summary>
            /// Saves the guild member's information.
            /// </summary>
            public void SaveGuildInformation()
            {
                _pool._saveHandler(this);
            }

            /// <summary>
            /// Notifies the guild member that they have been invited to join a guild.
            /// </summary>
            /// <param name="inviter">The guild member that invited them into the guild.</param>
            /// <param name="guild">The guild they are being invited to join.</param>
            public void SendGuildInvite(IGuildMember inviter, IGuild guild)
            {
            }

            #endregion

            #region IPoolable Members

            /// <summary>
            /// Gets or sets the index of the object in the pool. This value should never be used by anything
            /// other than the pool that owns this object.
            /// </summary>
            int IPoolable.PoolIndex { get; set; }

            #endregion
        }

        /// <summary>
        /// A struct that contains the values required to create an <see cref="IGuildMember"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public struct TemporaryGuildMemberPoolValues : IEquatable<TemporaryGuildMemberPoolValues>
        {
            readonly IGuild _guild;
            readonly int _id;
            readonly string _name;
            readonly GuildRank _rank;

            /// <summary>
            /// Initializes a new instance of the <see cref="TemporaryGuildMemberPoolValues"/> struct.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="id">The id.</param>
            /// <param name="guild">The guild.</param>
            /// <param name="rank">The rank.</param>
            public TemporaryGuildMemberPoolValues(string name, int id, IGuild guild, GuildRank rank)
            {
                _name = name;
                _id = id;
                _guild = guild;
                _rank = rank;
            }

            /// <summary>
            /// Gets the guild.
            /// </summary>
            /// <value>The guild.</value>
            public IGuild Guild
            {
                get { return _guild; }
            }

            /// <summary>
            /// Gets the ID.
            /// </summary>
            /// <value>The ID.</value>
            public int ID
            {
                get { return _id; }
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name
            {
                get { return _name; }
            }

            /// <summary>
            /// Gets the rank.
            /// </summary>
            /// <value>The rank.</value>
            public GuildRank Rank
            {
                get { return _rank; }
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            public bool Equals(TemporaryGuildMemberPoolValues other)
            {
                return Equals(other._guild, _guild) && other._id == _id && Equals(other._name, _name) && other._rank.Equals(_rank);
            }

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <param name="obj">Another object to compare to.</param>
            /// <returns>
            /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                return obj is TemporaryGuildMemberPoolValues && this == (TemporaryGuildMemberPoolValues)obj;
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            /// <returns>
            /// A 32-bit signed integer that is the hash code for this instance.
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    var result = (_guild != null ? _guild.GetHashCode() : 0);
                    result = (result * 397) ^ _id;
                    result = (result * 397) ^ (_name != null ? _name.GetHashCode() : 0);
                    result = (result * 397) ^ _rank.GetHashCode();
                    return result;
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(TemporaryGuildMemberPoolValues left, TemporaryGuildMemberPoolValues right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(TemporaryGuildMemberPoolValues left, TemporaryGuildMemberPoolValues right)
            {
                return !left.Equals(right);
            }
        }
    }
}