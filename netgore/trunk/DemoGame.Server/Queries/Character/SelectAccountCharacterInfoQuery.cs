using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectAccountCharacterInfoQuery : DbQueryReader<CharacterID>
    {
        static readonly string _queryStr = FormatQueryString("SELECT `name`,`body_id` FROM `{0}` WHERE `id`=@id",
                                                             CharacterTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAccountCharacterInfoQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAccountCharacterInfoQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ArePrimaryKeys(CharacterTable.DbKeyColumns, "id");
            QueryAsserts.ContainsColumns(CharacterTable.DbColumns, "name", "body_id");
        }

        public AccountCharacterInfo Execute(CharacterID id, byte accountCharacterIndex)
        {
            AccountCharacterInfo ret;

            using (var r = ExecuteReader(id))
            {
                if (!r.Read())
                    return null;

                var ct = new CharacterTable();
                ct.TryReadValues(r);

                ret = new AccountCharacterInfo(accountCharacterIndex, ct.Name, ct.BodyID);
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
            return CreateParameters("id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="characterID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID characterID)
        {
            p["id"] = characterID;
        }
    }
}