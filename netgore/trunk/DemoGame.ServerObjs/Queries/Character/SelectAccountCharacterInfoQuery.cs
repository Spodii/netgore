using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectAccountCharacterInfoQuery : DbQueryReader<CharacterID>
    {
        static readonly string _queryStr = string.Format("SELECT `name`,`body_id` FROM `{0}` WHERE `id`=@id",
                                                         CharacterTable.TableName);

        /// <summary>
        /// DbQueryReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectAccountCharacterInfoQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ArePrimaryKeys(CharacterTable.DbKeyColumns, "id");
            QueryAsserts.ContainsColumns(CharacterTable.DbColumns, "name", "body_id");
        }

        public AccountCharacterInfo Execute(CharacterID id, byte accountCharacterIndex)
        {
            AccountCharacterInfo ret;

            using (IDataReader r = ExecuteReader(id))
            {
                if (!r.Read())
                    throw new ArgumentException(string.Format("Failed to find Character with ID `{0}`.", id));

                CharacterTable ct = new CharacterTable();
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
            return CreateParameters("@id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="characterID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID characterID)
        {
            p["@id"] = characterID;
        }
    }
}