using System.Linq;
using SFML.Graphics;

namespace NetGore.Content
{
    /// <summary>
    /// Extension methods for the <see cref="IContentManager"/>.
    /// </summary>
    public static class IContentManagerExtensions
    {
        /// <summary>
        /// Loads an asset that has been created through the Content Pipeline.
        /// </summary>
        /// <param name="contentManager">The <see cref="IContentManager"/> to use to load the asset.</param>
        /// <param name="contentAssetName">The name of the asset to load.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>
        /// The asset loaded from the <paramref name="contentManager"/>, or null if the loading failed.
        /// </returns>
        public static Font LoadFont(this IContentManager contentManager, ContentAssetName contentAssetName, int fontSize,
                                    ContentLevel level)
        {
            if (contentManager == null || contentAssetName == null)
                return null;

            return contentManager.LoadFont(contentAssetName.Value, fontSize, level);
        }

        /// <summary>
        /// Loads an asset that has been created through the Content Pipeline.
        /// </summary>
        /// <param name="contentManager">The <see cref="IContentManager"/> to use to load the asset.</param>
        /// <param name="contentAssetName">The name of the asset to load.</param>
        /// <param name="level">The <see cref="ContentLevel"/> to load the asset into.</param>
        /// <returns>
        /// The asset loaded from the <paramref name="contentManager"/>, or null if the loading failed.
        /// </returns>
        public static Texture LoadImage(this IContentManager contentManager, ContentAssetName contentAssetName, ContentLevel level)
        {
            if (contentManager == null || contentAssetName == null)
                return null;

            return contentManager.LoadImage(contentAssetName.Value, level);
        }
    }
}