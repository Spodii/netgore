using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an object that can be set to an atlas
    /// </summary>
    public interface ITextureAtlas
    {
        /// <summary>
        /// Gets the texture source rectangle of the original image
        /// </summary>
        Rectangle SourceRect { get; }

        /// <summary>
        /// Gets the original texture
        /// </summary>
        Texture2D Texture { get; }

        /// <summary>
        /// Sets the atlas information
        /// </summary>
        /// <param name="texture">Atlas texture</param>
        /// <param name="atlasSourceRect">Source rect for the image in the atlas texture</param>
        void SetAtlas(Texture2D texture, Rectangle atlasSourceRect);
    }
}