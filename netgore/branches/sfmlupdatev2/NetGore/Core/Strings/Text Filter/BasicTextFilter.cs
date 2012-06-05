using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// A basic text filter that matches literal strings. Multiple strings to match can be separated by a semi-colon.
    /// </summary>
    public class BasicTextFilter : TextFilter
    {
        string[] _filterPieces;

        /// <summary>
        /// When overridden in the derived class, gets the unique name of this <see cref="TextFilter"/> implementation.
        /// This value must be costant for every instance of this filter, and must be unique for the <see cref="Type"/>.
        /// </summary>
        protected override string DisplayNameInternal
        {
            get { return "Text"; }
        }

        /// <summary>
        /// When overridden in the derived class, creates a deep copy of this filter.
        /// </summary>
        /// <returns>The deep copy of this filter.</returns>
        protected override TextFilter DeepCopyInternal()
        {
            var ret = new BasicTextFilter();

            if (_filterPieces != null)
            {
                ret._filterPieces = new string[_filterPieces.Length];
                _filterPieces.CopyTo(ret._filterPieces, 0);
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, checks if the given <paramref name="text"/> passes the filter.
        /// </summary>
        /// <param name="text">The text to test the filter on.</param>
        /// <returns>
        /// True if the <paramref name="text"/> passes the filter; otherwise false.
        /// </returns>
        protected override bool FilterInternal(string text)
        {
            if (_filterPieces == null || _filterPieces.Length == 0)
                return true;

            for (var i = 0; i < _filterPieces.Length; i++)
            {
                if (text.Contains(_filterPieces[i], StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// When overridden in the derived class, tries to set the filter.
        /// </summary>
        /// <param name="filter">The filter to try to use. This value will never be a null or empty string.</param>
        /// <returns>
        /// True if the filter was successfully set; otherwise false.
        /// </returns>
        protected override bool TrySetFilterInternal(string filter)
        {
            _filterPieces = filter.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            return true;
        }
    }
}