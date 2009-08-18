using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.EditorTools;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    public partial class EditGrhForm : Form
    {
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

            GrhIndex[] frames = null;

            if (radioAnimated.Checked)
            {
                // Generate the frames
                var framesText = txtFrames.Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                frames = new GrhIndex[framesText.Length];
                for (int i = 0; i < framesText.Length; i++)
                {
                    // First check if it was entered as by the index
                    if (!GrhIndex.TryParse(framesText[i], out frames[i]))
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
                foreach (GrhIndex frame in frames)
                {
                    if (GrhInfo.GetData(frame) == null)
                    {
                        MessageBox.Show("GrhIndex [" + frame + "] does not exist! Aborting save...");
                        return;
                    }
                }
            }

            // Validate the strings
            GrhIndex newIndex;
            if (GrhIndex.TryParse(txtIndex.Text, out newIndex))
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
                bool autoSize = chkAutoSize.Checked;

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
                _gd.AutomaticSize = autoSize;
            }
            else
            {
                // Animated
                float speed = float.Parse(txtSpeed.Text);
                _gd.Load(newIndex, frames, 1f / speed, txtCategory.Text, txtTitle.Text);
            }

            // Set the MapGrhWalls
            var walls = new List<WallEntityBase>();
            foreach (object o in lstWalls.Items)
            {
                WallEntityBase wall = o as WallEntityBase;
                if (wall != null)
                    walls.Add(wall);
            }
            _mapGrhWalls[_gd] = walls;

            // Save
            Enabled = false;
            GrhInfo.Save(ContentPaths.Dev.Data.Join("grhdata.xml"));
            _mapGrhWalls.Save(ContentPaths.Dev.Data.Join("grhdatawalls.xml"));
            Enabled = true;
            Close();
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            WallEntity wall = new WallEntity(Vector2.Zero, new Vector2(16));
            lstWalls.AddItemAndReselect(wall);
            lstWalls.SelectedItem = wall;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        void btnRemove_Click(object sender, EventArgs e)
        {
            WallEntityBase wall = lstWalls.SelectedItem as WallEntityBase;
            if (wall == null)
                return;
            lstWalls.RemoveItemAndReselect(wall);
        }

        void chkAutoSize_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = !chkAutoSize.Checked;

            txtX.Enabled = enabled;
            txtY.Enabled = enabled;
            txtW.Enabled = enabled;
            txtH.Enabled = enabled;
        }

        void cmbWallType_SelectedIndexChanged(object sender, EventArgs e)
        {
            WallEntityBase wall = lstWalls.SelectedItem as WallEntityBase;
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
            foreach (CollisionType item in EnumHelper.GetValues<CollisionType>())
            {
                cmbWallType.Items.Add(item);
            }
            cmbWallType.SelectedItem = CollisionType.Full;
        }

        void lstWalls_SelectedIndexChanged(object sender, EventArgs e)
        {
            WallEntityBase wall = lstWalls.SelectedItem as WallEntityBase;
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
            chkAutoSize.Checked = _gd.AutomaticSize;

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
            var walls = _mapGrhWalls[_gd];
            if (walls != null)
            {
                foreach (WallEntityBase wall in walls)
                {
                    lstWalls.AddItemAndReselect(wall);
                }
            }
        }

        void txtCategory_TextChanged(object sender, EventArgs e)
        {
            if (txtCategory.Text == _gd.Category)
                txtCategory.BackColor = EditorColors.Normal;
            else
                txtCategory.BackColor = EditorColors.Changed;
        }

        void txtH_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (uint.TryParse(txtH.Text, out o))
            {
                if (o == _gd.GetOriginalSource().Height)
                    txtH.BackColor = EditorColors.Normal;
                else
                    txtH.BackColor = EditorColors.Changed;
            }
            else
                txtH.BackColor = EditorColors.Error;
        }

        void txtIndex_TextChanged(object sender, EventArgs e)
        {
            ushort o;
            if (ushort.TryParse(txtIndex.Text, out o))
            {
                if (o == _gd.GrhIndex)
                    txtIndex.BackColor = EditorColors.Normal;
                else
                    txtIndex.BackColor = EditorColors.Changed;
            }
            else
                txtIndex.BackColor = EditorColors.Error;
        }

        void txtTexture_TextChanged(object sender, EventArgs e)
        {
            if (txtTexture.Text == _gd.TextureName)
                txtTexture.BackColor = EditorColors.Normal;
            else
            {
                try
                {
                    ContentManager cm = _gd.ContentManager;
                    cm.Load<Texture2D>("Grh" + DirSep + txtTexture.Text);
                    txtTexture.BackColor = EditorColors.Changed;
                }
                catch
                {
                    txtTexture.BackColor = EditorColors.Error;
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
                txtTitle.BackColor = EditorColors.Normal;
            else
                txtTitle.BackColor = EditorColors.Changed;
        }

        void txtW_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (uint.TryParse(txtW.Text, out o))
            {
                if (o == _gd.GetOriginalSource().Width)
                    txtW.BackColor = EditorColors.Normal;
                else
                    txtW.BackColor = EditorColors.Changed;
            }
            else
                txtW.BackColor = EditorColors.Error;
        }

        void txtWallH_TextChanged(object sender, EventArgs e)
        {
            WallEntityBase wall = lstWalls.SelectedItem as WallEntityBase;
            if (wall == null)
                return;

            try
            {
                wall.Resize(new Vector2(wall.Size.X, float.Parse(txtWallH.Text)));
                txtWallH.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtWallH.BackColor = EditorColors.Error;
            }
        }

        void txtWallW_TextChanged(object sender, EventArgs e)
        {
            WallEntityBase wall = lstWalls.SelectedItem as WallEntityBase;
            if (wall == null)
                return;

            try
            {
                wall.Resize(new Vector2(float.Parse(txtWallW.Text), wall.Size.Y));
                txtWallW.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtWallW.BackColor = EditorColors.Error;
            }
        }

        void txtWallX_TextChanged(object sender, EventArgs e)
        {
            WallEntityBase wall = lstWalls.SelectedItem as WallEntityBase;
            if (wall == null)
                return;

            try
            {
                wall.Position = new Vector2(float.Parse(txtWallX.Text), wall.CB.Min.Y);
                txtWallX.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtWallX.BackColor = EditorColors.Error;
            }
        }

        void txtWallY_TextChanged(object sender, EventArgs e)
        {
            WallEntityBase wall = lstWalls.SelectedItem as WallEntityBase;
            if (wall == null)
                return;

            try
            {
                wall.Position = new Vector2(wall.CB.Min.X, float.Parse(txtWallY.Text));
                txtWallY.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtWallY.BackColor = EditorColors.Error;
            }
        }

        void txtX_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (uint.TryParse(txtX.Text, out o))
            {
                if (o == _gd.GetOriginalSource().X)
                    txtX.BackColor = EditorColors.Normal;
                else
                    txtX.BackColor = EditorColors.Changed;
            }
            else
                txtX.BackColor = EditorColors.Error;
        }

        void txtY_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (uint.TryParse(txtY.Text, out o))
            {
                if (o == _gd.GetOriginalSource().Y)
                    txtY.BackColor = EditorColors.Normal;
                else
                    txtY.BackColor = EditorColors.Changed;
            }
            else
                txtY.BackColor = EditorColors.Error;
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