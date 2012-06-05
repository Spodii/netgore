using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectItemTemplateIDsQuery : DbQueryReader
    {
        /// <summary>
        /// DbQueryReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectItemTemplateIDsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ArePrimaryKeys(ItemTemplateTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT `id` FROM `{0}`

            var q = qb.Select(ItemTemplateTable.TableName).Add("id");
            return q.ToString();
        }

        public IEnumerable<ItemTemplateID> Execute()
        {
            var ret = new List<ItemTemplateID>();

            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    var id = r.GetItemTemplateID(0);
                    ret.Add(id);
                }
            }

            return ret;
        }
    }
}