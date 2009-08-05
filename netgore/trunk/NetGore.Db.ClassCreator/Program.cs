using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetGore.Db.ClassCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            CSharpCodeFormatter formatter = new CSharpCodeFormatter();

            using (MySqlClassGenerator generator = new MySqlClassGenerator("localhost", "root", "", "demogame"))
            {
                string outputDir = AppDomain.CurrentDomain.BaseDirectory + "TestOutput";
                generator.Generate(formatter, "DemoGame.Server", outputDir);
            }
        }
    }
}
