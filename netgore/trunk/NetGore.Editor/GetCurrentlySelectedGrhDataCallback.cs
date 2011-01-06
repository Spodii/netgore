using System.Linq;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// Delegate for getting a the <see cref="GrhData"/> currently selected in some object.
    /// </summary>
    /// <param name="sender">The object to get the currently selected <see cref="GrhData"/> for.</param>
    /// <returns>The currently selected <see cref="GrhData"/>.</returns>
    public delegate GrhData GetCurrentlySelectedGrhDataCallback(object sender);
}