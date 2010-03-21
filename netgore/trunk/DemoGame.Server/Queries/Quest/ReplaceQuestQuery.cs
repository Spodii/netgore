using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class ReplaceQuestQuery : DbQueryNonReader<IQuestTable>
    {
        static readonly string _queryStr = string.Format("REPLACE INTO `{0}` {1}", QuestTable.TableName,
                                                         FormatParametersIntoValuesString(QuestTable.DbColumns));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceQuestQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public ReplaceQuestQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
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
            return CreateParameters(QuestTable.DbColumns.Select(x => "@" + x));
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, IQuestTable item)
        {
            item.CopyValues(p);
        }
    }
}
