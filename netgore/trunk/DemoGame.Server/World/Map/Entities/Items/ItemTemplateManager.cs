using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server
{
    /// <summary>
    /// Manages the <see cref="ItemTemplateManager"/> instances.
    /// </summary>
    public class ItemTemplateManager : DbTableDataManager<ItemTemplateID, IItemTemplateTable>
    {
        static readonly ItemTemplateManager _instance;
        static readonly SafeRandom _rnd = new SafeRandom();

        SelectItemTemplateQuery _selectItemTemplateQuery;

        /// <summary>
        /// Initializes the <see cref="ItemTemplateManager"/> class.
        /// </summary>
        static ItemTemplateManager()
        {
            _instance = new ItemTemplateManager(DbControllerBase.GetInstance());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemTemplateManager"/> class.
        /// </summary>
        /// <param name="dbController">The IDbController.</param>
        ItemTemplateManager(IDbController dbController) : base(dbController)
        {
        }

        /// <summary>
        /// Gets an instance of the <see cref="ItemTemplateManager"/>.
        /// </summary>
        public static ItemTemplateManager Instance
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
            _selectItemTemplateQuery = dbController.GetQuery<SelectItemTemplateQuery>();

            base.CacheDbQueries(dbController);
        }

        /// <summary>
        /// When overridden in the derived class, gets all of the IDs in the table being managed.
        /// </summary>
        /// <returns>An IEnumerable of all of the IDs in the table being managed.</returns>
        protected override IEnumerable<ItemTemplateID> GetIDs()
        {
            return DbController.GetQuery<SelectItemTemplateIDsQuery>().Execute();
        }

        /// <summary>
        /// Get a random ItemTemplate.
        /// </summary>
        /// <returns>A random ItemTemplate. Will not be null.</returns>
        public IItemTemplateTable GetRandomTemplate()
        {
            var max = Length;

            IItemTemplateTable template;
            do
            {
                var i = _rnd.Next(0, max);
                if (!TryGetValue(new ItemTemplateID(i), out template))
                    template = null;
            }
            while (template == null);

            return template;
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> as an int.</returns>
        protected override int IDToInt(ItemTemplateID value)
        {
            return (int)value;
        }

        /// <summary>
        /// When overridden in the derived class, converts the int to a <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The int as a <paramref name="value"/>.</returns>
        public override ItemTemplateID IntToID(int value)
        {
            return new ItemTemplateID(value);
        }

        /// <summary>
        /// When overridden in the derived class, loads an item from the database.
        /// </summary>
        /// <param name="id">The ID of the item to load.</param>
        /// <returns>The item loaded from the database.</returns>
        protected override IItemTemplateTable LoadItem(ItemTemplateID id)
        {
            var v = _selectItemTemplateQuery.Execute(id);
            return v;
        }
    }
}