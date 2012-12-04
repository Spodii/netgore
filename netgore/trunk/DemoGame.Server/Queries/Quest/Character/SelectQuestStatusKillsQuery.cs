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
    public class SelectQuestStatusKillsQuery : DbQueryReader<CharacterID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQuestStatusKillsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectQuestStatusKillsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(CharacterQuestStatusKillsTable.DbColumns, "character_template_id", "count", "character_id", "quest_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT `quest_id`,`character_template_id`,`count` FROM `{0}` WHERE `character_id`=@charID

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(CharacterQuestStatusKillsTable.TableName).Add("quest_id", "character_template_id", "count").Where(
                    f.Equals(s.EscapeColumn("character_id"), s.Parameterize("charID")));
            return q.ToString();
        }

        public IEnumerable<KeyValuePair<QuestID, List<KeyValuePair<CharacterTemplateID, ushort>>>> Execute(CharacterID charID)
        {
            var ret = new Dictionary<QuestID, List<KeyValuePair<CharacterTemplateID, ushort>>>();

            using (var r = ExecuteReader(charID))
            {
                while (r.Read())
                {
                    var questID = r.GetQuestID("quest_id");
                    var charTemplateID = r.GetCharacterTemplateID("character_template_id");
                    var count = r.GetUInt16("count");
                    var value = new KeyValuePair<CharacterTemplateID, ushort>(charTemplateID, count);

                    List<KeyValuePair<CharacterTemplateID, ushort>> list;
                    if (!ret.TryGetValue(questID, out list))
                    {
                        list = new List<KeyValuePair<CharacterTemplateID, ushort>>();
                        ret.Add(questID, list);
                    }

                    list.Add(value);
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
            return CreateParameters("charID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID item)
        {
            p["charID"] = (int)item;
        }
    }
}