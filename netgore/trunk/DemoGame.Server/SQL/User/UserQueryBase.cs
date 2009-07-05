using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public abstract class UserQueryBase : DbQueryNonReader<Character>
    {
        static readonly IEnumerable<string> _otherFields = new string[] { "@body", "@id", "@map", "@name", "@x", "@y" };

        static readonly IEnumerable<string> _statFields =
            UserStats.DatabaseStats.Select(statType => "@" + statType.GetDatabaseField());

        static string _queryFieldsStr;

        /// <summary>
        /// Gets an IEnumerable of the name of each field, except for the ID, without the parameter prefix.
        /// </summary>
        protected static IEnumerable<string> FieldNames
        {
            get { return GetAllFields().Where(x => x != "@id").Select(x => GetParameterNameWithoutPrefix(x)); }
        }

        /// <summary>
        /// Gets the static cache of the GetValuesQuery(FieldNames).
        /// </summary>
        protected static string QueryFieldsStr
        {
            get
            {
                if (_queryFieldsStr == null)
                    _queryFieldsStr = FormatParametersIntoString(FieldNames);

                return _queryFieldsStr;
            }
        }

        protected UserQueryBase(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        static IEnumerable<string> GetAllFields()
        {
            return _statFields.Concat(_otherFields);
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(GetAllFields());
        }

        protected override void SetParameters(DbParameterValues p, Character character)
        {
            p["@id"] = character.ID;
            p["@map"] = character.Map.Index;
            p["@x"] = character.Position.X;
            p["@y"] = character.Position.Y;
            p["@body"] = character.BodyInfo.Index;
            p["@name"] = character.Name;

            foreach (IStat stat in character.Stats)
            {
                string fieldName = stat.StatType.GetDatabaseField();
                string key = "@" + fieldName;
                if (!p.Contains(key))
                    continue;

                p[key] = stat.Value;
            }
        }
    }
}