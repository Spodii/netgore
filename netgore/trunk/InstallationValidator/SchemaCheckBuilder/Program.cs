using System;
using System.Linq;
using InstallationValidator;
using NetGore;

namespace SchemaCheckBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            DBConnectionSettings dbs = new DBConnectionSettings("..\\..\\..\\..\\" + MySqlHelper.DBSettingsFile);
            var schema = new SchemaReader(dbs);

            var dbs2 = new DBConnectionSettings(dbs.User, dbs.Pass, "demogame", dbs.Host);
            var schema2 = new SchemaReader(dbs2);

            var comp = SchemaComparer.Compare(schema, schema2);

            foreach (var line in comp)
                Console.WriteLine(line);

            Console.ReadLine();
        }
    }
}