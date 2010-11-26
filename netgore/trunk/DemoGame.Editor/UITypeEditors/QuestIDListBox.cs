using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Editor;
using NetGore.Features.Quests;
using NetGore.IO;

namespace DemoGame.Editor.UITypeEditors
{
    public class QuestIDListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestIDListBox"/> class.
        /// </summary>
        public QuestIDListBox()
        {
            if (DesignMode)
                return;

            DrawMode = DrawMode.OwnerDrawFixed;
            Sorted = true;
        }

        /// <summary>
        /// Gets the string to draw for a list item.
        /// </summary>
        /// <param name="x">The item to draw.</param>
        /// <returns>The string to draw for a list item.</returns>
        static string GetDrawString(QuestID x)
        {
            var qdc = QuestDescriptionCollection.Create(ContentPaths.Dev);
            var t = qdc[x];

            if (t == null)
                return x.ToString();
            else
                return t.QuestID + ". " + t.Name + " - " + t.Description;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DesignMode || !ControlHelper.DrawListItem<QuestID>(Items, e, x => GetDrawString(x)))
                base.OnDrawItem(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (DesignMode)
                return;

            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Delete)
                this.DeleteSelectedItem();
        }

        /// <summary>
        /// Sorts the items in the <see cref="T:System.Windows.Forms.ListBox"/>.
        /// </summary>
        protected override void Sort()
        {
            if (DesignMode)
                return;

            var v = Items.Cast<QuestID>().ToImmutable();
            Items.Clear();
            Items.AddRange(v.Cast<object>().ToArray());
        }
    }
}