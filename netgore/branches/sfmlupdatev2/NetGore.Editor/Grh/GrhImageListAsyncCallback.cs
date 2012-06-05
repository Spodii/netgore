using System.Drawing;
using System.Linq;
using NetGore.Graphics;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// Delegate for a callback method for getting an <see cref="Image"/> asynchronously.
    /// </summary>
    /// <param name="sender">The <see cref="GrhImageList"/> the callback came from.</param>
    /// <param name="gd">The <see cref="StationaryGrhData"/> that the <paramref name="image"/> is for. May be null if the
    /// <paramref name="image"/> is equal to <see cref="GrhImageList.ErrorImage"/> or null.</param>
    /// <param name="image">The <see cref="Image"/> that was created.</param>
    /// <param name="userState">The optional user state object that was passed to the method.</param>
    public delegate void GrhImageListAsyncCallback(GrhImageList sender, StationaryGrhData gd, Image image, object userState);
}