using System.Data;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectQuestRequireFinishItemQuery : SelectQuestValuesQueryBase<IQuestRequireFinishItemTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQuestRequireFinishItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectQuestRequireFinishItemQuery(DbConnectionPool connectionPool)
            : base(connectionPool, QuestRequireFinishItemTable.TableName)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a row from the database.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> to use to read.</param>
        /// <returns>The values read from the <paramref name="reader"/>.</returns>
        protected override IQuestRequireFinishItemTable ReadRow(IDataReader reader)
        {
            var r = new QuestRequireFinishItemTable();
            r.ReadValues(reader);
            return r;
        }
    }
}