using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore.EditorTools;
using NetGore.Graphics;

namespace DemoGame.SkeletonEditor
{
    class GameScreenControl : GraphicsDeviceControl
    {
        public ScreenForm ScreenForm { get; set; }

        /// <summary>
        /// Derived classes override this to draw themselves using the GraphicsDevice.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use for drawing.</param>
        protected override void Draw(ISpriteBatch spriteBatch)
        {
            ScreenForm.UpdateGame();
            ScreenForm.DrawGame(spriteBatch);
        }
    }
}