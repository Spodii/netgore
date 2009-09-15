using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectAllianceAttackableQuery : DbQueryReader<AllianceID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `alliance_id`=@id",
                                                            AllianceAttackableTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAllianceAttackableQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAllianceAttackableQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<AllianceAttackableTable> Execute(AllianceID id)
        {
            var ret = new List<AllianceAttackableTable>();

            using (IDataReader r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    AllianceAttackableTable item = new AllianceAttackableTable();
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
            return CreateParameters("@id");
        }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="id">The id.</param>
        protected override void SetParameters(DbParameterValues p, AllianceID id)
        {
            p["@id"] = (int)id;
        }
    }
}