using System.Linq;
using NetGore.EditorTools;

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