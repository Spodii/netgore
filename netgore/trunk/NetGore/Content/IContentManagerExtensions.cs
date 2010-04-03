using System.Linq;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="IContentManager"/>.
    /// </summary>
    public static class IContentManagerExtensions
    {
        /// <summary>
        /// Loads an asset that has been created through the Content Pipeline.
        /// </summary>
        /// <typeparam name="T">The type of asset to load.</typeparam>
        /// <param name="contentManager">The <see cref="IContentManager"/> to use to load the asset.</param>
        /// <param name="contentAssetName">The name of the asset to load.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>
        /// The asset loaded from the <paramref name="contentManager"/>, or null if the loading failed.
        /// </returns>
        public static T Load<T>(this IContentManager contentManager, ContentAssetName contentAssetName, ContentLevel level)
        {
            if (contentManager == null || contentAssetName == null)
                return default(T);

            return contentManager.Load<T>(contentAssetName.Value, level);
        }
    }
}