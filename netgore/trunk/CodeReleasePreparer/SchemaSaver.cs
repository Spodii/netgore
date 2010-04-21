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
        public static void Save()
        {
            // Load the database settings
            var dbSettings = new DbConnectionSettings(Paths.Root + MySqlHelper.DbSettingsFile, true);

            // Get the schema
            var schema = new SchemaReader(dbSettings);

            // Save
            schema.Save(Paths.Root + MySqlHelper.DbSchemaFile);
        }
    }
}