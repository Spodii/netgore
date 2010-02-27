using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Delegate for handling events from the <see cref="IDrawable"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IDrawable"/> the event came from.</param>
    /// <param name="sb">The <see cref="ISpriteBatch"/> that is being used to draw the <paramref name="sender"/>.</param>
    public delegate void IDrawableDrawEventHandler(IDrawable sender, ISpriteBatch sb);

    /// <summary>
    /// Delegate for handling events from the <see cref="IDrawable"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IDrawable"/> the event came from.</param>
    public delegate void IDrawableEventHandler(IDrawable sender);
}