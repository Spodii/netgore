using System.Linq;

#if !TOPDOWN
#pragma warning disable 1587
#endif

#if TOPDOWN

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using NetGore;
using SFML.Graphics;

namespace DemoGame
{
    public abstract partial class CharacterEntity
    {
        /// <summary>
        /// Gets if the character meets the requirements allowing them to jump. For top-down, this is always false.
        /// </summary>
        [Browsable(false)]
        public bool CanJump
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets if the character is moving up.
        /// </summary>
        [Browsable(false)]
        public bool IsMovingDown
        {
            get { return Velocity.Y > 0; }
        }

        /// <summary>
        /// Gets if the character is moving up.
        /// </summary>
        [Browsable(false)]
        public bool IsMovingUp
        {
            get { return Velocity.Y < 0; }
        }

        /// <summary>
        /// Stops the character's controllable movement. Any forces acting upon the character, such as gravity, will
        /// not be affected.
        /// </summary>
        public virtual void StopMoving()
        {
            SetVelocity(Vector2.Zero);
        }

        /// <summary>
        /// Stops the character's horizontal movement. Any forces acting upon the character, such as gravity, will
        /// not be affected.
        /// </summary>
        public virtual void StopMovingHorizontal()
        {
            SetVelocity(new Vector2(0, Velocity.Y));
        }

        /// <summary>
        /// Stops the character's vertical movement. Any forces acting upon the character, such as gravity, will
        /// not be affected.
        /// </summary>
        public virtual void StopMovingVertical()
        {
            SetVelocity(new Vector2(Velocity.X, 0));
        }

        /// <summary>
        /// Updates the character's state.
        /// </summary>
        void UpdateState()
        {
            var newHeading = DirectionHelper.FromVector(Velocity);
            if (newHeading.HasValue)
                _heading = newHeading.Value;
        }
    }
}

#endif