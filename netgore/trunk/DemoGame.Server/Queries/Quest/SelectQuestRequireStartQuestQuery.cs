using System.Data;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectQuestRequireStartQuestQuery : SelectQuestValuesQueryBase<IQuestRequireStartQuestTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQuestRequireStartQuestQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectQuestRequireStartQuestQuery(DbConnectionPool connectionPool)
            : base(connectionPool, QuestRequireStartQuestTable.TableName)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a row from the database.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> to use to read.</param>
        /// <returns>The values read from the <paramref name="reader"/>.</returns>
        protected override IQuestRequireStartQuestTable ReadRow(IDataReader reader)
        {
            var r = new QuestRequireStartQuestTable();
            r.ReadValues(reader);
            return r;
        }
    }
}