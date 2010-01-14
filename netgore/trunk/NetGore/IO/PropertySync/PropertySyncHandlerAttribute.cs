using System;
using System.Linq;

namespace NetGore.IO.PropertySync
{
    /// <summary>
    /// Attribute that marks a class as a PropertySyncHandler that handles the specified type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PropertySyncHandlerAttribute : Attribute
    {
        readonly Type _handledType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncHandlerAttribute"/> class.
        /// </summary>
        /// <param name="handledType">The Property value Type that the class handles.</param>
        public PropertySyncHandlerAttribute(Type handledType)
        {
            _handledType = handledType;
        }

        /// <summary>
        /// Gets the Property value Type that the class handles.
        /// </summary>
        public Type HandledType
        {
            get { return _handledType; }
        }
    }
}