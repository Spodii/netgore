using System.Drawing;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Editor
{
    /// <summary>
    /// Contains helper functions for the Image class.
    /// </summary>
    public static class ImageHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Creates an Image with a solid color.
        /// </summary>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <param name="color">Fill color.</param>
        /// <returns>Image filled with the specified color.</returns>
        public static Image CreateSolid(int width, int height, Color color)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Creating solid image of size {0}x{1} with color {2}.", width, height, color);

            var bmp = new Bitmap(width, height);
            bmp.SetResolution(72, 72);

            using (var g = System.Drawing.Graphics.FromImage(bmp))
            {
                using (var brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, 0, 0, width, height);
                }
            }

            return bmp;
        }
    }
}