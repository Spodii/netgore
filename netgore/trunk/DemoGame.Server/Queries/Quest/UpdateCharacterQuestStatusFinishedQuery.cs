using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class UpdateCharacterQuestStatusFinishedQuery : DbQueryNonReader<UpdateCharacterQuestStatusFinishedQuery.QueryArgs>
    {
        static readonly string _queryStr =
            string.Format("UPDATE `{0}` SET `completed_on`=NOW() WHERE `character_id`=@charID AND `quest_id`=@questID",
                          CharacterQuestStatusTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCharacterQuestStatusFinishedQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public UpdateCharacterQuestStatusFinishedQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(CharacterQuestStatusTable.DbColumns, "character_id", "quest_id", "completed_on");
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@charID", "@questID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@charID"] = (int)item.CharacterID;
            p["@questID"] = (int)item.QuestID;
        }

        public struct QueryArgs
        {
            public readonly CharacterID CharacterID;
            public readonly QuestID QuestID;

            public QueryArgs(CharacterID characterID, QuestID questID)
            {
                CharacterID = characterID;
                QuestID = questID;
            }
        }
    }
}