using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.EditorTools.WinForms;

namespace DemoGame.SkeletonEditor
{
    class GameScreenControl : GraphicsDeviceControl
    {
        public new ScreenForm Parent;

        protected override void Draw()
        {
            Parent.UpdateGame();
            Parent.DrawGame();
        }

        protected override void Initialize()
        {
            Application.Idle += delegate { Invalidate(); };
        }
    }
}