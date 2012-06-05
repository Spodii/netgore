using System.Linq;
using DemoGame.Server.Properties;
using NetGore;
using NetGore.Collections;

namespace DemoGame.Server
{
    /// <summary>
    /// Keeps track of the items that are on a map, and how long they have been on the map. After they have been sitting
    /// on the map for a period of time, the are disposed. This collection is not thread safe.
    /// </summary>
    class MapItemsCleaner : TimeExpirationQueue<ItemEntityBase>
    {
        /// <summary>
        /// When overridden in the derived class, gets the minimum amount of time in milliseconds that must elapsed
        /// between calls to Update. If this amount of time has not elapsed, calls to Update will just return 0.
        /// </summary>
        protected override TickCount UpdateRate
        {
            get { return ServerSettings.Default.MapItemExpirationUpdateRate; }
        }

        /// <summary>
        /// Adds an item to start tracking. If the <paramref name="item"/> already exists in the collection, its
        /// time until deletion will be reset.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="currentTime">The current time.</param>
        public void Add(ItemEntityBase item, TickCount currentTime)
        {
            Add(item, currentTime, ServerSettings.Default.DefaultMapItemLife);
        }

        /// <summary>
        /// When overridden in the derived class, handles when an item has expired since it has been in this collection
        /// for longer the allowed time.
        /// </summary>
        /// <param name="item">The item that has expired.</param>
        protected override void ExpireItem(ItemEntityBase item)
        {
            var i = item as ItemEntity;
            if (i != null)
            {
                var template = i.ItemTemplateID;
                if (template.HasValue)
                    EventCounterManager.ItemTemplate.Increment(template.Value, ItemTemplateEventCounterType.ExpiredOnMap,
                        item.Amount);
            }

            item.Dispose();
        }
    }
}