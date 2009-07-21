using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DemoGame.Server.Queries
{
    public static class StatsQueryHelper
    {
        public static IEnumerable<StatTypeField> GetStatTypeFields(string table, StatCollectionType statCollectionType)
        {
            var statTypes = DBTableStatTypes.GetTableStatTypes(table, statCollectionType);
            var statFields = statTypes.Select(x => new StatTypeField(x, x.GetDatabaseField(statCollectionType)));
            statFields = statFields.ToArray();
            return statFields;
        }

        public static IEnumerable<StatTypeField> GetStatTypeFields(IEnumerable<StatType> statTypes,
                                                                   StatCollectionType statCollectionType)
        {
            var statFields = statTypes.Select(x => new StatTypeField(x, x.GetDatabaseField(statCollectionType)));
            statFields = statFields.ToArray();
            return statFields;
        }

        public static IEnumerable<StatTypeValue> ReadStatValues(IDataReader r, IEnumerable<StatTypeField> dbStatFields)
        {
            var stats = new List<StatTypeValue>();
            foreach (StatTypeField statField in dbStatFields)
            {
                int ordinal = r.GetOrdinal(statField.Field);
                int statValue = r.GetInt32(ordinal);
                stats.Add(new StatTypeValue(statField.StatType, statValue));
            }

            return stats;
        }
    }
}