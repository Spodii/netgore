using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework.Content;

namespace NetGore
{
    public class XnaContentManager : IContentManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly StringComparer _stringComp = StringComparer.OrdinalIgnoreCase;

        readonly object _assetSync = new object();
        readonly Dictionary<string, object>[] _loadedAssets;
        readonly IServiceProvider _serviceProvider;

        bool _isDisposed = false;

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

            _serviceProvider = serviceProvider;
            RootDirectory = rootDir;

            // Create the dictionaries for each level
            int maxLevel = EnumHelper<ContentLevel>.MaxValue;
            _loadedAssets = new Dictionary<string, object>[maxLevel + 1];

            for (int i = 0; i < _loadedAssets.Length; i++)
            {
                _loadedAssets[i] = new Dictionary<string, object>(_stringComp);
            }
        }

        /// <summary>
        /// The <see cref="ContentManager"/> instance used to load the assets from file. Unfortunately, XNA
        /// makes it very difficult to do this without a <see cref="ContentManager"/>.
        /// </summary>
        readonly ContentManager _cm;

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
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="XnaContentManager"/> is reclaimed by garbage collection.
        /// </summary>
        ~XnaContentManager()
        {
            Dispose(false);
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

          
            return _cm.Load<T>(assetName);

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

                return asset;
            }
        }

        /// <summary>
        /// Unloads all content from all levels.
        /// </summary>
        public void Unload()
        {
            if (IsDisposed)
                return;
        }

        /// <summary>
        /// Unloads all content from the specified <see cref="ContentLevel"/>, and all levels
        /// below that level.
        /// </summary>
        /// <param name="level">The level of the content to unload.</param>
        public void Unload(ContentLevel level)
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
            }
        }

        #endregion
    }
}