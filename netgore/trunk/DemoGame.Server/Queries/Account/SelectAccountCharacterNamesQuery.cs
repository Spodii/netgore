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
    public class SelectAccountCharacterNamesQuery : DbQueryReader<AccountID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAccountCharacterNamesQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAccountCharacterNamesQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(CharacterTable.DbColumns, "name", "id");
            QueryAsserts.ContainsColumns(AccountCharacterTable.DbColumns, "account_id", "character_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            /*
                SELECT c.name FROM `account_character` a
	                INNER JOIN `view_user_character` c
		                ON a.character_id = c.id
	                WHERE a.account_id = @accountID;
            */

            var f = qb.Functions;
            var q =
                qb.Select(AccountCharacterTable.TableName, "a").AddFunc("c.name").InnerJoinOnColumn(
                    ViewUserCharacterTable.TableName, "c", "id", "a", "character_id").Where(f.Equals("a.account_id", "@accountID"));
            return q.ToString();
        }

        public IEnumerable<string> Execute(AccountID accountID)
        {
            var ret = new List<string>(4);

            using (var r = ExecuteReader(accountID))
            {
                while (r.Read())
                {
                    Debug.Assert(r.FieldCount == 1);
                    var value = r.GetString(0);
                    ret.Add(value);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>
        /// IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.
        /// </returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("accountID");
        }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="accountID">The account ID.</param>
        protected override void SetParameters(DbParameterValues p, AccountID accountID)
        {
            p["accountID"] = (int)accountID;
        }
    }
}