using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class UpdateAccountFriendsQuery : DbQueryNonReader<KeyValuePair<AccountID, string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAccountFriendsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public UpdateAccountFriendsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(AccountTable.DbColumns, "id", "friends");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // UPDATE `{0}` SET `friends` = @value WHERE `id`=@id

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Update(AccountTable.TableName).Add("friends", s.Parameterize("value")).Where(f.Equals(
                    s.EscapeColumn("id"), s.Parameterize("id")));
            return q.ToString();
        }

        /// <summary>
        /// Executes the query on the database using the specified values.
        /// </summary>
        /// <param name="id">The <see cref="AccountID"/>.</param>
        /// <param name="friends">The new <see cref="string"/> value.</param>
        public void Execute(AccountID id, string friends)
        {
            Execute(new KeyValuePair<AccountID, string>(id, friends));
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("id", "value");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, KeyValuePair<AccountID, string> item)
        {
            p["id"] = (int)item.Key;
            p["value"] = (string)item.Value;
        }
    }
}