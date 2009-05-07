using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;
using NetGore.Db;
using NetGore.Extensions;

namespace DemoGame.Server
{
    public class SelectItemQuery : SelectItemQueryBase<int>
    {
        const string _queryString = "SELECT * FROM `items` WHERE `guid`=@guid";

        public SelectItemQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<System.Data.Common.DbParameter> InitializeParameters()
        {
            return CreateParameters("@guid");
        }

        protected override void SetParameters(DbParameterValues p, int item)
        {
            p["@guid"] = item;
        }

        public ItemValues Execute(int guid)
        {
            if (guid <= 0)
                throw new ArgumentOutOfRangeException("guid");

            ItemValues retValues;

            using (var r = ExecuteReader(guid))
            {
                if (!r.Read())
                    throw new DataException("Query contained no results for the specified guid.");

                retValues = GetItemValues(r);
            }

            return retValues;
        }
    }
}