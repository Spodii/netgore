using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Graphics;
using NetGore.IO;
using SFML;

namespace NetGore.EditorTools.Grhs
{
    /// <summary>
    /// A <see cref="TreeNode"/> for the <see cref="GrhTreeView"/> that represents a single <see cref="GrhData"/>.
    /// </summary>
    [Serializable]
    public class GrhTreeViewNode : TreeNode, IGrhTreeViewNode
    {
        static readonly GrhImageList _grhImageList = GrhImageList.Instance;
        readonly GrhData _grhData;

        Grh _animationGrh = null;
        Image _image;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhTreeViewNode"/> class.
        /// </summary>
        /// <param name="treeView">The <see cref="GrhTreeView"/> this node belongs to.</param>
        /// <param name="grhData">The <see cref="GrhData"/> this node contains.</param>
        GrhTreeViewNode(GrhTreeView treeView, GrhData grhData)
        {
            _grhData = grhData;
            Update(treeView);
        }

        /// <summary>
        /// Gets the <see cref="GrhData"/> for this node.
        /// </summary>
        public GrhData GrhData
        {
            get { return _grhData; }
        }

        /// <summary>
        /// Gets if the image for this node ever needs to update.
        /// </summary>
        public bool NeedsToUpdateImage
        {
            get { return _animationGrh != null; }
        }

