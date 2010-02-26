using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics.GUI;

namespace NetGore.Features.Quests
{
    public class QuestDescriptionListBox : ListBox<IQuestDescription>
    {
        readonly Func<QuestID, bool> _hasStartQuestReqs;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestDescriptionListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <param name="hasStartQuestReqs">A func used to check if the user has the requirements to start a quest.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public QuestDescriptionListBox(Control parent, Vector2 position, Vector2 clientSize, Func<QuestID, bool> hasStartQuestReqs)
            : base(parent, position, clientSize)
        {
            _hasStartQuestReqs = hasStartQuestReqs;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestDescriptionListBox"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <param name="hasStartQuestReqs">A func used to check if the user has the requirements to start a quest.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public QuestDescriptionListBox(IGUIManager guiManager, Vector2 position, Vector2 clientSize,
                                       Func<QuestID, bool> hasStartQuestReqs) : base(guiManager, position, clientSize)
        {
            _hasStartQuestReqs = hasStartQuestReqs;
        }

        /// <summary>
        /// Gets or sets the items to display.
        /// </summary>
        public override IEnumerable<IQuestDescription> Items
        {
            get
            {
                var ret = base.Items.OrderBy(x => x.QuestID).OrderBy(x => _hasStartQuestReqs(x.QuestID));
                return ret;
            }
            set { base.Items = value; }
        }

        /// <summary>
        /// Gets the default item drawer.
        /// </summary>
        /// <returns>The default item drawer.</returns>
        protected override Action<SpriteBatch, Vector2, int> GetDefaultItemDrawer()
        {
            return QuestDescriptionDrawer;
        }

        /// <summary>
        /// Gets if the user has the requirements to start the given quest.
        /// </summary>
        /// <param name="questID">The ID of the quest to start.</param>
        /// <returns>True if the user has the requirements to start the quest with the given <paramref name="questID"/>;
        /// otherwise false.</returns>
        protected virtual bool HasStartQuestReqs(QuestID questID)
        {
            return _hasStartQuestReqs(questID);
        }

        void QuestDescriptionDrawer(SpriteBatch sb, Vector2 pos, int index)
        {
            var item = Items.ElementAtOrDefault(index);
            if (item == null)
                return;

            string indexStr = "  " + (index + 1) + ". ";
            var indexStrWidth = Font.MeasureString(indexStr).X;

            pos = pos.Round();

            sb.DrawString(Font, indexStr, pos, ForeColor);

            var color = HasStartQuestReqs(item.QuestID) ? ForeColor : new Color(150, 0, 0);
            sb.DrawString(Font, item.Name, pos + new Vector2(indexStrWidth, 0), color);
        }
    }
}