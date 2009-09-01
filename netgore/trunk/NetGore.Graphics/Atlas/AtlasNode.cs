using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics
{
    public class AtlasNode
    {
        ITextureAtlas _iTextureAtlas = null;
        AtlasNode _left = null;
        Rectangle _rect;
        AtlasNode _right = null;

        /// <summary>
        /// Gets the height of the node's rectangle (Rect.Height)
        /// </summary>
        public int Height
        {
            get { return Rect.Height; }
        }

        /// <summary>
        /// Gets if the node is a leaf (has no children)
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                // A node will always have the right child set unless a leaf
                // When we detatch a set leaf, we always detatch just the left one
                return Right == null;
            }
        }

        /// <summary>
        /// Gets or sets the ITextureAtlas associated with the node (leaf nodes only)
        /// </summary>
        public ITextureAtlas ITextureAtlas
        {
            get { return _iTextureAtlas; }
            set { _iTextureAtlas = value; }
        }

        /// <summary>
        /// Gets or sets the left child node
        /// </summary>
        public AtlasNode Left
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// Gets or sets the rectangular area of the node
        /// </summary>
        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }

        /// <summary>
        /// Gets or sets te right child node
        /// </summary>
        public AtlasNode Right
        {
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        /// Gets the width of the node's rectangle (Rect.Width)
        /// </summary>
        public int Width
        {
            get { return Rect.Width; }
        }

        /// <summary>
        /// Gets the X coordinate of the node's rectangle (Rect.X)
        /// </summary>
        public int X
        {
            get { return Rect.X; }
        }

        /// <summary>
        /// Gets the Y coordinate of the node's rectangle (Rect.Y)
        /// </summary>
        public int Y
        {
            get { return Rect.Y; }
        }

        /// <summary>
        /// AtlasNode constructor
        /// </summary>
        /// <param name="width">Initial width of the canvas</param>
        /// <param name="height">Initial height of the canvas</param>
        public AtlasNode(int width, int height)
        {
            _rect = new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// AtlasNode constructor
        /// </summary>
        /// <param name="rect">Rectangle area for the node</param>
        AtlasNode(Rectangle rect)
        {
            _rect = rect;
        }

        /// <summary>
        /// Inserts an area into the node
        /// </summary>
        /// <param name="writer">Width of the area to insert</param>
        /// <param name="h">Height of the area to insert</param>
        /// <returns>Node representing the new area</returns>
        public AtlasNode Insert(int w, int h)
        {
            if (!IsLeaf)
            {
                // Insert into children (left first then right)
                AtlasNode ret = null;
                if (Left != null)
                    ret = Left.Insert(w, h);
                if (ret == null && Right != null)
                    ret = Right.Insert(w, h);
                return ret;
            }
            else
            {
                // Check if it fits and not in use
                if (ITextureAtlas != null || w > Rect.Width || h > Rect.Height)
                    return null;

                // Perfect fit
                if (w == Rect.Width && h == Rect.Height)
                    return this;

                // Not a perfect fit, split the node up into new nodes
                int diffW = Rect.Width - w;
                int diffH = Rect.Height - h;

                // Decide which way to split
                if (diffW > diffH)
                {
                    // Split vertically
                    Left = new AtlasNode(new Rectangle(Rect.Left, Rect.Top, w, Rect.Height));
                    Right = new AtlasNode(new Rectangle(Rect.Left + w, Rect.Top, Rect.Width - w, Rect.Height));
                }
                else
                {
                    // Split horizontally
                    Left = new AtlasNode(new Rectangle(Rect.Left, Rect.Top, Rect.Width, h));
                    Right = new AtlasNode(new Rectangle(Rect.Left, Rect.Top + h, Rect.Width, Rect.Height - h));
                }

                // Insert the rectangle first new child
                AtlasNode node = Left.Insert(w, h);

                // If the insert is a perfect fit, drop the node from the tree to prevent checking it again
                if (diffW == 0 || diffH == 0)
                    Left = null;

                return node;
            }
        }
    }
}