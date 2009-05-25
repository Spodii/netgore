using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetGore.IO;

namespace NetGore
{
    [PropertySyncHandler(typeof(byte))]
    public sealed class PropertySyncByte : PropertySyncBase<byte>
    {
        public PropertySyncByte(object bindObject, PropertyInfo p)
            : base(bindObject, p)
        {
        }

        protected override byte Read(string name, IValueReader reader)
        {
            return reader.ReadByte(name);
        }

        protected override void Write(string name, IValueWriter writer, byte value)
        {
            writer.Write(name, value);
        }
    }
}
