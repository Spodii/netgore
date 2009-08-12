using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectAllianceQuery : DbQueryReader<AllianceID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", AllianceTable.TableName);

        public SelectAllianceQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
            QueryAsserts.ArePrimaryKeys(AllianceTable.DbKeyColumns, "id");
        }

        public AllianceTable Execute(AllianceID id)
        {
            AllianceTable ret;

            using (IDataReader r = ExecuteReader(id))
            {
                if (!r.Read())
                    throw new ArgumentOutOfRangeException("id", string.Format("No alliance found for id `{0}`.", id));

                ret = new AllianceTable();
                ret.ReadValues(r);
            }

            return ret;
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
        protected override void SetParameters(DbParameterValues p, AllianceID id)
        {
            p["@id"] = (int)id;
        }
    }
}