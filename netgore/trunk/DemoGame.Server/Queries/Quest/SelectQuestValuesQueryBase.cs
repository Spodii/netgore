using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Quests;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// Base class for selecting multiple rows from a table using a <see cref="QuestID"/>.
    /// </summary>
    /// <typeparam name="T">The type of return value.</typeparam>
    public abstract class SelectQuestValuesQueryBase<T> : DbQueryReader<QuestID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQuestValuesQueryBase{T}"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        /// <param name="tableName">Name of the table.</param>
        protected SelectQuestValuesQueryBase(DbConnectionPool connectionPool, string tableName)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder, tableName))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <param name="tableName">Name of the table to select from.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb, string tableName)
        {
            // SELECT * FROM `{0}` WHERE `quest_id`=@id

            var f = qb.Functions;
            var s = qb.Settings;
            var q = qb.Select(tableName).AllColumns().Where(f.Equals(s.EscapeColumn("quest_id"), s.Parameterize("id")));
            return q.ToString();
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="id">The <see cref="QuestID"/> to get the values for.</param>
        /// <returns>The returned values from the query.</returns>
        public virtual IEnumerable<T> Execute(QuestID id)
        {
            var ret = new List<T>();

            using (var r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    var q = ReadRow(r);
                    ret.Add(q);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("id");
        }

        /// <summary>
        /// When overridden in the derived class, reads a row from the database.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> to use to read.</param>
        /// <returns>The values read from the <paramref name="reader"/>.</returns>
        protected abstract T ReadRow(IDataReader reader);

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QuestID item)
        {
            p["id"] = (int)item;
        }
    }
}