using System.ComponentModel;
using System.Linq;
using NetGore;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame
{
    /// <summary>
    /// Defines the basics of a single item.
    /// </summary>
    public abstract class ItemEntityBase : DynamicEntity, IPickupableEntity, IUpdateableEntity
    {
        /// <summary>
        /// Maximum number of items allowed in a single item stack.
        /// </summary>
        public const byte MaxStackSize = 99;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntity"/> class.
        /// </summary>
        /// <param name="position">The initial world position.</param>
        /// <param name="size">The initial size.</param>
        protected ItemEntityBase(Vector2 position, Vector2 size) : base(position, size)
        {
        }

        /// <summary>
        /// Gets or sets the size of this item cluster (1 for a single item).
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        public abstract byte Amount { get; set; }

        /// <summary>
        /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
        /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
        /// </summary>
        [Browsable(false)]
        public override bool CollidesAgainstWalls
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the index of the graphic that is used for this item.
        /// </summary>
        [SyncValue]
        public abstract GrhIndex GraphicIndex { get; set; }

        /// <summary>
        /// Checks if this item can be stacked with another item. To stack, both items must contain the same
        /// stat modifiers, name, description, value, and graphic index.
        /// </summary>
        /// <param name="source">Item to check if can stack on this item</param>
        /// <returns>True if the two items can stack on each other, else false</returns>
        public abstract bool CanStack(ItemEntityBase source);

        /// <summary>
        /// Handles when the Entity collides into another entity. Not synonymous with CollideFrom we
        /// were the ones who collided into the <paramref name="collideWith"/> Entity. For example, if the
        /// two Entities in question were a moving Character and a stationary Wall, this Entity would be
        /// the Character and <paramref name="collideWith"/> would be the Wall.
        /// </summary>
        /// <param name="collideWith">Entity that this Entity collided with</param>
        /// <param name="displacement">Displacement between the two Entities</param>
        public override void CollideInto(Entity collideWith, Vector2 displacement)
        {
        }

        /// <summary>
        /// Creates a deep copy of the inheritor, which is a new class with the same values, and returns
        /// the copy as an ItemEntityBase.
        /// </summary>
        /// <returns>A deep copy of the object</returns>
        public abstract ItemEntityBase DeepCopy();

        /// <summary>
        /// Removes all records of the <see cref="ItemEntityBase"/> and disposes it. Use this when you want to completely remove an
        /// <see cref="ItemEntityBase"/>, not just unload it from memory.
        /// </summary>
        public void Destroy()
        {
            if (IsDisposed)
                return;

            HandleDestroy();
        }

        /// <summary>
        /// When overridden in the derived class, handles destroying the <see cref="ItemEntityBase"/>.
        /// </summary>
        protected abstract void HandleDestroy();

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected override void HandleUpdate(IMap imap, int deltaTime)
        {
            // For items, once they hit the ground, they no longer update
            if (IsOnGround)
                return;

            // Perform typical collision detection/etc
            UpdateVelocity(imap, deltaTime);
            base.HandleUpdate(imap, deltaTime);
        }

        #region IPickupableEntity Members

        /// <summary>
        /// Notifies listeners that this <see cref="Entity"/> was picked up
        /// </summary>
        public abstract event TypedEventHandler<Entity, EventArgs<CharacterEntity>> PickedUp;

        /// <summary>
        /// Checks if this <see cref="Entity"/> can be picked up by the specified <paramref name="charEntity"/>, but does
        /// not actually pick up this <see cref="Entity"/>.
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="Entity"/>.</param>
        /// <returns>True if this <see cref="Entity"/> can be picked up, else false.</returns>
        public abstract bool CanPickup(CharacterEntity charEntity);

        /// <summary>
        /// Picks up this <see cref="Entity"/>.
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to pick up this <see cref="Entity"/>.</param>
        /// <returns>True if this <see cref="Entity"/> was successfully picked up, else false.</returns>
        public abstract bool Pickup(CharacterEntity charEntity);

        #endregion

        #region IUpdateableEntity Members

        /// <summary>
        /// Updates the <see cref="IUpdateableEntity"/>.
        /// </summary>
        /// <param name="imap">The map that this <see cref="IUpdateableEntity"/> is on.</param>
        /// <param name="deltaTime">Time elapsed (in milliseconds) since the last update.</param>
        public void Update(IMap imap, int deltaTime)
        {
            HandleUpdate(imap, deltaTime);
        }

        #endregion
    }
}