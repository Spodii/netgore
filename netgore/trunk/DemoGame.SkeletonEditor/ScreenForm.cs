using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.EditorTools;
using NetGore.Graphics;
using NetGore.IO;
using Color=System.Drawing.Color;

namespace DemoGame.SkeletonEditor
{
    partial class ScreenForm : Form
    {
        const string _filterBody = "Skeleton body (*" + SkeletonBodyInfo.FileSuffix + ")|*" + SkeletonBodyInfo.FileSuffix;
        const string _filterFrame = "Skeleton frame (*" + Skeleton.FileSuffix + ")|*" + Skeleton.FileSuffix;
        const string _filterSet = "Skeleton set (*" + SkeletonSet.FileSuffix + ")|*" + SkeletonSet.FileSuffix;
        const string _skeletonSetFromStringDelimiter = "\r\n";

        static readonly Color ColorError = Color.Red;
        static readonly Color ColorNormal = SystemColors.Window;

        readonly List<XNALine> _centerLines = new List<XNALine>();
        readonly IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> _switches;
        readonly Stopwatch _watch = new Stopwatch();

        ICamera2D _camera;
        ContentManager _content;
        int _currentTime = 0;

        /// <summary>
        /// World position of the cursor for the game screen
        /// </summary>
        Vector2 _cursorPos = new Vector2();

        string _fileAnim = string.Empty;
        string _fileBody = string.Empty;
        string _fileFrame = string.Empty;
        SkeletonBody _frameBody = null;
        bool _moveSelectedNode = false;
        SpriteBatch _sb;
        SkeletonNode _selectedNode = null;
        Skeleton _skeleton;
        SkeletonAnimation _skeletonAnim;
        SkeletonDrawer _skeletonDrawer;
        KeyEventArgs ks = new KeyEventArgs(Keys.None);

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
                    return ((ListItem<SkeletonBodyItem>)lstBodies.SelectedItem).Item;
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
            get { return _selectedNode; }
            set
            {
                if (_selectedNode != value)
                {
                    _selectedNode = value;
                    ChangeSelectedNode();
                }
            }
        }

        /// <summary>
        /// Gets the SkeletonAnimation's SkeletonBody
        /// </summary>
        public SkeletonBody SkeletonBody
        {
            get { return _skeletonAnim.SkeletonBody; }
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            Array.Resize(ref SkeletonBody.BodyItems, SkeletonBody.BodyItems.Length + 1);
            var spriteCategorization = new SpriteCategorization("Character.Naked", "Body");
            var grhData = GrhInfo.GetData(spriteCategorization);
            SkeletonBodyItemInfo bodyItemInfo = new SkeletonBodyItemInfo(grhData.GrhIndex, _skeleton.RootNode.Name, string.Empty,
                                                                         Vector2.Zero, Vector2.Zero);
            SkeletonBodyItem bodyItem = new SkeletonBodyItem(bodyItemInfo);
            SkeletonBody.BodyItems[SkeletonBody.BodyItems.Length - 1] = bodyItem;
            UpdateBodyList();
        }

        void btnAnimLoad_Click(object sender, EventArgs e)
        {
            string result = GetLoadSkeletonDialogResult(_filterSet);

            if (result != null && result.Length > 1)
                LoadAnim(result);
        }

        void btnAnimSave_Click(object sender, EventArgs e)
        {
            if (FileAnim != null && FileAnim.Length > 1)
            {
                SkeletonSet skelSet = SkeletonLoader.LoadSkeletonSetFromString(txtFrames.Text, _skeletonSetFromStringDelimiter);
                skelSet.Write(FileAnim);
            }
        }

        void btnAnimSaveAs_Click(object sender, EventArgs e)
        {
            string result = GetSaveSkeletonDialogResult(_filterSet);

            if (result != null && result.Length > 1)
            {
                SkeletonSet skelSet = SkeletonLoader.LoadSkeletonSetFromString(txtFrames.Text, _skeletonSetFromStringDelimiter);
                skelSet.Write(result);
                FileAnim = result;
            }
        }

