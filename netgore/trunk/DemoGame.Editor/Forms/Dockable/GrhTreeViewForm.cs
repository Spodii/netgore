using System;
using System.Linq;
using NetGore.Editor.Docking;

namespace DemoGame.Editor
{
    public partial class GrhTreeViewForm : DockContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrhTreeViewForm"/> class.
        /// </summary>
        public GrhTreeViewForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
                return;

            gtv.Initialize(GlobalState.Instance.ContentManager, (pos, size) => new WallEntity(pos, size),
                           GlobalState.Instance.MapGrhWalls);
        }
    }
}