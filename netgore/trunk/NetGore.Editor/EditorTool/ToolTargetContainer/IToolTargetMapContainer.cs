using System.Linq;
using NetGore.Graphics;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Interface for an <see cref="IToolTargetContainer"/> that is a container for an <see cref="IDrawableMap"/>.
    /// </summary>
    public interface IToolTargetMapContainer : IToolTargetContainer
    {
        /// <summary>
        /// Gets the <see cref="IDrawingManager"/> that is used to draw to the target container.
        /// Can be null.
        /// </summary>
        IDrawingManager DrawingManager { get; }

        /// <summary>
        /// Gets the <see cref="IDrawableMap"/> that this <see cref="IToolTargetMapContainer"/> holds.
        /// Can be null.
        /// </summary>
        IDrawableMap Map { get; }
    }
}