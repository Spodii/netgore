using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;
using Platyform.Extensions;

namespace DemoGame.Server
{
    public class ReplaceItemQuery : NonReaderQueryBase<ItemValues>
    {
        public const string ItemsTableName = "items";

        /// <summary>
        /// CommandText used for the ReplaceItemQuery. Cached statically since every ReplaceItemQuery will
        /// be using the same query string. The string itself is generated on the constructor of the first
        /// ReplaceItemQuery created.
        /// </summary>
        static string _queryString = null;

        readonly MySqlParameter _amount = new MySqlParameter("@amount", null);
        readonly MySqlParameter _description = new MySqlParameter("@description", null);
        readonly MySqlParameter _graphic = new MySqlParameter("@graphic", null);
        readonly MySqlParameter _guid = new MySqlParameter("@guid", null);
        readonly MySqlParameter _height = new MySqlParameter("@height", null);
        readonly MySqlParameter _name = new MySqlParameter("@name", null);
        readonly Dictionary<StatType, MySqlParameter> _statParams = new Dictionary<StatType, MySqlParameter>();
        readonly MySqlParameter _type = new MySqlParameter("@type", null);
        readonly MySqlParameter _value = new MySqlParameter("@value", null);
        readonly MySqlParameter _width = new MySqlParameter("@width", null);

        public ReplaceItemQuery(MySqlConnection conn) : base(conn)
        {
            // Add the stats to the dictionary
            foreach (StatType statType in ItemStats.DatabaseStats)
            {
                string fieldName = statType.GetDatabaseField();
                _statParams.Add(statType, new MySqlParameter("@" + fieldName, null));
            }

            // Create an IEnumerable for all of the MySqlParameters
            var itemVars = new MySqlParameter[] { _guid, _width, _height, _name, _description, _amount, _graphic, _value, _type };
            Dictionary<StatType, MySqlParameter>.ValueCollection itemStats = _statParams.Values;
            var itemParams = itemVars.Concat(itemStats);

            // Add all the parameters
            AddParameters(itemParams);

            // Create the query string, if needed
            if (_queryString == null)
            {
                _queryString = BuildQueryString(from param in itemParams select GetFieldName(param));

#if DEBUG
                Debug.Assert((from param in itemParams select param.ParameterName).All(name => name.StartsWith("@")),
                             "Somehow one or more parameters do not start with @ while the code below relies on the assumption they do.");
                Debug.Assert(IsDBSynchronized(conn, itemParams), "Database not synchronized.");
#endif
            }

            // Initialize the query
            Initialize(_queryString);
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

#if DEBUG
        /// <summary>
        /// Checks that the database is properly synchronized with the code
        /// </summary>
        /// <param name="conn">MySqlConnection to connect with</param>
        /// <param name="itemParams">IEnumerable of MySqlParameters</param>
        /// <returns>True if no errors found, false if errors found</returns>
        static bool IsDBSynchronized(MySqlConnection conn, IEnumerable<MySqlParameter> itemParams)
        {
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT * FROM `{0}` WHERE 0=1", ItemsTableName);
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    r.Read();

                    foreach (MySqlParameter param in itemParams)
                    {
                        // Ensure the field exists
                        string fieldName = GetFieldName(param);
                        r.GetOrdinal(fieldName);
                    }
                }
            }

            return true;
        }
#endif

        /// <summary>
        /// Sets the parameters
        /// </summary>
        /// <param name="itemData"></param>
        protected override void SetParameters(ItemValues itemData)
        {
            _guid.Value = itemData.Guid;
            _name.Value = itemData.Name;
            _description.Value = itemData.Description;
            _type.Value = (byte)itemData.Type;
            _amount.Value = itemData.Amount;
            _graphic.Value = itemData.GraphicIndex;
            _value.Value = itemData.Value;
            _width.Value = itemData.Width;
            _height.Value = itemData.Height;

            foreach (StatType statType in ItemStats.DatabaseStats)
            {
                IStat stat = itemData.Stats.GetStat(statType);
                _statParams[stat.StatType].Value = stat.Value;
            }
        }
    }
}