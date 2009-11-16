using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.IO;

namespace NetGore.EditorTools
{
    public class GrhDataTextureTextBox : AutoValidateTextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrhDataTextureTextBox"/> class.
        /// </summary>
        public GrhDataTextureTextBox()
        {
            AutoCompleteMode = AutoCompleteMode.Suggest;
            AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteCustomSource = AutoCompleteSources.Textures;
        }

        /// <summary>
        /// When overridden in the derived class, checks if the <see cref="AutoValidateTextBox"/> is in a valid
        /// state and contains valid text.
        /// </summary>
        /// <returns>True if valid; otherwise false.</returns>
        protected override bool IsValid()
        {
            var txt = Text;

            if (string.IsNullOrEmpty(txt))
                return false;

            return File.Exists(ContentPaths.Build.Grhs.Join(txt) + "." + ContentPaths.CompiledContentSuffix);
        }
    }
}
