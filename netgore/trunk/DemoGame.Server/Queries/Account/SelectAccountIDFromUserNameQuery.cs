using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectAccountIDFromUserNameQuery : DbQueryReader<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAccountIDFromUserNameQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectAccountIDFromUserNameQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            /*
                SELECT a.account_id FROM `account_character` a
                    INNER JOIN `view_user_character` c
                        ON c.id = a.character_id
                WHERE c.name = @name;
            */

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(AccountCharacterTable.TableName, "a").AddFunc("a.account_id").InnerJoinOnColumn(
                    ViewUserCharacterTable.TableName, "c", "id", "a", "character_id").Where(f.Equals("c.name",
                        s.Parameterize("name")));
            return q.ToString();
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="accountName">The user name to get the <see cref="AccountID"/> for.</param>
        /// <returns>The ID of the account containing the user with the given <paramref name="accountName"/>, or null if no such
        /// user exists or that user is not on any account.</returns>
        public AccountID? Execute(string accountName)
        {
            AccountID? ret;

            using (var r = ExecuteReader(accountName))
            {
                if (!r.Read())
                    ret = null;
                else
                    ret = r.GetAccountID(0);
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("name");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, string item)
        {
            p["name"] = item;
        }
    }
}