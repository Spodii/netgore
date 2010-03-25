using System.ComponentModel;
using System.Linq;
using NetGore.EditorTools;
using NetGore.Graphics;

namespace DemoGame.SkeletonEditor
{
    class GameScreenControl : GraphicsDeviceControl
    {
        /// <summary>
        /// Gets or sets the <see cref="ScreenForm"/>.
        /// </summary>
        [Browsable(false)]
        public ScreenForm ScreenForm { get; set; }

        /// <summary>
        /// Derived classes override this to draw themselves using the GraphicsDevice.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use for drawing.</param>
        protected override void Draw(ISpriteBatch spriteBatch)
        {
            ScreenForm.UpdateGame();
            ScreenForm.DrawGame();
        }
    }
}