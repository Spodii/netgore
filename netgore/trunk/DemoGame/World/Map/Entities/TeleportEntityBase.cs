using System.ComponentModel;
using System.Linq;
using NetGore;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame
{
    /// <summary>
    /// An entity that will teleport a Character when touched.
    /// </summary>
    public abstract class TeleportEntityBase : DynamicEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeleportEntityBase"/> class.
        /// </summary>
        protected TeleportEntityBase() : base(Vector2.Zero, Vector2.One)
        {
            Weight = 0f;
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
        public Vector2 Destination { get; set; }

        /// <summary>
        /// Gets or sets the index of the map that the Entity will be teleported to upon use.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        [Category("Teleport")]
        [DisplayName("DestinationMap")]
        [Description("Map to teleport the Entity that uses this Teleport to.")]
        [Browsable(true)]
        public MapID DestinationMap { get; set; }

        /// <summary>
        /// Gets or sets the index of the instanced map that the Entity will be teleported to upon use.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        [Category("Teleport")]
        [DisplayName("DestinationMapInstance")]
        [Description("Instanced Map to teleport the Entity that uses this Teleport to.")]
        [Browsable(true)]
        public MapID DestinationMapInstance { get; set; }

        /// <summary>
        /// Gets or sets if group members are teleported too - only with instanced maps.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        [Category("Teleport")]
        [DisplayName("Teleport Group Members")]
        [Description("Do you want group members teleported too?")]
        [Browsable(true)]
        public bool TeleportGroupMembers { get; set; }
    }
}