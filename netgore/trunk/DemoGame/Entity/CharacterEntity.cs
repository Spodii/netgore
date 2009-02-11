using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Platyform;
using Platyform.Extensions;

namespace DemoGame
{
    /// <summary>
    /// Abstract class for an Entity that is a Character
    /// </summary>
    public abstract class CharacterEntity : Entity
    {
        Direction _heading = Direction.East;

        CharacterState _state = CharacterState.Idle;

        /// <summary>
        /// Gets if the character meets the requirements allowing them to jump
        /// </summary>
        public bool CanJump
        {
            get { return OnGround; }
        }

        /// <summary>
        /// Gets the direction the character is currently facing
        /// </summary>
        public Direction Heading
        {
            get { return _heading; }
        }

        /// <summary>
        /// Gets if the character is moving left or right
        /// </summary>
        public bool IsMoving
        {
            get { return Velocity.X != 0; }
        }

        /// <summary>
        /// Gets if the character is moving to the left
        /// </summary>
        public bool IsMovingLeft
        {
            get { return Velocity.X < 0; }
        }

        /// <summary>
        /// Gets if the character is moving to the right
        /// </summary>
        public bool IsMovingRight
        {
            get { return Velocity.X > 0; }
        }

        /// <summary>
        /// Gets or sets the character's unique index for the map they are on
        /// </summary>
        public ushort MapCharIndex { get; set; }

        /// <summary>
        /// Gets the character's current state
        /// </summary>
        public CharacterState State
        {
            get { return _state; }
        }

        /// <summary>
        /// Handle collision against other entities
        /// </summary>
        /// <param name="collideWith">Entity the character collided with</param>
        /// <param name="displacement">Displacement between the character and entity</param>
        public override void CollideInto(Entity collideWith, Vector2 displacement)
        {
            // Collision against a wall
            if (collideWith is WallEntity)
            {
                // Move the character away from the wall
                Move(displacement);

                // Check for vertical collision
                if (displacement.Y != 0)
                {
                    if (Velocity.Y >= 0 && displacement.Y < 0)
                    {
                        // Collision from falling (land on feet)
                        SetVelocity(new Vector2(Velocity.X, 0.0f));
                        OnGround = true;
                    }
                    else if (Velocity.Y < 0 && displacement.Y > 0)
                    {
                        // Collision from rising (hit head)
                        SetVelocity(new Vector2(Velocity.X, 0.0f));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the map interface for the CharacterEntity. If no valid map interface is supplied,
        /// no map-based collision detection and updating can be used.
        /// </summary>
        /// <returns>Map interface for the CharacterEntity</returns>
        protected abstract IMap GetIMap();

        /// <summary>
        /// Sets the character's heading
        /// </summary>
        /// <param name="newHeading">New heading for the character</param>
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
        /// Perform character updating post-collision
        /// </summary>
        public override void Update(IMap imap, float deltaTime)
        {
            // Perform pre-collision detection updating
            UpdatePreCollision(deltaTime);

            // Performs basic entity updating
            base.Update(imap, deltaTime);

            // Perform post-collision detection updating
            UpdatePostCollision(deltaTime);
        }

        /// <summary>
        /// Moves the character to the new location. Unlike Teleport(), this will not set the
        /// velocity to zero, and is intended for position corrections / resynchronization.
        /// </summary>
        /// <param name="position">Correct position</param>
        public virtual void UpdatePosition(Vector2 position)
        {
            base.Teleport(position);
        }

        protected virtual void UpdatePostCollision(float deltaTime)
        {
            // Update the character's state
            UpdateState();
        }

        /// <summary>
        /// Performs the pre-collision detection updating
        /// </summary>
        protected virtual void UpdatePreCollision(float deltaTime)
        {
            // Update velocity
            UpdateVelocity(deltaTime);
        }

        /// <summary>
        /// Updates the character's state
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
    }
}