using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    public class GrhTreeViewCancelEventArgs : TreeViewCancelEventArgs
    {
        public readonly GrhData GrhData;

        public GrhTreeViewCancelEventArgs(GrhData grhData, TreeViewCancelEventArgs args)
            : base(args.Node, args.Cancel, args.Action)
        {
            GrhData = grhData;
        }
    }
}
