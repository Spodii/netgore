using System;
using System.Linq;

namespace SFML.Graphics
{
    public static class Vector2Extensions
    {
        public static Vector2 Abs(this Vector2 v)
        {
            return new Vector2(Math.Abs(v.X), Math.Abs(v.Y));
        }
    }
}