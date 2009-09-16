using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class ActiveStatusEffectIDCreator : IDCreatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveStatusEffectIDCreator"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public ActiveStatusEffectIDCreator(DbConnectionPool connectionPool)
            : base(connectionPool, CharacterStatusEffectTable.TableName, "id", 2048, 128)
        {
            QueryAsserts.ArePrimaryKeys(CharacterStatusEffectTable.DbKeyColumns, "id");
        }
    }
}