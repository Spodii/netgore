using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace SFML.Window
{
    public partial struct Vector2i
    {
        public static implicit operator Vector2(Vector2i v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static implicit operator Vector2i(Vector2 v)
        {
            return new Vector2i((int)v.X, (int)v.Y);
        }
    }

    public partial struct Vector2u
    {
        public static implicit operator Vector2(Vector2u v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static implicit operator Vector2u(Vector2 v)
        {
            return new Vector2u((uint)v.X, (uint)v.Y);
        }
    }
}