        void btnBodyLoad_Click(object sender, EventArgs e)
        {
            string result = GetLoadSkeletonDialogResult(_filterBody);

            if (result != null && result.Length > 1)
                LoadBody(result);
        }

        void btnBodySave_Click(object sender, EventArgs e)
        {
            if (FileBody != null && FileBody.Length > 1)
                SkeletonBody.BodyInfo.Save(FileBody);
        }

        void btnBodySaveAs_Click(object sender, EventArgs e)
        {
            string result = GetSaveSkeletonDialogResult(_filterBody);

            if (result != null && result.Length > 1)
            {
                SkeletonBody.BodyInfo.Save(result);
                FileBody = result;
            }
        }

        void btnCopyInherits_Click(object sender, EventArgs e)
        {
            var results = GetLoadSkeletonDialogResults(_filterFrame);

            if (results == null || results.Length <= 0)
                return;

            foreach (string s in results)
            {
                if (!File.Exists(s))
                    continue;

                Skeleton tmpSkel = SkeletonLoader.LoadSkeleton(s);
                _skeleton.CopyIsModifier(tmpSkel);
                tmpSkel.Write(s);
            }
        }

        void btnCopyLen_Click(object sender, EventArgs e)
        {
            var results = GetLoadSkeletonDialogResults(_filterFrame);

            if (results == null || results.Length <= 0)
                return;

            foreach (string s in results)
            {
                if (!File.Exists(s))
                    continue;

                Skeleton tmpSkel = SkeletonLoader.LoadSkeleton(s);
                Skeleton.CopyLength(_skeleton, tmpSkel);
                tmpSkel.Write(s);
            }
        }

        void btnCopyRoot_Click(object sender, EventArgs e)
        {
            DialogResult rX = MessageBox.Show("Copy the X axis?", "Skeleton frame root copy", MessageBoxButtons.YesNoCancel);
            if (rX == DialogResult.Cancel)
                return;

            DialogResult rY = MessageBox.Show("Copy the Y axis?", "Skeleton frame root copy", MessageBoxButtons.YesNoCancel);
            if (rY == DialogResult.Cancel)
                return;

            if (rX == DialogResult.No && rY == DialogResult.No)
                return;

            var results = GetLoadSkeletonDialogResults(_filterFrame);

            if (results != null && results.Length > 0)
            {
                foreach (string s in results)
                {
                    if (!File.Exists(s))
                        continue;

                    Skeleton tmpSkel = SkeletonLoader.LoadSkeleton(s);
                    Vector2 newPos = _skeleton.RootNode.Position;
                    if (rX == DialogResult.Yes)
                        newPos.X = tmpSkel.RootNode.X;
                    if (rY == DialogResult.Yes)
                        newPos.Y = tmpSkel.RootNode.Y;
                    tmpSkel.RootNode.MoveTo(newPos);
                    tmpSkel.Write(s);
                }
            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstBodies.SelectedIndex == -1)
                return;

            int sel = lstBodies.SelectedIndex;
            for (int i = sel; i < lstBodies.Items.Count - 1; i++)
            {
                lstBodies.Items[i] = lstBodies.Items[i + 1];
                SkeletonBody.BodyItems[i] = SkeletonBody.BodyItems[i + 1];
            }
            lstBodies.RemoveItemAtAndReselect(lstBodies.Items.Count - 1);
            Array.Resize(ref SkeletonBody.BodyItems, SkeletonBody.BodyItems.Length - 1);
        }

