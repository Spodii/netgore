using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Interface for getting the current time in milliseconds.
    /// </summary>
    public interface IGetTime
    {
        /// <summary>
        /// Gets the current time in milliseconds.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        TickCount GetTime();
    }
}