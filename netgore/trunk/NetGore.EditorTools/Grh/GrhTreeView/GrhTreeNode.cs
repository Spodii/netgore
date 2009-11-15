using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    /// <summary>
    /// A node for a <see cref="GrhTreeView"/>.
    /// </summary>
    public class GrhTreeNode
    {
        readonly Grh _grh;
        readonly TreeNode _treeNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhTreeNode"/> class.
        /// </summary>
        /// <param name="tn">The <see cref="TreeNode"/>.</param>
        /// <param name="grh">The <see cref="Grh"/>.</param>
        public GrhTreeNode(TreeNode tn, Grh grh)
        {
            _treeNode = tn;
            _grh = grh;
        }

        /// <summary>
        /// Gets the <see cref="Grh"/>.
        /// </summary>
        public Grh Grh
        {
            get { return _grh; }
        }

        /// <summary>
        /// Gets the <see cref="TreeNode"/>.
        /// </summary>
        public TreeNode TreeNode
        {
            get { return _treeNode; }
        }
    }
}