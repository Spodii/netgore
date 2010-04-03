using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Interface for a content manager.
    /// </summary>
    public interface IContentManager : IDisposable
    {
        /// <summary>
        /// Gets if this object is disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets or sets the root directory.
        /// </summary>
        string RootDirectory { get; set; }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/>.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Starts tracking items that are loaded by this <see cref="IContentManager"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IContentManager.IsTrackingLoads"/> is true.</exception>
        void BeginTrackingLoads();

        /// <summary>
        /// Gets if <see cref="IContentManager.BeginTrackingLoads"/> has been called and loaded items are being tracked.
        /// This will be set false when <see cref="IContentManager.EndTrackingLoads"/> is called.
        /// </summary>
        bool IsTrackingLoads { get; }

        /// <summary>
        /// Gets all of the assets that were loaded since <see cref="IContentManager.BeginTrackingLoads"/> was called.
        /// Content that was unloaded will still be included in the returned collection.
        /// </summary>
        /// <returns>All of the assets that were loaded since <see cref="IContentManager.BeginTrackingLoads"/>
        /// was called.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IContentManager.IsTrackingLoads"/> is false.</exception>
        IEnumerable<KeyValuePair<string, object>> EndTrackingLoads();

        /// <summary>
        /// Sets the level of an asset.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The new <see cref="ContentLevel"/>.</param>
        void SetLevel(string assetName, ContentLevel level);

        /// <summary>
        /// Sets the level of an asset only if the specified level is lower than the current level.
        /// A lower <see cref="ContentLevel"/> unloads less frequently.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The new <see cref="ContentLevel"/>.</param>
        void SetLevelMin(string assetName, ContentLevel level);

        /// <summary>
        /// Sets the level of an asset only if the specified level is greater than the current level.
        /// A lower <see cref="ContentLevel"/> unloads less frequently.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The new <see cref="ContentLevel"/>.</param>
        void SetLevelMax(string assetName, ContentLevel level);

        /// <summary>
        /// Gets the content level of an asset.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">When this method returns true, contains the <see cref="ContentLevel"/>
        /// of the asset.</param>
        /// <returns>True if the asset was found; otherwise false.</returns>
        bool TryGetContentLevel(string assetName, out ContentLevel level);

        /// <summary>
        /// Loads an asset.
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>The loaded asset.</returns>
        T Load<T>(string assetName, ContentLevel level);

        /// <summary>
        /// Unloads all content from all levels.
        /// </summary>
        void Unload();

        /// <summary>
        /// Unloads all content from the specified <see cref="ContentLevel"/>, and all levels
        /// below that level.
        /// </summary>
        /// <param name="level">The level of the content to unload.</param>
        void Unload(ContentLevel level);
    }
}