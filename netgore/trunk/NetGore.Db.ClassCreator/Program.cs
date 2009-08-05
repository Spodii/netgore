using System;
using System.IO;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            CSharpCodeFormatter formatter = new CSharpCodeFormatter();

            using (MySqlClassGenerator generator = new MySqlClassGenerator("localhost", "root", "", "demogame"))
            {
                string outputDir = string.Format("{0}..{1}..{1}..{1}..{1}DemoGame.ServerObjs{1}DbObjs{1}",
                                                 AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar);
                generator.Generate(formatter, "DemoGame.Server.DbObjs", outputDir);
            }
        }
    }
}