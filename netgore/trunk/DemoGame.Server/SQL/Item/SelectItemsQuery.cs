using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DemoGame.Extensions;
using NetGore.Db;
using NetGore.Extensions;

namespace DemoGame.Server
{
    public class SelectItemsQuery : SelectItemQueryBase<SelectItemsQueryValues>
    {
        const string _queryString = "SELECT * FROM `items` WHERE `guid` BETWEEN @low AND @high";

        public SelectItemsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<ItemValues> Execute(int low, int high)
        {
            return Execute(new SelectItemsQueryValues(low, high));
        }

        public IEnumerable<ItemValues> Execute(SelectItemsQueryValues values)
        {
            List<ItemValues> retValues = new List<ItemValues>();

            using (var r = ExecuteReader(values))
            {
                while (r.Read())
                {
                    if (!r.Read())
                        throw new DataException("Query contained no results for the specified item guid range.");

                    ItemValues tempValues = GetItemValues(r);
                    retValues.Add(tempValues);
                }
            }

            return retValues;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@low", "@high");
        }

        protected override void SetParameters(DbParameterValues p, SelectItemsQueryValues item)
        {
            p["@low"] = item.Low;
            p["@high"] = item.High;
        }
    }

    public struct SelectItemsQueryValues
    {
        public readonly int Low;
        public readonly int High;

        public SelectItemsQueryValues(int low, int high)
        {
            if (low > high)
            {
                Debug.Fail("low is greater than high.");
                
                // Swap values
                // HACK: Should have an extension for swapping values
                int tmp = low;
                low = high;
                high = tmp;
            }

            if (low < 0)
                throw new ArgumentOutOfRangeException("low", "Value must be greater than or equal to 0.");
            if (high < 0)
                throw new ArgumentOutOfRangeException("high", "Value must be greater than or equal to 0.");
            
            Low = low;
            High = high;
        }
    }
}
