using System.Linq;
using System.Windows.Forms;
using log4net.Core;

namespace DemoGame.Server.UI
{
    sealed class LogListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogListBox"/> class.
        /// </summary>
        public LogListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DoubleBuffered = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0 || e.Index >= Items.Count)
                return;

            var item = Items[e.Index] as LoggingEvent;
            if (item == null)
                return;

            e.Graphics.DrawString(item.RenderedMessage, e.Font, item.Level.GetSystemColorBrush(), e.Bounds);

            e.DrawFocusRectangle();
        }
    }
}