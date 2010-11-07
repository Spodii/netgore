using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// Contains the event arguments for a <see cref="GrhTreeView"/> event for <see cref="TreeViewCancelEventArgs"/>.
    /// </summary>
    public class GrhTreeViewCancelEventArgs : TreeViewCancelEventArgs
    {
        readonly GrhData _grhData;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhTreeViewCancelEventArgs"/> class.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/>.</param>
        /// <param name="args">The <see cref="System.Windows.Forms.TreeViewCancelEventArgs"/> instance containing the event data.</param>
        public GrhTreeViewCancelEventArgs(GrhData grhData, TreeViewCancelEventArgs args)
            : base(args.Node, args.Cancel, args.Action)
        {
            _grhData = grhData;
        }

        /// <summary>
        /// Gets the <see cref="GrhData"/>.
        /// </summary>
        public GrhData GrhData
        {
            get { return _grhData; }
        }
    }
}