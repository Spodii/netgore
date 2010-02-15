using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Delegate for handling events from the <see cref="IDrawable"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IDrawable"/> the event came from.</param>
    /// <param name="sb">The <see cref="SpriteBatch"/> that is being used to draw the <paramref name="sender"/>.</param>
    public delegate void IDrawableDrawEventHandler(IDrawable sender, SpriteBatch sb);

    /// <summary>
    /// Delegate for handling events from the <see cref="IDrawable"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IDrawable"/> the event came from.</param>
    public delegate void IDrawableEventHandler(IDrawable sender);
}