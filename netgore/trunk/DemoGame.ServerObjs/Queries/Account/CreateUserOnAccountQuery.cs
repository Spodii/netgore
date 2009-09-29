using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class CreateUserOnAccountQuery : DbQueryReader<CreateUserOnAccountQuery.QueryArgs>
    {
        const string _queryStr = "SELECT CreateUserOnAccount(@accountID, @characterName, @characterID)";

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserOnAccountQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public CreateUserOnAccountQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@accountID", "@characterName", "@characterID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@accountID"] = (int)item.AccountID;
            p["@characterName"] = item.CharacterName;
            p["@characterID"] = (int)item.CharacterID;
        }

        public bool TryExecute(AccountID accountID, CharacterID characterID, string characterName, out string errorMsg)
        {
            if (!GameData.CharacterName.IsValid(characterName))
            {
                errorMsg = "Invalid character name.";
                return false;
            }

            QueryArgs queryArgs = new QueryArgs(accountID, characterID, characterName);
            using (IDataReader r = ExecuteReader(queryArgs))
            {
                if (!r.Read())
                {
                    errorMsg = "Unknown error.";
                    return false;
                }

                errorMsg = r.GetString(0);
            }

            // Make sure the error message is trimmed, and empty if there was no error
            if (errorMsg == null || errorMsg.Length <= 1)
            {
                errorMsg = string.Empty;
                return true;
            }

            errorMsg = errorMsg.Trim();
            if (errorMsg.Length <= 1)
            {
                errorMsg = string.Empty;
                return true;
            }

            // The error string wasn't empty, so must be an actual error message, meaning it wasn't successful
            return false;
        }

        /// <summary>
        /// The arguments for the <see cref="CreateUserOnAccountQuery"/> query.
        /// </summary>
        public struct QueryArgs
        {
            /// <summary>
            /// The account ID.
            /// </summary>
            public readonly AccountID AccountID;

            /// <summary>
            /// The character ID.
            /// </summary>
            public readonly CharacterID CharacterID;

            /// <summary>
            /// The character name.
            /// </summary>
            public readonly string CharacterName;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="accountID">The account ID.</param>
            /// <param name="characterID">The character ID.</param>
            /// <param name="characterName">Name of the character.</param>
            public QueryArgs(AccountID accountID, CharacterID characterID, string characterName)
            {
                AccountID = accountID;
                CharacterID = characterID;
                CharacterName = characterName;
            }
        }
    }
}