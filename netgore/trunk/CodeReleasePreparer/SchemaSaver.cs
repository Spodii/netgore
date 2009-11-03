using System.IO;
using System.Linq;
using InstallationValidator;
using InstallationValidator.SchemaChecker;
using NetGore;

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
            var dbSettings = new DBConnectionSettings(Paths.Root + MySqlHelper.DBSettingsFile);

            // Get the schema
            var schema = new SchemaReader(dbSettings);

            // Save
            schema.Serialize(Paths.Root + MySqlHelper.DbSchemaFile);
        }
    }
}