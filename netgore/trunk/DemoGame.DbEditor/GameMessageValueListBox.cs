using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DemoGame.DbEditor
{
    /// <summary>
    /// A <see cref="ListBox"/> for a <see cref="GameMessage"/> and its corresponding value string.
    /// </summary>
    public class GameMessageValueListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMessageValueListBox"/> class.
        /// </summary>
        public GameMessageValueListBox()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            DrawMode = DrawMode.OwnerDrawFixed;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            // Ensure a valid index
            if (e.Index < 0 || e.Index >= Items.Count)
            {
                base.OnDrawItem(e);
                return;
            }
            
            // Ensure a valid type
            var itemObj = Items[e.Index];
            if (!(itemObj is KeyValuePair<GameMessage, string>))
            {
                base.OnDrawItem(e);
                return;
            }

            e.DrawBackground();

            var item = (KeyValuePair<GameMessage, string>)itemObj;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            // Draw the message key
            var keyStr = item.Key.ToString();
            using (var brush = new SolidBrush((e.State & DrawItemState.Selected) != 0 ? Color.LightGreen : Color.Green))
            {
                e.Graphics.DrawString(keyStr, e.Font, brush, e.Bounds);
            }

            var keyWidth = (int)e.Graphics.MeasureString(keyStr, e.Font).Width;
            var valueBounds = new Rectangle(e.Bounds.X + keyWidth, e.Bounds.Y, e.Bounds.Width - keyWidth, e.Bounds.Height);

            // Draw the message text
            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(item.Value, e.Font, brush, valueBounds);
            }

            if ((e.State & DrawItemState.Selected) != 0)
                e.DrawFocusRectangle();
        }
    }
}