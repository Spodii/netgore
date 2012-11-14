using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Describes a single item on a single atlas texture. This describes what <see cref="ITextureAtlasable"/>
    /// item it is, and where in the atlas texture the <see cref="ITextureAtlasable"/> will go.
    /// </summary>
    class AtlasTextureItem
    {
        readonly Rectangle _rect;

        /// <summary>
        /// Initializes a new instance of the <see cref="AtlasTextureItem"/> class.
        /// </summary>
        /// <param name="width">Width of the rectangular area the tree nodes can occupy.</param>
        /// <param name="height">Height of the rectangular area the tree nodes can occupy.</param>
        public AtlasTextureItem(int width, int height)
        {
            ITextureAtlasable = null;
            _rect = new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtlasTextureItem"/> class.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> describing where to put this item.</param>
        internal AtlasTextureItem(Rectangle rect)
        {
            ITextureAtlasable = null;
            _rect = rect;
        }

        /// <summary>
        /// Gets the node's height.
        /// </summary>
        public int Height
        {
            get { return Rect.Height; }
        }

        /// <summary>
        /// Gets or sets the <see cref="ITextureAtlasable"/> associated with the node (leaf nodes only).
        /// </summary>
        public ITextureAtlasable ITextureAtlasable { get; internal set; }

        /// <summary>
        /// Gets the rectangular area of the node.
        /// </summary>
        public Rectangle Rect
        {
            get { return _rect; }
        }

        /// <summary>
        /// Gets the node's width.
        /// </summary>
        public int Width
        {
            get { return Rect.Width; }
        }

        /// <summary>
        /// Gets the X coordinate of the node.
        /// </summary>
        public int X
        {
            get { return Rect.X; }
        }

        /// <summary>
        /// Gets the Y coordinate of the node.
        /// </summary>
        public int Y
        {
            get { return Rect.Y; }
        }
    }
}