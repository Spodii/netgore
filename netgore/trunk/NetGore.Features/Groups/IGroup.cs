using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Interface for a group. Groups can contain multiple members, view the other members in the group,
    /// distribute rewards between the group, and perform group-related functions like group-chatting.
    /// </summary>
    public interface IGroup : IDisposable
    {
        /// <summary>
        /// Notifies listeners when this group has been disbanded.
        /// </summary>
        event IGroupEventHandler Disbanded;

        /// <summary>
        /// Notifies listeners when a group member joins the group.
        /// </summary>
        event IGroupMemberEventHandler MemberJoin;

        /// <summary>
        /// Notifies listeners when a group member leaves the group.
        /// </summary>
        event IGroupMemberEventHandler MemberLeave;

        /// <summary>
        /// Gets the <see cref="IGroupable"/> that is the founder of this group. If this value is false, the group
        /// is assumed to be disbanded.
        /// </summary>
        IGroupable Founder { get; }

        /// <summary>
        /// Gets all of the members in the group, including the <see cref="IGroup.Founder"/>.
        /// </summary>
        IEnumerable<IGroupable> Members { get; }

        /// <summary>
        /// Forces the group to disband.
        /// </summary>
        void Disband();

        /// <summary>
        /// Removes a member from the group.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        /// <returns>True if the <paramref name="member"/> was successfully removed; false if the <paramref name="member"/>
        /// was either not in a group already, or was in a different group. If the <paramref name="member"/> is not
        /// in this group, their <see cref="IGroupable.Group"/> value will not be altered.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is null.</exception>
        bool RemoveMember(IGroupable member);

        /// <summary>
        /// Tries to add an <see cref="IGroupable"/> to this group.
        /// </summary>
        /// <param name="groupable">The <see cref="IGroupable"/> to try to add to the group.</param>
        /// <returns>True if the <paramref name="groupable"/> was successfully added to the group;
        /// otherwise false.
        /// This method will always return false is the <paramref name="groupable"/> is already in a group.</returns>
        bool TryAdd(IGroupable groupable);
    }
}