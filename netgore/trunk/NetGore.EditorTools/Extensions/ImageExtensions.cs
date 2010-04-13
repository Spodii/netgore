using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Image=SFML.Graphics.Image;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Extension methods for the <see cref="SFML.Graphics.Image"/> and <see cref="System.Drawing.Image"/> class.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Creates an <see cref="SFML.Graphics.Image"/> from an array of bytes. The color is assumed to be in R8G8B8A8 format.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="source">The <see cref="Rectangle"/> to copy on the source image.</param>
        /// <param name="destWidth">The width of the generated image.</param>
        /// <param name="destHeight">The height of the generated image.</param>
        /// <returns>
        /// The <see cref="Bitmap"/> containing the <paramref name="image"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="image"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="source"/> specifies an area
        /// outside of the <paramref name="image"/>.</exception>
        public static Bitmap ToBitmap(this Image image, Rectangle source, int destWidth, int destHeight)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            using (var original = ToBitmap(image, source))
            {
                return original.CreateScaled(destWidth, destHeight, true);
            }
        }

        /// <summary>
        /// Creates a scaled version of a <see cref="System.Drawing.Image"/>.
        /// </summary>
        /// <param name="original">The original <see cref="System.Drawing.Image"/>.</param>
        /// <param name="destWidth">The target width.</param>
        /// <param name="destHeight">The target height.</param>
        /// <param name="keepAspectRatio">If true, the image will maintain the aspect ratio.</param>
        /// <returns>A <see cref="Bitmap"/> resized to the given values.</returns>
        public static Bitmap CreateScaled(this System.Drawing.Image original, int destWidth, int destHeight, bool keepAspectRatio)
        {
            var ret = new Bitmap(destWidth, destHeight);

            using (var g = System.Drawing.Graphics.FromImage(ret))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                int w;
                int h;

                if (keepAspectRatio)
                {
                    // Get the destination size, maintaining aspect ratio
                    float aspectRatio = (float)original.Width / original.Height;
                    if (aspectRatio > 1)
                    {
                        w = destWidth;
                        h = (int)(destHeight * (1f / aspectRatio));

                        if (h <= 4 && destHeight >= 4)
                            h = 4;
                    }
                    else
                    {
                        w = (int)(destWidth * aspectRatio);
                        h = destHeight;

                        if (w <= 4 && destWidth >= 4)
                            w = 4;
                    }
                }
                else
                {
                    // Don't maintain aspect ratio
                    w = destWidth;
                    h = destHeight;
                }

                Debug.Assert(w <= destWidth);
                Debug.Assert(h <= destHeight);

                // Center
                int x = (int)((destWidth - w) / 2f);
                int y = (int)((destHeight - h) / 2f);

                g.DrawImage(original, x, y, w, h);
            }

            return ret;
        }

        /// <summary>
        /// Creates an <see cref="SFML.Graphics.Image"/> from an array of bytes. The color is assumed to be in R8G8B8A8 format.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="source">The <see cref="Rectangle"/> to copy on the source image.</param>
        /// <param name="destWidth">The width of the generated image.</param>
        /// <param name="destHeight">The height of the generated image.</param>
        /// <returns>
        /// The <see cref="Bitmap"/> containing the <paramref name="image"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="image"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="source"/> specifies an area
        /// outside of the <paramref name="image"/>.</exception>
        public static Bitmap ToBitmap(this Image image, SFML.Graphics.Rectangle source, int destWidth, int destHeight)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            return ToBitmap(image, new Rectangle(source.X, source.Y, source.Width, source.Height), destWidth, destHeight);
        }

        /// <summary>
        /// Creates an <see cref="SFML.Graphics.Image"/> from an array of bytes. The color is assumed to be in R8G8B8A8 format.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="source">The <see cref="Rectangle"/> to copy on the source image.</param>
        /// <returns>
        /// The <see cref="Bitmap"/> containing the <paramref name="image"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="image"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="source"/> specifies an area
        /// outside of the <paramref name="image"/>.</exception>
        public static Bitmap ToBitmap(this Image image, SFML.Graphics.Rectangle source)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            return ToBitmap(image, new Rectangle(source.X, source.Y, source.Width, source.Height));
        }

        /// <summary>
        /// Creates an <see cref="SFML.Graphics.Image"/> from an array of bytes. The color is assumed to be in R8G8B8A8 format.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="source">The <see cref="Rectangle"/> to copy on the source image.</param>
        /// <returns>
        /// The <see cref="Bitmap"/> containing the <paramref name="image"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="image"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="source"/> specifies an area
        /// outside of the <paramref name="image"/>.</exception>
        public static unsafe Bitmap ToBitmap(this Image image, Rectangle source)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            if (source.X < 0 || source.Y < 0 || source.Right > image.Width || source.Bottom > image.Height)
                throw new ArgumentOutOfRangeException("source");

            var pixels = image.Pixels;

            // Create the target bitmap
            Bitmap b = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
            
            // Lock the whole bitmap for write only
            var rect = new Rectangle(0, 0, source.Width, source.Height);
            var data = b.LockBits(rect, ImageLockMode.WriteOnly, b.PixelFormat);

            const int bytesPerColor = 4;

            int srcStride = rect.Width;

            try
            {
                // Copy the pixel values byte-by-byte, making sure to copy the RGBA source to the ARGB destination
                for (int y = 0; y < data.Height; y++)
                {
                    var srcOffRow = (y + source.Y) * srcStride * bytesPerColor;
                    byte* row = (byte*)data.Scan0 + (y * data.Stride);

                    for (int x = 0; x < data.Width; x++)
                    {
                        var srcOff = srcOffRow + ((x + source.X) * bytesPerColor);
                        var dstOff = x * bytesPerColor;

                        // row[A]
                        row[dstOff + 3] = pixels[srcOff + 3];

                        // row[R]
                        row[dstOff + 2] = pixels[srcOff + 0];

                        // row[G]
                        row[dstOff + 1] = pixels[srcOff + 1];

                        // row[B]
                        row[dstOff + 0] = pixels[srcOff + 2];
                    }
                }
            }
            finally
            {
                b.UnlockBits(data);
            }

            return b;
        }
    }
}