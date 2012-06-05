using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.World;

namespace DemoGame.Server
{
    /// <summary>
    /// Describes a <see cref="Map"/> that can be instantiated multiple times. Instanced maps are behave differently in that
    /// they do not persist indefinitely like normal maps, and there can be multiple instances of the same <see cref="MapInstance"/>,
    /// but only one <see cref="Map"/>. That is, a <see cref="Map"/> for a specific <see cref="MapID"/> should be looked at as only having
    /// one instance that persists, while <see cref="MapInstance"/>s can have multiple instances and do not persist.
    /// </summary>
    public class MapInstance : Map
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// How long, in milliseconds, to wait after the last user leaves an instanced map before deleting it. Although this
        /// delay does result in the instance staying in memory longer (wasting resources that could otherwise be used for other stuff),
        /// it is important to make sure that everything has time to completely leave the map before disposing of it.
        /// </summary>
        const int _deleteTimeout = 5000; // 5 seconds

        /// <summary>
        /// How long, in milliseconds, to wait to delete an instanced map before any users are ever added to it. This shouldn't ever
        /// really happen anyways, since map instances shouldn't be created and not used. But just in case that does happen, this will
        /// make sure we get rid of that map eventually instead of having it sit in memory doing nothing forever. It is important that this
        /// delay is plenty long just in case the server is bogged down. Definitely don't want to end up adding users to a disposed map!
        /// </summary>
        const int _initialDeleteTimeout = 30000; // 30 seconds

        /// <summary>
        /// A cached empty <see cref="IEnumerable{T}"/> of <see cref="NPC"/>s.
        /// </summary>
        static readonly IEnumerable<NPC> _emptyNPCs = Enumerable.Empty<NPC>();

        /// <summary>
        /// When this <see cref="MapInstance"/> will be deleted. If null, then we simply will not be checking to delete the map.
        /// This value will be null when there are users on the map.
        /// </summary>
        TickCount? _deleteTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapInstance"/> class.
        /// </summary>
        /// <param name="mapID">ID of the Map to create the instance of.</param>
        /// <param name="world">World that the <see cref="MapInstance"/> will be inside of.</param>
        /// <exception cref="ArgumentException"><paramref name="mapID"/> returned false for <see cref="MapBase.MapIDExists"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="world"/> is null.</exception>
        public MapInstance(MapID mapID, World world) : base(mapID, world)
        {
            Load();

            world.AddMapInstance(this);

            _deleteTime = TickCount.Now + _initialDeleteTimeout;
        }

        /// <summary>
        /// Gets if this is an instanced map.
        /// </summary>
        public override bool IsInstanced
        {
            get { return true; }
        }

        /// <summary>
        /// Disposes of the map and all of the Entities on it.
        /// </summary>
        public override void Dispose()
        {
            _deleteTime = null;

            base.Dispose();
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional processing on Entities added to the map.
        /// This is called after the Entity has finished being added to the map.
        /// </summary>
        /// <param name="entity">Entity that was added to the map.</param>
        protected override void EntityAdded(Entity entity)
        {
            base.EntityAdded(entity);

            // If a user was added, make sure the deletion timeout is cleared
            if (entity is User)
                _deleteTime = null;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional processing on Entities removed from the map.
        /// This is called after the Entity has finished being removed from the map.
        /// </summary>
        /// <param name="entity">Entity that was removed from the map.</param>
        protected override void EntityRemoved(Entity entity)
        {
            base.EntityRemoved(entity);

            // If the entity removed was a user, check if it was the last user. If so, start the countdown to deletion.
            if (entity is User)
            {
                if (Users.IsEmpty())
                    _deleteTime = TickCount.Now + _deleteTimeout;
                else
                    _deleteTime = null;
            }
        }

        /// <summary>
        /// Handles loading the persistent NPCs on the map.
        /// </summary>
        protected override IEnumerable<NPC> LoadPersistentNPCs()
        {
            // Do NOT load persistent anything on instanced maps!
            return _emptyNPCs;
        }

        /// <summary>
        /// Updates the map.
        /// </summary>
        /// <param name="deltaTime">The amount of time that elapsed since the last update.</param>
        public override void Update(int deltaTime)
        {
            if (IsDisposed)
                return;

            // For an instanced map, if we go a certain amount of time with no users on it (even if there are NPCs still alive and active),
            // we will delete it since we work under the assumption that once everyone leaves an instanced map, there is no way to get
            // back to that particular instance.
            if (_deleteTime.HasValue && _deleteTime.Value < TickCount.Now)
            {
                // Double-check the users really are all gone, just to be safe
                if (!Users.IsEmpty())
                {
                    const string errmsg =
                        "MapInstance `{0}`'s DeleteTime value has been reached, but `{1}` user(s) ({2}) are still on the map! There is probably a code logic error with DeleteTime...";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, this, Users.Count(), Users.Implode());
                    Debug.Fail(string.Format(errmsg, this, Users.Count(), Users.Implode()));

                    _deleteTime = null;
                }
                else
                {
                    // Dispose of the map and abort updating
                    Dispose();
                    return;
                }
            }

            base.Update(deltaTime);
        }
    }
}