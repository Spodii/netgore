using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.EditorTools.Properties;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    public static class GrhImageList
    {
        static readonly ImageList _imageList = new ImageList();

        public static ImageList ImageList { get { return _imageList; } }

        /// <summary>
        /// Gets the key for the special image of an open folder.
        /// </summary>
        public static string OpenFolderKey { get { return "_openfolder"; } }

        /// <summary>
        /// Gets the key for the special image of a closed folder.
        /// </summary>
        public static string ClosedFolderKey { get { return "_folder"; } }

        /// <summary>
        /// Gets the key for the default image.
        /// </summary>
        public static string DefaultImageKey { get { return "_default"; } }

        static GrhImageList()
        {
            ImageList.TransparentColor = Color.Magenta;

            // Add the default image
            Image defaultImg = ImageHelper.CreateSolid(32, 32, Color.Magenta);
            ImageList.Images.Add(defaultImg);

            // Add the special images
            ImageList.Images.Add(OpenFolderKey, Resources.folderopen);
            ImageList.Images.Add(ClosedFolderKey, Resources.folder);

            // Listen for new GrhDatas being added/removed
            GrhInfo.OnAdd += GrhInfo_OnAdd;
            GrhInfo.OnRemove += GrhInfo_OnRemove;

            // Add the existing GrhDatas
            foreach (var gd in GrhInfo.GrhDatas)
                AddImage(gd);
        }

        static Image CreateImage(GrhData grhData)
        {
            if (grhData == null || string.IsNullOrEmpty(grhData.TextureName))
                return null;

            int destWidth = ImageList.ImageSize.Width;
            int destHeight = ImageList.ImageSize.Height;
  
            var sourceRect = grhData.SourceRect;
            return ImageHelper.CreateFromTexture(grhData.Texture, sourceRect.X, sourceRect.Y, sourceRect.Width,
                                                        sourceRect.Height, destWidth, destHeight);
        }

        public static string GetImageKey(GrhData grhData)
        {
            return grhData.GrhIndex.ToString();
        }

        static void AddImage(GrhData grhData)
        {
            if (grhData == null)
                return;

            if (string.IsNullOrEmpty(grhData.TextureName))
                return;

            Image img = CreateImage(grhData);

            var key = GetImageKey(grhData);

            if (ImageList.Images.ContainsKey(key))
                ImageList.Images.RemoveByKey(key);

            ImageList.Images.Add(key, img);
        }

        static void GrhInfo_OnRemove(GrhData grhData)
        {
            var key = GetImageKey(grhData);
            ImageList.Images.RemoveByKey(key);
        }

        static void GrhInfo_OnAdd(GrhData grhData)
        {
            AddImage(grhData);
        }
    }
}
