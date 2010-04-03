using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.IO;
using NetGore;

namespace NetGore.EditorTools
{
    public partial class MissingTexturesForm : Form
    {
        readonly IContentManager _cm;
        readonly TextureHashCollection _hashCollection;
        readonly Dictionary<string, List<GrhData>> _missingTextures;

        public MissingTexturesForm(TextureHashCollection hashCollection, IEnumerable<GrhData> missingTextures, IContentManager cm)
        {
            if (hashCollection == null)
                throw new ArgumentNullException("hashCollection");
            if (missingTextures == null)
                throw new ArgumentNullException("missingTextures");
            if (cm == null)
                throw new ArgumentNullException("cm");

            InitializeComponent();

            _cm = cm;
            _hashCollection = hashCollection;

            // Get the missing GrhDatas
            _missingTextures = CreateTextureDict(missingTextures);
        }

        void ApplyBtn_Click(object sender, EventArgs e)
        {
            // Ensure the new texture is valid
            string newTexture = NewTxt.Text;
            if (string.IsNullOrEmpty(newTexture))
                return;
            if (!new ContentAssetName(newTexture).ContentExists())
                return;

            // Find the selected item
            TextureListItem selectedItem = TextureList.SelectedItem as TextureListItem;
            if (selectedItem == null)
                return;

            // Change all of the GrhDatas
            string currTexture = selectedItem.TextureName;
            foreach (var gd in selectedItem.GrhDatas.OfType<StationaryGrhData>())
            {
                if (gd.TextureName.ToString() != currTexture)
                    Debug.Fail("Somehow we got a GrhData with a wrong texture!");
                else
                    gd.ChangeTexture(newTexture);
            }

            // Ensure we got everything
            Debug.Assert(
                GrhInfo.GrhDatas.OfType<StationaryGrhData>().Where(x => x.TextureName.ToString() == currTexture).Count() == 0,
                "One or more textures failed to be changed!");

            RemoveSelectedTextureListItem();
        }

        void BadGrhDatasList_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextureListItem selectedItem = TextureList.SelectedItem as TextureListItem;
            if (selectedItem == null)
            {
                NewTxt.Enabled = false;
                GrhDatasList.Enabled = false;
                return;
            }

            NewTxt.Enabled = true;
            GrhDatasList.Enabled = true;

            CurrentTxt.Text = selectedItem.TextureName;
            NewTxt.Text = _hashCollection.FindBestMatchTexture(selectedItem.TextureName) ?? string.Empty;

            GrhDatasList.Items.Clear();
            foreach (GrhData grhData in selectedItem.GrhDatas)
            {
                GrhDatasList.AddItemAndReselect(grhData.Categorization.ToString());
            }
        }

        /// <summary>
        /// Creates a Dictionary from a collection of GrhDatas, where the key is the TextureName and
        /// the value is a List of all of the GrhDatas that use that texture.
        /// </summary>
        /// <param name="grhDatas">GrhDatas to include in the Dictionary.</param>
        /// <returns>A Dictionary from a collection of GrhDatas, where the key is the TextureName and
        /// the value is a List of all of the GrhDatas that use that texture.</returns>
        static Dictionary<string, List<GrhData>> CreateTextureDict(IEnumerable<GrhData> grhDatas)
        {
            var ret = new Dictionary<string, List<GrhData>>();
            foreach (var gd in grhDatas.OfType<StationaryGrhData>())
            {
                List<GrhData> list;
                if (!ret.TryGetValue(gd.TextureName.ToString(), out list))
                {
                    list = new List<GrhData>();
                    ret.Add(gd.TextureName.ToString(), list);
                }

                list.Add(gd);
            }

            return ret;
        }

        void DeleteBtn_Click(object sender, EventArgs e)
        {
            // Find the selected item
            TextureListItem selectedItem = TextureList.SelectedItem as TextureListItem;
            if (selectedItem == null)
                return;

            // Delete every GrhData using the texture
            foreach (GrhData gd in selectedItem.GrhDatas)
            {
                GrhInfo.Delete(gd);
            }

            RemoveSelectedTextureListItem();
        }

        void MissingTexturesForm_Load(object sender, EventArgs e)
        {
            // Populate the list
            TextureList.Items.Clear();
            foreach (var item in _missingTextures)
            {
                TextureListItem listItem = new TextureListItem(item.Key, item.Value);
                TextureList.AddItemAndReselect(listItem);
            }
        }

        void NewTxt_TextChanged(object sender, EventArgs e)
        {
            string textureName = NewTxt.Text;

            var assetName = new ContentAssetName(NewTxt.Text);
            if (!string.IsNullOrEmpty(textureName) && assetName.ContentExists())
            {
                NewTxt.BackColor = EditorColors.Normal;

                Texture2D texture = _cm.Load<Texture2D>(assetName, ContentLevel.Temporary);
                int w = texture.Width;
                int h = texture.Height;

                Image previewImage = ImageHelper.CreateFromTexture(texture, 0, 0, w, h, PreviewPic.Width, PreviewPic.Height);
                PreviewPic.Image = previewImage;
            }
            else
            {
                PreviewPic.Image = null;
                NewTxt.BackColor = EditorColors.Error;
            }
        }

        void RemoveSelectedTextureListItem()
        {
            object selectedItem = TextureList.SelectedItem;
            if (selectedItem == null)
                return;

            // Remove the entry
            TextureList.RemoveItemAndReselect(selectedItem);

            // Auto-select the first item in the list
            if (TextureList.Items.Count > 0)
                TextureList.SelectedIndex = 0;
            else
                Close();
        }

        /// <summary>
        /// A single item for the TextureList
        /// </summary>
        class TextureListItem
        {
            /// <summary>
            /// IEnumerable of the GrhDatas that use this texture.
            /// </summary>
            public readonly IEnumerable<GrhData> GrhDatas;

            /// <summary>
            /// Name of the texture.
            /// </summary>
            public readonly string TextureName;

            public TextureListItem(string textureName, IEnumerable<GrhData> grhDatas)
            {
                TextureName = textureName;
                GrhDatas = grhDatas;
            }

            public override string ToString()
            {
                return TextureName;
            }
        }
    }
}