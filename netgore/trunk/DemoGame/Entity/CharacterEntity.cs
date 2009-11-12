using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Abstract class for an Entity that is a Character.
    /// </summary>
    public abstract class CharacterEntity : DynamicEntity, IUpdateableEntity
    {
        /// <summary>
        /// The maximum distance, in pixels, a character can pick up an item.
        /// </summary>
        public const int MaxPickupDistance = 32;

        Direction _heading = Direction.East;
        CharacterState _state = CharacterState.Idle;

        /// <summary>
        /// Gets or sets (protected) the CharacterEntity's BodyInfo.
        /// </summary>
        public BodyInfo BodyInfo { get; protected set; }

        /// <summary>
        /// Synchronizes the BodyInfo index for the CharacterEntity.
        /// </summary>
        [SyncValue("BodyIndex")]
        protected internal BodyIndex BodyInfoIndex
        {
            get { return BodyInfo.Index; }
            set { BodyInfo = BodyInfoManager.Instance.GetBody(value); }
        }

        /// <summary>
        /// Gets if the character meets the requirements allowing them to jump.
        /// </summary>
        [Browsable(false)]
        public bool CanJump
        {
            get { return OnGround; }
        }

        /// <summary>
        /// When overridden in the derived class, gets or protected sets if the CharacterEntity has a chat dialog.
        /// </summary>
        [SyncValue("HasChatDialog")]
        [Browsable(false)]
        public abstract bool HasChatDialog { get; protected set; }

        /// <summary>
        /// When overridden in the derived class, gets or protected sets if the CharacterEntity has a shop.
        /// </summary>
        [SyncValue("HasShop")]
        [Browsable(false)]
        public abstract bool HasShop { get; protected set; }

        /// <summary>
        /// Gets the direction the character is currently facing.
        /// </summary>
        public Direction Heading
        {
            get { return _heading; }
        }

        /// <summary>
        /// Gets if the character is moving left or right.
        /// </summary>
        [Browsable(false)]
        public bool IsMoving
        {
            get { return Velocity.X != 0; }
        }

        /// <summary>
        /// Gets if the character is moving to the left.
        /// </summary>
        [Browsable(false)]
        public bool IsMovingLeft
        {
            get { return Velocity.X < 0; }
        }

        /// <summary>
        /// Gets if the character is moving to the right.
        /// </summary>
        [Browsable(false)]
        public bool IsMovingRight
        {
            get { return Velocity.X > 0; }
        }

        /// <summary>
        /// Gets or sets the name of the CharacterEntity.
        /// </summary>
        [SyncValue]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets the character's current state
        /// </summary>
        public CharacterState State
        {
            get { return _state; }
        }

        /// <summary>
        /// Handles collision against other entities.
        /// </summary>
        /// <param name="collideWith">Entity the character collided with.</param>
        /// <param name="displacement">Displacement between the character and entity.</param>
        public override void CollideInto(Entity collideWith, Vector2 displacement)
        {
            // Collision against a wall
            if (collideWith is WallEntityBase)
                WallEntityBase.HandleCollideInto(this, displacement);
        }

        /// <summary>
        /// Gets the map interface for the CharacterEntity. If no valid map interface is supplied,
        /// no map-based collision detection and updating can be used.
        /// </summary>
        /// <returns>Map interface for the CharacterEntity.</returns>
        protected abstract IMap GetIMap();

        /// <summary>
        /// Gets a <see cref="Rectangle"/> representing the region a <see cref="CharacterEntity"/> can pick up
        /// <see cref="IPickupableEntity"/> entities from.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> representing the region a <see cref="CharacterEntity"/> can pick up
        /// <see cref="IPickupableEntity"/> entities from.</returns>
        public Rectangle GetPickupRegion()
        {
            return new Rectangle((int)Position.X - MaxPickupDistance, (int)Position.Y - MaxPickupDistance,
                                 (int)Size.X + (MaxPickupDistance * 2), (int)Size.Y + (MaxPickupDistance * 2));
        }

        /// <summary>
        /// Checks if the given <paramref name="toPickup"/> is close enough to the <see cref="CharacterEntity"/>
        /// to be picked up.
        /// </summary>
        /// <param name="toPickup">The <see cref="Entity"/> to be picked up.</param>
        /// <returns>True if the <paramref name="toPickup"/> is close enough to be picked up; otherwise false.</returns>
        public bool IsInPickupRegion(Entity toPickup)
        {
            var dist = toPickup.CB.GetDistance(CB);
            return (dist <= MaxPickupDistance);
        }

        /// <summary>
        /// Sets the character's heading.
        /// </summary>
        /// <param name="newHeading">New heading for the character.</param>
        public virtual void SetHeading(Direction newHeading)
        {
            _heading = newHeading;
        }

        /// <summary>
        /// Stop the character's controllable movement (ie moving left or right).
        /// </summary>
        public virtual void StopMoving()
        {
            SetVelocity(new Vector2(0.0f, Velocity.Y));
        }

        /// <summary>
        /// Moves the character to a new location instantly. The character's velocity will
        /// also be set to zero upon teleporting.
        /// </summary>
        /// <param name="position">New position</param>
        public override void Teleport(Vector2 position)
        {
            // Force the character to stop moving
            _state = CharacterState.Falling;
            SetVelocity(Vector2.Zero);
            StopMoving();

            // Teleport
            base.Teleport(position);
        }

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected override void HandleUpdate(IMap imap, float deltaTime)
        {
            ThreadAsserts.IsMainThread();
            Debug.Assert(imap != null, "How the hell is a null Map updating?");
            Debug.Assert(deltaTime >= 0, "Unless we're going back in time, deltaTime < 0 makes no sense at all.");

            // Perform pre-collision detection updating
            UpdatePreCollision(deltaTime);

            // Performs basic entity updating
            base.HandleUpdate(imap, deltaTime);

            // Perform post-collision detection updating
            UpdatePostCollision(deltaTime);
        }

        /// <summary>
        /// Moves the character to the new location. Unlike Teleport(), this will not set the
        /// velocity to zero, and is intended for position corrections / resynchronization.
        /// </summary>
        /// <param name="position">Correct position.</param>
        public virtual void UpdatePosition(Vector2 position)
        {
            base.Teleport(position);
        }

        /// <summary>
        /// Performs the post-collision detection updating.
        /// </summary>
        /// <param name="deltaTime">Time elapsed (in milliseconds) since the last update.</param>
        protected virtual void UpdatePostCollision(float deltaTime)
        {
            // Update the character's state
            UpdateState();
        }

        /// <summary>
        /// Performs the pre-collision detection updating.
        /// </summary>
        /// <param name="deltaTime">Time elapsed (in milliseconds) since the last update.</param>
        protected virtual void UpdatePreCollision(float deltaTime)
        {
            // Update velocity
            UpdateVelocity(deltaTime);
        }

        /// <summary>
        /// Updates the character's state.
        /// </summary>
        void UpdateState()
        {
            // Start with the idle state
            CharacterState newState = CharacterState.Idle;

            // Check the vertical state (moving up or down)
            if (Velocity.Y != 0 || !OnGround)
            {
                if (Velocity.Y > 0)
                    newState = CharacterState.Falling;
                else
                    newState = CharacterState.Jumping;
            }

            // Check the horizontal state (left or moving)
            if (Velocity.X != 0)
            {
                // Set the heading accordingly
                if (Velocity.X < 0)
                    _heading = Direction.West;
                else
                    _heading = Direction.East;

                // Change from the stationary state to its moving equivilant
                switch (newState)
                {
                    case CharacterState.Idle:
                        if (Velocity.X > 0)
                            newState = CharacterState.WalkingRight;
                        else
                            newState = CharacterState.WalkingLeft;
                        break;

                    case CharacterState.Falling:
                        if (Velocity.X > 0)
                            newState = CharacterState.FallingRight;
                        else
                            newState = CharacterState.FallingLeft;
                        break;

                    case CharacterState.Jumping:
                        if (Velocity.X > 0)
                            newState = CharacterState.JumpingRight;
                        else
                            newState = CharacterState.JumpingLeft;
                        break;
                }
            }

            // Set the new state
            _state = newState;
        }

        /// <summary>
        /// Updates the <see cref="IUpdateableEntity"/>.
        /// </summary>
        /// <param name="imap">The map that this <see cref="IUpdateableEntity"/> is on.</param>
        /// <param name="deltaTime">Time elapsed (in milliseconds) since the last update.</param>
        public void Update(IMap imap, float deltaTime)
        {
            HandleUpdate(imap, deltaTime);
        }
    }
}