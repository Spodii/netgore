using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    public class XnaSpriteBatch : ISpriteBatch
    {
        readonly SpriteBatch _sb;

        public XnaSpriteBatch(GraphicsDevice graphicsDevice)
        {
            _sb = new SpriteBatch(graphicsDevice);
        }

        #region ISpriteBatch Members

        public event EventHandler Disposing
        {
            add { _sb.Disposing += value; }
            remove { _sb.Disposing -= value; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return _sb.GraphicsDevice; }
        }

        public bool IsDisposed
        {
            get { return _sb.IsDisposed; }
        }

        public string Name
        {
            get { return _sb.Name; }
            set { _sb.Name = value; }
        }

        public object Tag
        {
            get { return _sb.Tag; }
            set { _sb.Tag = value; }
        }

        public virtual void Begin(SpriteBlendMode blendMode, SpriteSortMode sortMode, SaveStateMode stateMode,
                                  Matrix transformMatrix)
        {
            _sb.Begin(blendMode, sortMode, stateMode, transformMatrix);
        }

        public virtual void Begin(SpriteBlendMode blendMode, SpriteSortMode sortMode, SaveStateMode stateMode)
        {
            _sb.Begin(blendMode, sortMode, stateMode);
        }

        public virtual void Begin(SpriteBlendMode blendMode)
        {
            _sb.Begin(blendMode);
        }

        public virtual void Begin()
        {
            _sb.Begin();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            _sb.Dispose();
        }

        public virtual void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color,
                                 float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            _sb.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        public virtual void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            _sb.Draw(texture, destinationRectangle, sourceRectangle, color);
        }

        public virtual void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            _sb.Draw(texture, destinationRectangle, color);
        }

        public virtual void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                 Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            _sb.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public virtual void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
                                 Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            _sb.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public virtual void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            _sb.Draw(texture, position, sourceRectangle, color);
        }

        public virtual void Draw(Texture2D texture, Vector2 position, Color color)
        {
            _sb.Draw(texture, position, color);
        }

        public virtual void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            _sb.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public virtual void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            _sb.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public virtual void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            _sb.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public virtual void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
                                       Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            _sb.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public virtual void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            _sb.DrawString(spriteFont, text, position, color);
        }

        public virtual void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            _sb.DrawString(spriteFont, text, position, color);
        }

        public virtual void End()
        {
            _sb.End();
        }

        #endregion
    }
}