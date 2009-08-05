using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectItemQuery : DbQueryReader<ItemID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", DBTables.Item);

        public SelectItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public ItemValues Execute(ItemID id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id");

            ItemValues retValues;

            using (IDataReader r = ExecuteReader(id))
            {
                if (!r.Read())
                    throw new DataException(string.Format("Query contained no results for id `{0}`.", id));

                retValues = ItemQueryHelper.ReadItemValues(r);
            }

            return retValues;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, ItemID id)
        {
            p["@id"] = (int)id;
        }
    }
}