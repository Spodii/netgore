using System;
using System.Linq;
using System.Text;
using NetGore.Content;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// An implementation of <see cref="ISpriteBatch"/> using the <see cref="RenderWindow"/>.
    /// </summary>
    public class SpriteBatch : ISpriteBatch
    {
        readonly RenderWindow _rw;
        readonly SFML.Graphics.Sprite _sprite = new SFML.Graphics.Sprite();
        readonly String2D _str = new String2D();

        bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatch"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        public SpriteBatch(RenderWindow graphicsDevice)
        {
            _rw = graphicsDevice;
        }

        /// <summary>
        /// Gets the graphics device associated with this sprite batch.
        /// </summary>
        public RenderWindow GraphicsDevice
        {
            get { return _rw; }
        }

        /// <summary>
        /// Checks if an asset is in a valid state to be used.
        /// </summary>
        /// <param name="asset">The asset to check if valid.</param>
        /// <returns>True if the <paramref name="asset"/> is in a valid state to be used; otherwise false.</returns>
        protected static bool IsAssetValid(Image asset)
        {
            if (asset == null || asset.IsDisposed())
                return false;

            return true;
        }

        /// <summary>
        /// Checks if an asset is in a valid state to be used.
        /// </summary>
        /// <param name="asset">The asset to check if valid.</param>
        /// <returns>True if the <paramref name="asset"/> is in a valid state to be used; otherwise false.</returns>
        protected static bool IsAssetValid(Font asset)
        {
            if (asset == null || asset.IsDisposed())
                return false;

            return true;
        }

        #region ISpriteBatch Members

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets or sets the name of this sprite batch.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets an object that uniquely identifies this sprite batch.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="halfSize">The half-size of the view area.</param>
        /// <param name="center">The position of the center of the view.</param>
        public void Begin(BlendMode blendMode, Vector2 halfSize, Vector2 center)
        {
            _rw.CurrentView.HalfSize = halfSize;
            _rw.CurrentView.Center = center;

            _sprite.BlendMode = blendMode;

            ContentManager.DoNotUnload = true;
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the world.</param>
        public virtual void Begin(BlendMode blendMode, ICamera2D camera)
        {
            var v = camera.Size / 2f;
            Begin(blendMode, v, v + camera.Min);
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        public virtual void Begin(BlendMode blendMode)
        {
            var v = new Vector2(_rw.Width, _rw.Height) / 2f;
            _rw.CurrentView.HalfSize = v;
            _rw.CurrentView.Center = v;

            _sprite.BlendMode = blendMode;

            ContentManager.DoNotUnload = true;
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        public virtual void Begin()
        {
            Begin(BlendMode.Alpha);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            _isDisposed = true;
        }

        /// <summary>
        /// Draws a raw <see cref="SFML.Graphics.Sprite"/>. Recommended to avoid using when possible, but when needed,
        /// can provide a slight performance boost when drawing a large number of sprites with identical state.
        /// </summary>
        /// <param name="sprite">The <see cref="SFML.Graphics.Sprite"/> to draw.</param>
        public virtual void Draw(SFML.Graphics.Sprite sprite)
        {
            if (sprite == null || !IsAssetValid(sprite.Image))
                return;

            _rw.Draw(sprite);
        }

        /// <summary>
        /// Adds a sprite to the batch of sprites to be rendered, specifying the texture, destination, and source rectangles,
        /// color tint, rotation, origin, effects, and sort depth.
        /// </summary>
        /// <param name="texture">The sprite texture.</param>
        /// <param name="destinationRectangle">A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        /// If this rectangle is not the same size as sourceRectangle, the sprite is scaled to fit.</param>
        /// <param name="sourceRectangle">A rectangle specifying, in texels, which section of the rectangle to draw.
        /// Use null to draw the entire texture.</param>
        /// <param name="color">The color channel modulation to use. Use <see cref="Color.White"/> for full color with
        /// no tinting.</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin.</param>
        /// <param name="origin">The origin of the sprite. Specify (0,0) for the upper-left corner.</param>
        /// <param name="effects">Rotations to apply prior to rendering.</param>
        public virtual void Draw(Image texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color,
                                 float rotation, Vector2 origin, SpriteEffects effects)
        {
            if (!IsAssetValid(texture))
                return;

            _sprite.Image = texture;
            _sprite.Position = new Vector2(destinationRectangle.X, destinationRectangle.Y);
            _sprite.SubRect = sourceRectangle.HasValue
                                  ? (IntRect)sourceRectangle
                                  : new IntRect(0, 0, (int)_sprite.Image.Width, (int)_sprite.Image.Height);
            _sprite.Color = color;
            _sprite.Rotation = -MathHelper.ToDegrees(rotation);
            _sprite.Center = origin;
            _sprite.FlipX((effects & SpriteEffects.FlipHorizontally) != 0);
            _sprite.FlipY((effects & SpriteEffects.FlipVertically) != 0);
            _sprite.Scale = new Vector2((float)destinationRectangle.Width / _sprite.SubRect.Width,
                                        (float)destinationRectangle.Height / _sprite.SubRect.Height);
       
            _rw.Draw(_sprite);
        }

        /// <summary>
        /// Adds a sprite to the batch of sprites to be rendered, specifying the texture, destination, and source rectangles,
        /// color tint, rotation, origin, effects, and sort depth.
        /// </summary>
        /// <param name="texture">The sprite texture.</param>
        /// <param name="destinationRectangle">A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        /// If this rectangle is not the same size as sourceRectangle, the sprite is scaled to fit.</param>
        /// <param name="sourceRectangle">A rectangle specifying, in texels, which section of the rectangle to draw.
        /// Use null to draw the entire texture.</param>
        /// <param name="color">The color channel modulation to use. Use <see cref="Color.White"/> for full color with
        /// no tinting.</param>
        public virtual void Draw(Image texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            Draw(texture, destinationRectangle, sourceRectangle, color, 0f, Vector2.Zero, SpriteEffects.None);
        }

        /// <summary>
        /// Adds a sprite to the batch of sprites to be rendered, specifying the texture, destination, and source rectangles,
        /// color tint, rotation, origin, effects, and sort depth.
        /// </summary>
        /// <param name="texture">The sprite texture.</param>
        /// <param name="destinationRectangle">A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        /// If this rectangle is not the same size as sourceRectangle, the sprite is scaled to fit.</param>
        /// <param name="color">The color channel modulation to use. Use <see cref="Color.White"/> for full color with
        /// no tinting.</param>
        public virtual void Draw(Image texture, Rectangle destinationRectangle, Color color)
        {
            Draw(texture, destinationRectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None);
        }

        /// <summary>
        /// Adds a sprite to the batch of sprites to be rendered, specifying the texture, destination, and source rectangles,
        /// color tint, rotation, origin, effects, and sort depth.
        /// </summary>
        /// <param name="texture">The sprite texture.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        /// <param name="sourceRectangle">A rectangle specifying, in texels, which section of the rectangle to draw.
        /// Use null to draw the entire texture.</param>
        /// <param name="color">The color channel modulation to use. Use <see cref="Color.White"/> for full color with
        /// no tinting.</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin.</param>
        /// <param name="origin">The origin of the sprite. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Vector containing separate scalar multiples for the x- and y-axes of the sprite.</param>
        /// <param name="effects">Rotations to apply prior to rendering.</param>
        public virtual void Draw(Image texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                 Vector2 origin, Vector2 scale, SpriteEffects effects)
        {
            if (!IsAssetValid(texture))
                return;

            _sprite.Image = texture;
            _sprite.Position = position;
            _sprite.SubRect = sourceRectangle.HasValue
                                  ? (IntRect)sourceRectangle
                                  : new IntRect(0, 0, (int)_sprite.Image.Width, (int)_sprite.Image.Height);
            _sprite.Color = color;
            _sprite.Rotation = -MathHelper.ToDegrees(rotation);
            _sprite.Center = origin;
            _sprite.FlipX((effects & SpriteEffects.FlipHorizontally) != 0);
            _sprite.FlipY((effects & SpriteEffects.FlipVertically) != 0);
            _sprite.Scale = scale;

            _rw.Draw(_sprite);
        }

        /// <summary>
        /// Adds a sprite to the batch of sprites to be rendered, specifying the texture, destination, and source rectangles,
        /// color tint, rotation, origin, effects, and sort depth.
        /// </summary>
        /// <param name="texture">The sprite texture.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        /// <param name="sourceRectangle">A rectangle specifying, in texels, which section of the rectangle to draw.
        /// Use null to draw the entire texture.</param>
        /// <param name="color">The color channel modulation to use. Use <see cref="Color.White"/> for full color with
        /// no tinting.</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin.</param>
        /// <param name="origin">The origin of the sprite. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Uniform multiple by which to scale the sprite width and height.</param>
        /// <param name="effects">Rotations to apply prior to rendering.</param>
        public virtual void Draw(Image texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                 Vector2 origin, float scale, SpriteEffects effects)
        {
            Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale), effects);
        }

        /// <summary>
        /// Adds a sprite to the batch of sprites to be rendered, specifying the texture, destination, and source rectangles,
        /// color tint, rotation, origin, effects, and sort depth.
        /// </summary>
        /// <param name="texture">The sprite texture.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        /// <param name="sourceRectangle">A rectangle specifying, in texels, which section of the rectangle to draw.
        /// Use null to draw the entire texture.</param>
        /// <param name="color">The color channel modulation to use. Use <see cref="Color.White"/> for full color with
        /// no tinting.</param>
        public virtual void Draw(Image texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            Draw(texture, position, sourceRectangle, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None);
        }

        /// <summary>
        /// Adds a sprite to the batch of sprites to be rendered, specifying the texture, destination, and source rectangles,
        /// color tint, rotation, origin, effects, and sort depth.
        /// </summary>
        /// <param name="texture">The sprite texture.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        /// <param name="color">The color channel modulation to use. Use <see cref="Color.White"/> for full color with
        /// no tinting.</param>
        public virtual void Draw(Image texture, Vector2 position, Color color)
        {
            Draw(texture, position, null, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="spriteFont">The sprite font.</param>
        /// <param name="text">The mutable (read/write) string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        /// <param name="rotation">The angle, in radians, to rotate the text around the origin.</param>
        /// <param name="origin">The origin of the string. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Vector containing separate scalar multiples for the x- and y-axes of the sprite.</param>
        /// <param name="style">How to style the drawn string.</param>
        public virtual void DrawString(Font spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, Vector2 scale, String2D.Styles style)
        {
            if (!IsAssetValid(spriteFont))
                return;

            DrawString(spriteFont, text.ToString(), position, color, rotation, origin, scale, style);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="spriteFont">The sprite font.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        /// <param name="rotation">The angle, in radians, to rotate the text around the origin.</param>
        /// <param name="origin">The origin of the string. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Vector containing separate scalar multiples for the x- and y-axes of the sprite.</param>
        /// <param name="style">How to style the drawn string.</param>
        public virtual void DrawString(Font spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin,
                                       Vector2 scale, String2D.Styles style)
        {
            if (!IsAssetValid(spriteFont) || string.IsNullOrEmpty(text))
                return;

            _str.Font = spriteFont;
            _str.Text = text;
            _str.Position = position;
            _str.Color = color;
            _str.Rotation = rotation;
            _str.Center = origin;
            _str.Scale = scale;
            _str.Style = style;
            _str.Size = spriteFont.CharacterSize;

            _rw.Draw(_str);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="spriteFont">The sprite font.</param>
        /// <param name="text">The mutable (read/write) string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        /// <param name="rotation">The angle, in radians, to rotate the text around the origin.</param>
        /// <param name="origin">The origin of the string. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Vector containing separate scalar multiples for the x- and y-axes of the sprite.</param>
        /// <param name="style">How to style the drawn string.</param>
        public virtual void DrawString(Font spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, float scale, String2D.Styles style)
        {
            if (!IsAssetValid(spriteFont))
                return;

            DrawString(spriteFont, text.ToString(), position, color, rotation, origin, new Vector2(scale), style);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="spriteFont">The sprite font.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        /// <param name="rotation">The angle, in radians, to rotate the text around the origin.</param>
        /// <param name="origin">The origin of the string. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Vector containing separate scalar multiples for the x- and y-axes of the sprite.</param>
        /// <param name="style">How to style the drawn string.</param>
        public virtual void DrawString(Font spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin,
                                       float scale, String2D.Styles style)
        {
            if (!IsAssetValid(spriteFont))
                return;

            DrawString(spriteFont, text, position, color, rotation, origin, new Vector2(scale), style);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="spriteFont">The sprite font.</param>
        /// <param name="text">The mutable (read/write) string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        public virtual void DrawString(Font spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            if (!IsAssetValid(spriteFont))
                return;

            DrawString(spriteFont, text.ToString(), position, color, 0f, Vector2.Zero, 1f, String2D.Styles.Regular);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="spriteFont">The sprite font.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        public virtual void DrawString(Font spriteFont, string text, Vector2 position, Color color)
        {
            if (!IsAssetValid(spriteFont))
                return;

            DrawString(spriteFont, text, position, color, 0f, Vector2.Zero, 1f, String2D.Styles.Regular);
        }

        /// <summary>
        /// Flushes the sprite batch and restores the device state to how it was before Begin was called.
        /// </summary>
        public virtual void End()
        {
            ContentManager.DoNotUnload = false;
        }

        #endregion
    }
}