using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectAllianceAttackableQuery : DbQueryReader<AllianceID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `alliance_id`=@id",
                                                            DBTables.AllianceAttackable);

        public SelectAllianceAttackableQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public SelectAllianceAttackableQueryValues Execute(AllianceID id)
        {
            var attackableIDs = new List<AllianceID>();

            using (IDataReader r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    AllianceID attackableID = r.GetAllianceID("attackable_id");
                    attackableIDs.Add(attackableID);
                }
            }

            SelectAllianceAttackableQueryValues ret = new SelectAllianceAttackableQueryValues(id, attackableIDs);
            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, AllianceID id)
        {
            p["@id"] = id;
        }
    }
}