        /// <summary>
        /// Appends the frame information to the given <paramref name="sb"/>.
        /// </summary>
        /// <param name="frames">The frames to append the information for.</param>
        /// <param name="sb">The <see cref="StringBuilder"/> to append to.</param>
        static void AppendFramesToolTipText(IEnumerable<StationaryGrhData> frames, StringBuilder sb)
        {
            const string framePadding = "  ";
            const string frameSeperator = ",";

            var count = frames.Count();

            sb.AppendLine("Frames: " + count);
            sb.Append(framePadding);

            var i = 0;
            foreach (var frame in frames)
            {
                sb.Append(frame.GrhIndex);

                if ((++i) % 6 == 0)
                {
                    // Add a break every 6 indices
                    sb.AppendLine();
                    sb.Append(framePadding);
                }
                else
                {
                    // Separate the frame indicies
                    sb.Append(frameSeperator);
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="GrhTreeViewNode"/>.
        /// </summary>
        /// <param name="grhTreeView">The <see cref="GrhTreeView"/> that will contain the node.</param>
        /// <param name="grhData">The <see cref="GrhData"/> that the node will contain.</param>
        /// <returns>The new <see cref="GrhTreeViewNode"/>.</returns>
        public static GrhTreeViewNode Create(GrhTreeView grhTreeView, GrhData grhData)
        {
            var existingNode = grhTreeView.FindGrhDataNode(grhData);
            if (existingNode != null)
            {
                existingNode.Update();
                return existingNode;
            }

            return new GrhTreeViewNode(grhTreeView, grhData);
        }

        /// <summary>
        /// Creates the tooltip text to use for a <see cref="GrhData"/>.
        /// </summary>
        /// <returns>The tooltip text to use for a <see cref="GrhData"/>.</returns>
        string GetToolTipText()
        {
            var ret = string.Empty;

            try
            {
                if (GrhData is StationaryGrhData)
                    ret = GetToolTipTextStationary((StationaryGrhData)GrhData);
                else
                    ret = GetToolTipTextAnimated(GrhData);
            }
            catch (LoadingFailedException)
            {
            }

            return ret;
        }

        /// <summary>
        /// Gets the tool tip text for any animated <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/>.</param>
        /// <returns>The tool tip text for any animated <see cref="GrhData"/>.</returns>
        static string GetToolTipTextAnimated(GrhData grhData)
        {
            var sb = new StringBuilder();

            if (grhData is AutomaticAnimatedGrhData)
                sb.AppendLine("*Automatic Animated GrhData*");

            sb.AppendLine("Grh: " + grhData.GrhIndex);
            AppendFramesToolTipText(grhData.Frames, sb);
            sb.AppendLine();
            sb.Append("Speed: " + (1f / grhData.Speed));

            return sb.ToString();
        }

        /// <summary>
        /// Gets the tool tip text for a <see cref="StationaryGrhData"/>.
        /// </summary>
        /// <param name="grhData">The <see cref="StationaryGrhData"/>.</param>
        /// <returns>The tool tip text for a <see cref="StationaryGrhData"/>.</returns>
        static string GetToolTipTextStationary(StationaryGrhData grhData)
        {
            // Stationary
            var sb = new StringBuilder();

            sb.AppendLine("Grh: " + grhData.GrhIndex);
            sb.AppendLine("Texture: " + grhData.TextureName);

            var sourceRect = grhData.SourceRect;
            sb.AppendLine("Pos: (" + sourceRect.X + "," + sourceRect.Y + ")");
            sb.Append("Size: " + sourceRect.Width + "x" + sourceRect.Height);

            return sb.ToString();
        }

        void InsertIntoTree(GrhTreeView treeView)
        {
            Remove();
            var folder = GrhTreeViewFolderNode.Create(treeView, GrhData.Categorization.Category.ToString());
            folder.Nodes.Add(this);
        }

        public void RemoveRecursive()
        {
            var parent = Parent as GrhTreeViewFolderNode;
            Remove();

            if (parent != null)
                parent.RemoveIfEmpty();
        }

        /// <summary>
        /// Sets the icon for the node.
        /// </summary>
        void SetIconImage()
        {
            // Set the preview picture
            if (GrhData is StationaryGrhData)
                SetIconImageStationary((StationaryGrhData)GrhData);
            else
                SetIconImageAnimated(GrhData);
        }

        /// <summary>
        /// Sets the icon for an animated <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/>.</param>
        void SetIconImageAnimated(GrhData grhData)
        {
            // If we have already created the Grh for animations, we just need to make sure we have the
            // correct GrhData set on it
            if (_animationGrh != null)
            {
                _animationGrh.SetGrh(grhData);
                return;
            }

            _animationGrh = new Grh(grhData, AnimType.Loop, TickCount.Now);
            _image = _grhImageList.GetImage(grhData.GetFrame(0));
        }

        /// <summary>
        /// Sets the icon for a stationary <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData">The <see cref="StationaryGrhData"/>.</param>
        void SetIconImageStationary(StationaryGrhData grhData)
        {
            _image = _grhImageList.GetImage(grhData);
        }

        /// <summary>
        /// Makes the <see cref="GrhData"/> handled by this <see cref="GrhTreeViewNode"/> synchronize
        /// to the information displayed by the node (in opposed to <see cref="Update"/>, which makes the node
        /// update to match the <see cref="GrhData"/>).
        /// </summary>
        public void SyncGrhData()
        {
            // Check that the GrhData still exists
            var gd = GrhInfo.GetData(GrhData.GrhIndex);
            if (gd != _grhData)
            {
                RemoveRecursive();
                return;
            }

            var category = ((GrhTreeViewFolderNode)Parent).FullCategory;
            var title = Text;
            GrhData.SetCategorization(new SpriteCategorization(category, title));
        }

        /// <summary>
        /// Updates the <see cref="GrhTreeViewNode"/> for when anything involving the <see cref="GrhData"/> changes.
        /// </summary>
        public void Update()
        {
            Update((GrhTreeView)TreeView);
        }

        void Update(GrhTreeView treeView)
        {
            // Check that the GrhData still exists
            var gd = GrhInfo.GetData(GrhData.GrhIndex);
            if (gd != _grhData)
            {
                RemoveRecursive();
                return;
            }

            // Update everything
            var v = GrhData.GrhIndex.ToString();
            if (!StringComparer.Ordinal.Equals(v, Name))
                Name = v;

            v = GrhData.Categorization.Title.ToString();
            if (!StringComparer.Ordinal.Equals(v, Text))
                Text = v;

            v = GetToolTipText();
            if (!StringComparer.Ordinal.Equals(v, ToolTipText))
                ToolTipText = v;

            InsertIntoTree(treeView);
            SetIconImage();
        }

        internal void UpdateIconImage()
        {
            if (!NeedsToUpdateImage)
                return;

            // Store the GrhIndex of the animation before updating it to compare if there was a change
            var current = _animationGrh.CurrentGrhData;
            if (current == null)
            {
                _image = null;
                return;
            }

            var oldGrhData = current;

            // Update the Grh
            _animationGrh.Update(TickCount.Now);

            // Check that the GrhIndex changed from the update
            if (oldGrhData != _animationGrh.CurrentGrhData)
            {
                // Change the image
                _image = _grhImageList.GetImage(current);
                ((GrhTreeView)TreeView).RefreshNodeImage(this);
            }
        }

        #region IGrhTreeViewNode Members

        /// <summary>
        /// Gets the <see cref="Image"/> to use to draw the <see cref="GrhTreeViewNode"/>.
        /// </summary>
        public Image Image
        {
            get { return _image; }
        }

        #endregion
    }
}