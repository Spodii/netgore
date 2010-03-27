using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using NetGore;

namespace DemoGame.EditorTools
{
    /// <summary>
    /// A <see cref="Form"/> for listing the character template information from the database.
    /// </summary>
    public class MapUITypeEditorForm : UITypeEditorDbListForm<IMapTable>
    {
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The default selected item.</param>
        public MapUITypeEditorForm(object selected)
        {
            _selected = selected;
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>
        /// The string to display for the <paramref name="item"/>.
        /// </returns>
        protected override string GetItemDisplayString(IMapTable item)
        {
            return item.ID + ". " + item.Name;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<IMapTable> GetListItems()
        {
            var ids = DbController.GetQuery<SelectMapIDsQuery>().Execute();

            var ret = new List<IMapTable>();
            var templateQuery = DbController.GetQuery<SelectMapQuery>();
            foreach (var id in ids)
            {
                var template = templateQuery.Execute(id);
                ret.Add(template);
            }

            return ret.OrderBy(x => x.ID);
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override IMapTable SetDefaultSelectedItem(IEnumerable<IMapTable> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            if (_selected is string)
            {
                var stringComp = StringComparer.Ordinal;
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x.Name, asString));
            }

            if (_selected is MapID)
            {
                var asID = (MapID)_selected;
                return items.FirstOrDefault(x => x.ID == asID);
            }

            if (_selected is MapBase)
            {
                var asMap = (MapBase)_selected;
                return items.FirstOrDefault(x => x == asMap);
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}