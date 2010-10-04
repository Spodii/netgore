using System;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// A <see cref="TextBox"/> that produces visual indications if the text is changed to something different
    /// or invalid.
    /// </summary>
    public class AutoValidateTextBox : TextBox
    {
        bool _isValid;
        string _originalText;

        /// <summary>
        /// Gets if the text in this <see cref="AutoValidateTextBox"/> is currently valid;
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
            private set
            {
                if (_isValid == value)
                    return;

                _isValid = value;

                if (_isValid)
                {
                    if (TrackTextChanged && Text != _originalText)
                        BackColor = EditorColors.Changed;
                    else
                        BackColor = EditorColors.Normal;
                }
                else
                    BackColor = EditorColors.Error;
            }
        }

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
        /// <param name="sanitize">Gets if the <paramref name="text"/> should be sanitized automatically first.
        /// Default is false.</param>
        public void ChangeTextToDefault(string text, bool sanitize = false)
        {
            if (sanitize)
                text = GetSanitizedText(text);

            Text = text;
            ApplyTextChanges();
        }

        /// <summary>
        /// When overridden in the derived class, checks if the <see cref="AutoValidateTextBox"/> is in a valid
        /// state and contains valid text.
        /// </summary>
        /// <param name="text">The text to check if valid.</param>
        /// <returns>True if valid; otherwise false.</returns>
        protected virtual bool GetIsValid(string text)
        {
            return true;
        }

        /// <summary>
        /// Gets the sanitized text for this <see cref="AutoValidateTextBox"/>. This should always be used when
        /// you want to make use of the contents of the <see cref="AutoValidateTextBox"/>. The sanitized
        /// text is only guaranteed to be valid if IsValid is true.
        /// </summary>
        /// <returns>
        /// The sanitized text for this <see cref="AutoValidateTextBox"/>.
        /// </returns>
        public string GetSanitizedText()
        {
            return GetSanitizedText(Text);
        }

        /// <summary>
        /// Gets the sanitized text for this <see cref="AutoValidateTextBox"/>. This should always be used when
        /// you want to make use of the contents of the <see cref="AutoValidateTextBox"/>. The sanitized
        /// text is only guaranteed to be valid if IsValid is true.
        /// </summary>
        /// <param name="text">The text to sanitize.</param>
        /// <returns>The sanitized text for this <see cref="AutoValidateTextBox"/>.</returns>
        public virtual string GetSanitizedText(string text)
        {
            return text;
        }

        /// <summary>
        /// Handles when the text has changed.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (DesignMode)
                return;

            if (Text == null)
            {
                IsValid = false;
                return;
            }

            var sanitized = GetSanitizedText(Text);
            IsValid = GetIsValid(sanitized);
        }
    }
}