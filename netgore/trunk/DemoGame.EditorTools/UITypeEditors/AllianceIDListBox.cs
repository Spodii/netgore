using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Server;
using NetGore;
using NetGore.EditorTools;

namespace DemoGame.EditorTools
{
    public class AllianceIDListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceIDListBox"/> class.
        /// </summary>
        public AllianceIDListBox()
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

            var item = (AllianceID)Items[e.Index];

            e.DrawBackground();
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            string str;

            var t = AllianceManager.Instance[item];

            if (t == null)
                str = item.ToString();
            else
                str = t.ID + ". " + t.Name;

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

            var v = Items.Cast<AllianceID>().ToImmutable();
            Items.Clear();
            Items.AddRange(v.Cast<object>().ToArray());
        }
    }
}