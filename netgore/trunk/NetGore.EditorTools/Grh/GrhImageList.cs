using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using log4net;
using NetGore.EditorTools.Properties;
using NetGore.Graphics;
using NetGore.IO;
using Rectangle=SFML.Graphics.Rectangle;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Contains the <see cref="ImageList"/> used for the <see cref="GrhData"/>s to display the shrunken
    /// icon on the <see cref="GrhTreeView"/> or any other iconized <see cref="GrhData"/> preview.
    /// </summary>
    public class GrhImageList
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly GrhImageList _instance;

        readonly Dictionary<string, GrhImageListCacheItem> _imageCache =
            new Dictionary<string, GrhImageListCacheItem>(StringComparer.OrdinalIgnoreCase);

        readonly ImageList _imageList = new ImageList();

        /// <summary>
        /// If the cache is dirty. That is, if there is items that have been loaded into the <see cref="GrhImageList"/>
        /// since the last call to <see cref="GrhImageList.Save()"/>.
        /// </summary>
        bool _dirty = false;

        /// <summary>
        /// An <see cref="Image"/> used for when an <see cref="Image"/> failed to be created properly. This way, we
        /// at least have something to display.
        /// </summary>
        static readonly Image _errorImage;

        /// <summary>
        /// Initializes the <see cref="GrhImageList"/> class.
        /// </summary>
        static GrhImageList()
        {
            // Create the error image
            Bitmap bmp = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.DrawLine(Pens.Red, new Point(0, 0), new Point(bmp.Width, bmp.Height));
                g.DrawLine(Pens.Red, new Point(bmp.Width, 0), new Point(0, bmp.Height));
            }
            _errorImage = bmp;

            // Create the list instance
            _instance = new GrhImageList();
        }

        /// <summary>
        /// Initializes the <see cref="GrhImageList"/> class.
        /// </summary>
        GrhImageList()
        {
            ImageList.TransparentColor = EngineSettings.TransparencyColor.ToSystemColor();

            // Add the default image
            Image defaultImg = ImageHelper.CreateSolid(32, 32, EngineSettings.TransparencyColor.ToSystemColor());
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
            GrhInfo.Added += GrhInfo_Added;
            GrhInfo.Removed += GrhInfo_Removed;

            // Add the existing GrhDatas
            foreach (var gd in existingGrhDatas)
            {
                GrhInfo_Added(gd);
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
        public ImageList ImageList
        {
            get { return _imageList; }
        }

        /// <summary>
        /// Gets or sets the <see cref="GrhImageList"/> instance. If the value has not been set, the
        /// default <see cref="GrhImageList"/> provider will be used. As a result, this property will never
        /// return null.
        /// </summary>
        public static GrhImageList Instance
        {
            get { return _instance; }
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
        void AddImage(StationaryGrhData grhData)
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
        Image CreateImage(StationaryGrhData grhData)
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
            _dirty = true;
            Rectangle src = grhData.SourceRect;
            var dest = ImageList.ImageSize;

            var tex = grhData.Texture;
            if (tex == null)
            {
                const string errmsg = "Failed to acquire the texture for StationaryGrhData `{0}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, grhData);
                Debug.Fail(string.Format(errmsg, grhData));
                return _errorImage;
            }

            return tex.ToBitmap(src, dest.Width, dest.Height);
        }

        /// <summary>
        /// Gets the image key for a <see cref="GrhIndex"/>.
        /// </summary>
        /// <param name="grhIndex">The <see cref="GrhIndex"/> to get the image key for.</param>
        /// <returns>The image key for the <paramref name="grhIndex"/>.</returns>
        public static string GetImageKey(GrhIndex grhIndex)
        {
            if (grhIndex.IsInvalid)
                return string.Empty;

            return grhIndex.ToString();
        }

        /// <summary>
        /// Gets the image key for a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> to get the image key for.</param>
        /// <returns>The image key for the <paramref name="grhData"/>.</returns>
        public static string GetImageKey(StationaryGrhData grhData)
        {
            if (grhData == null)
                return string.Empty;

            if (!grhData.GrhIndex.IsInvalid)
            {
                // For normal GrhDatas, we return the unique GrhIndex
                return GetImageKey(grhData.GrhIndex);
            }
            else
            {
                // When we have a frame for a GrhData with an invalid GrhIndex, we prefix a "_" and the use the texture name
                string textureName = grhData.TextureName != null ? grhData.TextureName.ToString() : null;
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
        void GrhInfo_Added(GrhData grhData)
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
        void GrhInfo_Removed(GrhData grhData)
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
        /// Asynchronously prepares the <see cref="GrhImageList"/> now so that it won't have to be loaded later.
        /// This is not required to be called since if the instance has not been loaded when the
        /// <see cref="GrhImageList.Instance"/> is called, it will be loaded on-demand.
        /// </summary>
        public static void Prepare()
        {
            // Check that the instance isn't loaded to avoid creating an unneeded thread
            if (_instance != null)
                return;

            // Spawn a background thread to just grab the property since the property itself will handle loading
            // the instance if it is not already loaded
            Thread t = new Thread(() => Debug.Assert(Instance != null)) { IsBackground = true };
            t.Start();
        }

        /// <summary>
        /// Saves the <see cref="GrhImageList"/> information to a cache file so prevent having to recreate
        /// the images next time the <see cref="GrhImageList"/> is loaded.
        /// </summary>
        public void Save()
        {
            Save(false);
        }

        /// <summary>
        /// Saves the <see cref="GrhImageList"/> information to a cache file so prevent having to recreate
        /// the images next time the <see cref="GrhImageList"/> is loaded.
        /// </summary>
        /// <param name="forced">By default, the <see cref="GrhImageList"/> will only be saved if there has
        /// been changes to it since the last save or the cache file does not exist. If this value is set
        /// to true, the file will be saved anyways.</param>
        public void Save(bool forced)
        {
            if (!forced && !_dirty && File.Exists(CacheFilePath))
                return;

            _dirty = false;

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
        /// Gets the <see cref="Image"/> for a <see cref="Grh"/>.
        /// </summary>
        /// <param name="grh">The <see cref="Grh"/> to get the image for.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="grh"/>, or null if invalid.</returns>
        public Image TryGetImage(Grh grh)
        {
            if (grh == null)
                return null;

            return TryGetImage(grh.CurrentGrhData);
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> to get the image for.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="grhData"/>, or null if invalid.</returns>
        public Image TryGetImage(GrhData grhData)
        {
            if (grhData == null)
                return null;

            var gd = grhData.Frames.FirstOrDefault();
            if (gd == null)
                return null;

            return TryGetImage(gd.GrhIndex);
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for a <see cref="GrhIndex"/>.
        /// </summary>
        /// <param name="grhIndex">The <see cref="GrhIndex"/> to get the image for.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="grhIndex"/>, or null if invalid.</returns>
        public Image TryGetImage(GrhIndex grhIndex)
        {
            var imageKey = GetImageKey(grhIndex);
            if (string.IsNullOrEmpty(imageKey))
                return null;

            var images = ImageList.Images;
            if (!images.ContainsKey(imageKey))
                return null;

            return images[imageKey];
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for an object.
        /// </summary>
        /// <param name="o">The object to get the <see cref="Image"/> for. Multiple types are supported.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="o"/>, or null if invalid.</returns>
        public Image TryGetImage(object o)
        {
            if (o is int)
                return TryGetImage(new GrhIndex((int)o));
            else if (o is GrhIndex)
                return TryGetImage((GrhIndex)o);
            else if (o is IConvertible)
                return TryGetImage(new GrhIndex(((IConvertible)o).ToInt32(CultureInfo.InvariantCulture)));
            else if (o is Grh)
                return TryGetImage((Grh)o);
            else if (o is GrhData)
                return TryGetImage((GrhData)o);
            else
                return null;
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