using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectItemsQuery : DbQueryReader<SelectItemsQueryValues>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id` BETWEEN @low AND @high", DBTables.Item);

        public SelectItemsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<ItemValues> Execute(ItemID low, ItemID high)
        {
            return Execute(new SelectItemsQueryValues(low, high));
        }

        public IEnumerable<ItemValues> Execute(SelectItemsQueryValues values)
        {
            var retValues = new List<ItemValues>();

            using (IDataReader r = ExecuteReader(values))
            {
                while (r.Read())
                {
                    if (!r.Read())
                        throw new DataException("Query contained no results for the specified Item ID range.");

                    ItemValues tempValues = ItemQueryHelper.ReadItemValues(r);
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
        public readonly ItemID High;
        public readonly ItemID Low;

        public SelectItemsQueryValues(ItemID low, ItemID high)
        {
            if (low > high)
            {
                Debug.Fail("low is greater than high. Can be fixed, but this is often the indication of a bigger problem.");

                // Swap values
                ItemID tmp = low;
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