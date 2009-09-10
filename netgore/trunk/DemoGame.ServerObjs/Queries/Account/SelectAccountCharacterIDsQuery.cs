using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectAccountCharacterIDsQuery : DbQueryReader<int>
    {
        static readonly string _queryStr = string.Format("SELECT `id` FROM `{0}` WHERE `account_id`=@accountID",
            CharacterTable.TableName);

        /// <summary>
        /// DbQueryReader constructor.
        /// </summary>
        public SelectAccountCharacterIDsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(CharacterTable.DbColumns, "id", "account_id");
        }

        public IEnumerable<CharacterID> Execute(int accountID)
        {
            List<CharacterID> ret = new List<CharacterID>(4);

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
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@accountID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="accountID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, int accountID)
        {
            p["@accountID"] = accountID;
        }
    }
}
