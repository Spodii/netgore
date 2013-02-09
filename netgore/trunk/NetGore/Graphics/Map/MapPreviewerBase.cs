using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using SFML.Graphics;
using Color = SFML.Graphics.Color;
using Image = System.Drawing.Image;
using Point = System.Drawing.Point;

namespace NetGore.Graphics
{
    /// <summary>
    /// Draws all of a <see cref="IDrawableMap"/> to a texture, then saves it to a file.
    /// </summary>
    public abstract class MapPreviewerBase<T> where T : IDrawableMap
    {
        /// <summary>
        /// The <see cref="PixelFormat"/> for the generated <see cref="System.Drawing.Image"/>s. Doesn't use the alpha channel since
        /// we don't really need it.
        /// </summary>
        const PixelFormat _generatedImagePixelFormat = PixelFormat.Format24bppRgb;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPreviewerBase{T}"/> class.
        /// </summary>
        protected MapPreviewerBase()
        {
            TextureSize = new Vector2(1024);
            BackgroundColor = new Color(255, 0, 255);
        }

        /// <summary>
        /// Gets or sets the background color for the generated preview map.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the texture size to use for the generated previews.
        /// </summary>
        public Vector2 TextureSize { get; set; }

        /// <summary>
        /// Creates the preview of a map.
        /// </summary>
        /// <param name="map">The map to create the preview of.</param>
        /// <param name="drawExtensions">The collection of <see cref="IMapDrawingExtension"/>s applied to the map.</param>
        /// <param name="filePath">The file path to save the created preview to.</param>
        public void CreatePreview(T map, ICollection<IMapDrawingExtension> drawExtensions, string filePath)
        {
            using (var tex = CreatePreview(map, drawExtensions))
            {
                SaveTexture(tex, filePath);
            }
        }

        /// <summary>
        /// Creates the preview of a map.
        /// </summary>
        /// <param name="map">The map to create the preview of.</param>
        /// <param name="drawExtensions">The collection of <see cref="IMapDrawingExtension"/>s applied to the map.</param>
        public Image CreatePreview(T map, ICollection<IMapDrawingExtension> drawExtensions)
        {
            // Set up the new camera
            var cam = new Camera2D(TextureSize);

            // Store the existing map values so we can restore them when done
            var oldCamera = map.Camera;
            var oldDrawFilter = map.DrawFilter;
            var oldDrawParticles = map.DrawParticles;
            var oldExtensions = drawExtensions != null ? drawExtensions.ToArray() : new IMapDrawingExtension[0];

            // Set the new values
            SetMapValues(map, cam, DrawFilter, false);

            if (drawExtensions != null)
                drawExtensions.Clear();

            // Create the master image
            var master = new Bitmap((int)map.Width, (int)map.Height, _generatedImagePixelFormat);

            // Create the Graphics instance to draw to the master image
            using (var g = System.Drawing.Graphics.FromImage(master))
            {
                // Create the RenderTarget to draw to
                using (var renderTarget = new RenderTexture((uint)cam.Size.X, (uint)cam.Size.Y))
                {
                    var renderTargetSize = renderTarget.Size;

                    // Create the SpriteBatch
                    using (var sb = new SpriteBatch(renderTarget))
                    {
                        // Loop through as many times as needed to cover the whole map
                        for (var x = 0; x < map.Width; x += (int)renderTargetSize.X)
                        {
                            for (var y = 0; y < map.Height; y += (int)renderTargetSize.Y)
                            {
                                // Clear the target with the background color
                                renderTarget.Clear(BackgroundColor);

                                // Move the camera
                                cam.Min = new Vector2(x, y);

                                // Draw the map
                                sb.Begin(BlendMode.Alpha, cam);
                                map.Draw(sb);
                                sb.End();

                                // Finalize the rendering to the RenderTarget
                                renderTarget.Display();

                                // Grab the segment image and copy it to the master image
                                var sfmlImage = renderTarget.Texture;
                                var segmentImage = sfmlImage.ToBitmap();

                                ImageCopy(g, segmentImage, new Point(x, y));
                            }
                        }
                    }
                }
            }

            // Restore the map values
            SetMapValues(map, oldCamera, oldDrawFilter, oldDrawParticles);

            if (drawExtensions != null)
            {
                foreach (var ex in oldExtensions)
                {
                    drawExtensions.Add(ex);
                }
            }

            return master;
        }

        /// <summary>
        /// The filter used to determine if an <see cref="IDrawable"/> will be drawn when creating the map preview.
        /// </summary>
        /// <param name="drawable">The <see cref="IDrawable"/> to determine if will be drawn.</param>
        /// <returns>True if the <paramref name="drawable"/> will be drawn; otherwise false.</returns>
        protected virtual bool DrawFilter(IDrawable drawable)
        {
            if (drawable is MapGrh) // Always draw MapGrhs
                return true;

            if (drawable.MapRenderLayer != MapRenderLayer.Dynamic) // Draw anything else not on the Dynamic layer
                return true;

            return false;
        }

        /// <summary>
        /// Handles setting up the <see cref="SpriteBatch"/> and drawing the <paramref name="map"/>. This is where
        /// all the actual drawing to the preview map is done.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use for drawing.</param>
        /// <param name="map">The <see cref="IDrawableMap"/> to draw.</param>
        protected virtual void DrawMap(ISpriteBatch sb, IDrawableMap map)
        {
            sb.Begin(BlendMode.Alpha, map.Camera);
            map.Draw(sb);
            sb.End();
        }

        /// <summary>
        /// Copies an image.
        /// </summary>
        /// <param name="g">The <see cref="System.Drawing.Graphics"/> to use to draw to the destination.</param>
        /// <param name="source">The source <see cref="Image"/>.</param>
        /// <param name="offset">The offset to draw the <paramref name="source"/> onto the destination.</param>
        static void ImageCopy(System.Drawing.Graphics g, Image source, Point offset)
        {
            g.DrawImageUnscaled(source, offset);
        }

        /// <summary>
        /// Saves an <see cref="Texture"/> to file.
        /// </summary>
        /// <param name="img">The <see cref="System.Drawing.Image"/> to save.</param>
        /// <param name="filePath">The file path to save the file to.</param>
        protected virtual void SaveTexture(Image img, string filePath)
        {
            // Scale to 10% or 20 if it's a small map
            float scaledPercent = ((float)10 / 100);
            if (img.Height < 1300)
                scaledPercent = ((float)20 / 100);
            var newWidth = (int)(img.Width * scaledPercent).Clamp(1, 2048);
            var newHeight = (int)(img.Height * scaledPercent).Clamp(1, 2048);
            var newImg = img.CreateScaled(newWidth, newHeight, true, null, null);
            newImg.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            newImg.Save(filePath, ImageFormat.Png);
        }

        /// <summary>
        /// When overridden in the derived class, sets the given values on the map.
        /// </summary>
        /// <param name="map">The map to set the values on.</param>
        /// <param name="camera">The camera to use.</param>
        /// <param name="drawFilter">The draw filter to use.</param>
        /// <param name="drawParticles">The draw particles value to use.</param>
        protected abstract void SetMapValues(T map, ICamera2D camera, Func<IDrawable, bool> drawFilter, bool drawParticles);
    }
}