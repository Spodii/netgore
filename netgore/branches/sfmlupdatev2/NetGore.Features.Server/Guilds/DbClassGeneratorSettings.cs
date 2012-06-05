using System.Linq;
using NetGore.Db.ClassCreator;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Implementation of the <see cref="IDbClassGeneratorSettingsProvider"/> for the guild database tables.
    /// </summary>
    public class DbClassGeneratorSettings : IDbClassGeneratorSettingsProvider
    {
        #region IDbClassGeneratorSettingsProvider Members

        /// <summary>
        /// Applies the custom settings to the <see cref="DbClassGenerator"/>.
        /// </summary>
        /// <param name="gen">The <see cref="DbClassGenerator"/> to apply the custom settings to.</param>
        public void ApplySettings(DbClassGenerator gen)
        {
            gen.Formatter.AddAlias("guild_id", "GuildID");

            gen.AddCustomType(typeof(GuildID), "guild", "id");

            gen.AddCustomType(typeof(GuildID), "*", "guild_id");
            gen.AddCustomType(typeof(GuildRank), "guild_member", "rank");
        }

        #endregion
    }
}