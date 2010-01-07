using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore.EditorTools.Properties;
using NetGore.Graphics;
using NetGore.IO;
using Rectangle=Microsoft.Xna.Framework.Rectangle;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Contains the <see cref="ImageList"/> used for the <see cref="GrhData"/>s to display the shrunken
    /// icon on the <see cref="GrhTreeView"/> or any other iconized <see cref="GrhData"/> preview.
    /// </summary>
    static class GrhImageList
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly Dictionary<GrhIndex, GrhImageListCacheItem> _imageCache =
            new Dictionary<GrhIndex, GrhImageListCacheItem>();

        static readonly ImageList _imageList = new ImageList();

        /// <summary>
        /// Initializes the <see cref="GrhImageList"/> class.
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

            // Read the cache
            var cacheItems = Load();
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
        /// Gets the path to the file to use for the <see cref="GrhImageList"/> cache.
        /// </summary>
        static string CacheFilePath
        {
            get { return ContentPaths.Build.Data.Join("grhimagelistcache.bin"); }
        }

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
        /// Adds the image for a GrhData to the ImageList.
        /// </summary>
        /// <param name="grhData">GrhData to add.</param>
        static void AddImage(GrhData grhData)
        {
            if (grhData == null || grhData.IsAnimated)
                return;

            string key = GetImageKey(grhData);
            Image img = CreateImage(grhData);

            // If the image already exists, remove the old one and add this new one
            if (ImageList.Images.ContainsKey(key))
            {
                var tempImg = ImageList.Images[key];
                ImageList.Images.RemoveByKey(key);

                if (tempImg != null)
                    tempImg.Dispose();
            }

            // Check that the image was created successfully
            if (img == null)
            {
                const string errmsg =
                    "Failed to create image for GrhData `{0}` - GrhImageList.CreateImage() returned a null image.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, grhData);
            }
            else
                ImageList.Images.Add(key, img);
        }

        /// <summary>
        /// Creates the Image for a GrhData.
        /// </summary>
        /// <param name="grhData">GrhData to create the image for.</param>
        /// <returns>The Image for the <paramref name="grhData"/>.</returns>
        static Image CreateImage(GrhData grhData)
        {
            if (grhData == null || grhData.TextureName == null)
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
        /// Gets the image key for a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> to get the image key for.</param>
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

        /// <summary>
        /// Loads the <see cref="GrhImageListCacheItem"/>s from the <see cref="GrhImageList"/> cache file.
        /// </summary>
        /// <returns>An IEnumerable of the loaded <see cref="GrhImageListCacheItem"/>s. If the cache file
        /// does not exist, this will be empty.</returns>
        static IEnumerable<GrhImageListCacheItem> Load()
        {
            GrhImageListCacheItem[] ret;

            if (!File.Exists(CacheFilePath))
                return Enumerable.Empty<GrhImageListCacheItem>();

            using (FileStream stream = new FileStream(CacheFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    int count = r.ReadInt32();
                    ret = new GrhImageListCacheItem[count];

                    for (int i = 0; i < count; i++)
                    {
                        GrhImageListCacheItem item = GrhImageListCacheItem.Read(r);
                        ret[i] = item;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Saves the <see cref="GrhImageList"/> information to a file cache.
        /// </summary>
        public static void Save()
        {
            var grhDatas = GrhInfo.GrhDatas.ToArray();
            var validItems = new Stack<GrhImageListCacheItem>(grhDatas.Length);

            foreach (GrhData gd in grhDatas)
            {
                string key = GetImageKey(gd);
                Image image = ImageList.Images[key];

                if (image == null)
                    continue;

                GrhImageListCacheItem item = new GrhImageListCacheItem(gd.GrhIndex, image, gd.OriginalSourceRect);
                validItems.Push(item);
            }

            using (FileStream stream = new FileStream(CacheFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192))
            {
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    int count = validItems.Count;
                    w.Write(count);

                    foreach (GrhImageListCacheItem item in validItems)
                    {
                        item.Write(w);
                    }
                }
            }
        }

        /// <summary>
        /// Contains the information for a single <see cref="GrhImageList"/> item in the cache.
        /// </summary>
        struct GrhImageListCacheItem
        {
            /// <summary>
            /// The <see cref="GrhIndex"/> of the cached item.
            /// </summary>
            public readonly GrhIndex GrhIndex;

            /// <summary>
            /// The <see cref="Image"/> for the <see cref="GrhData"/>.
            /// </summary>
            public readonly Image Image;

            /// <summary>
            /// The <see cref="Microsoft.Xna.Framework.Rectangle"/> describing the source that the <see cref="Image"/> came from.
            /// </summary>
            public readonly Rectangle SourceRect;

            /// <summary>
            /// Initializes a new instance of the <see cref="GrhImageListCacheItem"/> struct.
            /// </summary>
            /// <param name="grhIndex">The <see cref="GrhIndex"/>.</param>
            /// <param name="image">The image.</param>
            /// <param name="sourceRect">The source rect.</param>
            public GrhImageListCacheItem(GrhIndex grhIndex, Image image, Rectangle sourceRect)
            {
                GrhIndex = grhIndex;
                Image = image;
                SourceRect = sourceRect;
            }

            public static GrhImageListCacheItem Read(BinaryReader r)
            {
                int x = r.ReadInt32();
                int y = r.ReadInt32();
                int w = r.ReadInt32();
                int h = r.ReadInt32();
                int i = r.ReadInt32();

                int l = r.ReadInt32();
                var b = new byte[l];
                r.Read(b, 0, l);

                Image img;
                using (MemoryStream ms = new MemoryStream(b))
                {
                    img = Image.FromStream(ms);
                }

                return new GrhImageListCacheItem(new GrhIndex(i), img, new Rectangle(x, y, w, h));
            }

            public void Write(BinaryWriter w)
            {
                w.Write(SourceRect.X);
                w.Write(SourceRect.Y);
                w.Write(SourceRect.Width);
                w.Write(SourceRect.Height);
                w.Write((int)GrhIndex);

                byte[] asArray;
                using (MemoryStream ms = new MemoryStream())
                {
                    Image.Save(ms, ImageFormat.Png);
                    asArray = ms.ToArray();
                }

                w.Write(asArray.Length);
                w.Write(asArray);
            }
        }
    }
}