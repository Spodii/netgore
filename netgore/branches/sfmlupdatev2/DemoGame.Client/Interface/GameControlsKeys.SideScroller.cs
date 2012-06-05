#if TOPDOWN
#pragma warning disable 1587
#endif

#if !TOPDOWN

using System.Linq;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    public static partial class GameControlsKeys
    {
        static GameControlKeys _jump;

        public static GameControlKeys Jump
        {
            get { return _jump; }
        }

        /// <summary>
        /// Initializes the perspective-specific <see cref="GameControlKeys"/>.
        /// </summary>
        static void InitPerspectiveSpecificKeys()
        {
            _jump = new GameControlKeys("Jump", SKC("MoveUp"));
            _moveStop = new GameControlKeys("Move Stop", null, _moveLeft.KeysDown.Concat(_moveRight.KeysDown));
        }
    }
}

#endif