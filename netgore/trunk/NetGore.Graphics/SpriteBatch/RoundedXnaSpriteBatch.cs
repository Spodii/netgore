using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    public class RoundedXnaSpriteBatch : XnaSpriteBatch
    {
        public RoundedXnaSpriteBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
        }

        public override void Draw(Texture2D texture, Vector2 position, Color color)
        {
            base.Draw(texture, position.Round(), color);
        }

        public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            base.Draw(texture, position.Round(), sourceRectangle, color);
        }

        public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                  Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            base.Draw(texture, position.Round(), sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public override void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                  Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            base.Draw(texture, position.Round(), sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            base.DrawString(spriteFont, text, position.Round(), color);
        }

        public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
                                        Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(spriteFont, text, position.Round(), color, rotation, origin, scale, effects, layerDepth);
        }

        public override void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
                                        Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(spriteFont, text, position.Round(), color, rotation, origin, scale, effects, layerDepth);
        }

        public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            base.DrawString(spriteFont, text, position.Round(), color);
        }

        public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
                                        Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(spriteFont, text, position.Round(), color, rotation, origin, scale, effects, layerDepth);
        }

        public override void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
                                        Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(spriteFont, text, position.Round(), color, rotation, origin, scale, effects, layerDepth);
        }
    }
}