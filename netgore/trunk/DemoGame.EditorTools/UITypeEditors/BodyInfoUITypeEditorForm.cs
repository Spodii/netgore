using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.EditorTools;

namespace DemoGame.EditorTools
{
    /// <summary>
    /// A <see cref="Form"/> for listing the <see cref="BodyInfo"/>.
    /// </summary>
    public class BodyInfoUITypeEditorForm : UITypeEditorListForm<BodyInfo>
    {
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyInfoUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The default selected item.</param>
        public BodyInfoUITypeEditorForm(object selected)
        {
            _selected = selected;
        }

        /// <summary>
        /// When overridden in the derived class, draws the <paramref name="item"/>.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawItemEventArgs"/> instance containing the event data.</param>
        /// <param name="item">The item being drawn.</param>
        protected override void DrawListItem(DrawItemEventArgs e, BodyInfo item)
        {
            e.DrawBackground();

            if (item == null)
                return;

            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(string.Format("{0}. {1}", item.ID, item.Body), e.Font, brush, e.Bounds);
            }

            if (e.State == DrawItemState.Selected)
                e.DrawFocusRectangle();
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<BodyInfo> GetListItems()
        {
            return BodyInfoManager.Instance.Bodies;
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override BodyInfo SetDefaultSelectedItem(IEnumerable<BodyInfo> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            if (_selected is string)
            {
                var stringComp = StringComparer.Ordinal;
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x.ID, asString));
            }

            if (_selected is BodyID)
            {
                var asID = (BodyID)_selected;
                return items.FirstOrDefault(x => x.ID == asID);
            }

            if (_selected is BodyInfo)
            {
                var asBody = (BodyInfo)_selected;
                return items.FirstOrDefault(x => x == asBody);
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}