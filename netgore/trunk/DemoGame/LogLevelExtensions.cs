using System.Linq;
using log4net.Core;
using SFML.Graphics;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="Level"/> class.
    /// </summary>
    public static class LogLevelExtensions
    {
        static readonly Color _colorDebug = new Color(0, 100, 0, 255);
        static readonly Color _colorError = new Color(0, 100, 0, 255);
        static readonly Color _colorFatal = new Color(255, 0, 0, 255);
        static readonly Color _colorInfo = new Color(0, 0, 100, 255);
        static readonly Color _colorWarn = new Color(100, 0, 100, 255);

        /// <summary>
        /// Gets the color to use for a log message.
        /// </summary>
        /// <param name="level">The log message level.</param>
        /// <returns>The color to use for the <paramref name="level"/>.</returns>
        public static Color GetColor(this Level level)
        {
            if (level == Level.Debug)
                return _colorDebug;

            if (level == Level.Info)
                return _colorInfo;

            if (level == Level.Warn)
                return _colorWarn;

            if (level == Level.Error)
                return _colorError;

            if (level == Level.Fatal)
                return _colorFatal;

            return Color.Black;
        }
    }
}