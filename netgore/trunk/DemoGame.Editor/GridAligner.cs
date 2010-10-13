using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// Aligns objects to the map grid.
    /// </summary>
    public class GridAligner
    {
        static readonly GridAligner _instance;
        bool _alignByDefault;

        /// <summary>
        /// Initializes the <see cref="GridAligner"/> class.
        /// </summary>
        static GridAligner()
        {
            _instance = new GridAligner();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridAligner"/> class.
        /// </summary>
        GridAligner()
        {
            InvertAlignmentKey = Keys.Shift;
            AlignByDefault = false;
        }

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
        /// Gets the size of the grid.
        /// </summary>
        public static Vector2 GridSize
        {
            get { return GlobalState.Instance.Map.GridSize; }
        }

        /// <summary>
        /// Gets the <see cref="GridAligner"/> instance.
        /// </summary>
        public static GridAligner Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets or sets the key that, when pressed, will invert the alignment from the default behavior to the non-default behavior.
        /// The default value is <see cref="Keys.Shift"/>.
        /// </summary>
        [DefaultValue(Keys.Shift)]
        public Keys InvertAlignmentKey { get; set; }

        public Vector2 Align(ISpatial spatial)
        {
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

        public Vector2 Resize(ISpatial spatial)
        {
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