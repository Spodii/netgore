using System;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Provides helper methods for the <see cref="Alignment"/> enum.
    /// </summary>
    public sealed class AlignmentHelper : EnumIOHelper<Alignment>
    {
        static readonly AlignmentHelper _instance;

        /// <summary>
        /// Initializes the <see cref="AlignmentHelper"/> class.
        /// </summary>
        static AlignmentHelper()
        {
            _instance = new AlignmentHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlignmentHelper"/> class.
        /// </summary>
        AlignmentHelper()
        {
        }

        /// <summary>
        /// Gets the <see cref="AlignmentHelper"/> instance.
        /// </summary>
        public static AlignmentHelper Instance
        {
            get { return _instance; }
        }

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

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="Alignment"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="Alignment"/>.</returns>
        protected override Alignment FromInt(int value)
        {
            return (Alignment)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="Alignment"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected override int ToInt(Alignment value)
        {
            return (int)value;
        }
    }
}