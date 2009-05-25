using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    [PropertySyncHandler(typeof(CollisionType))]
    public sealed class PropertySyncCollisionType : PropertySyncBase<CollisionType>
    {
        public PropertySyncCollisionType(object bindObject, PropertyInfo p)
            : base(bindObject, p)
        {
        }

        protected override CollisionType Read(string name, IValueReader reader)
        {
            return reader.ReadCollisionType(name);
        }

        protected override void Write(string name, IValueWriter writer, CollisionType value)
        {
            writer.Write(name, value);
        }
    }
}
