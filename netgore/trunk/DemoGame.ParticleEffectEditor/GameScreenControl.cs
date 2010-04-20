using System.ComponentModel;
using System.Linq;
using NetGore.EditorTools;
using NetGore.Graphics;

namespace DemoGame.ParticleEffectEditor
{
    public class GameScreenControl : GraphicsDeviceControl
    {
        [Browsable(false)]
        public ScreenForm ScreenForm { get; set; }

        /// <summary>
        /// Derived classes override this to draw themselves using the GraphicsDevice.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to use for drawing.</param>
        protected override void Draw(ISpriteBatch spriteBatch)
        {
            var screenToUse = ScreenForm;
            if (screenToUse == null)
                return;

            ScreenForm.UpdateGame();
            ScreenForm.DrawGame(spriteBatch);
        }
    }
}