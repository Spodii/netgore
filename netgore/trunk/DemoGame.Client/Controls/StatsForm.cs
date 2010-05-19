using System;
using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.Stats;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// Handles a request to raise a stat from the StatsForm.
    /// </summary>
    /// <param name="statsForm">StatsForm that the event came from.</param>
    /// <param name="statType">Type of the stat requested to be raise.</param>
    delegate void RaiseStatHandler(StatsForm statsForm, StatType statType);

    /// <summary>
    /// A <see cref="Form"/> containing the stats for a user.
    /// </summary>
    class StatsForm : Form
    {
        delegate string UserInfoLabelValueHandler(UserInfo userInfo);

        const float _xOffset = 21;
        readonly Grh _addStatGrh = new Grh(GrhInfo.GetData("GUI", "AddStat"));
        readonly UserInfo _userInfo;

        float _yOffset = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsForm"/> class.
        /// </summary>
        /// <param name="userInfo">The user info.</param>
        /// <param name="parent">The parent.</param>
        public StatsForm(UserInfo userInfo, Control parent) : base(parent, Vector2.Zero, new Vector2(225, 275))
        {
            if (userInfo == null)
                throw new ArgumentNullException("userInfo");

            _userInfo = userInfo;

            _yOffset = 2;

            NewUserInfoLabel("Level", x => x.Level.ToString());
            NewUserInfoLabel("Exp", x => x.Exp.ToString());
            NewUserInfoLabel("StatPoints", x => x.StatPoints.ToString());

            AddLine();

            NewUserInfoLabel("HP", x => x.HP + " / " + x.ModStats[StatType.MaxHP]);
            NewUserInfoLabel("MP", x => x.MP + " / " + x.ModStats[StatType.MaxMP]);

            AddLine();

            NewStatLabel(StatType.MinHit);
            NewStatLabel(StatType.MaxHit);
            NewStatLabel(StatType.Defence);

            AddLine();

            NewStatLabel(StatType.Str);
            NewStatLabel(StatType.Agi);
            NewStatLabel(StatType.Int);
        }

        /// <summary>
        /// Notifies listeners when a Stat is requested to be raised.
        /// </summary>
        public event RaiseStatHandler RequestRaiseStat;

        public UserInfo UserInfo
        {
            get { return _userInfo; }
        }

        void AddLine()
        {
            _yOffset += Font.GetLineSpacing();
        }

        void NewStatLabel(StatType statType)
        {
            AddLine();

            // Create the stat label
            var pos = new Vector2(_xOffset, _yOffset);
            new StatLabel(this, statType, pos);

            // Add the stat raise button
            if (StatTypeHelper.RaisableStats.Contains(statType))
            {
                var statPB = new RaiseStatPB(pos - new Vector2(22, 0), _addStatGrh, this, statType);
                statPB.Clicked += StatPB_Clicked;
            }
        }

        void NewUserInfoLabel(string title, UserInfoLabelValueHandler valueHandler)
        {
            AddLine();
            var pos = new Vector2(_xOffset, _yOffset);
            new UserInfoLabel(this, pos, title, valueHandler);
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Stats";
        }

        void StatPB_Clicked(object sender, MouseButtonEventArgs e)
        {
            var statPB = (RaiseStatPB)sender;

            // Ensure the stat can be raised
            if (!statPB.CanRaiseStat)
                return;

            if (RequestRaiseStat == null)
                return;

            RequestRaiseStat(this, statPB.StatType);
        }

        protected override void UpdateControl(TickCount currentTime)
        {
            base.UpdateControl(currentTime);

            if (!IsVisible)
                return;

            if (UserInfo.BaseStats == null)
            {
                Debug.Fail("CharacterStats is null.");
                return;
            }
        }

        class RaiseStatPB : PictureBox
        {
            readonly StatType _statType;
            readonly StatsForm _statsForm;

            public RaiseStatPB(Vector2 position, ISprite sprite, StatsForm parent, StatType statType)
                : base(parent, position, sprite.Size)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");
                if (sprite == null)
                    throw new ArgumentNullException("sprite");

                _statsForm = parent;
                _statType = statType;
            }

            public bool CanRaiseStat
            {
                get { return Points >= StatCost; }
            }

            int Points
            {
                get { return _statsForm.UserInfo.StatPoints; }
            }

            int StatCost
            {
                get { return GameData.StatCost(StatLevel); }
            }

            int StatLevel
            {
                get { return Stats[_statType]; }
            }

            public StatType StatType
            {
                get { return _statType; }
            }

            StatCollection<StatType> Stats
            {
                get { return _statsForm.UserInfo.BaseStats; }
            }

            /// <summary>
            /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(TickCount currentTime)
            {
                base.UpdateControl(currentTime);

                IsVisible = CanRaiseStat;
            }
        }

        class StatLabel : Label
        {
            /// <summary>
            /// How frequently we will attempt to update the label's text.
            /// </summary>
            const int _textUpdateRate = 250;

            readonly StatType _statType;
            readonly StatsForm _statsForm;

            int _lastBaseValue = int.MinValue;
            int _lastModValue = int.MinValue;
            TickCount _lastUpdateTextTime = TickCount.MinValue;

            public StatLabel(StatsForm statsForm, StatType statType, Vector2 pos) : base(statsForm, pos)
            {
                if (statsForm == null)
                    throw new ArgumentNullException("statsForm");

                _statsForm = statsForm;
                _statType = statType;
            }

            /// <summary>
            /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(TickCount currentTime)
            {
                base.UpdateControl(currentTime);

                // Check that enough time has elapsed since the last update
                if (_lastUpdateTextTime + _textUpdateRate > currentTime)
                    return;

                _lastUpdateTextTime = currentTime;

                // Get the stat values
                int baseValue = _statsForm.UserInfo.BaseStats[_statType];
                int modValue = _statsForm.UserInfo.ModStats[_statType];

                // Check that they have changed before creating the new text
                if (_lastBaseValue == baseValue && _lastModValue == modValue)
                    return;

                _lastBaseValue = baseValue;
                _lastModValue = modValue;

                Text = _statType + ": " + baseValue + " (" + modValue + ")";
            }
        }

        class UserInfoLabel : Label
        {
            /// <summary>
            /// How frequently we will attempt to update the label's text.
            /// </summary>
            const int _textUpdateRate = 250;

            readonly StatsForm _statsForm;
            readonly string _title;
            readonly UserInfoLabelValueHandler _valueHandler;

            TickCount _lastUpdateTextTime = TickCount.MinValue;

            public UserInfoLabel(StatsForm parent, Vector2 pos, string title, UserInfoLabelValueHandler valueHandler)
                : base(parent, pos)
            {
                if (valueHandler == null)
                    throw new ArgumentNullException("valueHandler");

                _title = title;
                _statsForm = parent;
                _valueHandler = valueHandler;
            }

            /// <summary>
            /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(TickCount currentTime)
            {
                base.UpdateControl(currentTime);

                // Check that enough time has elapsed since the last update
                if (_lastUpdateTextTime + _textUpdateRate > currentTime)
                    return;

                _lastUpdateTextTime = currentTime;

                Text = _title + ": " + _valueHandler(_statsForm.UserInfo);
            }
        }
    }
}