using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoGame.Editor
{
    /// <summary>
    /// <see cref="EventArgs"/> for when a filter in a <see cref="MapDrawFilterHelperCollection"/> is renamed.
    /// </summary>
    public class MapDrawFilterHelperCollectionRenameEventArgs : MapDrawFilterHelperCollectionEventArgs
    {
        readonly string _oldName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDrawFilterHelperCollectionRenameEventArgs"/> class.
        /// </summary>
        /// <param name="filterName">The name of the filter.</param>
        /// <param name="filter">The <see cref="MapDrawFilterHelper"/>.</param>
        /// <param name="oldName">The old name of the filter.</param>
        public MapDrawFilterHelperCollectionRenameEventArgs(string filterName, MapDrawFilterHelper filter, string oldName)
            : base(filterName, filter)
        {
            _oldName = oldName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDrawFilterHelperCollectionRenameEventArgs"/> class.
        /// </summary>
        /// <param name="filter">The name of the filter and the <see cref="MapDrawFilterHelper"/> itself.</param>
        /// <param name="oldName">The old name of the filter.</param>
        public MapDrawFilterHelperCollectionRenameEventArgs(KeyValuePair<string, MapDrawFilterHelper> filter, string oldName)
            : base(filter)
        {
            _oldName = oldName;
        }

        /// <summary>
        /// Gets the old name of the filter.
        /// </summary>
        public string OldName
        {
            get { return _oldName; }
        }
    }
}