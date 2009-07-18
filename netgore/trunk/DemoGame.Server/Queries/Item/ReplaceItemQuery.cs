using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class ReplaceItemQuery : DbQueryNonReader<ItemValues>
    {
        static readonly string _queryString;

        static ReplaceItemQuery()
        {
            _queryString = BuildQueryString(ItemQueryHelper.AllDBFields);
        }

        public ReplaceItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        /// <summary>
        /// Builds the query string
        /// </summary>
        /// <param name="fields">IEnumerable of all the fields</param>
        /// <returns>A string to be used for the query</returns>
        static string BuildQueryString(IEnumerable<string> fields)
        {
            StringBuilder sb = new StringBuilder();

            // Header
            sb.AppendFormat("REPLACE INTO `{0}` (", DBTables.Item);

            // Field names
            foreach (string field in fields)
            {
                sb.Append("`" + field + "`,");
            }
            sb.Remove(sb.Length - 1, 1); // Remove the trailing comma

            // End field names, start values
            sb.Append(") VALUES (");

            // Values
            foreach (string field in fields)
            {
                sb.Append("@" + field + ",");
            }
            sb.Remove(sb.Length - 1, 1); // Remove the trailing comma

            // Close values
            sb.Append(")");

            return sb.ToString();
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(ItemQueryHelper.AllDBFields);
        }

        protected override void SetParameters(DbParameterValues p, ItemValues item)
        {
            p["@id"] = item.ID;
            p["@name"] = item.Name;
            p["@description"] = item.Description;
            p["@type"] = item.Type;
            p["@amount"] = item.Amount;
            p["@graphic"] = item.GraphicIndex;
            p["@value"] = item.Value;
            p["@width"] = item.Width;
            p["@height"] = item.Height;
            p["@hp"] = (int)item.HP;
            p["@mp"] = (int)item.MP;

            foreach (var statField in ItemQueryHelper.BaseDBStatFields)
            {
                string paramName = "@" + statField.Field;
                int statValue = GetStatValue(item.BaseStats, statField.StatType);
                p[paramName] = statValue;
                
            }

            foreach (var statField in ItemQueryHelper.ReqDBStatFields)
            {
                string paramName = "@" + statField.Field;
                int statValue = GetStatValue(item.ReqStats, statField.StatType);
                p[paramName] = statValue;
            }
        }

        static int GetStatValue(IEnumerable<StatTypeValue> e, StatType statType)
        {
            foreach (var pair in e)
            {
                if (pair.StatType == statType)
                    return pair.Value;
            }

            return 0;
        }
    }
}