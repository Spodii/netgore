using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectItemQuery : SelectItemQueryBase<int>
    {
        const string _queryString = "SELECT * FROM `items` WHERE `guid`=@guid";

        public SelectItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public ItemValues Execute(int guid)
        {
            if (guid <= 0)
                throw new ArgumentOutOfRangeException("guid");

            ItemValues retValues;

            using (IDataReader r = ExecuteReader(guid))
            {
                if (!r.Read())
                    throw new DataException("Query contained no results for the specified guid.");

                retValues = GetItemValues(r);
            }

            return retValues;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@guid");
        }

        protected override void SetParameters(DbParameterValues p, int item)
        {
            p["@guid"] = item;
        }
    }
}