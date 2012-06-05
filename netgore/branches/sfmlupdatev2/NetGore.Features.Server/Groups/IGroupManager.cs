using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Interface for a manager of the <see cref="IGroup"/>s.
    /// </summary>
    public interface IGroupManager
    {
        /// <summary>
        /// Notifies listeners when a new group has been created.
        /// </summary>
        event TypedEventHandler<IGroupManager, EventArgs<IGroup>> GroupCreated;

        /// <summary>
        /// Gets all of the active <see cref="IGroup"/>s managed by this <see cref="IGroupManager"/>.
        /// </summary>
        IEnumerable<IGroup> Groups { get; }

        /// <summary>
        /// Creates a new <see cref="IGroup"/>.
        /// </summary>
        /// <param name="founder">The <see cref="IGroupable"/> that will be the founder of the group.</param>
        /// <returns>If the group was successfully created, returns the new <see cref="IGroup"/> with the
        /// <paramref name="founder"/> set as the group's founder. Otherwise, returns null.
        /// A group may not be created by someone who is already in a group.</returns>
        IGroup TryCreateGroup(IGroupable founder);
    }
}