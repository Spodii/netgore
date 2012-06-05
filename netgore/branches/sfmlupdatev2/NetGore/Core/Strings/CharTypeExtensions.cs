using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="CharType"/> enum.
    /// </summary>
    public static class CharTypeExtensions
    {
        /// <summary>
        /// Gets a <see cref="string"/> of only the characters that are valid for the specified <see cref="CharType"/>.
        /// </summary>
        /// <param name="validChars">The valid chars.</param>
        /// <param name="inStr">The string to remove the characters from that are not valid for the <paramref name="validChars"/>.</param>
        /// <returns>The <paramref name="inStr"/> minus all characters that were not valid for the <paramref name="validChars"/>.</returns>
        public static string GetValidCharsOnly(this CharType validChars, string inStr)
        {
            if (string.IsNullOrEmpty(inStr))
                return inStr;

            var output = inStr;

            for (var i = 0; i < output.Length; i++)
            {
                var c = inStr[i];

                if (!validChars.IsValidChar(c))
                {
                    output = output.Remove(i, 1);
                    i--;
                }
            }

            return output;
        }

        /// <summary>
        /// Checks if a <see cref="char"/> is valid for a given <see cref="CharType"/>.
        /// </summary>
        /// <param name="ct">The <see cref="CharType"/>.</param>
        /// <param name="c">The <see cref="char"/>.</param>
        /// <returns>True if the <paramref name="c"/> is a valid <see cref="char"/> for the <paramref name="ct"/>; otherwise false.</returns>
        public static bool IsValidChar(this CharType ct, char c)
        {
            if (ct == 0)
                return false;

            if (((ct & CharType.AlphaLower) != 0) && char.IsLower(c))
                return true;

            if (((ct & CharType.AlphaUpper) != 0) && char.IsUpper(c))
                return true;

            if (((ct & CharType.Numeric) != 0) && char.IsNumber(c))
                return true;

            if (((ct & CharType.Whitespace) != 0) && char.IsWhiteSpace(c))
                return true;

            if (((ct & CharType.Punctuation) != 0) && char.IsPunctuation(c))
                return true;

            return false;
        }
    }
}