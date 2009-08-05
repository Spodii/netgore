using System;
using System.IO;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputDir = string.Format("{0}..{1}..{1}..{1}..{1}DemoGame.ServerObjs{1}DbObjs{1}",
                                             AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar);

            using (MySqlClassGenerator generator = new MySqlClassGenerator("localhost", "root", "", "demogame"))
            {
                generator.AddUsing("NetGore.Db");
                generator.SetDataReaderReadMethod(typeof(float), "GetFloat");

                generator.Generate("DemoGame.Server.DbObjs", outputDir);
            }
        }
    }
}