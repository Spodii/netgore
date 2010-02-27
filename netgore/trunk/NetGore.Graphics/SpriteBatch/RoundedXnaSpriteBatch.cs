using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// An implementation of the <see cref="XnaSpriteBatch"/> that rounds all the drawing positions to prevent
    /// sprites and text from drawing blurred.
    /// </summary>
    public class RoundedXnaSpriteBatch : XnaSpriteBatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundedXnaSpriteBatch"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        public RoundedXnaSpriteBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice)
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
        public override void Draw(Texture2D texture, Vector2 position, Color color)
        {
            base.Draw(texture, position.Round(), color);
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
        public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            base.Draw(texture, position.Round(), sourceRectangle, color);
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
        public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                  Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            base.Draw(texture, position.Round(), sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
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
        public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                  Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            base.Draw(texture, position.Round(), sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="spriteFont">The sprite font.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            base.DrawString(spriteFont, text, position.Round(), color);
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
        public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
                                        Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(spriteFont, text, position.Round(), color, rotation, origin, scale, effects, layerDepth);
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
        public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
                                        Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(spriteFont, text, position.Round(), color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Adds a mutable sprite string to the batch of sprites to be rendered, specifying the font, output text,
        /// screen position, color tint, rotation, origin, scale, effects, and depth.
        /// </summary>
        /// <param name="spriteFont">The sprite font.</param>
        /// <param name="text">The mutable (read/write) string to draw.</param>
        /// <param name="position">The location, in screen coordinates, where the text will be drawn.</param>
        /// <param name="color">The desired color of the text.</param>
        public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            base.DrawString(spriteFont, text, position.Round(), color);
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
        public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
                                        Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(spriteFont, text, position.Round(), color, rotation, origin, scale, effects, layerDepth);
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
        public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
                                        Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(spriteFont, text, position.Round(), color, rotation, origin, scale, effects, layerDepth);
        }
    }
}