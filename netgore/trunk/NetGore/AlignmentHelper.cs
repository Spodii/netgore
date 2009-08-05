using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    public static class AlignmentHelper
    {
        public static Vector2 FindOffset(Alignment alignment, Vector2 itemSize, Vector2 targetSize)
        {
            switch (alignment)
            {
                case Alignment.TopLeft:
                    return new Vector2(0, 0);

                case Alignment.TopRight:
                    return new Vector2(targetSize.X - itemSize.X, 0);

                case Alignment.BottomLeft:
                    return new Vector2(0, targetSize.Y - itemSize.Y);

                case Alignment.BottomRight:
                    return targetSize - itemSize;

                case Alignment.Top:
                    return new Vector2(targetSize.X / 2f - itemSize.X / 2f, 0);

                case Alignment.Bottom:
                    return new Vector2(targetSize.X / 2f - itemSize.X / 2f, targetSize.Y - itemSize.Y);

                case Alignment.Left:
                    return new Vector2(0, targetSize.Y / 2f - itemSize.Y / 2f);

                case Alignment.Right:
                    return new Vector2(targetSize.X - itemSize.X, targetSize.Y / 2f - itemSize.Y / 2f);

                case Alignment.Center:
                    return targetSize / 2f - itemSize / 2f;

                default:
                    throw new ArgumentException("Unknown alignment value specified", "alignment");
            }
        }

        public static Vector2 FindOffset(Alignment alignment, Vector2 itemSize, Rectangle targetRegion)
        {
            Vector2 min = new Vector2(targetRegion.X, targetRegion.Y);
            Vector2 regionSize = new Vector2(targetRegion.Width, targetRegion.Height);
            Vector2 offset = FindOffset(alignment, itemSize, regionSize);

            return offset + min;
        }
    }
}