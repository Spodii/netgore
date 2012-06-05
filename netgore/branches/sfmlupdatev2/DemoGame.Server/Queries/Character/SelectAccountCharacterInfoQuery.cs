using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectAccountCharacterInfoQuery : DbQueryReader<CharacterID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAccountCharacterInfoQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAccountCharacterInfoQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(ViewUserCharacterTable.DbColumns, "name", "body_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            /*
                SELECT `name`,`body_id` 
                    FROM `{0}` WHERE `id`=@id
            */

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(ViewUserCharacterTable.TableName).Add("name", "body_id").Where(f.Equals(s.EscapeColumn("id"),
                    s.Parameterize("id")));
            return q.ToString();
        }

        public AccountCharacterInfo Execute(CharacterID id, byte accountCharacterIndex)
        {
            AccountCharacterInfo ret;

            using (var r = ExecuteReader(id))
            {
                if (!r.Read())
                    return null;

                var name = r.GetString("name");
                var bodyID = r.GetBodyID("body_id");

                Debug.Assert(!string.IsNullOrEmpty(name));
                ret = new AccountCharacterInfo(accountCharacterIndex, name, bodyID);
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