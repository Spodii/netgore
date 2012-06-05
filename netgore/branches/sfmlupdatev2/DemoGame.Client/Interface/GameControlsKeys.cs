using System.Collections.Generic;
using System.Linq;
using DemoGame.Client.Properties;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    /// <summary>
    /// Contains all of the <see cref="GameControlKeys"/> used for the <see cref="GameControl"/>s.
    /// </summary>
    public static partial class GameControlsKeys
    {
        static readonly GameControlKeys _attack;
        static readonly GameControlKeys _emoteEllipsis;
        static readonly GameControlKeys _emoteExclamation;
        static readonly GameControlKeys _emoteHeartbroken;
        static readonly GameControlKeys _emoteHearts;
        static readonly GameControlKeys _emoteMeat;
        static readonly GameControlKeys _emoteQuestion;
        static readonly GameControlKeys _emoteSweat;
        static readonly GameControlKeys _moveLeft;
        static readonly GameControlKeys _moveRight;
        static readonly GameControlKeys _pickUp;
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
        static readonly GameControlKeys _talkToNPC;
        static readonly GameControlKeys _useShop;
        static readonly GameControlKeys _useWorld;
        static readonly GameControlKeys _windowEquipped;
        static readonly GameControlKeys _windowGuild;
        static readonly GameControlKeys _windowInventory;
        static readonly GameControlKeys _windowSkills;
        static readonly GameControlKeys _windowStats;

        static GameControlKeys _moveStop;

        /// <summary>
        /// Initializes the <see cref="GameControlsKeys"/> class.
        /// </summary>
        static GameControlsKeys()
        {
            // Create the GameControlKeys with the default keys
            _moveLeft = new GameControlKeys("Move Left", SKC("MoveLeft"), SKC("MoveRight"));
            _moveRight = new GameControlKeys("Move Right", SKC("MoveRight"), SKC("MoveLeft"));

            _attack = new GameControlKeys("Attack", SKC("Attack"));
            _useWorld = new GameControlKeys("Use World", SKC("UseWorld"));
            _useShop = new GameControlKeys("Use Shop", SKC("UseShop"));
            _talkToNPC = new GameControlKeys("Talk To NPC", SKC("TalkToNPC"));
            _pickUp = new GameControlKeys("Pick Up", SKC("PickUp"));

            var emoteKeysDown = SKCs("Emote_Modifier1", "Emote_Modifier2");
            _emoteEllipsis = new GameControlKeys("Emote Ellipsis", emoteKeysDown, null, SKCs("EmoteEllipsis"));
            _emoteExclamation = new GameControlKeys("Emote Exclamation", emoteKeysDown, null, SKCs("EmoteExclamation"));
            _emoteHeartbroken = new GameControlKeys("Emote Heartbroken", emoteKeysDown, null, SKCs("EmoteHeartbroken"));
            _emoteHearts = new GameControlKeys("Emote Hearts", emoteKeysDown, null, SKCs("EmoteHearts"));
            _emoteMeat = new GameControlKeys("Emote Meat", emoteKeysDown, null, SKCs("EmoteMeat"));
            _emoteQuestion = new GameControlKeys("Emote Question", emoteKeysDown, null, SKCs("EmoteQuestion"));
            _emoteSweat = new GameControlKeys("Emote Sweat", emoteKeysDown, null, SKCs("EmoteSweat"));

            var quickBarKeysDown = SKCs("QuickBarItem_Modifier1", "QuickBarItem_Modifier2");
            _quickBarItem0 = new GameControlKeys("Quick bar item 0", quickBarKeysDown, null, SKCs("QuickBarItem0"));
            _quickBarItem1 = new GameControlKeys("Quick bar item 1", quickBarKeysDown, null, SKCs("QuickBarItem1"));
            _quickBarItem2 = new GameControlKeys("Quick bar item 2", quickBarKeysDown, null, SKCs("QuickBarItem2"));
            _quickBarItem3 = new GameControlKeys("Quick bar item 3", quickBarKeysDown, null, SKCs("QuickBarItem3"));
            _quickBarItem4 = new GameControlKeys("Quick bar item 4", quickBarKeysDown, null, SKCs("QuickBarItem4"));
            _quickBarItem5 = new GameControlKeys("Quick bar item 5", quickBarKeysDown, null, SKCs("QuickBarItem5"));
            _quickBarItem6 = new GameControlKeys("Quick bar item 6", quickBarKeysDown, null, SKCs("QuickBarItem6"));
            _quickBarItem7 = new GameControlKeys("Quick bar item 7", quickBarKeysDown, null, SKCs("QuickBarItem7"));
            _quickBarItem8 = new GameControlKeys("Quick bar item 8", quickBarKeysDown, null, SKCs("QuickBarItem8"));
            _quickBarItem9 = new GameControlKeys("Quick bar item 9", quickBarKeysDown, null, SKCs("QuickBarItem9"));

            var windowKeysDown = SKCs("Window_Modifier1", "Window_Modifier2");
            _windowStats = new GameControlKeys("Stats window", windowKeysDown, null, SKCs("Window_Stats"));
            _windowSkills = new GameControlKeys("Skills window", windowKeysDown, null, SKCs("Window_Skills"));
            _windowInventory = new GameControlKeys("Inventory window", windowKeysDown, null, SKCs("Window_Inventory"));
            _windowEquipped = new GameControlKeys("Equipped window", windowKeysDown, null, SKCs("Window_Equipped"));
            _windowGuild = new GameControlKeys("Guild window", windowKeysDown, null, SKCs("Window_Guild"));

            // Initialize the keys specific to the perspective being used
            InitPerspectiveSpecificKeys();
        }

        public static GameControlKeys Attack
        {
            get { return _attack; }
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

        public static GameControlKeys WindowEquipped
        {
            get { return _windowEquipped; }
        }

        public static GameControlKeys WindowGuild
        {
            get { return _windowGuild; }
        }

        public static GameControlKeys WindowInventory
        {
            get { return _windowInventory; }
        }

        public static GameControlKeys WindowSkills
        {
            get { return _windowSkills; }
        }

        public static GameControlKeys WindowStats
        {
            get { return _windowStats; }
        }

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
        /// Helper method to create a <see cref="SettingsKeyCodeReference"/>.
        /// </summary>
        /// <param name="settingName">The setting name for the key, without the Keys_ prefix.</param>
        /// <returns>The <see cref="IKeyCodeReference"/> instance.</returns>
        static IEnumerable<IKeyCodeReference> SKCs(string settingName)
        {
            return new IKeyCodeReference[] { SKC(settingName) };
        }

        /// <summary>
        /// Helper method to create a <see cref="SettingsKeyCodeReference"/>.
        /// </summary>
        /// <param name="settingName">The setting name for the key, without the Keys_ prefix.</param>
        /// <returns>The <see cref="IKeyCodeReference"/> instance.</returns>
        static IEnumerable<IKeyCodeReference> SKCs(params string[] settingName)
        {
            var ret = new List<IKeyCodeReference>();
            foreach (var sn in settingName)
            {
                ret.Add(SKC(sn));
            }

            return ret.Distinct().ToArray();
        }
    }
}