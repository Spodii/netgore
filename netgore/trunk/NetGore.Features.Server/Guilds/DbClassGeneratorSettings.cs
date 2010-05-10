using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Db.ClassCreator;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Implementation of the <see cref="IDbClassGeneratorSettingsProvider"/> for the guild database tables.
    /// </summary>
    public class DbClassGeneratorSettings : IDbClassGeneratorSettingsProvider
    {
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
    }
}
