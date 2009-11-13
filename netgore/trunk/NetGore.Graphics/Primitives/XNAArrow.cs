using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;

namespace NetGore.Graphics
{
    public static class XNAArrow
    {
        public static void Draw(SpriteBatch sb, Vector2 source, Vector2 dest, Color color)
        {
            float dist = Vector2.Distance(source, dest);
            float angle = GetAngle(source, dest);

            // Get the length of the segments
            float segLen = dist / 6;
            if (segLen < 5.0f)
                segLen = 5.0f;
            if (segLen > 25.0f)
                segLen = 25.0f;

            // Primary line
            float tailLen = dist - (segLen / 2);
            Vector2 pDest = source + (new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * tailLen);
            XNALine.Draw(sb, source, pDest, color);

            // Arrow segment 1
            float ang1 = angle - MathHelper.PiOver4;
            Vector2 seg1 = dest - (new Vector2((float)Math.Cos(ang1), (float)Math.Sin(ang1)) * segLen);
            XNALine.Draw(sb, dest, seg1, color);

            // Arrow segment 2
            float ang2 = angle + MathHelper.PiOver4;
            Vector2 seg2 = dest - (new Vector2((float)Math.Cos(ang2), (float)Math.Sin(ang2)) * segLen);
            XNALine.Draw(sb, dest, seg2, color);

            // Arrow segment 3
            XNALine.Draw(sb, seg1, seg2, color);
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