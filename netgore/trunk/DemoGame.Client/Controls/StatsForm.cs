using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;

// FUTURE: This is very ineffeciently and poorly done

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
        readonly Grh _addStatGrh = new Grh(GrhInfo.GetDatas("GUI", "AddStat"));
        readonly CharacterStats _charStats;
        float _yOffset = 0;

        /// <summary>
        /// Notifies listeners when a Stat is requested to be raised.
        /// </summary>
        public event RaiseStatHandler OnRaiseStat;

        /// <summary>
        /// Gets the CharacterStats displayed.
        /// </summary>
        public CharacterStats CharacterStats
        {
            get { return _charStats; }
        }

        public StatsForm(CharacterStats charStats, Control parent)
            : base(parent.GUIManager, "Stats", Vector2.Zero, new Vector2(225, 425), parent)
        {
            if (charStats == null)
                throw new ArgumentNullException("charStats");

            _charStats = charStats;

            _yOffset = 2;

            NewStatLabel(charStats.GetStat(StatType.Level));
            NewStatLabel(charStats.GetStat(StatType.Cash));
            NewStatLabel(charStats.GetStat(StatType.Exp));
            NewPointsLabel();

            AddLine();

            NewStatLabel(charStats.GetStat(StatType.MinHit));
            NewStatLabel(charStats.GetStat(StatType.MaxHit));
            NewStatLabel(charStats.GetStat(StatType.Defence));

            AddLine();

            NewStatLabel(charStats.GetStat(StatType.Str));
            NewStatLabel(charStats.GetStat(StatType.Agi));
            NewStatLabel(charStats.GetStat(StatType.Dex));
            NewStatLabel(charStats.GetStat(StatType.Int));
            NewStatLabel(charStats.GetStat(StatType.Bra));

            AddLine();

            NewStatLabel(charStats.GetStat(StatType.WS));
            NewStatLabel(charStats.GetStat(StatType.Armor));
            NewStatLabel(charStats.GetStat(StatType.Acc));
            NewStatLabel(charStats.GetStat(StatType.Evade));
            NewStatLabel(charStats.GetStat(StatType.Perc));
            NewStatLabel(charStats.GetStat(StatType.Regen));
            NewStatLabel(charStats.GetStat(StatType.Recov));
            NewStatLabel(charStats.GetStat(StatType.Tact));
            NewStatLabel(charStats.GetStat(StatType.Imm));
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

        void NewStatLabel(IStat stat)
        {
            AddLine();

            Vector2 pos = new Vector2(_xOffset, _yOffset);
            new StatLabel(pos, stat, this);

            // Add the mod stat where applicable
            try
            {
                StatType modStat = stat.StatType.GetMod();
                new StatLabel(new Vector2(pos.X + 70, pos.Y), CharacterStats.GetStat(modStat), this);
            }
            catch (ArgumentException)
            {
            }

            // Add the stat raise button
            if (CharacterStats.RaiseableStats.Contains(stat.StatType))
            {
                RaiseStatPB statPB = new RaiseStatPB(pos - new Vector2(22, 0), _addStatGrh, this, stat.StatType);
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

        protected override void UpdateControl()
        {
            base.UpdateControl();

            if (!IsVisible)
                return;

            if (CharacterStats == null)
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
                get { return _statsForm.CharacterStats.Points; }
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
                get { return Stats.Points; }
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
                get { return _statsForm.CharacterStats; }
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

            protected override void UpdateControl()
            {
                base.UpdateControl();
                IsVisible = CanRaiseStat;
            }
        }

        class StatLabel : Label
        {
            public StatLabel(Vector2 pos, IStat stat, Control parent) : base(string.Empty, pos, parent)
            {
                if (stat == null)
                    throw new ArgumentNullException("stat");

                Tag = stat;
            }

            protected override void DrawControl(SpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);
                Text = Tag.ToString();
            }
        }
    }
}