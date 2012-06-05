using System;
using System.Linq;
using System.Reflection;

namespace NetGore.IO.PropertySync
{
    /// <summary>
    /// Contains the information needed for creating an <see cref="IPropertySync"/>.
    /// </summary>
    public abstract class SyncValueAttributeInfo
    {
        readonly string _name;
        readonly bool _skipNetworkSync;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncValueAttributeInfo"/> class.
        /// </summary>
        /// <param name="pi">The <see cref="PropertyInfo"/>.</param>
        /// <param name="sva">The <see cref="SyncValueAttribute"/>.</param>
        protected internal SyncValueAttributeInfo(PropertyInfo pi, SyncValueAttribute sva)
        {
            _skipNetworkSync = sva.SkipNetworkSync;

            // Find the name to use (CustomName if one supplied, otherwise use the Property's name)
            if (string.IsNullOrEmpty(sva.CustomName))
                _name = pi.Name;
            else
                _name = sva.CustomName;
        }

        /// <summary>
        /// Gets the name to use when synchronizing this property.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the property's Type.
        /// </summary>
        public abstract Type PropertyType { get; }

        /// <summary>
        /// Gets if to skip synchronizing over the network.
        /// </summary>
        public bool SkipNetworkSync
        {
            get { return _skipNetworkSync; }
        }
    }

    /// <summary>
    /// Contains the information needed for creating an <see cref="IPropertySync"/>.
    /// </summary>
    /// <typeparam name="T">The type of the property's value.</typeparam>
    class SyncValueAttributeInfo<T> : SyncValueAttributeInfo
    {
        readonly IPropertyInterface<object, T> _propertyInterface;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncValueAttributeInfo{T}"/> class.
        /// </summary>
        /// <param name="pi">The <see cref="PropertyInfo"/>.</param>
        /// <param name="sva">The <see cref="SyncValueAttribute"/>.</param>
        public SyncValueAttributeInfo(PropertyInfo pi, SyncValueAttribute sva) : base(pi, sva)
        {
            _propertyInterface = PropertyInterface<object, T>.Instance[pi];
        }

        /// <summary>
        /// Gets the IPropertyInterface.
        /// </summary>
        public IPropertyInterface<object, T> PropertyInterface
        {
            get { return _propertyInterface; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the property's Type.
        /// </summary>
        public override Type PropertyType
        {
            get { return typeof(T); }
        }
    }
}