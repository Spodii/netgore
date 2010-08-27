using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    public class FileServerListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileServerListBox"/> class.
        /// </summary>
        public FileServerListBox()
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
            const int iconSize = 0;

            e.DrawBackground();

            // Check for a valid index and ITestable item
            FileServerInfo item;
            if (e.Index < 0 || e.Index >= Items.Count || ((item = (Items[e.Index] as FileServerInfo)) == null))
                return;

            // Get the rectangle describing where to draw the text
            var textRect = new Rectangle(e.Bounds.X + iconSize, e.Bounds.Y, e.Bounds.Width - iconSize, e.Bounds.Height);

            // Set up a smooth drawing
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Draw the text
            e.Graphics.DrawString(item.Host + (item.IsBusySyncing ? " [Syncing]" : ""), e.Font, Brushes.Black, textRect);

            // Draw the focus rectangle
            e.DrawFocusRectangle();

            // Get what status icon to draw based on the last status of the test
            // TODO: Create status icon display
            /*
            Image drawImage;
            switch (item.LastRunStatus)
            {
                case TestStatus.Failed:
                    drawImage = Resources.fail;
                    break;
                case TestStatus.Passed:
                    drawImage = Resources.pass;
                    break;
                default:
                    drawImage = Resources.idle;
                    break;
            }

            // Draw the stauts icon
            e.Graphics.DrawImage(drawImage, new Point(e.Bounds.X, e.Bounds.Y));
            */
        }
    }
}