        void btnDown_Click(object sender, EventArgs e)
        {
            int selIndex = lstBodies.SelectedIndex;
            if (selIndex < 0 || selIndex >= lstBodies.Items.Count - 1)
                return;

            object o1 = lstBodies.Items[selIndex];
            object o2 = lstBodies.Items[selIndex + 1];
            lstBodies.Items[selIndex] = o2;
            lstBodies.Items[selIndex + 1] = o1;

            o1 = SkeletonBody.BodyItems[selIndex];
            o2 = SkeletonBody.BodyItems[selIndex + 1];
            SkeletonBody.BodyItems[selIndex] = (SkeletonBodyItem)o2;
            SkeletonBody.BodyItems[selIndex + 1] = (SkeletonBodyItem)o1;

            lstBodies.SelectedIndex = selIndex + 1;
        }

        void btnFall_Click(object sender, EventArgs e)
        {
            if (!radioAnimate.Checked)
                return;

            SkeletonSet newSet = new SkeletonSet(SkeletonLoader.FallingSkeletonSetName, ContentPaths.Dev);
            _skeletonAnim.ChangeSet(newSet);
        }

        void btnInterpolate_Click(object sender, EventArgs e)
        {
            string result = GetLoadSkeletonDialogResult(_filterFrame);

            if (result == null || result.Length <= 1)
                return;

            Skeleton frame1Skeleton = SkeletonLoader.LoadSkeleton(result);
            SkeletonFrame frame1 = new SkeletonFrame(result, frame1Skeleton, 10);

            result = GetLoadSkeletonDialogResult(_filterFrame);

            if (result == null || result.Length <= 1)
                return;

            Skeleton frame2Skeleton = SkeletonLoader.LoadSkeleton(result);
            SkeletonFrame frame2 = new SkeletonFrame(result, frame2Skeleton, 10);

            var frames = new[] { frame1, frame2 };

            SkeletonSet ss = new SkeletonSet(frames);
            SkeletonAnimation sa = new SkeletonAnimation(GetTime(), ss);

            sa.Update(5);
            LoadFrame(sa.Skeleton);
        }

        void btnJump_Click(object sender, EventArgs e)
        {
            if (!radioAnimate.Checked)
                return;

            SkeletonSet newSet = new SkeletonSet(SkeletonLoader.JumpingSkeletonSetName, ContentPaths.Dev);
            _skeletonAnim.ChangeSet(newSet);
        }

        void btnPause_Click(object sender, EventArgs e)
        {
            if (_watch.IsRunning)
                _watch.Stop();
            else
                _watch.Start();
        }

        void btnPlay_Click(object sender, EventArgs e)
        {
            if (!_watch.IsRunning)
                _watch.Start();

            SetAnimByTxt();
        }

