using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.Db;

// TODO: Make this a query to just select ONE ItemTemplate, then add a new query to get all ItemTemplateIDs

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectItemTemplatesQuery : DbQueryReader
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}`", DBTables.ItemTemplate);

        public SelectItemTemplatesQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<ItemTemplate> Execute()
        {
            var ret = new List<ItemTemplate>();

            using (IDataReader r = ExecuteReader())
            {
                while (r.Read())
                {
                    ItemTemplate template = ItemTemplateQueryHelper.ReadItemTemplate(r);
                    ret.Add(template);
                }
            }

            return ret;
        }
    }
}