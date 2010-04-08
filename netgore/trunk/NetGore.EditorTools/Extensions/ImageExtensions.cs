using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Image=SFML.Graphics.Image;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Extension methods for the <see cref="SFML.Graphics.Image"/> class.
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
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="source"/> specifies an area
        /// outside of the <paramref name="image"/>.</exception>
        public static Bitmap ToBitmap(this Image image, Rectangle source, int destWidth, int destHeight)
        {
            using (var bmp = ToBitmap(image, source))
            {
                return new Bitmap(bmp, destHeight, destHeight);
            }
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
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="source"/> specifies an area
        /// outside of the <paramref name="image"/>.</exception>
        public static Bitmap ToBitmap(this Image image, SFML.Graphics.Rectangle source, int destWidth, int destHeight)
        {
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
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="source"/> specifies an area
        /// outside of the <paramref name="image"/>.</exception>
        public static Bitmap ToBitmap(this Image image, SFML.Graphics.Rectangle source)
        {
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
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="source"/> specifies an area
        /// outside of the <paramref name="image"/>.</exception>
        public static unsafe Bitmap ToBitmap(this Image image, Rectangle source)
        {
            if (source.X < 0 || source.Y < 0 || source.Right > image.Width || source.Bottom > image.Height)
                throw new ArgumentOutOfRangeException("source");

            var pixels = image.Pixels;

            // Create the target bitmap
            Bitmap b = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
            
            // Lock the whole bitmap for write only
            var rect = new Rectangle(0, 0, source.Width, source.Height);
            var data = b.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int srcStride = rect.Width;

            try
            {
                // Copy the pixel values byte-by-byte, making sure to copy the RGBA source to the ARGB destination
                for (int y = 0; y < data.Height; y++)
                {
                    var srcOffRow = (y + source.Y) * srcStride * 4;
                    byte* row = (byte*)data.Scan0 + (y * data.Stride);

                    for (int x = 0; x < data.Width; x++)
                    {
                        var srcOff = srcOffRow + ((x + source.X) * 4);
                        var dstOff = x * 4;

                        // ARGB <- RGBA

                        // row[A]
                        row[dstOff + 0] = pixels[srcOff + 3];

                        // row[R]
                        row[dstOff + 1] = pixels[srcOff + 0];

                        // row[G]
                        row[dstOff + 2] = pixels[srcOff + 1];

                        // row[B]
                        row[dstOff + 3] = pixels[srcOff + 2];
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