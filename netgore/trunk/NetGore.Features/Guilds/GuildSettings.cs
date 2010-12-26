using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Features.Guilds
{
    public class GuildSettings
    {
        /// <summary>
        /// The settings instance.
        /// </summary>
        static GuildSettings _instance;

        readonly byte _highestRank;
        readonly int _inviteResponseTime;
        readonly GuildRank _minRankDemote;
        readonly GuildRank _minRankInvite;
        readonly GuildRank _minRankKick;
        readonly GuildRank _minRankPromote;
        readonly GuildRank _minRankRename;
        readonly GuildRank _minRankViewLog;
        readonly StringRules _nameRules;
        readonly string[] _rankNames;
        readonly StringRules _tagRules;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildSettings"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="rankNames" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="nameRules" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="tagRules" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">There must be exactly one rank name for each rank.</exception>
        /// <exception cref="ArgumentException">Guild rank names may not be empty or null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>inviteResponseTime</c> is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>minRankRename</c> is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>minRankViewLog</c> is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>minRankKick</c> is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>minRankInvite</c> is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>minRankPromote</c> is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>minRankDemote</c> is out of range.</exception>
        public GuildSettings(int inviteResponseTime, GuildRank highestRank, string[] rankNames, StringRules nameRules,
                             StringRules tagRules, GuildRank minRankRename, GuildRank minRankViewLog, GuildRank minRankKick,
                             GuildRank minRankInvite, GuildRank minRankPromote, GuildRank minRankDemote)
        {
            if (rankNames == null)
                throw new ArgumentNullException("rankNames");
            if (rankNames.Length != highestRank + 1)
                throw new ArgumentException("There must be exactly one rank name for each rank.", "rankNames");
            if (rankNames.Any(x => string.IsNullOrEmpty(x)))
                throw new ArgumentException("Guild rank names may not be empty or null.", "rankNames");
            if (nameRules == null)
                throw new ArgumentNullException("nameRules");
            if (tagRules == null)
                throw new ArgumentNullException("tagRules");
            if (inviteResponseTime <= 0)
                throw new ArgumentOutOfRangeException("inviteResponseTime");

            if (minRankRename > highestRank)
                throw new ArgumentOutOfRangeException("minRankRename");
            if (minRankViewLog > highestRank)
                throw new ArgumentOutOfRangeException("minRankViewLog");
            if (minRankKick > highestRank)
                throw new ArgumentOutOfRangeException("minRankKick");
            if (minRankInvite > highestRank)
                throw new ArgumentOutOfRangeException("minRankInvite");
            if (minRankPromote > highestRank)
                throw new ArgumentOutOfRangeException("minRankPromote");
            if (minRankDemote > highestRank)
                throw new ArgumentOutOfRangeException("minRankDemote");

            _inviteResponseTime = inviteResponseTime;

            _highestRank = highestRank;
            _rankNames = rankNames;
            _nameRules = nameRules;
            _tagRules = tagRules;

            _minRankRename = minRankRename;
            _minRankViewLog = minRankViewLog;
            _minRankKick = minRankKick;
            _minRankInvite = minRankInvite;
            _minRankPromote = minRankPromote;
            _minRankDemote = minRankDemote;
        }

        /// <summary>
        /// Gets the highest guild ranking.
        /// </summary>
        public GuildRank HighestRank
        {
            get { return _highestRank; }
        }

        /// <summary>
        /// Gets the <see cref="GuildSettings"/> instance.
        /// </summary>
        public static GuildSettings Instance
        {
            get
            {
                Debug.Assert(_instance != null, "The settings instance should not be null!");
                return _instance;
            }
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that a user has to accept a guild invite after receiving it. After this
        /// amount of time, the invite will expire and they will have to be invited again.
        /// </summary>
        public int InviteResponseTime
        {
            get { return _inviteResponseTime; }
        }

        /// <summary>
        /// Gets the minimum rank required for a guild member to demote another guild member.
        /// </summary>
        public GuildRank MinRankDemote
        {
            get { return _minRankDemote; }
        }

        /// <summary>
        /// Gets the minimum rank required for a guild member to invite a member to the guild.
        /// </summary>
        public GuildRank MinRankInvite
        {
            get { return _minRankInvite; }
        }

        /// <summary>
        /// Gets the minimum rank required for a guild member to kick another member.
        /// </summary>
        public GuildRank MinRankKick
        {
            get { return _minRankKick; }
        }

        /// <summary>
        /// Gets the minimum rank required for a guild member to promote another guild member.
        /// </summary>
        public GuildRank MinRankPromote
        {
            get { return _minRankPromote; }
        }

        /// <summary>
        /// Gets the minimum rank required for a guild member to rename the guild.
        /// </summary>
        public GuildRank MinRankRename
        {
            get { return _minRankRename; }
        }

        /// <summary>
        /// Gets the minimum rank required for a guild member to view the guild's event log.
        /// </summary>
        public GuildRank MinRankViewLog
        {
            get { return _minRankViewLog; }
        }

        /// <summary>
        /// Gets the rules for the guild's name.
        /// </summary>
        public StringRules NameRules
        {
            get { return _nameRules; }
        }

        /// <summary>
        /// Gets the array of names for the guild ranks.
        /// </summary>
        public ICollection<string> RankNames
        {
            get { return _rankNames; }
        }

        /// <summary>
        /// Gets the rules for the guild's tag.
        /// </summary>
        public StringRules TagRules
        {
            get { return _tagRules; }
        }

        /// <summary>
        /// Gets the name of the given <paramref name="rank"/>.
        /// </summary>
        /// <param name="rank">The guild rank.</param>
        /// <returns>The name of the given <paramref name="rank"/>.</returns>
        public string GetRankName(GuildRank rank)
        {
            return _rankNames[rank];
        }

        /// <summary>
        /// Initializes the <see cref="GuildSettings"/>. This must only be called once and called as early as possible.
        /// </summary>
        /// <param name="settings">The settings instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings" /> is <c>null</c>.</exception>
        /// <exception cref="MethodAccessException">This method must be called once and only once.</exception>
        public static void Initialize(GuildSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (_instance != null)
                throw new MethodAccessException("This method must be called once and only once.");

            _instance = settings;
        }

        /// <summary>
        /// Gets if the <paramref name="name"/> is a valid guild name.
        /// </summary>
        /// <param name="name">The guild name.</param>
        /// <returns>True if the <paramref name="name"/> is a valid guild name; otherwise false.</returns>
        public bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            return _nameRules.IsValid(name);
        }

        /// <summary>
        /// Gets if the <paramref name="tag"/> is a valid guild tag.
        /// </summary>
        /// <param name="tag">The guild tag.</param>
        /// <returns>True if the <paramref name="tag"/> is a valid guild tag; otherwise false.</returns>
        public bool IsValidTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return false;

            return _tagRules.IsValid(tag);
        }
    }
}