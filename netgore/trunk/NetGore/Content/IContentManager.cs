using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.IO;
using SFML.Audio;
using SFML.Graphics;

namespace NetGore.Content
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
        /// Gets if <see cref="IContentManager.BeginTrackingLoads"/> has been called and loaded items are being tracked.
        /// This will be set false when <see cref="IContentManager.EndTrackingLoads"/> is called.
        /// </summary>
        bool IsTrackingLoads { get; }

        /// <summary>
        /// Gets or sets absolute path to the root content directory. The default value, which is
        /// <see cref="ContentPaths.Build"/>'s root, should be fine for most all cases.
        /// </summary>
        PathString RootDirectory { get; set; }

        /// <summary>
        /// Starts tracking items that are loaded by this <see cref="IContentManager"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IContentManager.IsTrackingLoads"/> is true.</exception>
        void BeginTrackingLoads();

        /// <summary>
        /// Gets all of the assets that were loaded since <see cref="IContentManager.BeginTrackingLoads"/> was called.
        /// Content that was unloaded will still be included in the returned collection.
        /// </summary>
        /// <returns>All of the assets that were loaded since <see cref="IContentManager.BeginTrackingLoads"/>
        /// was called.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IContentManager.IsTrackingLoads"/> is false.</exception>
        IEnumerable<KeyValuePair<string, object>> EndTrackingLoads();

        /// <summary>
        /// Loads a <see cref="Font"/> asset.
        /// </summary>
        /// <param name="assetName">The name of the asset to load.</param>
        /// <param name="fontSize">The size of the font.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>The loaded asset.</returns>
        Font LoadFont(string assetName, int fontSize, ContentLevel level);

        /// <summary>
        /// Loads an <see cref="Texture"/> asset.
        /// </summary>
        /// <param name="assetName">The name of the asset to load.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>The loaded asset.</returns>
        Texture LoadImage(string assetName, ContentLevel level);

        /// <summary>
        /// Loads a <see cref="SoundBuffer"/> asset.
        /// </summary>
        /// <param name="assetName">The name of the asset to load.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>The loaded asset.</returns>
        SoundBuffer LoadSoundBuffer(string assetName, ContentLevel level);

        /// <summary>
        /// Sets the level of an asset.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The new <see cref="ContentLevel"/>.</param>
        void SetLevel(string assetName, ContentLevel level);

        /// <summary>
        /// Sets the level of an asset only if the specified level is greater than the current level.
        /// A lower <see cref="ContentLevel"/> unloads less frequently.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The new <see cref="ContentLevel"/>.</param>
        void SetLevelMax(string assetName, ContentLevel level);

        /// <summary>
        /// Sets the level of an asset only if the specified level is lower than the current level.
        /// A lower <see cref="ContentLevel"/> unloads less frequently.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">The new <see cref="ContentLevel"/>.</param>
        void SetLevelMin(string assetName, ContentLevel level);

        /// <summary>
        /// Gets the content level of an asset.
        /// </summary>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="level">When this method returns true, contains the <see cref="ContentLevel"/>
        /// of the asset.</param>
        /// <returns>True if the asset was found; otherwise false.</returns>
        bool TryGetContentLevel(string assetName, out ContentLevel level);

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
        void Unload(ContentLevel level = ContentLevel.Global, bool ignoreTime = false);
    }
}