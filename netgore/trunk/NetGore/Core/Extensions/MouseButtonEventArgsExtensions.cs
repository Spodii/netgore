using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="MouseButtonEventArgs"/> class.
    /// </summary>
    public static class MouseButtonEventArgsExtensions
    {
        public static Vector2 Location(this MouseButtonEventArgs e)
        {
            return new Vector2(e.X, e.Y);
        }
    }
}