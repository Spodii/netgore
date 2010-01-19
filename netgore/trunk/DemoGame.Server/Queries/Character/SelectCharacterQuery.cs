using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterQuery : DbQueryReader<string>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `name`=@name", CharacterTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        /// <summary>
        /// Executes the specified character name.
        /// </summary>
        /// <param name="characterName">The name of the character.</param>
        /// <returns>The ICharacterTable for the Character with the name <paramref name="characterName"/>.</returns>
        public ICharacterTable Execute(string characterName)
        {
            CharacterTable ret;

            using (var r = ExecuteReader(characterName))
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