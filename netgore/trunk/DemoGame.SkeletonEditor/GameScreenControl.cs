using System;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using NetGore;
using NetGore.Editor.WinForms;
using NetGore.Graphics;

namespace DemoGame.SkeletonEditor
{
    class GameScreenControl : GraphicsDeviceControl
    {
        readonly Timer t = new Timer();

        /// <summary>
        /// Gets or sets the <see cref="ScreenForm"/>.
        /// </summary>
        [Browsable(false)]
        public ScreenForm ScreenForm { get; set; }

        public GameScreenControl()
        {
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 1000 / 60;
            t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Created && RenderWindow != null)
                Invoke((Action)(() => InvokeDrawing(TickCount.Now)));
        }

        /// <summary>
        /// When overridden in the derived class, draws the graphics to the control.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected override void HandleDraw(NetGore.TickCount currentTime)
        {
            ScreenForm.UpdateGame();
            ScreenForm.DrawGame();
        }
    }
}