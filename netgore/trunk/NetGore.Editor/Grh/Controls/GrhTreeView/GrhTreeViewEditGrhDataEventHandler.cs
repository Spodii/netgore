using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.Editor.Grhs
{
    public delegate void GrhTreeViewEditGrhDataEventHandler(GrhTreeView sender, TreeNode node, GrhData gd, bool deleteOnCancel);
}