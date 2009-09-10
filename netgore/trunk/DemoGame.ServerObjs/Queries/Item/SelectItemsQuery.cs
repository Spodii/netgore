using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectItemsQuery : DbQueryReader<SelectItemsQuery.QueryValues>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id` BETWEEN @low AND @high",
                                                            ItemTable.TableName);

        public SelectItemsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
            QueryAsserts.ArePrimaryKeys(ItemTable.DbKeyColumns, "id");
        }

        public IEnumerable<IItemTable> Execute(ItemID low, ItemID high)
        {
            return Execute(new QueryValues(low, high));
        }

        public IEnumerable<IItemTable> Execute(QueryValues values)
        {
            var retValues = new List<IItemTable>();

            using (IDataReader r = ExecuteReader(values))
            {
                while (r.Read())
                {
                    if (!r.Read())
                        throw new DataException("Query contained no results for the specified Item ID range.");

                    ItemTable itemValues = new ItemTable();
                    itemValues.ReadValues(r);
                    retValues.Add(itemValues);
                }
            }

            return retValues;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@low", "@high");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="characterID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryValues item)
        {
            p["@low"] = (int)item.Low;
            p["@high"] = (int)item.High;
        }

        public struct QueryValues
        {
            public readonly ItemID High;
            public readonly ItemID Low;

            public QueryValues(ItemID low, ItemID high)
            {
                if (low < 0)
                    throw new ArgumentOutOfRangeException("low", "Value must be greater than or equal to 0.");
                if (high < 0)
                    throw new ArgumentOutOfRangeException("high", "Value must be greater than or equal to 0.");

                if (low > high)
                {
                    Debug.Fail("low is greater than high. Can be fixed, but this is often the indication of a bigger problem.");

                    // Swap values
                    ItemID tmp = low;
                    low = high;
                    high = tmp;
                }

                Low = low;
                High = high;
            }
        }
    }
}