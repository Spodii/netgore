using System;
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

        static readonly Dictionary<string, GrhImageListCacheItem> _imageCache =
            new Dictionary<string, GrhImageListCacheItem>(StringComparer.OrdinalIgnoreCase);

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
                _imageCache.Add(item.Key, item);
            }

            // Get the existing GrhDatas before hooking to the events just in case hooking to the events results in
            // GrhDatas being added... or something...
            var existingGrhDatas = GrhInfo.GrhDatas.ToImmutable();

            // Listen for new GrhDatas being added/removed
            GrhInfo.OnAdd += GrhInfo_OnAdd;
            GrhInfo.OnRemove += GrhInfo_OnRemove;

            // Add the existing GrhDatas
            foreach (var gd in existingGrhDatas)
            {
                GrhInfo_OnAdd(gd);
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
        static void AddImage(StationaryGrhData grhData)
        {
            if (grhData == null)
                return;

            string key = GetImageKey(grhData);
            Image img = CreateImage(grhData);

            // If the image already exists, remove the old one and add this new one. This should only happen if, for some
            // reason, we try to add the StationaryGrhData to the image list twice.
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
        static Image CreateImage(StationaryGrhData grhData)
        {
            if (grhData == null || grhData.TextureName == null)
                return null;

            // Check if the item is in the cache
            GrhImageListCacheItem cacheItem;
            var key = GetImageKey(grhData);
            if (_imageCache.TryGetValue(key, out cacheItem))
            {
                if (grhData.OriginalSourceRect == cacheItem.SourceRect)
                {
                    // Item was in the cache and fits the conditions, so use it
                    return cacheItem.Image;
                }
                else
                {
                    // Conditions didn't match the conditions of the cached copy, so remove it from the cache
                    _imageCache.Remove(key);
                }
            }

            // Item wasn't in the cache, so we have to create it
            Rectangle src = grhData.SourceRect;
            var dest = ImageList.ImageSize;
            return ImageHelper.CreateFromTexture(grhData.Texture, src.X, src.Y, src.Width, src.Height, dest.Width, dest.Height);
        }

        /// <summary>
        /// Gets the image key for a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> to get the image key for.</param>
        /// <returns>The image key for the <paramref name="grhData"/>.</returns>
        public static string GetImageKey(StationaryGrhData grhData)
        {
            if (grhData.GrhIndex != AutomaticAnimatedGrhData.FrameGrhIndex)
            {
                // For normal GrhDatas, we return the unique GrhIndex
                return grhData.GrhIndex.ToString();
            }
            else
            {
                // When we have a frame for an AutomaticAnimatedGrhData, we prefix a "_" and the use the texture name
                string textureName = grhData.TextureName.ToString();
                if (string.IsNullOrEmpty(textureName))
                    return string.Empty;
                else
                    return "_" + textureName;
            }
        }

        /// <summary>
        /// Handles when a GrhData is added to the global GrhData list.
        /// </summary>
        /// <param name="grhData">GrhData that was added.</param>
        static void GrhInfo_OnAdd(GrhData grhData)
        {
            if (grhData is StationaryGrhData)
            {
                // For stationary GrhDatas, its easy enough - just add it
                AddImage((StationaryGrhData)grhData);
            }
            else if (grhData is AutomaticAnimatedGrhData)
            {
                // For AutomaticAnimatedGrhDatas, we will want to loop through the frames to add them
                foreach (var frame in grhData.Frames)
                {
                    AddImage(frame);
                }
            }
        }

        /// <summary>
        /// Handles when a GrhData is removed from the global GrhData list.
        /// </summary>
        /// <param name="grhData">GrhData that was removed.</param>
        static void GrhInfo_OnRemove(GrhData grhData)
        {
            if (grhData is StationaryGrhData)
            {
                // For stationary GrhDatas, just remove it
                string key = GetImageKey((StationaryGrhData)grhData);
                ImageList.Images.RemoveByKey(key);
            }
            else if (grhData is AutomaticAnimatedGrhData)
            {
                // For AutomaticAnimatedGrhDatas, we have to remove each frame
                foreach (var frame in grhData.Frames)
                {
                    string key = GetImageKey(frame);
                    ImageList.Images.RemoveByKey(key);
                }
            }
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
            var grhDatas = GrhInfo.GrhDatas.OfType<StationaryGrhData>().ToArray();
            var validItems = new Stack<GrhImageListCacheItem>(grhDatas.Length);

            // Only save the unique GrhDatas that we have an image for
            foreach (var gd in GrhInfo.GrhDatas.SelectMany(x => x.Frames).Distinct())
            {
                string key = GetImageKey(gd);
                Image image = ImageList.Images[key];

                if (image == null)
                    continue;

                GrhImageListCacheItem item = new GrhImageListCacheItem(key, image, gd.OriginalSourceRect);
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
        class GrhImageListCacheItem
        {
            /// <summary>
            /// The <see cref="Image"/> for the <see cref="GrhData"/>.
            /// </summary>
            public readonly Image Image;

            /// <summary>
            /// The key of the cached item.
            /// </summary>
            public readonly string Key;

            /// <summary>
            /// The <see cref="Microsoft.Xna.Framework.Rectangle"/> describing the source that the <see cref="Image"/> came from.
            /// </summary>
            public readonly Rectangle SourceRect;

            /// <summary>
            /// Initializes a new instance of the <see cref="GrhImageListCacheItem"/> struct.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="image">The image.</param>
            /// <param name="sourceRect">The source rect.</param>
            public GrhImageListCacheItem(string key, Image image, Rectangle sourceRect)
            {
                Key = key;
                Image = image;
                SourceRect = sourceRect;
            }

            public static GrhImageListCacheItem Read(BinaryReader r)
            {
                int x = r.ReadInt32();
                int y = r.ReadInt32();
                int w = r.ReadInt32();
                int h = r.ReadInt32();
                string key = r.ReadString();

                int len = r.ReadInt32();
                var b = new byte[len];
                r.Read(b, 0, len);

                MemoryStream ms = new MemoryStream(b);
                Image img = Image.FromStream(ms);

                return new GrhImageListCacheItem(key, img, new Rectangle(x, y, w, h));
            }

            public void Write(BinaryWriter w)
            {
                w.Write(SourceRect.X);
                w.Write(SourceRect.Y);
                w.Write(SourceRect.Width);
                w.Write(SourceRect.Height);
                w.Write(Key);

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