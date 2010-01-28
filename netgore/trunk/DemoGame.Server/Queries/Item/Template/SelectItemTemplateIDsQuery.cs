using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectItemTemplateIDsQuery : DbQueryReader
    {
        static readonly string _queryStr = string.Format("SELECT `id` FROM `{0}`", ItemTemplateTable.TableName);

        /// <summary>
        /// DbQueryReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectItemTemplateIDsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ArePrimaryKeys(ItemTemplateTable.DbKeyColumns, "id");
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