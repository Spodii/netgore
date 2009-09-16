using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterByIDQuery : DbQueryReader<CharacterID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", CharacterTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterByIDQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterByIDQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
            QueryAsserts.ArePrimaryKeys(CharacterTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// Executes the specified character ID.
        /// </summary>
        /// <param name="characterID">The character ID.</param>
        /// <returns>The ICharacterTable for the Character with the given <paramref name="characterID"/>.</returns>
        public ICharacterTable Execute(CharacterID characterID)
        {
            CharacterTable ret;

            using (IDataReader r = ExecuteReader(characterID))
            {
                if (!r.Read())
                    throw new ArgumentException(string.Format("Could not find character with ID `{0}`.", characterID),
                                                characterID.ToString());

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
            return CreateParameters("@id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="characterID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID characterID)
        {
            p["@id"] = (int)characterID;
        }
    }
}