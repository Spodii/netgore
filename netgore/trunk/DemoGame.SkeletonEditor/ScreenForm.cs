using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Content;
using NetGore.EditorTools;
using NetGore.Graphics;
using NetGore.IO;
using SFML.Graphics;

namespace DemoGame.SkeletonEditor
{
    partial class ScreenForm : Form
    {
        const string _filterBody = "Skeleton body (*" + SkeletonBodyInfo.FileSuffix + ")|*" + SkeletonBodyInfo.FileSuffix;
        const string _filterFrame = "Skeleton frame (*" + Skeleton.FileSuffix + ")|*" + Skeleton.FileSuffix;
        const string _filterSet = "Skeleton set (*" + SkeletonSet.FileSuffix + ")|*" + SkeletonSet.FileSuffix;
        const string _skeletonSetFromStringDelimiter = "\r\n";

        readonly IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> _switches;
        readonly Stopwatch _watch = new Stopwatch();

        ICamera2D _camera;
        IContentManager _content;
        int _currentTime = 0;

        /// <summary>
        /// World position of the cursor for the game screen
        /// </summary>
        Vector2 _cursorPos = new Vector2();

        DrawingManager _drawingManager;

        string _fileAnim = string.Empty;
        string _fileBody = string.Empty;
        string _fileFrame = string.Empty;
        Font _font;
        SkeletonBody _frameBody = null;
        bool _moveSelectedNode = false;
        Skeleton _skeleton;
        SkeletonAnimation _skeletonAnim;
        KeyEventArgs ks = new KeyEventArgs(Keys.None);

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenForm"/> class.
        /// </summary>
        /// <param name="switches">The command-line switches.</param>
        public ScreenForm(IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> switches)
        {
            _switches = switches;

            InitializeComponent();
            HookInput();
            GameScreen.ScreenForm = this;
        }

        /// <summary>
        /// Gets or sets the file for the current skeleton animation
        /// </summary>
        string FileAnim
        {
            get { return _fileAnim; }
            set
            {
                _fileAnim = value;
                lblAnimation.Text = "Loaded: " + Path.GetFileName(_fileAnim);
            }
        }

        /// <summary>
        /// Gets or sets the file for the current skeleton body
        /// </summary>
        string FileBody
        {
            get { return _fileBody; }
            set
            {
                _fileBody = value;
                gbBodies.Text = "Bone Bodies: " + Path.GetFileName(_fileBody);
            }
        }

        /// <summary>
        /// Gets or sets the file for the current skeleton frame
        /// </summary>
        string FileFrame
        {
            get { return _fileFrame; }
            set
            {
                _fileFrame = value;
                lblSkeleton.Text = "Loaded: " + Path.GetFileName(_fileFrame);
            }
        }

