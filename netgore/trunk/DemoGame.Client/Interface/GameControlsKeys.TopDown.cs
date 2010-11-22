using System.Linq;

#if !TOPDOWN
#pragma warning disable 1587
#endif

#if TOPDOWN

using System;
using System.Collections.Generic;
using System.Text;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    public static partial class GameControlsKeys
    {
        static GameControlKeys _moveDown;
        static GameControlKeys _moveStopHorizontal;
        static GameControlKeys _moveStopVertical;
        static GameControlKeys _moveUp;

        /// <summary>
        /// Initializes the perspective-specific <see cref="GameControlKeys"/>.
        /// </summary>
        static void InitPerspectiveSpecificKeys()
        {
            _moveUp = new GameControlKeys("Move Up", SKC("MoveUp"), SKC("MoveDown"));
            _moveDown = new GameControlKeys("Move Down", SKC("MoveDown"), SKC("MoveUp"));
            _moveStopHorizontal = new GameControlKeys("Move Stop Horizontal", null, _moveLeft.KeysDown.Concat(_moveRight.KeysDown));
            _moveStopVertical = new GameControlKeys("Move Stop Vertical", null, _moveUp.KeysDown.Concat(_moveDown.KeysDown));
            _moveStop = new GameControlKeys("Move Stop", null,
                _moveLeft.KeysDown.Concat(_moveRight.KeysDown).Concat(_moveUp.KeysDown).Concat(_moveDown.KeysDown));
        }

        public static GameControlKeys MoveDown
        {
            get { return _moveDown; }
        }

        public static GameControlKeys MoveStopHorizontal
        {
            get { return _moveStopHorizontal; }
        }

        public static GameControlKeys MoveStopVertical
        {
            get { return _moveStopVertical; }
        }

        public static GameControlKeys MoveUp
        {
            get { return _moveUp; }
        }
    }
}

#endif