using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore.EditorTools;

namespace DemoGame.ParticleEffectEditor
{
    public class GameScreenControl : GraphicsDeviceControl
    {
        public ScreenForm ScreenForm { get; set; }

        /// <summary>
        /// Derived classes override this to draw themselves using the GraphicsDevice.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to use for drawing.</param>
        protected override void Draw(SpriteBatch spriteBatch)
        {
            ScreenForm screenToUse = ScreenForm;
            if (screenToUse == null)
                return;

            ScreenForm.UpdateGame();
            ScreenForm.DrawGame(spriteBatch);
        }
    }
}