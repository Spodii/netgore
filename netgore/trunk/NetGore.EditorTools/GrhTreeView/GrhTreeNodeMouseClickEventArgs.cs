using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    public class GrhTreeNodeMouseClickEventArgs : TreeNodeMouseClickEventArgs
    {
        public readonly GrhData GrhData;

        public GrhTreeNodeMouseClickEventArgs(GrhData grhData, TreeNodeMouseClickEventArgs args)
            : base(args.Node, args.Button, args.Clicks, args.X, args.Y)
        {
            GrhData = grhData;
        }
    }
}