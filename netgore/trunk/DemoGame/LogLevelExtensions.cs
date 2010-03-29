using System.Linq;
using log4net.Core;
using Microsoft.Xna.Framework.Graphics;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="Level"/> class.
    /// </summary>
    public static class LogLevelExtensions
    {
        /// <summary>
        /// Gets the color to use for a log message.
        /// </summary>
        /// <param name="level">The log message level.</param>
        /// <returns>The color to use for the <paramref name="level"/>.</returns>
        public static Color GetColor(this Level level)
        {
            if (level == Level.Debug)
                return Color.DarkGreen;

            if (level == Level.Info)
                return Color.DarkBlue;

            if (level == Level.Warn)
                return Color.DarkViolet;

            if (level == Level.Error)
                return Color.DarkRed;

            if (level == Level.Fatal)
                return Color.Red;

            return Color.Black;
        }
    }
}