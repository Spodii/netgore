using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DemoGame.Server.DbObjs;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor
{
    public partial class ShiftContentsForm : Form
    {
        readonly EditorMap _map;

        public ShiftContentsForm(EditorMap map)
        {
            if (map == null)
                throw new ArgumentNullException("map");

            _map = map;

            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Vector2 moveVector;
            try
            {
                moveVector = new Vector2(int.Parse(txtX.Text), int.Parse(txtY.Text));
            }
            catch
            {
                MessageBox.Show("Invalid inputs. Please enter a valid numeric value into the X/Y textbox.", "Invalid inputs", MessageBoxButtons.OK);
                return;
            }

            _map.UndoManager.Snapshot();

            foreach (var x in _map.GetAllSpatials())
                x.TryMove(x.Position + moveVector);

            foreach (var x in _map.ParticleEffects)
                x.Position += moveVector;

            _map.UndoManager.Snapshot();

            Close();
        }
    }
}
