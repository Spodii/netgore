using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DemoGame;

namespace NetGore.Db.ClassCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputDir = string.Format("{0}..{1}..{1}..{1}..{1}DemoGame.ServerObjs{1}DbObjs{1}",
                                             AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar);

            List<ColumnCollectionItem> columnItems = new List<ColumnCollectionItem>();
            foreach (var statType in Enum.GetValues(typeof(StatType)).Cast<StatType>())
            {
                var dbField = statType.GetDatabaseField(StatCollectionType.Base);
                var item = ColumnCollectionItem.FromEnum(dbField, statType);
                columnItems.Add(item);
            }

            using (MySqlClassGenerator generator = new MySqlClassGenerator("localhost", "root", "", "demogame"))
            {
                generator.AddUsing("NetGore.Db");
                generator.SetDataReaderReadMethod(typeof(float), "GetFloat");
                generator.AddColumnCollection("Stats", typeof(StatType), typeof(int), "character", columnItems);

                generator.Generate("DemoGame.Server.DbObjs", outputDir);
            }
        }
    }
}