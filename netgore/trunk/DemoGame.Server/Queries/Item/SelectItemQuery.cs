using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectItemQuery : DbQueryReader<ItemID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectItemQuery(DbConnectionPool connectionPool) : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ArePrimaryKeys(ItemTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT * FROM `{0}` WHERE `id`=@id

            var f = qb.Functions;
            var s = qb.Settings;
            var q = qb.Select(ItemTable.TableName).AllColumns().Where(f.Equals(s.EscapeColumn("id"), s.Parameterize("id")));
            return q.ToString();
        }

        /// <summary>
        /// Selects the <see cref="IItemTable"/> for the given <see cref="ItemID"/>.
        /// </summary>
        /// <param name="id">The <see cref="ItemID"/> of the item to select.</param>
        /// <returns>The <see cref="IItemTable"/> for the given <paramref name="id"/>,
        /// or null if the <paramref name="id"/> is invalid.</returns>
        public IItemTable Execute(ItemID id)
        {
            Debug.Assert(id > 0, "ID should always be > 0.");

            ItemTable retValues;

            using (var r = ExecuteReader(id))
            {
                if (!r.Read())
                    return null;

                retValues = new ItemTable();
                retValues.ReadValues(r);
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
            return CreateParameters("id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="id">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ItemID id)
        {
            p["id"] = (int)id;
        }
    }
}