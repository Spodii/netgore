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
    /// A <see cref="Form"/> containing the stats for a user.
    /// </summary>
    public class StatsForm : Form
    {
        delegate string UserInfoLabelValueHandler(UserInfo userInfo);

        const float _xOffset = 21;

        readonly Grh _addStatGrh;
        readonly UserInfo _userInfo;

        float _yOffset = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsForm"/> class.
        /// </summary>
        /// <param name="userInfo">The user info.</param>
        /// <param name="parent">The parent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="userInfo" /> is <c>null</c>.</exception>
        public StatsForm(UserInfo userInfo, Control parent) : base(parent, Vector2.Zero, new Vector2(225, 275))
        {
            if (userInfo == null)
                throw new ArgumentNullException("userInfo");

            _addStatGrh = new Grh(GrhInfo.GetData("GUI", "AddStat"));

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
        public event TypedEventHandler<StatsForm, EventArgs<StatType>> RequestRaiseStat;

        /// <summary>
        /// Gets the <see cref="UserInfo"/> that the <see cref="StatsForm"/> is displaying the information for.
        /// </summary>
        public UserInfo UserInfo
        {
            get { return _userInfo; }
        }

        /// <summary>
        /// Adds a line break to the label positioning offset.
        /// </summary>
        void AddLine()
        {
            _yOffset += Font.GetLineSpacing();
        }

        /// <summary>
        /// Creates a label (along with a stat raising button when applicable) for a <see cref="StatType"/>.
        /// </summary>
        /// <param name="statType">The <see cref="StatType"/>.</param>
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

        /// <summary>
        /// Adds a <see cref="UserInfoLabel"/>.
        /// </summary>
        /// <param name="title">The label's title.</param>
        /// <param name="valueHandler">The <see cref="UserInfoLabelValueHandler"/> for getting the value to display for the label.</param>
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

        /// <summary>
        /// Handles the Clicked event of the StatPB control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void StatPB_Clicked(object sender, MouseButtonEventArgs e)
        {
            var statPB = (RaiseStatPB)sender;

            // Ensure the stat can be raised
            if (!statPB.CanRaiseStat)
                return;

            if (RequestRaiseStat != null)
                RequestRaiseStat.Raise(this, EventArgsHelper.Create(statPB.StatType));
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
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

        /// <summary>
        /// The <see cref="PictureBox"/> for the button to raise a stat.
        /// </summary>
        class RaiseStatPB : PictureBox
        {
            readonly StatType _statType;
            readonly StatsForm _statsForm;

            /// <summary>
            /// Initializes a new instance of the <see cref="RaiseStatPB"/> class.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="sprite">The sprite.</param>
            /// <param name="parent">The parent.</param>
            /// <param name="statType">Type of the stat.</param>
            /// <exception cref="ArgumentNullException"><paramref name="parent" /> is <c>null</c>.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="sprite" /> is <c>null</c>.</exception>
            public RaiseStatPB(Vector2 position, ISprite sprite, StatsForm parent, StatType statType)
                : base(parent, position, sprite.Size)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");
                if (sprite == null)
                    throw new ArgumentNullException("sprite");

                Sprite = sprite;

                _statsForm = parent;
                _statType = statType;
            }

            /// <summary>
            /// Gets if this stat can be raised.
            /// </summary>
            public bool CanRaiseStat
            {
                get { return Points >= StatCost; }
            }

            /// <summary>
            /// Gets the number of points the user has available to spend.
            /// </summary>
            int Points
            {
                get { return _statsForm.UserInfo.StatPoints; }
            }

            /// <summary>
            /// Gets the cost of raising this stat.
            /// </summary>
            int StatCost
            {
                get { return GameData.StatCost(StatLevel); }
            }

            /// <summary>
            /// Gets the current level of this stat.
            /// </summary>
            int StatLevel
            {
                get { return Stats[_statType]; }
            }

            /// <summary>
            /// Gets the <see cref="StatType"/> that this <see cref="RaiseStatPB"/> is for raising.
            /// </summary>
            public StatType StatType
            {
                get { return _statType; }
            }

            /// <summary>
            /// Gets the collection of the user's base stats.
            /// </summary>
            StatCollection<StatType> Stats
            {
                get { return _statsForm.UserInfo.BaseStats; }
            }

            /// <summary>
            /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
            /// not visible.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(TickCount currentTime)
            {
                base.UpdateControl(currentTime);

                IsVisible = CanRaiseStat;
            }
        }

        /// <summary>
        /// A <see cref="Label"/> for displaying the name, base value, and mod values of a stat.
        /// </summary>
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

            /// <summary>
            /// Initializes a new instance of the <see cref="StatLabel"/> class.
            /// </summary>
            /// <param name="statsForm">The stats form.</param>
            /// <param name="statType">Type of the stat.</param>
            /// <param name="pos">The position.</param>
            /// <exception cref="ArgumentNullException"><paramref name="statsForm" /> is <c>null</c>.</exception>
            public StatLabel(StatsForm statsForm, StatType statType, Vector2 pos) : base(statsForm, pos)
            {
                if (statsForm == null)
                    throw new ArgumentNullException("statsForm");

                _statsForm = statsForm;
                _statType = statType;
            }

            /// <summary>
            /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
            /// not visible.
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

        /// <summary>
        /// A <see cref="Label"/> used to display misc information that is regularly grabbed from a delegate.
        /// </summary>
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

            /// <summary>
            /// Initializes a new instance of the <see cref="UserInfoLabel"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="pos">The position.</param>
            /// <param name="title">The title.</param>
            /// <param name="valueHandler">The <see cref="UserInfoLabelValueHandler"/> describing what value to grab and how to grab it.</param>
            /// <exception cref="ArgumentNullException"><paramref name="valueHandler" /> is <c>null</c>.</exception>
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
            /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
            /// not visible.
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