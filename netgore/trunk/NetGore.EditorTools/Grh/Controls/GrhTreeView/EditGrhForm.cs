using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.IO;
using Point=System.Drawing.Point;

namespace NetGore.EditorTools
{
    public partial class EditGrhForm : Form
    {
        /// <summary>
        /// The default amount the camera is zoomed in on the preview of the <see cref="GrhData"/> being edited.
        /// </summary>
        const float _defaultZoomLevel = 4f;

        static readonly Color _autoWallColor = new Color(255, 255, 255, 150);

        /// <summary>
        /// Character used to separate directories
        /// </summary>
        static readonly char DirSep = Path.DirectorySeparatorChar;

        readonly ICamera2D _camera;

        readonly CreateWallEntityHandler _createWall;
        readonly GrhData _gd;
        readonly Grh _grh;
        readonly MapGrhWalls _mapGrhWalls;
        readonly Stopwatch _stopwatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditGrhForm"/> class.
        /// </summary>
        /// <param name="gd">The <see cref="GrhData"/> to edit.</param>
        /// <param name="mapGrhWalls">The <see cref="MapGrhWalls"/> instance.</param>
        /// <param name="createWall">Delegate describing how to create wall instances.</param>
        /// <param name="screenSize">Size of the screen used to display the preview.</param>
        public EditGrhForm(GrhData gd, MapGrhWalls mapGrhWalls, CreateWallEntityHandler createWall, Vector2 screenSize)
        {
            if (gd == null)
                throw new ArgumentNullException("gd");
            if (mapGrhWalls == null)
                throw new ArgumentNullException("mapGrhWalls");
            if (createWall == null)
                throw new ArgumentNullException("createWall");

            WasCanceled = false;

            // Set the local members
            _createWall = createWall;
            _gd = gd;
            _mapGrhWalls = mapGrhWalls;

            // Set up the camera
            Vector2 pos;
            try
            {
                pos = gd.Size / 2f;
            }
            catch (ContentLoadException)
            {
                pos = new Vector2(32f) / 2f;
            }

            _camera = new Camera2D(screenSize);
            _camera.Zoom(pos, screenSize, _defaultZoomLevel);

            // Set up the rest of the stuff
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            _grh = new Grh(gd, AnimType.Loop, (int)_stopwatch.ElapsedMilliseconds);

            Location = new Point(0, 0);

            InitializeComponent();
            ShowGrhInfo();
        }

        public ICamera2D Camera
        {
            get { return _camera; }
        }

        public Grh Grh
        {
            get { return _grh; }
        }

        /// <summary>
        /// Gets the <see cref="GrhData"/> being edited by this form.
        /// </summary>
        public GrhData GrhData
        {
            get { return _gd; }
        }

        /// <summary>
        /// Gets if this form was closed by pressing "Cancel".
        /// </summary>
        public bool WasCanceled { get; private set; }

