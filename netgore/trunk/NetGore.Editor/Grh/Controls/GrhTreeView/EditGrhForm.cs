using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NetGore.Content;
using WeifenLuo.WinFormsUI.Docking;
using NetGore.Graphics;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Editor.Grhs
{
    public partial class EditGrhForm : DockContent
    {
        /// <summary>
        /// Character used to separate directories
        /// </summary>
        static readonly char DirSep = Path.DirectorySeparatorChar;

        readonly CreateWallEntityHandler _createWall;
        readonly Grh _grh;
        readonly MapGrhWalls _mapGrhWalls;

        GrhData _gd;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditGrhForm"/> class.
        /// </summary>
        /// <param name="gd">The <see cref="GrhData"/> to edit.</param>
        /// <param name="mapGrhWalls">The <see cref="MapGrhWalls"/> instance.</param>
        /// <param name="createWall">Delegate describing how to create wall instances.</param>
        /// <param name="deleteOnCancel">If the <see cref="GrhData"/> will be deleted if the editing is canceled.</param>
        /// <exception cref="ArgumentNullException"><paramref name="gd" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="mapGrhWalls" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="createWall" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Cannot edit an <see cref="AutomaticAnimatedGrhData"/>.</exception>
        public EditGrhForm(GrhData gd, MapGrhWalls mapGrhWalls, CreateWallEntityHandler createWall, bool deleteOnCancel)
        {
            if (gd == null)
                throw new ArgumentNullException("gd");
            if (mapGrhWalls == null)
                throw new ArgumentNullException("mapGrhWalls");
            if (createWall == null)
                throw new ArgumentNullException("createWall");
            if (gd is AutomaticAnimatedGrhData)
                throw new ArgumentException("Cannot edit an AutomaticAnimatedGrhData.", "gd");

            DeleteOnCancel = deleteOnCancel;
            WasCanceled = false;

            // Set the local members
            _createWall = createWall;
            _gd = gd;
            _mapGrhWalls = mapGrhWalls;

            _grh = new Grh(gd, AnimType.Loop, TickCount.Now);

            InitializeComponent();

            ShowGrhInfo();
        }

        /// <summary>
        /// Gets the <see cref="WallEntityBase"/>s that are bound to the <see cref="GrhData"/> being edited.
        /// </summary>
        public IEnumerable<WallEntityBase> BoundWalls
        {
            get { return lstWalls.Items.OfType<WallEntityBase>(); }
        }

        /// <summary>
        /// Gets if the <see cref="GrhData"/> being edited will be deleted when the editing is canceled.
        /// </summary>
        public bool DeleteOnCancel { get; set; }

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

        /// <summary>
        /// Handles when one of the bound walls have changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.EventArgs{Vector2}"/> instance containing the event data.</param>
        void BoundWallChanged(ISpatial sender, EventArgs<Vector2> e)
        {
            var index = lstWalls.Items.IndexOf(sender);
            if (index < 0)
                return;

            lstWalls.RefreshItemAt(index);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (DesignMode)
                return;

            if (e.Cancel)
                return;

            // Remove the event hooks from the walls
            var walls = _mapGrhWalls[_gd];
            if (walls != null)
            {
                foreach (var wall in walls)
                {
                    wall.Moved -= BoundWallChanged;
                    wall.Resized -= BoundWallChanged;
                }
            }

            // Delete the GrhData if the form was canceled
            if (DeleteOnCancel && DialogResult != DialogResult.OK)
                GrhInfo.Delete(GrhData);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
                return;

            screen.Grh = Grh;
            screen.Walls = BoundWalls;
        }

        /// <summary>
        /// Shows the information for the <see cref="_gd"/>.
        /// </summary>
        /// <exception cref="UnsupportedGrhDataTypeException">The <see cref="_gd"/> is not of a supported type.</exception>
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
                foreach (var wall in walls)
                {
                    lstWalls.AddItemAndReselect(wall);

                    wall.Moved -= BoundWallChanged;
                    wall.Moved += BoundWallChanged;

                    wall.Resized -= BoundWallChanged;
                    wall.Resized += BoundWallChanged;
                }
            }
        }

        void ShowGrhInfoForAnimated(GrhData grhData)
        {
            radioStationary.Checked = false;
            radioAnimated.Checked = true;
            txtFrames.Text = string.Empty;

            for (var i = 0; i < grhData.FramesCount; i++)
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
            var r = grhData.OriginalSourceRect;
            txtX.Text = r.X.ToString();
            txtY.Text = r.Y.ToString();
            txtW.Text = r.Width.ToString();
            txtH.Text = r.Height.ToString();
            txtTexture.ChangeTextToDefault(grhData.TextureName.ToString(), true);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GrhData")]
        bool ValidateCategorization(bool showMessage)
        {
            var gd = GrhInfo.GetData(txtCategory.GetSanitizedText(), txtTitle.Text);
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

        /// <summary>
        /// Handles the Click event of the btnAccept control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "StationaryGrhData")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GrhIndex")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GrhData")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AnimatedGrhData")]
        void btnAccept_Click(object sender, EventArgs e)
        {
            var gdStationary = _gd as StationaryGrhData;
            var gdAnimated = _gd as AnimatedGrhData;

            // Validate the category and title, making sure its unique
            if (!ValidateCategorization(true))
                return;

            if (radioAnimated.Checked)
            {
                // Generate the frames
                var framesText = txtFrames.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                var frames = new GrhIndex[framesText.Length];
                for (var i = 0; i < framesText.Length; i++)
                {
                    // First check if it was entered as by the index
                    if (!Parser.Current.TryParse(framesText[i], out frames[i]))
                    {
                        // Support it being entered by category
                        var lastPeriod = framesText[i].LastIndexOf('.');
                        var category = framesText[i].Substring(0, lastPeriod);
                        var title = framesText[i].Substring(lastPeriod + 1);
                        var tempGD = GrhInfo.GetData(category, title);
                        if (tempGD != null)
                            frames[i] = tempGD.GrhIndex;
                    }
                }

                // Check that all the frames are valid
                foreach (var frame in frames)
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
                var cm = gdStationary.ContentManager;
                var x = Parser.Current.ParseInt(txtX.Text);
                var y = Parser.Current.ParseInt(txtY.Text);
                var w = Parser.Current.ParseInt(txtW.Text);
                var h = Parser.Current.ParseInt(txtH.Text);
                var textureName = txtTexture.GetSanitizedText();
                var autoSize = chkAutoSize.Checked;

                // Validate the texture
                try
                {
                    cm.LoadImage("Grh" + DirSep + textureName, ContentLevel.Map);
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

                var speed = Parser.Current.ParseFloat(txtSpeed.Text);
                gdAnimated.SetSpeed(speed);
            }

            // Set the MapGrhWalls
            _mapGrhWalls[_gd] = BoundWalls.ToList();

            // Write
            Enabled = false;
            GrhInfo.Save(ContentPaths.Dev);
            Enabled = true;

            WasCanceled = false;

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAdd_Click(object sender, EventArgs e)
        {
            var wall = _createWall(Vector2.Zero, new Vector2(16));
            lstWalls.AddItemAndReselect(wall);
            lstWalls.SelectedItem = wall;

            wall.Moved -= BoundWallChanged;
            wall.Moved += BoundWallChanged;

            wall.Resized -= BoundWallChanged;
            wall.Resized += BoundWallChanged;
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            WasCanceled = true;
            Close();

            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Handles the Click event of the btnRemove control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnRemove_Click(object sender, EventArgs e)
        {
            var wall = lstWalls.SelectedItem as WallEntityBase;
            if (wall == null)
                return;

            lstWalls.RemoveItemAndReselect(wall);
            wall.Moved -= BoundWallChanged;
            wall.Resized -= BoundWallChanged;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkAutoSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void chkAutoSize_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = !chkAutoSize.Checked;

            txtX.Enabled = enabled;
            txtY.Enabled = enabled;
            txtW.Enabled = enabled;
            txtH.Enabled = enabled;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkPlatform control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void chkPlatform_CheckedChanged(object sender, EventArgs e)
        {
            var wall = lstWalls.SelectedItem as WallEntityBase;
            if (wall == null)
                return;

            wall.IsPlatform = chkPlatform.Checked;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstWalls control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstWalls_SelectedIndexChanged(object sender, EventArgs e)
        {
            var wall = lstWalls.SelectedItem as WallEntityBase;
            var isEnabled = (wall != null);

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

        /// <summary>
        /// Handles the CheckedChanged event of the radioAnimated control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GrhData")]
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

        /// <summary>
        /// Handles the CheckedChanged event of the radioStationary control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GrhData")]
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

        /// <summary>
        /// Handles the Tick event of the tmrUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void tmrUpdate_Tick(object sender, EventArgs e)
        {
            screen.InvokeDrawing(sender, EventArgsHelper.Create(TickCount.Now));
        }

        /// <summary>
        /// Handles the TextChanged event of the txtH control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtH_TextChanged(object sender, EventArgs e)
        {
            var asStationary = _gd as StationaryGrhData;
            if (asStationary == null)
                return;

            uint o;
            if (Parser.Current.TryParse(txtH.Text, out o))
            {
                if (o == asStationary.OriginalSourceRect.Height)
                    txtH.BackColor = EditorColors.Normal;
                else
                    txtH.BackColor = EditorColors.Changed;
            }
            else
                txtH.BackColor = EditorColors.Error;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtIndex control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the KeyDown event of the txtTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void txtTitle_KeyDown(object sender, KeyEventArgs e)
        {
            // Forward to "Accept"
            if (e.KeyCode == Keys.Enter)
                btnAccept_Click(this, new EventArgs());
        }

        /// <summary>
        /// Handles the TextChanged event of the txtTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (txtTitle.Text == _gd.Categorization.Title.ToString())
                txtTitle.BackColor = EditorColors.Normal;
            else
                txtTitle.BackColor = EditorColors.Changed;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtW control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtW_TextChanged(object sender, EventArgs e)
        {
            var asStationary = _gd as StationaryGrhData;
            if (asStationary == null)
                return;

            uint o;
            if (Parser.Current.TryParse(txtW.Text, out o))
            {
                if (o == asStationary.OriginalSourceRect.Width)
                    txtW.BackColor = EditorColors.Normal;
                else
                    txtW.BackColor = EditorColors.Changed;
            }
            else
                txtW.BackColor = EditorColors.Error;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtWallH control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtWallH_TextChanged(object sender, EventArgs e)
        {
            var wall = lstWalls.SelectedItem as WallEntityBase;
            if (wall == null)
                return;

            try
            {
                wall.Size = new Vector2(wall.Size.X, Parser.Current.ParseFloat(txtWallH.Text));
                txtWallH.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtWallH.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtWallW control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtWallW_TextChanged(object sender, EventArgs e)
        {
            var wall = lstWalls.SelectedItem as WallEntityBase;
            if (wall == null)
                return;

            try
            {
                wall.Size = new Vector2(Parser.Current.ParseFloat(txtWallW.Text), wall.Size.Y);
                txtWallW.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtWallW.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtWallX control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtWallX_TextChanged(object sender, EventArgs e)
        {
            var wall = lstWalls.SelectedItem as WallEntityBase;
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

        /// <summary>
        /// Handles the TextChanged event of the txtWallY control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtWallY_TextChanged(object sender, EventArgs e)
        {
            var wall = lstWalls.SelectedItem as WallEntityBase;
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

        /// <summary>
        /// Handles the TextChanged event of the txtX control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtX_TextChanged(object sender, EventArgs e)
        {
            var asStationary = _gd as StationaryGrhData;
            if (asStationary == null)
                return;

            uint o;
            if (Parser.Current.TryParse(txtX.Text, out o))
            {
                if (o == asStationary.OriginalSourceRect.X)
                    txtX.BackColor = EditorColors.Normal;
                else
                    txtX.BackColor = EditorColors.Changed;
            }
            else
                txtX.BackColor = EditorColors.Error;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtY control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtY_TextChanged(object sender, EventArgs e)
        {
            var asStationary = _gd as StationaryGrhData;
            if (asStationary == null)
                return;

            uint o;
            if (Parser.Current.TryParse(txtY.Text, out o))
            {
                if (o == asStationary.OriginalSourceRect.Y)
                    txtY.BackColor = EditorColors.Normal;
                else
                    txtY.BackColor = EditorColors.Changed;
            }
            else
                txtY.BackColor = EditorColors.Error;
        }
    }
}