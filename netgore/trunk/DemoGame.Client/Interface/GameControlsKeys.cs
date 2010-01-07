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
        static readonly GameControlKeys _moveLeft;
        static readonly GameControlKeys _moveRight;
        static readonly GameControlKeys _moveStop;
        static readonly GameControlKeys _pickUp;
        static readonly GameControlKeys _talkToNPC;
        static readonly GameControlKeys _useShop;
        static readonly GameControlKeys _useWorld;
        static readonly GameControlKeys _emoteEllipsis;
        static readonly GameControlKeys _emoteExclamation;
        static readonly GameControlKeys _emoteHeartbroken;
        static readonly GameControlKeys _emoteHearts;
        static readonly GameControlKeys _emoteMeat;
        static readonly GameControlKeys _emoteQuestion;
        static readonly GameControlKeys _emoteSweat;

#if !TOPDOWN
        static readonly GameControlKeys _jump;
#endif

#if TOPDOWN
        static readonly GameControlKeys _moveDown;
        static readonly GameControlKeys _moveStopHorizontal;
        static readonly GameControlKeys _moveStopVertical;
        static readonly GameControlKeys _moveUp;
#endif

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

            _emoteEllipsis = new GameControlKeys("Emote Ellipsis", Keys.D1);
            _emoteExclamation = new GameControlKeys("Emote Exclamation", Keys.D2);
            _emoteHeartbroken = new GameControlKeys("Emote Heartbroken", Keys.D3);
            _emoteHearts = new GameControlKeys("Emote Hearts", Keys.D4);
            _emoteMeat = new GameControlKeys("Emote Meat", Keys.D5);
            _emoteQuestion = new GameControlKeys("Emote Question", Keys.D6);
            _emoteSweat = new GameControlKeys("Emote Sweat", Keys.D7);
        }

        public static GameControlKeys EmoteEllipsis
        {
            get { return _emoteEllipsis; }
        }

        public static GameControlKeys EmoteExclamation
        {
            get { return _emoteExclamation; }
        }

        public static GameControlKeys EmoteHeartbroken
        {
            get { return _emoteHeartbroken; }
        }

        public static GameControlKeys EmoteHearts
        {
            get { return _emoteHearts; }
        }

        public static GameControlKeys EmoteMeat
        {
            get { return _emoteMeat; }
        }

        public static GameControlKeys EmoteQuestion
        {
            get { return _emoteQuestion; }
        }

        public static GameControlKeys EmoteSweat
        {
            get { return _emoteSweat; }
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