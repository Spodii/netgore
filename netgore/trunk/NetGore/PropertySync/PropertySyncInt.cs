using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetGore.IO;

namespace NetGore
{
    [PropertySyncHandler(typeof(int))]
    public sealed class PropertySyncInt : PropertySyncBase<int>
    {
        public PropertySyncInt(object bindObject, PropertyInfo p)
            : base(bindObject, p)
        {
        }

        protected override int Read(string name, IValueReader reader)
        {
            return reader.ReadInt(name);
        }

        protected override void Write(string name, IValueWriter writer, int value)
        {
            writer.Write(name, value);
        }
    }
}
