using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectAllianceHostileQuery : DbQueryReader<AllianceID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAllianceHostileQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAllianceHostileQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT * FROM `{0}` WHERE `alliance_id`=@id

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(AllianceHostileTable.TableName).AllColumns().Where(f.Equals(s.EscapeColumn("alliance_id"),
                    s.Parameterize("id")));
            return q.ToString();
        }

        public IEnumerable<AllianceHostileTable> Execute(AllianceID id)
        {
            var ret = new List<AllianceHostileTable>();

            using (var r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    var item = new AllianceHostileTable();
                    item.ReadValues(r);
                    ret.Add(item);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>
        /// IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.
        /// </returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="id"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="id">The value used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, AllianceID id)
        {
            p["id"] = (int)id;
        }
    }
}