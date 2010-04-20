// TODO: [SFML2] Render targets are supported in SFML 2.0. Until then, this will not be able to work properly.

using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

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
            TextureSize = new Vector2(2048);
            BackgroundColor = new Color(255, 0, 255, 255);
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
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="map">The map to create the preview of.</param>
        /// <param name="drawExtensions">The collection of <see cref="IMapDrawingExtension"/>s applied to the map.</param>
        /// <param name="filePath">The file path to save the created preview to.</param>
        public void CreatePreview(RenderWindow rw, T map, ICollection<IMapDrawingExtension> drawExtensions, string filePath)
        {
            using (var tex = CreatePreview(rw, map, drawExtensions))
            {
                SaveTexture(tex, filePath);
            }
        }

        /// <summary>
        /// Creates the preview of a map.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="map">The map to create the preview of.</param>
        /// <param name="drawExtensions">The collection of <see cref="IMapDrawingExtension"/>s applied to the map.</param>
        public Image CreatePreview(RenderWindow rw, T map, ICollection<IMapDrawingExtension> drawExtensions)
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
            // NOTE: Render the map to the render target here. Below is the old code used for XNA.
            //var ret = RenderTarget2DHelper.CreateTexture2D(graphicsDevice, (int)TextureSize.X, (int)TextureSize.Y,
            //                                                     BackgroundColor, x => DrawMap(x, map));

            // NOTE: Create a temp image until we have this working again
            var ret = new Image(128, 128, Color.Black);

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
        /// Saves an <see cref="Image"/> to file.
        /// </summary>
        /// <param name="texture">The <see cref="Image"/> to save.</param>
        /// <param name="filePath">The file path to save the file to.</param>
        protected virtual void SaveTexture(Image texture, string filePath)
        {
            texture.SaveToFile(filePath);
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