using System.Collections.Generic;
using System.Linq;
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
        /// Initializes the <see cref="MapItemsCleaner"/> class.
        /// </summary>
        static MapItemsCleaner()
        {
            // Set the default item life to 3 minutes
            DefaultItemLife = 1000 * 60 * 3;

            // Default update rate to 30 seconds
            DefaultUpdateRate = 1000 * 30;
        }

        /// <summary>
        /// Gets or sets the amount of time an item may remain on the map before it is removed. This value is used for all
        /// <see cref="MapItemsCleaner"/> instances.
        /// </summary>
        public static int DefaultItemLife { get; set; }

        /// <summary>
        /// Gets or sets the minimum amount of time in milliseconds that may elapse between checks for expired items.
        /// The lower this value, the closer the time the items are removed will be to the
        /// actual sepcified time, but the greater the performance cost. It is recommended to keep this
        /// value greater than at least 10 seconds to avoid unneccesary performance overhead. This value is used for all
        /// <see cref="MapItemsCleaner"/> instances.
        /// </summary>
        public static int DefaultUpdateRate { get; set; }

        /// <summary>
        /// When overridden in the derived class, gets the minimum amount of time in milliseconds that must elapsed
        /// between calls to Update. If this amount of time has not elapsed, calls to Update will just return 0.
        /// </summary>
        /// <value></value>
        protected override int UpdateRate
        {
            get { return DefaultUpdateRate; }
        }

        /// <summary>
        /// Adds an item to start tracking. If the <paramref name="item"/> already exists in the collection, its
        /// time until deletion will be reset.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="currentTime">The current time.</param>
        public void Add(ItemEntityBase item, int currentTime)
        {
            Add(item, currentTime, DefaultItemLife);
        }

        /// <summary>
        /// When overridden in the derived class, handles when an item has expired since it has been in this collection
        /// for longer the allowed time.
        /// </summary>
        /// <param name="item">The item that has expired.</param>
        protected override void ExpireItem(ItemEntityBase item)
        {
            item.Dispose();
        }
    }
}