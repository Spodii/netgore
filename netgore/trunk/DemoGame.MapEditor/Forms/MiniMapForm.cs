using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.EditorTools;
using NetGore.Graphics;

namespace DemoGame.MapEditor.Forms
{
    public partial class MiniMapForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MiniMapForm"/> class.
        /// </summary>
        public MiniMapForm()
        {
            InitializeComponent();
        }

        public ICamera2D Camera
        {
            get { return mpc.Camera; }
            set
            {
                mpc.Camera = value;
                miResizeToAspect_Click(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the Click event of the miResizeToAspect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void miResizeToAspect_Click(object sender, EventArgs e)
        {
            if (Camera == null || Camera.Map == null)
                return;

            var targetRatio = Camera.Map.Size.X / Camera.Map.Size.Y;
            this.ResizeToAspectRatio(targetRatio, true);
        }
    }
}