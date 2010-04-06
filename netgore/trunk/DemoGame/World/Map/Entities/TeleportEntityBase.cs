using System.ComponentModel;
using System.Linq;
using NetGore;
using SFML.Graphics;

namespace DemoGame
{
    /// <summary>
    /// An <see cref="Entity"/> that can teleport another <see cref="Entity"/> to a new position and map upon use.
    /// </summary>
    public abstract class TeleportEntityBase : DynamicEntity, IUsableEntity
    {
        Vector2 _destination;
        MapID _destinationMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeleportEntityBase"/> class.
        /// </summary>
        protected TeleportEntityBase() : base(Vector2.Zero, Vector2.One)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Weight = 0f;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
        /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
        /// </summary>
        [Browsable(false)]
        public override bool CollidesAgainstWalls
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets the map position that the Entity will be teleported to upon use.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        [Category("Teleport")]
        [DisplayName("Destination")]
        [Description("Position to teleport the Entity that uses this Teleport to.")]
        [Browsable(true)]
        public Vector2 Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        /// <summary>
        /// Gets or sets the index of the map that the Entity will be teleported to upon use.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        [Category("Teleport")]
        [DisplayName("DestinationMap")]
        [Description("Map to teleport the Entity that uses this Teleport to.")]
        [Browsable(true)]
        public MapID DestinationMap
        {
            get { return _destinationMap; }
            set { _destinationMap = value; }
        }

        #region IUsableEntity Members

        /// <summary>
        /// Notifies the listeners when the IUsableEntity was used, and the DynamicEntity that used it. On the Client, this
        /// event will only be triggered if NotifyClientsOfUsage is true. The DynamicEntity argument
        /// that used this IUsableEntity may be null.
        /// </summary>
        public abstract event EntityEventHandler<DynamicEntity> OnUse;

        /// <summary>
        /// Gets if the Client should be notified when this IUsableEntity is used. If true, when this IUsableEntity is
        /// used on the Server, every Client in the Map will be notified of the usage. As a result, Use() will be called
        /// on each Client. If false, this message will never be sent. Only set to true if any code is placed in
        /// Use() on the Client implementation of the IUsableEntity, or there are expected to be listeners to OnUse.
        /// </summary>
        [Browsable(false)]
        public bool NotifyClientsOfUsage
        {
            get { return false; }
        }

        /// <summary>
        /// Client: 
        ///     Checks if the Client's character can attempt to use the IUsableEntity. If false, the Client
        ///     wont even attempt to use the IUsableEntity. If true, the Client will attempt to use it, but
        ///     it is not guarenteed the Server will also allow it to be used.
        /// Server:
        ///     Checks if the specified Entity may use the IUsableEntity.
        /// </summary>
        /// <param name="charEntity">The CharacterEntity that is trying to use this IUsableEntity. For the Client,
        /// this will always be the User's Character. Can be null.</param>
        /// <returns>True if this IUsableEntity can be used by the <paramref name="charEntity"/>, else false.</returns>
        public virtual bool CanUse(DynamicEntity charEntity)
        {
            if (charEntity == null)
                return false;

            return true;
        }

        /// <summary>
        /// Client:
        ///     Handles any additional usage stuff. When this is called, it is to be assumed that the Server has recognized
        ///     the IUsableEntity as having been successfully used.
        /// Server:
        ///     Attempts to use this IUsableEntity on the <paramref name="charEntity"/>.
        /// </summary>
        /// <param name="charEntity">CharacterEntity that is trying to use this IUsableEntity. Can be null.</param>
        /// <returns>True if this IUsableEntity was successfully used, else false. On the Client, this is generally
        /// unused.</returns>
        public abstract bool Use(DynamicEntity charEntity);

        #endregion
    }
}