using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetGore
{
    /// <summary>
    /// Contains the rules used to define restrictions on a string.
    /// </summary>
    public class StringRules
    {
        readonly CharType _allowedChars;
        readonly ushort _maxLength;
        readonly ushort _minLength;
        readonly Regex _regex;

        /// <summary>
        /// Gets the set of allowed characters.
        /// </summary>
        public CharType AllowedChars
        {
            get { return _allowedChars; }
        }

        /// <summary>
        /// Gets the maximum string length allowed.
        /// </summary>
        public int MaxLength
        {
            get { return _maxLength; }
        }

        /// <summary>
        /// Gets the minimum string length allowed.
        /// </summary>
        public int MinLength
        {
            get { return _minLength; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringRules"/> class.
        /// </summary>
        /// <param name="minLength">The minimum string length allowed.</param>
        /// <param name="maxLength">The maximum string length allowed.</param>
        /// <param name="allowedChars">The set of allowed characters.</param>
        public StringRules(int minLength, int maxLength, CharType allowedChars) : this(minLength, maxLength, allowedChars, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringRules"/> class.
        /// </summary>
        /// <param name="minLength">The minimum string length allowed.</param>
        /// <param name="maxLength">The maximum string length allowed.</param>
        /// <param name="allowedChars">The set of allowed characters.</param>
        /// <param name="regexOptions">The additional regex options to use.</param>
        public StringRules(int minLength, int maxLength, CharType allowedChars, RegexOptions regexOptions)
        {
            if (minLength < 0)
                throw new ArgumentOutOfRangeException("minLength");
            if (maxLength < minLength)
                throw new ArgumentOutOfRangeException("maxLength");
            if (minLength > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("minLength");
            if (maxLength > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("maxLength");
            if ((int)allowedChars == 0)
                throw new ArgumentException("At least one character group must be allowed.", "allowedChars");

            _minLength = (ushort)minLength;
            _maxLength = (ushort)maxLength;
            _allowedChars = allowedChars;

            string regexStr = BuildRegexString(minLength, maxLength, allowedChars);

            _regex = new Regex(regexStr, RegexOptions.Compiled | regexOptions);
        }

        /// <summary>
        /// Builds the Regex string to use for checking the rules.
        /// </summary>
        /// <param name="minLength">The minimum allowed length.</param>
        /// <param name="maxLength">The maximum allowed length.</param>
        /// <param name="allowedChars">The allowed sets of characters.</param>
        /// <returns>The Regex string to use for checking the rules.</returns>
        static string BuildRegexString(int minLength, int maxLength, CharType allowedChars)
        {
            StringBuilder sb = new StringBuilder();

            // Start of string
            sb.Append("^");

            // Group of allowed characters
            sb.Append("[");

            if ((allowedChars & CharType.AlphaLower) != 0)
                sb.Append("a-z");
            if ((allowedChars & CharType.AlphaUpper) != 0)
                sb.Append("A-Z");
            if ((allowedChars & CharType.Numeric) != 0)
                sb.Append("0-9");
            if ((allowedChars & CharType.Punctuation) != 0)
                sb.Append("\\p{P}");
            if ((allowedChars & CharType.Whitespace) != 0)
                sb.Append("\\s");

            sb.Append("]");

            // Length
            sb.Append("{");
            sb.Append(minLength);
            sb.Append(",");
            sb.Append(maxLength);
            sb.Append("}");

            // End of string
            sb.Append("$");

            return sb.ToString();
        }

        /// <summary>
        /// Tests the given input string to see if it is valid according to the specified rules.
        /// </summary>
        /// <param name="s">The string to test.</param>
        /// <returns>True if <paramref name="s"/> is valid; otherwise false.</returns>
        public bool IsValid(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            if (s.Length < MinLength || s.Length > MaxLength)
                return false;

            return _regex.IsMatch(s);
        }

        /// <summary>
        /// Tests the given input string to see if it is valid according to the specified rules.
        /// </summary>
        /// <param name="s">The string to test.</param>
        /// <param name="startAt">The character position at which to start the checking.</param>
        /// <returns>True if <paramref name="s"/> is valid; otherwise false.</returns>
        public bool IsValid(string s, int startAt)
        {
            return _regex.IsMatch(s, startAt);
        }
    }
}