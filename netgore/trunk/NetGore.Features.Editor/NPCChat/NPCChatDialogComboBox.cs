using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// A <see cref="ComboBox"/> for <see cref="NPCChatDialogBase"/>s.
    /// </summary>
    public class NPCChatDialogComboBox : ComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatDialogComboBox"/> class.
        /// </summary>
        public NPCChatDialogComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ComboBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"/> that contains the event data. </param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            // Get the dialog to draw
            NPCChatDialogBase dialog = null;
            if (e.Index >= 0 && e.Index < Items.Count)
                dialog = Items[e.Index] as NPCChatDialogBase;

            // If invalid, use the default drawing
            if (dialog == null)
            {
                base.OnDrawItem(e);
                return;
            }

            e.DrawBackground();
            e.Graphics.DrawString(string.Format("{0}: {1}", dialog.ID, dialog.Title), e.Font, new SolidBrush(e.ForeColor),
                e.Bounds);
            e.DrawFocusRectangle();

            // TODO: Fix the display for the selected item; do not let the text be edited
        }
    }
}