using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Editor helper methods for particle effects.
    /// </summary>
    public static class ParticleEffectHelper
    {
        /// <summary>
        /// Gets the name to display for an effect from a file path.
        /// </summary>
        /// <param name="filePath">The file path for the particle effect.</param>
        /// <returns>The name to display for the particle effect.</returns>
        public static string GetEffectDisplayNameFromFile(string filePath)
        {
            try
            {
                return Path.GetFileNameWithoutExtension(filePath);
            }
            catch (ArgumentException)
            {
                return filePath ?? "Unnamed";
            }
        }
    }
}
