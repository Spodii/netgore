using System;
using System.ComponentModel;
using System.Linq;
using NetGore.IO;
using NetGore.World;

namespace NetGore.Graphics
{
    // TODO: Add support for BackgroundLayerLayout.Stretched

    /// <summary>
    /// A single simple image that sits in the background of the map.
    /// </summary>
    public class BackgroundLayer : BackgroundImage
    {
        const string _horizontalLayoutKey = "HorizontalLayout";
        const string _verticalLayoutKey = "VerticalLayout";

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
        /// <param name="cameraProvider">The camera provider.</param>
        /// <param name="map">The map that this <see cref="BackgroundImage"/> is on.</param>
        /// <param name="reader">The reader.</param>
        public BackgroundLayer(ICamera2DProvider cameraProvider, IMap map, IValueReader reader)
            : base(cameraProvider, map, reader)
        {
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
        /// Draws the image to the specified <see cref="ISpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch"><see cref="ISpriteBatch"/> to draw the image to.</param>
        public override void Draw(ISpriteBatch spriteBatch)
        {
            var spriteSize = SpriteSourceSize;

            // Adjust the horizontal layout
            switch (HorizontalLayout)
            {
                case BackgroundLayerLayout.Stretched:
                    spriteSize.X = GetStretchedSize(Camera.Size.X * Camera.Scale, Map.Size.X, Depth);
                    break;

                case BackgroundLayerLayout.Tiled:
                    // TODO: Add tiling support
                    throw new NotImplementedException("No support for tiling yet...");
            }

            // Adjust the veritcal layout
            switch (VerticalLayout)
            {
                case BackgroundLayerLayout.Stretched:
                    spriteSize.Y = GetStretchedSize(Camera.Size.Y * Camera.Scale, Map.Size.Y, Depth);
                    break;

                case BackgroundLayerLayout.Tiled:
                    // TODO: Add tiling support
                    throw new NotImplementedException("No support for tiling yet...");
            }

            Draw(spriteBatch, spriteSize);
        }

        /// <summary>
        /// Gets the size to use for a BackgroundLayer sprite to stretch it across the whole map.
        /// </summary>
        /// <param name="normalCameraSize">The unscaled size of the camera for the given axis. This will always make the layer
        /// spread across the whole map exactly only for that zoom level. Zooming in will make it so the whole image cannot be shown
        /// while zooming out will show past the image.</param>
        /// <param name="targetSize">Target sprite size for the given axis.</param>
        /// <param name="depth">Depth of the BackgroundImage.</param>
        /// <returns>The size to use for a BackgroundLayer sprite to stretch it across the whole map.</returns>
        protected static float GetStretchedSize(float normalCameraSize, float targetSize, float depth)
        {
            return normalCameraSize + ((targetSize - normalCameraSize) / depth);
        }

        /// <summary>
        /// Reads the <see cref="BackgroundImage"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        protected override void Read(IValueReader reader)
        {
            base.Read(reader);

            HorizontalLayout = reader.ReadEnum<BackgroundLayerLayout>(_horizontalLayoutKey);
            VerticalLayout = reader.ReadEnum<BackgroundLayerLayout>(_verticalLayoutKey);
        }

        /// <summary>
        /// Writes the <see cref="BackgroundImage"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public override void Write(IValueWriter writer)
        {
            base.Write(writer);

            writer.WriteEnum(_horizontalLayoutKey, HorizontalLayout);
            writer.WriteEnum(_verticalLayoutKey, VerticalLayout);
        }
    }
}