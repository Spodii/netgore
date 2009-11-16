using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    public class AutoValidateTextBox : TextBox
    {
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (IsValid())
            {
                BackColor = EditorColors.Normal;
            }
            else
            {
                BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// When overridden in the derived class, checks if the <see cref="AutoValidateTextBox"/> is in a valid
        /// state and contains valid text.
        /// </summary>
        /// <returns>True if valid; otherwise false.</returns>
        protected virtual bool IsValid()
        {
            return true;
        }
    }
}
