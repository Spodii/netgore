using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class PeerTradingGetLostItemsQuery : DbQueryReader<CharacterID>
    {
        static readonly IEnumerable<ItemID> _emptyCollection = Enumerable.Empty<ItemID>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerTradingGetLostItemsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        public PeerTradingGetLostItemsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(ActiveTradeItemTable.DbColumns, "character_id", "item_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT `item_id` FROM `{0}` WHERE `character_id` = @characterID

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(ActiveTradeItemTable.TableName).Add("item_id").Where(f.Equals(s.EscapeColumn("character_id"),
                    s.Parameterize("characterID")));
            return q.ToString();
        }

        public IEnumerable<ItemID> Execute(CharacterID characterID)
        {
            List<ItemID> ret = null;

            using (var r = ExecuteReader(characterID))
            {
                while (r.Read())
                {
                    var itemID = r.GetItemID("item_id");

                    // Delay the creation of the return list if possible so we can avoid needless garbage
                    if (ret == null)
                        ret = new List<ItemID>();

                    ret.Add(itemID);
                }
            }

            Debug.Assert(ret == null || !ret.HasDuplicates(),
                "There shouldn't be any duplicates since the item_id field should be a primary key...");

            // If there were no items for this character, return the empty collection
            if (ret == null)
                return _emptyCollection;
            else
                return ret;
        }

        #region Overrides of DbQueryBase

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("characterID");
        }

        #endregion

        #region Overrides of DbQueryReader<CharacterID>

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterID item)
        {
            p["characterID"] = (int)item;
        }

        #endregion
    }
}