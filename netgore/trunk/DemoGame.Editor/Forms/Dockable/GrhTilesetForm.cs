using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using NetGore.Graphics;
using SFML.Graphics;
using WeifenLuo.WinFormsUI.Docking;

namespace DemoGame.Editor.Forms.Dockable
{
    public partial class GrhTilesetForm : DockContent
    {
        public GrhTilesetForm()
        {
            InitializeComponent();
        }

        private void grhTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private TilesetConfiguration _tilesetConfiguration = null;

        static int ExtractNumber(GrhData data)
        {
            var text = data.Categorization.Title.ToString();

            Match match = Regex.Match(text, @"(\d+)");
            if (match == null)
            {
                return 0;
            }

            int value;
            if (!int.TryParse(match.Value, out value))
            {
                return 0;
            }

            return value;
        }

        private void selectGRHFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GrhTilesetSelectForm form = new GrhTilesetSelectForm();
            form.ShowDialog();

            if (form.TilesetConfiguration != null)
            {



                _tilesetConfiguration = form.TilesetConfiguration;
                _tilesetConfiguration.GrhDatas.Sort((x, y) => ExtractNumber(x).CompareTo(ExtractNumber(y)));
                grhAtlasView1.TilesetConfiguration = _tilesetConfiguration;
            }
        
        }

        private void camScrollX_ValueChanged(object sender, EventArgs e)
        {
            ComputeCamera();
        }

        private void ComputeCamera()
        {
            var tileSize = EditorSettings.Default.GridSize;
            grhAtlasView1.Camera.Min = new Vector2(camScrollX.Value*tileSize.X, camScrollY.Value*tileSize.Y);
        }

        private void camScrollY_ValueChanged(object sender, EventArgs e)
        {
            ComputeCamera();
        }
    }
}
