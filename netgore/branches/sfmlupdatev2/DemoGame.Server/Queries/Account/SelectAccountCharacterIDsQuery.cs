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
    public class SelectAccountCharacterIDsQuery : DbQueryReader<AccountID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAccountCharacterIDsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAccountCharacterIDsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(AccountCharacterTable.DbColumns, "character_id", "account_id", "time_deleted");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            /*
                SELECT `character_id` FROM `account_character`
                    WHERE `account_id` = @accountID
                        AND `time_deleted` IS NULL;
            */

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(AccountCharacterTable.TableName).Add("character_id").Where(
                    f.And(f.Equals(s.EscapeColumn("account_id"), s.Parameterize("accountID")),
                        f.IsNull(s.EscapeColumn("time_deleted"))));
            return q.ToString();
        }

        public IEnumerable<CharacterID> Execute(AccountID accountID)
        {
            var ret = new List<CharacterID>(4);

            using (var r = ExecuteReader(accountID))
            {
                while (r.Read())
                {
                    Debug.Assert(r.FieldCount == 1);
                    var value = r.GetCharacterID(0);
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