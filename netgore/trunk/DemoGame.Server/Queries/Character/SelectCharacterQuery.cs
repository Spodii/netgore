using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectCharacterQuery : DbQueryReader<string>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `name`=@name", DBTables.Character);

        public SelectCharacterQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public SelectCharacterQueryValues Execute(string characterName)
        {
            SelectCharacterQueryValues ret;

            using (IDataReader r = ExecuteReader(characterName))
            {
                if (!r.Read())
                    throw new ArgumentException(string.Format("Could not find character `{0}`.", characterName), characterName);

                ret = CharacterQueryHelper.ReadCharacterQueryValues(r);
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@name");
        }

        protected override void SetParameters(DbParameterValues p, string item)
        {
            p["@name"] = item;
        }
    }
}