using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

#if TOPDOWN
#pragma warning disable 1587
#endif

#if !TOPDOWN

namespace DemoGame.Server
{
    public partial class Character
    {
        /// <summary>
        /// Makes the Character jump if <see cref="Character.CanJump"/> is true. If <see cref="Character.CanJump"/> is false,
        /// this will do nothing.
        /// </summary>
        public void Jump()
        {
            if (!IsAlive)
                return;

            if (!CanJump)
                return;

            if (_skillCaster.IsCastingSkill)
                return;

            SetVelocity(Velocity + new Vector2(0.0f, -0.48f)); // TODO: Put jump velocity in server configs
        }
    }
}

#endif

#if TOPDOWN
#pragma warning restore 1587
#endif