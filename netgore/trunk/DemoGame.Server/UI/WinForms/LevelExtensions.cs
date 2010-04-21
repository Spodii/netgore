using System.Drawing;
using System.Linq;
using log4net.Core;
using NetGore.Collections;

namespace DemoGame.Server.UI
{
    /// <summary>
    /// Extension methods for the <see cref="Level"/> class.
    /// </summary>
    public static class LevelExtensions
    {
        static readonly HashCache<Level, Brush> _brushes = new HashCache<Level, Brush>(x => new SolidBrush(x.GetSystemColor()));

        /// <summary>
        /// Gets the color to use for a log message.
        /// </summary>
        /// <param name="level">The log message level.</param>
        /// <returns>The color to use for the <paramref name="level"/>.</returns>
        public static Color GetSystemColor(this Level level)
        {
            var c = level.GetColor();
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// Gets the brush to use for a log message.
        /// </summary>
        /// <param name="level">The log message level.</param>
        /// <returns>The brush to use for the <paramref name="level"/>.</returns>
        public static Brush GetSystemColorBrush(this Level level)
        {
            return _brushes[level];
        }
    }
}