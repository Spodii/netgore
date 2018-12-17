using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using log4net;
using MySql.Data.MySqlClient;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class CreateAccountQuery : DbQueryReader<CreateAccountQuery.QueryArgs>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public CreateAccountQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(AccountTable.DbColumns, "name", "password", "email", "time_created", "time_last_login",
                "creator_ip");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // INSERT IGNORE INTO `{0}`
            //      (`name`,`password`,`email`,`time_created`,`time_last_login`,`creator_ip`)
            //      VALUES (@name,@password,@email,NOW(),NOW(),@creator_ip)

            var f = qb.Functions;
            var q =
                qb.Insert(AccountTable.TableName).IgnoreExists().AddAutoParam("name", "password", "email", "creator_ip").Add(
                    "time_created", f.Now()).Add("time_last_login", f.Now());
            return q.ToString();
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("name", "password", "email", "creator_ip");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["name"] = item.Name;
            p["password"] = item.Password;
            p["email"] = item.Email;
            p["creator_ip"] = item.IP;
        }

        /// <summary>
        /// Tries to execute the query to create an account.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email.</param>
        /// <param name="ip">The IP address.</param>
        /// <returns>True if the account was successfully created; otherwise false.</returns>
        public bool TryExecute(string name, string password, string email, uint ip)
        {
            if (!GameData.AccountName.IsValid(name))
                return false;
            if (!GameData.AccountPassword.IsValid(password))
                return false;
            if (!GameData.AccountEmail.IsValid(email))
                return false;

            bool success;

            password = UserAccountManager.EncodePassword(password);
            var queryArgs = new QueryArgs(name, password, email, ip);
            try
            {
                using (var r = ExecuteReader(queryArgs))
                {
                    switch (r.RecordsAffected)
                    {
                        case 0:
                            success = false;
                            break;

                        case 1:
                            success = true;
                            break;

                        default:
                            success = true;
                            Debug.Fail("How was there more than one affected row!?");
                            break;
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        // Duplicate key
                        break;

                    default:
                        const string errmsg = "Failed to execute query. Exception: {0}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, ex);
                        Debug.Fail(string.Format(errmsg, ex));
                        break;
                }

                success = false;
            }

            return success;
        }

        /// <summary>
        /// The arguments for the <see cref="CreateAccountQuery"/> query.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public class QueryArgs
        {
            /// <summary>
            /// The email address.
            /// </summary>
            public readonly string Email;

            /// <summary>
            /// The IP address.
            /// </summary>
            public readonly uint IP;

            /// <summary>
            /// The name.
            /// </summary>
            public readonly string Name;

            /// <summary>
            /// The password.
            /// </summary>
            public readonly string Password;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="password">The password.</param>
            /// <param name="email">The email.</param>
            /// <param name="ip">The IP address.</param>
            public QueryArgs(string name, string password, string email, uint ip)
            {
                Name = name;
                Password = password;
                Email = email;
                IP = ip;
            }
        }
    }
}