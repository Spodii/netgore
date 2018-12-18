using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using SFML;
using SFML.Audio;
using SFML.Graphics;

namespace NetGore.Content
{
    /// <summary>
    /// An implementation of <see cref="IContentManager"/> for managing content in various levels of persistence.
    /// This way you can easily clear groups of content instead of clearing and having to re-load all content.
    /// </summary>
    public class ContentManager : IContentManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// When the length of a file name for a lazy asset exceeds this length, trim it down to only show the
        /// last characters, length of which is defined by <see cref="_lazyAssetTrimmedFileNameLength"/>.
        /// </summary>
        const int _lazyAssetTrimFileNameLength = 105;

        /// <summary>
        /// The length of a trimmed file name for lazy asset.
        /// </summary>
        const int _lazyAssetTrimmedFileNameLength = 100;

        /// <summary>
        /// Gets the minimum amount of time that an asset must go unused before being unloaded.
        /// </summary>
        const int _minElapsedTimeToUnload = 1000 * 60; // 1 minute

        static readonly object _instanceSync = new object();
        static readonly StringComparer _stringComp = StringComparer.OrdinalIgnoreCase;
        static IContentManager _instance;

        readonly object _assetSync = new object();
        readonly Dictionary<string, IMyLazyAsset>[] _loadedAssets;
        readonly Dictionary<string, IMyLazyAsset> _trackedLoads = new Dictionary<string, IMyLazyAsset>(_stringComp);

        bool _isDisposed = false;
        bool _isTrackingLoads = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManager"/> class.
        /// </summary>
        ContentManager()
        {
            RootDirectory = ContentPaths.Build.Root;

            // Create the dictionaries for each level
            var maxLevel = EnumHelper<ContentLevel>.MaxValue;
            _loadedAssets = new Dictionary<string, IMyLazyAsset>[maxLevel + 1];

            for (var i = 0; i < _loadedAssets.Length; i++)
            {
                _loadedAssets[i] = new Dictionary<string, IMyLazyAsset>(_stringComp);
            }
        }

        /// <summary>
        /// Changes the <see cref="ContentLevel"/> of an asset. This should only be called from a block
        /// locked by <see cref="_assetSync"/>.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="oldLevel">The old <see cref="ContentLevel"/>.</param>
        /// <param name="newLevel">The new <see cref="ContentLevel"/>.</param>
        /// <returns>True if the change was successful; false if the asset is not loaded or was not
        /// in the <paramref name="oldLevel"/>.</returns>
        bool ChangeAssetLevelNoLock(string assetName, ContentLevel oldLevel, ContentLevel newLevel)
        {
            if (oldLevel == newLevel)
                return false;

            // Grab from the old dictionary
            var oldDict = _loadedAssets[(int)oldLevel];
            IMyLazyAsset asset;
            if (!oldDict.TryGetValue(assetName, out asset))
                return false;

            // Remove
            if (!oldDict.Remove(assetName))
                Debug.Fail("Uhm... how the hell...?");

            // Add to the new dictionary
            var newDict = _loadedAssets[(int)newLevel];
            newDict.Add(assetName, asset);

            return true;
        }

        /// <summary>
        /// Creates a <see cref="IContentManager"/>.
        /// </summary>
        /// <returns>The <see cref="IContentManager"/> instance.</returns>
        public static IContentManager Create()
        {
            lock (_instanceSync)
            {
                return _instance ?? (_instance = new ContentManager());
            }
        }

