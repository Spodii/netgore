using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoGame.Editor
{
    /// <summary>
    /// <see cref="EventArgs"/> for events from a <see cref="MapDrawFilterHelperCollection"/>.
    /// </summary>
    public class MapDrawFilterHelperCollectionEventArgs : EventArgs
    {
        readonly string _filterName;
        readonly MapDrawFilterHelper _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDrawFilterHelperCollectionEventArgs"/> class.
        /// </summary>
        /// <param name="filter">The name of the filter and the <see cref="MapDrawFilterHelper"/> itself.</param>
        public MapDrawFilterHelperCollectionEventArgs(KeyValuePair<string, MapDrawFilterHelper> filter)
        {
            _filterName = filter.Key;
            _filter = filter.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDrawFilterHelperCollectionEventArgs"/> class.
        /// </summary>
        /// <param name="filterName">The name of the filter.</param>
        /// <param name="filter">The <see cref="MapDrawFilterHelper"/>.</param>
        public MapDrawFilterHelperCollectionEventArgs(string filterName, MapDrawFilterHelper filter)
        {
            _filterName = filterName;
            _filter = filter;
        }

        /// <summary>
        /// Gets the <see cref="FilterName"/> and <see cref="Filter"/> as a <see cref="KeyValuePair{T,U}"/>.
        /// </summary>
        public KeyValuePair<string, MapDrawFilterHelper> FilterNameAndFilter { get { return new KeyValuePair<string, MapDrawFilterHelper>(FilterName, Filter); } }

        /// <summary>
        /// Gets the name of the <see cref="Filter"/>.
        /// </summary>
        public string FilterName { get { return _filterName; } }

        /// <summary>
        /// Gets the <see cref="MapDrawFilterHelper"/>.
        /// </summary>
        public MapDrawFilterHelper Filter { get { return _filter; } }
    }
}