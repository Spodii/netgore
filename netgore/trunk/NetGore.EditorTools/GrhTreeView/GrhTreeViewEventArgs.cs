using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    public class GrhTreeViewEventArgs : TreeViewEventArgs
    {
        public readonly GrhData GrhData;

        public GrhTreeViewEventArgs(GrhData grhData, TreeViewEventArgs args) : base(args.Node, args.Action)
        {
            GrhData = grhData;
        }
    }
}