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
            sb.AppendFormat("REPLACE INTO `{0}` (", ItemTable.TableName);

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

        static int GetStatValue(IEnumerable<StatTypeValue> e, StatType statType)
        {
            foreach (StatTypeValue pair in e)
            {
                if (pair.StatType == statType)
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
            return CreateParameters(ItemQueryHelper.AllDBFields);
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
            p["@graphic"] = item.GraphicIndex;
            p["@value"] = item.Value;
            p["@width"] = item.Width;
            p["@height"] = item.Height;
            p["@hp"] = (int)item.HP;
            p["@mp"] = (int)item.MP;

            foreach (StatTypeField statField in ItemQueryHelper.BaseDBStatFields)
            {
                string paramName = "@" + statField.Field;
                int statValue = GetStatValue(item.BaseStats, statField.StatType);
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