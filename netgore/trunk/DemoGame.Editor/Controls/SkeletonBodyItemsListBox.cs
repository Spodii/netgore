using System.Linq;
using System.Windows.Forms;
using NetGore.Editor;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="ListBox"/> for the <see cref="SkeletonBodyItem"/>s.
    /// </summary>
    public class SkeletonBodyItemsListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonBodyItemsListBox"/> class.
        /// </summary>
        public SkeletonBodyItemsListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        static string GetDSIString(SkeletonBodyItem dsi)
        {
            var s = "_null_";
            if (dsi.Source != null)
                s = dsi.Source.Name;
            var d = "_null_";
            if (dsi.Dest != null)
                d = dsi.Dest.Name;

            string textureName;
            if (dsi.Grh.GrhData == null)
                textureName = "*";
            else
                textureName = dsi.Grh.GrhData.Categorization.ToString();

            return textureName + ": " + s + " -> " + d;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ComboBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (!ControlHelper.DrawListItem<SkeletonBodyItem>(Items, e, x => GetDSIString(x)))
                base.OnDrawItem(e);
        }
    }
}