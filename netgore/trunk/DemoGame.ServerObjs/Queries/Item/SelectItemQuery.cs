using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

// TODO: !! Cleanup query

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectItemQuery : DbQueryReader<ItemID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", ItemTable.TableName);

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

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="id">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ItemID id)
        {
            p["@id"] = (int)id;
        }
    }
}