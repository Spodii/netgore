using System.Data;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectQuestRewardItemQuery : SelectQuestValuesQueryBase<IQuestRewardItemTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQuestRewardItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectQuestRewardItemQuery(DbConnectionPool connectionPool) : base(connectionPool, QuestRewardItemTable.TableName)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a row from the database.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> to use to read.</param>
        /// <returns>The values read from the <paramref name="reader"/>.</returns>
        protected override IQuestRewardItemTable ReadRow(IDataReader reader)
        {
            var r = new QuestRewardItemTable();
            r.ReadValues(reader);
            return r;
        }
    }
}