using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PropertySyncHandlerAttribute : Attribute
    {
        readonly Type _handledType;

        public Type HandledType { get { return _handledType; } }

        public PropertySyncHandlerAttribute(Type handledType)
        {
            _handledType = handledType;
        }
    }
}
