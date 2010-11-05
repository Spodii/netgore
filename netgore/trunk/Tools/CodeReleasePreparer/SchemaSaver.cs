using System.Linq;
using InstallationValidator;
using NetGore.Db;
using NetGore.Db.Schema;

namespace CodeReleasePreparer
{
    /// <summary>
    /// Serializes the current schema so the InstallationValidator can use it to check if the user's database (if exists)
    /// matches the schema format of the release's database.
    /// </summary>
    public static class SchemaSaver
    {
        /// <summary>
        /// Saves the schema to file.
        /// </summary>
        /// <param name="path">The file path to save to. If null, the default path will be used.</param>
        public static void Save(string path = null)
        {
            if (string.IsNullOrEmpty(path))
                path = Paths.Root + MySqlHelper.DbSchemaFile;

            // Load the database settings
            var dbSettings = new DbConnectionSettings(Paths.Root + MySqlHelper.DbSettingsFile, true);

            // Get the schema
            var schema = new SchemaReader(dbSettings);

            // Save
            schema.Save(path);
        }
    }
}