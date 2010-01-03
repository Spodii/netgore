using System;
using System.ComponentModel;
using System.Linq;
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
        static readonly BackgroundLayerLayoutHelper _backgroundLayerLayoutHelper = BackgroundLayerLayoutHelper.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundLayer"/> class.
        /// </summary>
        /// <param name="cameraProvider">The camera provider.</param>
        /// <param name="map">The map that this <see cref="BackgroundImage"/> is on.</param>
        public BackgroundLayer(ICamera2DProvider cameraProvider, IMap map) : base(cameraProvider, map)
        {
            // Set the default values
            HorizontalLayout = BackgroundLayerLayout.Stretched;
            VerticalLayout = BackgroundLayerLayout.Stretched;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundLayer"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="currentTime">The current time.</param>
        /// <param name="cameraProvider">The camera provider.</param>
        /// <param name="map">The map that this <see cref="BackgroundImage"/> is on.</param>
        public BackgroundLayer(ICamera2DProvider cameraProvider, IMap map, IValueReader reader, int currentTime)
            : base(cameraProvider, map,reader, currentTime)
        {
            HorizontalLayout = reader.ReadEnum(_backgroundLayerLayoutHelper, "HorizontalLayout");
            VerticalLayout = reader.ReadEnum(_backgroundLayerLayoutHelper, "VerticalLayout");
        }

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
        /// Draws the image to the specified SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw the image to.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spriteSize = SpriteSourceSize;

            // Adjust the horizontal layout
            switch (HorizontalLayout)
            {
                case BackgroundLayerLayout.Stretched:
                    spriteSize.X = GetStretchedSize(Camera.Size.X, Map.Size.X, Depth);
                    break;

                case BackgroundLayerLayout.Tiled:
                    throw new NotImplementedException("No support for tiling yet...");
            }

            // Adjust the veritcal layout
            switch (VerticalLayout)
            {
                case BackgroundLayerLayout.Stretched:
                    spriteSize.Y = GetStretchedSize(Camera.Size.Y, Map.Size.Y, Depth);
                    break;

                case BackgroundLayerLayout.Tiled:
                    throw new NotImplementedException("No support for tiling yet...");
            }

            Draw(spriteBatch, spriteSize);
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

            writer.WriteEnum(_backgroundLayerLayoutHelper, "HorizontalLayout", HorizontalLayout);
            writer.WriteEnum(_backgroundLayerLayoutHelper, "VerticalLayout", VerticalLayout);
        }
    }
}