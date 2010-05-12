using System.Linq;
using NetGore.Db.ClassCreator;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Implementation of the <see cref="IDbClassGeneratorSettingsProvider"/> for the quest database tables.
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
            gen.Formatter.AddAlias("quest_id", "QuestID");
            gen.Formatter.AddAlias("req_quest_id", "ReqQuestID");

            gen.AddCustomType(typeof(QuestID), "quest", "id");

            gen.AddCustomType(typeof(QuestID), "*", "quest_id", "req_quest_id");
        }

        #endregion
    }
}