using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using log4net;
using MySql.Data.MySqlClient;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class CreateAccountQuery : DbQueryReader<CreateAccountQuery.QueryArgs>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly string _queryStr =
            string.Format(
                "INSERT INTO `{0}` (`id`,`name`,`password`,`email`,`time_created`,`time_last_login`,`creator_ip`)" +
                " VALUES (@id,@name,@password,@email,NOW(),NOW(),@ip)", AccountTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public CreateAccountQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(AccountTable.DbColumns, "id", "name", "password", "email", "time_created",
                                         "time_last_login");
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id", "@name", "@password", "@email", "@ip");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@id"] = (int)item.AccountID;
            p["@name"] = item.Name;
            p["@password"] = item.Password;
            p["@email"] = item.Email;
            p["@ip"] = item.IP;
        }

        /// <summary>
        /// Tries to execute the query to create an account.
        /// </summary>
        /// <param name="accountID">The account ID.</param>
        /// <param name="name">The name.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email.</param>
        /// <param name="ip">The IP address.</param>
        /// <returns>True if the account was successfully created; otherwise false.</returns>
        public bool TryExecute(AccountID accountID, string name, string password, string email, uint ip)
        {
            if (!GameData.AccountName.IsValid(name))
                return false;
            if (!GameData.AccountPassword.IsValid(password))
                return false;
            if (!GameData.AccountEmail.IsValid(email))
                return false;

            bool success;

            password = UserAccount.EncodePassword(password);
            var queryArgs = new QueryArgs(accountID, name, password, email, ip);
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
        public class QueryArgs
        {
            /// <summary>
            /// The account ID.
            /// </summary>
            public readonly AccountID AccountID;

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
            /// <param name="accountID">The account ID.</param>
            /// <param name="name">The name.</param>
            /// <param name="password">The password.</param>
            /// <param name="email">The email.</param>
            /// <param name="ip">The IP address.</param>
            public QueryArgs(AccountID accountID, string name, string password, string email, uint ip)
            {
                AccountID = accountID;
                Name = name;
                Password = password;
                Email = email;
                IP = ip;
            }
        }
    }
}