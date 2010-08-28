using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    /// <summary>
    /// A list box for displaying <see cref="ServerInfoBase"/>s.
    /// </summary>
    public class ServerInfoListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerInfoListBox"/> class.
        /// </summary>
        public ServerInfoListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DoubleBuffered = true;
        }

        /// <summary>
        /// Refreshes the display of an item in this <see cref="ListBox"/>.
        /// </summary>
        /// <param name="item">The item to refresh.</param>
        public void RefreshItem(object item)
        {
            try
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i] == item)
                    {
                        Invalidate(GetItemRectangle(i));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }
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
            ServerInfoBase item;
            if (e.Index < 0 || e.Index >= Items.Count || ((item = (Items[e.Index] as ServerInfoBase)) == null))
                return;

            // Get the rectangle describing where to draw the text
            var textRect = new Rectangle(e.Bounds.X + iconSize, e.Bounds.Y, e.Bounds.Width - iconSize, e.Bounds.Height);

            // Set up a smooth drawing
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Draw the text
            var displayText = item.Host;
            if (item.IsBusySyncing)
                displayText += " [Syncing]";

            e.Graphics.DrawString(displayText, e.Font, Brushes.Black, textRect);

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
