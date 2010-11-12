using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectAllianceAttackableQuery : DbQueryReader<AllianceID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAllianceAttackableQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAllianceAttackableQuery(DbConnectionPool connectionPool)
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
                qb.Select(AllianceAttackableTable.TableName).AllColumns().Where(f.Equals(s.EscapeColumn("alliance_id"),
                    s.Parameterize("id")));
            return q.ToString();
        }

        public IEnumerable<AllianceAttackableTable> Execute(AllianceID id)
        {
            var ret = new List<AllianceAttackableTable>();

            using (var r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    var item = new AllianceAttackableTable();
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
        /// Sets the parameters.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="id">The id.</param>
        protected override void SetParameters(DbParameterValues p, AllianceID id)
        {
            p["id"] = (int)id;
        }
    }
}