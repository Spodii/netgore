using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using DemoGame.Server.DbObjs;
using NetGore.Db;

// TODO: !! Cleanup query

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class ReplaceItemQuery : DbQueryNonReader<ItemValues>
    {
        static readonly string _queryString = string.Format("REPLACE INTO `{0}` {1}", ItemTable.TableName, FormatParametersIntoValuesString(ItemTable.DbColumns));

        public ReplaceItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        static int GetStatValue(IEnumerable<KeyValuePair<StatType, int>> e, StatType statType)
        {
            foreach (var pair in e)
            {
                if (pair.Key == statType)
                    return pair.Value;
            }

            return 0;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(ItemTable.DbColumns.Select(x => "@" + x));
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ItemValues item)
        {
            p["@id"] = (int)item.ID;
            p["@name"] = item.Name;
            p["@description"] = item.Description;
            p["@type"] = item.Type;
            p["@amount"] = item.Amount;
            p["@graphic"] = item.Graphic;
            p["@value"] = item.Value;
            p["@width"] = item.Width;
            p["@height"] = item.Height;
            p["@hp"] = (int)item.HP;
            p["@mp"] = (int)item.MP;

            foreach (StatTypeField statField in ItemQueryHelper.BaseDBStatFields)
            {
                string paramName = "@" + statField.Field;
                int statValue = GetStatValue(item.Stats, statField.StatType);
                p[paramName] = statValue;
            }

            foreach (StatTypeField statField in ItemQueryHelper.ReqDBStatFields)
            {
                string paramName = "@" + statField.Field;
                int statValue = GetStatValue(item.ReqStats, statField.StatType);
                p[paramName] = statValue;
            }
        }
    }
}