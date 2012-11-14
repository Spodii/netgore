using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// A container that assists in managing the guild state for guild members.
    /// </summary>
    /// <typeparam name="T">The type of guild member.</typeparam>
    public abstract class GuildMemberInfo<T> where T : class, IGuildMember
    {
        static readonly IObjectPool<GuildInviteStatus> _guildInvitePool =
            new ObjectPool<GuildInviteStatus>(x => new GuildInviteStatus(), null, x => x.Reset(), true);

        static readonly int _inviteResponseTime = GuildSettings.Instance.InviteResponseTime;

        readonly List<GuildInviteStatus> _invites = new List<GuildInviteStatus>();
        readonly T _owner;

        IGuild _guild;
        GuildRank _guildRank;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberInfo{T}"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="IGuildMember"/> that this <see cref="GuildMemberInfo{T}"/>
        /// will be handling the state values for.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner" /> is <c>null</c>.</exception>
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
                    HandleJoinGuild(_guild);

                    if (IsLoading())
                        _guild.AddOnlineMember(Owner);
                    else
                        _guild.AddNewOnlineMember(Owner);
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
        /// Accepts the invite to a <see cref="IGuild"/>. This method will also make sure that this member
        /// has an outstanding invite to the guild.
        /// </summary>
        /// <param name="guild">The guild to join.</param>
        /// <param name="currentTime">The current time.</param>
        /// <returns>
        /// True if this member successfully joined the <paramref name="guild"/>; otherwise false.
        /// </returns>
        public bool AcceptInvite(IGuild guild, TickCount currentTime)
        {
            if (Owner.Guild != null)
                return false;

            if (guild == null)
                return false;

            // Update the invites
            UpdateInvites(currentTime);

            // Make sure there is an invite to this guild
            if (_invites.All(x => x.Guild != guild))
                return false;

            // Join the guild
            Owner.Guild = guild;

            // Remove all outstanding invites
            foreach (var current in _invites)
            {
                _guildInvitePool.Free(current);
            }

            _invites.Clear();

            return true;
        }

        /// <summary>
        /// Gets all of the current live invites the <see cref="GuildMemberInfo{T}.Owner"/> has. Make sure to call
        /// <see cref="GuildInviteStatus.DeepCopy"/> if you want to hold onto a reference.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>All of the current live invites the <see cref="GuildMemberInfo{T}.Owner"/> has</returns>
        public IEnumerable<GuildInviteStatus> GetInvites(TickCount currentTime)
        {
            UpdateInvites(currentTime);
            return _invites;
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="GuildMemberInfo{T}.Owner"/> is demoted.
        /// </summary>
        /// <param name="rank">The new rank.</param>
        protected virtual void HandleDemotion(GuildRank rank)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="GuildMemberInfo{T}.Owner"/> joins a guild.
        /// </summary>
        /// <param name="guild">The guild that was joined.</param>
        protected virtual void HandleJoinGuild(IGuild guild)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="GuildMemberInfo{T}.Owner"/> leaves a guild.
        /// </summary>
        /// <param name="guild">The guild that was left.</param>
        protected virtual void HandleLeaveGuild(IGuild guild)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="GuildMemberInfo{T}.Owner"/> is promoted.
        /// </summary>
        /// <param name="rank">The new rank.</param>
        protected virtual void HandlePromotion(GuildRank rank)
        {
        }

        /// <summary>
        /// Gets if the <see cref="GuildMemberInfo{T}.Owner"/> is only having their guild values set because they are
        /// loading, not because they are joining/leaving a guild.
        /// </summary>
        /// <returns>True if they <see cref="GuildMemberInfo{T}.Owner"/> is loading; otherwise false.</returns>
        protected abstract bool IsLoading();

        /// <summary>
        /// Handles when the guild member receives an invite to a guild.
        /// </summary>
        /// <param name="guild">The guild they were invited to.</param>
        /// <param name="currentTime">The current time.</param>
        public void ReceiveInvite(IGuild guild, TickCount currentTime)
        {
            UpdateInvites(currentTime);

            // If an invite for this guild already exists, update the invite time on that invite instead
            for (var i = 0; i < _invites.Count; i++)
            {
                if (_invites[i].Guild == guild)
                {
                    _invites[i].InviteTime = currentTime;
                    return;
                }
            }

            // Add the invite
            var item = _guildInvitePool.Acquire();
            item.Initialize(guild, currentTime);
            _invites.Add(item);
        }

        /// <summary>
        /// Updates the invites, removing expired ones.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void UpdateInvites(TickCount currentTime)
        {
            for (var i = 0; i < _invites.Count; i++)
            {
                var current = _invites[i];
                if (current.InviteTime + _inviteResponseTime < currentTime)
                {
                    _guildInvitePool.Free(current);
                    _invites.RemoveAt(i);
                    --i;
                }
            }
        }
    }
}