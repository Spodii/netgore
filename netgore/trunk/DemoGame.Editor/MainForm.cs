using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoGame.Editor
{
    public partial class MainForm : Form
    {
        readonly MapForm _mapForm = new MapForm();

        public MainForm()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mapForm.Show(dockPanel);
        }
    }
}
