using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterTemplateSkillsQuery : DbQueryReader<CharacterTemplateID>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterTemplateSkillsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterTemplateSkillsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(CharacterTemplateSkillTable.DbColumns, "character_template_id", "skill_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            /*
                SELECT `skill_id`
                    FROM `{0}` WHERE `character_template_id`=@id
            */

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(CharacterTemplateSkillTable.TableName).Add("skill_id").Where(
                    f.Equals(s.EscapeColumn("character_template_id"), s.Parameterize("id")));
            return q.ToString();
        }

        public IEnumerable<SkillType> Execute(CharacterTemplateID charTemplateID)
        {
            var ret = new List<SkillType>();

            using (var r = ExecuteReader(charTemplateID))
            {
                while (r.Read())
                {
                    var skill = (SkillType)r.GetInt32(0);

                    if (!EnumHelper<SkillType>.IsDefined(skill))
                    {
                        // Invalid SkillType - do not return
                        const string errmsg = "Found invalid SkillType `{0}` on character template ID `{1}`.";
                        if (log.IsWarnEnabled)
                            log.WarnFormat(errmsg, skill, charTemplateID);
                    }
                    else
                    {
                        // Valid SkillType - add to return list
                        ret.Add(skill);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterTemplateID item)
        {
            p["id"] = (int)item;
        }
    }
}