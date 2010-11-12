using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class InsertWorldStatsCountItemBuyQuery : DbQueryNonReader<KeyValuePair<int, int>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertWorldStatsCountItemBuyQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        public InsertWorldStatsCountItemBuyQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(WorldStatsCountItemBuyTable.DbColumns, "item_template_id", "count");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // INSERT INTO `{0}` (`item_template_id`,`count`) VALUES (@id,@count)
            //      ON DUPLICATE KEY UPDATE `count`=`count`+@count

            var s = qb.Settings;
            var f = qb.Functions;
            var q =
                qb.Insert(WorldStatsCountItemBuyTable.TableName).AddParam("item_template_id", "id").AddParam("count", "count").
                    ODKU().Add("count", f.Add(s.EscapeColumn("count"), s.Parameterize("count")));
            return q.ToString();
        }

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <param name="itemTemplateID">The item template ID.</param>
        /// <param name="amount">The amount.</param>
        public void Execute(int itemTemplateID, int amount)
        {
            Execute(new KeyValuePair<int, int>(itemTemplateID, amount));
        }

        #region Overrides of DbQueryBase

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("id", "count");
        }

        #endregion

        #region Overrides of DbQueryNonReader<int>

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, KeyValuePair<int, int> item)
        {
            p["id"] = item.Key;
            p["count"] = item.Value;
        }

        #endregion
    }
}