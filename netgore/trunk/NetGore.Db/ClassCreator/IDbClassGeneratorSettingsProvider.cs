using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Interface for a class that provides additional settings to a <see cref="DbClassGenerator"/>, such as custom types
    /// and aliases.
    /// </summary>
    public interface IDbClassGeneratorSettingsProvider
    {
        /// <summary>
        /// Applies the custom settings to the <see cref="DbClassGenerator"/>.
        /// </summary>
        /// <param name="gen">The <see cref="DbClassGenerator"/> to apply the custom settings to.</param>
        void ApplySettings(DbClassGenerator gen);
    }
}