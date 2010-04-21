using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using NetGore.Db.Schema;

namespace InstallationValidator.Tests
{
    public sealed class DatabasePopulated : TestableBase
    {
        const string _description =
            "Checks to make sure the database is populated, and using the expected schema. The schema check is primarily for ensuring that your database is up-to-date after upgrading NetGore. While the engine can function if the schema does not match, it is not guaranteed, and not recommended unless you change the respective code.";

        const string _testName = "Database populated";

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabasePopulated"/> class.
        /// </summary>
        public DatabasePopulated() : base(_testName, _description)
        {
        }

        /// <summary>
        /// When overridden in the derived class, runs the test.
        /// </summary>
        /// <param name="errorMessage">When the method returns false, contains an error message as to why
        /// the test failed. Otherwise, contains an empty string.</param>
        /// <returns>
        /// True if the test passed; false if the test failed.
        /// </returns>
        protected override bool RunTest(ref string errorMessage)
        {
            string errmsg;
            if (!TestInternal(out errmsg))
                MySqlHelper.AskToImportDatabase(true);

            var success = TestInternal(out errmsg);

            if (!success)
                errorMessage = errmsg;

            return success;
        }

        /// <summary>
        /// Actually runs the test.
        /// </summary>
        /// <returns>If false, call <see cref="MySqlHelper.AskToImportDatabase"/>.</returns>
        static bool TestInternal(out string errmsg)
        {
            var schema = LoadSchemaFile.Schema;
            if (schema == null)
            {
                errmsg = "Could not run test since the database schema file failed to load.";
                return true;
            }

            SchemaReader userSchema;
            try
            {
                userSchema = new SchemaReader(MySqlHelper.ConnectionSettings);
            }
            catch (MySqlException ex)
            {
                errmsg = AppendErrorDetails("Failed to read the database schema from MySql. Error: " + ex.Message, ex.ToString());
                return false;
            }

            var diffs = SchemaComparer.Compare(schema, userSchema);

            const string failmsg2 =
                "\nOne or more differences were found in your database schema in comparison to" +
                " the release's database schema. If you have manually altered your database's schema (such as add" +
                " fields or alter field types), you can ignore this message. Otherwise, this likely means that you" +
                " are using the wrong database dump file for this version of NetGore. Often times, this is because" +
                " you have the database from an older version of NetGore that is not compatible with this version." +
                "\nTo resolve this issue, please use the database dump (db.sql) from this version of NetGore." +
                " Make sure to back up your database if you want to save any of the contents!" +
                "\n\nThe following is a list of all the differences found in your database schema: \n{0}";

            errmsg = string.Format(failmsg2, ToNewLines(diffs));

            return diffs == null || diffs.Count() == 0;
        }

        static string ToNewLines(IEnumerable<string> s)
        {
            var ret = new StringBuilder();
            foreach (var item in s)
            {
                ret.AppendLine(item);
            }
            return ret.ToString();
        }
    }
}