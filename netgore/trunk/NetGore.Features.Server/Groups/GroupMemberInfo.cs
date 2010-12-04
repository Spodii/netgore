using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// A container that assists in managing the group state for group members.
    /// </summary>
    /// <typeparam name="T">The type of group member.</typeparam>
    public abstract class GroupMemberInfo<T> where T : IGroupable
    {
        static readonly int _inviteResponseTime = GroupSettings.Instance.InviteResponseTime;

        readonly T _owner;

        IGroup _group;
        IGroup _inviteGroup;
        TickCount _inviteTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupMemberInfo{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        protected GroupMemberInfo(T owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Gets or sets the <see cref="IGroup"/> that the <see cref="GroupMemberInfo{T}.Owner"/> is in. This should only
        /// be set by the <see cref="GroupMemberInfo{T}.Owner"/>'s <see cref="IGroupable.Group"/> property.
        /// </summary>
        public IGroup Group
        {
            get { return _group; }
            set
            {
                if (_group == value)
                    return;

                if (value == null)
                    OnLeaveGroup(_group);

                if (_group != null)
                    SetGroupEventListeners(_group, true);

                _group = value;

                if (_group != null)
                {
                    OnJoinGroup(_group);
                    SetGroupEventListeners(_group, false);
                }
            }
        }

        /// <summary>
        /// Gets the owner that this <see cref="GroupMemberInfo{T}"/> belongs to.
        /// </summary>
        public T Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// Listens to the <see cref="IGroup.MemberJoin"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{IGroupable}"/> instance containing the event data.</param>
        void Group_MemberJoin(IGroup sender, EventArgs<IGroupable> e)
        {
            // Make sure the event is for the group we are in, and the member is not us
            if (sender != Group || Owner.Equals(e))
                return;

            OnGroupMemberJoined(e.Item1);
        }

        /// <summary>
        /// Listens to the <see cref="IGroup.MemberLeave"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{IGroupable}"/> instance containing the event data.</param>
        void Group_MemberLeave(IGroup sender, EventArgs<IGroupable> e)
        {
            // Make sure the event is for the group we are in, and the member is not us
            if (sender != Group || Owner.Equals(e))
                return;

            OnGroupMemberLeft(e.Item1);
        }

        /// <summary>
        /// Handles when the <see cref="GroupMemberInfo{T}.Owner"/> is disposed.
        /// </summary>
        public virtual void HandleDisposed()
        {
            if (Group != null)
                Group.RemoveMember(Owner);
        }

        /// <summary>
        /// Tries to accept an outstanding group invite.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>The group that they joined, or null if there was no available groups to join.</returns>
        public IGroup JoinGroup(TickCount currentTime)
        {
            UpdateTime(currentTime);

            if (_inviteGroup == null || Owner.Group != null)
                return null;

            var g = _inviteGroup;

            _inviteGroup = null;

            // Try to join the group
            if (!g.TryAddMember(Owner))
            {
                OnFailedJoinGroup(_inviteGroup);
                return null;
            }

            return Owner.Group;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when the owner
        /// fails to join a group they were invited to.
        /// </summary>
        /// <param name="group">The group they failed to join.</param>
        protected virtual void OnFailedJoinGroup(IGroup group)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when a group member
        /// joins the group the <see cref="GroupMemberInfo{T}.Owner"/> is currently in.
        /// </summary>
        /// <param name="groupMember">The group member that joined the group. This will never be equal to
        /// the <see cref="GroupMemberInfo{T}.Owner"/>. That is, we will only receive events related to other
        /// group members in our group.</param>
        protected virtual void OnGroupMemberJoined(IGroupable groupMember)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when a group member
        /// leaves the group the <see cref="GroupMemberInfo{T}.Owner"/> is currently in.
        /// </summary>
        /// <param name="groupMember">The group member that joined the group. This will never be equal to
        /// the <see cref="GroupMemberInfo{T}.Owner"/>. That is, we will only receive events related to other
        /// group members in our group.</param>
        protected virtual void OnGroupMemberLeft(IGroupable groupMember)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when the owner joins a group.
        /// </summary>
        /// <param name="group">The group the owner joined.</param>
        protected virtual void OnJoinGroup(IGroup group)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when the owner leaves a group.
        /// </summary>
        /// <param name="group">The group the owner left.</param>
        protected virtual void OnLeaveGroup(IGroup group)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when the owner
        /// is invited to a group.
        /// </summary>
        /// <param name="group">The group they were invited to.</param>
        protected virtual void OnReceiveInvite(IGroup group)
        {
        }

        /// <summary>
        /// Gets the <see cref="IGroup"/> that the <see cref="GroupMemberInfo{T}.Owner"/> has an outstanding invite to.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>The <see cref="IGroup"/> that the <see cref="GroupMemberInfo{T}.Owner"/> has an outstanding invite to,
        /// or null if they do not have any outstanding invites.</returns>
        public IGroup OutstandingInviteGroup(TickCount currentTime)
        {
            UpdateTime(currentTime);

            return _inviteGroup;
        }

        /// <summary>
        /// Handles when the <see cref="GroupMemberInfo{T}.Owner"/> receives an invite to a group.
        /// </summary>
        /// <param name="group">The group they were invited to.</param>
        /// <param name="currentTime">The current time.</param>
        public void ReceiveInvite(IGroup group, TickCount currentTime)
        {
            // Don't place invites when they are already in a group
            if (Owner.Group == null)
            {
                _inviteGroup = group;
                _inviteTime = currentTime;
            }

            OnReceiveInvite(group);
        }

        /// <summary>
        /// Sets the event listeners for the current group.
        /// </summary>
        /// <param name="group">The group to set (or remove) the event listners on.</param>
        /// <param name="remove">If true, the event listeners will be removed. Otherwise, they will be added.</param>
        void SetGroupEventListeners(IGroup group, bool remove)
        {
            if (remove)
            {
                group.MemberJoin -= Group_MemberJoin;
                group.MemberLeave -= Group_MemberLeave;
            }
            else
            {
                group.MemberJoin += Group_MemberJoin;
                group.MemberLeave += Group_MemberLeave;
            }
        }

        /// <summary>
        /// Checks if the outstanding group invite has expired.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void UpdateTime(TickCount currentTime)
        {
            if (_inviteGroup != null)
            {
                if (currentTime + _inviteResponseTime < _inviteTime)
                    _inviteGroup = null;
            }
        }
    }
}