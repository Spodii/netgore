using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DemoGame;

namespace NetGore.Db.ClassCreator
{
    class Program
    {
        static IEnumerable<ColumnCollectionItem> GetStatColumnCollectionItems(StatCollectionType statCollectionType)
        {
            List<ColumnCollectionItem> columnItems = new List<ColumnCollectionItem>();
            foreach (var statType in Enum.GetValues(typeof(StatType)).Cast<StatType>())
            {
                var dbField = statType.GetDatabaseField(statCollectionType);
                var item = ColumnCollectionItem.FromEnum(dbField, statType);
                columnItems.Add(item);
            }

            return columnItems;
        }

        static void Main(string[] args)
        {
            // Output directory for the generated code - points to the ...\DemoGame.ServerObjs\DbObjs\ folder
            string outputDir = string.Format("{0}..{1}..{1}..{1}..{1}DemoGame.ServerObjs{1}DbObjs{1}",
                                             AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar);

            var baseStatColumns = GetStatColumnCollectionItems(StatCollectionType.Base);
            var reqStatColumns = GetStatColumnCollectionItems(StatCollectionType.Requirement);

            using (MySqlClassGenerator generator = new MySqlClassGenerator("localhost", "root", "", "demogame"))
            {
                // Custom usings
                generator.AddUsing("NetGore.Db");

                // Custom DataReader methods
                generator.SetDataReaderReadMethod(typeof(float), "GetFloat");

                // Custom column collections
                string[] baseStatTables = new string[] { "character", "character_template", "item", "item_template" };
                string[] reqStatTables = new string[] { "item", "item_template" };

                generator.AddColumnCollection("Stat", typeof(StatType), typeof(int), baseStatTables, baseStatColumns);
                generator.AddColumnCollection("ReqStat", typeof(StatType), typeof(int), reqStatTables, reqStatColumns);

                // Generate
                generator.Generate("DemoGame.Server.DbObjs", outputDir);
            }
        }
    }
}