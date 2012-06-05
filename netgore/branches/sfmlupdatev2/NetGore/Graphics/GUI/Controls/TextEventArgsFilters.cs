using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Provides some simple filters for <see cref="TextEventArgs"/>. Primarily intended to be used with the
    /// <see cref="TextBox.AllowKeysHandler"/> and <see cref="TextBox.IgnoreKeysHandler"/> filters for the
    /// <see cref="TextBox"/> <see cref="Control"/>.
    /// </summary>
    public static class TextEventArgsFilters
    {
        static readonly char[] _commonChars = new char[] { '\b' /* Backspace */ };
        static readonly Func<TextEventArgs, bool> _isDigitFunc;
        static readonly Func<TextEventArgs, bool> _isLetterFunc;
        static readonly Func<TextEventArgs, bool> _isLetterOrDigit;
        static readonly Func<TextEventArgs, bool> _isPunctuation;

        /// <summary>
        /// Initializes the <see cref="TextEventArgsFilters"/> class.
        /// </summary>
        static TextEventArgsFilters()
        {
            _isDigitFunc = IsDigit;
            _isLetterFunc = IsLetter;
            _isLetterOrDigit = IsLetterOrDigit;
            _isPunctuation = IsPunctuation;
        }

        /// <summary>
        /// Gets the characters that are always accepted by the filter methods in this class.
        /// </summary>
        public static IEnumerable<char> CommonChars
        {
            get { return _commonChars; }
        }

        /// <summary>
        /// Gets a <see cref="Func{T,TResult}"/> for the <see cref="IsDigit"/> method.
        /// </summary>
        public static Func<TextEventArgs, bool> IsDigitFunc
        {
            get { return _isDigitFunc; }
        }

        /// <summary>
        /// Gets a <see cref="Func{TextEventArgs, Boolean}"/> for the <see cref="IsLetter"/> method.
        /// </summary>
        public static Func<TextEventArgs, bool> IsLetterFunc
        {
            get { return _isLetterFunc; }
        }

        /// <summary>
        /// Gets a <see cref="Func{TextEventArgs, Boolean}"/> for the <see cref="IsLetterOrDigit"/> method.
        /// </summary>
        public static Func<TextEventArgs, bool> IsLetterOrDigitFunc
        {
            get { return _isLetterOrDigit; }
        }

        /// <summary>
        /// Gets a <see cref="Func{TextEventArgs, Boolean}"/> for the <see cref="IsPunctuation"/> method.
        /// </summary>
        public static Func<TextEventArgs, bool> IsPunctuationFunc
        {
            get { return _isPunctuation; }
        }

        /// <summary>
        /// Gets if the <see cref="TextEventArgs"/> contains one of the <see cref="CommonChars"/>.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/>.</param>
        /// <returns>True if the <paramref name="e"/> contains one of the <see cref="CommonChars"/>; otherwise false.</returns>
        public static bool IsCommonKey(TextEventArgs e)
        {
            if (e.Unicode.Length != 1)
                return false;

            if (_commonChars.Contains(e.Unicode[0]))
                return true;

            return false;
        }

        /// <summary>
        /// Gets if the text in the <see cref="TextEventArgs"/> is a digit.
        /// Common edit-related keys, such as backspace, also return true.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/>.</param>
        /// <returns>True if the <paramref name="e"/> is of the expected type; otherwise false.</returns>
        public static bool IsDigit(TextEventArgs e)
        {
            if (e.Unicode.Length != 1)
                return false;

            return char.IsDigit(e.Unicode[0]) || IsCommonKey(e);
        }

        /// <summary>
        /// Gets if the text in the <see cref="TextEventArgs"/> is a letter.
        /// Common edit-related keys, such as backspace, also return true.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/>.</param>
        /// <returns>True if the <paramref name="e"/> is of the expected type; otherwise false.</returns>
        public static bool IsLetter(TextEventArgs e)
        {
            if (e.Unicode.Length != 1)
                return false;

            return char.IsLetter(e.Unicode[0]) || IsCommonKey(e);
        }

        /// <summary>
        /// Gets if the text in the <see cref="TextEventArgs"/> is a letter or digit.
        /// Common edit-related keys, such as backspace, also return true.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/>.</param>
        /// <returns>True if the <paramref name="e"/> is of the expected type; otherwise false.</returns>
        public static bool IsLetterOrDigit(TextEventArgs e)
        {
            if (e.Unicode.Length != 1)
                return false;

            return char.IsLetterOrDigit(e.Unicode[0]) || IsCommonKey(e);
        }

        /// <summary>
        /// Gets if the text in the <see cref="TextEventArgs"/> is a punctuation mark.
        /// Common edit-related keys, such as backspace, also return true.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/>.</param>
        /// <returns>True if the <paramref name="e"/> is of the expected type; otherwise false.</returns>
        public static bool IsPunctuation(TextEventArgs e)
        {
            if (e.Unicode.Length != 1)
                return false;

            return char.IsPunctuation(e.Unicode[0]) || IsCommonKey(e);
        }
    }
}