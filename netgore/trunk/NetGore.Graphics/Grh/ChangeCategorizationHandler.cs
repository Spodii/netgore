using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Handles when a GrhData changes its categorization.
    /// </summary>
    /// <param name="grhData">GrhData that had it's categorization changed.</param>
    /// <param name="oldCategory">The old category.</param>
    /// <param name="oldTitle">The old title.</param>
    public delegate void ChangeCategorizationHandler(GrhData grhData, string oldCategory, string oldTitle);
}