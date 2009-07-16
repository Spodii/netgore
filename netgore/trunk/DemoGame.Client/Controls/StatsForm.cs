using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        readonly CharacterStats _baseStats;
        readonly CharacterStats _modStats;
        float _yOffset = 0;

        /// <summary>
        /// Notifies listeners when a Stat is requested to be raised.
        /// </summary>
        public event RaiseStatHandler OnRaiseStat;

        public CharacterStats CharacterBaseStats
        {
            get { return _baseStats; }
        }

        public CharacterStats CharacterModStats
        {
            get { return _modStats; }
        }

        public StatsForm(CharacterStats baseStats, CharacterStats modStats, Control parent)
            : base(parent.GUIManager, "Stats", Vector2.Zero, new Vector2(225, 425), parent)
        {
            if (baseStats == null)
                throw new ArgumentNullException("baseStats");

            _baseStats = baseStats;
            _modStats = modStats;

            _yOffset = 2;

            //NewStatLabel(charStats.GetStat(StatType.Level));
            //NewStatLabel(charStats.GetStat(StatType.Cash));
            //NewStatLabel(charStats.GetStat(StatType.Exp));

            AddLine();

            NewStatLabel(StatType.MinHit);
            NewStatLabel(StatType.MaxHit);
            NewStatLabel(StatType.Defence);

            AddLine();

            NewStatLabel(StatType.Str);
            NewStatLabel(StatType.Agi);
            NewStatLabel(StatType.Dex);
            NewStatLabel(StatType.Int);
            NewStatLabel(StatType.Bra);

            AddLine();

            NewStatLabel(StatType.WS);
            NewStatLabel(StatType.Armor);
            NewStatLabel(StatType.Acc);
            NewStatLabel(StatType.Evade);
            NewStatLabel(StatType.Perc);
            NewStatLabel(StatType.Regen);
            NewStatLabel(StatType.Recov);
            NewStatLabel(StatType.Tact);
            NewStatLabel(StatType.Imm);
        }

        void AddLine()
        {
            _yOffset += Font.LineSpacing;
        }

        void NewPointsLabel()
        {
            AddLine();
            Vector2 pos = new Vector2(_xOffset, _yOffset);
            new PointsLabel(pos, this);
        }

        void NewStatLabel(StatType statType)
        {
            AddLine();
            
            // Create the stat label
            Vector2 pos = new Vector2(_xOffset, _yOffset);
            new StatLabel(this, statType, pos);

            // Add the stat raise button
            if (StatFactory.RaisableStats.Contains(statType))
            {
                RaiseStatPB statPB = new RaiseStatPB(pos - new Vector2(22, 0), _addStatGrh, this, statType);
                statPB.OnClick += StatPB_OnClick;
            }
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

            if (CharacterBaseStats == null)
            {
                Debug.Fail("CharacterStats is null.");
                return;
            }
        }

        #region IRestorableSettings Members

        public void Load(IDictionary<string, string> items)
        {
            Position = new Vector2(float.Parse(items["X"]), float.Parse(items["Y"]));
            IsVisible = bool.Parse(items["IsVisible"]);
        }

        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[]
                   { new NodeItem("X", Position.X), new NodeItem("Y", Position.Y), new NodeItem("IsVisible", IsVisible) };
        }

        #endregion

        class PointsLabel : Label
        {
            readonly StatsForm _statsForm;

            int Points
            {
                get
                {
                    // TODO: [STATS] Return the correct number of points
                    return 0;
                    //return _statsForm.CharacterStats.Points; 
                }
            }

            public PointsLabel(Vector2 pos, StatsForm parent) : base(string.Empty, pos, parent)
            {
                _statsForm = parent;
            }

            protected override void DrawControl(SpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);
                Text = string.Format("Points: {0}", Points);
            }
        }

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
                get
                {
                    // TODO: [STATS] Return the correct number of points
                    return 0;
                    //return Stats.Points; 
                }
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
                get { return _statsForm.CharacterBaseStats; }
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
            readonly StatType _statType;
            readonly StatsForm _statsForm;

            public StatLabel(StatsForm statsForm, StatType statType, Vector2 pos, Control parent)
                : base(string.Empty, pos, parent)
            {
                if (statsForm == null)
                    throw new ArgumentNullException("statsForm");

                _statsForm = statsForm;
                _statType = statType;
            }

            public StatLabel(StatsForm statsForm, StatType statType, Vector2 pos)
                : this(statsForm, statType, pos, statsForm)
            {
            }

            protected override void DrawControl(SpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);

                int baseValue;
                if (!_statsForm.CharacterBaseStats.TryGetStatValue(_statType, out baseValue))
                    baseValue = 0;

                int modValue;
                if (!_statsForm.CharacterModStats.TryGetStatValue(_statType, out modValue))
                    modValue = 0;

                Text = _statType + ": " + baseValue + " (" + modValue + ")";
            }
        }
    }
}