using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using InstallationValidator.Properties;

namespace InstallationValidator
{
    /// <summary>
    /// A <see cref="ListBox"/> for showing the status of <see cref="ITestable"/>s.
    /// </summary>
    public class TestListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestListBox"/> class.
        /// </summary>
        public TestListBox()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            DrawMode = DrawMode.OwnerDrawFixed;
            DoubleBuffered = true;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();

            // Check for a valid index and ITestable item
            ITestable test;
            if (e.Index < 0 || e.Index >= Items.Count || ((test = (Items[e.Index] as ITestable)) == null))
                return;

            // Get the rectangle describing where to draw the text
            var textRect = new Rectangle(e.Bounds.X + 10, e.Bounds.Y, e.Bounds.Width - 10, e.Bounds.Height);

            // Set up a smooth drawing
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Draw the text
            e.Graphics.DrawString(test.Name, e.Font, Brushes.Black, textRect);

            // Draw the focus rectangle
            e.DrawFocusRectangle();

            // Get what status icon to draw based on the last status of the test
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

            // Draw the stauts icon
            e.Graphics.DrawImage(drawImage, new Point(e.Bounds.X, e.Bounds.Y));
        }
    }
}