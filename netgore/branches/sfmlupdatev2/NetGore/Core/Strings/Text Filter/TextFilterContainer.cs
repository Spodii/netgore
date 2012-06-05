using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Allows for easier usage of the text filters by abstracting what type of filter is being used and
    /// allowing filters to be set by name. This is the recommended way to using the text filters.
    /// </summary>
    public class TextFilterContainer
    {
        TextFilter _filter = new BasicTextFilter();

        /// <summary>
        /// Notifies listeners when the filter has changed. This can be the result of either the
        /// <see cref="TextFilterContainer.Filter"/> property changing, the filter type changing, or the
        /// filter string changing.
        /// </summary>
        public event TypedEventHandler<TextFilterContainer> FilterChanged;

        /// <summary>
        /// Gets or sets the current <see cref="TextFilter"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public TextFilter Filter
        {
            get { return _filter; }
            set
            {
                if (_filter == value)
                    return;

                if (value == null)
                    throw new ArgumentNullException("value");

                _filter = value;

                if (FilterChanged != null)
                    FilterChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the display name of the current filter.
        /// </summary>
        public string FilterName
        {
            get { return _filter.DisplayName; }
        }

        /// <summary>
        /// Tries to change the filter.
        /// </summary>
        /// <param name="filter">The filter string to change to.</param>
        /// <returns>
        /// True if the filter successfully changed; otherwise false.
        /// </returns>
        public bool TryChangeFilter(string filter)
        {
            return TryChangeFilter(filter, FilterName);
        }

        /// <summary>
        /// Tries to change the filter.
        /// </summary>
        /// <param name="filter">The filter string to change to.</param>
        /// <param name="filterTypeName">The name of the filter to change to.</param>
        /// <returns>
        /// True if the filter successfully changed; otherwise false.
        /// </returns>
        public bool TryChangeFilter(string filter, string filterTypeName)
        {
            var tempFilter = _filter;

            // Change the filter type if we need to
            if (!StringComparer.Ordinal.Equals(filterTypeName, FilterName))
            {
                tempFilter = TextFilter.TryCreateInstance(filterTypeName);
                if (tempFilter == null)
                    return false;
            }
            else
            {
                // Check that the filter string has changed
                if (StringComparer.Ordinal.Equals(filter, tempFilter.FilterString))
                    return true;
            }

            // Change the filter
            if (!tempFilter.TrySetFilter(filter))
                return false;

            _filter = tempFilter;

            if (FilterChanged != null)
                FilterChanged.Raise(this, EventArgs.Empty);

            return true;
        }
    }
}