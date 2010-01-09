using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NetGore.Graphics;
using NetGore.IO;

namespace NetGore.EditorTools
{
    /// <summary>
    /// A <see cref="TreeNode"/> for the <see cref="GrhTreeView"/> that represents a single <see cref="GrhData"/>.
    /// </summary>
    public class GrhTreeViewNode : TreeNode
    {
        readonly GrhData _grhData;

        Grh _animationGrh = null;

        GrhTreeViewNode(GrhTreeView treeView, GrhData grhData)
        {
            _grhData = grhData;
            Update(treeView);
        }

        public GrhData GrhData
        {
            get { return _grhData; }
        }

        public bool NeedsToUpdateImage
        {
            get { return _animationGrh != null; }
        }

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

        static string GetToolTipText(StationaryGrhData grhData)
        {
            // Stationary
            Rectangle sourceRect = grhData.SourceRect;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Grh: " + grhData.GrhIndex);
            sb.AppendLine("Texture: " + grhData.TextureName);
            sb.AppendLine("Pos: (" + sourceRect.X + "," + sourceRect.Y + ")");
            sb.Append("Size: " + sourceRect.Width + "x" + sourceRect.Height);

            return sb.ToString();
        }

        static void AppendFramesToolTipText(IEnumerable<StationaryGrhData> frames, StringBuilder sb)
        {
            const string framePadding = "  ";
            const string frameSeperator = ",";

            int count = frames.Count();

            sb.AppendLine("Frames: " + count);
            sb.Append(framePadding);

            int i = 0;
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

        static string GetToolTipText(AutomaticAnimatedGrhData grhData)
        {
            // Automatic Animated

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("*Automatic Animated GrhData*");
            sb.AppendLine("Grh: " + grhData.GrhIndex);
            AppendFramesToolTipText(grhData.Frames, sb);
            sb.AppendLine();
            sb.Append("Speed: " + (1f / grhData.Speed));

            return sb.ToString();
        }

        static string GetToolTipText(AnimatedGrhData grhData)
        {
            // Animated

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Grh: " + grhData.GrhIndex);
            AppendFramesToolTipText(grhData.Frames, sb);
            sb.AppendLine();
            sb.Append("Speed: " + (1f / grhData.Speed));

            return sb.ToString();
        }

        /// <summary>
        /// Creates the tooltip text to use for a <see cref="GrhData"/>.
        /// </summary>
        /// <returns>The tooltip text to use for a <see cref="GrhData"/>.</returns>
        string GetToolTipText()
        {
            string ret = string.Empty;

            try
            {
                if (GrhData is StationaryGrhData)
                {
                    ret = GetToolTipText((StationaryGrhData)GrhData);
                }
                else if (GrhData is AnimatedGrhData)
                {
                    ret = GetToolTipText((AnimatedGrhData)GrhData);
                }
                else if (GrhData is AutomaticAnimatedGrhData)
                {
                    ret = GetToolTipText((AutomaticAnimatedGrhData)GrhData);
                }
            }
            catch (ContentLoadException)
            {
            }

            return ret;
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

        void SetIconImage(StationaryGrhData grhData)
        {
            string imageKey = GrhImageList.GetImageKey(grhData);
            SetImageKeys(imageKey);
        }

        void SetIconImage(AnimatedGrhData grhData)
        {
            if (_animationGrh != null)
                return;

            _animationGrh = new Grh(grhData, AnimType.Loop, Environment.TickCount);

            string imageKey;
            var frame = grhData.GetFrame(0);
            if (frame != null)
            {
                imageKey = GrhImageList.GetImageKey(frame);
            }
            else
            {
                imageKey = null;
            }

            SetImageKeys(imageKey);
        }

        void SetIconImage(AutomaticAnimatedGrhData grhData)
        {
            if (_animationGrh != null)
                return;

            _animationGrh = new Grh(grhData, AnimType.Loop, Environment.TickCount);

            string imageKey;
            var frame = grhData.GetFrame(0);
            if (frame != null)
            {
                imageKey = GrhImageList.GetImageKey(frame);
            }
            else
            {
                imageKey = null;
            }

            SetImageKeys(imageKey);
        }

        void SetIconImage()
        {
            // Set the preview picture
            if (GrhData is StationaryGrhData)
            {
                SetIconImage((StationaryGrhData)GrhData);
            }
            else if (GrhData is AnimatedGrhData)
            {
                SetIconImage((AnimatedGrhData)GrhData);
            }
            else if (GrhData is AutomaticAnimatedGrhData)
            {
                SetIconImage((AutomaticAnimatedGrhData)GrhData);
            }
            else
            {
                throw new UnsupportedGrhDataTypeException(GrhData);
            }
        }

        void SetImageKeys(string imageKey)
        {
            if (ImageKey == imageKey)
                return;

            ImageKey = imageKey;
            SelectedImageKey = imageKey;
            StateImageKey = imageKey;
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

            string category = ((GrhTreeViewFolderNode)Parent).FullCategory;
            string title = Text;
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
            Name = GrhData.GrhIndex.ToString();
            Text = GrhData.Categorization.Title.ToString();
            ToolTipText = GetToolTipText();
            InsertIntoTree(treeView);
            SetIconImage();
        }

        public void UpdateIconImage()
        {
            if (!NeedsToUpdateImage)
                return;

            // Store the GrhIndex of the animation before updating it to compare if there was a change
            var current = _animationGrh.CurrentGrhData;
            if (current == null)
            {
                SetImageKeys(null);
                return;
            }

            GrhIndex oldGrhIndex = current.GrhIndex;

            // Update the Grh
            _animationGrh.Update(Environment.TickCount);

            // Check that the GrhIndex changed from the update
            if (oldGrhIndex != _animationGrh.CurrentGrhData.GrhIndex)
            {
                // Change the image
                string imageKey = GrhImageList.GetImageKey(current);
                SetImageKeys(imageKey);
            }
        }
    }
}