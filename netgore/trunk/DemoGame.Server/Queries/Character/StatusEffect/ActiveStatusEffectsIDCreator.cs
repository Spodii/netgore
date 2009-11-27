using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class ActiveStatusEffectIDCreator : IDCreatorBase<ActiveStatusEffectID>
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

        /// <summary>
        /// When overridden in the derived class, converts the given int to type <see cref="ActiveStatusEffectID"/>.
        /// </summary>
        /// <param name="value">The value to convert to type <see cref="ActiveStatusEffectID"/>.</param>
        /// <returns>The <paramref name="value"/> as type <see cref="ActiveStatusEffectID"/>.</returns>
        protected override ActiveStatusEffectID FromInt(int value)
        {
            return new ActiveStatusEffectID(value);
        }

        /// <summary>
        /// When overridden in the derived class, converts the given value of type <see cref="ActiveStatusEffectID"/> to
        /// an int.
        /// </summary>
        /// <param name="value">The value to convert to an int.</param>
        /// <returns>The int value of the <paramref name="value"/>.</returns>
        protected override int ToInt(ActiveStatusEffectID value)
        {
            return (int)value;
        }
    }
}