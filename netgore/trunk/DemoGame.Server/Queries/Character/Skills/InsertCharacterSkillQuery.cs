using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class InsertCharacterSkillQuery : DbQueryNonReader<KeyValuePair<CharacterID, SkillType>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCharacterSkillQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public InsertCharacterSkillQuery(DbConnectionPool connectionPool)
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
                INSERT INTO `character_skill` (`character_id`,`skill_id`) VALUES (@character_id, @skill_id) 
                    ON DUPLICATE KEY UPDATE `character_id`=`character_id`
            */

            var s = qb.Settings;
            var q = qb.Insert(CharacterSkillTable.TableName).AddAutoParam("character_id", "skill_id").ODKU().Add("character_id", "character_id");

            return q.ToString();
        }

        public void Execute(CharacterID characterID, SkillType skillType)
        {
            Execute(new KeyValuePair<CharacterID, SkillType>(characterID, skillType));
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