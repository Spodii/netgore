using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// A single simple image that sits in the background of the map.
    /// </summary>
    public class BackgroundLayer : BackgroundImage
    {
        /// <summary>
        /// Gets or sets how the image is drawn on the horizontal axis. Default is Stretched.
        /// </summary>
        [Category("Display")]
        [DisplayName("Horizontal")]
        [Description("How the image is drawn on the horizontal axis.")]
        [DefaultValue(BackgroundLayerLayout.Stretched)]
        [Browsable(true)]
        public BackgroundLayerLayout HorizontalLayout { get; set; }

        /// <summary>
        /// Gets or sets how the image is drawn on the vertical axis. Default is Stretched.
        /// </summary>
        [Category("Display")]
        [DisplayName("Vertical")]
        [Description("How the image is drawn on the vertical axis.")]
        [DefaultValue(BackgroundLayerLayout.Stretched)]
        [Browsable(true)]
        public BackgroundLayerLayout VerticalLayout { get; set; }

        /// <summary>
        /// BackgroundLayer constructor.
        /// </summary>
        public BackgroundLayer()
        {
            // Set the default values
            HorizontalLayout = BackgroundLayerLayout.Stretched;
            VerticalLayout = BackgroundLayerLayout.Stretched;
        }

        public BackgroundLayer(IValueReader reader, int currentTime) : base(reader, currentTime)
        {
            HorizontalLayout = reader.ReadBackgroundLayerLayout("HorizontalLayout");
            VerticalLayout = reader.ReadBackgroundLayerLayout("VerticalLayout");
        }

        /// <summary>
        /// Draws the image to the specified SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw the image to.</param>
        /// <param name="camera">Camera that describes the current view.</param>
        /// <param name="mapSize">Size of the map to draw to.</param>
        public override void Draw(SpriteBatch spriteBatch, Camera2D camera, Vector2 mapSize)
        {
            Vector2 spriteSize = SpriteSourceSize;

            // Adjust the horizontal layout
            switch (HorizontalLayout)
            {
                case BackgroundLayerLayout.Stretched:
                    spriteSize.X = GetStretchedSize(camera.Size.X, mapSize.X, Depth);
                    break;

                case BackgroundLayerLayout.Tiled:
                    throw new NotImplementedException("No support for tiling yet...");
            }

            // Adjust the veritcal layout
            switch (VerticalLayout)
            {
                case BackgroundLayerLayout.Stretched:
                    spriteSize.Y = GetStretchedSize(camera.Size.Y, mapSize.Y, Depth);
                    break;

                case BackgroundLayerLayout.Tiled:
                    throw new NotImplementedException("No support for tiling yet...");
            }

            base.Draw(spriteBatch, camera, mapSize, spriteSize);
        }

        /// <summary>
        /// Gets the size to use for a BackgroundLayer sprite to stretch it across the whole map.
        /// </summary>
        /// <param name="cameraSize">Size of the camera for the given axis.</param>
        /// <param name="targetSize">Target sprite size for the given axis.</param>
        /// <param name="depth">Depth of the BackgroundImage.</param>
        /// <returns>The size to use for a BackgroundLayer sprite to stretch it across the whole map.</returns>
        protected static float GetStretchedSize(float cameraSize, float targetSize, float depth)
        {
            return cameraSize + ((targetSize - cameraSize) / depth);
        }

        public override void Write(IValueWriter writer)
        {
            base.Write(writer);

            writer.Write("HorizontalLayout", HorizontalLayout);
            writer.Write("VerticalLayout", VerticalLayout);
        }
    }
}