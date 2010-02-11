using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Handles events from an <see cref="IGroup"/>.
    /// </summary>
    /// <param name="group">The <see cref="IGroup"/> the event came from.</param>
    /// <param name="member">The group member the event is related to.</param>
    public delegate void IGroupMemberEventHandler(IGroup group, IGroupable member);
}