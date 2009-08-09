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
    public class SelectCharacterByIDQuery : DbQueryReader<CharacterID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", DBTables.Character);

        public SelectCharacterByIDQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public CharacterTable Execute(CharacterID characterID)
        {
            CharacterTable ret;

            using (IDataReader r = ExecuteReader(characterID))
            {
                if (!r.Read())
                    throw new ArgumentException(string.Format("Could not find character with ID `{0}`.", characterID),
                                                characterID.ToString());

                ret = new CharacterTable(r);
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, CharacterID characterID)
        {
            p["@id"] = (int)characterID;
        }
    }
}