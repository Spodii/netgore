using System;
using System.IO;
using System.Linq;
using NetGore.Db.Schema;

namespace InstallationValidator.Tests
{
    public sealed class LoadSchemaFile : TestableBase
    {
        const string _description =
            "Attempts to load the database schema file that describes the schema of the database for this release. If this file cannot be loaded, the engine will function fine, but the Installation Validator will not be able to check if your database schema is up-to-date.";

        const string _failMessage = "Failed to load the database schema file from: {0}";
        const string _testName = "Load database schema file";

        static SchemaReader _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadSchemaFile"/> class.
        /// </summary>
        public LoadSchemaFile() : base(_testName, _description)
        {
        }

        /// <summary>
        /// Gets the most recently loaded <see cref="SchemaReader"/> from this test. If null, the test likely failed
        /// and the schema was not loaded.
        /// </summary>
        public static SchemaReader Schema
        {
            get { return _schema; }
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
            var schemaFile = MySqlHelper.DbSchemaFile;

            if (!File.Exists(schemaFile))
            {
                const string errmsg =
                    "The database schema file could not be found at path `{0}`, so this program will not be" +
                    " able to check your database schema to see if it is up-to-date.";
                errorMessage = string.Format(errmsg, schemaFile);
                return false;
            }

            try
            {
                var schema = SchemaReader.Load(schemaFile);
                _schema = schema;
            }
            catch (Exception ex)
            {
                errorMessage = AppendErrorDetails(string.Format(_failMessage, schemaFile), ex.ToString());
                return false;
            }

            return true;
        }
    }
}