using System.Linq;
using NetGore.Collections;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Contains the status of the live and outstanding guild invites.
    /// </summary>
    public class GuildInviteStatus : IPoolable
    {
        IGuild _guild;
        TickCount _inviteTime;

        /// <summary>
        /// Gets the guild the invite is for.
        /// </summary>
        public IGuild Guild
        {
            get { return _guild; }
        }

        /// <summary>
        /// Gets when the invite to the guild was made.
        /// </summary>
        public TickCount InviteTime
        {
            get { return _inviteTime; }
            internal set { _inviteTime = value; }
        }

        /// <summary>
        /// Creates a deep copy of this object. This is recommended for whenever you want to hold onto a reference
        /// of this object for an extended period without having to worry about if it changes.
        /// </summary>
        /// <returns>A deep copy of this object.</returns>
        public GuildInviteStatus DeepCopy()
        {
            var ret = new GuildInviteStatus();
            ret.Initialize(_guild, _inviteTime);
            return ret;
        }

        /// <summary>
        /// Initializes the values of this object.
        /// </summary>
        /// <param name="guild">The guild.</param>
        /// <param name="inviteTime">The invite time.</param>
        internal void Initialize(IGuild guild, TickCount inviteTime)
        {
            _guild = guild;
            _inviteTime = inviteTime;
        }

        /// <summary>
        /// Resets the values of this object.
        /// </summary>
        internal void Reset()
        {
            _guild = null;
            _inviteTime = TickCount.MinValue;
        }

        #region IPoolable Members

        /// <summary>
        /// Gets or sets the index of the object in the pool. This value should never be used by anything
        /// other than the pool that owns this object.
        /// </summary>
        int IPoolable.PoolIndex { get; set; }

        #endregion
    }
}