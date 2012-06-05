using System;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.UI
{
    sealed class UITypeEditorListFormListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UITypeEditorListFormListBox"/> class.
        /// </summary>
        public UITypeEditorListFormListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        /// <summary>
        /// Gets or sets the <see cref="Action{T}"/> used to the list box items.
        /// </summary>
        public Action<DrawItemEventArgs> DrawItemHandler { get; set; }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data.</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            // Handle invalid items using the default drawing
            if (e.Index < 0 || e.Index >= Items.Count)
            {
                base.OnDrawItem(e);
                return;
            }

            // Draw the item
            if (DrawItemHandler != null)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
                DrawItemHandler(e);
            }
            else
                base.OnDrawItem(e);
        }
    }
}