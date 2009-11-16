using System.Linq;
using NetGore;

namespace NetGore.Graphics
{
    /// <summary>
    /// Handles when a <see cref="GrhData"/> changes its categorization.
    /// </summary>
    /// <param name="grhData"><see cref="GrhData"/> that had it's categorization changed.</param>
    /// <param name="oldCategory">The old category.</param>
    /// <param name="oldTitle">The old title.</param>
    public delegate void GrhDataChangeCategorizationHandler(GrhData grhData, string oldCategory, string oldTitle);

    /// <summary>
    /// Handles when a <see cref="GrhData"/> changes its texture.
    /// </summary>
    /// <param name="grhData"><see cref="GrhData"/> that had it's texture changed.</param>
    /// <param name="oldTexture">The old texture.</param>
    public delegate void GrhDataChangeTextureHandler(GrhData grhData, string oldTexture);
}