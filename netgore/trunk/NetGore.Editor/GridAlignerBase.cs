using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Editor
{
    /// <summary>
    /// Base class for an object that aligns objects to the map grid.
    /// </summary>
    public abstract class GridAlignerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridAlignerBase"/> class.
        /// </summary>
        protected GridAlignerBase()
        {
            InvertAlignmentKey = Keys.Control;
            AlignByDefault = true;
        }

        /// <summary>
        /// Gets or sets if objects are aligned to the grid by default.
        /// The default value is true.
        /// </summary>
        [DefaultValue(true)]
        public bool AlignByDefault { get; set; }

        /// <summary>
        /// Gets the size of the grid.
        /// </summary>
        public abstract Vector2 GridSize { get; }

        /// <summary>
        /// Gets or sets the key that, when pressed, will invert the alignment from the default behavior to the non-default behavior.
        /// The default value is <see cref="Keys.Shift"/>.
        /// </summary>
        [DefaultValue(Keys.Shift)]
        public Keys InvertAlignmentKey { get; set; }

        /// <summary>
        /// Gets if the alignment will occur at this time.
        /// </summary>
        protected bool WillAlign
        {
            get
            {
                var keyDown = Input.IsKeyDown(InvertAlignmentKey);
                if (keyDown)
                    return !AlignByDefault;
                else
                    return AlignByDefault;
            }
        }

        /// <summary>
        /// Aligns an <see cref="ISpatial"/>'s position to the grid.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/>.</param>
        /// <param name="forceAlign">When true, aligning to the grid will be forced.</param>
        /// <returns>The position for the <paramref name="spatial"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="spatial"/> is null.</exception>
        public Vector2 Align(ISpatial spatial, bool forceAlign = false)
        {
            if (spatial == null)
                throw new ArgumentNullException("spatial");

            return Align(spatial.Position, forceAlign);
        }

        /// <summary>
        /// When aligning to the grid, aligns the given <paramref name="pos"/> to the grid.
        /// </summary>
        /// <param name="pos">The position to align to the grid.</param>
        /// <param name="forceAlign">When true, aligning to the grid will be forced.</param>
        /// <returns>The position to use.</returns>
        public Vector2 Align(Vector2 pos, bool forceAlign = false)
        {
            if (!WillAlign && !forceAlign)
                return pos;

            return (pos / GridSize).Floor() * GridSize;
        }

        /// <summary>
        /// Gets if a position is currently aligned to the grid.
        /// </summary>
        /// <param name="pos">The position to check if aligned to the grid.</param>
        /// <returns>True if aligned to the grid; otherwise false.</returns>
        public bool IsAligned(Vector2 pos)
        {
            pos = pos.Abs();
            return 
                (int)Math.Round(pos.X) % (int)Math.Round(GridSize.X) == 0 &&
                (int)Math.Round(pos.Y) % (int)Math.Round(GridSize.Y) == 0;
        }

        /// <summary>
        /// Fits an <see cref="ISpatial"/> into the grid so that all sides are snapped to the grid.
        /// The <paramref name="spatial"/> must support both <see cref="ISpatial.SupportsMove"/> and
        /// <see cref="ISpatial.SupportsResize"/>.
        /// </summary>
        /// <param name="forceAlign">When true, aligning to the grid will be forced.</param>
        /// <param name="spatial">The <see cref="ISpatial"/>.</param>
        public void Fit(ISpatial spatial, bool forceAlign = false)
        {
            if (spatial == null)
                return;

            if (!WillAlign && !forceAlign)
                return;

            if (!spatial.SupportsMove || !spatial.SupportsResize)
                return;

            var pos = Align(spatial, forceAlign);
            if (!spatial.TryMove(pos))
                return;

            var size = Resize(spatial, forceAlign);
            spatial.TryResize(size);
        }

        /// <summary>
        /// Gets the new size to give an <see cref="ISpatial"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/>.</param>
        /// <param name="forceAlign">When true, aligning to the grid will be forced.</param>
        /// <returns>The new size for the <paramref name="spatial"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="spatial"/> is null.</exception>
        public Vector2 Resize(ISpatial spatial, bool forceAlign = false)
        {
            if (spatial == null)
                throw new ArgumentNullException("spatial");

            if (!WillAlign && !forceAlign)
                return spatial.Size;

            var s = Align(spatial.Max, forceAlign) - spatial.Position;

            if (s.X < GridSize.X)
                s.X = GridSize.X;
            if (s.Y < GridSize.Y)
                s.Y = GridSize.Y;

            return s;
        }
    }
}