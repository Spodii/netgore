using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Handles events from an <see cref="IGroup"/>.
    /// </summary>
    /// <param name="group">The <see cref="IGroup"/> the event came from.</param>
    public delegate void IGroupEventHandler(IGroup group);
}