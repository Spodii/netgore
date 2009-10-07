using System.Linq;
using Microsoft.Xna.Framework.Input;
using NetGore;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    /// <summary>
    /// Contains all of the <see cref="GameControlKeys"/> used for the <see cref="GameControl"/>s.
    /// </summary>
    public static class GameControlsKeys
    {
        static readonly GameControlKeys _attack;
        static readonly GameControlKeys _jump;
        static readonly GameControlKeys _moveLeft;
        static readonly GameControlKeys _moveRight;
        static readonly GameControlKeys _moveStop;
        static readonly GameControlKeys _pickUp;
        static readonly GameControlKeys _talkToNPC;
        static readonly GameControlKeys _useShop;
        static readonly GameControlKeys _useWorld;

        public static GameControlKeys Attack
        {
            get { return _attack; }
        }

        public static GameControlKeys Jump
        {
            get { return _jump; }
        }

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

        /// <summary>
        /// Initializes the <see cref="GameControlsKeys"/> class.
        /// </summary>
        static GameControlsKeys()
        {
            // Create the GameControlKeys with the default keys
            _jump = new GameControlKeys("Jump", Keys.Up);
            _moveLeft = new GameControlKeys("Move Left", Keys.Left, Keys.Right);
            _moveRight = new GameControlKeys("Move Right", Keys.Right, Keys.Left);
            _moveStop = new GameControlKeys("Attack", null, _moveLeft.KeysDown.Concat(_moveRight.KeysDown));

            _attack = new GameControlKeys("Attack", Keys.LeftControl);
            _useWorld = new GameControlKeys("Use World", Keys.LeftAlt);
            _useShop = new GameControlKeys("Use Shop", Keys.LeftAlt);
            _talkToNPC = new GameControlKeys("Talk To NPC", Keys.LeftAlt);
            _pickUp = new GameControlKeys("Pick Up", Keys.Space);
        }
    }
}