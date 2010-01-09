using System;
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

        /// <summary>
        /// Creates the tooltip text to use for a <see cref="GrhData"/>.
        /// </summary>
        /// <returns>The tooltip text to use for a <see cref="GrhData"/>.</returns>
        string GetToolTipText()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                var stationary = GrhData as StationaryGrhData;
                var animated = GrhData as AnimatedGrhData;

                if (stationary != null)
                {
                    // Stationary
                    Rectangle sourceRect = stationary.SourceRect;

                    sb.AppendLine("Grh: " + GrhData.GrhIndex);
                    sb.AppendLine("Texture: " + stationary.TextureName);
                    sb.AppendLine("Pos: (" + sourceRect.X + "," + sourceRect.Y + ")");
                    sb.Append("Size: " + sourceRect.Width + "x" + sourceRect.Height);
                }
                else if (animated != null)
                {
                    // Animated
                    const string framePadding = "  ";
                    const string frameSeperator = ",";

                    sb.AppendLine("Grh: " + GrhData.GrhIndex);
                    sb.AppendLine("Frames: " + animated.Frames.Length);

                    sb.Append(framePadding);
                    for (int i = 0; i < animated.Frames.Length; i++)
                    {
                        sb.Append(animated.Frames[i].GrhIndex);

                        if ((i + 1) % 6 == 0)
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

                    sb.AppendLine();
                    sb.Append("Speed: " + (1f / animated.Speed));

                    return sb.ToString();
                }
                else
                    throw new Exception("Unknown GrhData type...");
            }
            catch (ContentLoadException)
            {
            }

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

        void SetIconImage()
        {
            // Set the preview picture
            if (GrhData is StationaryGrhData)
            {
                // Static image
                string imageKey = GrhImageList.GetImageKey(GrhData);
                SetImageKeys(imageKey);
            }
            else if (GrhData is AnimatedGrhData)
            {
                // Animation
                var asAnimated = GrhData as AnimatedGrhData;
                if (_animationGrh == null)
                {
                    _animationGrh = new Grh(GrhData, AnimType.Loop, Environment.TickCount);
                    string imageKey = GrhImageList.GetImageKey(asAnimated.Frames[0]);
                    SetImageKeys(imageKey);
                }
            }
            else
                throw new Exception("Unsupported GrhData type...");
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
            GrhIndex oldGrhIndex = _animationGrh.CurrentGrhData.GrhIndex;

            // Update the Grh
            _animationGrh.Update(Environment.TickCount);

            // Check that the GrhIndex changed from the update
            if (oldGrhIndex != _animationGrh.CurrentGrhData.GrhIndex)
            {
                // Change the image
                string imageKey = GrhImageList.GetImageKey(_animationGrh.CurrentGrhData);
                SetImageKeys(imageKey);
            }
        }
    }
}