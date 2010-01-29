using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// A struct containing a guild member's name and rank.
    /// </summary>
    public struct GuildMemberNameRank
    {
        readonly string _name;
        readonly GuildRank _rank;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberNameRank"/> struct.
        /// </summary>
        /// <param name="name">The guild member's name.</param>
        /// <param name="rank">The guild member's rank.</param>
        public GuildMemberNameRank(string name, GuildRank rank)
        {
            _name = name;
            _rank = rank;
        }

        /// <summary>
        /// Gets the guild member's name.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the guild member's rank.
        /// </summary>
        public GuildRank Rank
        {
            get { return _rank; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="GuildMemberNameRank"/> to <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator KeyValuePair<string, GuildRank>(GuildMemberNameRank v)
        {
            return new KeyValuePair<string, GuildRank>(v.Name, v.Rank);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="KeyValuePair{K,V}"/> to <see cref="GuildMemberNameRank"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator GuildMemberNameRank(KeyValuePair<string, GuildRank> v)
        {
            return new GuildMemberNameRank(v.Key, v.Value);
        }
    }
}