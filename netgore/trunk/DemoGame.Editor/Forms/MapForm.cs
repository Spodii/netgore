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
    public partial class MapForm : ChildWindowForm
    {
        public MapForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            ClientSize = new Size((int)GameData.ScreenSize.X, (int)GameData.ScreenSize.Y);

            base.OnLoad(e);
        }
    }
}
