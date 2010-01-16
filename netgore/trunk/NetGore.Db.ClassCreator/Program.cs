using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DemoGame;
using NetGore.Stats;

namespace NetGore.Db.ClassCreator
{
    class Program
    {
        const string _tempNamespaceName = "[TEMPNAMESPACENAME]";

        /// <summary>
        /// Contains the of the tables that will be exposed to the whole project instead of just the server.
        /// </summary>
        static readonly ICollection<string> _globalTables = new string[] { "map" };

        /// <summary>
        /// Output directory for the generated code that is referenced by the whole project.
        /// Points to the ...\DemoGame\DbObjs\ folder.
        /// </summary>
        static readonly string _outputGameDir = string.Format("{0}..{1}..{1}..{1}..{1}DemoGame{1}DbObjs{1}",
                                                              AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar);

        /// <summary>
        /// Output directory for the generated code that is referenced only by the server.
        /// Points to the ...\DemoGame.Server\DbObjs\ folder.
        /// </summary>
        static readonly string _outputServerDir = string.Format("{0}..{1}..{1}..{1}..{1}DemoGame.Server{1}DbObjs{1}",
                                                                AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar);

        static IEnumerable<ColumnCollectionItem> GetStatColumnCollectionItems(CodeFormatter formatter,
                                                                              StatCollectionType statCollectionType)
        {
            var columnItems = new List<ColumnCollectionItem>();
            foreach (var statType in EnumHelper<StatType>.Values)
            {
                var dbField = statType.GetDatabaseField(statCollectionType);
                var item = ColumnCollectionItem.FromEnum(formatter, dbField, statType);
                columnItems.Add(item);
            }

            return columnItems;
        }

