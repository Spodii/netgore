using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Server;
using NetGore;
using NetGore.EditorTools;

namespace DemoGame.EditorTools
{
    public class CharacterTemplateAmountListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateAmountListBox"/> class.
        /// </summary>
        public CharacterTemplateAmountListBox()
        {
            if (DesignMode)
                return;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            DrawMode = DrawMode.OwnerDrawFixed;
            Sorted = true;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
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
                {
                    this.RemoveItemAtAndReselect(SelectedIndex);
                }
            }
        }

        /// <summary>
        /// Sorts the items in the <see cref="T:System.Windows.Forms.ListBox"/>.
        /// </summary>
        protected override void Sort()
        {
            if (DesignMode)
                return;

            var v = Items.Cast<MutablePair<CharacterTemplateID, ushort>>().ToImmutable();
            Items.Clear();
            Items.AddRange(v.Cast<object>().ToArray());
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

            var item = (MutablePair<CharacterTemplateID, ushort>)Items[e.Index];

            e.DrawBackground();

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            // Draw the key
            var keyStr = item.Key.ToString();

            var itm = CharacterTemplateManager.Instance;
            if (itm != null)
            {
                var template = itm[item.Key];
                if (template != null)
                    keyStr += " [" + template.TemplateTable.Name + "]";
            }
            keyStr += " - ";

            using (var brush = new SolidBrush((e.State & DrawItemState.Selected) != 0 ? Color.LightGreen : Color.Green))
            {
                e.Graphics.DrawString(keyStr, e.Font, brush, e.Bounds);
            }

            var keyWidth = (int)e.Graphics.MeasureString(keyStr, e.Font).Width;
            var valueBounds = new Rectangle(e.Bounds.X + keyWidth, e.Bounds.Y, e.Bounds.Width - keyWidth, e.Bounds.Height);

            // Draw the value
            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(item.Value.ToString(), e.Font, brush, valueBounds);
            }

            if ((e.State & DrawItemState.Selected) != 0)
                e.DrawFocusRectangle();
        }
    }
}