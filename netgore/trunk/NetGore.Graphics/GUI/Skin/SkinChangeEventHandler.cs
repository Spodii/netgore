using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Handles when the skin has changed.
    /// </summary>
    /// <param name="newSkinName">The name of the new skin.</param>
    /// <param name="oldSkinName">The name of the old skin.</param>
    public delegate void SkinChangeEventHandler(string newSkinName, string oldSkinName);
}