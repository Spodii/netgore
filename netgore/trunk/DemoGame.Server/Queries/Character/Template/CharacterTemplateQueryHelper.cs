using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public static class CharacterTemplateQueryHelper
    {
        public static IEnumerable<string> AllDBFields { get; private set; }
        public static IEnumerable<string> NonStatDBFields { get; private set; }
        public static IEnumerable<StatTypeField> StatDBFields { get; private set; }

        static CharacterTemplateQueryHelper()
        {
            StatDBFields = StatsQueryHelper.GetStatTypeFields(StatFactory.AllStats, StatCollectionType.Base);

            NonStatDBFields = new string[] { "body", "id", "name", "give_exp", "give_cash", "alliance_id", "respawn", "ai", "statpoints", "exp", "level" };

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
            uint exp = r.GetUInt32("exp");
            uint statPoints = r.GetUInt32("statpoints");
            byte level = r.GetByte("level");

            var stats = StatsQueryHelper.ReadStatValues(r, StatDBFields);

            SelectCharacterTemplateQueryValues ret = new SelectCharacterTemplateQueryValues(templateID, name, bodyIndex, ai,
                                                                                            allianceID, respawn, giveExp, giveCash,
                                                                                            exp, statPoints, level, stats);

            return ret;
        }
    }
}