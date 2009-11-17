using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    /// <summary>
    /// A <see cref="TextBox"/> that produces visual indications if the text is changed to something different
    /// or invalid.
    /// </summary>
    public class AutoValidateTextBox : TextBox
    {
        string _originalText;

        /// <summary>
        /// Gets or sets if the text is tracked for being changed. Default is true.
        /// </summary>
        public bool TrackTextChanged { get; set; }

        /// <summary>
        /// Sets the current text as the "original" text.
        /// </summary>
        public void ApplyTextChanges()
        {
            TrackTextChanged = true;
            _originalText = string.Empty;
        }

        /// <summary>
        /// Changes the text and sets the new text as the default. Essentially just setting the text and
        /// <see cref="ApplyTextChanges"/> combined.
        /// </summary>
        /// <param name="text">The new default and display text.</param>
        public void ChangeTextToDefault(string text)
        {
            Text = text;
            ApplyTextChanges();
        }

        /// <summary>
        /// Handles when the text has changed.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (IsValid())
            {
                if (TrackTextChanged && Text != _originalText)
                {
                    if (BackColor != EditorColors.Changed)
                        BackColor = EditorColors.Changed;
                }
                else
                {
                    if (BackColor != EditorColors.Normal)
                        BackColor = EditorColors.Normal;
                }
            }
            else
            {
                if (BackColor != EditorColors.Error)
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
