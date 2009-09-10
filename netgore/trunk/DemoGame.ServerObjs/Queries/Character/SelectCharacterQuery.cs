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
    public class SelectCharacterQuery : DbQueryReader<string>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `name`=@name", CharacterTable.TableName);

        public SelectCharacterQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public CharacterTable Execute(string characterName)
        {
            CharacterTable ret;

            using (IDataReader r = ExecuteReader(characterName))
            {
                if (!r.Read())
                    throw new ArgumentException(string.Format("Could not find character `{0}`.", characterName), characterName);

                ret = new CharacterTable();
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
            return CreateParameters("@name");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, string item)
        {
            p["@name"] = item;
        }
    }
}