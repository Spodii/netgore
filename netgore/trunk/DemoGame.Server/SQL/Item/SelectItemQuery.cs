using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectItemQuery : SelectItemQueryBase<int>
    {
        const string _queryString = "SELECT * FROM `item` WHERE `id`=@id";

        public SelectItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public ItemValues Execute(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id");

            ItemValues retValues;

            using (IDataReader r = ExecuteReader(id))
            {
                if (!r.Read())
                    throw new DataException(string.Format("Query contained no results for id `{0}`.", id));

                retValues = GetItemValues(r);
            }

            return retValues;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, int id)
        {
            p["@id"] = id;
        }
    }
}