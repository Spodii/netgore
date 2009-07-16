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
        protected UserQueryBase(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        static readonly IEnumerable<string> _dbParameterCache = CharacterQueryHelper.AllDBFields.Select(x => "@" + x);

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(_dbParameterCache);
        }

        protected override void SetParameters(DbParameterValues p, Character character)
        {
            p["@id"] = character.ID;
            p["@template_id"] = character.TemplateID;
            p["@map"] = character.Map.Index;
            p["@x"] = character.Position.X;
            p["@y"] = character.Position.Y;
            p["@body"] = character.BodyInfo.Index;
            p["@name"] = character.Name;

            foreach (var stat in character.BaseStats)
            {
                string fieldName = stat.StatType.GetDatabaseField(StatCollectionType.Base);
                string key = "@" + fieldName;

                Debug.Assert(p.Contains(key), "If any parameter is missing, something is wrong with the initialization.");
                p[key] = stat.Value;
            }
        }
    }
}