using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Interface for a class that contains Say commands.
    /// </summary>
    /// <typeparam name="T">The Type of User.</typeparam>
    public interface ISayCommands<T> where T : class
    {
        /// <summary>
        /// Gets or sets the current user for which the command being handled came from.
        /// </summary>
        T User { get; set; }
    }
}