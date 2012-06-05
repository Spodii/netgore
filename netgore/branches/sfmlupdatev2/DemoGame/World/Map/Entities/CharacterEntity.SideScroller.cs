#if TOPDOWN
#pragma warning disable 1587
#endif

#if !TOPDOWN

using System.ComponentModel;
using System.Linq;
using NetGore;
using SFML.Graphics;

namespace DemoGame
{
    public abstract partial class CharacterEntity
    {
        CharacterState _state = CharacterState.Idle;

        /// <summary>
        /// Gets if the character meets the requirements allowing them to jump. For top-down, this is always false.
        /// </summary>
        [Browsable(false)]
        public bool CanJump
        {
            get { return IsOnGround; }
        }

        /// <summary>
        /// Gets the character's current state.
        /// </summary>
        public CharacterState State
        {
            get { return _state; }
        }

        /// <summary>
        /// Stops the character's controllable movement. Any forces acting upon the character, such as gravity, will
        /// not be affected.
        /// </summary>
        public virtual void StopMoving()
        {
            SetVelocity(new Vector2(0.0f, Velocity.Y));
        }

        /// <summary>
        /// Updates the character's state.
        /// </summary>
        void UpdateState()
        {
            // Start with the idle state
            var newState = CharacterState.Idle;

            // Check the vertical state (moving up or down)
            if (Velocity.Y != 0 || StandingOn == null)
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

#endif