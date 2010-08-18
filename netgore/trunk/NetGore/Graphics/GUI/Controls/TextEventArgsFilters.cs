using System;
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
        static readonly Func<TextEventArgs, bool> _isDigitFunc;
        static readonly Func<TextEventArgs, bool> _isLetterFunc;
        static readonly Func<TextEventArgs, bool> _isLetterOrDigit;
        static readonly Func<TextEventArgs, bool> _isPunctuation;
        static readonly Func<TextEventArgs, bool> _notIsDigitFunc;
        static readonly Func<TextEventArgs, bool> _notIsLetterFunc;
        static readonly Func<TextEventArgs, bool> _notIsLetterOrDigit;
        static readonly Func<TextEventArgs, bool> _notIsPunctuation;

        /// <summary>
        /// Initializes the <see cref="TextEventArgsFilters"/> class.
        /// </summary>
        static TextEventArgsFilters()
        {
            _isDigitFunc = IsDigit;
            _notIsDigitFunc = (x => !IsDigit(x));

            _isLetterFunc = IsLetter;
            _notIsLetterFunc = (x => !IsLetter(x));

            _isLetterOrDigit = IsLetterOrDigit;
            _notIsLetterOrDigit = (x => !IsLetterOrDigit(x));

            _isPunctuation = IsPunctuation;
            _notIsPunctuation = (x => !IsPunctuation(x));
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
        /// Gets a <see cref="Func{TextEventArgs, Boolean}"/> for the NOT of the <see cref="IsDigit"/> method.
        /// </summary>
        public static Func<TextEventArgs, bool> NotIsDigitFunc
        {
            get { return _notIsDigitFunc; }
        }

        /// <summary>
        /// Gets a <see cref="Func{TextEventArgs, Boolean}"/> for the NOT of the <see cref="IsLetter"/> method.
        /// </summary>
        public static Func<TextEventArgs, bool> NotIsLetterFunc
        {
            get { return _notIsLetterFunc; }
        }

        /// <summary>
        /// Gets a <see cref="Func{TextEventArgs, Boolean}"/> for the NOT of the <see cref="IsLetterOrDigit"/> method.
        /// </summary>
        public static Func<TextEventArgs, bool> NotIsLetterOrDigitFunc
        {
            get { return _notIsLetterOrDigit; }
        }

        /// <summary>
        /// Gets a <see cref="Func{TextEventArgs, Boolean}"/> for the NOT of the <see cref="IsPunctuation"/> method.
        /// </summary>
        public static Func<TextEventArgs, bool> NotIsPunctuationFunc
        {
            get { return _notIsPunctuation; }
        }

        /// <summary>
        /// Gets if the text in the <see cref="TextEventArgs"/> is a digit.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/>.</param>
        /// <returns>True if the <paramref name="e"/> is of the expected type; otherwise false.</returns>
        public static bool IsDigit(TextEventArgs e)
        {
            if (e.Unicode.Length != 1)
                return false;

            return char.IsDigit(e.Unicode[0]);
        }

        /// <summary>
        /// Gets if the text in the <see cref="TextEventArgs"/> is a letter.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/>.</param>
        /// <returns>True if the <paramref name="e"/> is of the expected type; otherwise false.</returns>
        public static bool IsLetter(TextEventArgs e)
        {
            if (e.Unicode.Length != 1)
                return false;

            return char.IsLetter(e.Unicode[0]);
        }

        /// <summary>
        /// Gets if the text in the <see cref="TextEventArgs"/> is a letter or digit.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/>.</param>
        /// <returns>True if the <paramref name="e"/> is of the expected type; otherwise false.</returns>
        public static bool IsLetterOrDigit(TextEventArgs e)
        {
            if (e.Unicode.Length != 1)
                return false;

            return char.IsLetterOrDigit(e.Unicode[0]);
        }

        /// <summary>
        /// Gets if the text in the <see cref="TextEventArgs"/> is a punctuation mark.
        /// </summary>
        /// <param name="e">The <see cref="TextEventArgs"/>.</param>
        /// <returns>True if the <paramref name="e"/> is of the expected type; otherwise false.</returns>
        public static bool IsPunctuation(TextEventArgs e)
        {
            if (e.Unicode.Length != 1)
                return false;

            return char.IsPunctuation(e.Unicode[0]);
        }
    }
}