using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    public static class RenderArrow
    {
        public static void Draw(ISpriteBatch sb, Vector2 source, Vector2 dest, Color color)
        {
            var dist = Vector2.Distance(source, dest);
            var angle = GetAngle(source, dest);

            // Get the length of the segments
            var segLen = dist / 6;
            if (segLen < 5.0f)
                segLen = 5.0f;
            if (segLen > 25.0f)
                segLen = 25.0f;

            // Primary line
            var tailLen = dist - (segLen / 2);
            var pDest = source + (new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * tailLen);
            RenderLine.Draw(sb, source, pDest, color);

            // Arrow segment 1
            var ang1 = angle - MathHelper.PiOver4;
            var seg1 = dest - (new Vector2((float)Math.Cos(ang1), (float)Math.Sin(ang1)) * segLen);
            RenderLine.Draw(sb, dest, seg1, color);

            // Arrow segment 2
            var ang2 = angle + MathHelper.PiOver4;
            var seg2 = dest - (new Vector2((float)Math.Cos(ang2), (float)Math.Sin(ang2)) * segLen);
            RenderLine.Draw(sb, dest, seg2, color);

            // Arrow segment 3
            RenderLine.Draw(sb, seg1, seg2, color);
        }

        /// <summary>
        /// Finds the angle (in radians) between two points
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <returns>Angle between the two points, in radians</returns>
        static float GetAngle(Vector2 p1, Vector2 p2)
        {
            return (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }
    }
}