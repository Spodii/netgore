using System.Linq;
using Microsoft.Xna.Framework.Content;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extension methods for the <see cref="ContentManager"/>.
    /// </summary>
    public static class ContentManagerExtensions
    {
        /// <summary>
        /// Loads an asset that has been created through the Content Pipeline.
        /// </summary>
        /// <typeparam name="T">The type of asset to load.</typeparam>
        /// <param name="contentManager">The <see cref="ContentManager"/> to use to load the asset.</param>
        /// <param name="contentAssetName">The name of the asset to load.</param>
        /// <returns>The asset loaded from the <paramref name="contentManager"/>, or null if the loading failed.</returns>
        public static T Load<T>(this ContentManager contentManager, ContentAssetName contentAssetName)
        {
            if (contentManager == null || contentAssetName == null)
                return default(T);

            return contentManager.Load<T>(contentAssetName.Value);
        }
    }
}