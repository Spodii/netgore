using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework.Graphics;
using Color=System.Drawing.Color;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Contains helper functions for the Image class.
    /// </summary>
    public static class ImageHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Creates an Image from a Texture2D.
        /// </summary>
        /// <param name="texture">Texture to get the Image from.</param>
        /// <param name="x">Source X.</param>
        /// <param name="y">Source Y.</param>
        /// <param name="width">Source width.</param>
        /// <param name="height">Source height.</param>
        /// <param name="destWidth">Destination width.</param>
        /// <param name="destHeight">Destination height.</param>
        /// <returns>The Image created from the Texture2D.</returns>
        public static Image CreateFromTexture(Texture texture, int x, int y, int width, int height, int destWidth, int destHeight)
        {
            if (texture == null)
                return null;

            // Save the texture to a file
            string filePath = Path.GetTempFileName();
            texture.Save(filePath, ImageFileFormat.Png);
            if (!File.Exists(filePath))
            {
                const string errmsg = "Failed to create temporary file at `{0}`.";
                Debug.Fail(string.Format(errmsg, filePath));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, filePath);
                return null;
            }

            Image ret;

            // Open the texture file we saved
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (Image tmp = Image.FromStream(fileStream))
                    {
                        // Create the bitmap
                        using (Bitmap bmp = new Bitmap(destWidth, destHeight, PixelFormat.Format32bppArgb))
                        {
                            bmp.SetResolution(72, 72);

                            // Create the graphics from the bitmap
                            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
                            {
                                // Clear the canvas with a transparent black
                                g.Clear(Color.FromArgb(0, 0, 0, 0));

                                // Set some smoothing for the scaling
                                g.SmoothingMode = SmoothingMode.HighQuality;
                                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                g.InterpolationMode = InterpolationMode.HighQualityBilinear;

                                // Draw the texture image onto the bitmap
                                Rectangle srcRect = new Rectangle(x, y, width, height);
                                Rectangle destRect = new Rectangle(0, 0, Math.Min(destWidth, width), Math.Min(destHeight, height));
                                g.DrawImage(tmp, destRect, srcRect, GraphicsUnit.Pixel);
                            }

                            // Save the bitmap into a memory stream, then use that stream to load the image
                            // Keep in mind we do not dispose of the bitmapStream since that will be used by the
                            // image we return, so we need it to live
                            MemoryStream bitmapStream = new MemoryStream();
                            bmp.Save(bitmapStream, ImageFormat.Png);
                            ret = Image.FromStream(bitmapStream);
                        }
                    }
                }
            }
            finally
            {
                // Delete the temporary file
                File.Delete(filePath);
            }

            return ret;
        }

        /// <summary>
        /// Creates an Image with a solid color.
        /// </summary>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <param name="color">Fill color.</param>
        /// <returns>Image filled with the specified color.</returns>
        public static Image CreateSolid(int width, int height, Color color)
        {
            Bitmap bmp = new Bitmap(width, height);
            bmp.SetResolution(72, 72);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            g.DrawRectangle(new Pen(color, width * 2), 0, 0, width, height);
            return bmp;
        }
    }
}