using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Base class for a <see cref="IGuild"/>.
    /// </summary>
    public abstract class GuildBase : IGuild
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly GuildSettings _guildSettings = GuildSettings.Instance;

        readonly IGuildManager _guildManager;
        readonly GuildID _id;
        readonly List<IGuildMember> _onlineMembers = new List<IGuildMember>();

        bool _isDestroyed = false;
        string _name;
        string _tag;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildBase"/> class.
        /// </summary>
        /// <param name="guildManager">The <see cref="IGuildManager"/> managing this guild.</param>
        /// <param name="id">The unique ID of the guild.</param>
        /// <param name="name">The unique name of the guild.</param>
        /// <param name="tag">The guild's unique tag.</param>
        protected GuildBase(IGuildManager guildManager, GuildID id, string name, string tag)
        {
            _guildManager = guildManager;
            _id = id;
            _name = name;
            _tag = tag;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Save();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Makes sure that the guild event invoker and target are valid.
        /// </summary>
        /// <param name="invoker">The guild member invoking the event.</param>
        /// <param name="target">The guild member the event is being invoked on.</param>
        /// <returns>True if the parameters are valid; otherwise false.</returns>
        bool EnsureValidEventSource(IGuildMember invoker, IGuildMember target)
        {
            if (!EnsureValidEventSource(invoker))
                return false;

            if (target == null)
            {
                const string errmsg = "Guild event target is null. Invoker: `{0}`";
                Debug.Fail(errmsg);
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, invoker);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Makes sure that the guild event invoker is valid and part of a guild.
        /// </summary>
        /// <param name="invoker">The guild member invoking the event.</param>
        /// <returns>True if the invoker is valid; otherwise false.</returns>
        bool EnsureValidEventSource(IGuildMember invoker)
        {
            if (IsDestroyed)
            {
                const string errmsg = "Tried to invoke an event on guild `{0}`, but it has been destroyed already!";
                Debug.Fail(string.Format(errmsg, this));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return false;
            }

            if (invoker == null)
            {
                const string errmsg = "Guild event invoker is null.";
                Debug.Fail(errmsg);
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                return false;
            }

            if (invoker.Guild != this)
            {
                const string errmsg = "Guild event invoker `{0}` is not part of this guild instance (`{1}`).";
                Debug.Fail(string.Format(errmsg, invoker, this));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, invoker, this);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Makes sure that the guild event invoker and target are valid, and part of the same guild.
        /// </summary>
        /// <param name="invoker">The guild member invoking the event.</param>
        /// <param name="target">The guild member the event is being invoked on.</param>
        /// <returns>True if the parameters are valid; otherwise false.</returns>
        bool EnsureValidEventSourceSameGuild(IGuildMember invoker, IGuildMember target)
        {
            if (!EnsureValidEventSource(invoker, target))
                return false;

            if (invoker.Guild != target.Guild)
            {
                const string errmsg =
                    "Guild event invoker `{0}` is part of guild `{1}`, while their target `{2}` is part" +
                    " of a different guild, `{3}`.";
                Debug.Fail(errmsg);
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, invoker, invoker.Guild, target, target.Guild);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensures a guild member invoking an event meets the rank requirements to invoke the event.
        /// </summary>
        /// <param name="invoker">The guild member invoking the event.</param>
        /// <param name="minRequiredRank">The minimum rank required to invoke the event.</param>
        /// <returns>True if the <paramref name="invoker"/> is a high enough rank to invoke the event;
        /// otherwise false.</returns>
        static bool EnsureValidRank(IGuildMember invoker, GuildRank minRequiredRank)
        {
            if (invoker.GuildRank < minRequiredRank)
            {
                const string errmsg =
                    "Guild member `{0}` from guild `{1}` tried to invoke an event, but their rank was not" +
                    " high enough (rank: `{2}` req: `{3}`).";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, invoker, invoker.Guild, invoker.GuildRank, minRequiredRank);
                return false;
            }

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, gets the number of founders (highest rank) in the guild.
        /// </summary>
        /// <returns>The number of founders (highest rank) in the guild.</returns>
        protected abstract int GetNumberOfFounders();

        /// <summary>
        /// When overridden in the derived class, handles destroying the guild. This needs to remove all members
        /// in the guild from the guild, and remove the guild itself from the database.
        /// </summary>
        protected abstract void HandleDestroyed();

        /// <summary>
        /// When overridden in the derived class, attempts to set the name of this guild to the given
        /// <paramref name="newName"/> in the database. The <paramref name="newName"/> does not need to
        /// be checked if valid.
        /// </summary>
        /// <param name="newName">The new name for the guild.</param>
        /// <returns>True if the name was successfully changed; otherwise false.</returns>
        protected abstract bool InternalTryChangeName(string newName);

        /// <summary>
        /// When overridden in the derived class, attempts to set the tag of this guild to the given
        /// <paramref name="newTag"/> in the database. The <paramref name="newTag"/> does not need to
        /// be checked if valid.
        /// </summary>
        /// <param name="newTag">The new tag for the guild.</param>
        /// <returns>True if the tag was successfully changed; otherwise false.</returns>
        protected abstract bool InternalTryChangeTag(string newTag);

        /// <summary>
        /// Does the actual handling of demoting a guild member.
        /// </summary>
        /// <param name="invoker">The guild member is who demoting the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being demoted.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully demoted the <paramref name="target"/>;
        /// otherwise false.</returns>
        protected virtual bool InternalTryDemoteMember(IGuildMember invoker, IGuildMember target)
        {
            // Demote
            target.GuildRank = new GuildRank((byte)(target.GuildRank - 1));

            if (target.GuildRank < 0 || target.GuildRank > _guildSettings.HighestRank)
            {
                const string errmsg =
                    "Somehow, when `{0}` demoted `{1}`, their rank ended up at the invalid value of `{2}`." +
                    " Rank being reset to 0.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, invoker, target, target.GuildRank);
                Debug.Fail(string.Format(errmsg, invoker, target, target.GuildRank));
                target.GuildRank = 0;
            }

            // Log the event
            GuildManager.LogEvent(invoker, GuildEvents.Demote, target);

            OnMemberDemoted(invoker, target);

            if (MemberDemoted != null)
                MemberDemoted.Raise(invoker, EventArgsHelper.Create(target));

            return true;
        }

        /// <summary>
        /// Does the actual handling of inviting a member into the guild.
        /// </summary>
        /// <param name="invoker">The guild member is who inviting the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being invited to the guild.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully invited the <paramref name="target"/>
        /// to the guild; otherwise false.</returns>
        protected virtual bool InternalTryInviteMember(IGuildMember invoker, IGuildMember target)
        {
            // Send the invite
            target.SendGuildInvite(invoker, this);

            // Log the event
            GuildManager.LogEvent(invoker, GuildEvents.Invite, target);

            if (MemberInvited != null)
                MemberInvited.Raise(invoker, EventArgsHelper.Create(target));

            OnMemberInvited(invoker, target);

            return true;
        }

        /// <summary>
        /// Does the actual handling of kicking a member out of a guild.
        /// </summary>
        /// <param name="invoker">The guild member is who kicking out the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being kicked out of the guild.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully kicked the <paramref name="target"/>
        /// out of the guild; otherwise false.</returns>
        protected virtual bool InternalTryKickMember(IGuildMember invoker, IGuildMember target)
        {
            // Kick the target out of the guild
            target.Guild = null;

            // Log the event
            GuildManager.LogEvent(invoker, GuildEvents.Kick, target);

            if (MemberKicked != null)
                MemberKicked.Raise(invoker, EventArgsHelper.Create(target));

            OnMemberKicked(invoker, target);

            return true;
        }

        /// <summary>
        /// Does the actual handling of making a member leave the guild.
        /// </summary>
        /// <param name="invoker">The guild member who is leaving.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully left the guild; otherwise false.</returns>
        protected virtual bool InternalTryLeaveGuild(IGuildMember invoker)
        {
            // If they were the only founder remaining, destroy the guild completely instead
            if (invoker.GuildRank == _guildSettings.HighestRank)
            {
                var founders = GetNumberOfFounders();
                if (founders == 1)
                {
                    DestroyGuild();
                    return true;
                }
            }

            // Leave the guild
            invoker.Guild = null;

            return true;
        }

        /// <summary>
        /// Does the actual handling of promoting a guild member.
        /// </summary>
        /// <param name="invoker">The guild member is who promoting the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being promoted.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully promoted the <paramref name="target"/>;
        /// otherwise false.</returns>
        protected virtual bool InternalTryPromoteMember(IGuildMember invoker, IGuildMember target)
        {
            // Promote
            target.GuildRank = new GuildRank((byte)(target.GuildRank + 1));

            if (target.GuildRank > invoker.GuildRank)
            {
                const string errmsg =
                    "Somehow, when `{0}` promoted `{1}`, their rank [{2}] ended up greater than that of" +
                    " the member who promoted them [{3}].";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, invoker, target, target.GuildRank, invoker.GuildRank);
                Debug.Fail(string.Format(errmsg, invoker, target, target.GuildRank, invoker.GuildRank));
                target.GuildRank = invoker.GuildRank;
            }

            // Log the event
            GuildManager.LogEvent(invoker, GuildEvents.Promote, target);

            if (MemberPromoted != null)
                MemberPromoted.Raise(invoker, EventArgsHelper.Create(target));

            OnMemberPromoted(invoker, target);

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, does the actually handling of viewing the event log for the guild.
        /// This should send the latest entries of the guild event log to the <paramref name="invoker"/>.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the log; otherwise false.</returns>
        protected abstract bool InternalTryViewEventLog(IGuildMember invoker);

        /// <summary>
        /// When overridden in the derived class, displays the members of the guild to the <paramref name="invoker"/>.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the member list; otherwise false.</returns>
        protected abstract bool InternalTryViewMembers(IGuildMember invoker);

        /// <summary>
        /// When overridden in the derived class, displays the online members of the guild to the <paramref name="invoker"/>.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the online member list; otherwise false.</returns>
        protected abstract bool InternalTryViewOnlineMembers(IGuildMember invoker);

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after a new guild member is added.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        protected virtual void OnMemberAdded(IGuildMember newMember)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after a guild member is demoted.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <param name="target">The optional guild member the event involves.</param>
        protected virtual void OnMemberDemoted(IGuildMember invoker, IGuildMember target)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after a guild member is invited.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <param name="target">The optional guild member the event involves.</param>
        protected virtual void OnMemberInvited(IGuildMember invoker, IGuildMember target)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after a guild member is kicked.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <param name="target">The optional guild member the event involves.</param>
        protected virtual void OnMemberKicked(IGuildMember invoker, IGuildMember target)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after a guild member is promoted.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <param name="target">The optional guild member the event involves.</param>
        protected virtual void OnMemberPromoted(IGuildMember invoker, IGuildMember target)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after the guild's name has changed.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        protected virtual void OnNameChanged(IGuildMember invoker, string oldName, string newName)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when a member of this
        /// guild has come online.
        /// </summary>
        /// <param name="guildMember">The guild member that came online.</param>
        protected virtual void OnOnlineUserAdded(IGuildMember guildMember)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when a member of this
        /// guild has gone offline.
        /// </summary>
        /// <param name="guildMember">The guild member that went offline.</param>
        protected virtual void OnOnlineUserRemoved(IGuildMember guildMember)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling after the guild's tag has changed.
        /// Use this instead of the corresponding event when possible.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <param name="oldTag">The old tag.</param>
        /// <param name="newTag">The new tag.</param>
        protected virtual void OnTagChanged(IGuildMember invoker, string oldTag, string newTag)
        {
        }

        #region IGuild Members

        /// <summary>
        /// Notifies listeners when the guild has been destroyed.
        /// </summary>
        public event TypedEventHandler<IGuild> Destroyed;

        /// <summary>
        /// Notifies listeners when a new member has joined the guild.
        /// </summary>
        public event TypedEventHandler<IGuild, EventArgs<IGuildMember>> MemberAdded;

        /// <summary>
        /// Notifies listeners when a member has been demoted.
        /// </summary>
        public event TypedEventHandler<IGuildMember, EventArgs<IGuildMember>> MemberDemoted;

        /// <summary>
        /// Notifies listeners when a member has been invited into the guild.
        /// </summary>
        public event TypedEventHandler<IGuildMember, EventArgs<IGuildMember>> MemberInvited;

        /// <summary>
        /// Notifies listeners when a member has been kicked from the guild.
        /// </summary>
        public event TypedEventHandler<IGuildMember, EventArgs<IGuildMember>> MemberKicked;

        /// <summary>
        /// Notifies listeners when a member has been promoted.
        /// </summary>
        public event TypedEventHandler<IGuildMember, EventArgs<IGuildMember>> MemberPromoted;

        /// <summary>
        /// Notifies listeners when the guild's name has been changed.
        /// </summary>
        public event TypedEventHandler<IGuild, GuildRenameEventArgs> NameChanged;

        /// <summary>
        /// Notifies listeners when a member of this guild has come online.
        /// </summary>
        public event TypedEventHandler<IGuild, EventArgs<IGuildMember>> OnlineUserAdded;

        /// <summary>
        /// Notifies listeners when a member of this guild has gone offline.
        /// </summary>
        public event TypedEventHandler<IGuild, EventArgs<IGuildMember>> OnlineUserRemoved;

        /// <summary>
        /// Notifies listeners when the guild's tag has been changed.
        /// </summary>
        public event TypedEventHandler<IGuild, GuildRenameEventArgs> TagChanged;

        /// <summary>
        /// Gets the <see cref="IGuildManager"/> managing this guild.
        /// </summary>
        public IGuildManager GuildManager
        {
            get { return _guildManager; }
        }

        /// <summary>
        /// Gets the unique ID of the guild.
        /// </summary>
        public GuildID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets if this guild has been destroyed. If this is true, nobody should be in this guild.
        /// </summary>
        public bool IsDestroyed
        {
            get { return _isDestroyed; }
        }

        /// <summary>
        /// Gets the unique name of the guild.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the online guild members in this guild.
        /// </summary>
        public IEnumerable<IGuildMember> OnlineMembers
        {
            get { return _onlineMembers.ToImmutable(); }
        }

        /// <summary>
        /// Gets the guild's unique tag.
        /// </summary>
        public string Tag
        {
            get { return _tag; }
        }

        /// <summary>
        /// Adds the reference of an online guild member to this guild that is new to the guild.
        /// This does not make the user join or leave the guild in any way, just allows the guild to keep track of the
        /// members that are online.
        /// </summary>
        /// <param name="newMember">The online guild member to add.</param>
        public void AddNewOnlineMember(IGuildMember newMember)
        {
            if (MemberAdded != null)
                MemberAdded.Raise(this, EventArgsHelper.Create(newMember));

            OnMemberAdded(newMember);

            AddOnlineMember(newMember);
        }

        /// <summary>
        /// Adds the reference of an online guild member to this guild. This does not make the user join or leave the
        /// guild in any way, just allows the guild to keep track of the members that are online.
        /// </summary>
        /// <param name="member">The online guild member to add.</param>
        public void AddOnlineMember(IGuildMember member)
        {
            if (member.Guild != this)
            {
                const string errmsg = "The guild member `{0}` does not belong to this guild [{1}]!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, member, this);
                Debug.Fail(string.Format(errmsg, member, this));
                return;
            }

            if (_onlineMembers.Contains(member))
            {
                const string errmsg =
                    "Member `{0}` is already in the online list for guild `{1}`." +
                    " Not really a problem that can't be easily fixed, but should be avoided since it is needless overhead.";
                Debug.Fail(string.Format(errmsg, member, this));
                return;
            }

            _onlineMembers.Add(member);

            if (OnlineUserAdded != null)
                OnlineUserAdded.Raise(this, EventArgsHelper.Create(member));

            OnOnlineUserAdded(member);
        }

        /// <summary>
        /// Destroys the guild completely and removes all members from it.
        /// </summary>
        public virtual void DestroyGuild()
        {
            HandleDestroyed();

            _isDestroyed = true;

            if (Destroyed != null)
                Destroyed.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Save();
        }

        /// <summary>
        /// Gets the name and rank for all the members in the guild.
        /// </summary>
        /// <returns>The name and rank for all the members in the guild.</returns>
        public abstract IEnumerable<GuildMemberNameRank> GetMembers();

        /// <summary>
        /// Removes the reference of an online guild member from this guild. This does not make the user join or leave the
        /// guild in any way, just allows the guild to keep track of the members that are online.
        /// </summary>
        /// <param name="member">The online guild member to remove.</param>
        public void RemoveOnlineMember(IGuildMember member)
        {
            if (member.Guild != this)
            {
                const string errmsg = "The guild member `{0}` does not belong to this guild [{1}]!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, member, this);
                Debug.Fail(string.Format(errmsg, member, this));
                return;
            }

            if (!_onlineMembers.Remove(member))
            {
                const string errmsg =
                    "Member `{0}` was not in the online list for guild `{1}`." +
                    " Not really a problem that can't be easily fixed, but should be avoided since it is needless overhead.";
                Debug.Fail(string.Format(errmsg, member, this));
                return;
            }

            if (OnlineUserRemoved != null)
                OnlineUserRemoved.Raise(this, EventArgsHelper.Create(member));

            OnOnlineUserRemoved(member);
        }

        /// <summary>
        /// When overridden in the derived class, saves all of the guild's information to the database.
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Tries to change the name of the guild.
        /// </summary>
        /// <param name="invoker">The guild member trying to change the guild's name.</param>
        /// <param name="newName">The new name of the guild.</param>
        /// <returns>True if the name was successfully changed; otherwise false.</returns>
        public bool TryChangeName(IGuildMember invoker, string newName)
        {
            if (!EnsureValidEventSource(invoker))
                return false;

            if (!EnsureValidRank(invoker, _guildSettings.MinRankRename))
                return false;

            if (_name == newName || !GuildManager.IsNameAvailable(newName))
                return false;

            var oldValue = Name;

            var success = InternalTryChangeName(newName);

            if (success)
            {
                _name = newName;
                Save();

                OnNameChanged(invoker, oldValue, Name);

                if (NameChanged != null)
                    NameChanged.Raise(this, new GuildRenameEventArgs(invoker, oldValue, Name));
            }

            return success;
        }

        /// <summary>
        /// Tries to change the tag of the guild.
        /// </summary>
        /// <param name="invoker">The guild member trying to change the guild's tag.</param>
        /// <param name="newTag">The new tag of the guild.</param>
        /// <returns>True if the tag was successfully changed; otherwise false.</returns>
        public bool TryChangeTag(IGuildMember invoker, string newTag)
        {
            if (!EnsureValidEventSource(invoker))
                return false;

            if (!EnsureValidRank(invoker, _guildSettings.MinRankRename))
                return false;

            if (_tag == newTag || !GuildManager.IsTagAvailable(newTag))
                return false;

            var oldValue = Tag;

            var success = InternalTryChangeTag(newTag);

            if (success)
            {
                _tag = newTag;
                Save();

                OnTagChanged(invoker, oldValue, Tag);

                if (TagChanged != null)
                    TagChanged.Raise(this, new GuildRenameEventArgs(invoker, oldValue, Tag));
            }

            return success;
        }

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to demote the <paramref name="target"/>.
        /// </summary>
        /// <param name="invoker">The guild member is who demoting the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being demoted.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully demoted the <paramref name="target"/>;
        /// otherwise false.</returns>
        public bool TryDemoteMember(IGuildMember invoker, IGuildMember target)
        {
            // Ensure the parameters are valid
            if (!EnsureValidEventSourceSameGuild(invoker, target))
                return false;

            // Ensure the invoker has the needed permissions
            if (!EnsureValidRank(invoker, _guildSettings.MinRankPromote))
                return false;

            // Only allow promotions to be done on a member that is a lower or equal rank than the one demoting
            if (invoker.GuildRank < target.GuildRank)
            {
                const string errmsg =
                    "Guild member `{0}` tried to demote member `{1}`, but their rank [{2}] is" +
                    " is not greater than or equal to the rank of the one they are trying to demote [{3}].";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, invoker, target, invoker.GuildRank, target.GuildRank);
                return false;
            }

            return InternalTryDemoteMember(invoker, target);
        }

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to invite the <paramref name="target"/> to the guild.
        /// </summary>
        /// <param name="invoker">The guild member is who inviting the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being invited to the guild.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully invited the <paramref name="target"/>
        /// to the guild; otherwise false.</returns>
        public bool TryInviteMember(IGuildMember invoker, IGuildMember target)
        {
            // Ensure the parameters are valid, and the target is not in a guild yet
            if (!EnsureValidEventSource(invoker, target))
                return false;

            if (target.Guild != null)
            {
                const string errmsg =
                    "Guild member `{0}` tried to invite `{1}` into guild `{2}`, but the target" +
                    " is already part of a guild, `{3}`.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, invoker, target, invoker.Guild, target.Guild);
                return false;
            }

            // Ensure the user has the needed permission level for the action
            if (!EnsureValidRank(invoker, _guildSettings.MinRankInvite))
                return false;

            return InternalTryInviteMember(invoker, target);
        }

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to kick the <paramref name="target"/> out of the guild.
        /// </summary>
        /// <param name="invoker">The guild member is who kicking out the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being kicked out of the guild.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully kicked the <paramref name="target"/>
        /// out of the guild; otherwise false.</returns>
        public bool TryKickMember(IGuildMember invoker, IGuildMember target)
        {
            // Ensure the user and invoker are in the same guild
            if (!EnsureValidEventSourceSameGuild(invoker, target))
                return false;

            // Ensure the user has the needed permission level for the action
            if (!EnsureValidRank(invoker, _guildSettings.MinRankKick))
                return false;

            // The invoker's rank must be greater than or equal to the target's rank
            if (invoker.GuildRank < target.GuildRank)
            {
                const string errmsg =
                    "Guild member `{0}` from guild `{1}` tried to kick out `{2}`, but their rank [{3}] is" +
                    " is lower than the rank of the one they are trying to kick [{4}].";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, invoker, invoker.Guild, target, invoker.GuildRank, target.GuildRank);
                return false;
            }

            return InternalTryKickMember(invoker, target);
        }

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to leave the guild.
        /// </summary>
        /// <param name="invoker">The guild member who is leaving.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully left the guild; otherwise false.</returns>
        public bool TryLeaveGuild(IGuildMember invoker)
        {
            // Ensure a valid invoker
            if (!EnsureValidEventSource(invoker))
                return false;

            return InternalTryLeaveGuild(invoker);
        }

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to promote the <paramref name="target"/>.
        /// </summary>
        /// <param name="invoker">The guild member is who promoting the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being promoted.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully promoted the <paramref name="target"/>;
        /// otherwise false.</returns>
        public bool TryPromoteMember(IGuildMember invoker, IGuildMember target)
        {
            // Ensure the parameters are valid
            if (!EnsureValidEventSourceSameGuild(invoker, target))
                return false;

            // Ensure the invoker has the needed permissions
            if (!EnsureValidRank(invoker, _guildSettings.MinRankPromote))
                return false;

            // Only allow promotions to be done on a member that is a lower rank than the one promoting
            if (invoker.GuildRank <= target.GuildRank)
            {
                const string errmsg =
                    "Guild member `{0}` tried to promote member `{1}`, but their rank [{2}] is" +
                    " is not greater than the rank of the one they are trying to promote [{3}].";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, invoker, target, invoker.GuildRank, target.GuildRank);
                return false;
            }

            if (target.GuildRank == 0)
                return false;

            return InternalTryPromoteMember(invoker, target);
        }

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to view the event log for the guild.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the log; otherwise false.</returns>
        public bool TryViewEventLog(IGuildMember invoker)
        {
            // Ensure the parameters are valid
            if (!EnsureValidEventSource(invoker))
                return false;

            // Ensure the user has the needed permission level for the action
            if (!EnsureValidRank(invoker, _guildSettings.MinRankViewLog))
                return false;

            return InternalTryViewEventLog(invoker);
        }

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to view the list of guild members.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the member list; otherwise false.</returns>
        public bool TryViewMembers(IGuildMember invoker)
        {
            // Ensure the parameters are valid
            if (!EnsureValidEventSource(invoker))
                return false;

            return InternalTryViewMembers(invoker);
        }

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to view the list of online guild members.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the online member list; otherwise false.</returns>
        public bool TryViewOnlineMembers(IGuildMember invoker)
        {
            // Ensure the parameters are valid
            if (!EnsureValidEventSource(invoker))
                return false;

            return InternalTryViewOnlineMembers(invoker);
        }

        #endregion
    }
}