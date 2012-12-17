using System.Linq;
using System.Text;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// An implementation of the <see cref="SpriteBatch"/> that rounds all the drawing positions to prevent
    /// sprites and text from drawing blurred.
    /// This class is NOT thread-safe! If you wish to use threaded rendering, you must use a separate <see cref="ISpriteBatch"/>
    /// for each thread or manually add thread safety.
    /// </summary>
    public class RoundedSpriteBatch : SpriteBatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundedSpriteBatch"/> class.
        /// </summary>
        /// <param name="renderTarget">The <see cref="RenderTarget"/> to draw to.</param>
        public RoundedSpriteBatch(RenderTarget renderTarget) : base(renderTarget)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundedSpriteBatch"/> class.
        /// </summary>
        public RoundedSpriteBatch()
        {
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
        public override void Draw(Texture texture, Vector2 position, Color color, Shader shader = null)
        {
            base.Draw(texture, position.Round(), color, shader);
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
        public override void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, Shader shader = null)
        {
            base.Draw(texture, position.Round(), sourceRectangle, color, shader);
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
        public override void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                  Vector2 origin, float scale, SpriteEffects effects = SpriteEffects.None, Shader shader = null)
        {
            base.Draw(texture, position.Round(), sourceRectangle, color, rotation, origin, scale, effects, shader);
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
        public override void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                  Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, Shader shader = null)
        {
            base.Draw(texture, position.Round(), sourceRectangle, color, rotation, origin, scale, effects, shader);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="font">The <see cref="Font"/> to use to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        public override void DrawString(Font font, string text, Vector2 position, Color color)
        {
            base.DrawString(font, text, position.Round(), color);
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
        public override void DrawString(Font font, string text, Vector2 position, Color color, float rotation, Vector2 origin,
                                        float scale, Text.Styles style = Text.Styles.Regular, Shader shader = null)
        {
            base.DrawString(font, text, position.Round(), color, rotation, origin, scale, style, shader);
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
        public override void DrawString(Font font, string text, Vector2 position, Color color, float rotation, Vector2 origin,
                                        Vector2 scale, Text.Styles style = Text.Styles.Regular, Shader shader = null)
        {
            base.DrawString(font, text, position.Round(), color, rotation, origin, scale, style, shader);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="font">The <see cref="Font"/> to use to draw.</param>
        /// <param name="text">The mutable (read/write) string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        public override void DrawString(Font font, StringBuilder text, Vector2 position, Color color)
        {
            base.DrawString(font, text, position.Round(), color);
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
        public override void DrawString(Font font, StringBuilder text, Vector2 position, Color color, float rotation,
                                        Vector2 origin, float scale, Text.Styles style = Text.Styles.Regular, Shader shader = null)
        {
            base.DrawString(font, text, position.Round(), color, rotation, origin, scale, style, shader);
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
        public override void DrawString(Font font, StringBuilder text, Vector2 position, Color color, float rotation,
                                        Vector2 origin, Vector2 scale, Text.Styles style = Text.Styles.Regular,
                                        Shader shader = null)
        {
            base.DrawString(font, text, position.Round(), color, rotation, origin, scale, style, shader);
        }
    }
}