using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.EditorTools.Docking;

namespace DemoGame.Editor
{
    public partial class ChildWindowForm : DockContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChildWindowForm"/> class.
        /// </summary>
        public ChildWindowForm()
        {
            HideOnClose = true;

            InitializeComponent();
        }
    }
}
