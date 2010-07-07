using System.Linq;
using NetGore.World;

namespace NetGore.Features.ActionDisplays
{
    /// <summary>
    /// Delegate for calling a method that implements the <see cref="ActionDisplayScriptAttribute"/>.
    /// </summary>
    /// <param name="actionDisplay">The <see cref="ActionDisplay"/> being used.</param>
    /// <param name="map">The map that the entities are on.</param>
    /// <param name="source">The <see cref="Entity"/> that this action came from (the invoker of the action).</param>
    /// <param name="target">The <see cref="Entity"/> that this action is targeting. It is possible that this will be
    /// equal to the <paramref name="source"/> or be null.</param>
    public delegate void ActionDisplayScriptHandler(ActionDisplay actionDisplay, IMap map, Entity source, Entity target);
}