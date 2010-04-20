using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extension methods for the <see cref="Color"/> class.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Linearly interpolates between two <see cref="Color"/>s.
        /// </summary>
        /// <param name="first">The first value.</param>
        /// <param name="second">The second value.</param>
        /// <param name="amount">The value between 0.0 and 1.0 indicating the weight of <paramref name="second"/>.</param>
        /// <returns>The linearly interpolated <see cref="Color"/>.</returns>
        public static Color Lerp(this Color first, Color second, float amount)
        {
            var r = (byte)MathHelper.Lerp(first.R, second.R, amount);
            var g = (byte)MathHelper.Lerp(first.G, second.G, amount);
            var b = (byte)MathHelper.Lerp(first.B, second.B, amount);
            var a = (byte)MathHelper.Lerp(first.A, second.A, amount);
            return new Color(r, g, b, a);
        }
    }
}