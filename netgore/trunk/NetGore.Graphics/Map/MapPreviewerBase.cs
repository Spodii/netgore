using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Draws all of a <see cref="IDrawableMap"/> to a texture, then saves it to a file.
    /// </summary>
    public abstract class MapPreviewerBase<T> where T : IDrawableMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPreviewerBase{T}"/> class.
        /// </summary>
        protected MapPreviewerBase()
        {
            ImageFormat = ImageFileFormat.Png;
            TextureSize = new Vector2(512);
        }

        /// <summary>
        /// Gets or sets the image format to use when creating previews.
        /// </summary>
        public ImageFileFormat ImageFormat { get; set; }

        /// <summary>
        /// Gets or sets the texture size to use for the generated previews.
        /// </summary>
        public Vector2 TextureSize { get; set; }

        /// <summary>
        /// Creates the preview of a map.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="map">The map to create the preview of.</param>
        /// <param name="drawExtensions">The collection of <see cref="IMapDrawingExtension"/>s applied to the map.</param>
        /// <param name="filePath">The file path to save the created preview to.</param>
        public void CreatePreview(GraphicsDevice graphicsDevice, T map, ICollection<IMapDrawingExtension> drawExtensions,
                                  string filePath)
        {
            using (var tex = CreatePreview(graphicsDevice, map, drawExtensions))
            {
                SaveTexture(tex, filePath, ImageFormat);
            }
        }

        /// <summary>
        /// Creates the preview of a map.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="map">The map to create the preview of.</param>
        /// <param name="drawExtensions">The collection of <see cref="IMapDrawingExtension"/>s applied to the map.</param>
        public Texture2D CreatePreview(GraphicsDevice graphicsDevice, T map, ICollection<IMapDrawingExtension> drawExtensions)
        {
            // Set up the new camera
            var cam = new Camera2D(TextureSize);
            var scaleValues = cam.Size / map.Size;
            cam.Scale = Math.Min(scaleValues.X, scaleValues.Y);
            cam.Size = map.Size * cam.Scale;
            cam.Min = Vector2.Zero;

            // Store the existing map values so we can restore them when done
            var oldCamera = map.Camera;
            var oldDrawFilter = map.DrawFilter;
            var oldDrawParticles = map.DrawParticles;
            var oldExtensions = drawExtensions.ToArray();

            // Set the new values
            SetMapValues(map, cam, DrawFilter, false);
            drawExtensions.Clear();

            // Create the SpriteBatch
            Texture2D ret;
            using (var sb = new SpriteBatch(graphicsDevice))
            {
                SurfaceFormat format = graphicsDevice.PresentationParameters.BackBufferFormat;
                MultiSampleType sample = graphicsDevice.PresentationParameters.MultiSampleType;
                int q = graphicsDevice.PresentationParameters.MultiSampleQuality;

                // Store the old graphics device values
                var oldDepthStencilBuffer = graphicsDevice.DepthStencilBuffer;
                var oldRenderTarget = graphicsDevice.GetRenderTarget(0) as RenderTarget2D;

                // Create the target to render to
                using (
                    var target = new RenderTarget2D(graphicsDevice, (int)TextureSize.X, (int)TextureSize.Y, 1, format, sample, q,
                                                    RenderTargetUsage.PreserveContents))
                {
                    // Set the graphics device to render to our target
                    graphicsDevice.DepthStencilBuffer = null;
                    graphicsDevice.SetRenderTarget(0, target);
                    graphicsDevice.Clear(ClearOptions.Target, new Color(255, 0, 255, 255), 1.0f, 0);

                    // Draw
                    sb.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Texture, SaveStateMode.SaveState, cam.Matrix);
                    map.Draw(sb);
                    sb.End();

                    // Restore the render target to the backbuffer
                    graphicsDevice.SetRenderTarget(0, oldRenderTarget);

                    // Get the texture
                    ret = target.GetTexture();
                }

                // Restore the graphics device values
                graphicsDevice.DepthStencilBuffer = oldDepthStencilBuffer;
            }

            // Restore the map values
            SetMapValues(map, oldCamera, oldDrawFilter, oldDrawParticles);

            foreach (var ex in oldExtensions)
            {
                drawExtensions.Add(ex);
            }

            return ret;
        }

        /// <summary>
        /// The filter used to determine if an <see cref="IDrawable"/> will be drawn when creating the map preview.
        /// </summary>
        /// <param name="drawable">The <see cref="IDrawable"/> to determine if will be drawn.</param>
        /// <returns>True if the <paramref name="drawable"/> will be drawn; otherwise false.</returns>
        protected virtual bool DrawFilter(IDrawable drawable)
        {
            if (drawable.MapRenderLayer == MapRenderLayer.SpriteBackground)
                return true;

            if (drawable.MapRenderLayer == MapRenderLayer.SpriteForeground)
                return true;

            return false;
        }

        /// <summary>
        /// Saves a <see cref="Texture2D"/> to file.
        /// </summary>
        /// <param name="texture">The <see cref="Texture2D"/> to save.</param>
        /// <param name="filePath">The file path to save the file to.</param>
        /// <param name="imageFormat">The image format to use for saving.</param>
        protected virtual void SaveTexture(Texture2D texture, string filePath, ImageFileFormat imageFormat)
        {
            texture.Save(filePath, imageFormat);
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