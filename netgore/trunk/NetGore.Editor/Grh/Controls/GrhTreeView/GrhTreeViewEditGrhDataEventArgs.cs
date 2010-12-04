using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.Editor.Grhs
{
    public class GrhTreeViewEditGrhDataEventArgs : EventArgs
    {
        readonly TreeNode _node;
        readonly GrhData _grhData;
        readonly bool _deleteOnCancel;

        public GrhTreeViewEditGrhDataEventArgs(TreeNode node, GrhData grhData, bool deleteOnCancel)
        {
            _node = node;
            _grhData = grhData;
            _deleteOnCancel = deleteOnCancel;
        }

        public TreeNode TreeNode { get { return _node; } }

        public GrhData GrhData { get { return _grhData; } }

        public bool DeleteOnCancel { get { return _deleteOnCancel; } }
    }
}