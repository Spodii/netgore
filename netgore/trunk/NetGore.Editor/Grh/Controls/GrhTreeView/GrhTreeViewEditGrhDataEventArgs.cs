using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.Editor.Grhs
{
    public class GrhTreeViewEditGrhDataEventArgs : EventArgs
    {
        readonly bool _deleteOnCancel;
        readonly GrhData _grhData;
        readonly TreeNode _node;

        public GrhTreeViewEditGrhDataEventArgs(TreeNode node, GrhData grhData, bool deleteOnCancel)
        {
            _node = node;
            _grhData = grhData;
            _deleteOnCancel = deleteOnCancel;
        }

        public bool DeleteOnCancel
        {
            get { return _deleteOnCancel; }
        }

        public GrhData GrhData
        {
            get { return _grhData; }
        }

        public TreeNode TreeNode
        {
            get { return _node; }
        }
    }
}