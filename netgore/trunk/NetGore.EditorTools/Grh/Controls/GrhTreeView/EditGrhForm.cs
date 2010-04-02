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
        static readonly Color _autoWallColor = new Color(255, 255, 255, 150);

        /// <summary>
        /// Character used to separate directories
        /// </summary>
        static readonly char DirSep = Path.DirectorySeparatorChar;

        readonly ICamera2D _camera;

        readonly CreateWallEntityHandler _createWall;

        readonly Grh _grh;
        readonly MapGrhWalls _mapGrhWalls;
        readonly Stopwatch _stopwatch;
        GrhData _gd;

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
            if (gd is AutomaticAnimatedGrhData)
                throw new ArgumentException("Cannot edit an AutomaticAnimatedGrhData.", "gd");

            WasCanceled = false;

            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            // Set the local members
            _createWall = createWall;
            _gd = gd;
            _mapGrhWalls = mapGrhWalls;

            _grh = new Grh(gd, AnimType.Loop, (int)_stopwatch.ElapsedMilliseconds);

            // Set up the camera
            Vector2 pos;
            try
            {
                pos = gd.Size / 2f;
            }
            catch (ContentLoadException)
            {
                Close();
                return;
            }

            _camera = new Camera2D(screenSize);
            Camera.Zoom(pos, screenSize, Camera.GetFillScreenZoomLevel(_grh.Size));

            // Add a padding around the screen around each side of the item
            Camera.Scale -= Camera.Scale * _defaultViewPaddingPercent;
            Camera.Translate(_grh.Size * (1f / Camera.Scale) * -(_defaultViewPaddingPercent / 2f));

            Location = new Point(0, 0);
            InitializeComponent();
            ShowGrhInfo();
        }

        /// <summary>
        /// The percent of the screen to use as padding around the item veing viewed. This way the item doesn't
        /// actually stretch all the way out to the sides.
        /// </summary>
        const float _defaultViewPaddingPercent = 0.05f;

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
            StationaryGrhData gdStationary = _gd as StationaryGrhData;
            AnimatedGrhData gdAnimated = _gd as AnimatedGrhData;

            // Validate the category and title, making sure its unique
            if (!ValidateCategorization(true))
                return;

            if (radioAnimated.Checked)
            {
                // Generate the frames
                var framesText = txtFrames.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                var frames = new GrhIndex[framesText.Length];
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
                if (gdStationary == null)
                {
                    MessageBox.Show("For some reason, could not cast the GrhData to StationaryGrhData...");
                    return;
                }

                // Stationary
                ContentManager cm = gdStationary.ContentManager;
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

                gdStationary.ChangeTexture(textureName, new Rectangle(x, y, w, h));
                _gd.SetCategorization(categorization);
                gdStationary.AutomaticSize = autoSize;
            }
            else
            {
                // Animated
                if (gdAnimated == null)
                {
                    MessageBox.Show("For some reason, could not cast the GrhData to AnimatedGrhData...");
                    return;
                }

                float speed = Parser.Current.ParseFloat(txtSpeed.Text);
                gdAnimated.SetSpeed(speed);
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

        void chkPlatform_CheckedChanged(object sender, EventArgs e)
        {
            WallEntityBase wall = lstWalls.SelectedItem as WallEntityBase;
            if (wall == null)
                return;

            wall.IsPlatform = chkPlatform.Checked;
        }

        public void Draw(ISpriteBatch sb)
        {
            // Update the Grh first
            _grh.Update((int)_stopwatch.ElapsedMilliseconds);

            // Begin rendering
            sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, Camera.Matrix);

            try
            {
                _grh.Draw(sb, Vector2.Zero);

                // Draw the walls
                foreach (var wall in lstWalls.Items.OfType<WallEntityBase>())
                {
                    var rect = wall.ToRectangle();
                    XNARectangle.Draw(sb, rect, _autoWallColor);
                }
            }
            finally
            {
                // End rendering
                sb.End();
            }
        }

        void lstWalls_SelectedIndexChanged(object sender, EventArgs e)
        {
            WallEntityBase wall = lstWalls.SelectedItem as WallEntityBase;
            bool isEnabled = (wall != null);

            chkPlatform.Enabled = isEnabled;
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
                chkPlatform.Checked = wall.IsPlatform;
            }
        }

        void radioAnimated_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioAnimated.Checked)
                return;

            if (!(_gd is AnimatedGrhData))
            {
                const string msg = "Are you sure you wish to convert this GrhData to animated? Most GrhData values will be lost.";
                const string cap = "Convert to animated?";
                if (MessageBox.Show(msg, cap, MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                var newGD = GrhInfo.ReplaceExistingWithAnimated(_gd.GrhIndex);
                if (newGD == null)
                {
                    MessageBox.Show("Conversion to animated failed for some reason...");
                    return;
                }

                _gd = newGD;
            }

            radioStationary.Checked = false;
            gbStationary.Visible = false;
            gbAnimated.Visible = true;
        }

        void radioStationary_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioStationary.Checked)
                return;

            if (!(_gd is StationaryGrhData))
            {
                const string msg =
                    "Are you sure you wish to convert this GrhData to stationary? Most GrhData values will be lost.";
                const string cap = "Convert to stationary?";
                if (MessageBox.Show(msg, cap, MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                var newGD = GrhInfo.ReplaceExistingWithStationary(_gd.GrhIndex);
                if (newGD == null)
                {
                    MessageBox.Show("Conversion to stationary failed for some reason...");
                    return;
                }

                _gd = newGD;
            }

            radioAnimated.Checked = false;
            gbStationary.Visible = true;
            gbAnimated.Visible = false;
        }

        void ShowGrhInfo()
        {
            txtCategory.ChangeTextToDefault(_gd.Categorization.Category.ToString(), true);
            txtTitle.Text = _gd.Categorization.Title.ToString();
            txtIndex.Text = _gd.GrhIndex.ToString();

            // Show the type-specific info
            if (_gd is StationaryGrhData)
                ShowGrhInfoForStationary((StationaryGrhData)_gd);
            else if (_gd is AnimatedGrhData)
                ShowGrhInfoForAnimated(_gd);
            else
                throw new UnsupportedGrhDataTypeException(_gd);

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

        void ShowGrhInfoForAnimated(GrhData grhData)
        {
            radioStationary.Checked = false;
            radioAnimated.Checked = true;
            txtFrames.Text = string.Empty;

            for (int i = 0; i < grhData.FramesCount; i++)
            {
                var frame = grhData.GetFrame(i);
                if (frame != null)
                    txtFrames.Text += frame.GrhIndex + Environment.NewLine;
            }

            txtSpeed.Text = (1f / grhData.Speed).ToString();
        }

        void ShowGrhInfoForStationary(StationaryGrhData grhData)
        {
            // Stationary
            chkAutoSize.Checked = grhData.AutomaticSize;
            radioStationary.Checked = true;
            radioAnimated.Checked = false;
            Rectangle r = grhData.GetOriginalSource();
            txtX.Text = r.X.ToString();
            txtY.Text = r.Y.ToString();
            txtW.Text = r.Width.ToString();
            txtH.Text = r.Height.ToString();
            txtTexture.ChangeTextToDefault(grhData.TextureName.ToString(), true);
        }

        void txtH_TextChanged(object sender, EventArgs e)
        {
            var asStationary = _gd as StationaryGrhData;
            if (asStationary == null)
                return;

            uint o;
            if (Parser.Current.TryParse(txtH.Text, out o))
            {
                if (o == asStationary.GetOriginalSource().Height)
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
            var asStationary = _gd as StationaryGrhData;
            if (asStationary == null)
                return;

            uint o;
            if (Parser.Current.TryParse(txtW.Text, out o))
            {
                if (o == asStationary.GetOriginalSource().Width)
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
            var asStationary = _gd as StationaryGrhData;
            if (asStationary == null)
                return;

            uint o;
            if (Parser.Current.TryParse(txtX.Text, out o))
            {
                if (o == asStationary.GetOriginalSource().X)
                    txtX.BackColor = EditorColors.Normal;
                else
                    txtX.BackColor = EditorColors.Changed;
            }
            else
                txtX.BackColor = EditorColors.Error;
        }

        void txtY_TextChanged(object sender, EventArgs e)
        {
            var asStationary = _gd as StationaryGrhData;
            if (asStationary == null)
                return;

            uint o;
            if (Parser.Current.TryParse(txtY.Text, out o))
            {
                if (o == asStationary.GetOriginalSource().Y)
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