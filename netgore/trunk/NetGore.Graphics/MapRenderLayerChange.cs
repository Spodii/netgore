using System.Linq;
using NetGore;

namespace NetGore.Graphics
{
    /// <summary>
    /// Delegate for when an <see cref="IDrawable"/>'s <see cref="MapRenderLayer"/> is changed.
    /// </summary>
    /// <param name="drawableEntity"><see cref="IDrawable"/> that changed their <see cref="MapRenderLayer"/>.</param>
    /// <param name="oldLayer">The previous <see cref="MapRenderLayer"/> value.</param>
    public delegate void MapRenderLayerChange(IDrawable drawableEntity, MapRenderLayer oldLayer);
}