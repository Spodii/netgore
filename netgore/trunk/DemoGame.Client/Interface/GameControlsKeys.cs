using System.Linq;
using DemoGame.Client.Properties;
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
        static readonly GameControlKeys _quickBarItem0;
        static readonly GameControlKeys _quickBarItem1;
        static readonly GameControlKeys _quickBarItem2;
        static readonly GameControlKeys _quickBarItem3;
        static readonly GameControlKeys _quickBarItem4;
        static readonly GameControlKeys _quickBarItem5;
        static readonly GameControlKeys _quickBarItem6;
        static readonly GameControlKeys _quickBarItem7;
        static readonly GameControlKeys _quickBarItem8;
        static readonly GameControlKeys _quickBarItem9;

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
        /// Helper method to create a <see cref="SettingsKeyCodeReference"/>.
        /// </summary>
        /// <param name="settingName">The setting name for the key, without the Keys_ prefix.</param>
        /// <returns>The <see cref="IKeyCodeReference"/> instance.</returns>
        static IKeyCodeReference SKC(string settingName)
        {
            const string prefix = "Keys_";
            return SettingsKeyCodeReference.Create(ClientSettings.Default, prefix + settingName);
        }

        /// <summary>
        /// Initializes the <see cref="GameControlsKeys"/> class.
        /// </summary>
        static GameControlsKeys()
        {
            // Create the GameControlKeys with the default keys
            _moveLeft = new GameControlKeys("Move Left", SKC("MoveLeft"), SKC("MoveRight"));
            _moveRight = new GameControlKeys("Move Right", SKC("MoveRight"), SKC("MoveLeft"));

#if TOPDOWN
            _moveUp = new GameControlKeys("Move Up", SKC("MoveUp"), SKC("MoveDown"));
            _moveDown = new GameControlKeys("Move Down", SKC("MoveDown"), SKC("MoveUp"));
            _moveStopHorizontal = new GameControlKeys("Move Stop Horizontal", null, _moveLeft.KeysDown.Concat(_moveRight.KeysDown));
            _moveStopVertical = new GameControlKeys("Move Stop Vertical", null, _moveUp.KeysDown.Concat(_moveDown.KeysDown));
            _moveStop = new GameControlKeys("Move Stop", null,
                _moveLeft.KeysDown.Concat(_moveRight.KeysDown).Concat(_moveUp.KeysDown).Concat(_moveDown.KeysDown));
#else
            _jump = new GameControlKeys("Jump", SKC("MoveUp"));
            _moveStop = new GameControlKeys("Move Stop", null, _moveLeft.KeysDown.Concat(_moveRight.KeysDown));
#endif

            _attack = new GameControlKeys("Attack", SKC("Attack"));
            _useWorld = new GameControlKeys("Use World", SKC("UseWorld"));
            _useShop = new GameControlKeys("Use Shop", SKC("UseShop"));
            _talkToNPC = new GameControlKeys("Talk To NPC", SKC("TalkToNPC"));
            _pickUp = new GameControlKeys("Pick Up", SKC("PickUp"));

            _emoteEllipsis = new GameControlKeys("Emote Ellipsis", SKC("EmoteEllipsis"));
            _emoteExclamation = new GameControlKeys("Emote Exclamation", SKC("EmoteExclamation"));
            _emoteHeartbroken = new GameControlKeys("Emote Heartbroken", SKC("EmoteHeartbroken"));
            _emoteHearts = new GameControlKeys("Emote Hearts", SKC("EmoteHearts"));
            _emoteMeat = new GameControlKeys("Emote Meat", SKC("EmoteMeat"));
            _emoteQuestion = new GameControlKeys("Emote Question", SKC("EmoteQuestion"));
            _emoteSweat = new GameControlKeys("Emote Sweat", SKC("EmoteSweat"));

            _quickBarItem0 = new GameControlKeys("Quick bar item 0", null, null, SKC("QuickBarItem0"));
            _quickBarItem1 = new GameControlKeys("Quick bar item 1", null, null, SKC("QuickBarItem1"));
            _quickBarItem2 = new GameControlKeys("Quick bar item 2", null, null, SKC("QuickBarItem2"));
            _quickBarItem3 = new GameControlKeys("Quick bar item 3", null, null, SKC("QuickBarItem3"));
            _quickBarItem4 = new GameControlKeys("Quick bar item 4", null, null, SKC("QuickBarItem4"));
            _quickBarItem5 = new GameControlKeys("Quick bar item 5", null, null, SKC("QuickBarItem5"));
            _quickBarItem6 = new GameControlKeys("Quick bar item 6", null, null, SKC("QuickBarItem6"));
            _quickBarItem7 = new GameControlKeys("Quick bar item 7", null, null, SKC("QuickBarItem7"));
            _quickBarItem8 = new GameControlKeys("Quick bar item 8", null, null, SKC("QuickBarItem8"));
            _quickBarItem9 = new GameControlKeys("Quick bar item 9", null, null, SKC("QuickBarItem9"));
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

        public static GameControlKeys QuickBarItem0
        {
            get { return _quickBarItem0; }
        }

        public static GameControlKeys QuickBarItem1
        {
            get { return _quickBarItem1; }
        }

        public static GameControlKeys QuickBarItem2
        {
            get { return _quickBarItem2; }
        }

        public static GameControlKeys QuickBarItem3
        {
            get { return _quickBarItem3; }
        }

        public static GameControlKeys QuickBarItem4
        {
            get { return _quickBarItem4; }
        }

        public static GameControlKeys QuickBarItem5
        {
            get { return _quickBarItem5; }
        }

        public static GameControlKeys QuickBarItem6
        {
            get { return _quickBarItem6; }
        }

        public static GameControlKeys QuickBarItem7
        {
            get { return _quickBarItem7; }
        }

        public static GameControlKeys QuickBarItem8
        {
            get { return _quickBarItem8; }
        }

        public static GameControlKeys QuickBarItem9
        {
            get { return _quickBarItem9; }
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