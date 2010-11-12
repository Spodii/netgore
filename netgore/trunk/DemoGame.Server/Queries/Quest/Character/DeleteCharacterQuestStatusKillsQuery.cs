using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Quests;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class DeleteCharacterQuestStatusKillsQuery : DbQueryNonReader<DeleteCharacterQuestStatusKillsQuery.QueryArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCharacterQuestStatusKillsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public DeleteCharacterQuestStatusKillsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(CharacterQuestStatusKillsTable.DbColumns, "character_id", "quest_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // DELETE FROM `{0}` WHERE `character_id`=@charID AND `quest_id`=@questID

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Delete(CharacterQuestStatusKillsTable.TableName).Where(
                    f.And(f.Equals(s.EscapeColumn("character_id"), s.Parameterize("charID")),
                        f.Equals(s.EscapeColumn("quest_id"), s.Parameterize("questID"))));
            return q.ToString();
        }

        /// <summary>
        /// Executes the query on the database using the specified parameters.
        /// </summary>
        /// <param name="characterID">The character ID.</param>
        /// <param name="questID">The quest ID.</param>
        public void Execute(CharacterID characterID, QuestID questID)
        {
            Execute(new QueryArgs(characterID, questID));
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("charID", "questID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["charID"] = (int)item.CharacterID;
            p["questID"] = (int)item.QuestID;
        }

        /// <summary>
        /// Arguments for the <see cref="DeleteCharacterQuestStatusKillsQuery"/> query.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public struct QueryArgs
        {
            /// <summary>
            /// The character ID.
            /// </summary>
            public readonly CharacterID CharacterID;

            /// <summary>
            /// The quest ID.
            /// </summary>
            public readonly QuestID QuestID;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="characterID">The character ID.</param>
            /// <param name="questID">The quest ID.</param>
            public QueryArgs(CharacterID characterID, QuestID questID)
            {
                CharacterID = characterID;
                QuestID = questID;
            }
        }
    }
}