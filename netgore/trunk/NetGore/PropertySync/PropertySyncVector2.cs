using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    [PropertySyncHandler(typeof(Vector2))]
    public sealed class PropertySyncVector2 : PropertySyncBase<Vector2>
    {
        public PropertySyncVector2(object bindObject, PropertyInfo p)
            : base(bindObject, p)
        {
        }

        protected override Vector2 Read(string name, IValueReader reader)
        {
            return reader.ReadVector2(name);
        }

        protected override void Write(string name, IValueWriter writer, Vector2 value)
        {
            writer.Write(name, value);
        }
    }
}
