using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    [PropertySyncHandler(typeof(ushort))]
    public sealed class PropertySyncUShort : PropertySyncBase<ushort>
    {
        public PropertySyncUShort(object bindObject, PropertyInfo p)
            : base(bindObject, p)
        {
        }

        protected override ushort Read(string name, IValueReader reader)
        {
            return reader.ReadUShort(name);
        }

        protected override void Write(string name, IValueWriter writer, ushort value)
        {
            writer.Write(name, value);
        }
    }
}
