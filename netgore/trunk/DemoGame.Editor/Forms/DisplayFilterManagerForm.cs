using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoGame.Editor.Forms
{
    public partial class DisplayFilterManagerForm : Form
    {
        MapDrawFilterHelperCollection _collection;

        public DisplayFilterManagerForm()
        {
            InitializeComponent();
        }

        public MapDrawFilterHelperCollection Collection
        {
            get
            {
                return _collection;
            }
            set
            {
                if (_collection == value)
                    return;

                lstItems.Items.Clear();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var c = Collection;
            if (c == null)
                return;


        }
    }
}
