using System;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="GameMessage"/> class.
    /// </summary>
    public static class GameMessageCollectionExtensions
    {
        /// <summary>
        /// Gets a message from a <see cref="GameMessage"/> packed into a string using <see cref="GameMessageHelper.AsString"/>.
        /// </summary>
        /// <param name="coll">The <see cref="GameMessageCollection"/>.</param>
        /// <param name="s">The <see cref="GameMessage"/> and arguments packed into a string.</param>
        /// <returns>The parsed <see cref="GameMessage"/>.</returns>
        public static string GetMessageFromString(this GameMessageCollection coll, string s)
        {
            var kvp = GameMessageHelper.FromString(s);
            return coll.GetMessage(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Gets a message from a <see cref="GameMessage"/> packed into a string using <see cref="GameMessageHelper.AsString"/>.
        /// </summary>
        /// <param name="coll">The <see cref="GameMessageCollection"/>.</param>
        /// <param name="s">The <see cref="GameMessage"/> and arguments packed into a string.</param>
        /// <returns>The parsed <see cref="GameMessage"/>, or null if the parsing failed.</returns>
        public static string TryGetMessageFromString(this GameMessageCollection coll, string s)
        {
            try
            {
                var kvp = GameMessageHelper.FromString(s);
                return coll.GetMessage(kvp.Key, kvp.Value);
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}