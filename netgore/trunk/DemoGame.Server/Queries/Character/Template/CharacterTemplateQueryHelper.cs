using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public static class CharacterTemplateQueryHelper
    {
        public static IEnumerable<StatTypeField> StatDBFields { get; private set; }
        public static IEnumerable<string> NonStatDBFields { get; private set; }
        public static IEnumerable<string> AllDBFields { get; private set; }

        static CharacterTemplateQueryHelper()
        {
            StatDBFields = StatsQueryHelper.GetStatTypeFields(StatFactory.AllStats, StatCollectionType.Base);

            NonStatDBFields = new string[] { "body", "id", "name", "give_exp", "give_cash", "alliance_id", "respawn", "ai" };

            AllDBFields = StatDBFields.Select(x => x.Field).Concat(NonStatDBFields).ToArray();
        }

        public static SelectCharacterTemplateQueryValues ReadCharacterTemplateValues(IDataReader r)
        {
            // Load the general NPC template values
            CharacterTemplateID templateID = r.GetCharacterTemplateID("id");
            string name = r.GetString("name");
            string ai = r.GetString("ai");
            AllianceID allianceID = r.GetAllianceID("alliance_id");
            BodyIndex bodyIndex = r.GetBodyIndex("body");
            ushort respawn = r.GetUInt16("respawn");
            ushort giveExp = r.GetUInt16("give_exp");
            ushort giveCash = r.GetUInt16("give_cash");

            var stats = StatsQueryHelper.ReadStatValues(r, StatDBFields);

            var ret = new SelectCharacterTemplateQueryValues(templateID, name, bodyIndex, ai, allianceID, respawn, giveExp, giveCash, stats);

            return ret;
        }
    }
}
