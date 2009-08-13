using System.Collections.Generic;
using System.Data;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

// TODO: Make this a query to just select ONE ItemTemplate, then add a new query to get all ItemTemplateIDs

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectItemTemplatesQuery : DbQueryReader
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}`", ItemTemplateTable.TableName);

        public SelectItemTemplatesQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<IItemTemplateTable> Execute()
        {
            var ret = new List<IItemTemplateTable>();

            using (IDataReader r = ExecuteReader())
            {
                while (r.Read())
                {
                    ItemTemplateTable template = new ItemTemplateTable();
                    template.ReadValues(r);
                    ret.Add(template);
                }
            }

            return ret;
        }
    }
}