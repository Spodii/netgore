using System;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using NetGore;
using NetGore.Editor.WinForms;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    public class SkeletonEditorScreenControl : GraphicsDeviceControl
    {
        /// <summary>
        /// Gets or sets the <see cref="SkeletonEditorForm"/>.
        /// </summary>
        [Browsable(false)]
        public SkeletonEditorForm SkeletonEditorForm { get; set; }

        /// <summary>
        /// Derived classes override this to initialize their drawing code.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            if (DesignMode)
                return;

            // Add an event hook to the tick timer so we can update ourself
            GlobalState.Instance.Tick -= InvokeDrawing;
            GlobalState.Instance.Tick += InvokeDrawing;
        }

        /// <summary>
        /// When overridden in the derived class, draws the graphics to the control.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected override void HandleDraw(TickCount currentTime)
        {
            SkeletonEditorForm.UpdateGame();
            SkeletonEditorForm.DrawGame();
        }
    }
}