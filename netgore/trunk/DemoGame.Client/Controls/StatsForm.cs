using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Handles a request to raise a stat from the StatsForm.
    /// </summary>
    /// <param name="statsForm">StatsForm that the event came from.</param>
    /// <param name="statType">Type of the stat requested to be raise.</param>
    delegate void RaiseStatHandler(StatsForm statsForm, StatType statType);

    class StatsForm : Form, IRestorableSettings
    {
        const float _xOffset = 21;
        readonly Grh _addStatGrh = new Grh(GrhInfo.GetData("GUI", "AddStat"));
        readonly UserInfo _userInfo;

        float _yOffset = 0;

        /// <summary>
        /// Notifies listeners when a Stat is requested to be raised.
        /// </summary>
        public event RaiseStatHandler OnRaiseStat;

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
        /// Initializes a new instance of the <see cref="StatsForm"/> class.
        /// </summary>
        /// <param name="userInfo">The user info.</param>
        /// <param name="parent">The parent.</param>
        public StatsForm(UserInfo userInfo, Control parent)
            : base(parent, Vector2.Zero, new Vector2(225, 275))
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

        public UserInfo UserInfo
        {
            get { return _userInfo; }
        }

        void AddLine()
        {
            _yOffset += Font.LineSpacing;
        }

        void NewStatLabel(StatType statType)
        {
            AddLine();

            // Create the stat label
            Vector2 pos = new Vector2(_xOffset, _yOffset);
            new StatLabel(this, statType, pos);

            // Add the stat raise button
            if (StatTypeHelper.RaisableStats.Contains(statType))
            {
                RaiseStatPB statPB = new RaiseStatPB(pos - new Vector2(22, 0), _addStatGrh, this, statType);
                statPB.OnClick += StatPB_OnClick;
            }
        }

        void NewUserInfoLabel(string title, UserInfoLabelValueHandler valueHandler)
        {
            AddLine();
            Vector2 pos = new Vector2(_xOffset, _yOffset);
            new UserInfoLabel(this, pos, title, valueHandler);
        }

        void StatPB_OnClick(object sender, MouseClickEventArgs e)
        {
            RaiseStatPB statPB = (RaiseStatPB)sender;

            // Ensure the stat can be raised
            if (!statPB.CanRaiseStat)
                return;

            if (OnRaiseStat == null)
                return;

            OnRaiseStat(this, statPB.StatType);
        }

        protected override void UpdateControl(int currentTime)
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

        #region IRestorableSettings Members

        /// <summary>
        /// Loads the values supplied by the <paramref name="items"/> to reconstruct the settings.
        /// </summary>
        /// <param name="items">NodeItems containing the values to restore.</param>
        public void Load(IDictionary<string, string> items)
        {
            Position = new Vector2(items.AsFloat("X", Position.X), items.AsFloat("Y", Position.Y));
            IsVisible = items.AsBool("IsVisible", IsVisible);
        }

        /// <summary>
        /// Returns the key and value pairs needed to restore the settings.
        /// </summary>
        /// <returns>The key and value pairs needed to restore the settings.</returns>
        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[]
            { new NodeItem("X", Position.X), new NodeItem("Y", Position.Y), new NodeItem("IsVisible", IsVisible) };
        }

        #endregion

        class RaiseStatPB : PictureBox
        {
            readonly StatsForm _statsForm;
            readonly StatType _statType;

            public RaiseStatPB(Vector2 position, Grh sprite, StatsForm parent, StatType statType)
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

            CharacterStats Stats
            {
                get { return _statsForm.UserInfo.BaseStats; }
            }

            public StatType StatType
            {
                get { return _statType; }
            }

            /// <summary>
            /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(int currentTime)
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

            readonly StatsForm _statsForm;
            readonly StatType _statType;

            int _lastUpdateTextTime = int.MinValue;
            int _lastBaseValue = int.MinValue;
            int _lastModValue = int.MinValue;

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
            protected override void UpdateControl(int currentTime)
            {
                base.UpdateControl(currentTime);

                // Check that enough time has elapsed since the last update
                if (_lastUpdateTextTime + _textUpdateRate > currentTime)
                    return;

                _lastUpdateTextTime = currentTime;

                // Get the stat values
                int baseValue;
                if (!_statsForm.UserInfo.BaseStats.TryGetStatValue(_statType, out baseValue))
                    baseValue = 0;

                int modValue;
                if (!_statsForm.UserInfo.ModStats.TryGetStatValue(_statType, out modValue))
                    modValue = 0;

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

            int _lastUpdateTextTime = int.MinValue;

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
            protected override void UpdateControl(int currentTime)
            {
                base.UpdateControl(currentTime);

                // Check that enough time has elapsed since the last update
                if (_lastUpdateTextTime + _textUpdateRate > currentTime)
                    return;

                _lastUpdateTextTime = currentTime;

                Text = _title + ": " + _valueHandler(_statsForm.UserInfo);
            }
        }

        delegate string UserInfoLabelValueHandler(UserInfo userInfo);
    }
}