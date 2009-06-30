using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;

using NetGore.Db;

namespace DemoGame.Server
{
    public class ReplaceItemQuery : DbQueryNonReader<ItemValues>
    {
        public const string ItemsTableName = "items";

        static readonly IEnumerable<string> _otherFields = new string[]
                                                           {
                                                               "amount", "description", "graphic", "guid", "height", "name", "type",
                                                               "value", "width"
                                                           };

        static readonly string _queryString;

        static readonly IEnumerable<string> _statFields = ItemStats.DatabaseStats.Select(statType => statType.GetDatabaseField());

        static ReplaceItemQuery()
        {
            _queryString = BuildQueryString(GetFields());
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
            StringBuilder sb = new StringBuilder(256 + ItemStats.DatabaseStats.Count() * 8);

            // Header
            sb.AppendFormat("REPLACE INTO `{0}` (", ItemsTableName);

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

        static IEnumerable<string> GetFields()
        {
            return _statFields.Concat(_otherFields);
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(GetFields());
        }

        protected override void SetParameters(DbParameterValues p, ItemValues item)
        {
            p["@guid"] = item.Guid;
            p["@name"] = item.Name;
            p["@description"] = item.Description;
            p["@type"] = item.Type;
            p["@amount"] = item.Amount;
            p["@graphic"] = item.GraphicIndex;
            p["@value"] = item.Value;
            p["@width"] = item.Width;
            p["@height"] = item.Height;

            foreach (StatType statType in ItemStats.DatabaseStats)
            {
                string paramName = "@" + statType.GetDatabaseField();
                IStat stat = item.Stats.GetStat(statType);
                p[paramName] = stat.Value;
            }
        }
    }
}