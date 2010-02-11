using System;
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

        readonly int _maxMembersPerGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupSettings"/> class.
        /// </summary>
        /// <param name="maxMembersPerGroup">The maximum number of members allowed in a single group.</param>
        public GroupSettings(int maxMembersPerGroup)
        {
            _maxMembersPerGroup = maxMembersPerGroup;
        }

        /// <summary>
        /// Gets the <see cref="GroupSettings"/> instance.
        /// </summary>
        public static GroupSettings Instance
        {
            get { return _instance; }
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