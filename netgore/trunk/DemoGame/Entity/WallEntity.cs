using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    public class WallEntity : WallEntityBase
    {
        public WallEntity(Vector2 position, Vector2 size)
            : base(position, size)
        {
        }

        public WallEntity(Vector2 position, Vector2 size, CollisionType collisionType)
            : base(position, size, collisionType)
        {
        }

        public WallEntity(IValueReader r) : base(r)
        {
        }
    }
}
