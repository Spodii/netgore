using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class ReplaceQuestRewardItemQuery : DbQueryNonReader<IQuestRewardItemTable>
    {
        static readonly string _queryStr = string.Format("REPLACE INTO `{0}` {1}", QuestRewardItemTable.TableName,
                                                         FormatParametersIntoValuesString(QuestRewardItemTable.DbColumns));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceQuestRewardItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public ReplaceQuestRewardItemQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
        }

        public int Execute(QuestID questID, ItemTemplateID itemID, byte amount)
        {
            return Execute(new QuestRewardItemTable(amount, itemID, questID));
        }

        public int Execute(QuestID questID, IEnumerable<KeyValuePair<ItemTemplateID, byte>> items)
        {
            int sum = 0;
            foreach (var item in items)
            {
                sum += Execute(questID, item.Key, item.Value);
            }
            return sum;
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
            return CreateParameters(QuestRewardItemTable.DbColumns.Select(x => "@" + x));
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, IQuestRewardItemTable item)
        {
            item.CopyValues(p);
        }
    }
}