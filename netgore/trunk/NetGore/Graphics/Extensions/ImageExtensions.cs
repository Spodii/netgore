using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extension methods for the <see cref="SFML.Graphics.Image"/> and <see cref="System.Drawing.Image"/> class.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Creates a scaled version of a <see cref="System.Drawing.Image"/>.
        /// </summary>
        /// <param name="original">The original <see cref="System.Drawing.Image"/>.</param>
        /// <param name="destWidth">The target width.</param>
        /// <param name="destHeight">The target height.</param>
        /// <param name="keepAspectRatio">If true, the image will maintain the aspect ratio.</param>
        /// <param name="bgColor">The background color of the new image.</param>
        /// <param name="transparentColor">The color to make transparent in the new image.</param>
        /// <returns>
        /// A <see cref="Bitmap"/> resized to the given values.
        /// </returns>
        public static Bitmap CreateScaled(this Image original, int destWidth, int destHeight, bool keepAspectRatio, Color? bgColor,
                                          Color? transparentColor)
        {
            int w;
            int h;

            if (keepAspectRatio)
            {
                // Get the destination size, maintaining aspect ratio
                var aspectRatio = (float)original.Width / original.Height;
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
            var x = (int)((destWidth - w) / 2f);
            var y = (int)((destHeight - h) / 2f);

            // Create the new bitmap to return
            var ret = new Bitmap(destWidth, destHeight);
            using (var g = System.Drawing.Graphics.FromImage(ret))
            {
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;

                if (bgColor.HasValue)
                    g.Clear(bgColor.Value);

                g.DrawImage(original, x, y, w, h);
            }

            if (transparentColor.HasValue)
                ret.MakeTransparent(transparentColor.Value);

            Debug.Assert(ret.Width == destWidth);
            Debug.Assert(ret.Height == destHeight);

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
        public static Bitmap ToBitmap(this SFML.Graphics.Image image, Rectangle source, int destWidth, int destHeight)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            if (destWidth == source.Width && destHeight == source.Height)
            {
                // The destination is the same size as the source
                return ToBitmap(image, source);
            }
            else
            {
                // The destination is not the same size as the source, so we will have to scale it
                using (var original = ToBitmap(image, source))
                {
                    return original.CreateScaled(destWidth, destHeight, true, null, null);
                }
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
        /// <exception cref="ArgumentNullException"><paramref name="image"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="source"/> specifies an area
        /// outside of the <paramref name="image"/>.</exception>
        public static Bitmap ToBitmap(this SFML.Graphics.Image image, SFML.Graphics.Rectangle source, int destWidth,
                                      int destHeight)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            var rect= new Rectangle(source.X, source.Y, source.Width, source.Height);
            return ToBitmap(image, rect, destWidth, destHeight);
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
        public static Bitmap ToBitmap(this SFML.Graphics.Image image, SFML.Graphics.Rectangle source)
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
        public static unsafe Bitmap ToBitmap(this SFML.Graphics.Image image, Rectangle source)
        {
            const int bytesPerColor = 4;

            if (image == null)
                throw new ArgumentNullException("image");

            if (source.X < 0 || source.Y < 0 || source.Right > image.Width || source.Bottom > image.Height)
                throw new ArgumentOutOfRangeException("source");

            var pixels = image.Pixels;

            // Create the target bitmap
            var b = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);

            // Lock the whole bitmap for write only
            var rect = new Rectangle(0, 0, source.Width, source.Height);
            var data = b.LockBits(rect, ImageLockMode.WriteOnly, b.PixelFormat);

            try
            {
                var srcStride = rect.Width * bytesPerColor;
                var dataStride = data.Stride / bytesPerColor;
                int srcX = source.X;
                int srcY = source.Y;

                // Grab the pointer to the pixels array
                fixed (byte* p = pixels)
                {
                    // Copy the pixel values byte-by-byte, making sure to copy the RGBA source to the ARGB destination
                    for (var y = 0; y < data.Height; y++)
                    {
                        var srcOffColumn = (y + srcY) * srcStride;
                        var row = (int*)data.Scan0 + (y * dataStride);

                        var srcOff = srcOffColumn + (srcX * bytesPerColor);

                        for (var x = 0; x < data.Width; x++)
                        {
                            // Masks for getting the 4 bytes in the int
                            const int b0 = 255;
                            const int b1 = b0 << 8;
                            const int b2 = b0 << 16;
                            const int b3 = b0 << 24;

                            // Get the raw value at the source (pixels[]) as an int (instead of grabbing each byte at a time)
                            int raw = *((int*)(p+srcOff));

                            // Convert to the correct format by moving doing the following to the source bytes:
                            //      src 0 -> dst 2 (move left 2 bytes)
                            //      src 1 -> dst 1 (no moving)
                            //      src 2 -> dst 0 (move right 2 bytes)
                            //      src 3 -> dst 3 (no moving)
                            int converted = (raw & (b3 | b1)) | ((raw & b0) << 16) | ((raw & b2) >> 16);

                            // Store the converted result
                            row[x] = converted;

                            // Move the source over 4 bytes
                            srcOff += bytesPerColor;
                        }
                    }
                }
            }
            finally
            {
                b.UnlockBits(data);
            }

            return b;
        }

        /// <summary>
        /// Creates a <see cref="SFML.Graphics.Image"/> from a <see cref="System.Drawing.Image"/>.
        /// </summary>
        /// <param name="img">The <see cref="System.Drawing.Image"/>.</param>
        /// <returns>The <see cref="SFML.Graphics.Image"/>.</returns>
        public static SFML.Graphics.Image ToSFMLImage(this Image img)
        {
            using (var ms = new MemoryStream((img.Width * img.Height * 4) + 64))
            {
                img.Save(ms, ImageFormat.Bmp);

                return new SFML.Graphics.Image(ms);
            }
        }
    }
}