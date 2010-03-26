using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Graphics;

namespace DemoGame.MapEditor.Forms
{
    public partial class MiniMapForm : Form
    {
        public ICamera2D Camera { get { return mpc.Camera; } set { mpc.Camera = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniMapForm"/> class.
        /// </summary>
        public MiniMapForm()
        {
            InitializeComponent();
        }
    }
}
