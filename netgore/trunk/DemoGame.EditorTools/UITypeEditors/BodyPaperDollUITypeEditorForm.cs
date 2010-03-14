using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Graphics;
using NetGore.IO;

namespace DemoGame.EditorTools
{
    /// <summary>
    /// A <see cref="Form"/> for listing the character template information from the database.
    /// </summary>
    public class BodyPaperDollUITypeEditorForm : UITypeEditorDbListForm<string>
    {
        readonly string _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyPaperDollUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The default selected item.</param>
        public BodyPaperDollUITypeEditorForm(string selected)
        {
            _selected = selected;
        }

        /// <summary>
        /// When overridden in the derived class, draws the <paramref name="item"/>.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawItemEventArgs"/> instance containing the event data.</param>
        /// <param name="item">The item being drawn.</param>
        protected override void DrawListItem(DrawItemEventArgs e, string item)
        {
            e.DrawBackground();

            if (item == null)
                return;

            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(item, e.Font, brush, e.Bounds);
            }

            if (e.State == DrawItemState.Selected)
                e.DrawFocusRectangle();
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<string> GetListItems()
        {
            return SkeletonBodyInfo.GetBodyNames(ContentPaths.Build).OrderBy(x => x, NaturalStringComparer.Instance);
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override string SetDefaultSelectedItem(IEnumerable<string> items)
        {
            if (string.IsNullOrEmpty(_selected))
                return base.SetDefaultSelectedItem(items);

            return _selected;
        }
    }
}