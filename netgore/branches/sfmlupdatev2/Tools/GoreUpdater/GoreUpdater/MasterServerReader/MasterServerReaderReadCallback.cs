using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling reading the information from the master servers.
    /// </summary>
    /// <param name="sender">The <see cref="IMasterServerReader"/> this event came from.</param>
    /// <param name="info">The information from the master server(s).</param>
    /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
    /// from the method call.</param>
    public delegate void MasterServerReaderReadCallback(IMasterServerReader sender, IMasterServerReadInfo info, object userState);
}