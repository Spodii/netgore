using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class UpdateCharacterQuery : DbQueryNonReader<Character>
    {
        static readonly string _queryString = string.Format("UPDATE `{0}` SET {1} WHERE `id`=@id", CharacterTable.TableName,
                                                            FormatParametersIntoString(
                                                                CharacterTable.DbNonKeyColumns.Except(FieldsToNotUpdate,
                                                                                                      StringComparer.
                                                                                                          OrdinalIgnoreCase)));

        /// <summary>
        /// Gets the fields that will not be updated when the Character is updated.
        /// </summary>
        static IEnumerable<string> FieldsToNotUpdate
        {
            get { yield return "password"; }
        }

        public UpdateCharacterQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(CharacterTable.DbColumns.Select(x => "@" + x));
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="character">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, Character character)
        {
            p["@id"] = character.ID;
            p["@cash"] = character.Cash;
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