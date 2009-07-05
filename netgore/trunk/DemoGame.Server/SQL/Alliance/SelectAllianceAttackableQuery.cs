using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class SelectAllianceAttackableQuery : DbQueryReader<byte>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `alliance_id`=@id", DBTables.AllianceAttackable);

        public SelectAllianceAttackableQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public SelectAllianceAttackableQueryValues Execute(byte id)
        {
            var attackableIDs = new List<byte>();

            using (var r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    byte attackableID = r.GetByte("attackable_id");
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

        protected override void SetParameters(DbParameterValues p, byte id)
        {
            p["@id"] = id;
        }
    }
}
