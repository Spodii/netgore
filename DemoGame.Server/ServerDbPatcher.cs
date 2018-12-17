using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.Server.Queries;
using DemoGame.Server.UI;
using NetGore.Db;
using log4net;
using NetGore.IO;

namespace DemoGame.Server
{
    public static class ServerDbPatcher
    {
        /// <summary>
        /// Finds all patches that have not been applied yet.
        /// </summary>
        public static Tuple<string, string, DateTime>[] GetMissingPatches()
        {
            var db = Server.CreateDbController();

            // Ensure patches table exists
            const string createTableSQL = @"
CREATE TABLE IF NOT EXISTS `applied_patches` (
	`file_name`  varchar(255) NOT NULL,
	`date_applied` datetime NOT NULL,
	PRIMARY KEY (`file_name`)
);";

            db.ExecuteNonQuery(createTableSQL);

            // Get all existing db patch scripts
            const string getExecutedPatchesSQL = "SELECT `file_name` FROM `applied_patches`";

            HashSet<string> executedPatchFiles = new HashSet<string>(db.ExecuteQuery<string>(getExecutedPatchesSQL, r => r.GetString(0)));

            // Find the db patch scripts
            DirectoryInfo dbPatchesDir = PathHelper.FindParentDirectoryWhere(Application.ExecutablePath,
                x => x.GetDirectories().Any(y => StringComparer.OrdinalIgnoreCase.Equals(y.Name, "DbPatches")))
                .EnumerateDirectories().SingleOrDefault(x => StringComparer.OrdinalIgnoreCase.Equals(x.Name, "DbPatches"));

            if (dbPatchesDir == null)
                throw new Exception("Failed to find DbPatches directory. Cannot apply patches.");

            FileInfo[] allPatchFiles = dbPatchesDir.GetFiles("*.sql", SearchOption.AllDirectories);

            // Parse out the relative file path & date for each file
            var patchFiles = allPatchFiles.Select(x =>
                new Tuple<string, string, DateTime>(
                    x.FullName,
                    x.Name,
                    DateTime.ParseExact(x.Name.Substring(0, x.Name.IndexOf(' ')), "yyyy-mm-dd", CultureInfo.InvariantCulture))
                );

            // Take only the patches not yet applied
            patchFiles = patchFiles.Where(x => !executedPatchFiles.Contains(x.Item2));

            // Sort
            patchFiles = patchFiles.OrderBy(x => x.Item3).ThenBy(x => x.Item2);

            return patchFiles.ToArray();
        }

        /// <summary>
        /// Applies all database patches that have not been applied yet.
        /// </summary>
        public static void ApplyPatches()
        {
            var db = Server.CreateDbController();

            var missingPatches = GetMissingPatches();

            Console.WriteLine("Total patches to apply: " + missingPatches.Length);

            // Execute any missing patch files, in order by date then name
            int counter = 0;
            foreach (var patchFileInfo in missingPatches)
            {
                Console.WriteLine("Applying patch {0}/{1}: {2}", ++counter, missingPatches.Length, patchFileInfo.Item2);

                // Get the sql
                string patchFileContentsSQL = File.ReadAllText(patchFileInfo.Item1);
                string insertRecordSQL = string.Format("INSERT INTO `applied_patches` SET `file_name` = '{0}', `date_applied` = NOW()", patchFileInfo.Item2);

                // Execute in transaction
                db.ExecuteNonQueries(patchFileContentsSQL, insertRecordSQL);
            }

            Console.WriteLine("Done");
        }
    }
}
