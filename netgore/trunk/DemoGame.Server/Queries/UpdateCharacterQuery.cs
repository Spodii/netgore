using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class UpdateCharacterQuery : DbQueryNonReader<ICharacterTable>
    {
        static readonly string _queryString = string.Format("UPDATE `{0}` SET {1} WHERE `id`=@id", CharacterTable.TableName,
            FormatParametersIntoString(CharacterTable.DbNonKeyColumns.Except(FieldsToNotUpdate, StringComparer.OrdinalIgnoreCase)));

        /// <summary>
        /// Gets the fields that will not be updated when the Character is updated.
        /// </summary>
        static IEnumerable<string> FieldsToNotUpdate
        {
            get { return new string[] { "password" }; }
        }

        public UpdateCharacterQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(CharacterTable.DbColumns.Select(x => "@" + x));
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="character">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ICharacterTable character)
        {
            character.TryCopyValues(p);
        }
    }
}