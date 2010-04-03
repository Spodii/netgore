using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// An implementation of <see cref="ISpriteBatch"/> that uses the XNA <see cref="SpriteBatch"/>.
    /// </summary>
    public class XnaSpriteBatch : ISpriteBatch
    {
        readonly SpriteBatch _sb;

        /// <summary>
        /// Initializes a new instance of the <see cref="XnaSpriteBatch"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        public XnaSpriteBatch(GraphicsDevice graphicsDevice)
        {
            _sb = new SpriteBatch(graphicsDevice);
        }

        protected static bool IsAssetValid(GraphicsResource texture)
        {
            if (texture == null || texture.IsDisposed)
                return false;

            return true;
        }

        protected static bool IsAssetValid(SpriteFont sf)
        {
            if (sf == null)
                return false;

            return true;
        }

        #region ISpriteBatch Members

        /// <summary>
        /// Occurs when Dispose is called or when this object is finalized and collected by the garbage collector.
        /// </summary>
        public event EventHandler Disposing
        {
            add { _sb.Disposing += value; }
            remove { _sb.Disposing -= value; }
        }

        /// <summary>
        /// Gets the graphics device associated with this sprite batch.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return _sb.GraphicsDevice; }
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _sb.IsDisposed; }
        }

        /// <summary>
        /// Gets or sets the name of this sprite batch.
        /// </summary>
        public string Name
        {
            get { return _sb.Name; }
            set { _sb.Name = value; }
        }

        /// <summary>
        /// Gets or sets an object that uniquely identifies this sprite batch.
        /// </summary>
        public object Tag
        {
            get { return _sb.Tag; }
            set { _sb.Tag = value; }
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="stateMode">Rendering state options.</param>
        /// <param name="transformMatrix">A matrix to apply to position, rotation, scale, and depth data passed to Draw.</param>
        public virtual void Begin(SpriteBlendMode blendMode, SpriteSortMode sortMode, SaveStateMode stateMode,
                                  Matrix transformMatrix)
        {
            _sb.Begin(blendMode, sortMode, stateMode, transformMatrix);

            XnaContentManager.DoNotUnload = true;
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="stateMode">Rendering state options.</param>
        public virtual void Begin(SpriteBlendMode blendMode, SpriteSortMode sortMode, SaveStateMode stateMode)
        {
            _sb.Begin(blendMode, sortMode, stateMode);

            XnaContentManager.DoNotUnload = true;
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        public virtual void Begin(SpriteBlendMode blendMode)
        {
            _sb.Begin(blendMode);

            XnaContentManager.DoNotUnload = true;
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        public virtual void Begin()
        {
            _sb.Begin();

            XnaContentManager.DoNotUnload = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            _sb.Dispose();
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
        /// <param name="layerDepth">The sorting depth of the sprite, between 0 (front) and 1 (back). You must specify
        /// either <see cref="SpriteSortMode.FrontToBack"/> or <see cref="SpriteSortMode.BackToFront"/> for this parameter
        /// to affect sprite drawing.</param>
        public virtual void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color,
                                 float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            if (!IsAssetValid(texture))
                return;

            _sb.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
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
        public virtual void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            if (!IsAssetValid(texture))
                return;

            _sb.Draw(texture, destinationRectangle, sourceRectangle, color);
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
        public virtual void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            if (!IsAssetValid(texture))
                return;

            _sb.Draw(texture, destinationRectangle, color);
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
        /// <param name="layerDepth">The sorting depth of the sprite, between 0 (front) and 1 (back). You must specify
        /// either <see cref="SpriteSortMode.FrontToBack"/> or <see cref="SpriteSortMode.BackToFront"/> for this parameter
        /// to affect sprite drawing.</param>
        public virtual void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                 Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (!IsAssetValid(texture))
                return;

            _sb.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
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
        /// <param name="layerDepth">The sorting depth of the sprite, between 0 (front) and 1 (back). You must specify
        /// either <see cref="SpriteSortMode.FrontToBack"/> or <see cref="SpriteSortMode.BackToFront"/> for this parameter
        /// to affect sprite drawing.</param>
        public virtual void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                 Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (!IsAssetValid(texture))
                return;

            _sb.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
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
        public virtual void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            if (!IsAssetValid(texture))
                return;

            _sb.Draw(texture, position, sourceRectangle, color);
        }

        /// <summary>
        /// Adds a sprite to the batch of sprites to be rendered, specifying the texture, destination, and source rectangles,
        /// color tint, rotation, origin, effects, and sort depth.
        /// </summary>
        /// <param name="texture">The sprite texture.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        /// <param name="color">The color channel modulation to use. Use <see cref="Color.White"/> for full color with
        /// no tinting.</param>
        public virtual void Draw(Texture2D texture, Vector2 position, Color color)
        {
            if (!IsAssetValid(texture))
                return;

            _sb.Draw(texture, position, color);
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
        /// <param name="effects">Rotations to apply prior to rendering.</param>
        /// <param name="layerDepth">The sorting depth of the sprite, between 0 (front) and 1 (back).</param>
        public virtual void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (!IsAssetValid(spriteFont))
                return;

            _sb.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
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
        /// <param name="effects">Rotations to apply prior to rendering.</param>
        /// <param name="layerDepth">The sorting depth of the sprite, between 0 (front) and 1 (back).</param>
        public virtual void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (!IsAssetValid(spriteFont))
                return;

            _sb.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
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
        /// <param name="effects">Rotations to apply prior to rendering.</param>
        /// <param name="layerDepth">The sorting depth of the sprite, between 0 (front) and 1 (back).</param>
        public virtual void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (!IsAssetValid(spriteFont))
                return;

            _sb.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
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
        /// <param name="effects">Rotations to apply prior to rendering.</param>
        /// <param name="layerDepth">The sorting depth of the sprite, between 0 (front) and 1 (back).</param>
        public virtual void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (!IsAssetValid(spriteFont))
                return;

            _sb.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="spriteFont">The sprite font.</param>
        /// <param name="text">The mutable (read/write) string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        public virtual void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            if (!IsAssetValid(spriteFont))
                return;

            _sb.DrawString(spriteFont, text, position, color);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="spriteFont">The sprite font.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        public virtual void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            if (!IsAssetValid(spriteFont))
                return;

            _sb.DrawString(spriteFont, text, position, color);
        }

        /// <summary>
        /// Flushes the sprite batch and restores the device state to how it was before Begin was called.
        /// </summary>
        public virtual void End()
        {
            _sb.End();

            XnaContentManager.DoNotUnload = false;
        }

        #endregion
    }
}