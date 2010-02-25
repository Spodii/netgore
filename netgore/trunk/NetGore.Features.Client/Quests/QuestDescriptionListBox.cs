using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics.GUI;

namespace NetGore.Features.Quests
{
    public class QuestDescriptionListBox : ListBox<IQuestDescription>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestDescriptionListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public QuestDescriptionListBox(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestDescriptionListBox"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public QuestDescriptionListBox(IGUIManager guiManager, Vector2 position, Vector2 clientSize) : base(guiManager, position, clientSize)
        {
        }

        /// <summary>
        /// Gets the default item drawer.
        /// </summary>
        /// <returns>The default item drawer.</returns>
        protected override Action<SpriteBatch, Vector2, int> GetDefaultItemDrawer()
        {
            return QuestDescriptionDrawer;
        }

        void QuestDescriptionDrawer(SpriteBatch sb, Vector2 pos, int index)
        {
            var item = Items.ElementAtOrDefault(index);
            if (item == null)
                return;

            string indexStr = "  " + (index + 1) + ". ";
            var indexStrWidth = Font.MeasureString(indexStr).X;

            sb.DrawString(Font, indexStr, pos, ForeColor);
            sb.DrawString(Font, item.Name, pos + new Vector2(indexStrWidth, 0), ForeColor);
        }
    }
}
