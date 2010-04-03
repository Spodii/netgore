using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework.Content;

namespace NetGore
{
    /// <summary>
    /// <see cref="IContentManager"/> for Xna.
    /// </summary>
    public class XnaContentManager : IContentManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly StringComparer _stringComp = StringComparer.OrdinalIgnoreCase;
        static bool _doNotUnload;

        readonly object _assetSync = new object();

        /// <summary>
        /// The <see cref="ContentManager"/> instance used to load the assets from file. Unfortunately, XNA
        /// makes it very difficult to do this without a <see cref="ContentManager"/>.
        /// </summary>
        readonly ContentManager _cm;

        /// <summary>
        /// Contains the internal <see cref="IList"/> in the <see cref="_cm"/>.
        /// This is cleared every time the <see cref="_cm"/> loads an asset to remove record that the asset was
        /// loaded since we only use the <see cref="ContentManager"/> for loading (not management).
        /// </summary>
        readonly IList _cmDisposableAssets;

        /// <summary>
        /// Contains the internal <see cref="IDictionary"/> in the <see cref="_cm"/>.
        /// This is cleared every time the <see cref="_cm"/> loads an asset to remove record that the asset was
        /// loaded since we only use the <see cref="ContentManager"/> for loading (not management).
        /// </summary>
        readonly IDictionary _cmLoadedAssetsDict;

        readonly Dictionary<string, object>[] _loadedAssets;
        readonly IServiceProvider _serviceProvider;
        readonly Dictionary<string, object> _trackedLoads = new Dictionary<string, object>(_stringComp);

        bool _isDisposed = false;
        bool _isTrackingLoads;
        ContentLevel? _queuedUnloadLevel = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="XnaContentManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is null.</exception>
        public XnaContentManager(IServiceProvider serviceProvider) : this(serviceProvider, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XnaContentManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="rootDir">The root content directory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="rootDir"/> is null.</exception>
        public XnaContentManager(IServiceProvider serviceProvider, string rootDir)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            if (rootDir == null)
                throw new ArgumentNullException("rootDir");

            _cm = new ContentManager(serviceProvider, rootDir);

            const BindingFlags flags =
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField;
            _cmLoadedAssetsDict = (IDictionary)typeof(ContentManager).GetField("loadedAssets", flags).GetValue(_cm);
            _cmDisposableAssets = (IList)typeof(ContentManager).GetField("disposableAssets", flags).GetValue(_cm);

            _serviceProvider = serviceProvider;
            RootDirectory = rootDir;

            // Create the dictionaries for each level
            int maxLevel = EnumHelper<ContentLevel>.MaxValue;
            _loadedAssets = new Dictionary<string, object>[maxLevel + 1];

            for (int i = 0; i < _loadedAssets.Length; i++)
            {
                _loadedAssets[i] = new Dictionary<string, object>(_stringComp);
            }

            DoNotUploadSetFalse += XnaContentManager_DoNotUploadSetFalse;
        }

        /// <summary>
        /// Notifies listeners when <see cref="DoNotUnload"/> is set from true to false.
        /// </summary>
        static event EventHandler DoNotUploadSetFalse;

        /// <summary>
        /// Gets or sets if <see cref="XnaContentManager.Unload()"/> must queue unload calls.
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
            object asset;
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
        /// Disposes of the content manager.
        /// </summary>
        /// <param name="disposeManaged">If false, this was called from the destructor.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            DoNotUploadSetFalse -= XnaContentManager_DoNotUploadSetFalse;
            Unload();
        }

        void DoUnload(ContentLevel level)
        {
            lock (_assetSync)
            {
                // Loop through the given level and all levels below it
                for (int i = (int)level; i < _loadedAssets.Length; i++)
                {
                    // Get the dictionary for the level
                    var dic = _loadedAssets[i];

                    // Dispose all items, then clear the level
                    try
                    {
                        foreach (var disposable in dic.Values.OfType<IDisposable>())
                        {
                            disposable.Dispose();
                        }
                    }
                    finally
                    {
                        dic.Clear();
                    }
                }

                _queuedUnloadLevel = null;
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="XnaContentManager"/> is reclaimed by garbage collection.
        /// </summary>
        ~XnaContentManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Checks if an asset is loaded.
        /// </summary>
        /// <param name="assetName">The name of the asset to look for.</param>
        /// <param name="asset">When this method returns true, contains the asset instance.</param>
        /// <param name="level">When this method returns true, contains the asset's <see cref="ContentLevel"/>.</param>
        /// <returns>True if the asset is loaded; otherwise false.</returns>
        protected bool IsAssetLoaded(string assetName, out object asset, out ContentLevel level)
        {
            for (int i = 0; i < _loadedAssets.Length; i++)
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
        /// Reads an asset from file.
        /// </summary>
        /// <typeparam name="T">The type of the asset.</typeparam>
        /// <param name="assetName">The name of the asset.</param>
        /// <returns>The asset instance.</returns>
        protected T ReadAsset<T>(string assetName)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(ToString());

            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            // Load through the ContentManager
            var ret = _cm.Load<T>(assetName);

            // Clear the ContentManager's internal collections
            _cmLoadedAssetsDict.Clear();
            _cmDisposableAssets.Clear();

            return ret;
        }

        /// <summary>
        /// Handles the DoNotUploadSetFalse event of the XnaContentManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void XnaContentManager_DoNotUploadSetFalse(object sender, EventArgs e)
        {
            if (_queuedUnloadLevel != null)
                DoUnload(_queuedUnloadLevel.Value);
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
        /// Gets or sets the root directory.
        /// </summary>
        /// <value></value>
        public string RootDirectory { get; set; }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <value></value>
        public IServiceProvider ServiceProvider
        {
            get { return _serviceProvider; }
        }

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

            var ret = _trackedLoads.ToArray();
            _trackedLoads.Clear();

            return ret;
        }

        /// <summary>
        /// Loads an asset.
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>The loaded asset.</returns>
        public T Load<T>(string assetName, ContentLevel level)
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
                object existingAsset;
                ContentLevel existingLevel;
                if (IsAssetLoaded(assetName, out existingAsset, out existingLevel))
                {
                    // Ensure a valid type was specified
                    if (!(existingAsset is T))
                    {
                        const string errmsg = "Invalid type `{0}` specified for asset `{1}`, which is of type `{2}`.";
                        string err = string.Format(errmsg, typeof(T), assetName, existingAsset.GetType());
                        if (log.IsErrorEnabled)
                            log.Error(err);
                        throw new ContentLoadException(err);
                    }

                    // If the specified level parameter is greater than the asset's current level, promote it
                    if (level < existingLevel)
                    {
                        bool success = ChangeAssetLevelNoLock(assetName, existingLevel, level);
                        Debug.Assert(success);
                    }

                    return (T)existingAsset;
                }

                // Load a new asset and add it into the appropriate level
                var asset = ReadAsset<T>(assetName);
                _loadedAssets[levelInt].Add(assetName, asset);

                if (IsTrackingLoads && !_trackedLoads.ContainsKey(assetName))
                    _trackedLoads.Add(assetName, asset);

                return asset;
            }
        }

        /// <summary>
        /// Sets the level of an asset.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The new <see cref="ContentLevel"/>.</param>
        public void SetLevel(string assetName, ContentLevel level)
        {
            lock (_assetSync)
            {
                object asset;
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
            lock (_assetSync)
            {
                object asset;
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
            lock (_assetSync)
            {
                object asset;
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
            lock (_assetSync)
            {
                object o;
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
    }
}