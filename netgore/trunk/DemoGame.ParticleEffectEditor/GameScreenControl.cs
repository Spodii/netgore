using System.ComponentModel;
using System.Linq;
using NetGore.Editor.WinForms;

using NetGore.Graphics;

namespace DemoGame.ParticleEffectEditor
{
    public class GameScreenControl : GraphicsDeviceControl
    {
        [Browsable(false)]
        public ScreenForm ScreenForm { get; set; }

        /// <summary>
        /// When overridden in the derived class, draws the graphics to the control.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected override void HandleDraw(NetGore.TickCount currentTime)
        {
            /*
            var screenToUse = ScreenForm;
            if (screenToUse == null)
                return;

            ScreenForm.UpdateGame();
            ScreenForm.DrawGame(spriteBatch);
            */
        }
    }
}