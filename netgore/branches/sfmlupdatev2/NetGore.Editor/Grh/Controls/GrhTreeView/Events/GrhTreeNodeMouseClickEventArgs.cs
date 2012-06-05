using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.Editor.Grhs
{
    public class GrhTreeNodeMouseClickEventArgs : TreeNodeMouseClickEventArgs
    {
        readonly GrhData _grhData;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhTreeNodeMouseClickEventArgs"/> class.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/>.</param>
        /// <param name="args">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        public GrhTreeNodeMouseClickEventArgs(GrhData grhData, TreeNodeMouseClickEventArgs args)
            : base(args.Node, args.Button, args.Clicks, args.X, args.Y)
        {
            _grhData = grhData;
        }

        /// <summary>
        /// The <see cref="GrhData"/>.
        /// </summary>
        public GrhData GrhData
        {
            get { return _grhData; }
        }
    }
}