        /// <summary>
        /// Gets the selected drawable skeleton item in the list
        /// </summary>
        public SkeletonBodyItem SelectedDSI
        {
            get
            {
                try
                {
                    return lstBodies.TypedSelectedItem;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently selected node
        /// </summary>
        public SkeletonNode SelectedNode
        {
            get { return cmbSkeletonNodes.TypedSelectedItem; }
            set { cmbSkeletonNodes.SelectedItem = value; }
        }

        /// <summary>
        /// Gets the SkeletonAnimation's SkeletonBody
        /// </summary>
        public SkeletonBody SkeletonBody
        {
            get { return _skeletonAnim.SkeletonBody; }
        }

        /// <summary>
        /// Draws the screen.
        /// </summary>
        internal void DrawGame()
        {
            // Screen
            var sb = _drawingManager.BeginDrawWorld(_camera, false, true);

            try
            {
                // Draw the center lines
                RenderLine.Draw(sb, new Vector2(-100, 0), new Vector2(100, 0), Color.Lime);
                RenderLine.Draw(sb, new Vector2(0, -5), new Vector2(0, 5), Color.Red);

                if (radioEdit.Checked)
                {
                    // Edit skeleton
                    SkeletonDrawer.Draw(_skeleton, _camera, sb, SelectedNode);
                    if (_frameBody != null && chkDrawBody.Checked)
                        _frameBody.Draw(sb, Vector2.Zero);
                }
                else
                {
                    // Animate skeletons
                    if (chkDrawBody.Checked)
                        _skeletonAnim.Draw(sb);
                    if (chkDrawSkel.Checked)
                        SkeletonDrawer.Draw(_skeletonAnim.Skeleton, _camera, sb);
                }
            }
            finally
            {
                _drawingManager.EndDrawWorld();
            }

            // On-screen GUI
            sb = _drawingManager.BeginDrawGUI();
            try
            {
                var cursorPosText = _cursorPos.Round().ToString();
                var screenSize = new Vector2(GameScreen.Size.Width, GameScreen.Size.Height);
                var strSize = _font.MeasureString(cursorPosText);
                var fontColor = Color.White;
                var borderColor = Color.Black;
                sb.DrawStringShaded(_font, cursorPosText, screenSize - strSize - new Vector2(3), fontColor, borderColor);
            }
            finally
            {
                _drawingManager.EndDrawGUI();
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the GameScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void GameScreen_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioEdit.Checked)
            {
                if (e.Button == MouseButtons.Left)
                {
                    // Select new node
                    var nodes = _skeleton.RootNode.GetAllNodes();
                    foreach (var node in nodes)
                    {
                        if (node.HitTest(_camera, _cursorPos))
                        {
                            SelectedNode = node; // Select the node
                            _moveSelectedNode = true; // Enable dragging
                            break;
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (_skeleton.RootNode == null)
                    {
                        // Create the root node
                        _skeleton.RootNode = new SkeletonNode(_cursorPos);
                        SelectedNode = _skeleton.RootNode;
                        SelectedNode.Name = "New Root";
                    }
                    else
                    {
                        // Create a child node
                        SelectedNode = new SkeletonNode(SelectedNode, _cursorPos);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the GameScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void GameScreen_MouseMove(object sender, MouseEventArgs e)
        {
            _cursorPos = _camera.ToWorld(e.X, e.Y);

            if (_skeleton.RootNode == null || e.Button != MouseButtons.Left || !_moveSelectedNode)
                return;

            if (chkCanTransform.Checked)
            {
                if (ks.Control)
                {
                    // Unlocked movement, move the node and its children
                    SelectedNode.MoveTo(_cursorPos);
                    UpdateNodeInfo();
                }
                else
                {
                    // Unlocked movement, move just the one node
                    SelectedNode.Position = _cursorPos;
                    UpdateNodeInfo();
                }
            }
            else
            {
                if (ks.Control)
                {
                    // Locked movement, the node and all of its children
                    SelectedNode.Rotate(_cursorPos);
                    UpdateNodeInfo();
                }
                else
                {
                    // Locked movement, move the node and its children
                    if (SelectedNode.Parent != null)
                        SelectedNode.SetAngle(_cursorPos);
                    else
                        SelectedNode.MoveTo(_cursorPos);
                    UpdateNodeInfo();
                }
            }
        }

        /// <summary>
        /// Handles the MouseUp event of the GameScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void GameScreen_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _moveSelectedNode = false;
        }

        static string GetLoadSkeletonDialogResult(string filter)
        {
            string result;

            using (var fd = new OpenFileDialog())
            {
                fd.Filter = filter;
                fd.InitialDirectory = ContentPaths.Dev.Skeletons;
                fd.RestoreDirectory = true;
                fd.ShowDialog();
                result = fd.FileName;
            }

            return result;
        }

        static string[] GetLoadSkeletonDialogResults(string filter)
        {
            string[] result;

            using (var fd = new OpenFileDialog())
            {
                fd.Filter = filter;
                fd.InitialDirectory = ContentPaths.Dev.Skeletons;
                fd.RestoreDirectory = true;
                fd.ShowDialog();
                result = fd.FileNames;
            }

            return result;
        }

        static string GetSaveSkeletonDialogResult(string filter)
        {
            string result;

            using (var fd = new SaveFileDialog())
            {
                fd.Filter = filter;
                fd.InitialDirectory = ContentPaths.Dev.Skeletons;
                fd.RestoreDirectory = true;
                fd.ShowDialog();
                result = fd.FileName;
            }

            return result;
        }

        /// <summary>
        /// Gets the current game time where time 0 is when the application started
        /// </summary>
        /// <returns>Current game time in milliseconds</returns>
        public int GetTime()
        {
            return _currentTime;
        }

        static void HandleSwitch_SaveAll()
        {
            var files = Directory.GetFiles(ContentPaths.Dev.Skeletons);

            // Frames
            foreach (var file in files.Where(x => x.EndsWith(Skeleton.FileSuffix, StringComparison.OrdinalIgnoreCase)))
            {
                var skeleton = SkeletonLoader.LoadSkeleton(file);
                skeleton.Write(file);
            }

            // Sets
            foreach (var file in files.Where(x => x.EndsWith(SkeletonSet.FileSuffix, StringComparison.OrdinalIgnoreCase)))
            {
                var set = SkeletonLoader.LoadSkeletonSet(file);
                set.Write(file);
            }

            // Bodies
            foreach (var file in files.Where(x => x.EndsWith(SkeletonBodyInfo.FileSuffix, StringComparison.OrdinalIgnoreCase)))
            {
                var body = SkeletonLoader.LoadSkeletonBodyInfo(file);
                body.Save(file);
            }
        }

        void HandleSwitches(IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> switches)
        {
            if (switches == null || switches.Count() == 0)
                return;

            var willClose = false;

            foreach (var item in switches)
            {
                switch (item.Key)
                {
                    case CommandLineSwitch.SaveAll:
                        HandleSwitch_SaveAll();
                        break;

                    case CommandLineSwitch.Close:
                        willClose = true;
                        break;
                }
            }

            // To close, we actually will create a timer to close the form one ms from now
            if (willClose)
            {
                var t = new Timer { Interval = 1 };
                t.Tick += delegate { Close(); };
                t.Start();
            }
        }

        void HookInput()
        {
            RecursiveHookInput(this);
        }

        void KeyDownForward(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        void KeyUpForward(object sender, KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        public void LoadAnim(string filePath)
        {
            var newSet = SkeletonLoader.LoadSkeletonSet(filePath);
            _skeletonAnim.ChangeSet(newSet);

            FileAnim = filePath;
            txtFrames.Text = _skeletonAnim.SkeletonSet.GetFramesString();
            UpdateAnimationNodeCBs();
        }

        public void LoadBody(string filePath)
        {
            var bodyInfo = SkeletonLoader.LoadSkeletonBodyInfo(filePath);
            _skeletonAnim.SkeletonBody = new SkeletonBody(bodyInfo, _skeletonAnim.Skeleton);
            _frameBody = new SkeletonBody(bodyInfo, _skeleton);
            UpdateBodyList();
            FileBody = filePath;
        }

        public void LoadFrame(string filePath)
        {
            var newSkeleton = SkeletonLoader.LoadSkeleton(filePath);
            LoadFrame(newSkeleton);
            FileFrame = filePath;
        }

        void LoadFrame(Skeleton skel)
        {
            _skeleton = skel;

            if (_frameBody != null)
                _frameBody.Attach(_skeleton);

            SelectedNode = _skeleton.RootNode;

            UpdateFrameNodeCBs();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            ks = e;

            const float TranslateRate = 7f;
            const float ScaleRate = 0.08f;

            switch (e.KeyCode)
            {
                case Keys.NumPad8:
                    _camera.Translate(new Vector2(0, -TranslateRate / _camera.Scale));
                    break;
                case Keys.NumPad4:
                    _camera.Translate(new Vector2(-TranslateRate / _camera.Scale, 0));
                    break;
                case Keys.NumPad6:
                    _camera.Translate(new Vector2(TranslateRate / _camera.Scale, 0));
                    break;
                case Keys.NumPad2:
                    _camera.Translate(new Vector2(0, TranslateRate / _camera.Scale));
                    break;
                case Keys.NumPad9:
                    _camera.Zoom(_camera.Min + ((_camera.Size / 2) / _camera.Scale), _camera.Size, _camera.Scale + ScaleRate);
                    break;
                case Keys.NumPad7:
                    _camera.Zoom(_camera.Min + ((_camera.Size / 2) / _camera.Scale), _camera.Size, _camera.Scale - ScaleRate);
                    break;
                case Keys.Delete:
                    if (chkCanAlter.Checked && SelectedNode != null)
                    {
                        var removeNode = SelectedNode;
                        SelectedNode = SelectedNode.Parent;
                        removeNode.Remove();
                    }
                    break;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            ks = e;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Create the engine objects
            _drawingManager = new DrawingManager(GameScreen.RenderWindow);
            _camera = new Camera2D(new Vector2(GameScreen.Width, GameScreen.Height)) { KeepInMap = false };
            _content = ContentManager.Create();
            _font = _content.LoadFont("Font/Arial", 14, ContentLevel.GameScreen);
            GrhInfo.Load(ContentPaths.Dev, _content);

            // Create the skeleton-related objects
            _skeleton = new Skeleton();
            var frameSkeleton = new Skeleton(SkeletonLoader.StandingSkeletonName, ContentPaths.Dev);
            var frame = new SkeletonFrame(SkeletonLoader.StandingSkeletonName, frameSkeleton);
            _skeletonAnim = new SkeletonAnimation(GetTime(), frame);

            LoadFrame(Skeleton.GetFilePath(SkeletonLoader.StandingSkeletonName, ContentPaths.Dev));
            LoadAnim(SkeletonSet.GetFilePath(SkeletonLoader.WalkingSkeletonSetName, ContentPaths.Dev));
            LoadBody(SkeletonBodyInfo.GetFilePath(SkeletonLoader.BasicSkeletonBodyName, ContentPaths.Dev));

            _watch.Start();

            HandleSwitches(_switches);

            // Zoom in and start animation
            radioAnimate.Checked = true;
            chkDrawSkel.Checked = false;
            _camera.Zoom(new Vector2(0, -25), _camera.Size, 3f);
        }

        void RecursiveHookInput(Control rootC)
        {
            foreach (Control c in rootC.Controls)
            {
                if (c.GetType() != typeof(TextBoxBase) && c.GetType() != typeof(ListControl))
                {
                    c.KeyDown += KeyDownForward;
                    c.KeyUp += KeyUpForward;
                }
                RecursiveHookInput(c);
            }
        }

        void SetAnimByTxt()
        {
            try
            {
                var newSet = SkeletonLoader.LoadSkeletonSetFromString(txtFrames.Text, _skeletonSetFromStringDelimiter);
                if (newSet == null)
                    throw new Exception();

                _skeletonAnim.ChangeSet(newSet);
                txtFrames.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtFrames.BackColor = EditorColors.Error;
            }
        }

        void UpdateAnimationNodeCBs()
        {
            var nodes = _skeletonAnim.Skeleton.RootNode.GetAllNodes();

            cmbSource.Items.Clear();
            cmbSource.AddItems(nodes);

            cmbTarget.Items.Clear();
            cmbTarget.AddItems(nodes);
        }

        void UpdateBodyList()
        {
            lstBodies.Items.Clear();
            lstBodies.AddItems(SkeletonBody.BodyItems);

            UpdateAnimationNodeCBs();
        }

        void UpdateFrameNodeCBs()
        {
            cmbSkeletonNodes.Items.Clear();
            var nodes = _skeleton.RootNode.GetAllNodes();
            cmbSkeletonNodes.AddItems(nodes);
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        public void UpdateGame()
        {
            if (!_watch.IsRunning)
                return;

            _currentTime = (int)_watch.ElapsedMilliseconds;
            _skeletonAnim.Update(_currentTime);
        }

        /// <summary>
        /// Updates the node info.
        /// </summary>
        public void UpdateNodeInfo()
        {
            if (SelectedNode == null)
                return;

            txtName.Text = SelectedNode.Name;
            txtX.Text = SelectedNode.X.ToString();
            txtY.Text = SelectedNode.Y.ToString();
            txtAngle.Text = SelectedNode.GetAngle().ToString();
            txtLength.Text = SelectedNode.GetLength().ToString();
            chkIsMod.Checked = SelectedNode.IsModifier;
        }

        void UpdateSelectedDSI()
        {
            lstBodies.RefreshItemAt(lstBodies.SelectedIndex);
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAdd_Click(object sender, EventArgs e)
        {
            Array.Resize(ref SkeletonBody.BodyItems, SkeletonBody.BodyItems.Length + 1);
            var spriteCategorization = new SpriteCategorization("Character.Naked", "Body");
            var grhData = GrhInfo.GetData(spriteCategorization);
            var bodyItemInfo = new SkeletonBodyItemInfo(grhData.GrhIndex, _skeleton.RootNode.Name, string.Empty, Vector2.Zero,
                                                        Vector2.Zero);
            var bodyItem = new SkeletonBodyItem(bodyItemInfo);
            SkeletonBody.BodyItems[SkeletonBody.BodyItems.Length - 1] = bodyItem;
            UpdateBodyList();
        }

        /// <summary>
        /// Handles the Click event of the btnAnimLoad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAnimLoad_Click(object sender, EventArgs e)
        {
            var result = GetLoadSkeletonDialogResult(_filterSet);

            if (result != null && result.Length > 1)
                LoadAnim(result);
        }

        /// <summary>
        /// Handles the Click event of the btnAnimSaveAs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAnimSaveAs_Click(object sender, EventArgs e)
        {
            var result = GetSaveSkeletonDialogResult(_filterSet);

            if (result != null && result.Length > 1)
            {
                var skelSet = SkeletonLoader.LoadSkeletonSetFromString(txtFrames.Text, _skeletonSetFromStringDelimiter);
                skelSet.Write(result);
                FileAnim = result;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnAnimSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAnimSave_Click(object sender, EventArgs e)
        {
            if (FileAnim != null && FileAnim.Length > 1)
            {
                var skelSet = SkeletonLoader.LoadSkeletonSetFromString(txtFrames.Text, _skeletonSetFromStringDelimiter);
                skelSet.Write(FileAnim);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnBodyLoad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnBodyLoad_Click(object sender, EventArgs e)
        {
            var result = GetLoadSkeletonDialogResult(_filterBody);

            if (result != null && result.Length > 1)
                LoadBody(result);
        }

        /// <summary>
        /// Handles the Click event of the btnBodySaveAs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnBodySaveAs_Click(object sender, EventArgs e)
        {
            var result = GetSaveSkeletonDialogResult(_filterBody);

            if (result != null && result.Length > 1)
            {
                SkeletonBody.BodyInfo.Save(result);
                FileBody = result;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnBodySave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnBodySave_Click(object sender, EventArgs e)
        {
            if (FileBody != null && FileBody.Length > 1)
                SkeletonBody.BodyInfo.Save(FileBody);
        }

        /// <summary>
        /// Handles the Click event of the btnClearTarget control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnClearTarget_Click(object sender, EventArgs e)
        {
            cmbTarget.SelectedItem = null;
        }

        /// <summary>
        /// Handles the Click event of the btnCopyInherits control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCopyInherits_Click(object sender, EventArgs e)
        {
            var results = GetLoadSkeletonDialogResults(_filterFrame);

            if (results == null || results.Length <= 0)
                return;

            foreach (var s in results)
            {
                if (!File.Exists(s))
                    continue;

                var tmpSkel = SkeletonLoader.LoadSkeleton(s);
                _skeleton.CopyIsModifier(tmpSkel);
                tmpSkel.Write(s);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCopyLen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCopyLen_Click(object sender, EventArgs e)
        {
            var results = GetLoadSkeletonDialogResults(_filterFrame);

            if (results == null || results.Length <= 0)
                return;

            foreach (var s in results)
            {
                if (!File.Exists(s))
                    continue;

                var tmpSkel = SkeletonLoader.LoadSkeleton(s);
                Skeleton.CopyLength(_skeleton, tmpSkel);
                tmpSkel.Write(s);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCopyRoot control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCopyRoot_Click(object sender, EventArgs e)
        {
            var rX = MessageBox.Show("Copy the X axis?", "Skeleton frame root copy", MessageBoxButtons.YesNoCancel);
            if (rX == DialogResult.Cancel)
                return;

            var rY = MessageBox.Show("Copy the Y axis?", "Skeleton frame root copy", MessageBoxButtons.YesNoCancel);
            if (rY == DialogResult.Cancel)
                return;

            if (rX == DialogResult.No && rY == DialogResult.No)
                return;

            var results = GetLoadSkeletonDialogResults(_filterFrame);

            if (results != null && results.Length > 0)
            {
                foreach (var s in results)
                {
                    if (!File.Exists(s))
                        continue;

                    var tmpSkel = SkeletonLoader.LoadSkeleton(s);
                    var newPos = _skeleton.RootNode.Position;
                    if (rX == DialogResult.Yes)
                        newPos.X = tmpSkel.RootNode.X;
                    if (rY == DialogResult.Yes)
                        newPos.Y = tmpSkel.RootNode.Y;
                    tmpSkel.RootNode.MoveTo(newPos);
                    tmpSkel.Write(s);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstBodies.SelectedIndex == -1)
                return;

            var sel = lstBodies.SelectedIndex;
            for (var i = sel; i < lstBodies.Items.Count - 1; i++)
            {
                lstBodies.Items[i] = lstBodies.Items[i + 1];
                SkeletonBody.BodyItems[i] = SkeletonBody.BodyItems[i + 1];
            }

            lstBodies.RemoveItemAtAndReselect(lstBodies.Items.Count - 1);
            Array.Resize(ref SkeletonBody.BodyItems, SkeletonBody.BodyItems.Length - 1);
        }

        /// <summary>
        /// Handles the Click event of the btnDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDown_Click(object sender, EventArgs e)
        {
            var selIndex = lstBodies.SelectedIndex;
            if (selIndex < 0 || selIndex >= lstBodies.Items.Count - 1)
                return;

            var o1 = lstBodies.Items[selIndex];
            var o2 = lstBodies.Items[selIndex + 1];
            lstBodies.Items[selIndex] = o2;
            lstBodies.Items[selIndex + 1] = o1;

            o1 = SkeletonBody.BodyItems[selIndex];
            o2 = SkeletonBody.BodyItems[selIndex + 1];
            SkeletonBody.BodyItems[selIndex] = (SkeletonBodyItem)o2;
            SkeletonBody.BodyItems[selIndex + 1] = (SkeletonBodyItem)o1;

            lstBodies.SelectedIndex = selIndex + 1;
        }

        /// <summary>
        /// Handles the Click event of the btnFall control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnFall_Click(object sender, EventArgs e)
        {
            if (!radioAnimate.Checked)
                return;

            var newSet = new SkeletonSet(SkeletonLoader.FallingSkeletonSetName, ContentPaths.Dev);
            _skeletonAnim.ChangeSet(newSet);
        }

        /// <summary>
        /// Handles the Click event of the btnInterpolate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnInterpolate_Click(object sender, EventArgs e)
        {
            var result = GetLoadSkeletonDialogResult(_filterFrame);

            if (result == null || result.Length <= 1)
                return;

            var frame1Skeleton = SkeletonLoader.LoadSkeleton(result);
            var frame1 = new SkeletonFrame(result, frame1Skeleton, 10);

            result = GetLoadSkeletonDialogResult(_filterFrame);

            if (result == null || result.Length <= 1)
                return;

            var frame2Skeleton = SkeletonLoader.LoadSkeleton(result);
            var frame2 = new SkeletonFrame(result, frame2Skeleton, 10);

            var frames = new[] { frame1, frame2 };

            var ss = new SkeletonSet(frames);
            var sa = new SkeletonAnimation(GetTime(), ss);

            sa.Update(5);
            LoadFrame(sa.Skeleton);
        }

        /// <summary>
        /// Handles the Click event of the btnJump control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnJump_Click(object sender, EventArgs e)
        {
            if (!radioAnimate.Checked)
                return;

            var newSet = new SkeletonSet(SkeletonLoader.JumpingSkeletonSetName, ContentPaths.Dev);
            _skeletonAnim.ChangeSet(newSet);
        }

        /// <summary>
        /// Handles the Click event of the btnPause control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnPause_Click(object sender, EventArgs e)
        {
            if (_watch.IsRunning)
                _watch.Stop();
            else
                _watch.Start();
        }

        /// <summary>
        /// Handles the Click event of the btnPlay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnPlay_Click(object sender, EventArgs e)
        {
            if (!_watch.IsRunning)
                _watch.Start();

            SetAnimByTxt();
        }

        /// <summary>
        /// Handles the Click event of the btnShiftNodes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnShiftNodes_Click(object sender, EventArgs e)
        {
            using (var f = new ShiftNodesInputForm())
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                var p = f.Value;
                if (p != Vector2.Zero)
                {
                    foreach (var node in _skeleton.RootNode.GetAllNodes())
                    {
                        node.Position += p;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSkeletonLoad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnSkeletonLoad_Click(object sender, EventArgs e)
        {
            var result = GetLoadSkeletonDialogResult(_filterFrame);

            if (result != null && result.Length > 1)
                LoadFrame(result);
        }

        /// <summary>
        /// Handles the Click event of the btnSkeletonSaveAs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnSkeletonSaveAs_Click(object sender, EventArgs e)
        {
            var result = GetSaveSkeletonDialogResult(_filterFrame);

            if (result != null && result.Length > 1)
            {
                _skeleton.Write(result);
                FileFrame = result;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSkeletonSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnSkeletonSave_Click(object sender, EventArgs e)
        {
            if (FileFrame != null && FileFrame.Length > 1)
                _skeleton.Write(FileFrame);
        }

        /// <summary>
        /// Handles the Click event of the btnStand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnStand_Click(object sender, EventArgs e)
        {
            if (!radioAnimate.Checked)
                return;

            var standingSet = SkeletonLoader.GetStandingSkeletonSet();
            _skeletonAnim.ChangeSet(standingSet);
        }

        /// <summary>
        /// Handles the Click event of the btnUp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnUp_Click(object sender, EventArgs e)
        {
            var selIndex = lstBodies.SelectedIndex;
            if (selIndex <= 0)
                return;

            var o1 = lstBodies.Items[selIndex];
            var o2 = lstBodies.Items[selIndex - 1];
            lstBodies.Items[selIndex] = o2;
            lstBodies.Items[selIndex - 1] = o1;

            o1 = SkeletonBody.BodyItems[selIndex];
            o2 = SkeletonBody.BodyItems[selIndex - 1];
            SkeletonBody.BodyItems[selIndex] = (SkeletonBodyItem)o2;
            SkeletonBody.BodyItems[selIndex - 1] = (SkeletonBodyItem)o1;

            lstBodies.SelectedIndex = selIndex - 1;
        }

        /// <summary>
        /// Handles the Click event of the btnWalk control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnWalk_Click(object sender, EventArgs e)
        {
            if (!radioAnimate.Checked)
                return;

            var newSet = new SkeletonSet(SkeletonLoader.WalkingSkeletonSetName, ContentPaths.Dev);
            _skeletonAnim.ChangeSet(newSet);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkIsMod control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void chkIsMod_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedNode.IsModifier = chkIsMod.Checked;
                chkIsMod.BackColor = EditorColors.Normal;
            }
            catch
            {
                chkIsMod.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbSkeletonNodes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void cmbSkeletonNodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateNodeInfo();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void cmbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedDSI.Source = cmbSource.TypedSelectedItem;
                cmbSource.BackColor = EditorColors.Normal;
                UpdateSelectedDSI();
            }
            catch
            {
                cmbSource.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbTarget control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void cmbTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedDSI.Dest = cmbTarget.TypedSelectedItem;
                cmbTarget.BackColor = EditorColors.Normal;
                UpdateSelectedDSI();
            }
            catch
            {
                cmbTarget.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstBodies control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstBodies_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = SelectedDSI;
            if (item == null)
                return;

            txtOffsetX.Text = item.ItemInfo.Offset.X.ToString();
            txtOffsetY.Text = item.ItemInfo.Offset.Y.ToString();
            txtOriginX.Text = item.ItemInfo.Origin.X.ToString();
            txtOriginY.Text = item.ItemInfo.Origin.Y.ToString();

            if (item.Grh.GrhData != null)
                txtGrhIndex.Text = item.Grh.GrhData.GrhIndex.ToString();

            cmbSource.SelectedItem = cmbSource.TypedItems.FirstOrDefault(x => x.Name == item.Source.Name);

            if (item.Dest == null)
                cmbTarget.SelectedItem = null;
            else
                cmbTarget.SelectedItem = cmbTarget.TypedItems.FirstOrDefault(x => x.Name == item.Dest.Name);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the radioAnimate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void radioAnimate_CheckedChanged(object sender, EventArgs e)
        {
            if (radioAnimate.Checked)
                SetAnimByTxt();
        }

        /// <summary>
        /// Handles the TextChanged event of the txtAngle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtAngle_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedNode.SetAngle(Parser.Current.ParseFloat(txtAngle.Text));
                txtAngle.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtAngle.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtFrames control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtFrames_TextChanged(object sender, EventArgs e)
        {
            SetAnimByTxt();
        }

        /// <summary>
        /// Handles the TextChanged event of the txtGrhIndex control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtGrhIndex_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var grhIndex = Parser.Current.ParseGrhIndex(txtGrhIndex.Text);
                var grhData = GrhInfo.GetData(grhIndex);
                if (grhData == null)
                {
                    txtGrhIndex.BackColor = EditorColors.Error;
                    return;
                }

                SelectedDSI.Grh.SetGrh(grhData, AnimType.Loop, (int)_watch.ElapsedMilliseconds);
                SelectedDSI.ItemInfo.GrhIndex = grhIndex;

                txtGrhIndex.BackColor = EditorColors.Normal;
                UpdateSelectedDSI();
            }
            catch
            {
                txtGrhIndex.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtLength control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtLength_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedNode.SetLength(Parser.Current.ParseFloat(txtLength.Text));
                txtLength.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtLength.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedNode.Name = txtName.Text;
                txtName.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtName.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtOffsetX control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtOffsetX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var x = Parser.Current.ParseFloat(txtOffsetX.Text);
                SelectedDSI.ItemInfo.Offset = new Vector2(x, SelectedDSI.ItemInfo.Offset.Y);
                txtOffsetX.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtOffsetX.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtOffsetY control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtOffsetY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var y = Parser.Current.ParseFloat(txtOffsetY.Text);
                SelectedDSI.ItemInfo.Offset = new Vector2(SelectedDSI.ItemInfo.Offset.X, y);
                txtOffsetY.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtOffsetY.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtOriginX control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtOriginX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var x = Parser.Current.ParseFloat(txtOriginX.Text);
                SelectedDSI.ItemInfo.Origin = new Vector2(x, SelectedDSI.ItemInfo.Origin.Y);
                txtOriginX.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtOriginX.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtOriginY control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtOriginY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var y = Parser.Current.ParseFloat(txtOriginY.Text);
                SelectedDSI.ItemInfo.Origin = new Vector2(SelectedDSI.ItemInfo.Origin.X, y);
                txtOriginY.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtOriginY.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtX control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (SelectedNode.Parent == null)
                    SelectedNode.MoveTo(new Vector2(Parser.Current.ParseFloat(txtX.Text), SelectedNode.Y));
                else
                    SelectedNode.X = Parser.Current.ParseFloat(txtX.Text);
                txtX.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtX.BackColor = EditorColors.Error;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtY control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (SelectedNode.Parent == null)
                    SelectedNode.MoveTo(new Vector2(SelectedNode.X, Parser.Current.ParseFloat(txtY.Text)));
                else
                    SelectedNode.Y = Parser.Current.ParseFloat(txtY.Text);
                txtY.BackColor = EditorColors.Normal;
            }
            catch
            {
                txtY.BackColor = EditorColors.Error;
            }
        }
    }
}