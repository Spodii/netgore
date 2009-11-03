using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using InstallationValidator;
using NetGore;

namespace SchemaCheckBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            DBConnectionSettings dbs = new DBConnectionSettings("..\\..\\..\\..\\" + MySqlHelper.DBSettingsFile);
            var x = new SchemaReader(dbs);

            x.Serialize("test.bin");

            var schema = SchemaReader.Deserialize("test.bin");

            var dbs2 = new DBConnectionSettings(dbs.User, dbs.Pass, "dg2", dbs.Host);
            var schema2 = new SchemaReader(dbs2);

            var comp = SchemaComparer.Compare(schema, schema2);

            foreach (var line in comp)
                Console.WriteLine(line);

            Console.ReadLine();
        }
    }
}