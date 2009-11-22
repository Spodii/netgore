using System.Collections.Generic;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Interface for a class that saves and loads its state values managed by a <see cref="SettingsManager"/>.
    /// </summary>
    public interface IRestorableSettings
    {
        /// <summary>
        /// Loads the values supplied by the <paramref name="items"/> to reconstruct the settings.
        /// </summary>
        /// <param name="items">Dictionary containing the values to restore.</param>
        void Load(IDictionary<string, string> items);

        /// <summary>
        /// Returns the key and value pairs needed to restore the settings.
        /// </summary>
        /// <returns>The key and value pairs needed to restore the settings.</returns>
        IEnumerable<NodeItem> Save();
    }
}