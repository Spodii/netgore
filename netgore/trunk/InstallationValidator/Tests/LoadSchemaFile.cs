using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetGore.Db.Schema;

namespace InstallationValidator.Tests
{
    public class LoadSchemaFile : ITestable
    {
        static SchemaReader _schema;

        /// <summary>
        /// Gets the most recently loaded <see cref="SchemaReader"/> from this test. If null, the test likely failed
        /// and the schema was not loaded.
        /// </summary>
        public static SchemaReader Schema
        {
            get { return _schema; }
        }

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Load database schema file";

            string schemaFile = MySqlHelper.DbSchemaFile;

            if (!File.Exists(schemaFile))
            {
                const string failmsg = "The database schema file could not be found at `{0}`, so this program will not be able to check your database schema to see if it is up-to-date.";
                Tester.Test(testName, false, string.Format(failmsg, schemaFile));
                return;
            }

            try
            {
                var schema = SchemaReader.Load(schemaFile);
                _schema = schema;
            }
            catch (Exception ex)
            {
                const string failmsg = "Failed to load database schema file at `{0}`. Details:\n";
                Tester.Test(testName, false, failmsg + ex);
                return;
            }

            Tester.Test(testName, true, null);
        }
    }
}
