using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class ActiveStatusEffectIDCreator : IDCreatorBase
    {
        public ActiveStatusEffectIDCreator(DbConnectionPool connectionPool)
            : base(connectionPool, CharacterStatusEffectTable.TableName, "id", 2048, 128)
        {
            QueryAsserts.ArePrimaryKeys(CharacterStatusEffectTable.DbKeyColumns, "id");
        }
    }
}
