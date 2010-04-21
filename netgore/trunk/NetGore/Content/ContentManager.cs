using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Gets the minimum amount of time that an asset must go unused before being unloaded.
        /// </summary>
        const int _minElapsedTimeToUnload = 1000 * 60; // 1 minute

        static readonly object _instanceSync = new object();
        static readonly StringComparer _stringComp = StringComparer.OrdinalIgnoreCase;
        static bool _doNotUnload;
        static IContentManager _instance;

        readonly object _assetSync = new object();
        readonly Dictionary<string, IMyLazyAsset>[] _loadedAssets;
        readonly Dictionary<string, IMyLazyAsset> _trackedLoads = new Dictionary<string, IMyLazyAsset>(_stringComp);

        bool _isDisposed = false;
        bool _isTrackingLoads = false;
        ContentLevel? _queuedUnloadLevel = null;

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

            DoNotUploadSetFalse += ContentManager_DoNotUploadSetFalse;
        }

        /// <summary>
        /// Notifies listeners when <see cref="DoNotUnload"/> is set from true to false.
        /// </summary>
        static event EventHandler DoNotUploadSetFalse;

        /// <summary>
        /// Gets or sets if <see cref="ContentManager.Unload()"/> must queue unload calls.
        /// </summary>
        internal static bool DoNotUnload
        {
            get { return _doNotUnload; }
            set
            {
                if (_doNotUnload == value)
                    return;

                _doNotUnload = value;

                if (!_doNotUnload)
                {
                    if (DoNotUploadSetFalse != null)
                        DoNotUploadSetFalse(null, EventArgs.Empty);
                }
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
        /// Handles the DoNotUploadSetFalse event of the <see cref="ContentManager"/> object.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void ContentManager_DoNotUploadSetFalse(object sender, EventArgs e)
        {
            if (_queuedUnloadLevel != null)
                DoUnload(_queuedUnloadLevel.Value);
        }

        /// <summary>
        /// Creates a <see cref="IContentManager"/>.
        /// </summary>
        /// <returns>The <see cref="IContentManager"/> instance.</returns>
        public static IContentManager Create()
        {
            lock (_instanceSync)
            {
                if (_instance == null)
                    _instance = new ContentManager();

                return _instance;
            }
        }

        /// <summary>
        /// Disposes of the content manager.
        /// </summary>
        /// <param name="disposeManaged">If false, this was called from the destructor.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            DoNotUploadSetFalse -= ContentManager_DoNotUploadSetFalse;
            Unload();
        }

        void DoUnload(ContentLevel level)
        {
            var currTime = Environment.TickCount;

            lock (_assetSync)
            {
                // If the queued level is set, and it is a lower level, then use that as the level instead
                if (_queuedUnloadLevel.HasValue && _queuedUnloadLevel.Value < level)
                    level = _queuedUnloadLevel.Value;

                _queuedUnloadLevel = null;

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
                            if (currTime - asset.LastUsedTime < _minElapsedTimeToUnload)
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
        /// Loads an asset.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <param name="loader">The loader.</param>
        /// <returns>The loaded asset.</returns>
        IMyLazyAsset Load(string assetName, ContentLevel level, Func<string, IMyLazyAsset> loader)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(ToString());

            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            var levelInt = (int)level;
            if (levelInt >= _loadedAssets.Length || levelInt < 0)
                throw new ArgumentOutOfRangeException("level", "Invalid ContentLevel value specified.");

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
        protected Image ReadAssetImage(string assetName)
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
        /// Loads an <see cref="Image"/> asset.
        /// </summary>
        /// <param name="assetName">The name of the asset to load.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>The loaded asset.</returns>
        public Image LoadImage(string assetName, ContentLevel level)
        {
            if (assetName == null)
                throw new ArgumentNullException("assetName");

            assetName = SanitizeAssetName(assetName);

            var ret = Load(assetName, level, x => (IMyLazyAsset)ReadAssetImage(x));
            return (Image)ret;
        }

        /// <summary>
        /// Loads a <see cref="SoundBuffer"/> asset.
        /// </summary>
        /// <param name="assetName">The name of the asset to load.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>The loaded asset.</returns>
        public SoundBuffer LoadSoundBuffer(string assetName, ContentLevel level)
        {
            if (assetName == null)
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
        public void SetLevel(string assetName, ContentLevel level)
        {
            if (assetName == null)
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
        public void SetLevelMax(string assetName, ContentLevel level)
        {
            if (assetName == null)
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
        public void SetLevelMin(string assetName, ContentLevel level)
        {
            if (assetName == null)
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
        public bool TryGetContentLevel(string assetName, out ContentLevel level)
        {
            if (assetName == null)
                throw new ArgumentNullException("assetName");

            assetName = SanitizeAssetName(assetName);

            lock (_assetSync)
            {
                IMyLazyAsset o;
                return IsAssetLoaded(assetName, out o, out level);
            }
        }

        /// <summary>
        /// Unloads all content from all levels.
        /// </summary>
        public void Unload()
        {
            if (IsDisposed)
                return;

            Unload(ContentLevel.Global);
        }

        /// <summary>
        /// Unloads all content from the specified <see cref="ContentLevel"/>, and all levels
        /// below that level.
        /// </summary>
        /// <param name="level">The level of the content to unload.</param>
        public void Unload(ContentLevel level)
        {
            if (DoNotUnload)
                _queuedUnloadLevel = level;
            else
                DoUnload(level);
        }

        #endregion

        /// <summary>
        /// Interface for the lazy assets of the <see cref="ContentManager"/>.
        /// </summary>
        interface IMyLazyAsset : IDisposable
        {
            /// <summary>
            /// Gets the <see cref="Environment.TickCount"/> that this asset was last used.
            /// </summary>
            int LastUsedTime { get; }
        }

        /// <summary>
        /// <see cref="LazyFont"/> implementation specifically for the <see cref="ContentManager"/>.
        /// </summary>
        sealed class MyLazyFont : LazyFont, IMyLazyAsset
        {
            int _lastUsed;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:SFML.Graphics.LazyFont"/> class.
            /// </summary>
            /// <param name="filename">Font file to load</param><param name="charSize">Character size</param><exception cref="T:SFML.LoadingFailedException"/>
            public MyLazyFont(string filename, uint charSize) : base(filename, charSize)
            {
                _lastUsed = Environment.TickCount;
            }

            /// <summary>
            /// Access to the internal pointer of the object.
            /// For internal use only
            /// </summary>
            public override IntPtr This
            {
                get
                {
                    _lastUsed = Environment.TickCount;
                    return base.This;
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
                if (FileName.Length > 15)
                    return "Image [..." + FileName.Substring(10) + "]";
                else
                    return "Image [" + FileName + "]";
            }

            #region IMyLazyAsset Members

            /// <summary>
            /// Gets the <see cref="Environment.TickCount"/> that this asset was last used.
            /// </summary>
            public int LastUsedTime
            {
                get { return _lastUsed; }
            }

            #endregion
        }

        /// <summary>
        /// <see cref="LazyImage"/> implementation specifically for the <see cref="ContentManager"/>.
        /// </summary>
        sealed class MyLazyImage : LazyImage, IMyLazyAsset
        {
            int _lastUsed;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:SFML.Graphics.LazyImage"/> class.
            /// </summary>
            /// <param name="filename">The file name.</param>
            public MyLazyImage(string filename) : base(filename)
            {
                _lastUsed = Environment.TickCount;
            }

            /// <summary>
            /// Access to the internal pointer of the object.
            /// For internal use only
            /// </summary>
            public override IntPtr This
            {
                get
                {
                    _lastUsed = Environment.TickCount;
                    return base.This;
                }
            }

            /// <summary>
            /// When overridden in the derived class, handles when the <see cref="T:SFML.Graphics.LazyImage"/> is reloaded.
            /// </summary>
            protected override void OnReload()
            {
                Smooth = false;
                CreateMaskFromColor(EngineSettings.TransparencyColor);
            }

            /// <summary>
            /// Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                if (FileName.Length > 15)
                    return "Image [..." + FileName.Substring(10) + "]";
                else
                    return "Image [" + FileName + "]";
            }

            #region IMyLazyAsset Members

            /// <summary>
            /// Gets the <see cref="Environment.TickCount"/> that this asset was last used.
            /// </summary>
            public int LastUsedTime
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
            int _lastUsed;

            /// <summary>
            /// Initializes a new instance of the <see cref="MyLazySoundBuffer"/> class.
            /// </summary>
            /// <param name="filename">The file name.</param>
            public MyLazySoundBuffer(string filename) : base(filename)
            {
                _lastUsed = Environment.TickCount;
            }

            /// <summary>
            /// Access to the internal pointer of the object.
            /// For internal use only
            /// </summary>
            public override IntPtr This
            {
                get
                {
                    _lastUsed = Environment.TickCount;
                    return base.This;
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
                if (FileName.Length > 15)
                    return "SoundBuffer [..." + FileName.Substring(10) + "]";
                else
                    return "SoundBuffer [" + FileName + "]";
            }

            #region IMyLazyAsset Members

            /// <summary>
            /// Gets the <see cref="Environment.TickCount"/> that this asset was last used.
            /// </summary>
            public int LastUsedTime
            {
                get { return _lastUsed; }
            }

            #endregion
        }
    }
}