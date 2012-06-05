using System.Linq;
using System.Windows.Forms;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using NetGore.IO;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// A <see cref="TextBox"/> for a <see cref="GrhData"/>'s category.
    /// </summary>
    public class GrhDataCategoryTextBox : AutoValidateTextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrhDataCategoryTextBox"/> class.
        /// </summary>
        public GrhDataCategoryTextBox()
        {
            AutoCompleteMode = AutoCompleteSources.DefaultAutoCompleteMode;
            AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteCustomSource = AutoCompleteSources.Categories;
        }

        /// <summary>
        /// When overridden in the derived class, checks if the <see cref="AutoValidateTextBox"/> is in a valid
        /// state and contains valid text.
        /// </summary>
        /// <param name="text">The text to check if valid.</param>
        /// <returns>True if valid; otherwise false.</returns>
        protected override bool GetIsValid(string text)
        {
            if (string.IsNullOrEmpty(Text))
                return false;

            // Ends with delimiter
            if (Text.EndsWith(SpriteCategorization.Delimiter))
                return false;

            // Starts with delimiter
            if (Text.StartsWith(SpriteCategorization.Delimiter))
                return false;

            // Contains consecutive delimiters
            if (Text.Contains(SpriteCategorization.Delimiter + SpriteCategorization.Delimiter))
                return false;

            return base.GetIsValid(text);
        }

        /// <summary>
        /// Gets the sanitized text for this <see cref="AutoValidateTextBox"/>. This should always be used when
        /// you want to make use of the contents of the <see cref="AutoValidateTextBox"/>. The sanitized
        /// text is only guaranteed to be valid if IsValid is true.
        /// </summary>
        /// <param name="text">The text to sanitize.</param>
        /// <returns>
        /// The sanitized text for this <see cref="AutoValidateTextBox"/>.
        /// </returns>
        public override string GetSanitizedText(string text)
        {
            return SpriteCategory.Sanitize(text);
        }
    }
}