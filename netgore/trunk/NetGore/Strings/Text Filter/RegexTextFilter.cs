using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetGore
{
    /// <summary>
    /// A text filter that uses a <see cref="Regex"/>.
    /// </summary>
    public class RegexTextFilter : TextFilter
    {
        const RegexOptions _regexOptions = RegexOptions.Singleline | RegexOptions.IgnoreCase;

        Regex _filter;

        /// <summary>
        /// When overridden in the derived class, gets the unique name of this <see cref="TextFilter"/> implementation.
        /// This value must be costant for every instance of this filter, and must be unique for the <see cref="Type"/>.
        /// </summary>
        protected override string DisplayNameInternal
        {
            get { return "Regex"; }
        }

        /// <summary>
        /// When overridden in the derived class, creates a deep copy of this filter.
        /// </summary>
        /// <returns>The deep copy of this filter.</returns>
        protected override TextFilter DeepCopyInternal()
        {
            var ret = new RegexTextFilter();

            if (_filter != null)
                ret._filter = new Regex(_filter.ToString(), _regexOptions);

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, checks if the given <paramref name="text"/> passes the filter.
        /// </summary>
        /// <param name="text">The text to test the filter on.</param>
        /// <returns>True if the <paramref name="text"/> passes the filter; otherwise false.</returns>
        protected override bool FilterInternal(string text)
        {
            if (_filter == null)
                return true;

            return _filter.IsMatch(text);
        }

        /// <summary>
        /// When overridden in the derived class, tries to set the filter.
        /// </summary>
        /// <param name="filter">The filter to try to use. This value will never be a null or empty string.</param>
        /// <returns>True if the filter was successfully set; otherwise false.</returns>
        protected override bool TrySetFilterInternal(string filter)
        {
            Regex newRegex;

            try
            {
                newRegex = new Regex(filter, _regexOptions);
            }
            catch (Exception)
            {
                return false;
            }

            _filter = newRegex;
            return true;
        }
    }
}