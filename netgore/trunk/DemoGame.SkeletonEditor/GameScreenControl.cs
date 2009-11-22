using System.Linq;
using System.Windows.Forms;
using NetGore.EditorTools.WinForms;

namespace DemoGame.SkeletonEditor
{
    class GameScreenControl : GraphicsDeviceControl
    {
        public ScreenForm ScreenForm { get; set; }

        protected override void Draw()
        {
            ScreenForm.UpdateGame();
            ScreenForm.DrawGame();
        }
    }
}