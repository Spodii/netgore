using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Server.Queries;
using NetGore.Db;

namespace DemoGame.Server
{
    /// <summary>
    /// Manages the <see cref="Alliance"/> instances.
    /// </summary>
    public class AllianceManager : DbTableDataManager<AllianceID, Alliance>
    {
        static readonly AllianceManager _instance;

        SelectAllianceAttackableQuery _selectAllianceAttackableQuery;
        SelectAllianceHostileQuery _selectAllianceHostileQuery;
        SelectAllianceQuery _selectAllianceQuery;

        /// <summary>
        /// Initializes the <see cref="AllianceManager"/> class.
        /// </summary>
        static AllianceManager()
        {
            _instance = new AllianceManager(DbControllerBase.GetInstance());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceManager"/> class.
        /// </summary>
        /// <param name="dbController">The IDbController.</param>
        AllianceManager(IDbController dbController) : base(dbController)
        {
        }

        /// <summary>
        /// Gets an instance of the <see cref="AllianceManager"/>.
        /// </summary>
        public static AllianceManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, provides a chance to cache frequently used queries instead of
        /// having to grab the query from the <see cref="IDbController"/> every time. Caching is completely
        /// optional, but if you do cache any queries, it should be done here. Do not use this method for
        /// anything other than caching queries from the <paramref name="dbController"/>.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> to grab the queries from.</param>
        protected override void CacheDbQueries(IDbController dbController)
        {
            _selectAllianceQuery = dbController.GetQuery<SelectAllianceQuery>();
            _selectAllianceAttackableQuery = dbController.GetQuery<SelectAllianceAttackableQuery>();
            _selectAllianceHostileQuery = dbController.GetQuery<SelectAllianceHostileQuery>();

            base.CacheDbQueries(dbController);
        }

        /// <summary>
        /// When overridden in the derived class, gets all of the IDs in the table being managed.
        /// </summary>
        /// <returns>An IEnumerable of all of the IDs in the table being managed.</returns>
        protected override IEnumerable<AllianceID> GetIDs()
        {
            return DbController.GetQuery<SelectAllianceIDsQuery>().Execute();
        }

        /// <summary>
        /// When overridden in the derived class, loads an item from the database.
        /// </summary>
        /// <param name="id">The ID of the item to load.</param>
        /// <returns>The item loaded from the database.</returns>
        protected override Alliance LoadItem(AllianceID id)
        {
            var values = _selectAllianceQuery.Execute(id);
            var attackables = _selectAllianceAttackableQuery.Execute(id);
            var hostiles = _selectAllianceHostileQuery.Execute(id);

            Debug.Assert(id == values.ID);
            Debug.Assert(attackables.All(x => x.AllianceID == id));
            Debug.Assert(hostiles.All(x => x.AllianceID == id));

            var ret = new Alliance(id, values.Name, attackables, hostiles);
            return ret;
        }
    }
}