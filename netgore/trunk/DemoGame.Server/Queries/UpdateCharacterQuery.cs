using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class UpdateCharacterQuery : DbQueryNonReader<Character>
    {
        static readonly IEnumerable<string> _dbParameterCache = CharacterQueryHelper.AllDBFields.Select(x => "@" + x);
        static readonly string _queryString;

        static UpdateCharacterQuery()
        {
            var dbFields = CharacterQueryHelper.AllDBFields;
            var dbFieldsExceptID = dbFields.Where(x => x != "id");

            Debug.Assert(dbFieldsExceptID.Count() == dbFields.Count() - 1);

            string setString = FormatParametersIntoString(dbFieldsExceptID);
            _queryString = string.Format("UPDATE `{0}` SET {1} WHERE `id`=@id", DBTables.Character, setString);
        }

        public UpdateCharacterQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(_dbParameterCache);
        }

        protected override void SetParameters(DbParameterValues p, Character character)
        {
            p["@id"] = character.ID;
            p["@character_template_id"] = character.TemplateID;
            p["@map_id"] = character.Map.Index;
            p["@x"] = character.Position.X;
            p["@y"] = character.Position.Y;
            p["@body_id"] = character.BodyInfo.Index;
            p["@name"] = character.Name;
            p["@hp"] = (int)character.HP;
            p["@mp"] = (int)character.MP;
            p["@level"] = character.Level;
            p["@exp"] = character.Exp;
            p["@statpoints"] = character.StatPoints;
            p["@respawn_map"] = character.RespawnMapIndex;
            p["@respawn_x"] = character.RespawnPosition.X;
            p["@respawn_y"] = character.RespawnPosition.Y;

            foreach (IStat stat in character.BaseStats)
            {
                string fieldName = stat.StatType.GetDatabaseField(StatCollectionType.Base);
                string key = "@" + fieldName;

                Debug.Assert(p.Contains(key), "If any parameter is missing, something is wrong with the initialization.");
                p[key] = stat.Value;
            }
        }
    }
}