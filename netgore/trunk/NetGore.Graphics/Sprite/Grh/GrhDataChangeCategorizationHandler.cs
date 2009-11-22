using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Handles when a <see cref="GrhData"/> changes its categorization.
    /// </summary>
    /// <param name="grhData"><see cref="GrhData"/> that had it's categorization changed.</param>
    /// <param name="oldCategorization">The old categorization.</param>
    public delegate void GrhDataChangeCategorizationHandler(GrhData grhData, SpriteCategorization oldCategorization);

    /// <summary>
    /// Handles when a <see cref="GrhData"/> changes its texture.
    /// </summary>
    /// <param name="grhData"><see cref="GrhData"/> that had it's texture changed.</param>
    /// <param name="oldTexture">The old texture.</param>
    public delegate void GrhDataChangeTextureHandler(GrhData grhData, ContentAssetName oldTexture);
}