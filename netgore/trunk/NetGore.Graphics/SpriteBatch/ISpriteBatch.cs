using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    public interface ISpriteBatch : IDisposable
    {
        event EventHandler Disposing;
        GraphicsDevice GraphicsDevice { get; }
        bool IsDisposed { get; }
        string Name { set; get; }
        object Tag { set; get; }

        void Begin(SpriteBlendMode blendMode, SpriteSortMode sortMode, SaveStateMode stateMode, Matrix transformMatrix);

        void Begin(SpriteBlendMode blendMode, SpriteSortMode sortMode, SaveStateMode stateMode);

        void Begin(SpriteBlendMode blendMode);

        void Begin();

        void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation,
                  Vector2 origin, SpriteEffects effects, float layerDepth);

        void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color);

        void Draw(Texture2D texture, Rectangle destinationRectangle, Color color);

        void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin,
                  Vector2 scale, SpriteEffects effects, float layerDepth);

        void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin,
                  float scale, SpriteEffects effects, float layerDepth);

        void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color);

        void Draw(Texture2D texture, Vector2 position, Color color);

        void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin,
                        Vector2 scale, SpriteEffects effects, float layerDepth);

        void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin,
                        Vector2 scale, SpriteEffects effects, float layerDepth);

        void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin,
                        float scale, SpriteEffects effects, float layerDepth);

        void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin,
                        float scale, SpriteEffects effects, float layerDepth);

        void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color);

        void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color);

        void End();
    }
}