using System.Linq;
using System.Windows.Forms;
using NetGore.EditorTools.WinForms;

namespace DemoGame.ParticleEffectEditor
{
    public class GameScreenControl : GraphicsDeviceControl
    {
        ScreenForm _screen;

        public ScreenForm Screen
        {
            get { return _screen; }
            set { _screen = value; }
        }

        /// <summary>
        /// Derived classes override this to draw themselves using the GraphicsDevice.
        /// </summary>
        protected override void Draw()
        {
            ScreenForm screenToUse = Screen;
            if (screenToUse == null)
                return;

            Screen.UpdateGame();
            Screen.DrawGame();
        }

        /// <summary>
        /// Derived classes override this to initialize their drawing code.
        /// </summary>
        protected override void Initialize()
        {
            Application.Idle += delegate { Invalidate(); };
        }
    }
}