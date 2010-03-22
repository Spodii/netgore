using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.EditorTools;
using NetGore.Features.Quests;
using NetGore.IO;

namespace DemoGame.EditorTools
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

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            DrawMode = DrawMode.OwnerDrawFixed;
            Sorted = true;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DesignMode)
                return;

            if (e.Index < 0 || e.Index >= Items.Count || Items[e.Index] == null)
            {
                base.OnDrawItem(e);
                return;
            }

            var item = (QuestID)Items[e.Index];

            e.DrawBackground();
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            string str;

            var qdc = QuestDescriptionCollection.Create(ContentPaths.Dev);
            var t = qdc[item];

            if (t == null)
                str = item.ToString();
            else
                str = t.QuestID + ". " + t.Name + " - " + t.Description;

            // Draw the value
            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(str, e.Font, brush, e.Bounds);
            }

            if ((e.State & DrawItemState.Selected) != 0)
                e.DrawFocusRectangle();
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
            {
                if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
                    this.RemoveItemAtAndReselect(SelectedIndex);
            }
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