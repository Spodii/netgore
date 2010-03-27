using System.Linq;
using System.Windows.Forms;
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
            set { mpc.Camera = value; }
        }
    }
}