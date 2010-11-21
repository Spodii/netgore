using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

#if !TOPDOWN
#pragma warning disable 1587
#endif

#if TOPDOWN

namespace DemoGame.Server
{
    public partial class Character
    {
        /// <summary>
        /// Starts moving the character down.
        /// </summary>
        public void MoveDown()
        {
            if (!IsAlive)
                return;

            if (IsMovingDown)
                return;

            if (_skillCaster.IsCastingSkill)
                return;

            SetVelocity(new Vector2(Velocity.X, _moveSpeedVelocityCache));
        }

        /// <summary>
        /// Starts moving the character up.
        /// </summary>
        public void MoveUp()
        {
            if (!IsAlive)
                return;

            if (IsMovingUp)
                return;

            if (_skillCaster.IsCastingSkill)
                return;

            SetVelocity(new Vector2(Velocity.X, -_moveSpeedVelocityCache));
        }
    }
}

#endif

#if !TOPDOWN
#pragma warning restore 1587
#endif