using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Extensions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.EditorTools;
using NetGore.Graphics;
using Color=System.Drawing.Color;

namespace DemoGame.MapEditor
{
    public partial class BatchRenameTextureForm : Form
    {
        static readonly Color ColorChanged = Color.Lime;
        static readonly Color ColorError = Color.Red;
        static readonly Color ColorNormal = SystemColors.Window;

        /// <summary>
        /// Character used to separate directories
        /// </summary>
        static readonly char DirSep = Path.DirectorySeparatorChar;

        readonly ContentManager _cm;
        readonly TreeNode _root;

        public BatchRenameTextureForm(TreeNode rootNode, ContentManager cm)
        {
            _cm = cm;
            _root = rootNode;
            InitializeComponent();
        }

        void BatchRenameTextureForm_Load(object sender, EventArgs e)
        {
            // Display the current tree location
            lblTreeLoc.Text = _root.FullPath;

            // Update the textures list
            lstTextures.Items.Clear();
            UpdateTextures(_root);
        }

        void btnApply_Click(object sender, EventArgs e)
        {
            // Do not try changing if an invalid item is selected
            if (lstTextures.SelectedItem == null)
                return;

            string oldTexture = (string)lstTextures.SelectedItem;
            if (string.IsNullOrEmpty(oldTexture))
                return;

            // Validate the new texture
            string newTexture = txtCurrent.Text;
            if (!IsValidTexture("Grh" + DirSep + newTexture))
            {
                MessageBox.Show("The new entered texture is invalid.");
                return;
            }

            // Change all GrhDatas with the old texture to the new one
            ChangeTexture(_root, oldTexture, newTexture);

            // Change successful
            string txt = string.Format("Texture successfully changed from {2}{0}{2} -- to -- {2}{1}", oldTexture, newTexture,
                                       Environment.NewLine);
            MessageBox.Show(txt, "Change successful!", MessageBoxButtons.OK);
        }

        static void ChangeTexture(TreeNode root, string oldTexture, string newTexture)
        {
            if (root.Nodes.Count > 0)
            {
                // Recursively loop through folders
                foreach (TreeNode node in root.Nodes)
                {
                    ChangeTexture(node, oldTexture, newTexture);
                }
            }
            else
            {
                // Get the GrhData for the TreeNode
                GrhData gd = GrhTreeView.GetGrhData(root);
                if (gd == null)
                {
                    Debug.Fail("Invalid GrhData found for node.");
                    return;
                }

                // Check if the texure matches, and change if it does
                if (gd.TextureName == oldTexture)
                {
                    // To change the TextureName, we actually have to recreate the object
                    GrhData newGD = new GrhData();
                    newGD.Load(gd.ContentManager, gd.GrhIndex, newTexture, gd.X, gd.Y, gd.Width, gd.Height, gd.Category, gd.Title);

                    // Remove the old GrhData and add the new one
                    GrhInfo.GrhDatas.RemoveAt(gd.GrhIndex);
                    GrhInfo.GrhDatas[gd.GrhIndex] = newGD;
                }
            }
        }

        /// <summary>
        /// Checks if a texture is valid
        /// </summary>
        /// <param name="texture">Content path of the texture to check</param>
        /// <returns>True if a value texture, else false</returns>
        bool IsValidTexture(string texture)
        {
            try
            {
                // Load the texture into the content manager
                _cm.Load<Texture2D>(texture);
            }
            catch
            {
                // Exception raised, invalid texture
                return false;
            }

            // No exception, is valid texture
            return true;
        }

        void lstTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the referenced list
            lstReferenced.Items.Clear();

            if (lstTextures.SelectedItem == null)
            {
                // Invalid selection
                txtCurrent.Enabled = false;
                txtCurrent.Text = string.Empty;
            }
            else
            {
                // Valid selection, display in current textbox and show GrhDatas using this texture
                txtCurrent.Enabled = true;
                txtCurrent.Text = (string)lstTextures.SelectedItem;
                UpdateReferences(_root, (string)lstTextures.SelectedItem);
            }

            // Update the images
            picOldTexture.ImageLocation = ContentPaths.Dev.Grhs.Join(txtCurrent.Text + ".png");
            picNewTexture.ImageLocation = picOldTexture.ImageLocation;
        }

        void txtCurrent_TextChanged(object sender, EventArgs e)
        {
            // Do not try changing if an invalid item is selected
            if (lstTextures.SelectedItem == null)
                return;

            if (txtCurrent.Text == (string)lstTextures.SelectedItem)
            {
                // Same as original texture
                txtCurrent.BackColor = ColorNormal;
                picNewTexture.ImageLocation = picOldTexture.ImageLocation;
            }
            else
            {
                string texturePath = "Grh" + DirSep + txtCurrent.Text;

                // Try to change to the specified texture
                if (IsValidTexture(texturePath))
                {
                    txtCurrent.BackColor = ColorChanged;
                    picNewTexture.ImageLocation = ContentPaths.Dev.Grhs.Join(txtCurrent.Text + ".png");
                }
                else
                {
                    txtCurrent.BackColor = ColorError;
                    picNewTexture.ImageLocation = string.Empty;
                }
            }
        }

        void UpdateReferences(TreeNode root, string texture)
        {
            if (root.Nodes.Count > 0)
            {
                // Recursively loop through all directories
                foreach (TreeNode node in root.Nodes)
                {
                    UpdateReferences(node, texture);
                }
            }
            else
            {
                // Get the GrhData and, if using the texture, add it to the list
                GrhData gd = GrhTreeView.GetGrhData(root);
                if (gd == null)
                {
                    Debug.Fail("Invalid GrhData found for node.");
                    return;
                }

                if (gd.TextureName == texture)
                    lstReferenced.Items.Add(gd);
            }
        }

        /// <summary>
        /// Recursively adds all unique textures used by GrhDatas from the root TreeNode
        /// </summary>
        /// <param name="root">Root TreeNode</param>
        void UpdateTextures(TreeNode root)
        {
            if (root.Nodes.Count > 0)
            {
                // Add all the child nodes since this is a folder
                foreach (TreeNode node in root.Nodes)
                {
                    UpdateTextures(node);
                }
            }
            else
            {
                // Get the GrhData for the node
                GrhData gd = GrhTreeView.GetGrhData(root);
                if (gd == null)
                {
                    Debug.Fail("Invalid GrhData found for node.");
                    return;
                }

                // If the texture isn't already listed, add it
                if (!lstTextures.Items.Contains(gd.TextureName))
                    lstTextures.Items.Add(gd.TextureName);
            }
        }
    }
}