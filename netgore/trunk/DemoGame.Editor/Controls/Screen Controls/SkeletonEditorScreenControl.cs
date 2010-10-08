using System.ComponentModel;
using System.Linq;
using NetGore;
using NetGore.Editor.WinForms;

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
        /// When overridden in the derived class, draws the graphics to the control.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected override void HandleDraw(TickCount currentTime)
        {
            SkeletonEditorForm.UpdateGame();
            SkeletonEditorForm.DrawGame();
        }

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
    }
}