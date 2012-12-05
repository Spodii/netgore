using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.DbObjs;
using DemoGame.Server;
using DemoGame.Server.Queries;
using NetGore.Editor.UI;
using NetGore.Features.ActionDisplays;

namespace DemoGame.Editor.UITypeEditors
{
    /// <summary>
    /// A <see cref="Form"/> for listing the ActionDisplays.
    /// </summary>
    public class ActionDisplayIDUITypeEditorForm : UITypeEditorListForm<ActionDisplay>
    {
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDisplayIDUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The default selected item.</param>
        public ActionDisplayIDUITypeEditorForm(object selected)
        {
            _selected = selected;
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>The string to display for the <paramref name="item"/>.</returns>
        protected override string GetItemDisplayString(ActionDisplay item)
        {
            return item.ID + " [Script: " + (item.Script ?? string.Empty) + "]";
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<ActionDisplay> GetListItems()
        {
            return ActionDisplayScripts.ActionDisplays.OrderBy(x => x.ID);
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override ActionDisplay SetDefaultSelectedItem(IEnumerable<ActionDisplay> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            if (_selected is string)
            {
                var stringComp = StringComparer.Ordinal;
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x.ID, asString));
            }

            if (_selected is ActionDisplayID)
            {
                var asID = (ActionDisplayID)_selected;
                return items.FirstOrDefault(x => x.ID == asID);
            }

            if (_selected is ActionDisplay)
            {
                var asAD = (ActionDisplay)_selected;
                return items.FirstOrDefault(x => x == asAD);
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}