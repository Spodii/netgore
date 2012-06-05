using System;
using System.Linq;
using NetGore.IO.PropertySync;

namespace NetGore
{
    /// <summary>
    /// Attribute for a Property that will have its value synchronized by a <see cref="IPropertySync"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SyncValueAttribute : Attribute
    {
        readonly string _customName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncValueAttribute"/> class.
        /// </summary>
        public SyncValueAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncValueAttribute"/> class.
        /// </summary>
        /// <param name="customName">The custom name to be used for when synchronizing.</param>
        public SyncValueAttribute(string customName) : this()
        {
            _customName = customName;
        }

        /// <summary>
        /// Gets the custom name to be used for when synchronizing. Can be null, and is null by default.
        /// When null, the name of the Property will be used instead.
        /// </summary>
        public string CustomName
        {
            get { return _customName; }
        }

        /// <summary>
        /// Gets or sets if this <see cref="SyncValueAttribute"/> will be skipped when synchronizing values from
        /// the Server to the Client or vise versa. Only applicable to network synchronization. Default is true.
        /// </summary>
        public bool SkipNetworkSync { get; set; }
    }
}