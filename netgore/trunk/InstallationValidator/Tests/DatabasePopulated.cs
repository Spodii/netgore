using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Db.Schema;

namespace InstallationValidator.Tests
{
    public class DatabasePopulated : ITestable
    {
        /// <summary>
        /// Actually runs the test.
        /// </summary>
        /// <param name="print">Whether or not to print messages.</param>
        /// <returns>If false, call <see cref="MySqlHelper.AskToImportDatabase"/>.</returns>
        static bool TestInternal(bool print)
        {
            const string testName = "Database populated";

            var schema = LoadSchemaFile.Schema;
            if (schema == null)
            {
                const string failmsg = "Could not run test since the database schema file failed to load.";
                if (print)
                    Tester.Test(testName, false, failmsg);
                return true;
            }

            SchemaReader userSchema = new SchemaReader(MySqlHelper.ConnectionSettings);
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

            string msg = string.Format(failmsg2, ToNewLines(diffs));

            bool passed = diffs == null || diffs.Count() == 0;

            if (print)
                Tester.Test(testName, passed, msg);
            else if (!passed)
                Tester.Write(msg, System.ConsoleColor.White);

            return passed;
        }

        static string ToNewLines(IEnumerable<string> s)
        {
            StringBuilder ret = new StringBuilder();
            foreach (var item in s)
            {
                ret.AppendLine(item);
            }
            return ret.ToString();
        }

        #region ITestable Members

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            if (!TestInternal(false))
                MySqlHelper.AskToImportDatabase(true);

            TestInternal(true);
        }

        #endregion
    }
}