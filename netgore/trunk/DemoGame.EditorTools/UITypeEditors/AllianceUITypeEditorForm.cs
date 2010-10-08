using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Server;
using DemoGame.Server.Queries;

namespace DemoGame.Editor.UITypeEditors
{
    /// <summary>
    /// A <see cref="Form"/> for listing the <see cref="Alliance"/> information from the database.
    /// </summary>
    public class AllianceUITypeEditorForm : UITypeEditorDbListForm<IAllianceTable>
    {
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The default selected item.</param>
        public AllianceUITypeEditorForm(object selected)
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
        protected override string GetItemDisplayString(IAllianceTable item)
        {
            return item.ID + ". " + item.Name;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<IAllianceTable> GetListItems()
        {
            var ids = DbController.GetQuery<SelectAllianceIDsQuery>().Execute();

            var ret = new List<IAllianceTable>();
            var templateQuery = DbController.GetQuery<SelectAllianceQuery>();
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
        protected override IAllianceTable SetDefaultSelectedItem(IEnumerable<IAllianceTable> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            if (_selected is string)
            {
                var stringComp = StringComparer.Ordinal;
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x.Name, asString));
            }

            if (_selected is AllianceID)
            {
                var asID = (AllianceID)_selected;
                return items.FirstOrDefault(x => x.ID == asID);
            }

            if (_selected is IAllianceTable)
            {
                var asTable = (IAllianceTable)_selected;
                return items.FirstOrDefault(x => x == asTable);
            }

            if (_selected is Alliance)
            {
                var asAlliance = (Alliance)_selected;
                return items.FirstOrDefault(x => x.ID == asAlliance.ID);
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}