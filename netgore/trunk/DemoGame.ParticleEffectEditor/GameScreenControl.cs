using System.Linq;
using NetGore.EditorTools.WinForms;

namespace DemoGame.ParticleEffectEditor
{
    public class GameScreenControl : GraphicsDeviceControl
    {
        public ScreenForm ScreenForm { get; set; }

        /// <summary>
        /// Derived classes override this to draw themselves using the GraphicsDevice.
        /// </summary>
        protected override void Draw()
        {
            ScreenForm screenToUse = ScreenForm;
            if (screenToUse == null)
                return;

            ScreenForm.UpdateGame();
            ScreenForm.DrawGame();
        }
    }
}