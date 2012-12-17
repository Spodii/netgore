using System;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an object that can batch the drawing of multiple 2D sprites and text.
    /// Classes that implement this interface are NOT guarenteed to be thread-safe! If you wish to use
    /// threaded rendering, you must use a separate <see cref="ISpriteBatch"/> for each thread or manually
    /// add thread safety.
    /// </summary>
    public interface ISpriteBatch : IDisposable
    {
        /// <summary>
        /// Gets or sets the <see cref="BlendMode"/> currently being used.
        /// </summary>
        BlendMode BlendMode { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets if this <see cref="ISpriteBatch"/> is currently inbetween calls to Begin() and End().
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Gets or sets the name of this sprite batch.
        /// </summary>
        string Name { set; get; }

        /// <summary>
        /// Gets or sets the <see cref="RenderTarget"/> that this <see cref="SpriteBatch"/> is drawing to.
        /// </summary>
        RenderTarget RenderTarget { get; set; }

        /// <summary>
        /// Gets or sets an object that uniquely identifies this sprite batch.
        /// </summary>
        object Tag { set; get; }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="position">The top-left corner of the view area.</param>
        /// <param name="size">The size of the view area.</param>
        /// <param name="rotation">The amount to rotation the view in degrees.</param>
        void Begin(BlendMode blendMode, Vector2 position, Vector2 size, float rotation);

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the world.</param>
        void Begin(BlendMode blendMode, ICamera2D camera);

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        void Begin(BlendMode blendMode);

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state options,
        /// and a global transform matrix.
        /// </summary>
        void Begin();

        /// <summary>
        /// Draws a raw <see cref="SFML.Graphics.Sprite"/>. Recommended to avoid using when possible, but when needed,
        /// can provide a slight performance boost when drawing a large number of sprites with identical state.
        /// </summary>
        /// <param name="sprite">The <see cref="SFML.Graphics.Sprite"/> to draw.</param>
        /// <param name="shader">The <see cref="Shader"/> to use while drawing.</param>
        void Draw(SFML.Graphics.Sprite sprite, Shader shader = null);

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
        void Draw(Texture texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation,
                  Vector2 origin, SpriteEffects effects = SpriteEffects.None, Shader shader = null);

        /// <summary>
        /// Draws a raw <see cref="Drawable"/> object.
        /// </summary>
        /// <param name="drawable">The object to draw.</param>
        /// <param name="shader">The shader to use on the text being drawn.</param>
        void Draw(Drawable drawable, Shader shader = null);

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
        void Draw(Texture texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, Shader shader = null);

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
        void Draw(Texture texture, Rectangle destinationRectangle, Color color, Shader shader = null);

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
        void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin,
                  Vector2 scale, SpriteEffects effects = SpriteEffects.None, Shader shader = null);

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
        void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin,
                  float scale, SpriteEffects effects = SpriteEffects.None, Shader shader = null);

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
        void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, Shader shader = null);

        /// <summary>
        /// Adds a sprite to the batch of sprites to be rendered, specifying the texture, destination, and source rectangles,
        /// color tint, rotation, origin, effects, and sort depth.
        /// </summary>
        /// <param name="texture">The sprite texture.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        /// <param name="color">The color channel modulation to use. Use <see cref="Color.White"/> for full color with
        /// no tinting.</param>
        /// <param name="shader">The shader to use on the text being drawn.</param>
        void Draw(Texture texture, Vector2 position, Color color, Shader shader = null);

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
        void DrawString(Font font, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin,
                        Vector2 scale, Text.Styles style = Text.Styles.Regular, Shader shader = null);

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
        void DrawString(Font font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale,
                        Text.Styles style = Text.Styles.Regular, Shader shader = null);

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
        void DrawString(Font font, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin,
                        float scale = 1.0f, Text.Styles style = Text.Styles.Regular, Shader shader = null);

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
        void DrawString(Font font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale,
                        Text.Styles style = Text.Styles.Regular, Shader shader = null);

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="font">The <see cref="Font"/> to use to draw.</param>
        /// <param name="text">The mutable (read/write) string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        void DrawString(Font font, StringBuilder text, Vector2 position, Color color);

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="font">The <see cref="Font"/> to use to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        void DrawString(Font font, string text, Vector2 position, Color color);

        /// <summary>
        /// Flushes the sprite batch and restores the device state to how it was before Begin was called.
        /// </summary>
        void End();
    }
}