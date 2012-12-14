using System.ComponentModel;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A single simple image that sits in the background of the map.
    /// </summary>
    public class BackgroundLayer : BackgroundImage
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
        /// Gets the size to use for a <see cref="BackgroundLayer"/> sprite to stretch it across the whole map.
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
        /// Handles drawing the <see cref="BackgroundImage"/>.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        protected override void HandleDraw(ISpriteBatch sb)
        {
            var spriteSize = SpriteSourceSize;
            var pos = GetPosition(Map.Size, Camera, spriteSize);

            var vl = VerticalLayout;
            var hl = HorizontalLayout;

            if (hl == BackgroundLayerLayout.Stretched && vl == BackgroundLayerLayout.Stretched)
            {
                // Stretch both directions
                spriteSize.X = GetStretchedSize(Camera.Size.X * Camera.Scale, Map.Size.X, Depth);
                spriteSize.Y = GetStretchedSize(Camera.Size.Y * Camera.Scale, Map.Size.Y, Depth);

                var rect = new Rectangle(pos.X, pos.Y, spriteSize.X, spriteSize.Y);
                Sprite.Draw(sb, rect, Color);
            }
            else if (vl == BackgroundLayerLayout.Tiled &&
                     (hl == BackgroundLayerLayout.None || hl == BackgroundLayerLayout.Stretched))
            {
                // Stretch/none horizontal, tile vertical
                int drawWidth;
                if (hl == BackgroundLayerLayout.None)
                    drawWidth = (int)spriteSize.X;
                else
                    drawWidth = (int)GetStretchedSize(Camera.Size.X * Camera.Scale, Map.Size.X, Depth);

                sb.DrawTiledY((int)pos.Y, (int)Camera.Max.Y + 64, (int)pos.X, Sprite, Color.White, drawWidth);
            }
            else if (hl == BackgroundLayerLayout.Tiled &&
                     (vl == BackgroundLayerLayout.None || vl == BackgroundLayerLayout.Stretched))
            {
                // Tile horizontal, stretch/none vertical
                int drawHeight;
                if (vl == BackgroundLayerLayout.None)
                    drawHeight = (int)spriteSize.Y;
                else
                    drawHeight = (int)GetStretchedSize(Camera.Size.X * Camera.Scale, Map.Size.X, Depth);

                sb.DrawTiledX((int)pos.X, (int)Camera.Max.X + 64, (int)pos.Y, Sprite, Color.White, drawHeight);
            }
            else if (hl == BackgroundLayerLayout.Tiled && vl == BackgroundLayerLayout.Tiled)
            {
                // Tile both directions
                sb.DrawTiledXY((int)pos.X, (int)Camera.Max.X + 64, (int)pos.Y, (int)Camera.Max.Y + 64, Sprite, Color);
            }
            else if (hl == BackgroundLayerLayout.None && vl == BackgroundLayerLayout.None)
            {
                // None in both directions
                Sprite.Draw(sb, pos);
            }
            else
            {
                // Unknown or unhandled permutation
                const string errmsg =
                    "Unknown or unhandled BackgroundLayerLayout provided." +
                    " Changing layouts to BackgroundLayerLayout.None. HorizontalLayout: `{0}` VerticalLayout: `{1}`";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, HorizontalLayout, VerticalLayout);

                HorizontalLayout = BackgroundLayerLayout.None;
                VerticalLayout = BackgroundLayerLayout.None;
            }
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