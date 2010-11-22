using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class DeleteCharacterSkillQuery : DbQueryNonReader<KeyValuePair<CharacterID, SkillType>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCharacterSkillQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public DeleteCharacterSkillQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(CharacterSkillTable.DbColumns, "character_id", "skill_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            /*
                DELETE FROM `character_skill`
                    WHERE `character_id` = @character_id AND `skill_id` = @skill_id;
            */

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Delete(CharacterSkillTable.TableName).Where(
                    f.And(f.Equals(s.EscapeColumn("character_id"), s.Parameterize("character_id")),
                        f.Equals(s.EscapeColumn("skill_id"), s.Parameterize("skill_id"))));

            return q.ToString();
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>
        /// The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.
        /// </returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("character_id", "skill_id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, KeyValuePair<CharacterID, SkillType> item)
        {
            p["character_id"] = (int)item.Key;
            p["skill_id"] = (int)item.Value;
        }
    }
}