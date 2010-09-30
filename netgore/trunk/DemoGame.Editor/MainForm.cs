using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            ToolBar.GlobalToolBar = tbGlobal;
            ToolBar.NonGlobalToolBar = tbNonGlobal;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new EditMapForm();
            frm.MapScreenControl.ChangeMap(new NetGore.World.MapID(1));

            frm.Show(dockPanel);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: !!
        }
    }
}
