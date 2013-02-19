using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Editor.Grhs;
using NetGore.Graphics;

namespace DemoGame.Editor.Forms
{
    public partial class GrhTilesetSelectForm : Form
    {
        public GrhTilesetSelectForm()
        {
            InitializeComponent();

            grhTreeView1.RebuildTree();
        }

        private void GrhTilesetSelectFormcs_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public TilesetConfiguration TilesetConfiguration { get; set; }

        /// <summary>
        /// Checks if a <see cref="TreeNode"/> contains a <see cref="GrhData"/>, or is a folder.
        /// </summary>
        /// <param name="node">The <see cref="TreeNode"/> to check.</param>
        /// <returns>True if a Grh, false if a folder.</returns>
        static bool IsGrhDataNode(TreeNode node)
        {
            return node is GrhTreeViewNode;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Do validation
            if (IsGrhDataNode(grhTreeView1.SelectedNode))
            {
                MessageBox.Show("Please select a GRH folder - not a GRH itself!");
                return;
            }

            var grhDatas = new List<GrhData>();

            foreach (var node in grhTreeView1.SelectedNode.Nodes)
            {
                var grhData = node as GrhTreeViewNode;
                
                if (grhData != null)
                    grhDatas.Add(grhData.GrhData);
            }

           TilesetConfiguration = new TilesetConfiguration(grhDatas, (int) numericUpDown1.Value);
            Close();
        }
    }
}
