using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// Interface for a <see cref="TreeNode"/> on a <see cref="GrhTreeView"/>.
    /// </summary>
    public interface IGrhTreeViewNode
    {
        /// <summary>
        /// Gets the <see cref="Image"/> to use to draw the node.
        /// </summary>
        Image Image { get; }
    }
}