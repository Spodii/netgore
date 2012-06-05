using System;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Contains the settings for groups.
    /// </summary>
    public class GroupSettings
    {
        /// <summary>
        /// The settings instance.
        /// </summary>
        static GroupSettings _instance;

        readonly Func<IGroupable, IGroup, bool> _canJoinGroupHandler;

        readonly int _inviteResponseTime;
        readonly int _maxMembersPerGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupSettings"/> class.
        /// </summary>
        /// <param name="maxMembersPerGroup">The maximum number of members allowed in a single group.</param>
        /// <param name="inviteResponseTime">The amount of time in milliseconds that a user has to accept a group
        /// invite after receiving it. After this amount of time, the invite will expire and they will have to be
        /// invited again.</param>
        /// <param name="canJoinGroupHandler">a <see cref="Func{T1,T2,TResult}"/> used to determine if an <see cref="IGroupable"/>
        /// can join the specified <see cref="IGroup"/>. This does not need to take into consideration the group's factors,
        /// such as if it is full or closed.</param>
        public GroupSettings(int maxMembersPerGroup, int inviteResponseTime, Func<IGroupable, IGroup, bool> canJoinGroupHandler)
        {
            _canJoinGroupHandler = canJoinGroupHandler;
            _inviteResponseTime = inviteResponseTime;
            _maxMembersPerGroup = maxMembersPerGroup;
        }

        /// <summary>
        /// Gets a <see cref="Func{T,U,V}"/> used to determine if an <see cref="IGroupable"/> can join the
        /// specified <see cref="IGroup"/>. This does not need to take into consideration the group's factors, such
        /// as if it is full or closed.
        /// </summary>
        public Func<IGroupable, IGroup, bool> CanJoinGroupHandler
        {
            get { return _canJoinGroupHandler; }
        }

        /// <summary>
        /// Gets the <see cref="GroupSettings"/> instance.
        /// </summary>
        public static GroupSettings Instance
        {
            get
            {
                Debug.Assert(_instance != null, "The settings instance should not be null!");
                return _instance;
            }
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that a user has to accept a group invite after receiving it. After this
        /// amount of time, the invite will expire and they will have to be invited again.
        /// </summary>
        public int InviteResponseTime
        {
            get { return _inviteResponseTime; }
        }

        /// <summary>
        /// Gets the maximum number of members allowed in a single group.
        /// </summary>
        public int MaxMembersPerGroup
        {
            get { return _maxMembersPerGroup; }
        }

        /// <summary>
        /// Initializes the <see cref="GroupSettings"/>. This must only be called once and called as early as possible.
        /// </summary>
        /// <param name="settings">The settings instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings" /> is <c>null</c>.</exception>
        /// <exception cref="MethodAccessException">This method must be called once and only once.</exception>
        public static void Initialize(GroupSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (_instance != null)
                throw new MethodAccessException("This method must be called once and only once.");

            _instance = settings;
        }
    }
}