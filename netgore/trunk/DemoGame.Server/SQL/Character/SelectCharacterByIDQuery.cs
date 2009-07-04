using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectCharacterByIDQuery : DbQueryReader<uint>
    {
        const string _queryString = "SELECT * FROM `characters` WHERE `guid`=@characterID";

        public SelectCharacterByIDQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public SelectCharacterQueryValues Execute(uint characterID, CharacterStatsBase stats)
        {
            if (stats == null)
                throw new ArgumentNullException("stats");

            SelectCharacterQueryValues ret;

            using (IDataReader r = ExecuteReader(characterID))
            {
                ret = SelectCharacterQuery.ReadQueryValues(r, stats);
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@characterID");
        }

        protected override void SetParameters(DbParameterValues p, uint characterID)
        {
            p["@characterID"] = characterID;
        }
    }
}
