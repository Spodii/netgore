using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// An implementation of <see cref="ISpriteBatch"/> using the <see cref="RenderWindow"/>.
    /// This class is NOT thread-safe! If you wish to use threaded rendering, you must use a separate <see cref="ISpriteBatch"/>
    /// for each thread or manually add thread safety.
    /// </summary>
    public class SpriteBatch : ISpriteBatch
    {
        readonly SFML.Graphics.Sprite _sprite = new SFML.Graphics.Sprite();
        readonly Text _str = new Text();
        readonly View _view = new View();
        readonly RenderStates _renderState = RenderStates.Default;

        bool _isDisposed;
        bool _isStarted;
        RenderTarget _rt;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatch"/> class.
        /// </summary>
        /// <param name="renderTarget">The <see cref="RenderTarget"/> to draw to.</param>
        public SpriteBatch(RenderTarget renderTarget)
        {
            _rt = renderTarget;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatch"/> class.
        /// </summary>
        public SpriteBatch()
        {
        }

        /// <summary>
        /// Checks if an asset is in a valid state to be used.
        /// </summary>
        /// <param name="asset">The asset to check if valid.</param>
        /// <returns>True if the <paramref name="asset"/> is in a valid state to be used; otherwise false.</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        protected static bool IsAssetValid(Texture asset)
        {
            if (asset == null || asset.IsDisposed)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if an asset is in a valid state to be used.
        /// </summary>
        /// <param name="asset">The asset to check if valid.</param>
        /// <returns>True if the <paramref name="asset"/> is in a valid state to be used; otherwise false.</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        protected static bool IsAssetValid(Font asset)
        {
            if (asset == null || asset.IsDisposed)
                return false;

            return true;
        }

        #region ISpriteBatch Members

        /// <summary>
        /// Gets or sets the <see cref="BlendMode"/> currently being used.
        /// </summary>
        public BlendMode BlendMode
        {
            get { return _renderState.BlendMode; }
            set { _renderState.BlendMode = value; }
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets if this <see cref="ISpriteBatch"/> is currently inbetween calls to Begin() and End().
        /// </summary>
        public bool IsStarted
        {
            get { return _isStarted; }
        }

        /// <summary>
        /// Gets or sets the name of this sprite batch.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="RenderTarget"/> that this <see cref="SpriteBatch"/> is drawing to.
        /// </summary>
        public RenderTarget RenderTarget
        {
            get { return _rt; }
            set { _rt = value; }
        }

        /// <summary>
        /// Gets or sets an object that uniquely identifies this sprite batch.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="position">The top-left corner of the view area.</param>
        /// <param name="size">The size of the view area.</param>
        /// <param name="rotation">The amount to rotation the view in degrees.</param>
        public void Begin(BlendMode blendMode, Vector2 position, Vector2 size, float rotation)
        {
            position = position.Round();
            size = size.Round();

            _view.Reset(new FloatRect(position.X, position.Y, size.X, size.Y));
            _view.Rotate(rotation);
            _rt.SetView(_view);

            _renderState.BlendMode = blendMode;

            _isStarted = true;
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the world.</param>
        public virtual void Begin(BlendMode blendMode, ICamera2D camera)
        {
            Begin(blendMode, camera.Min, camera.Size, camera.Rotation);

            _isStarted = true;
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        public virtual void Begin(BlendMode blendMode)
        {
            var rtSize = _rt.Size;

            _view.Reset(new FloatRect(0f, 0f, rtSize.X, rtSize.Y));
            _view.Rotate(0f);
            _rt.SetView(_view);

            _renderState.BlendMode = blendMode;

            _isStarted = true;
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
        /// <param name="shader">The <see cref="Shader"/> to use while drawing.</param>
        public virtual void Draw(SFML.Graphics.Sprite sprite, Shader shader = null)
        {
            if (sprite == null || !IsAssetValid(sprite.Texture))
                return;

            _renderState.Shader = shader;
            _rt.Draw(sprite, _renderState);
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
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public virtual void Draw(Texture texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color,
                                 float rotation, Vector2 origin, SpriteEffects effects = SpriteEffects.None, Shader shader = null)
        {
            if (!IsAssetValid(texture))
                return;

            if (sourceRectangle.HasValue)
            {
                _sprite.TextureRect = (IntRect)sourceRectangle.Value;
            }
            else
            {
                Texture t = _sprite.Texture;
                var tSize = t.Size;
                _sprite.TextureRect = new IntRect(0, 0, (int)tSize.X, (int)tSize.Y);
            }

            var spriteTextureRect = _sprite.TextureRect;

            _sprite.Texture = texture;
            _sprite.Position = new Vector2(destinationRectangle.X, destinationRectangle.Y);
            _sprite.Color = color;
            _sprite.Rotation = MathHelper.ToDegrees(rotation);
            _sprite.Origin = origin;
            _sprite.Scale = new Vector2((float)destinationRectangle.Width / spriteTextureRect.Width, (float)destinationRectangle.Height / spriteTextureRect.Height)
                * GetScaleEffectMultiplier(effects);

            _renderState.Shader = shader;
            _rt.Draw(_sprite, _renderState);
        }

        /// <summary>
        /// Draws a raw <see cref="Drawable"/> object.
        /// </summary>
        /// <param name="drawable">The object to draw.</param>
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public void Draw(Drawable drawable, Shader shader = null)
        {
            if (drawable == null)
                return;

            _renderState.Shader = shader;
            _rt.Draw(drawable, _renderState);
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
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public virtual void Draw(Texture texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, Shader shader = null)
        {
            Draw(texture, destinationRectangle, sourceRectangle, color, 0f, Vector2.Zero, SpriteEffects.None, shader);
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
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public virtual void Draw(Texture texture, Rectangle destinationRectangle, Color color, Shader shader = null)
        {
            Draw(texture, destinationRectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, shader);
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
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public virtual void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                 Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, Shader shader = null)
        {
            if (!IsAssetValid(texture))
                return;

            if (sourceRectangle.HasValue)
            {
                _sprite.TextureRect = (IntRect)sourceRectangle.Value;
            }
            else
            {
                Texture t = _sprite.Texture;
                var tSize = t.Size;
                _sprite.TextureRect = new IntRect(0, 0, (int)tSize.X, (int)tSize.Y);
            }

            _sprite.Texture = texture;
            _sprite.Position = position;
            _sprite.Color = color;
            _sprite.Rotation = MathHelper.ToDegrees(rotation);
            _sprite.Origin = origin;
            _sprite.Scale = scale * GetScaleEffectMultiplier(effects);

            _renderState.Shader = shader;

            _rt.Draw(_sprite, _renderState);
        }

        /// <summary>
        /// Gets the scale multiplier for applying the given SpriteEffects to a sprite.
        /// </summary>
        static Vector2 GetScaleEffectMultiplier(SpriteEffects effects)
        {
            return new Vector2(
                ((effects & SpriteEffects.FlipHorizontally) != 0) ? -1 : 1,
                ((effects & SpriteEffects.FlipVertically) != 0) ? -1 : 1
            );
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
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public virtual void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                 Vector2 origin, float scale, SpriteEffects effects = SpriteEffects.None, Shader shader = null)
        {
            Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale), effects, shader);
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
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public virtual void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, Shader shader = null)
        {
            Draw(texture, position, sourceRectangle, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, shader);
        }

        /// <summary>
        /// Adds a sprite to the batch of sprites to be rendered, specifying the texture, destination, and source rectangles,
        /// color tint, rotation, origin, effects, and sort depth.
        /// </summary>
        /// <param name="texture">The sprite texture.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        /// <param name="color">The color channel modulation to use. Use <see cref="Color.White"/> for full color with
        /// no tinting.</param>
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public virtual void Draw(Texture texture, Vector2 position, Color color, Shader shader = null)
        {
            Draw(texture, position, null, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, shader);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="font">The <see cref="Font"/> to use to draw.</param>
        /// <param name="text">The mutable (read/write) string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        /// <param name="rotation">The angle, in radians, to rotate the text around the origin.</param>
        /// <param name="origin">The origin of the string. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Vector containing separate scalar multiples for the x- and y-axes of the sprite.</param>
        /// <param name="style">How to style the drawn string.</param>
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public virtual void DrawString(Font font, StringBuilder text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, Vector2 scale, Text.Styles style = Text.Styles.Regular, Shader shader = null)
        {
            if (!IsAssetValid(font))
                return;

            DrawString(font, text.ToString(), position, color, rotation, origin, scale, style, shader);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="font">The <see cref="Font"/> to use to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        /// <param name="rotation">The angle, in radians, to rotate the text around the origin.</param>
        /// <param name="origin">The origin of the string. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Vector containing separate scalar multiples for the x- and y-axes of the sprite.</param>
        /// <param name="style">How to style the drawn string.</param>
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public virtual void DrawString(Font font, string text, Vector2 position, Color color, float rotation, Vector2 origin,
                                       Vector2 scale, Text.Styles style = Text.Styles.Regular, Shader shader = null)
        {
            if (!IsAssetValid(font) || string.IsNullOrEmpty(text))
                return;

            _str.Font = font;
            _str.DisplayedString = text;
            _str.Position = position;
            _str.Color = color;
            _str.Rotation = rotation;
            _str.Origin = origin;
            _str.Scale = scale;
            _str.Style = style;
            _str.CharacterSize = font.DefaultSize;

            _renderState.Shader = shader;

            _rt.Draw(_str, _renderState);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="font">The <see cref="Font"/> to use to draw.</param>
        /// <param name="text">The mutable (read/write) string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        /// <param name="rotation">The angle, in radians, to rotate the text around the origin.</param>
        /// <param name="origin">The origin of the string. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Vector containing separate scalar multiples for the x- and y-axes of the sprite.</param>
        /// <param name="style">How to style the drawn string.</param>
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public virtual void DrawString(Font font, StringBuilder text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, float scale, Text.Styles style = Text.Styles.Regular, Shader shader = null)
        {
            if (!IsAssetValid(font))
                return;

            DrawString(font, text.ToString(), position, color, rotation, origin, new Vector2(scale), style, shader);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="font">The <see cref="Font"/> to use to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        /// <param name="rotation">The angle, in radians, to rotate the text around the origin.</param>
        /// <param name="origin">The origin of the string. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Vector containing separate scalar multiples for the x- and y-axes of the sprite.</param>
        /// <param name="style">How to style the drawn string.</param>
        /// <param name="shader">The shader to use on the text being drawn.</param>
        public virtual void DrawString(Font font, string text, Vector2 position, Color color, float rotation, Vector2 origin,
                                       float scale, Text.Styles style = Text.Styles.Regular, Shader shader = null)
        {
            if (!IsAssetValid(font))
                return;

            DrawString(font, text, position, color, rotation, origin, new Vector2(scale), style, shader);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="font">The <see cref="Font"/> to use to draw.</param>
        /// <param name="text">The mutable (read/write) string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        public virtual void DrawString(Font font, StringBuilder text, Vector2 position, Color color)
        {
            if (!IsAssetValid(font))
                return;

            DrawString(font, text.ToString(), position, color, 0.0f, Vector2.Zero, 1.0f);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="font">The <see cref="Font"/> to use to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        public virtual void DrawString(Font font, string text, Vector2 position, Color color)
        {
            if (!IsAssetValid(font))
                return;

            DrawString(font, text, position, color, 0.0f, Vector2.Zero, 1.0f);
        }

        /// <summary>
        /// Flushes the sprite batch and restores the device state to how it was before Begin was called.
        /// </summary>
        public virtual void End()
        {
            _isStarted = false;
        }

        #endregion
    }
}