using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectAllianceQuery : DbQueryReader<AllianceID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", AllianceTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAllianceQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAllianceQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
            QueryAsserts.ArePrimaryKeys(AllianceTable.DbKeyColumns, "id");
        }

        public AllianceTable Execute(AllianceID id)
        {
            AllianceTable ret;

            using (var r = ExecuteReader(id))
            {
                if (!r.Read())
                    return null;

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
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="id">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, AllianceID id)
        {
            p["@id"] = (int)id;
        }
    }
}