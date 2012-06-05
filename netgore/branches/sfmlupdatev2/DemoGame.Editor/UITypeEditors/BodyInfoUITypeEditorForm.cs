using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor.UI;

namespace DemoGame.Editor.UITypeEditors
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
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>The string to display for the <paramref name="item"/>.</returns>
        protected override string GetItemDisplayString(BodyInfo item)
        {
            return item.ID + ". " + item.Body;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<BodyInfo> GetListItems()
        {
            return BodyInfoManager.Instance.Bodies.OrderBy(x => x.ID);
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