        void btnShiftNodes_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Make this button shift all nodes in all seleced files by a defined amount");
        }

        void btnSkeletonLoad_Click(object sender, EventArgs e)
        {
            string result = GetLoadSkeletonDialogResult(_filterFrame);

            if (result != null && result.Length > 1)
                LoadFrame(result);
        }

        void btnSkeletonSave_Click(object sender, EventArgs e)
        {
            if (FileFrame != null && FileFrame.Length > 1)
                _skeleton.Write(FileFrame);
        }

        void btnSkeletonSaveAs_Click(object sender, EventArgs e)
        {
            string result = GetSaveSkeletonDialogResult(_filterFrame);

            if (result != null && result.Length > 1)
            {
                _skeleton.Write(result);
                FileFrame = result;
            }
        }

        void btnStand_Click(object sender, EventArgs e)
        {
            if (!radioAnimate.Checked)
                return;

            SkeletonSet standingSet = SkeletonLoader.GetStandingSkeletonSet();
            _skeletonAnim.ChangeSet(standingSet);
        }

        void btnUp_Click(object sender, EventArgs e)
        {
            int selIndex = lstBodies.SelectedIndex;
            if (selIndex <= 0)
                return;

            object o1 = lstBodies.Items[selIndex];
            object o2 = lstBodies.Items[selIndex - 1];
            lstBodies.Items[selIndex] = o2;
            lstBodies.Items[selIndex - 1] = o1;

            o1 = SkeletonBody.BodyItems[selIndex];
            o2 = SkeletonBody.BodyItems[selIndex - 1];
            SkeletonBody.BodyItems[selIndex] = (SkeletonBodyItem)o2;
            SkeletonBody.BodyItems[selIndex - 1] = (SkeletonBodyItem)o1;

            lstBodies.SelectedIndex = selIndex - 1;
        }

        void btnWalk_Click(object sender, EventArgs e)
        {
            if (!radioAnimate.Checked)
                return;

            SkeletonSet newSet = new SkeletonSet(SkeletonLoader.WalkingSkeletonSetName, ContentPaths.Dev);
            _skeletonAnim.ChangeSet(newSet);
        }

        void ChangeSelectedNode()
        {
            foreach (object item in cmbSkeletonNodes.Items)
            {
                if (((ListItem<SkeletonNode>)item).Item == SelectedNode)
                {
                    cmbSkeletonNodes.SelectedItem = item;
                    break;
                }
            }
            UpdateNodeInfo();
        }

        void chkIsMod_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedNode.IsModifier = chkIsMod.Checked;
                chkIsMod.BackColor = ColorNormal;
            }
            catch
            {
                chkIsMod.BackColor = ColorError;
            }
        }

        void cmbSkeletonNodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedNode = ((ListItem<SkeletonNode>)cmbSkeletonNodes.SelectedItem).Item;
        }

        void cmbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedDSI.Source = ((ListItem<SkeletonNode>)cmbSource.SelectedItem).Item;
                cmbSource.BackColor = ColorNormal;
                UpdateSelectedDSI();
            }
            catch
            {
                cmbSource.BackColor = ColorError;
            }
        }

        void cmbTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedDSI.Dest = ((ListItem<SkeletonNode>)cmbTarget.SelectedItem).Item;
                cmbTarget.BackColor = ColorNormal;
                UpdateSelectedDSI();
            }
            catch
            {
                cmbTarget.BackColor = ColorError;
            }
        }

        public void DrawGame()
        {
            GameScreen.GraphicsDevice.Clear(Microsoft.Xna.Framework.Graphics.Color.CornflowerBlue);

            // Screen
            _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, _camera.Matrix);

            foreach (XNALine l in _centerLines)
            {
                l.Draw(_sb);
            }

            if (radioEdit.Checked)
            {
                // Edit skeleton
                _skeletonDrawer.Draw(_skeleton, _camera, _sb, SelectedNode);
                if (_frameBody != null && chkDrawBody.Checked)
                    _frameBody.Draw(_sb, Vector2.Zero);
            }
            else
            {
                // Animate skeletons
                if (chkDrawBody.Checked)
                    _skeletonAnim.Draw(_sb);
                if (chkDrawSkel.Checked)
                    _skeletonDrawer.Draw(_skeletonAnim.Skeleton, _camera, _sb);
            }
            _sb.End();
        }

        void GameScreen_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioEdit.Checked)
            {
                if (e.Button == MouseButtons.Left)
                {
                    // Select new node
                    var nodes = _skeleton.RootNode.GetAllNodes();
                    foreach (SkeletonNode node in nodes)
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

        void GameScreen_MouseMove(object sender, MouseEventArgs e)
        {
            _cursorPos = _camera.ToWorld(e.X, e.Y);
            lblXY.Text = string.Format("({0},{1})", Math.Round(_cursorPos.X, 0), Math.Round(_cursorPos.Y, 0));

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

        void GameScreen_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _moveSelectedNode = false;
        }

        static string GetDSIString(SkeletonBodyItem dsi)
        {
            string s = "_null_";
            if (dsi.Dest != null)
                s = dsi.Dest.Name;

            string textureName;
            if (dsi.Grh.GrhData == null)
                textureName = "*";
            else
                textureName = ((StationaryGrhData)dsi.Grh.GrhData).TextureName.Value.Replace("Character/", string.Empty);

            return textureName + ": " + dsi.Source.Name + " -> " + s;
        }

        static string GetLoadSkeletonDialogResult(string filter)
        {
            string result;

            using (OpenFileDialog fd = new OpenFileDialog())
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

            using (OpenFileDialog fd = new OpenFileDialog())
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

            using (SaveFileDialog fd = new SaveFileDialog())
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
            foreach (string file in files.Where(x => x.EndsWith(Skeleton.FileSuffix, StringComparison.OrdinalIgnoreCase)))
            {
                Skeleton skeleton = SkeletonLoader.LoadSkeleton(file);
                skeleton.Write(file);
            }

            // Sets
            foreach (string file in files.Where(x => x.EndsWith(SkeletonSet.FileSuffix, StringComparison.OrdinalIgnoreCase)))
            {
                SkeletonSet set = SkeletonLoader.LoadSkeletonSet(file);
                set.Write(file);
            }

            // Bodies
            foreach (string file in files.Where(x => x.EndsWith(SkeletonBodyInfo.FileSuffix, StringComparison.OrdinalIgnoreCase)))
            {
                SkeletonBodyInfo body = SkeletonLoader.LoadSkeletonBodyInfo(file);
                body.Save(file);
            }
        }

        void HandleSwitches(IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> switches)
        {
            if (switches == null || switches.Count() == 0)
                return;

            bool willClose = false;

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
                Timer t = new Timer { Interval = 1 };
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
            SkeletonSet newSet = SkeletonLoader.LoadSkeletonSet(filePath);
            _skeletonAnim.ChangeSet(newSet);

            FileAnim = filePath;
            txtFrames.Text = _skeletonAnim.SkeletonSet.GetFramesString();
            UpdateAnimationNodeCBs();
        }

        public void LoadBody(string filePath)
        {
            SkeletonBodyInfo bodyInfo = SkeletonLoader.LoadSkeletonBodyInfo(filePath);
            _skeletonAnim.SkeletonBody = new SkeletonBody(bodyInfo, _skeletonAnim.Skeleton);
            _frameBody = new SkeletonBody(bodyInfo, _skeleton);
            UpdateBodyList();
            FileBody = filePath;
        }

        public void LoadFrame(string filePath)
        {
            Skeleton newSkeleton = SkeletonLoader.LoadSkeleton(filePath);
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

        void lstBodies_SelectedIndexChanged(object sender, EventArgs e)
        {
            SkeletonBodyItem item = SelectedDSI;
            if (item == null)
                return;

            txtOffsetX.Text = item.ItemInfo.Offset.X.ToString();
            txtOffsetY.Text = item.ItemInfo.Offset.Y.ToString();
            txtOriginX.Text = item.ItemInfo.Origin.X.ToString();
            txtOriginY.Text = item.ItemInfo.Origin.Y.ToString();
            if (item.Grh.GrhData != null)
                txtGrhIndex.Text = item.Grh.GrhData.GrhIndex.ToString();

            foreach (ListItem<SkeletonNode> node in cmbSource.Items)
            {
                if (node.Item.Name == item.Source.Name)
                    cmbSource.SelectedItem = node;
            }

            if (item.Dest == null)
                cmbTarget.SelectedIndex = 0;
            else
            {
                foreach (ListItem<SkeletonNode> node in cmbTarget.Items)
                {
                    if (node.Item != null && node.Item.Name == item.Dest.Name)
                        cmbTarget.SelectedItem = node;
                }
            }
        }

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
                        SkeletonNode removeNode = SelectedNode;
                        SelectedNode = SelectedNode.Parent;
                        removeNode.Remove();
                    }
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            ks = e;
        }

        void radioAnimate_CheckedChanged(object sender, EventArgs e)
        {
            if (radioAnimate.Checked)
                SetAnimByTxt();
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

        void ScreenForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Create the engine objects
                _camera = new Camera2D(new Vector2(GameScreen.Width, GameScreen.Height)) { KeepInMap = false };
                _content = new ContentManager(GameScreen.Services, "Content");
                _sb = new SpriteBatch(GameScreen.GraphicsDevice);
                GrhInfo.Load(ContentPaths.Dev, _content);

                // Create the skeleton-related objects
                _skeleton = new Skeleton();
                Skeleton frameSkeleton = new Skeleton(SkeletonLoader.StandingSkeletonName, ContentPaths.Dev);
                SkeletonFrame frame = new SkeletonFrame(SkeletonLoader.StandingSkeletonName, frameSkeleton);
                _skeletonAnim = new SkeletonAnimation(GetTime(), frame);
                _skeletonDrawer = new SkeletonDrawer();

                _sb = new SpriteBatch(GameScreen.GraphicsDevice);

                LoadFrame(Skeleton.GetFilePath(SkeletonLoader.StandingSkeletonName, ContentPaths.Dev));
                LoadAnim(SkeletonSet.GetFilePath(SkeletonLoader.WalkingSkeletonSetName, ContentPaths.Dev));
                LoadBody(SkeletonBodyInfo.GetFilePath(SkeletonLoader.BasicSkeletonBodyName, ContentPaths.Dev));

                // Center lines
                _centerLines.Add(new XNALine(new Vector2(-100, 0), new Vector2(100, 0),
                                             Microsoft.Xna.Framework.Graphics.Color.Lime));
                _centerLines.Add(new XNALine(new Vector2(0, -5), new Vector2(0, 5), Microsoft.Xna.Framework.Graphics.Color.Red));

                _watch.Start();

                HandleSwitches(_switches);

                // Zoom in and start animation
                radioAnimate.Checked = true;
                chkDrawSkel.Checked = false;
                _camera.Zoom(new Vector2(0, -25), _camera.Size, 3f);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex);
                throw;
            }
        }

        void SetAnimByTxt()
        {
            try
            {
                SkeletonSet newSet = SkeletonLoader.LoadSkeletonSetFromString(txtFrames.Text, _skeletonSetFromStringDelimiter);
                if (newSet == null)
                    throw new Exception();

                _skeletonAnim.ChangeSet(newSet);
                txtFrames.BackColor = ColorNormal;
            }
            catch
            {
                txtFrames.BackColor = ColorError;
            }
        }

        void txtAngle_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedNode.SetAngle(Parser.Current.ParseFloat(txtAngle.Text));
                txtAngle.BackColor = ColorNormal;
            }
            catch
            {
                txtAngle.BackColor = ColorError;
            }
        }

        void txtFrames_TextChanged(object sender, EventArgs e)
        {
            SetAnimByTxt();
        }

        void txtGrhIndex_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GrhIndex grhIndex = Parser.Current.ParseGrhIndex(txtGrhIndex.Text);
                var grhData = GrhInfo.GetData(grhIndex);
                if (grhData == null)
                {
                    txtGrhIndex.BackColor = ColorError;
                    return;
                }

                SelectedDSI.Grh.SetGrh(grhData, AnimType.Loop, (int)_watch.ElapsedMilliseconds);
                SelectedDSI.ItemInfo.GrhIndex = grhIndex;

                txtGrhIndex.BackColor = ColorNormal;
                UpdateSelectedDSI();
            }
            catch
            {
                txtGrhIndex.BackColor = ColorError;
            }
        }

        void txtLength_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedNode.SetLength(Parser.Current.ParseFloat(txtLength.Text));
                txtLength.BackColor = ColorNormal;
            }
            catch
            {
                txtLength.BackColor = ColorError;
            }
        }

        void txtName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedNode.Name = txtName.Text;
                var item = (ListItem<SkeletonNode>)cmbSkeletonNodes.Items[cmbSkeletonNodes.SelectedIndex];
                item.Name = SelectedNode.Name;
                cmbSkeletonNodes.Items[cmbSkeletonNodes.SelectedIndex] = item;
                txtName.BackColor = ColorNormal;
            }
            catch
            {
                txtName.BackColor = ColorError;
            }
        }

        void txtOffsetX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float x = Parser.Current.ParseFloat(txtOffsetX.Text);
                SelectedDSI.ItemInfo.Offset = new Vector2(x, SelectedDSI.ItemInfo.Offset.Y);
                txtOffsetX.BackColor = ColorNormal;
            }
            catch
            {
                txtOffsetX.BackColor = ColorError;
            }
        }

        void txtOffsetY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float y = Parser.Current.ParseFloat(txtOffsetY.Text);
                SelectedDSI.ItemInfo.Offset = new Vector2(SelectedDSI.ItemInfo.Offset.X, y);
                txtOffsetY.BackColor = ColorNormal;
            }
            catch
            {
                txtOffsetY.BackColor = ColorError;
            }
        }

        void txtOriginX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float x = Parser.Current.ParseFloat(txtOriginX.Text);
                SelectedDSI.ItemInfo.Origin = new Vector2(x, SelectedDSI.ItemInfo.Origin.Y);
                txtOriginX.BackColor = ColorNormal;
            }
            catch
            {
                txtOriginX.BackColor = ColorError;
            }
        }

        void txtOriginY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float y = Parser.Current.ParseFloat(txtOriginY.Text);
                SelectedDSI.ItemInfo.Origin = new Vector2(SelectedDSI.ItemInfo.Origin.X, y);
                txtOriginY.BackColor = ColorNormal;
            }
            catch
            {
                txtOriginY.BackColor = ColorError;
            }
        }

        void txtX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (SelectedNode.Parent == null)
                    SelectedNode.MoveTo(new Vector2(Parser.Current.ParseFloat(txtX.Text), SelectedNode.Y));
                else
                    SelectedNode.X = Parser.Current.ParseFloat(txtX.Text);
                txtX.BackColor = ColorNormal;
            }
            catch
            {
                txtX.BackColor = ColorError;
            }
        }

        void txtY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (SelectedNode.Parent == null)
                    SelectedNode.MoveTo(new Vector2(SelectedNode.X, Parser.Current.ParseFloat(txtY.Text)));
                else
                    SelectedNode.Y = Parser.Current.ParseFloat(txtY.Text);
                txtY.BackColor = ColorNormal;
            }
            catch
            {
                txtY.BackColor = ColorError;
            }
        }

        void UpdateAnimationNodeCBs()
        {
            var nodes = _skeletonAnim.Skeleton.RootNode.GetAllNodes();
            cmbSource.Items.Clear();
            cmbTarget.Items.Clear();
            cmbTarget.Items.Add(new ListItem<SkeletonNode>(null, "_null_"));
            foreach (SkeletonNode node in nodes)
            {
                var listItem = new ListItem<SkeletonNode>(node, node.Name);
                cmbSource.Items.Add(listItem);
                cmbTarget.Items.Add(listItem);
            }
        }

        void UpdateBodyList()
        {
            lstBodies.Items.Clear();
            for (int i = 0; i < SkeletonBody.BodyItems.Length; i++)
            {
                SkeletonBodyItem item = SkeletonBody.BodyItems[i];
                lstBodies.AddItemAndReselect(new ListItem<SkeletonBodyItem>(item, GetDSIString(item)));
            }
            UpdateAnimationNodeCBs();
        }

        void UpdateFrameNodeCBs()
        {
            cmbSkeletonNodes.Items.Clear();
            var nodes = _skeleton.RootNode.GetAllNodes();
            foreach (SkeletonNode node in nodes)
            {
                cmbSkeletonNodes.Items.Add(new ListItem<SkeletonNode>(node, node.Name));
            }
        }

        public void UpdateGame()
        {
            if (!_watch.IsRunning)
                return;

            _currentTime = (int)_watch.ElapsedMilliseconds;
            _skeletonAnim.Update(_currentTime);
        }

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
            var dsi = (ListItem<SkeletonBodyItem>)lstBodies.Items[lstBodies.SelectedIndex];
            dsi.Name = GetDSIString(dsi.Item);
            lstBodies.Items[lstBodies.SelectedIndex] = dsi;
        }
    }
}