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
            AutoCompleteMode = AutoCompleteSources.DefaultAutoCompleteMode;
            AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteCustomSource = AutoCompleteSources.Textures;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            // Change to the correct slash direction
            if (e.KeyChar == '\\')
                e.KeyChar = '/';

            base.OnKeyPress(e);
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

            if (!File.Exists(ContentPaths.Build.Grhs.Join(txt) + "." + ContentPaths.CompiledContentSuffix))
                return false;
                
            return base.IsValid();
        }
    }
}
