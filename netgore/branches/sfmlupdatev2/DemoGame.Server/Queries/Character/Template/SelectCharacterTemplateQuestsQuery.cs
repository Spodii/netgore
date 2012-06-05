using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Quests;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterTemplateQuestsQuery : DbQueryReader<CharacterTemplateID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterTemplateQuestsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectCharacterTemplateQuestsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT `quest_id` FROM `{0}` WHERE `character_template_id`=@id

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(CharacterTemplateQuestProviderTable.TableName).Add("quest_id").Where(
                    f.Equals(s.EscapeColumn("character_template_id"), s.Parameterize("id")));
            return q.ToString();
        }

        public IEnumerable<QuestID> Execute(CharacterTemplateID id)
        {
            var ret = new List<QuestID>();

            using (var r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    ret.Add(r.GetQuestID(0));
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
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