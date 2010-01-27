using System;
using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// A container that assists in managing the guild state for guild members.
    /// </summary>
    public class GuildMemberInfo<T> where T : class, IGuildMember
    {
        readonly T _owner;

        IGuild _guild;
        GuildRank _guildRank;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberInfo{T}"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="IGuildMember"/> that this <see cref="GuildMemberInfo{T}"/>
        /// will be handling the state values for.</param>
        protected GuildMemberInfo(T owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            _owner = owner;
        }

        /// <summary>
        /// Gets or sets the guild. The <see cref="GuildMemberInfo{T}.Owner"/> should implement their
        /// <see cref="IGuildMember.Guild"/> properly by using this property only.
        /// </summary>
        public IGuild Guild
        {
            get { return _guild; }
            set
            {
                if (_guild == value)
                    return;

                if (_guild != null)
                {
                    _guild.RemoveOnlineMember(Owner);
                    HandleLeaveGuild(_guild);
                }

                _guild = value;

                if (_guild != null)
                {
                    _guild.AddOnlineMember(Owner);
                    HandleJoinGuild(_guild);
                }

                Owner.SaveGuildInformation();
            }
        }

        /// <summary>
        /// Gets or sets the guild rank. The <see cref="GuildMemberInfo{T}.Owner"/> should implement their
        /// <see cref="IGuildMember.GuildRank"/> properly by using this property only.
        /// </summary>
        public GuildRank GuildRank
        {
            get { return _guildRank; }
            set
            {
                if (_guildRank == value)
                    return;

                if (value < _guildRank)
                    HandleDemotion(value);
                else
                    HandlePromotion(value);

                _guildRank = value;

                Owner.SaveGuildInformation();
            }
        }

        /// <summary>
        /// Gets the <see cref="IGuildMember"/> that this <see cref="GuildMemberInfo{T}"/> is handling the state values for.
        /// </summary>
        public T Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the owner is demoted.
        /// </summary>
        /// <param name="rank">The new rank.</param>
        protected virtual void HandleDemotion(GuildRank rank)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the owner joins a guild.
        /// </summary>
        /// <param name="guild">The guild that was joined.</param>
        protected virtual void HandleJoinGuild(IGuild guild)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the owner leaves a guild.
        /// </summary>
        /// <param name="guild">The guild that was left.</param>
        protected virtual void HandleLeaveGuild(IGuild guild)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the owner is promoted.
        /// </summary>
        /// <param name="rank">The new rank.</param>
        protected virtual void HandlePromotion(GuildRank rank)
        {
        }
    }
}