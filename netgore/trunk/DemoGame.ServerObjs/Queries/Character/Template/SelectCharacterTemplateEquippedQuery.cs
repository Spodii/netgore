using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterTemplateEquippedQuery : DbQueryReader<CharacterTemplateID>
    {
        static readonly string _queryString =
            string.Format("SELECT * FROM `{0}` WHERE `character_template_id`=@characterTemplateID",
                          CharacterTemplateEquippedTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterTemplateEquippedQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterTemplateEquippedQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<CharacterTemplateEquippedTable> Execute(CharacterTemplateID templateID)
        {
            var ret = new List<CharacterTemplateEquippedTable>();

            using (var r = ExecuteReader(templateID))
            {
                while (r.Read())
                {
                    var item = new CharacterTemplateEquippedTable();
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
            return CreateParameters("@characterTemplateID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="id">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterTemplateID id)
        {
            p["@characterTemplateID"] = (int)id;
        }
    }
}