        /// <summary>
        /// Disposes of the content manager.
        /// </summary>
        /// <param name="disposeManaged">If false, this was called from the destructor.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            Unload();
        }

        /// <summary>
        /// Does the actual work of unloading assets.
        /// </summary>
        /// <param name="level">The <see cref="ContentLevel"/> of the content to unload.</param>
        /// <param name="ignoreTime">If true, the last-used time will be ignored.</param>
        void DoUnload(ContentLevel level, bool ignoreTime)
        {
            var currTime = TickCount.Now;

            lock (_assetSync)
            {
                // Loop through the given level and all levels below it
                for (var i = (int)level; i < _loadedAssets.Length; i++)
                {
                    // Get the dictionary for the level
                    var dic = _loadedAssets[i];

                    // Dispose all items that haven't been used for the needed amount of time
                    foreach (var asset in dic.Values)
                    {
                        try
                        {
                            if (!ignoreTime && (currTime - asset.LastUsedTime < _minElapsedTimeToUnload))
                                continue;

                            asset.Dispose();
                        }
                        catch (Exception ex)
                        {
                            const string errmsg = "Failed to dispose asset `{0}`: {1}";
                            if (log.IsWarnEnabled)
                                log.WarnFormat(errmsg, asset, ex);
                            Debug.Fail(string.Format(errmsg, asset, ex));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ContentManager"/> is reclaimed by garbage collection.
        /// </summary>
        ~ContentManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the absolute file path for an asset.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <returns>The absolute file path for the <paramref name="assetName"/>.</returns>
        public static string GetAssetPath(string assetName)
        {
            var pipeIndex = assetName.LastIndexOf('|');
            if (pipeIndex > 0)
                assetName = assetName.Substring(0, pipeIndex);

            return ContentPaths.Build.Root.Join(assetName + ContentPaths.ContentFileSuffix);
        }

        protected static LoadingFailedException InvalidTypeException(object obj, Type expected)
        {
            const string errmsg = "Invalid type `{0}` specified for asset `{1}`, which is of type `{2}`.";
            var err = string.Format(errmsg, expected, obj, obj.GetType());
            if (log.IsErrorEnabled)
                log.Error(err);
            return new LoadingFailedException(err);
        }

        /// <summary>
        /// Checks if an asset is loaded.
        /// </summary>
        /// <param name="assetName">The name of the asset to look for.</param>
        /// <param name="asset">When this method returns true, contains the asset instance.</param>
        /// <param name="level">When this method returns true, contains the asset's <see cref="ContentLevel"/>.</param>
        /// <returns>True if the asset is loaded; otherwise false.</returns>
        bool IsAssetLoaded(string assetName, out IMyLazyAsset asset, out ContentLevel level)
        {
            for (var i = 0; i < _loadedAssets.Length; i++)
            {
                if (_loadedAssets[i].TryGetValue(assetName, out asset))
                {
                    level = (ContentLevel)i;
                    return true;
                }
            }

            asset = null;
            level = 0;
            return false;
        }

        /// <summary>
        /// Gets the ToString() for a lazy asset.
        /// </summary>
        /// <param name="type">The name of the lazy asset type.</param>
        /// <param name="fileName">The file path.</param>
        /// <returns>The string to display.</returns>
        static string LazyAssetToString(string type, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "Lazy" + type + " []";
            else if (fileName.Length > _lazyAssetTrimFileNameLength)
            {
                return "Lazy" + type + " [" +
                       fileName.Substring(fileName.Length - _lazyAssetTrimmedFileNameLength, _lazyAssetTrimmedFileNameLength) +
                       "]";
            }
            else
                return "Lazy" + type + " [" + fileName + "]";
        }

        /// <summary>
        /// Loads an asset.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <param name="loader">The loader.</param>
        /// <returns>The loaded asset.</returns>
        /// <exception cref="ObjectDisposedException"><c>ObjectDisposedException</c>.</exception>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>level</c> is out of range.</exception>
        IMyLazyAsset Load(string assetName, ContentLevel level, Func<string, IMyLazyAsset> loader)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(ToString());

            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            var levelInt = (int)level;
            if (levelInt >= _loadedAssets.Length || levelInt < 0)
                throw new ArgumentOutOfRangeException("level", string.Format("Invalid ContentLevel `{0}` value specified.", level));

            lock (_assetSync)
            {
                // Check if the asset is already loaded
                IMyLazyAsset existingAsset;
                ContentLevel existingLevel;
                if (IsAssetLoaded(assetName, out existingAsset, out existingLevel))
                {
                    // If the specified level parameter is greater than the asset's current level, promote it
                    if (level < existingLevel)
                    {
                        var success = ChangeAssetLevelNoLock(assetName, existingLevel, level);
                        Debug.Assert(success);
                    }

                    return existingAsset;
                }

                // Load a new asset and add it into the appropriate level
                var asset = loader(assetName);
                _loadedAssets[levelInt].Add(assetName, asset);

                if (IsTrackingLoads && !_trackedLoads.ContainsKey(assetName))
                    _trackedLoads.Add(assetName, asset);

                return asset;
            }
        }

        /// <summary>
        /// Reads an asset from file.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="fontSize">The font size.</param>
        /// <returns>The loaded asset.</returns>
        /// <exception cref="ObjectDisposedException"><c>ObjectDisposedException</c>.</exception>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        protected Font ReadAssetFont(string assetName, int fontSize)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(ToString());

            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            var ret = new MyLazyFont(GetAssetPath(assetName), (uint)fontSize);

            return ret;
        }

        /// <summary>
        /// Reads an asset from file.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <returns>The asset instance.</returns>
        /// <exception cref="ObjectDisposedException"><c>ObjectDisposedException</c>.</exception>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        protected Texture ReadAssetImage(string assetName)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(ToString());

            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            var path = GetAssetPath(assetName);
            var ret = new MyLazyImage(path);

            return ret;
        }

        /// <summary>
        /// Reads an asset from file.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <returns>The loaded asset.</returns>
        /// <exception cref="ObjectDisposedException"><c>ObjectDisposedException</c>.</exception>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        protected SoundBuffer ReadAssetSoundBuffer(string assetName)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(ToString());

            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            var ret = new MyLazySoundBuffer(GetAssetPath(assetName));

            return ret;
        }

        static string SanitizeAssetName(string assetName)
        {
            return assetName.Replace('\\', '/');
        }

        #region IContentManager Members

        /// <summary>
        /// Gets if this object is disposed.
        /// </summary>
        /// <value></value>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets if <see cref="IContentManager.BeginTrackingLoads"/> has been called and loaded items are being tracked.
        /// This will be set false when <see cref="IContentManager.EndTrackingLoads"/> is called.
        /// </summary>
        public bool IsTrackingLoads
        {
            get { return _isTrackingLoads; }
        }

        /// <summary>
        /// Gets or sets absolute path to the root content directory. The default value, which is
        /// <see cref="ContentPaths.Build"/>'s root, should be fine for most all cases.
        /// </summary>
        public PathString RootDirectory { get; set; }

        /// <summary>
        /// Starts tracking items that are loaded by this <see cref="IContentManager"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IContentManager.IsTrackingLoads"/> is true.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IsTrackingLoads")]
        public void BeginTrackingLoads()
        {
            if (IsTrackingLoads)
                throw new InvalidOperationException("IsTrackingLoads must be false.");

            _isTrackingLoads = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets all of the assets that were loaded since <see cref="IContentManager.BeginTrackingLoads"/> was called.
        /// Content that was unloaded will still be included in the returned collection.
        /// </summary>
        /// <returns>
        /// All of the assets that were loaded since <see cref="IContentManager.BeginTrackingLoads"/>
        /// was called.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="IContentManager.IsTrackingLoads"/> is false.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IsTrackingLoads")]
        public IEnumerable<KeyValuePair<string, object>> EndTrackingLoads()
        {
            if (!IsTrackingLoads)
                throw new InvalidOperationException("IsTrackingLoads must be true.");

            _isTrackingLoads = false;

            var ret = _trackedLoads.Select(x => new KeyValuePair<string, object>(x.Key, x.Value)).ToArray();
            _trackedLoads.Clear();

            return ret;
        }

        /// <summary>
        /// Loads a <see cref="Font"/> asset.
        /// </summary>
        /// <param name="assetName">The name of the asset to load.</param>
        /// <param name="fontSize">The size of the font.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>The loaded asset.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetName" /> is <c>null</c>.</exception>
        public Font LoadFont(string assetName, int fontSize, ContentLevel level)
        {
            if (assetName == null)
                throw new ArgumentNullException("assetName");

            assetName = SanitizeAssetName(assetName);

            assetName += "|" + fontSize;
            var ret = Load(assetName, level, x => (IMyLazyAsset)ReadAssetFont(x, fontSize));
            return (Font)ret;
        }

        /// <summary>
        /// Loads an <see cref="Texture"/> asset.
        /// </summary>
        /// <param name="assetName">The name of the asset to load.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>The loaded asset.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetName" /> is <c>null</c>.</exception>
        public Texture LoadImage(string assetName, ContentLevel level)
        {
            if (assetName == null)
                throw new ArgumentNullException("assetName");

            assetName = SanitizeAssetName(assetName);

            var ret = Load(assetName, level, x => (IMyLazyAsset)ReadAssetImage(x));
            return (Texture)ret;
        }

        /// <summary>
        /// Loads a <see cref="SoundBuffer"/> asset.
        /// </summary>
        /// <param name="assetName">The name of the asset to load.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>The loaded asset.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetName" /> is <c>null</c> or empty.</exception>
        public SoundBuffer LoadSoundBuffer(string assetName, ContentLevel level)
        {
            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            assetName = SanitizeAssetName(assetName);

            var ret = Load(assetName, level, x => (IMyLazyAsset)ReadAssetSoundBuffer(x));
            return (SoundBuffer)ret;
        }

        /// <summary>
        /// Sets the level of an asset.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The new <see cref="ContentLevel"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetName" /> is <c>null</c> or empty.</exception>
        public void SetLevel(string assetName, ContentLevel level)
        {
            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            assetName = SanitizeAssetName(assetName);

            lock (_assetSync)
            {
                IMyLazyAsset asset;
                ContentLevel currLevel;
                if (!IsAssetLoaded(assetName, out asset, out currLevel))
                    return;

                if (currLevel == level)
                    return;

                ChangeAssetLevelNoLock(assetName, currLevel, level);
            }
        }

        /// <summary>
        /// Sets the level of an asset only if the specified level is greater than the current level.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The new <see cref="ContentLevel"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetName" /> is <c>null</c> or empty.</exception>
        public void SetLevelMax(string assetName, ContentLevel level)
        {
            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            assetName = SanitizeAssetName(assetName);

            lock (_assetSync)
            {
                IMyLazyAsset asset;
                ContentLevel currLevel;
                if (!IsAssetLoaded(assetName, out asset, out currLevel))
                    return;

                if (currLevel <= level)
                    return;

                ChangeAssetLevelNoLock(assetName, currLevel, level);
            }
        }

        /// <summary>
        /// Sets the level of an asset only if the specified level is lower than the current level.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The new <see cref="ContentLevel"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetName" /> is <c>null</c> or empty.</exception>
        public void SetLevelMin(string assetName, ContentLevel level)
        {
            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            assetName = SanitizeAssetName(assetName);

            lock (_assetSync)
            {
                IMyLazyAsset asset;
                ContentLevel currLevel;
                if (!IsAssetLoaded(assetName, out asset, out currLevel))
                    return;

                if (currLevel >= level)
                    return;

                ChangeAssetLevelNoLock(assetName, currLevel, level);
            }
        }

        /// <summary>
        /// Gets the content level of an asset.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">When this method returns true, contains the <see cref="ContentLevel"/>
        /// of the asset.</param>
        /// <returns>True if the asset was found; otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetName" /> is <c>null</c> or empty.</exception>
        public bool TryGetContentLevel(string assetName, out ContentLevel level)
        {
            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            assetName = SanitizeAssetName(assetName);

            lock (_assetSync)
            {
                IMyLazyAsset o;
                return IsAssetLoaded(assetName, out o, out level);
            }
        }

        /// <summary>
        /// Unloads all content from the specified <see cref="ContentLevel"/>, and all levels
        /// below that level.
        /// </summary>
        /// <param name="level">The level of the content to unload. The content at this level, and all levels below it will be
        /// unloaded. The default is <see cref="ContentLevel.Global"/> to unload content from all levels.</param>
        /// <param name="ignoreTime">If true, the content in the <paramref name="level"/> will be forced to be unloaded even
        /// if it was recently used. By default, this is false to prevent excessive reloading. Usually you will only set this
        /// value to true if you are processing a lot of content at the same time just once, which usually only happens
        /// in the editors.</param>
        public void Unload(ContentLevel level = ContentLevel.Global, bool ignoreTime = false)
        {
            if (IsDisposed)
                return;

            DoUnload(level, ignoreTime);
        }

        #endregion

        /// <summary>
        /// Interface for the lazy assets of the <see cref="ContentManager"/>.
        /// </summary>
        interface IMyLazyAsset : IDisposable
        {
            /// <summary>
            /// Gets the <see cref="TickCount.Now"/> that this asset was last used.
            /// </summary>
            TickCount LastUsedTime { get; }
        }

        /// <summary>
        /// <see cref="LazyFont"/> implementation specifically for the <see cref="ContentManager"/>.
        /// </summary>
        sealed class MyLazyFont : LazyFont, IMyLazyAsset
        {
            TickCount _lastUsed;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:SFML.Graphics.LazyFont"/> class.
            /// </summary>
            /// <param name="filename">Font file to load</param><param name="charSize">Character size</param><exception cref="T:SFML.LoadingFailedException"/>
            public MyLazyFont(string filename, uint charSize) : base(filename, charSize)
            {
                _lastUsed = TickCount.Now;
            }

            /// <summary>
            /// Access to the internal pointer of the object.
            /// For internal use only
            /// </summary>
            public override IntPtr CPointer
            {
                get
                {
                    _lastUsed = TickCount.Now;
                    return base.CPointer;
                }
            }

            /// <summary>
            /// Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return LazyAssetToString("Font", FileName);
            }

            #region IMyLazyAsset Members

            /// <summary>
            /// Gets the <see cref="TickCount.Now"/> that this asset was last used.
            /// </summary>
            public TickCount LastUsedTime
            {
                get { return _lastUsed; }
            }

            #endregion
        }

        /// <summary>
        /// <see cref="LazyTexture"/> implementation specifically for the <see cref="ContentManager"/>.
        /// </summary>
        sealed class MyLazyImage : LazyTexture, IMyLazyAsset
        {
            TickCount _lastUsed;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:SFML.Graphics.LazyImage"/> class.
            /// </summary>
            /// <param name="filename">The file name.</param>
            public MyLazyImage(string filename) : base(filename)
            {
                _lastUsed = TickCount.Now;
            }

            /// <summary>
            /// Access to the internal pointer of the object.
            /// For internal use only
            /// </summary>
            public override IntPtr CPointer
            {
                get
                {
                    _lastUsed = TickCount.Now;
                    return base.CPointer;
                }
            }

            /// <summary>
            /// When overridden in the derived class, handles when the <see cref="T:SFML.Graphics.LazyImage"/> is reloaded.
            /// </summary>
            protected override void OnReload()
            {
                Smooth = false;
                using (var img = CopyToImage())
                {
                    img.CreateMaskFromColor(EngineSettings.TransparencyColor);
                    Update(img);
                }
            }

            /// <summary>
            /// Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return LazyAssetToString("Image", FileName);
            }

            #region IMyLazyAsset Members

            /// <summary>
            /// Gets the <see cref="TickCount.Now"/> that this asset was last used.
            /// </summary>
            public TickCount LastUsedTime
            {
                get { return _lastUsed; }
            }

            #endregion
        }

        /// <summary>
        /// <see cref="LazySoundBuffer"/> implementation specifically for the <see cref="ContentManager"/>.
        /// </summary>
        sealed class MyLazySoundBuffer : LazySoundBuffer, IMyLazyAsset
        {
            TickCount _lastUsed;

            /// <summary>
            /// Initializes a new instance of the <see cref="MyLazySoundBuffer"/> class.
            /// </summary>
            /// <param name="filename">The file name.</param>
            public MyLazySoundBuffer(string filename) : base(filename)
            {
                _lastUsed = TickCount.Now;
            }

            /// <summary>
            /// Access to the internal pointer of the object.
            /// For internal use only
            /// </summary>
            public override IntPtr CPointer
            {
                get
                {
                    _lastUsed = TickCount.Now;
                    return base.CPointer;
                }
            }

            /// <summary>
            /// Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return LazyAssetToString("SoundBuffer", FileName);
            }

            #region IMyLazyAsset Members

            /// <summary>
            /// Gets the <see cref="TickCount.Now"/> that this asset was last used.
            /// </summary>
            public TickCount LastUsedTime
            {
                get { return _lastUsed; }
            }

            #endregion
        }
    }
}