using System.Linq;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore.Editor;
using SFML.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// Aligns objects to the <see cref="Map"/>'s grid.
    /// </summary>
    public class GridAligner : GridAlignerBase
    {
        static readonly GridAligner _instance;

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
        }

        /// <summary>
        /// Gets the size of the grid.
        /// </summary>
        public override Vector2 GridSize
        {
            get { return EditorSettings.Default.GridSize; }
        }

        /// <summary>
        /// Gets the <see cref="GridAligner"/> instance.
        /// </summary>
        public static GridAligner Instance
        {
            get { return _instance; }
        }
    }
}