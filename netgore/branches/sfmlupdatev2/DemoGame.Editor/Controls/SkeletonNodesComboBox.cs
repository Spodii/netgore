using System.Linq;
using System.Windows.Forms;
using NetGore.Editor;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="ComboBox"/> for the <see cref="SkeletonNode"/>s.
    /// </summary>
    public class SkeletonNodesComboBox : ComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonNodesComboBox"/> class.
        /// </summary>
        public SkeletonNodesComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ComboBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (!ControlHelper.DrawListItem<SkeletonNode>(Items, e, x => x.Name))
                base.OnDrawItem(e);
        }
    }
}