        void btnAccept_Click(object sender, EventArgs e)
        {
            // TODO: Clean this up a lot by adding more specialized textboxes, and using .IsValid()

            // Validate the category and title, making sure its unique
            if (!ValidateCategorization(true))
                return;

            GrhIndex[] frames = null;

            if (radioAnimated.Checked)
            {
                // Generate the frames
                var framesText = txtFrames.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                frames = new GrhIndex[framesText.Length];
                for (int i = 0; i < framesText.Length; i++)
                {
                    // First check if it was entered as by the index
                    if (!Parser.Current.TryParse(framesText[i], out frames[i]))
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
            if (Parser.Current.TryParse(txtIndex.Text, out newIndex))
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

            // Get the categorization
            var categorization = new SpriteCategorization(txtCategory.GetSanitizedText(), txtTitle.Text);

            // Set the information
            if (radioStationary.Checked)
            {
                // Stationary
                ContentManager cm = _gd.ContentManager;
                int x = Parser.Current.ParseInt(txtX.Text);
                int y = Parser.Current.ParseInt(txtY.Text);
                int w = Parser.Current.ParseInt(txtW.Text);
                int h = Parser.Current.ParseInt(txtH.Text);
                string textureName = txtTexture.GetSanitizedText();
                bool autoSize = chkAutoSize.Checked;

                // Validate the texture
                try
                {
                    cm.Load<Texture2D>("Grh" + DirSep + textureName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to load texture [" + textureName + "]! Aborting save..." + Environment.NewLine + ex);
                    return;
                }

                _gd.ChangeTexture(textureName, new Rectangle(x, y, w, h));
                _gd.SetCategorization(categorization);
                _gd.AutomaticSize = autoSize;
            }
            else
            {
                // Animated
                float speed = Parser.Current.ParseFloat(txtSpeed.Text);
                _gd.Load(newIndex, frames, 1f / speed, categorization);
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

            // Write
            Enabled = false;
            GrhInfo.Save(ContentPaths.Dev);
            _mapGrhWalls.Save(ContentPaths.Dev);
            Enabled = true;

            WasCanceled = false;
            Close();
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            var wall = _createWall(Vector2.Zero, new Vector2(16));
            lstWalls.AddItemAndReselect(wall);
            lstWalls.SelectedItem = wall;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            WasCanceled = true;
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

        public void Draw(SpriteBatch sb)
        {
            // Update the Grh first
            _grh.Update((int)_stopwatch.ElapsedMilliseconds);

            // Begin rendering
            sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, _camera.Matrix);

            // Try/catch since invalid texture will throw an Exception that we will want to ignore in releases
#if !DEBUG
            try
            {
#endif
            _grh.Draw(sb, Vector2.Zero);
#if !DEBUG
            }
            catch
            {
            }
#endif

            // Draw the walls
            foreach (var wall in lstWalls.Items.OfType<WallEntityBase>())
            {
                var rect = wall.ToRectangle();
                XNARectangle.Draw(sb, rect, _autoWallColor);
            }

            // End rendering
            sb.End();
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
                txtWallX.Text = wall.Position.X.ToString();
                txtWallY.Text = wall.Position.Y.ToString();
                txtWallW.Text = wall.Size.X.ToString();
                txtWallH.Text = wall.Size.Y.ToString();
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
            txtCategory.ChangeTextToDefault(_gd.Categorization.Category.ToString(), true);
            txtTitle.Text = _gd.Categorization.Title.ToString();
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
                txtTexture.ChangeTextToDefault(_gd.TextureName.ToString(), true);
            }
            else
            {
                // Animated
                radioStationary.Checked = false;
                radioAnimated.Checked = true;
                txtFrames.Text = string.Empty;
                for (int i = 0; i < _gd.Frames.Length; i++)
                {
                    txtFrames.Text += _gd.Frames[i].GrhIndex + Environment.NewLine;
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

        void txtH_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (Parser.Current.TryParse(txtH.Text, out o))
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
            if (Parser.Current.TryParse(txtIndex.Text, out o))
            {
                if (o == _gd.GrhIndex)
                    txtIndex.BackColor = EditorColors.Normal;
                else
                    txtIndex.BackColor = EditorColors.Changed;
            }
            else
                txtIndex.BackColor = EditorColors.Error;
        }

        void txtTitle_KeyDown(object sender, KeyEventArgs e)
        {
            // Forward to "Accept"
            if (e.KeyCode == Keys.Enter)
                btnAccept_Click(this, new EventArgs());
        }

        void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (txtTitle.Text == _gd.Categorization.Title.ToString())
                txtTitle.BackColor = EditorColors.Normal;
            else
                txtTitle.BackColor = EditorColors.Changed;
        }

        void txtW_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (Parser.Current.TryParse(txtW.Text, out o))
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
                wall.Resize(new Vector2(wall.Size.X, Parser.Current.ParseFloat(txtWallH.Text)));
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
                wall.Resize(new Vector2(Parser.Current.ParseFloat(txtWallW.Text), wall.Size.Y));
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
                wall.Position = new Vector2(Parser.Current.ParseFloat(txtWallX.Text), wall.Position.Y);
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
                wall.Position = new Vector2(wall.Position.X, Parser.Current.ParseFloat(txtWallY.Text));
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
            if (Parser.Current.TryParse(txtX.Text, out o))
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
            if (Parser.Current.TryParse(txtY.Text, out o))
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
            GrhData gd = GrhInfo.GetData(txtCategory.GetSanitizedText(), txtTitle.Text);
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