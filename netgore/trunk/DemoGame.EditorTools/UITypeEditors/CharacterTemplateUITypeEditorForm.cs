using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Server;
using DemoGame.Server.Queries;

namespace DemoGame.EditorTools
{
    /// <summary>
    /// A <see cref="Form"/> for listing the character template information from the database.
    /// </summary>
    public class CharacterTemplateUITypeEditorForm : UITypeEditorDbListForm<ICharacterTemplateTable>
    {
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The default selected item.</param>
        public CharacterTemplateUITypeEditorForm(object selected)
        {
            _selected = selected;
        }

        /// <summary>
        /// When overridden in the derived class, draws the <paramref name="item"/>.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawItemEventArgs"/> instance containing the event data.</param>
        /// <param name="item">The item being drawn.</param>
        protected override void DrawListItem(DrawItemEventArgs e, ICharacterTemplateTable item)
        {
            e.DrawBackground();

            if (item == null)
                return;

            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(string.Format("{0}. {1}", item.ID, item.Name), e.Font, brush, e.Bounds);
            }

            if (e.State == DrawItemState.Selected)
                e.DrawFocusRectangle();
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<ICharacterTemplateTable> GetListItems()
        {
            var ids = DbController.GetQuery<SelectCharacterTemplateIDsQuery>().Execute();

            var ret = new List<ICharacterTemplateTable>();
            var templateQuery = DbController.GetQuery<SelectCharacterTemplateQuery>();
            foreach (var id in ids)
            {
                var template = templateQuery.Execute(id);
                ret.Add(template);
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override ICharacterTemplateTable SetDefaultSelectedItem(IEnumerable<ICharacterTemplateTable> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            if (_selected is string)
            {
                var stringComp = StringComparer.Ordinal;
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x.Name, asString));
            }

            if (_selected is CharacterTemplateID)
            {
                var asID = (CharacterTemplateID)_selected;
                return items.FirstOrDefault(x => x.ID == asID);
            }

            if (_selected is ICharacterTemplateTable)
            {
                var asTable = (ICharacterTemplateTable)_selected;
                return items.FirstOrDefault(x => x == asTable);
            }

            if (_selected is CharacterTemplate)
            {
                var asTemplate = (CharacterTemplate)_selected;
                return items.FirstOrDefault(x => x.ID == asTemplate.TemplateTable.ID);
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}