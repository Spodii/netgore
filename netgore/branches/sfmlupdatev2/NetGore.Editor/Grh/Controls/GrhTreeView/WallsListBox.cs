using System.Linq;
using System.Windows.Forms;
using NetGore.World;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// A <see cref="ListBox"/> for a collection of <see cref="WallEntityBase"/>s.
    /// </summary>
    public class WallsListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WallsListBox"/> class.
        /// </summary>
        public WallsListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The string to display.</returns>
        static string GetDrawString(WallEntityBase item)
        {
            return string.Format("({0},{1}) [{2}x{3}]{4}", item.Position.X, item.Position.Y, item.Size.X, item.Size.Y,
                item.IsPlatform ? " - Platform" : string.Empty);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DesignMode || !ControlHelper.DrawListItem<WallEntityBase>(Items, e, x => GetDrawString(x)))
                base.OnDrawItem(e);
        }
    }
}