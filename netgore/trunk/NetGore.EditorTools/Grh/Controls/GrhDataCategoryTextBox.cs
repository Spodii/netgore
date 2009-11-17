using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.EditorTools
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

        protected override bool IsValid()
        {
            if (string.IsNullOrEmpty(Text))
                return false;
            
            // Ends with delimiter
            if (Text.EndsWith(GrhDataCategorization.Delimiter))
                return false;

            // Starts with delimiter
            if (Text.StartsWith(GrhDataCategorization.Delimiter))
                return false;

            // Contains consecutive delimiters
            if (Text.Contains(GrhDataCategorization.Delimiter + GrhDataCategorization.Delimiter))
                return false;

            return base.IsValid();
        }
    }
}
