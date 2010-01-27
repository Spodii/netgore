using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// A container that assists in managing the guild state for guild members.
    /// </summary>
    public class GuildMemberInfo
    {
        readonly IGuildMember _owner;

        IGuild _guild;
        GuildRank _guildRank;

        /// <summary>
        /// Gets the <see cref="IGuildMember"/> that this <see cref="GuildMemberInfo"/> is handling the state values for.
        /// </summary>
        public IGuildMember Owner { get { return _owner; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberInfo"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="IGuildMember"/> that this <see cref="GuildMemberInfo"/>
        /// will be handling the state values for.</param>
        public GuildMemberInfo(IGuildMember owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            _owner = owner;
        }

        /// <summary>
        /// Gets or sets the guild. The <see cref="GuildMemberInfo.Owner"/> should implement their
        /// <see cref="IGuildMember.Guild"/> properly by using this property only.
        /// </summary>
        public IGuild Guild { get { return _guild; } 
            set {
                if (_guild == value)
                    return;

                if (_guild != null)
                    _guild.RemoveOnlineMember(Owner);

                _guild = value;

                if (_guild != null)
                    _guild.AddOnlineMember(Owner);
            } }

        /// <summary>
        /// Gets or sets the guild rank. The <see cref="GuildMemberInfo.Owner"/> should implement their
        /// <see cref="IGuildMember.GuildRank"/> properly by using this property only.
        /// </summary>
        public GuildRank GuildRank
        {
            get { return _guildRank; }
            set
            {
                _guildRank = value;
            }
        }
    }
}
