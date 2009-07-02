using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Node of a GrhTreeView
    /// </summary>
    public class GrhTreeNode
    {
        public readonly Grh Grh;
        public readonly TreeNode TreeNode;

        public GrhTreeNode(TreeNode tn, Grh grh)
        {
            TreeNode = tn;
            Grh = grh;
        }
    }
}