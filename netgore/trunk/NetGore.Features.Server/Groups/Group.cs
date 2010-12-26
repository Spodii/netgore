using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// A group. Groups can contain multiple members, view the other members in the group,
    /// distribute rewards between the group, and perform group-related functions like group-chatting.
    /// </summary>
    public class Group : IGroup
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly GroupSettings _groupSettings = GroupSettings.Instance;

        readonly List<IGroupable> _members = new List<IGroupable>();

        IGroupable _founder;
        GroupShareMode _shareMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="founder">The group founder.</param>
        /// <exception cref="ArgumentNullException"><paramref name="founder" /> is <c>null</c>.</exception>
        public Group(IGroupable founder)
        {
            if (founder == null)
                throw new ArgumentNullException("founder");

            _founder = founder;
            _members.Add(_founder);

            founder.Group = this;
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of the <see cref="IGroup.Disbanded"/> event.
        /// </summary>
        protected virtual void OnDisbanded()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of the <see cref="IGroup.MemberJoin"/> event.
        /// </summary>
        /// <param name="member">The group member that joined the group.</param>
        protected virtual void OnMemberJoin(IGroupable member)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of the <see cref="IGroup.MemberLeave"/> event.
        /// </summary>
        /// <param name="member">The group member that left the group.</param>
        protected virtual void OnMemberLeave(IGroupable member)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of the <see cref="IGroup.ShareModeChanged"/> event.
        /// </summary>
        protected virtual void OnShareModeChanged()
        {
        }

        #region IGroup Members

        /// <summary>
        /// Notifies listeners when this group has been disbandoned.
        /// </summary>
        public event TypedEventHandler<IGroup> Disbanded;

        /// <summary>
        /// Notifies listeners when a group member joins the group.
        /// </summary>
        public event TypedEventHandler<IGroup, EventArgs<IGroupable>> MemberJoin;

        /// <summary>
        /// Notifies listeners when a group member leaves the group.
        /// </summary>
        public event TypedEventHandler<IGroup, EventArgs<IGroupable>> MemberLeave;

        /// <summary>
        /// Notifies listeners when the <see cref="IGroup.ShareMode"/> has changed.
        /// </summary>
        public event TypedEventHandler<IGroup> ShareModeChanged;

        /// <summary>
        /// Gets the <see cref="IGroupable"/> that is the founder of this group. If this value is false, the group
        /// is assumed to be disbanded.
        /// </summary>
        public IGroupable Founder
        {
            get { return _founder; }
        }

        /// <summary>
        /// Gets all of the members in the group, including the <see cref="IGroup.Founder"/>.
        /// </summary>
        public IEnumerable<IGroupable> Members
        {
            get { return _members; }
        }

        /// <summary>
        /// Gets or sets how rewards are distributed among the group members.
        /// </summary>
        public GroupShareMode ShareMode
        {
            get { return _shareMode; }
            set
            {
                if (_shareMode == value)
                    return;

                _shareMode = value;

                if (ShareModeChanged != null)
                    ShareModeChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Forces the group to disband.
        /// </summary>
        public void Disband()
        {
            // Raise events
            OnDisbanded();

            if (Disbanded != null)
                Disbanded.Raise(this, EventArgs.Empty);

            // Clear the founder
            _founder = null;

            // Remove all members from the group
            foreach (var member in Members.ToImmutable())
            {
                RemoveMember(member);
            }

            Debug.Assert(_members.Count == 0, "Uh-oh, some members managed to not get removed!");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Disband();
        }

        /// <summary>
        /// Gets all the <see cref="IGroupable"/>s in this <see cref="IGroup"/> that are in range of the
        /// <paramref name="origin"/> group member. This will include all group members where
        /// <see cref="IGroupable.IsInShareDistance"/> returns true for the <paramref name="origin"/>.
        /// </summary>
        /// <param name="origin">The group member that will be used to get the group members that are near.</param>
        /// <param name="includeOrigin">If true, the <paramref name="origin"/> will be included in the returned
        /// collection. Otherwise, the <paramref name="origin"/> will not be included in the returned collection.</param>
        /// <returns>
        /// All the other group members within sharing range of the <paramref name="origin"/>.
        /// </returns>
        public IEnumerable<IGroupable> GetGroupMembersInShareRange(IGroupable origin, bool includeOrigin)
        {
            return _members.Where(x => x.IsInShareDistance(origin) && (includeOrigin || (x != origin)));
        }

        /// <summary>
        /// Removes a member from the group.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        /// <returns>
        /// True if the <paramref name="member"/> was successfully removed; false if the <paramref name="member"/>
        /// was either not in a group already, or was in a different group. If the <paramref name="member"/> is not
        /// in this group, their <see cref="IGroupable.Group"/> value will not be altered.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is null.</exception>
        public bool RemoveMember(IGroupable member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            // Ensure they are in this group
            if (member.Group != this)
                return false;

            // Remove their group value
            member.Group = null;

            // Remove the member from the member list
            if (!_members.Remove(member))
            {
                const string errmsg =
                    "Group member `{0}` was removed from group `{1}`, but they were not in the _group list. That is bad...";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, member, this);

                Debug.Fail(string.Format(errmsg, member, this));

                return false;
            }

            // Raise events
            OnMemberLeave(member);

            if (MemberLeave != null)
                MemberLeave.Raise(this, EventArgsHelper.Create(member));

            // If they were the founder, disband the group completely
            if (member == Founder)
                Disband();

            return true;
        }

        /// <summary>
        /// Tries to add an <see cref="IGroupable"/> to this group.
        /// </summary>
        /// <param name="groupable">The <see cref="IGroupable"/> to try to add to the group.</param>
        /// <returns>
        /// True if the <paramref name="groupable"/> was successfully added to the group;
        /// otherwise false.
        /// This method will always return false is the <paramref name="groupable"/> is already in a group.
        /// </returns>
        public virtual bool TryAddMember(IGroupable groupable)
        {
            // Check the max members value
            if (_members.Count >= _groupSettings.MaxMembersPerGroup)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to add `{0}` to group `{1}` - the group is full.", groupable, this);
                return false;
            }

            // Ensure not already in a group
            if (groupable.Group != null)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to add `{0}` to group `{1}` - they were already in a group (`{2}`).", groupable, this,
                        groupable.Group);
                return false;
            }

            // Check that they can be added
            if (_groupSettings.CanJoinGroupHandler != null && !_groupSettings.CanJoinGroupHandler(groupable, this))
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to add `{0}` to group `{1}` - GroupSettings.CanJoinGroupHandler returned false.",
                        groupable, this);
                return false;
            }

            // Add the member
            _members.Add(groupable);
            groupable.Group = this;

            if (log.IsInfoEnabled)
                log.InfoFormat("Added `{0}` to group `{1}`.", groupable, this);

            // Raise events
            OnMemberJoin(groupable);

            if (MemberJoin != null)
                MemberJoin.Raise(this, EventArgsHelper.Create(groupable));

            return true;
        }

        /// <summary>
        /// Tries to invite the <paramref name="member"/> to this group.
        /// </summary>
        /// <param name="member">The <see cref="IGroupable"/> to invite.</param>
        /// <returns>True if the <paramref name="member"/> was successfully invited; otherwise false.</returns>
        public bool TryInvite(IGroupable member)
        {
            if (!_groupSettings.CanJoinGroupHandler(member, this))
                return false;

            member.NotifyInvited(this);

            return true;
        }

        #endregion
    }
}