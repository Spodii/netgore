using System;
using System.ComponentModel;
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
        bool _alignByDefault;

        /// <summary>
        /// Gets or sets if objects are aligned to the grid by default.
        /// The default value is false.
        /// </summary>
        [DefaultValue(false)]
        public bool AlignByDefault
        {
            get { return _alignByDefault; }
            set { _alignByDefault = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridAlignerBase"/> class.
        /// </summary>
        protected GridAlignerBase()
        {
            InvertAlignmentKey = Keys.Shift;
            AlignByDefault = false;
        }

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
        /// Aligns an <see cref="ISpatial"/>'s position to the grid.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/>.</param>
        /// <returns>The position for the <paramref name="spatial"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="spatial"/> is null.</exception>
        public Vector2 Align(ISpatial spatial)
        {
            if (spatial == null)
                throw new ArgumentNullException("spatial");

            return Align(spatial.Position);
        }

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
        /// When aligning to the grid, aligns the given <paramref name="pos"/> to the grid.
        /// </summary>
        /// <param name="pos">The position to align to the grid.</param>
        /// <returns>The position to use.</returns>
        public Vector2 Align(Vector2 pos)
        {
            if (!WillAlign)
                return pos;

            return (pos / GridSize).Round() * GridSize;
        }

        /// <summary>
        /// Fits an <see cref="ISpatial"/> into the grid so that all sides are snapped to the grid.
        /// The <paramref name="spatial"/> must support both <see cref="ISpatial.SupportsMove"/> and
        /// <see cref="ISpatial.SupportsResize"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/>.</param>
        public void Fit(ISpatial spatial)
        {
            if (spatial == null)
                return;

            if (!WillAlign)
                return;

            if (!spatial.SupportsMove || !spatial.SupportsResize)
                return;

            var pos = Align(spatial);
            if (!spatial.TryMove(pos))
                return;

            var size = Resize(spatial);
            spatial.TryResize(size);
        }

        /// <summary>
        /// Gets the new size to give an <see cref="ISpatial"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/>.</param>
        /// <returns>The new size for the <paramref name="spatial"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="spatial"/> is null.</exception>
        public Vector2 Resize(ISpatial spatial)
        {
            if (spatial == null)
                throw new ArgumentNullException("spatial");

            if (!WillAlign)
                return spatial.Size;

            var s = Align(spatial.Max);

            if (s.X < 1)
                s.X = GridSize.X;
            if (s.Y < 1)
                s.Y = GridSize.Y;

            return s;
        }
    }
}