        static void Main()
        {
            Console.WriteLine("This program will generate the code files to match the database." +
                              " As a result, many code files will be altered. Are you sure you wish to continue?");
            Console.WriteLine("Press Y to continue, or any other key to abort.");

            if (Console.ReadKey().Key != ConsoleKey.Y)
                return;

            using (var generator = new MySqlClassGenerator("localhost", "root", "", "demogame"))
            {
                var baseStatColumns = GetStatColumnCollectionItems(generator.Formatter, StatCollectionType.Base);
                var reqStatColumns = GetStatColumnCollectionItems(generator.Formatter, StatCollectionType.Requirement);

                // Custom usings
                generator.AddUsing("NetGore.Db");
                generator.AddUsing("DemoGame.DbObjs");

                // Custom column collections
                var baseStatTables = new string[] { "character", "character_template", "item", "item_template" };
                var reqStatTables = new string[] { "item", "item_template" };

                generator.AddColumnCollection("Stat", typeof(StatType), typeof(int), baseStatTables, baseStatColumns);
                generator.AddColumnCollection("ReqStat", typeof(StatType), typeof(int), reqStatTables, reqStatColumns);

                // Custom external types
                const string accountID = "DemoGame.Server.AccountID";
                const string aiID = "NetGore.AI.AIID";
                const string allianceID = "DemoGame.Server.AllianceID";
                const string characterID = "DemoGame.Server.CharacterID";
                const string characterTemplateID = "DemoGame.Server.CharacterTemplateID";
                const string itemID = "DemoGame.Server.ItemID";
                const string itemTemplateID = "DemoGame.Server.ItemTemplateID";
                const string mapID = "NetGore.MapIndex";
                const string mapSpawnID = "DemoGame.Server.MapSpawnValuesID";
                const string bodyID = "DemoGame.BodyIndex";
                const string equipmentSlot = "DemoGame.EquipmentSlot";
                const string itemChance = "DemoGame.Server.ItemChance";
                const string grhIndex = "NetGore.GrhIndex";
                const string spValueType = "DemoGame.SPValueType";
                const string itemType = "DemoGame.ItemType";
                const string inventorySlot = "DemoGame.InventorySlot";
                const string statusEffectType = "DemoGame.StatusEffectType";
                const string activeStatusEffectID = "DemoGame.Server.ActiveStatusEffectID";
                const string shopID = "DemoGame.Server.ShopID";

                generator.AddCustomType(accountID, "account", "id");

                generator.AddCustomType(allianceID, "alliance", "id");

                generator.AddCustomType(characterID, "character", "id");
                generator.AddCustomType(characterTemplateID, "character", "template_id");

                generator.AddCustomType(equipmentSlot, "character_equipped", "slot");

                generator.AddCustomType(inventorySlot, "character_inventory", "slot");

                generator.AddCustomType(activeStatusEffectID, "character_status_effect", "id");
                generator.AddCustomType(statusEffectType, "character_status_effect", "status_effect_id");

                generator.AddCustomType(characterTemplateID, "character_template", "id");

                generator.AddCustomType(itemChance, "character_template_equipped", "chance");

                generator.AddCustomType(itemChance, "character_template_inventory", "chance");

                generator.AddCustomType(itemID, "item", "id");
                generator.AddCustomType(grhIndex, "item", "graphic");
                generator.AddCustomType(itemType, "item", "type");

                generator.AddCustomType(itemTemplateID, "item_template", "id");
                generator.AddCustomType(grhIndex, "item_template", "graphic");

                generator.AddCustomType(mapID, "map", "id");

                generator.AddCustomType(mapSpawnID, "map_spawn", "id");

                generator.AddCustomType(shopID, "shop", "id");

                // Mass-added custom types
                generator.AddCustomType(allianceID, "*", "alliance_id", "attackable_id", "hostile_id");
                generator.AddCustomType(characterID, "*", "character_id");
                generator.AddCustomType(accountID, "*", "account_id");
                generator.AddCustomType(mapID, "*", "map_id", "respawn_map");
                generator.AddCustomType(itemID, "*", "item_id");
                generator.AddCustomType(characterTemplateID, "*", "character_template_id");
                generator.AddCustomType(itemTemplateID, "*", "item_template_id");
                generator.AddCustomType(bodyID, "*", "body_id");
                generator.AddCustomType(spValueType, "*", "hp", "mp");
                generator.AddCustomType(shopID, "*", "shop_id");
                generator.AddCustomType(aiID, "*", "ai_id");

                // Renaming
                var formatter = generator.Formatter;
                formatter.AddAlias("alliance_id", "AllianceID");
                formatter.AddAlias("shop_id", "ShopID");
                formatter.AddAlias("account_id", "AccountID");
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
                formatter.AddAlias("give_exp", "GiveExp");
                formatter.AddAlias("give_cash", "GiveCash");
                formatter.AddAlias("status_effect_id", "StatusEffect");
                formatter.AddAlias("ai_id", "AIID");

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

                // Generate
                var codeItems = generator.Generate(_tempNamespaceName, _tempNamespaceName);
                foreach (var item in codeItems)
                {
                    SaveCodeFile(item);
                }
            }

            Console.WriteLine("Done");
        }

        static void SaveCodeFile(GeneratedTableCode gtc)
        {
            string saveDir;
            string code;

            var isInterfaceOrClass = (gtc.CodeType == GeneratedCodeType.Interface || gtc.CodeType == GeneratedCodeType.Class);
            var isGlobalTable = _globalTables.Contains(gtc.Table, StringComparer.OrdinalIgnoreCase);

            if ((isInterfaceOrClass && isGlobalTable) || gtc.CodeType == GeneratedCodeType.ColumnMetadata)
            {
                saveDir = _outputGameDir;
                code = gtc.Code.Replace(_tempNamespaceName, "DemoGame.DbObjs");
                code = code.Replace("using NetGore.Db;", string.Empty);
            }
            else
            {
                saveDir = _outputServerDir;
                if (gtc.CodeType == GeneratedCodeType.Interface)
                    saveDir += "Interfaces" + Path.DirectorySeparatorChar;
                else if (gtc.CodeType == GeneratedCodeType.ClassDbExtensions)
                    saveDir += "DbExtensions" + Path.DirectorySeparatorChar;
                code = gtc.Code.Replace(_tempNamespaceName, "DemoGame.Server.DbObjs");
            }

            if (!saveDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                saveDir += Path.DirectorySeparatorChar.ToString();

            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            var savePath = saveDir + gtc.ClassName + ".cs";

            File.WriteAllText(savePath, code);
        }
    }
}