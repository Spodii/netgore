using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectAccountCharacterIDsQuery : DbQueryReader<AccountID>
    {
        static readonly string _queryStr = string.Format("SELECT `id` FROM `{0}` WHERE `account_id`=@accountID",
                                                         CharacterTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAccountCharacterIDsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectAccountCharacterIDsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(CharacterTable.DbColumns, "id", "account_id");
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
            return CreateParameters("@accountID");
        }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="accountID">The account ID.</param>
        protected override void SetParameters(DbParameterValues p, AccountID accountID)
        {
            p["@accountID"] = (int)accountID;
        }
    }
}