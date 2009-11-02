using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace NetGore.Graphics
{
    /// <summary>
    /// Describes a single item on a single atlas texture. This describes what <see cref="ITextureAtlasable"/>
    /// item it is, and where in the atlas texture the <see cref="ITextureAtlasable"/> will go.
    /// </summary>
    internal class AtlasTextureItem
    {
        ITextureAtlasable _iTextureAtlas = null;
        Rectangle _rect;

        /// <summary>
        /// Gets the node's height.
        /// </summary>
        public int Height
        {
            get { return Rect.Height; }
        }

        /// <summary>
        /// Gets or sets the ITextureAtlasable associated with the n (leaf nodes only)
        /// </summary>
        public ITextureAtlasable ITextureAtlas
        {
            get { return _iTextureAtlas; }
            internal set { _iTextureAtlas = value; }
        }

        /// <summary>
        /// Gets or sets the rectangular area of the n
        /// </summary>
        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }

        /// <summary>
        /// Gets the width of the n's rectangle (Rect.Width)
        /// </summary>
        public int Width
        {
            get { return Rect.Width; }
        }

        /// <summary>
        /// Gets the X coordinate of the n's rectangle (Rect.X)
        /// </summary>
        public int X
        {
            get { return Rect.X; }
        }

        /// <summary>
        /// Gets the Y coordinate of the n's rectangle (Rect.Y)
        /// </summary>
        public int Y
        {
            get { return Rect.Y; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtlasTextureItem"/> class.
        /// </summary>
        /// <param name="width">Initial width of the canvas</param>
        /// <param name="height">Initial height of the canvas</param>
        public AtlasTextureItem(int width, int height)
        {
            _rect = new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtlasTextureItem"/> class.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> describing where to put this item.</param>
        internal AtlasTextureItem(Rectangle rect)
        {
            _rect = rect;
        }
    }
}