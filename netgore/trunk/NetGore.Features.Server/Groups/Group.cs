using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Features.Groups
{
    public interface IGroup : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="IGroupable"/> that is the founder of this group. If this value is false, the group
        /// is assumed to be disbanded.
        /// </summary>
        IGroupable Founder { get; }

        /// <summary>
        /// Notifies listeners when this group has been disbanded.
        /// </summary>
        event IGroupEventHandler Disbanded;

        /// <summary>
        /// Notifies listeners when a group member leaves the group.
        /// </summary>
        event IGroupMemberEventHandler MemberLeave;

        /// <summary>
        /// Notifies listeners when a group member joins the group.
        /// </summary>
        event IGroupMemberEventHandler MemberJoin;

        /// <summary>
        /// Gets all of the members in the group, including the <see cref="IGroup.Founder"/>.
        /// </summary>
        IEnumerable<IGroupable> Members { get; }

        /// <summary>
        /// Forces the group to disband.
        /// </summary>
        void Disband();

        /// <summary>
        /// Tries to add an <see cref="IGroupable"/> to this group.
        /// </summary>
        /// <param name="groupable">The <see cref="IGroupable"/> to try to add to the group.</param>
        /// <returns>True if the <paramref name="groupable"/> was successfully added to the group;
        /// otherwise false.
        /// This method will always return false is the <paramref name="groupable"/> is already in a group.</returns>
        bool TryAdd(IGroupable groupable);
    }

    public delegate void IGroupEventHandler(IGroup group);

    public delegate void IGroupMemberEventHandler(IGroup group, IGroupable member);

    public class Group : IGroup
    {
        readonly List<IGroupable> _members = new List<IGroupable>();

        IGroupable _founder;

        public Group(IGroupable founder)
        {
            if (founder == null)
                throw new ArgumentNullException("founder");

            _founder = founder;
            _members.Add(_founder);
        }

        /// <summary>
        /// Gets the <see cref="IGroupable"/> that is the founder of this group. If this value is false, the group
        /// is assumed to be disbanded.
        /// </summary>
        public IGroupable Founder
        {
            get { return _founder; }
        }

        /// <summary>
        /// Notifies listeners when this group has been disbandoned.
        /// </summary>
        public event IGroupEventHandler Disbanded;

        /// <summary>
        /// Notifies listeners when a group member leaves the group.
        /// </summary>
        public event IGroupMemberEventHandler MemberLeave;

        /// <summary>
        /// Notifies listeners when a group member joins the group.
        /// </summary>
        public event IGroupMemberEventHandler MemberJoin;

        /// <summary>
        /// Gets all of the members in the group, including the <see cref="IGroup.Founder"/>.
        /// </summary>
        public IEnumerable<IGroupable> Members
        {
            get { return _members; }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of the <see cref="IGroup.Disbanded"/> event.
        /// </summary>
        protected virtual void OnDisbanded()
        {
        }

        /// <summary>
        /// Forces the group to disband.
        /// </summary>
        public void Disband()
        {
            // Raise events
            OnDisbanded();

            if (Disbanded != null)
                Disbanded(this);

            // Clear the founder
            _founder = null;
        }

        static readonly GroupSettings _groupSettings = GroupSettings.Instance;

        /// <summary>
        /// Tries to add an <see cref="IGroupable"/> to this group.
        /// </summary>
        /// <param name="groupable">The <see cref="IGroupable"/> to try to add to the group.</param>
        /// <returns>
        /// True if the <paramref name="groupable"/> was successfully added to the group;
        /// otherwise false.
        /// This method will always return false is the <paramref name="groupable"/> is already in a group.
        /// </returns>
        public virtual bool TryAdd(IGroupable groupable)
        {
            // Check the max members value
            if (_members.Count >= _groupSettings.MaxMembersPerGroup)
                return false;

            // Ensure not already in a group
            if (groupable.Group != null)
                return false;

            // Add the member
            _members.Add(groupable);
            groupable.Group = this;

            return true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Disband();
        }
    }
}
