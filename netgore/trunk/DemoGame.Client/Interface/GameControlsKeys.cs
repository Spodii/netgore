using System.Linq;
using Microsoft.Xna.Framework.Input;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    /// <summary>
    /// Contains all of the <see cref="GameControlKeys"/> used for the <see cref="GameControl"/>s.
    /// </summary>
    public static class GameControlsKeys
    {
        static readonly GameControlKeys _attack;
#if !TOPDOWN
        static readonly GameControlKeys _jump;
#endif
#if TOPDOWN
        static readonly GameControlKeys _moveDown;
#endif
        static readonly GameControlKeys _moveLeft;
        static readonly GameControlKeys _moveRight;
        static readonly GameControlKeys _moveStop;
#if TOPDOWN
        static readonly GameControlKeys _moveStopHorizontal;
#endif
#if TOPDOWN
        static readonly GameControlKeys _moveStopVertical;
#endif
#if TOPDOWN
        static readonly GameControlKeys _moveUp;
#endif
        static readonly GameControlKeys _pickUp;
        static readonly GameControlKeys _talkToNPC;
        static readonly GameControlKeys _useShop;
        static readonly GameControlKeys _useWorld;

        /// <summary>
        /// Initializes the <see cref="GameControlsKeys"/> class.
        /// </summary>
        static GameControlsKeys()
        {
            // Create the GameControlKeys with the default keys
            _moveLeft = new GameControlKeys("Move Left", Keys.Left, Keys.Right);
            _moveRight = new GameControlKeys("Move Right", Keys.Right, Keys.Left);

#if TOPDOWN
            _moveUp = new GameControlKeys("Move Up", Keys.Up, Keys.Down);
            _moveDown = new GameControlKeys("Move Down", Keys.Down, Keys.Up);
            _moveStopHorizontal = new GameControlKeys("Move Stop Horizontal", null, _moveLeft.KeysDown.Concat(_moveRight.KeysDown));
            _moveStopVertical = new GameControlKeys("Move Stop Vertical", null, _moveUp.KeysDown.Concat(_moveDown.KeysDown));
            _moveStop = new GameControlKeys("Move Stop", null,
                _moveLeft.KeysDown.Concat(_moveRight.KeysDown).Concat(_moveUp.KeysDown).Concat(_moveDown.KeysDown));
#else
            _jump = new GameControlKeys("Jump", Keys.Up);
            _moveStop = new GameControlKeys("Move Stop", null, _moveLeft.KeysDown.Concat(_moveRight.KeysDown));
#endif

            _attack = new GameControlKeys("Attack", Keys.LeftControl);
            _useWorld = new GameControlKeys("Use World", Keys.LeftAlt);
            _useShop = new GameControlKeys("Use Shop", Keys.LeftAlt);
            _talkToNPC = new GameControlKeys("Talk To NPC", Keys.LeftAlt);
            _pickUp = new GameControlKeys("Pick Up", Keys.Space);
        }

        public static GameControlKeys Attack
        {
            get { return _attack; }
        }

#if !TOPDOWN
        public static GameControlKeys Jump
        {
            get { return _jump; }
        }
#endif

#if TOPDOWN
        public static GameControlKeys MoveDown
        {
            get { return _moveDown; }
        }
#endif

        public static GameControlKeys MoveLeft
        {
            get { return _moveLeft; }
        }

        public static GameControlKeys MoveRight
        {
            get { return _moveRight; }
        }

        public static GameControlKeys MoveStop
        {
            get { return _moveStop; }
        }

#if TOPDOWN
        public static GameControlKeys MoveStopHorizontal
        {
            get { return _moveStopHorizontal; }
        }
#endif

#if TOPDOWN
        public static GameControlKeys MoveStopVertical
        {
            get { return _moveStopVertical; }
        }
#endif

#if TOPDOWN
        public static GameControlKeys MoveUp
        {
            get { return _moveUp; }
        }
#endif

        public static GameControlKeys PickUp
        {
            get { return _pickUp; }
        }

        public static GameControlKeys TalkToNPC
        {
            get { return _talkToNPC; }
        }

        public static GameControlKeys UseShop
        {
            get { return _useShop; }
        }

        public static GameControlKeys UseWorld
        {
            get { return _useWorld; }
        }
    }
}