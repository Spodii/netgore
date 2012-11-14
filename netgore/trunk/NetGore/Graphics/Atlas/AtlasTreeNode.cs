using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Describes a tree node for a tree used to find the location to place a <see cref="AtlasTextureItem"/>
    /// in the texture for an atlas. This is only used when building the tree. When the tree is built and
    /// all the node items have found a free place they can take on the texture, the tree is no longer needed
    /// so only the <see cref="AtlasTextureItem"/> is referenced.
    /// </summary>
    class AtlasTreeNode
    {
        readonly AtlasTextureItem _atlasNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="AtlasTreeNode"/> class.
        /// </summary>
        /// <param name="node">The <see cref="AtlasTextureItem"/> that this <see cref="AtlasTreeNode"/> is for.</param>
        public AtlasTreeNode(AtlasTextureItem node)
        {
            Right = null;
            Left = null;
            _atlasNode = node;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtlasTreeNode"/> class.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> describing the area claimed by this
        /// <see cref="AtlasTreeNode"/>.</param>
        AtlasTreeNode(Rectangle rect)
        {
            Right = null;
            Left = null;
            _atlasNode = new AtlasTextureItem(rect);
        }

        /// <summary>
        /// Gets the <see cref="AtlasTextureItem"/> that this <see cref="AtlasTreeNode"/> is for.
        /// </summary>
        public AtlasTextureItem AtlasNode
        {
            get { return _atlasNode; }
        }

        ITextureAtlasable ITextureAtlasable
        {
            get { return AtlasNode.ITextureAtlasable; }
        }

        /// <summary>
        /// Gets if this <see cref="AtlasTreeNode"/> has no children (is a leaf).
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
        /// Gets the child <see cref="AtlasTreeNode"/> from this <see cref="AtlasTreeNode"/> to the left.
        /// </summary>
        public AtlasTreeNode Left { get; private set; }

        Rectangle Rect
        {
            get { return AtlasNode.Rect; }
        }

        /// <summary>
        /// Gets the child <see cref="AtlasTreeNode"/> from this <see cref="AtlasTreeNode"/> to the right.
        /// </summary>
        public AtlasTreeNode Right { get; private set; }

        /// <summary>
        /// Inserts a <see cref="ITextureAtlasable"/> into the tree from this <see cref="AtlasTreeNode"/>.
        /// </summary>
        /// <param name="w">Width of the area to insert into.</param>
        /// <param name="h">Height of the area to insert into.</param>
        /// <param name="atlasItem">The <see cref="ITextureAtlasable"/> that inserted node is for.</param>
        /// <returns>The <see cref="AtlasTreeNode"/> containing the area the claimed, or null if no area of the
        /// required size could be found.</returns>
        public AtlasTreeNode Insert(int w, int h, ITextureAtlasable atlasItem)
        {
            if (!IsLeaf)
            {
                // Insert into children (left first then right)
                AtlasTreeNode ret = null;
                if (Left != null)
                    ret = Left.Insert(w, h, atlasItem);

                if (ret == null && Right != null)
                    ret = Right.Insert(w, h, atlasItem);

                if (ret != null)
                    ret.AtlasNode.ITextureAtlasable = atlasItem;

                return ret;
            }
            else
            {
                // Check if it fits and not in use
                if (ITextureAtlasable != null || w > Rect.Width || h > Rect.Height)
                    return null;

                // Perfect fit
                if (w == Rect.Width && h == Rect.Height)
                    return this;

                // Not a perfect fit, split the node up into new nodes
                var diffW = Rect.Width - w;
                var diffH = Rect.Height - h;

                // Decide which way to split
                if (diffW > diffH)
                {
                    // Split vertically
                    Left = new AtlasTreeNode(new Rectangle(Rect.Left, Rect.Top, w, Rect.Height));
                    Right = new AtlasTreeNode(new Rectangle(Rect.Left + w, Rect.Top, Rect.Width - w, Rect.Height));
                }
                else
                {
                    // Split horizontally
                    Left = new AtlasTreeNode(new Rectangle(Rect.Left, Rect.Top, Rect.Width, h));
                    Right = new AtlasTreeNode(new Rectangle(Rect.Left, Rect.Top + h, Rect.Width, Rect.Height - h));
                }

                // Insert the rectangle first new child
                var node = Left.Insert(w, h, atlasItem);

                // If the insert is a perfect fit, drop the node from the tree to prevent checking it again
                if (diffW == 0 || diffH == 0)
                    Left = null;

                // If the node was created, set the atlas item
                if (node != null)
                    node.AtlasNode.ITextureAtlasable = atlasItem;

                return node;
            }
        }
    }
}