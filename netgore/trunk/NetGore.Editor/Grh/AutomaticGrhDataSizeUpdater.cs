using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Content;
using NetGore.Graphics;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// Handles updating the size of a <see cref="StationaryGrhData"/> where <see cref="StationaryGrhData.AutomaticSize"/> is set. This
    /// class helps ensure all sizes are up-to-date without having to update stuff that has not changed. It is recommended that this
    /// class is run whenever an editor starts up. This is not needed by the client since the client shouldn't ever have to do any
    /// updating of the information for content assets.
    /// </summary>
    public class AutomaticGrhDataSizeUpdater
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const string _rootNodeName = "AutoGrhDataSizes";
        static readonly AutomaticGrhDataSizeUpdater _instance;

        readonly Dictionary<GrhIndex, CacheItemInfo> _cache = new Dictionary<GrhIndex, CacheItemInfo>();

        /// <summary>
        /// Initializes the <see cref="AutomaticGrhDataSizeUpdater"/> class.
        /// </summary>
        static AutomaticGrhDataSizeUpdater()
        {
            _instance = new AutomaticGrhDataSizeUpdater();
        }

        /// <summary>
        /// Initializes the <see cref="AutomaticGrhDataSizeUpdater"/> class.
        /// </summary>
        protected AutomaticGrhDataSizeUpdater()
        {
            // Load the cache
            var cacheList = Load();

            _cache.Clear();

            // Use the cache as a dictionary instead of a list to perform fast look-ups
            foreach (var item in cacheList)
            {
                _cache.Add(item.GrhIndex, item);
            }
        }

        /// <summary>
        /// Gets the path to the file used for the cache.
        /// </summary>
        protected virtual string CacheFile
        {
            get { return ContentPaths.Dev.Data.Join("GrhDataAutoSizeCache" + EngineSettings.DataFileSuffix); }
        }

        /// <summary>
        /// Gets the <see cref="AutomaticGrhDataSizeUpdater"/> instance.
        /// </summary>
        public static AutomaticGrhDataSizeUpdater Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the size of a file.
        /// </summary>
        /// <param name="filePath">The path of the file to get the size of.</param>
        /// <returns>The size of the file at the <paramref name="filePath"/>.</returns>
        protected virtual uint GetFileSize(string filePath)
        {
            var fi = new FileInfo(filePath);
            Debug.Assert(fi.Length >= uint.MinValue && fi.Length <= uint.MaxValue);
            return (uint)fi.Length;
        }

        /// <summary>
        /// Loads a <see cref="List{T}"/> containing the <see cref="CacheItemInfo"/> values in the cache file.
        /// </summary>
        /// <returns>The <see cref="List{T}"/> containing the <see cref="CacheItemInfo"/> values in the cache file.</returns>
        protected virtual IEnumerable<CacheItemInfo> Load()
        {
            var ret = new List<CacheItemInfo>();

            // Make sure the cache file exists
            if (!File.Exists(CacheFile))
                return ret;

            // Load the items
            var r = XmlValueReader.CreateFromFile(CacheFile, _rootNodeName);
            var loadedItems = r.ReadManyNodes("Item", x => new CacheItemInfo(x));

            ret.AddRange(loadedItems);

            return ret;
        }

        /// <summary>
        /// Saves the cache items to the cache file.
        /// </summary>
        public virtual void Save()
        {
            var values =  _cache.Values.OrderBy(x => x.GrhIndex).ToImmutable();
            using (var writer = XmlValueWriter.Create(CacheFile, _rootNodeName))
            {
                writer.WriteManyNodes("Item", values, (w, x) => x.WriteState(w));
            }
        }

        static string TryGetAbsoluteFilePath(StationaryGrhData gd, ContentPaths contentPath)
        {
            string ret = null;
            var isValid = true;

            try
            {
                ret = gd.TextureName.GetAbsoluteFilePath(contentPath);
                if (!File.Exists(ret))
                    isValid = false;
            }
            catch
            {
                isValid = false;
            }

            if (!isValid)
            {
                const string errmsg =
                    "Could not update the size of GrhData `{0}` since the file for the texture named `{1}`" +
                    " could not be found in the ContentPaths.Dev. Expected file: {2}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, gd, gd.TextureName, ret ?? "[NULL]");
                return null;
            }
            else
                return ret;
        }

        /// <summary>
        /// Updates the sizes of the <see cref="StationaryGrhData"/>s where <see cref="StationaryGrhData.AutomaticSize"/> is set and
        /// the cached size is invalid.
        /// </summary>
        public virtual void UpdateSizes()
        {
            // Loop through all StationaryGrhDatas
            foreach (var gd in GrhInfo.GrhDatas.OfType<StationaryGrhData>())
            {
                // Check that AutomaticSize is set
                if (!gd.AutomaticSize)
                    continue;

                // Make sure the asset exists in the dev path
                var devTexturePath = TryGetAbsoluteFilePath(gd, ContentPaths.Dev);
                if (devTexturePath == null)
                    continue;

                // Get the size of the file from the Dev path since we will be needing it later
                var realFileSize = GetFileSize(devTexturePath);

                // Check if the GrhData is in the cache
                CacheItemInfo cacheItem;
                if (_cache.TryGetValue(gd.GrhIndex, out cacheItem))
                {
                    Debug.Assert(gd.GrhIndex == cacheItem.GrhIndex);

                    // The GrhData was in the cache, so check that the cache information is still valid
                    if (gd.Width == cacheItem.Width && gd.Height == cacheItem.Height && gd.TextureName == cacheItem.AssetName &&
                        realFileSize == cacheItem.FileSize)
                    {
                        // All of the cached values match the values in the GrhData, so assume its up-to-date and move to the next
                        continue;
                    }
                }

                // Make sure the asset exists in the build path
                var buildTexturePath = TryGetAbsoluteFilePath(gd, ContentPaths.Build);
                if (buildTexturePath == null)
                    continue;

                // The GrhData was not in the cache or the cache contains outdated values, so find the real size by grabbing it directly
                // from the GrhData's texture
                var realSize = gd.Texture.Size;

                // To avoid using too much memory, we will dispose of the texture after we are done. Since the textures are lazy-loaded and
                // automatically reload, this will not do any harm. We may end up disposing a few textures that were actually being used
                // right now, but that is not a big deal.
                gd.Texture.Dispose();

                // Set the new size
                gd.UpdateAutomaticSize(realSize);

                // Update the cache
                var addToCache = false;

                if (cacheItem == null)
                {
                    cacheItem = new CacheItemInfo();
                    addToCache = true;
                }

                cacheItem.AssetName = gd.TextureName;
                cacheItem.Width = (int)realSize.X;
                cacheItem.Height = (int)realSize.Y;
                cacheItem.GrhIndex = gd.GrhIndex;
                cacheItem.FileSize = realFileSize;

                // Since the CacheItemInfo is an object, we only need to add it to the dictionary if it didn't already exist in it
                if (addToCache)
                    _cache.Add(cacheItem.GrhIndex, cacheItem);
            }

            // Be sure to remove any cache items for GrhDatas that no longer exist just to make sure they don't cause any problems
            // later (and to help remove some storage overhead)
            var toRemove = new Stack<GrhIndex>();
            foreach (var cacheItem in _cache)
            {
                if (GrhInfo.GetData(cacheItem.Key) == null)
                    toRemove.Push(cacheItem.Key);
            }

            while (toRemove.Count > 0)
            {
                var removeGrhIndex = toRemove.Pop();
                _cache.Remove(removeGrhIndex);
            }

            // Save the cache and the GrhDatas
            Save();
            GrhInfo.Save(ContentPaths.Dev);
        }

        /// <summary>
        /// Contains information describing an image in the cache of a <see cref="AutomaticGrhDataSizeUpdater"/>.
        /// </summary>
        protected class CacheItemInfo : IPersistable
        {
            ushort _height;
            ushort _width;

            /// <summary>
            /// Initializes a new instance of the <see cref="CacheItemInfo"/> class.
            /// </summary>
            public CacheItemInfo()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CacheItemInfo"/> class.
            /// </summary>
            /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
            public CacheItemInfo(IValueReader reader)
            {
                ReadState(reader);
            }

            /// <summary>
            /// Gets or sets the <see cref="ContentAssetName"/> that was used when building this cached information.
            /// </summary>
            [SyncValue]
            public ContentAssetName AssetName { get; set; }

            /// <summary>
            /// Gets or sets the size of the image file.
            /// </summary>
            [SyncValue]
            public uint FileSize { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="GrhIndex"/> that this information is for.
            /// </summary>
            [SyncValue]
            public GrhIndex GrhIndex { get; set; }

            /// <summary>
            /// Gets or sets the height of the image.
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than <see cref="ushort.MinValue"/>
            /// or greater than <see cref="ushort.MaxValue"/>.</exception>
            [SyncValue]
            public int Height
            {
                get { return _height; }
                set
                {
                    if (value < ushort.MinValue || value > ushort.MaxValue)
                        throw new ArgumentOutOfRangeException("value");

                    _height = (ushort)value;
                }
            }

            /// <summary>
            /// Gets or sets the width of the image.
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than <see cref="ushort.MinValue"/>
            /// or greater than <see cref="ushort.MaxValue"/>.</exception>
            [SyncValue]
            public int Width
            {
                get { return _width; }
                set
                {
                    if (value < ushort.MinValue || value > ushort.MaxValue)
                        throw new ArgumentOutOfRangeException("value");

                    _width = (ushort)value;
                }
            }

            #region IPersistable Members

            /// <summary>
            /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
            /// same order as they were written.
            /// </summary>
            /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
            public void ReadState(IValueReader reader)
            {
                PersistableHelper.Read(this, reader);
            }

            /// <summary>
            /// Writes the state of the object to an <see cref="IValueWriter"/>.
            /// </summary>
            /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
            public void WriteState(IValueWriter writer)
            {
                PersistableHelper.Write(this, writer);
            }

            #endregion
        }
    }
}