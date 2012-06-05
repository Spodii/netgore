using System.Linq;
using System.Windows.Forms;
using NetGore.Content;
using NetGore.Editor.WinForms;

namespace NetGore.Editor.Grhs
{
    public class GrhDataTextureTextBox : AutoValidateTextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrhDataTextureTextBox"/> class.
        /// </summary>
        public GrhDataTextureTextBox()
        {
            AutoCompleteMode = AutoCompleteSources.DefaultAutoCompleteMode;
            AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteCustomSource = AutoCompleteSources.Textures;
        }

        /// <summary>
        /// When overridden in the derived class, checks if the <see cref="AutoValidateTextBox"/> is in a valid
        /// state and contains valid text.
        /// </summary>
        /// <returns>True if valid; otherwise false.</returns>
        protected override bool GetIsValid(string text)
        {
            var assetName = new TextureAssetName(text);

            if (!assetName.ContentExists())
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
            return ContentAssetName.Sanitize(text);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            // Change to the correct slash direction
            if (e.KeyChar == '\\')
                e.KeyChar = '/';

            base.OnKeyPress(e);
        }
    }
}