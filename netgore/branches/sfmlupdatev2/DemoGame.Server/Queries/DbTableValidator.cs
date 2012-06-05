using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Stats;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// Provides ways to perform runtime validation checks on database tables to ensure they are
    /// structured and named in ways that the code expects them to be.
    /// </summary>
    public static class DbTableValidator
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Ensures that the given columns exist.
        /// </summary>
        /// <param name="db">The <see cref="IDbController"/>.</param>
        /// <param name="dbTable">The name of the table to check.</param>
        /// <param name="columns">The columns to check that exist in the given <paramref name="dbTable"/>.</param>
        static void EnsureColumnsExist(IDbController db, string dbTable, IEnumerable<string> columns)
        {
            var dbColumns = db.GetTableColumns(dbTable);
            foreach (var column in columns)
            {
                if (!dbColumns.Contains(column))
                    Error("Table `{0}` does not contain required column `{1}`.", dbTable, column);
            }
        }

        /// <summary>
        /// Ensures that the columns needed for <see cref="StatType"/>s exist in a table.
        /// </summary>
        /// <param name="db">The <see cref="IDbController"/>.</param>
        /// <param name="dbTable">The name of the table to check.</param>
        /// <param name="columns">The <see cref="StatType"/>s to ensure exist.</param>
        /// <param name="statCollectionType">The <see cref="StatCollectionType"/> to use for checking.</param>
        static void EnsureStatColumnsExist(IDbController db, string dbTable, IEnumerable<StatType> columns,
                                           StatCollectionType statCollectionType)
        {
            EnsureColumnsExist(db, dbTable, columns.Select(x => x.GetDatabaseField(statCollectionType)));
        }

        /// <summary>
        /// Raises an error. Used by the assertion methods in this class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="DatabaseException">Always occurs when this method is called.</exception>
        static void Error(string message, params object[] parameters)
        {
            if (log.IsFatalEnabled)
                log.FatalFormat(message, parameters);

            Debug.Fail(string.Format(message, parameters));

            throw new DatabaseException(string.Format("Database tables validation failed. Reason: " + message, parameters));
        }

        /// <summary>
        /// Performs validation checks on the Character table.
        /// </summary>
        /// <param name="db">The DbController to use for performing the validation checks.</param>
        static void ValidateCharacterTable(IDbController db)
        {
            EnsureStatColumnsExist(db, CharacterTable.TableName, EnumHelper<StatType>.Values, StatCollectionType.Base);
        }

        /// <summary>
        /// Performs validation checks on the CharacterTemplate table.
        /// </summary>
        /// <param name="db">The DbController to use for performing the validation checks.</param>
        static void ValidateCharacterTemplateTable(IDbController db)
        {
            EnsureStatColumnsExist(db, CharacterTemplateTable.TableName, EnumHelper<StatType>.Values, StatCollectionType.Base);
        }

        /// <summary>
        /// Performs validation checks on the database tables to ensure the schemas are correct.
        /// </summary>
        /// <param name="db">The DbController to use for performing the validation checks.</param>
        public static void ValidateTables(IDbController db)
        {
            ValidateCharacterTable(db);
            ValidateCharacterTemplateTable(db);
        }
    }
}