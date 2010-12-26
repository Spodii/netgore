using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;

namespace NetGore
{
    /// <summary>
    /// Base class for a filter for a text filter. Instantiable derived classes must include an empty constructor.
    /// All <see cref="TextFilter"/>s implement a case-insensitive filtering.
    /// </summary>
    public abstract class TextFilter
    {
        /// <summary>
        /// The text filter instances with their name as the key.
        /// </summary>
        static readonly Dictionary<string, TextFilter> _filterInstancesByName =
            new Dictionary<string, TextFilter>(StringComparer.Ordinal);

        string _filterString = null;
        bool _passAll = true;

        /// <summary>
        /// Initializes the <see cref="TextFilter"/> class.
        /// </summary>
        static TextFilter()
        {
            new TypeFactory(CreateTypeFilter(), FactoryTypeAdded);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFilter"/> class.
        /// </summary>
        protected TextFilter()
        {
            Invert = false;
        }

        /// <summary>
        /// Gets the display name of the filter.
        /// </summary>
        public string DisplayName
        {
            get { return DisplayNameInternal; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the unique name of this <see cref="TextFilter"/> implementation.
        /// This value must be costant for every instance of this filter, and must be unique for the <see cref="Type"/>.
        /// </summary>
        protected abstract string DisplayNameInternal { get; }

        /// <summary>
        /// Gets the string currently being used as the filter.
        /// </summary>
        public string FilterString
        {
            get { return _filterString; }
        }

        /// <summary>
        /// Gets the name of all the valid text filters.
        /// </summary>
        public static IEnumerable<string> GetFilterNames
        {
            get { return _filterInstancesByName.Keys; }
        }

        /// <summary>
        /// Gets or sets if the filter results are to be inverted. If false, only the items that match the filter
        /// will be returned when filter. If true, only items that do NOT match the filter will be returned. Default
        /// is false.
        /// </summary>
        public bool Invert { get; set; }

        /// <summary>
        /// Gets the filter to use for filtering the types.
        /// </summary>
        /// <returns>The filter to use for filtering the types.</returns>
        static Func<Type, bool> CreateTypeFilter()
        {
            var filterCreator = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                Subclass = typeof(TextFilter),
                ConstructorParameters = Type.EmptyTypes,
                RequireConstructor = true
            };

            return filterCreator.GetFilter();
        }

        /// <summary>
        /// Creates a deep copy of the filter.
        /// </summary>
        /// <returns>A deep copy of the filter.</returns>
        public TextFilter DeepCopy()
        {
            return DeepCopyInternal();
        }

        /// <summary>
        /// When overridden in the derived class, creates a deep copy of this filter.
        /// </summary>
        /// <returns>The deep copy of this filter.</returns>
        protected abstract TextFilter DeepCopyInternal();

        /// <summary>
        /// Handles when a <see cref="TextFilter"/> type is added to the factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="e">The <see cref="NetGore.Collections.TypeFactoryLoadedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="TypeException">Duplicate text filter names found.</exception>
        static void FactoryTypeAdded(TypeFactory factory, TypeFactoryLoadedEventArgs e)
        {
            var instance = (TextFilter)TypeFactory.GetTypeInstance(e.LoadedType);

            var key = instance.DisplayName;

            // Ensure the name is not already in use
            if (_filterInstancesByName.ContainsKey(key))
            {
                const string errmsg = "A text filter with the name `{0}` already exists!";
                throw new TypeException(string.Format(errmsg, key), e.LoadedType);
            }

            // Add the instace
            _filterInstancesByName.Add(key, instance);
        }

        /// <summary>
        /// When overridden in the derived class, checks if the given <paramref name="text"/> passes the filter.
        /// </summary>
        /// <param name="text">The text to test the filter on.</param>
        /// <returns>True if the <paramref name="text"/> passes the filter; otherwise false.</returns>
        protected abstract bool FilterInternal(string text);

        /// <summary>
        /// Gets the items that pass the filter.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="items">The items to pass through the text filter.</param>
        /// <returns>The items that passed the filter.</returns>
        public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
        {
            if (_passAll)
                return items;

            return items.Where(x => FilterInternal(x.ToString()));
        }

        /// <summary>
        /// Gets the items that pass the filter.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="items">The items to pass through the text filter.</param>
        /// <param name="textSelector">The func used to select the string for the corresponding item.
        /// The default method is to just use <see cref="object.ToString"/>.</param>
        /// <returns>The items that passed the filter.</returns>
        public IEnumerable<T> FilterItems<T>(IEnumerable<T> items, Func<T, string> textSelector)
        {
            if (_passAll)
                return items;

            return items.Where(x => FilterInternal(textSelector(x)));
        }

        /// <summary>
        /// Gets the items that pass the filter.
        /// </summary>
        /// <param name="items">The items to pass through the text filter.</param>
        /// <returns>The items that passed the filter.</returns>
        public IEnumerable<string> FilterItems(IEnumerable<string> items)
        {
            if (_passAll)
                return items;

            return items.Where(FilterInternal);
        }

        /// <summary>
        /// Tries to create a <see cref="TextFilter"/> instance by its unique name.
        /// </summary>
        /// <param name="name">The name of the text filter to create the instance of.</param>
        /// <returns>The instance of the filter with the given <paramref name="name"/>, or null if the
        /// <paramref name="name"/> is invalid.</returns>
        public static TextFilter TryCreateInstance(string name)
        {
            TextFilter instance;
            if (_filterInstancesByName.TryGetValue(name, out instance))
                return instance.DeepCopy();

            return null;
        }

        /// <summary>
        /// Tries to set the filter.
        /// </summary>
        /// <param name="filter">The filter string. If null or empty, all items sent to the filter
        /// will pass and this method will return true.</param>
        /// <returns>True if the filter was successfully set; otherwise false.</returns>
        public bool TrySetFilter(string filter)
        {
            // Check for the same filter
            if (StringComparer.OrdinalIgnoreCase.Equals(_filterString, filter))
                return true;

            _filterString = filter;

            // Check for an empty filter
            if (string.IsNullOrEmpty(filter))
            {
                _passAll = true;
                return true;
            }

            _passAll = false;

            // Try to set the filter
            return TrySetFilterInternal(filter);
        }

        /// <summary>
        /// When overridden in the derived class, tries to set the filter.
        /// </summary>
        /// <param name="filter">The filter to try to use. This value will never be a null or empty string.</param>
        /// <returns>True if the filter was successfully set; otherwise false.</returns>
        protected abstract bool TrySetFilterInternal(string filter);
    }
}