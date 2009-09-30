using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectItemsQuery : DbQueryReader<SelectItemsQuery.QueryValues>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id` BETWEEN @low AND @high",
                                                            ItemTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectItemsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectItemsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
            QueryAsserts.ArePrimaryKeys(ItemTable.DbKeyColumns, "id");
        }

        public IEnumerable<IItemTable> Execute(ItemID low, ItemID high)
        {
            var retValues = new List<IItemTable>();

            using (var r = ExecuteReader(new QueryValues(low, high)))
            {
                while (r.Read())
                {
                    if (!r.Read())
                        throw new DataException("Query contained no results for the specified Item ID range.");

                    var itemValues = new ItemTable();
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
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryValues item)
        {
            p["@low"] = (int)item.Low;
            p["@high"] = (int)item.High;
        }

        /// <summary>
        /// A struct of the values used to execute the <see cref="SelectItemsQuery"/>.
        /// </summary>
        public struct QueryValues
        {
            /// <summary>
            /// The inclusive upper-bound <see cref="ItemID"/>.
            /// </summary>
            public readonly ItemID High;

            /// <summary>
            /// The inclusive lower-bound <see cref="ItemID"/>.
            /// </summary>
            public readonly ItemID Low;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryValues"/> struct.
            /// </summary>
            /// <param name="low">The inclusive lower-bound <see cref="ItemID"/>.</param>
            /// <param name="high">The inclusive upper-bound <see cref="ItemID"/>.</param>
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
                    var tmp = low;
                    low = high;
                    high = tmp;
                }

                Low = low;
                High = high;
            }
        }
    }
}