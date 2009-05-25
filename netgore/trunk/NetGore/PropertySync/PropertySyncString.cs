using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetGore.IO;

namespace NetGore
{
    [PropertySyncHandler(typeof(string))]
    public sealed class PropertySyncString : PropertySyncBase<string>
    {
        public PropertySyncString(object bindObject, PropertyInfo p)
            : base(bindObject, p)
        {
        }

        protected override string Read(string name, IValueReader reader)
        {
            return reader.ReadString(name);
        }

        protected override void Write(string name, IValueWriter writer, string value)
        {
            writer.Write(name, value);
        }
    }
}
