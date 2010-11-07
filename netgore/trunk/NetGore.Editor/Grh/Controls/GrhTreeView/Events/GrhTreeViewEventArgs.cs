using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// Contains the event arguments for a general <see cref="GrhTreeView"/> event.
    /// </summary>
    public class GrhTreeViewEventArgs : TreeViewEventArgs
    {
        readonly GrhData _grhData;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhTreeViewEventArgs"/> class.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/>.</param>
        /// <param name="args">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        public GrhTreeViewEventArgs(GrhData grhData, TreeViewEventArgs args) : base(args.Node, args.Action)
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