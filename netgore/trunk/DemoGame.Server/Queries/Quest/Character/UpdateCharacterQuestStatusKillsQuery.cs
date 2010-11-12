using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Quests;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class UpdateCharacterQuestStatusKillsQuery : DbQueryNonReader<ICharacterQuestStatusKillsTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCharacterQuestStatusKillsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public UpdateCharacterQuestStatusKillsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(CharacterQuestStatusKillsTable.DbColumns, "count");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // INSERT INTO {0} {1}
            //      ON DUPLICATE KEY UPDATE `count`=@count

            var q =
                qb.Insert(CharacterQuestStatusKillsTable.TableName).AddAutoParam(CharacterQuestStatusKillsTable.DbColumns).ODKU().
                    AddAutoParam("count");
            return q.ToString();
        }

        /// <summary>
        /// Executes the query on the database using the specified parameters.
        /// </summary>
        /// <param name="characterID">The character ID.</param>
        /// <param name="questID">The quest ID.</param>
        /// <param name="characterTemplateID">The character template ID.</param>
        /// <param name="count">The count.</param>
        public void Execute(CharacterID characterID, QuestID questID, CharacterTemplateID characterTemplateID, ushort count)
        {
            Execute(new QueryArgs(characterID, questID, characterTemplateID, count));
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(CharacterQuestStatusKillsTable.DbColumns);
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ICharacterQuestStatusKillsTable item)
        {
            item.CopyValues(p);
        }

        /// <summary>
        /// Arguments for the <see cref="UpdateCharacterQuestStatusKillsQuery"/> query.
        /// </summary>
        struct QueryArgs : ICharacterQuestStatusKillsTable
        {
            readonly CharacterID _characterID;
            readonly CharacterTemplateID _characterTemplateID;
            readonly ushort _count;
            readonly QuestID _questID;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="characterID">The character ID.</param>
            /// <param name="questID">The quest ID.</param>
            /// <param name="characterTemplateID">The character template ID.</param>
            /// <param name="count">The count.</param>
            public QueryArgs(CharacterID characterID, QuestID questID, CharacterTemplateID characterTemplateID, ushort count)
            {
                _characterID = characterID;
                _questID = questID;
                _characterTemplateID = characterTemplateID;
                _count = count;
            }

            #region ICharacterQuestStatusKillsTable Members

            /// <summary>
            /// Gets the value of the database column `character_id`.
            /// </summary>
            public CharacterID CharacterID
            {
                get { return _characterID; }
            }

            /// <summary>
            /// Gets the value of the database column `character_template_id`.
            /// </summary>
            public CharacterTemplateID CharacterTemplateID
            {
                get { return _characterTemplateID; }
            }

            /// <summary>
            /// Gets the value of the database column `count`.
            /// </summary>
            public ushort Count
            {
                get { return _count; }
            }

            /// <summary>
            /// Gets the value of the database column `quest_id`.
            /// </summary>
            public QuestID QuestID
            {
                get { return _questID; }
            }

            /// <summary>
            /// Creates a deep copy of this table. All the values will be the same
            /// but they will be contained in a different object instance.
            /// </summary>
            /// <returns>
            /// A deep copy of this table.
            /// </returns>
            public ICharacterQuestStatusKillsTable DeepCopy()
            {
                return new QueryArgs(CharacterID, QuestID, CharacterTemplateID, Count);
            }

            #endregion
        }
    }
}