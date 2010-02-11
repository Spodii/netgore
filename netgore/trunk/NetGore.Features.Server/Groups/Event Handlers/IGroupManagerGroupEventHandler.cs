using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Delegate for handling events from an <see cref="IGroupManager"/>.
    /// </summary>
    /// <param name="groupManager">The <see cref="IGroupManager"/> the event came from.</param>
    /// <param name="group">The <see cref="IGroup"/> related to the event.</param>
    public delegate void IGroupManagerGroupEventHandler(IGroupManager groupManager, IGroup group);
}