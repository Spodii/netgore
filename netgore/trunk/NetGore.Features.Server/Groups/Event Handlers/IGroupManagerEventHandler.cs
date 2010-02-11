using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Delegate for handling events from an <see cref="IGroupManager"/>.
    /// </summary>
    /// <param name="groupManager">The <see cref="IGroupManager"/> the event came from.</param>
    public delegate void IGroupManagerEventHandler(IGroupManager groupManager);
}