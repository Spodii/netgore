using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterStatusEffectsQuery : DbQueryReader<CharacterID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `character_id`=@character_id",
                                                            CharacterStatusEffectTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterStatusEffectsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterStatusEffectsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
            QueryAsserts.ContainsColumns(CharacterStatusEffectTable.DbColumns, "character_id");
        }

        public IEnumerable<ICharacterStatusEffectTable> Execute(CharacterID id)
        {
            var ret = new List<ICharacterStatusEffectTable>(4);

            using (var r = ExecuteReader(id))
            {
                while (r.Read())
                {
                    var item = new CharacterStatusEffectTable();
                    item.ReadValues(r);
                    ret.Add(item);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@character_id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="id">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID id)
        {
            p["@character_id"] = (int)id;
        }
    }
}