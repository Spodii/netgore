using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.EditorTools.Properties;
using NetGore.Graphics;
using Rectangle=Microsoft.Xna.Framework.Rectangle;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Contains the ImageList used for the GrhDatas.
    /// </summary>
    public static class GrhImageList
    {
        static readonly Dictionary<GrhIndex, GrhImageListCacheItem> _imageCache =
            new Dictionary<GrhIndex, GrhImageListCacheItem>();

        static readonly ImageList _imageList = new ImageList();

        /// <summary>
        /// Gets the key for the special image of a closed folder.
        /// </summary>
        public static string ClosedFolderKey
        {
            get { return "_folder"; }
        }

        /// <summary>
        /// Gets the key for the default image.
        /// </summary>
        public static string DefaultImageKey
        {
            get { return "_default"; }
        }

        /// <summary>
        /// Gets the ImageList used for the GrhDatas.
        /// </summary>
        public static ImageList ImageList
        {
            get { return _imageList; }
        }

        /// <summary>
        /// Gets the key for the special image of an open folder.
        /// </summary>
        public static string OpenFolderKey
        {
            get { return "_openfolder"; }
        }

        /// <summary>
        /// GrhImageList static constructor.
        /// </summary>
        static GrhImageList()
        {
            ImageList.TransparentColor = Color.Magenta;

            // Add the default image
            Image defaultImg = ImageHelper.CreateSolid(32, 32, Color.Magenta);
            ImageList.Images.Add(defaultImg);

            // Add the special images
            ImageList.Images.Add(OpenFolderKey, Resources.folderopen);
            ImageList.Images.Add(ClosedFolderKey, Resources.folder);

            // Load the cache
            var cacheItems = GrhImageListCache.Load();
            foreach (GrhImageListCacheItem item in cacheItems)
            {
                _imageCache.Add(item.GrhIndex, item);
            }

            // Listen for new GrhDatas being added/removed
            GrhInfo.OnAdd += GrhInfo_OnAdd;
            GrhInfo.OnRemove += GrhInfo_OnRemove;

            // Add the existing GrhDatas
            foreach (GrhData gd in GrhInfo.GrhDatas)
            {
                AddImage(gd);
            }
        }

        /// <summary>
        /// Adds the image for a GrhData to the ImageList.
        /// </summary>
        /// <param name="grhData">GrhData to add.</param>
        static void AddImage(GrhData grhData)
        {
            if (grhData == null)
                return;

            if (string.IsNullOrEmpty(grhData.TextureName))
                return;

            string key = GetImageKey(grhData);

            Image img = CreateImage(grhData);

            if (ImageList.Images.ContainsKey(key))
                ImageList.Images.RemoveByKey(key);

            ImageList.Images.Add(key, img);
        }

        /// <summary>
        /// Creates the Image for a GrhData.
        /// </summary>
        /// <param name="grhData">GrhData to create the image for.</param>
        /// <returns>The Image for the <paramref name="grhData"/>.</returns>
        static Image CreateImage(GrhData grhData)
        {
            if (grhData == null || string.IsNullOrEmpty(grhData.TextureName))
                return null;

            GrhImageListCacheItem cacheItem;
            if (_imageCache.TryGetValue(grhData.GrhIndex, out cacheItem))
            {
                if (grhData.OriginalSourceRect == cacheItem.SourceRect)
                    return cacheItem.Image;
            }

            int destWidth = ImageList.ImageSize.Width;
            int destHeight = ImageList.ImageSize.Height;

            Rectangle sourceRect = grhData.SourceRect;
            return ImageHelper.CreateFromTexture(grhData.Texture, sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height,
                                                 destWidth, destHeight);
        }

        /// <summary>
        /// Gets the image key for a GrhData.
        /// </summary>
        /// <param name="grhData">The GrhData to get the image key for.</param>
        /// <returns>The image key for the <paramref name="grhData"/>.</returns>
        public static string GetImageKey(GrhData grhData)
        {
            return grhData.GrhIndex.ToString();
        }

        /// <summary>
        /// Handles when a GrhData is added to the global GrhData list.
        /// </summary>
        /// <param name="grhData">GrhData that was added.</param>
        static void GrhInfo_OnAdd(GrhData grhData)
        {
            AddImage(grhData);
        }

        /// <summary>
        /// Handles when a GrhData is removed from the global GrhData list.
        /// </summary>
        /// <param name="grhData">GrhData that was removed.</param>
        static void GrhInfo_OnRemove(GrhData grhData)
        {
            string key = GetImageKey(grhData);
            ImageList.Images.RemoveByKey(key);
        }
    }
}