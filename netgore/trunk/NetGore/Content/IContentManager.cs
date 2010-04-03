using System;
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