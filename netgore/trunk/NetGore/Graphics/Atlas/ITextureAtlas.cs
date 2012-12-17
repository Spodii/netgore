using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an object that can be set to use a texture atlas.
    /// </summary>
    public interface ITextureAtlasable
    {
        /// <summary>
        /// Gets the texture source <see cref="Rectangle"/> of the image.
        /// </summary>
        Rectangle SourceRect { get; }

        /// <summary>
        /// Gets the original texture.
        /// </summary>
        Texture Texture { get; }

        /// <summary>
        /// Removes the atlas from the object and forces it to draw normally.
        /// </summary>
        void RemoveAtlas();

        /// <summary>
        /// Removes the atlas from the object and forces it to draw normally only if the given atlas
        /// is the atlas being used. If a different atlas is used, then it will not be removed.
        /// </summary>
        /// <param name="atlas">If the <see cref="ITextureAtlasable"/> is using this atlas, then the atlas
        /// should be removed.</param>
        void RemoveAtlas(Texture atlas);

        /// <summary>
        /// Sets the atlas information.
        /// </summary>
        /// <param name="texture">The atlas texture.</param>
        /// <param name="atlasSourceRect">The source <see cref="Rectangle"/> for the image in the atlas texture.</param>
        /// <exception cref="ArgumentNullException"><paramref name="texture" /> is <c>null</c>.</exception>
        void SetAtlas(Texture texture, Rectangle atlasSourceRect);
    }
}