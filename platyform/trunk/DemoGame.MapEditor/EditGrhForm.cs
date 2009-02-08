using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Platyform;
using Platyform.Graphics;
using Color=System.Drawing.Color;
using Rectangle=Microsoft.Xna.Framework.Rectangle;

namespace DemoGame.MapEditor
{
    public partial class EditGrhForm : Form
    {
        static readonly Color ColorChanged = Color.Lime;
        static readonly Color ColorError = Color.Red;
        static readonly Color ColorNormal = SystemColors.Window;

        /// <summary>
        /// Character used to separate directories
        /// </summary>
        static readonly char DirSep = Path.DirectorySeparatorChar;

        readonly GrhData _gd;
        readonly MapGrhWalls _mapGrhWalls;

        public EditGrhForm(GrhData gd, MapGrhWalls mapGrhWalls)
        {
            _gd = gd;
            _mapGrhWalls = mapGrhWalls;
            InitializeComponent();
            ShowGrhInfo();
        }

        void btnAccept_Click(object sender, EventArgs e)
        {
            // Validate the category and title, making sure its unique
            if (!ValidateCategorization(true))
                return;

            ushort[] frames = null;

            if (radioAnimated.Checked)
            {
                // Generate the frames
                var framesText = txtFrames.Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                frames = new ushort[framesText.Length];
                for (int i = 0; i < framesText.Length; i++)
                {
                    // First check if it was entered as by the index
                    if (!ushort.TryParse(framesText[i], out frames[i]))
                    {
                        // Support it being entered by category
                        int lastPeriod = framesText[i].LastIndexOf('.');
                        string category = framesText[i].Substring(0, lastPeriod);
                        string title = framesText[i].Substring(lastPeriod + 1);
                        GrhData tempGD = GrhInfo.GetData(category, title);
                        if (tempGD != null)
                            frames[i] = tempGD.GrhIndex;
                    }
                }

                // Check that all the frames are valid
                foreach (ushort frame in frames)
                {
                    if (GrhInfo.GetData(frame) == null)
                    {
                        MessageBox.Show("GrhIndex [" + frame + "] does not exist! Aborting save...");
                        return;
                    }
                }
            }

            // Validate the strings
            ushort newIndex;
            if (ushort.TryParse(txtIndex.Text, out newIndex))
            {
                if (newIndex != _gd.GrhIndex)
                {
                    if (
                        MessageBox.Show("Are you sure you wish to change the index? Changes will not be reflected on maps!",
                                        "Change GrhIndex", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;

                    if (GrhInfo.GetData(newIndex) != null)
                    {
                        MessageBox.Show("Index already in use");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid index specified");
                return;
            }
            if (txtCategory.Text.Length > 0)
                txtCategory.Text = txtCategory.Text.Replace('\\', '/');
            if (txtTexture.Text.Length > 0)
                txtTexture.Text = txtTexture.Text.Replace('\\', '/');

            // Move the array index to the new index
            GrhInfo.GrhDatas.RemoveAt(_gd.GrhIndex);
            GrhInfo.GrhDatas[newIndex] = _gd;

            // Set the information
            if (radioStationary.Checked)
            {
                // Stationary
                ContentManager cm = _gd.ContentManager;
                int x = int.Parse(txtX.Text);
                int y = int.Parse(txtY.Text);
                int w = int.Parse(txtW.Text);
                int h = int.Parse(txtH.Text);
                string textureName = txtTexture.Text;

                // Validate the texture
                try
                {
                    cm.Load<Texture2D>("Grh" + DirSep + textureName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to load texture [" + textureName + "]! Aborting save...\r\n" + ex);
                    return;
                }

                _gd.Load(cm, newIndex, textureName, x, y, w, h, txtCategory.Text, txtTitle.Text);
            }
            else
            {
                // Animated
                float speed = float.Parse(txtSpeed.Text);
                _gd.Load(newIndex, frames, 1f / speed, txtCategory.Text, txtTitle.Text);
            }

            // Set the MapGrhWalls
            var walls = new List<WallEntity>();
            foreach (object o in lstWalls.Items)
            {
                WallEntity wall = o as WallEntity;
                if (wall != null)
                    walls.Add(wall);
            }
            _mapGrhWalls[_gd.GrhIndex] = walls;

            // Save
            Enabled = false;
            GrhInfo.Save(ContentPaths.Dev.Data.Join("grhdata.xml"));
            _mapGrhWalls.Save(ContentPaths.Dev.Data.Join("grhdatawalls.xml"));
            Enabled = true;
            Close();
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            WallEntity wall = Entity.Create<Wall>(Vector2.Zero, 16, 16);
            lstWalls.Items.Add(wall);
            lstWalls.SelectedItem = wall;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        void btnRemove_Click(object sender, EventArgs e)
        {
            WallEntity wall = lstWalls.SelectedItem as WallEntity;
            if (wall == null)
                return;
            lstWalls.Items.Remove(wall);
        }

        void cmbWallType_SelectedIndexChanged(object sender, EventArgs e)
        {
            WallEntity wall = lstWalls.SelectedItem as WallEntity;
            if (wall == null)
                return;

            try
            {
                wall.CollisionType = (CollisionType)cmbWallType.SelectedItem;
            }
            catch
            {
                wall.CollisionType = CollisionType.Full;
                cmbWallType.SelectedItem = CollisionType.Full;
            }
        }

        void EditGrhForm_Load(object sender, EventArgs e)
        {
            // Set the wall types
            cmbWallType.Items.Clear();
            foreach (CollisionType item in Enum.GetValues(typeof(CollisionType)))
            {
                cmbWallType.Items.Add(item);
            }
            cmbWallType.SelectedItem = CollisionType.Full;
        }

        void lstWalls_SelectedIndexChanged(object sender, EventArgs e)
        {
            WallEntity wall = lstWalls.SelectedItem as WallEntity;
            bool isEnabled = (wall != null);

            cmbWallType.Enabled = isEnabled;
            txtWallX.Enabled = isEnabled;
            txtWallY.Enabled = isEnabled;
            txtWallH.Enabled = isEnabled;
            txtWallW.Enabled = isEnabled;
            btnRemove.Enabled = isEnabled;

            if (isEnabled)
            {
                cmbWallType.SelectedItem = wall.CollisionType;
                txtWallX.Text = wall.Position.X.ToString();
                txtWallY.Text = wall.Position.Y.ToString();
                txtWallW.Text = wall.CB.Width.ToString();
                txtWallH.Text = wall.CB.Height.ToString();
            }
        }

        void radioAnimated_CheckedChanged(object sender, EventArgs e)
        {
            if (radioAnimated.Checked)
            {
                radioStationary.Checked = false;
                gbStationary.Visible = false;
                gbAnimated.Visible = true;
            }
        }

        void radioStationary_CheckedChanged(object sender, EventArgs e)
        {
            if (radioStationary.Checked)
            {
                radioAnimated.Checked = false;
                gbStationary.Visible = true;
                gbAnimated.Visible = false;
            }
        }

        void ShowGrhInfo()
        {
            txtCategory.Text = _gd.Category;
            txtTitle.Text = _gd.Title;
            txtIndex.Text = _gd.GrhIndex.ToString();

            if (_gd.Frames == null || _gd.Frames.Length == 1)
            {
                // Stationary
                radioStationary.Checked = true;
                radioAnimated.Checked = false;
                Rectangle r = _gd.GetOriginalSource();
                txtX.Text = r.X.ToString();
                txtY.Text = r.Y.ToString();
                txtW.Text = r.Width.ToString();
                txtH.Text = r.Height.ToString();
                txtTexture.Text = _gd.TextureName;
            }
            else
            {
                // Animated
                radioStationary.Checked = false;
                radioAnimated.Checked = true;
                txtFrames.Text = string.Empty;
                for (int i = 0; i < _gd.Frames.Length; i++)
                {
                    txtFrames.Text += _gd.Frames[i].GrhIndex + "\r\n";
                }
                txtSpeed.Text = (1f / _gd.Speed).ToString();
            }

            // Bound walls
            lstWalls.Items.Clear();
            var walls = _mapGrhWalls[_gd.GrhIndex];
            if (walls != null)
            {
                foreach (WallEntity wall in walls)
                {
                    lstWalls.Items.Add(wall);
                }
            }
        }

        void txtCategory_TextChanged(object sender, EventArgs e)
        {
            if (txtCategory.Text == _gd.Category)
                txtCategory.BackColor = ColorNormal;
            else
                txtCategory.BackColor = ColorChanged;
        }

        void txtH_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (uint.TryParse(txtH.Text, out o))
            {
                if (o == _gd.GetOriginalSource().Height)
                    txtH.BackColor = ColorNormal;
                else
                    txtH.BackColor = ColorChanged;
            }
            else
                txtH.BackColor = ColorError;
        }

        void txtIndex_TextChanged(object sender, EventArgs e)
        {
            ushort o;
            if (ushort.TryParse(txtIndex.Text, out o))
            {
                if (o == _gd.GrhIndex)
                    txtIndex.BackColor = ColorNormal;
                else
                    txtIndex.BackColor = ColorChanged;
            }
            else
                txtIndex.BackColor = ColorError;
        }

        void txtTexture_TextChanged(object sender, EventArgs e)
        {
            if (txtTexture.Text == _gd.TextureName)
                txtTexture.BackColor = ColorNormal;
            else
            {
                try
                {
                    ContentManager cm = _gd.ContentManager;
                    cm.Load<Texture2D>("Grh" + DirSep + txtTexture.Text);
                    txtTexture.BackColor = ColorChanged;
                }
                catch
                {
                    txtTexture.BackColor = ColorError;
                }
            }
        }

        void txtTitle_KeyDown(object sender, KeyEventArgs e)
        {
            // Forward to "Accept"
            if (e.KeyCode == Keys.Enter)
                btnAccept_Click(this, new EventArgs());
        }

        void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (txtTitle.Text == _gd.Title)
                txtTitle.BackColor = ColorNormal;
            else
                txtTitle.BackColor = ColorChanged;
        }

        void txtW_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (uint.TryParse(txtW.Text, out o))
            {
                if (o == _gd.GetOriginalSource().Width)
                    txtW.BackColor = ColorNormal;
                else
                    txtW.BackColor = ColorChanged;
            }
            else
                txtW.BackColor = ColorError;
        }

        void txtWallH_TextChanged(object sender, EventArgs e)
        {
            WallEntity wall = lstWalls.SelectedItem as WallEntity;
            if (wall == null)
                return;

            try
            {
                wall.Resize(new Vector2(wall.Size.X, float.Parse(txtWallH.Text)));
                txtWallH.BackColor = ColorNormal;
            }
            catch
            {
                txtWallH.BackColor = ColorError;
            }
        }

        void txtWallW_TextChanged(object sender, EventArgs e)
        {
            WallEntity wall = lstWalls.SelectedItem as WallEntity;
            if (wall == null)
                return;

            try
            {
                wall.Resize(new Vector2(float.Parse(txtWallW.Text), wall.Size.Y));
                txtWallW.BackColor = ColorNormal;
            }
            catch
            {
                txtWallW.BackColor = ColorError;
            }
        }

        void txtWallX_TextChanged(object sender, EventArgs e)
        {
            WallEntity wall = lstWalls.SelectedItem as WallEntity;
            if (wall == null)
                return;

            try
            {
                wall.Position = new Vector2(float.Parse(txtWallX.Text), wall.CB.Min.Y);
                txtWallX.BackColor = ColorNormal;
            }
            catch
            {
                txtWallX.BackColor = ColorError;
            }
        }

        void txtWallY_TextChanged(object sender, EventArgs e)
        {
            WallEntity wall = lstWalls.SelectedItem as WallEntity;
            if (wall == null)
                return;

            try
            {
                wall.Position = new Vector2(wall.CB.Min.X, float.Parse(txtWallY.Text));
                txtWallY.BackColor = ColorNormal;
            }
            catch
            {
                txtWallY.BackColor = ColorError;
            }
        }

        void txtX_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (uint.TryParse(txtX.Text, out o))
            {
                if (o == _gd.GetOriginalSource().X)
                    txtX.BackColor = ColorNormal;
                else
                    txtX.BackColor = ColorChanged;
            }
            else
                txtX.BackColor = ColorError;
        }

        void txtY_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (uint.TryParse(txtY.Text, out o))
            {
                if (o == _gd.GetOriginalSource().Y)
                    txtY.BackColor = ColorNormal;
                else
                    txtY.BackColor = ColorChanged;
            }
            else
                txtY.BackColor = ColorError;
        }

        bool ValidateCategorization(bool showMessage)
        {
            GrhData gd = GrhInfo.GetData(txtCategory.Text, txtTitle.Text);
            if (gd != null && gd != _gd)
            {
                if (showMessage)
                {
                    MessageBox.Show("A GrhData with the given title and category already exists." +
                                    "Each GrhData must contain a unique title and category combination.");
                }
                return false;
            }
            return true;
        }
    }
}