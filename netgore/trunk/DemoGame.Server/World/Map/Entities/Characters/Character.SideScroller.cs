using System.Linq;
using DemoGame.Server.Properties;
using SFML.Graphics;

#if TOPDOWN
#pragma warning disable 1587
#endif

#if !TOPDOWN

namespace DemoGame.Server
{
    public partial class Character
    {
        static readonly float _jumpVelocity = ServerSettings.Default.CharacterJumpVelocity;

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

            SetVelocity(Velocity + new Vector2(0.0f, _jumpVelocity));
        }
    }
}

#endif

#if TOPDOWN
#pragma warning restore 1587
#endif