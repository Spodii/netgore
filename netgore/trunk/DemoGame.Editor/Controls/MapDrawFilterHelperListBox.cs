using System.Linq;
using System.Windows.Forms;
using NetGore.Editor;
using NetGore.Editor.WinForms;

namespace DemoGame.Editor.Forms
{
    /// <summary>
    /// A <see cref="ListBox"/> that shows a list of <see cref="MapDrawFilterHelper"/>s and tries to show their name by
    /// using a <see cref="MapDrawFilterHelperCollection"/>.
    /// </summary>
    public class MapDrawFilterHelperListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDrawFilterHelperListBox"/> class.
        /// </summary>
        public MapDrawFilterHelperListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            Sorted = true;
        }

        /// <summary>
        /// Gets or sets the <see cref="MapDrawFilterHelperCollection"/> to use to grab the name of the items.
        /// </summary>
        public MapDrawFilterHelperCollection Collection { get; set; }

        /// <summary>
        /// Gets the string to draw for a list item.
        /// </summary>
        /// <param name="x">The list item.</param>
        /// <returns>The string to draw for a list item.</returns>
        string GetDrawString(MapDrawFilterHelper x)
        {
            var c = Collection;

            if (x == null)
                return "[NULL]";

            if (c == null)
                return x.ToString();
            else
                return c.TryGetName(x) ?? x.ToString();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DesignMode || !ControlHelper.DrawListItem<MapDrawFilterHelper>(Items, e, x => GetDrawString(x)))
                base.OnDrawItem(e);
        }
    }
}