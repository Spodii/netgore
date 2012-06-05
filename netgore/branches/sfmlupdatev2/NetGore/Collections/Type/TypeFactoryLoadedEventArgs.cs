using System;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// <see cref="EventArgs"/> for when a <see cref="Type"/> is loaded into the <see cref="TypeFactory"/>.
    /// </summary>
    public class TypeFactoryLoadedEventArgs : EventArgs
    {
        readonly Type _loadedType;
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeFactoryLoadedEventArgs"/> class.
        /// </summary>
        /// <param name="loadedType">the <see cref="Type"/> that was loaded.</param>
        /// <param name="name">The name of the <see cref="Type"/>.</param>
        public TypeFactoryLoadedEventArgs(Type loadedType, string name)
        {
            _loadedType = loadedType;
            _name = name;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> that was loaded.
        /// </summary>
        public Type LoadedType
        {
            get { return _loadedType; }
        }

        /// <summary>
        /// Gets the name of the <see cref="Type"/>.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
    }
}