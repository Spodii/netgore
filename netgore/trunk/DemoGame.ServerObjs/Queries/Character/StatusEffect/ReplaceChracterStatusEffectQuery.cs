using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class ReplaceCharacterStatusEffectQuery : DbQueryNonReader<ICharacterStatusEffectTable>
    {
        static readonly string _queryString = string.Format("REPLACE INTO `{0}` {1}", CharacterStatusEffectTable.TableName,
                                                            FormatParametersIntoValuesString(CharacterStatusEffectTable.DbColumns));

        public ReplaceCharacterStatusEffectQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(CharacterStatusEffectTable.DbColumns.Select(x => "@" + x));
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <typeparam name="T">Type of the object containing the values to set.</typeparam>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="characterID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ICharacterStatusEffectTable item)
        {
            item.CopyValues(p);
        }
    }
}