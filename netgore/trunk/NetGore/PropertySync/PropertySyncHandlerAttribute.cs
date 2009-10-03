using System;
using System.Linq;
using NetGore;

namespace NetGore
{
    /// <summary>
    /// Attribute that marks a class as a PropertySyncHandler that handles the specified type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PropertySyncHandlerAttribute : Attribute
    {
        readonly Type _handledType;

        /// <summary>
        /// Gets the Property value Type that the class handles.
        /// </summary>
        public Type HandledType
        {
            get { return _handledType; }
        }

        /// <summary>
        /// PropertySyncHandlerAttribute constructor.
        /// </summary>
        /// <param name="handledType">The Property value Type that the class handles.</param>
        public PropertySyncHandlerAttribute(Type handledType)
        {
            _handledType = handledType;
        }
    }
}