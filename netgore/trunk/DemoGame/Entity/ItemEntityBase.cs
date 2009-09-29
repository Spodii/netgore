using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.RPGComponents;

namespace DemoGame
{
    /// <summary>
    /// Defines the basics of a single item.
    /// </summary>
    public abstract class ItemEntityBase : DynamicEntity, IPickupableEntity
    {
        /// <summary>
        /// Maximum number of items allowed in a single item stack.
        /// </summary>
        public const byte MaxStackSize = 99;

        /// <summary>
        /// Gets or sets the size of this item cluster (1 for a single item).
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        public abstract byte Amount { get; set; }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        public abstract string Description { get; set; }

        /// <summary>
        /// Gets or sets the index of the graphic that is used for this item.
        /// </summary>
        [SyncValue]
        public abstract GrhIndex GraphicIndex { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        public abstract string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of item this is.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        public abstract ItemType Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the item.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        public abstract int Value { get; set; }

        /// <summary>
        /// ItemEntityBase constructor
        /// </summary>
        /// <param name="pos">Position to place the item</param>
        /// <param name="size">Size of the item's CollisionBox</param>
        protected ItemEntityBase(Vector2 pos, Vector2 size)
        {
            // NOTE: Can I get rid of this constructor?
            CB = new CollisionBox(pos, size.X, size.Y);
        }

        /// <summary>
        /// ItemEntityBase constructor
        /// </summary>
        protected ItemEntityBase()
        {
        }

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
            // Collision against a wall
            if (collideWith is WallEntityBase)
            {
                // Move the item away from the wall
                Move(displacement);

                // Check for vertical collision
                if (displacement.Y != 0)
                {
                    if (Velocity.Y >= 0 && displacement.Y < 0)
                    {
                        // Collision from falling
                        SetVelocity(new Vector2(Velocity.X, 0.0f));
                        OnGround = true;
                    }
                    else if (Velocity.Y < 0 && displacement.Y > 0)
                    {
                        // Collision from rising
                        SetVelocity(new Vector2(Velocity.X, 0.0f));
                    }
                }
            }
        }

        /// <summary>
        /// Creates a deep copy of the inheritor, which is a new class with the same values, and returns
        /// the copy as an ItemEntityBase.
        /// </summary>
        /// <returns>A deep copy of the object</returns>
        public abstract ItemEntityBase DeepCopy();

        /// <summary>
        /// Updates the Entity
        /// </summary>
        /// <param name="imap">Map that the Entity is on</param>
        /// <param name="deltaTime">Time elapsed (in milliseconds) since the last update</param>
        public override void Update(IMap imap, float deltaTime)
        {
            // For items, once they hit the ground, they no longer update
            if (OnGround)
                return;

            // Perform typical collision detection/etc
            UpdateVelocity(deltaTime);
            base.Update(imap, deltaTime);
        }

        #region IPickupableEntity Members

        /// <summary>
        /// Notifies listeners that this <see cref="Entity"/> was picked up, and who it was picked up by.
        /// </summary>
        public abstract event EntityEventHandler<CharacterEntityBase> OnPickup;

        /// <summary>
        /// Checks if this <see cref="Entity"/> can be picked up by the specified <paramref name="charEntity"/>, but does
        /// not actually pick up this <see cref="Entity"/>
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntityBase"/> that is trying to use this <see cref="Entity"/></param>
        /// <returns>
        /// True if this <see cref="Entity"/> can be picked up, else false
        /// </returns>
        public abstract bool CanPickup(CharacterEntityBase charEntity);

        /// <summary>
        /// Picks up this <see cref="Entity"/>
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntityBase"/> that is trying to pick up this <see cref="Entity"/></param>
        /// <returns>
        /// True if this <see cref="Entity"/> was successfully picked up, else false
        /// </returns>
        public abstract bool Pickup(CharacterEntityBase charEntity);

        #endregion
    }
}