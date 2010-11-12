using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Quests;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class InsertQuestRequireKillQuery : DbQueryNonReader<IQuestRequireKillTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertQuestRequireKillQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public InsertQuestRequireKillQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // INSERT INTO {0} {1}
            //      ON DUPLICATE KEY UPDATE <{1} - keys>

            var q =
                qb.Insert(QuestRequireKillTable.TableName).AddAutoParam(QuestRequireKillTable.DbColumns).ODKU().AddFromInsert(
                    QuestRequireKillTable.DbKeyColumns);
            return q.ToString();
        }

        public void Execute(QuestID questID, CharacterTemplateID reqCharID, ushort amount)
        {
            Execute(new QuestRequireKillTable(amount, reqCharID, questID));
        }

        public void Execute(QuestID questID, IEnumerable<KeyValuePair<CharacterTemplateID, ushort>> reqChars)
        {
            foreach (var item in reqChars)
            {
                Execute(questID, item.Key, item.Value);
            }
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>
        /// IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.
        /// </returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(QuestRequireKillTable.DbColumns);
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, IQuestRequireKillTable item)
        {
            item.CopyValues(p);
        }
    }
}