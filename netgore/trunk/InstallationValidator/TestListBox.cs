using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstallationValidator.Properties;

namespace InstallationValidator
{
    public sealed class TestListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestListBox"/> class.
        /// </summary>
        public TestListBox()
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

            ITestable test;
            if (e.Index < 0 || e.Index >= Items.Count || ((test = (Items[e.Index] as ITestable)) == null))
                return;

            var textRect = new Rectangle(e.Bounds.X + 10, e.Bounds.Y, e.Bounds.Width - 10, e.Bounds.Height);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            e.Graphics.DrawString(test.Name, e.Font, Brushes.Black, textRect);

            e.DrawFocusRectangle();

            Image drawImage;
            switch (test.LastRunStatus)
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

            e.Graphics.DrawImage(drawImage, new Point(e.Bounds.X, e.Bounds.Y));
        }
    }
}
