using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the names of the different database tables.
    /// </summary>
    public static class DBTables
    {
        public const string Alliance = "alliance";
        public const string AllianceAttackable = "alliance_attackable";
        public const string AllianceHostile = "alliance_hostile";
        public const string Character = "character";
        public const string CharacterEquipped = "character_equipped";
        public const string CharacterInventory = "character_inventory";
        public const string CharacterTemplate = "character_template";
        public const string CharacterTemplateInventory = "character_template_inventory";
        public const string CharacterTemplateEquipped = "character_template_equipped";
        public const string Item = "item";
        public const string ItemTemplate = "item_template";
    }
}
