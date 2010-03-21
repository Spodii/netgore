using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using DemoGame.Server.Quests;
using NetGore.Features.Quests;
using NetGore.IO;

namespace DemoGame.EditorTools
{
    /// <summary>
    /// A <see cref="Form"/> for listing the <see cref="Quest"/> information from the database.
    /// </summary>
    public class QuestUITypeEditorForm : UITypeEditorDbListForm<IQuestTable>
    {
        readonly IQuestDescriptionCollection _questDescriptions = QuestDescriptionCollection.Create(ContentPaths.Dev);
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The default selected item.</param>
        public QuestUITypeEditorForm(object selected)
        {
            _selected = selected;
        }

        /// <summary>
        /// When overridden in the derived class, draws the <paramref name="item"/>.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawItemEventArgs"/> instance containing the event data.</param>
        /// <param name="item">The item being drawn.</param>
        protected override void DrawListItem(DrawItemEventArgs e, IQuestTable item)
        {
            e.DrawBackground();

            if (item == null)
                return;

            var desc = _questDescriptions[item.ID];

            string extraText;
            if (desc != null)
                extraText = desc.Name + " - " + desc.Description;
            else
                extraText = string.Empty;

            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(string.Format("{0}. {1}", item.ID, extraText), e.Font, brush, e.Bounds);
            }

            if (e.State == DrawItemState.Selected)
                e.DrawFocusRectangle();
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<IQuestTable> GetListItems()
        {
            var ids = DbController.GetQuery<SelectQuestIDsQuery>().Execute();

            var ret = new List<IQuestTable>();
            var templateQuery = DbController.GetQuery<SelectQuestQuery>();
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
        protected override IQuestTable SetDefaultSelectedItem(IEnumerable<IQuestTable> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            if (_selected is string)
            {
                var stringComp = StringComparer.Ordinal;
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x.ID, asString));
            }

            if (_selected is QuestID)
            {
                var asID = (QuestID)_selected;
                return items.FirstOrDefault(x => x.ID == asID);
            }

            if (_selected is QuestID?)
            {
                var asID = (QuestID?)_selected;
                if (!asID.HasValue)
                    return base.SetDefaultSelectedItem(items);
                else
                    return items.FirstOrDefault(x => x.ID == asID);
            }

            if (_selected is IQuestTable)
            {
                var asTable = (IQuestTable)_selected;
                return items.FirstOrDefault(x => x == asTable);
            }

            if (_selected is Quest)
            {
                var asQuest = (Quest)_selected;
                return items.FirstOrDefault(x => x.ID == asQuest.QuestID);
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}