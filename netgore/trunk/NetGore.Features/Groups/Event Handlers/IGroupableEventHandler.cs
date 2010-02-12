using System.Linq;

namespace NetGore.Features.Groups
{
    /// <summary>
    /// Delegate for handling events from an <see cref="IGroupable"/>.
    /// </summary>
    /// <param name="groupable">The <see cref="IGroupable"/> the event came from.</param>
    public delegate void IGroupableEventHandler(IGroupable groupable);
}