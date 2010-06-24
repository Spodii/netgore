using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class UpdateCharacterQuery : DbQueryNonReader<ICharacterTable>
    {
        static readonly string _queryStr = FormatQueryString("UPDATE `{0}` SET {1} WHERE `id`=@id", CharacterTable.TableName,
                                                            FormatParametersIntoString(CharacterTable.DbNonKeyColumns));

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCharacterQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public UpdateCharacterQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(CharacterTable.DbColumns);
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="character">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ICharacterTable character)
        {
            character.TryCopyValues(p);
        }
    }
}