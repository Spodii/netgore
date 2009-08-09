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
            var columnItems = new List<ColumnCollectionItem>();
            foreach (StatType statType in Enum.GetValues(typeof(StatType)).Cast<StatType>())
            {
                string dbField = statType.GetDatabaseField(statCollectionType);
                ColumnCollectionItem item = ColumnCollectionItem.FromEnum(dbField, statType);
                columnItems.Add(item);
            }

            return columnItems;
        }

        static void Main(string[] args)
        {
            // Output directory for the generated class code - points to the ...\DemoGame.ServerObjs\DbObjs\ folder
            string outputClassDir = string.Format("{0}..{1}..{1}..{1}..{1}DemoGame.ServerObjs{1}DbObjs{1}",
                                             AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar);


            // Output directory for the generated interface code - points to the ...\DemoGame\DbObjs\ folder
            string outputInterfaceDir = string.Format("{0}..{1}..{1}..{1}..{1}DemoGame{1}DbObjs{1}",
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
                var baseStatTables = new string[] { "character", "character_template", "item", "item_template" };
                var reqStatTables = new string[] { "item", "item_template" };

                generator.AddColumnCollection("Stat", typeof(StatType), typeof(int), baseStatTables, baseStatColumns);
                generator.AddColumnCollection("ReqStat", typeof(StatType), typeof(int), reqStatTables, reqStatColumns);

                // Custom external types
                const string allianceID = "DemoGame.Server.AllianceID";
                const string characterID = "DemoGame.Server.CharacterID";
                const string characterTemplateID = "DemoGame.Server.CharacterTemplateID";
                const string mapID = "NetGore.MapIndex";
                const string itemID = "DemoGame.Server.ItemID";
                const string itemTemplateID = "DemoGame.Server.ItemTemplateID";
                const string mapSpawnID = "DemoGame.Server.MapSpawnValuesID";
                const string bodyID = "DemoGame.BodyIndex";
                const string equipmentSlot = "DemoGame.EquipmentSlot";
                const string itemChance = "DemoGame.Server.ItemChance";

                generator.AddCustomType(allianceID, "alliance", "id");

                generator.AddCustomType(characterID, "character", "id");
                generator.AddCustomType(characterTemplateID, "character", "template_id");

                generator.AddCustomType(equipmentSlot, "character_equipped", "slot");

                generator.AddCustomType(itemChance, "character_template_equipped", "chance");

                generator.AddCustomType(itemChance, "character_template_inventory", "chance");

                generator.AddCustomType(itemID, "item", "id");

                generator.AddCustomType(itemTemplateID, "item_template", "id");

                generator.AddCustomType(mapID, "map", "id");

                generator.AddCustomType(mapSpawnID, "map_spawn", "id");

                // Mass-added custom types
                generator.AddCustomType(allianceID, "*", "alliance_id", "attackable_id", "hostile_id");
                generator.AddCustomType(characterID, "*", "character_id");
                generator.AddCustomType(mapID, "*", "map_id", "respawn_map");
                generator.AddCustomType(itemID, "*", "item_id");
                generator.AddCustomType(characterTemplateID, "*", "character_template_id");
                generator.AddCustomType(itemTemplateID, "*", "item_template_id");
                generator.AddCustomType(bodyID, "*", "body_id");

                // Renaming
                var formatter = generator.Formatter;
                formatter.AddAlias("alliance_id", "AllianceID");
                formatter.AddAlias("attackable_id", "AttackableID");
                formatter.AddAlias("hostile_id", "HostileID");
                formatter.AddAlias("character_id", "CharacterID");
                formatter.AddAlias("character_template_id", "CharacterTemplateID");
                formatter.AddAlias("item_template_id", "ItemTemplateID");
                formatter.AddAlias("item_id", "ItemID");
                formatter.AddAlias("map_id", "MapID");
                formatter.AddAlias("body_id", "BodyID");
                formatter.AddAlias("respawn_map", "RespawnMap");
                formatter.AddAlias("respawn_x", "RespawnX");
                formatter.AddAlias("respawn_y", "RespawnY");
                formatter.AddAlias("respawn_y", "RespawnY");
                formatter.AddAlias("give_exp", "GiveExp");
                formatter.AddAlias("give_cash", "GiveCash");

                formatter.AddAlias("Name");
                formatter.AddAlias("ID");
                formatter.AddAlias("AI");
                formatter.AddAlias("StatPoints");
                formatter.AddAlias("HP");
                formatter.AddAlias("MP");
                formatter.AddAlias("HP");
                formatter.AddAlias("MaxHP");
                formatter.AddAlias("MaxMP");
                formatter.AddAlias("MinHit");
                formatter.AddAlias("MaxHit");
                formatter.AddAlias("WS");

                // Generate
                generator.Generate("DemoGame.Server.DbObjs", "DemoGame.DbObjs", outputClassDir, outputInterfaceDir);
            }
        }
    }
}