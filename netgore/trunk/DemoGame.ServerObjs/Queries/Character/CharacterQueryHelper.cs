using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public static class CharacterQueryHelper
    {
        public static IEnumerable<StatTypeField> StatDBFields { get; private set; }
        public static IEnumerable<string> NonStatDBFields { get; private set; }
        public static IEnumerable<string> AllDBFields { get; private set; }

        static CharacterQueryHelper()
        {
            StatDBFields = StatsQueryHelper.GetStatTypeFields(StatFactory.AllStats, StatCollectionType.Base);

            NonStatDBFields = new string[] { "body", "id", "template_id", "map", "name", "x", "y", "hp", "mp", "level",
                "exp", "statpoints", "respawn_map", "respawn_x", "respawn_y" }.ToArray();

            AllDBFields = StatDBFields.Select(x => x.Field).Concat(NonStatDBFields).ToArray();
        }

        public static SelectCharacterQueryValues ReadCharacterQueryValues(IDataReader r)
        {
            // Read the general user values
            CharacterID id = r.GetCharacterID("id");
            CharacterTemplateID? templateID = r.GetCharacterTemplateIDNullable("template_id");
            string name = r.GetString("name");
            MapIndex mapIndex = r.GetMapIndex("map");
            float x = r.GetFloat("x");
            float y = r.GetFloat("y");
            BodyIndex body = r.GetBodyIndex("body");
            byte level = r.GetByte("level");
            uint exp = r.GetUInt32("exp");
            uint statpoints = r.GetUInt32("statpoints");
            uint cash = r.GetUInt32("cash");
            SPValueType hp = r.GetSPValueType("hp");
            SPValueType mp = r.GetSPValueType("mp");

            MapIndex? respawnMap = r.GetMapIndexNullable("respawn_map");
            float respawnX = r.GetFloat("respawn_x");
            float respawnY = r.GetFloat("respawn_y");

            // Read the user's stats
            var stats = StatsQueryHelper.ReadStatValues(r, StatDBFields);

            // Create the return object
            Vector2 pos = new Vector2(x, y);
            Vector2 respawnPos = new Vector2(respawnX, respawnY);
            var ret = new SelectCharacterQueryValues(id, templateID, name, mapIndex, pos, body, level, exp, statpoints, cash, hp, mp, respawnMap, respawnPos, stats);
            return ret;
        }
    }
}
