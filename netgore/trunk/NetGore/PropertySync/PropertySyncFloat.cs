using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetGore.IO;

namespace NetGore
{
    [PropertySyncHandler(typeof(float))]
    public sealed class PropertySyncFloat : PropertySyncBase<float>
    {
        public PropertySyncFloat(object bindObject, PropertyInfo p)
            : base(bindObject, p)
        {
        }

        protected override float Read(string name, IValueReader reader)
        {
            return reader.ReadFloat(name);
        }

        protected override void Write(string name, IValueWriter writer, float value)
        {
            writer.Write(name, value);
        }
    }
}
