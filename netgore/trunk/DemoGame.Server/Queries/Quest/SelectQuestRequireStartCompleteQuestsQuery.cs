using System.Data;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectQuestRequireStartCompleteQuestsQuery : SelectQuestValuesQueryBase<QuestID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQuestValuesQueryBase{T}"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectQuestRequireStartCompleteQuestsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, QuestRequireStartQuestTable.TableName)
        {
            QueryAsserts.ContainsColumns(QuestRequireStartQuestTable.DbColumns, "req_quest_id");
        }

        /// <summary>
        /// When overridden in the derived class, reads a row from the database.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> to use to read.</param>
        /// <returns>The values read from the <paramref name="reader"/>.</returns>
        protected override QuestID ReadRow(IDataReader reader)
        {
            return reader.GetQuestID("req_quest_id");
        }
    }
}