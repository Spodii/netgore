using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame;
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

        public UserInfo UserInfo
        {
            get { return _userInfo; }
        }

        public StatsForm(UserInfo userInfo, Control parent)
            : base(parent.GUIManager, "Stats", Vector2.Zero, new Vector2(225, 500), parent)
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
            new UserInfoLabel(pos, this, title, valueHandler);
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

            public RaiseStatPB(Vector2 position, ISprite sprite, StatsForm parent, StatType statType)
                : base(position, sprite, parent)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");
                if (sprite == null)
                    throw new ArgumentNullException("sprite");

                _statsForm = parent;
                _statType = statType;
            }

            protected override void UpdateControl(int currentTime)
            {
                base.UpdateControl(currentTime);
                IsVisible = CanRaiseStat;
            }
        }

        class StatLabel : Label
        {
            readonly StatsForm _statsForm;
            readonly StatType _statType;

            public StatLabel(StatsForm statsForm, StatType statType, Vector2 pos) : this(statsForm, statType, pos, statsForm)
            {
            }

            StatLabel(StatsForm statsForm, StatType statType, Vector2 pos, Control parent) : base(string.Empty, pos, parent)
            {
                if (statsForm == null)
                    throw new ArgumentNullException("statsForm");

                _statsForm = statsForm;
                _statType = statType;
            }

            protected override void DrawControl(SpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);

                int baseValue;
                if (!_statsForm.UserInfo.BaseStats.TryGetStatValue(_statType, out baseValue))
                    baseValue = 0;

                int modValue;
                if (!_statsForm.UserInfo.ModStats.TryGetStatValue(_statType, out modValue))
                    modValue = 0;

                Text = _statType + ": " + baseValue + " (" + modValue + ")";
            }
        }

        class UserInfoLabel : Label
        {
            readonly StatsForm _statsForm;
            readonly string _title;
            readonly UserInfoLabelValueHandler _valueHandler;

            public UserInfoLabel(Vector2 pos, StatsForm parent, string title, UserInfoLabelValueHandler valueHandler)
                : base(string.Empty, pos, parent)
            {
                if (valueHandler == null)
                    throw new ArgumentNullException("valueHandler");

                _title = title;
                _statsForm = parent;
                _valueHandler = valueHandler;
            }

            protected override void DrawControl(SpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);
                Text = _title + ": " + _valueHandler(_statsForm.UserInfo);
            }
        }

        delegate string UserInfoLabelValueHandler(UserInfo userInfo);
    }
}