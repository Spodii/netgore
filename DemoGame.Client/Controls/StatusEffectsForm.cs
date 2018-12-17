using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Features.StatusEffects;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> that contains the <see cref="StatusEffect{TStatType,TStatusEffectType}"/>s that are currently
    /// applied on a character.
    /// </summary>
    public class StatusEffectsForm : Form, IGetTime
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Vector2 _iconSize = new Vector2(32, 32);
        readonly IGetTime _getTime;
        readonly StatusEffectCollection _statusEffects = new StatusEffectCollection();

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectsForm"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="position">The position.</param>
        /// <param name="getTime">The get time.</param>
        public StatusEffectsForm(Control parent, Vector2 position, IGetTime getTime) : base(parent, position, _iconSize)
        {
            IsCloseButtonVisible = false;

            _getTime = getTime;

            Border = null;
            CanFocus = false;
            CanDrag = false;
        }

        public void AddStatusEffect(StatusEffectType statusEffectType, ushort power, ushort secsLeft)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Add StatusEffect `{0}`, power: `{1}`, secsLeft: `{2}`", statusEffectType, power, secsLeft);

            _statusEffects.Add(statusEffectType, power, secsLeft, GetTime());
        }

        protected override void DrawControl(ISpriteBatch spriteBatch)
        {
            var currentTime = GetTime();

            var offset = new Vector2(-_iconSize.X, 0);

            foreach (var dase in _statusEffects.ActiveStatusEffects)
            {
                dase.Draw(spriteBatch, Position + offset, currentTime);
                offset += new Vector2(0, _iconSize.Y);
            }
        }

        public void RemoveStatusEffect(StatusEffectType statusEffectType)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Remove StatusEffect `{0}`", statusEffectType);

            _statusEffects.Remove(statusEffectType);
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Status Effects";
        }

        protected override void UpdateControl(TickCount currentTime)
        {
            base.UpdateControl(currentTime);

            _statusEffects.Update(currentTime);
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public TickCount GetTime()
        {
            return _getTime.GetTime();
        }

        #endregion

        sealed class StatusEffectCollection
        {
            const int _updateRate = 500;
            readonly List<StatusEffectCollectionItem> _statusEffects = new List<StatusEffectCollectionItem>();

            TickCount _lastUpdateTime;

            public IEnumerable<StatusEffectCollectionItem> ActiveStatusEffects
            {
                get { return _statusEffects; }
            }

            public void Add(StatusEffectType statusEffectType, ushort power, ushort secsLeft, TickCount currentTime)
            {
                Remove(statusEffectType);

                var msLeft = secsLeft * 1000;
                var life = msLeft + currentTime;

                var ase = new StatusEffectCollectionItem(statusEffectType, power, (int)life);
                _statusEffects.Add(ase);
            }

            public void Remove(StatusEffectType statusEffectType)
            {
                _statusEffects.RemoveAll(x => x.StatusEffectType == statusEffectType);
            }

            public void Update(TickCount currentTime)
            {
                if (currentTime - _lastUpdateTime <= _updateRate)
                    return;

                _lastUpdateTime = currentTime;
            }
        }

        sealed class StatusEffectCollectionItem
        {
            readonly int _disableTime;
            readonly Grh _grh;
            readonly ushort _power;
            readonly StatusEffectInfoAttribute _statusEffectInfo;
            readonly StatusEffectType _statusEffectType;

            /// <summary>
            /// Initializes a new instance of the <see cref="StatusEffectCollectionItem"/> class.
            /// </summary>
            /// <param name="statusEffectType">The <see cref="StatusEffectType"/> to use.</param>
            /// <param name="power">The power of the status effect.</param>
            /// <param name="disableTime">The game time at which this status effect will be disabled.</param>
            public StatusEffectCollectionItem(StatusEffectType statusEffectType, ushort power, int disableTime)
            {
                _statusEffectType = statusEffectType;
                _power = power;
                _disableTime = disableTime;

                _statusEffectInfo = StatusEffectInfoManager.Instance.GetAttribute(statusEffectType);
                _grh = new Grh(_statusEffectInfo.Icon);
            }

            public int DisableTime
            {
                get { return _disableTime; }
            }

            public ushort Power
            {
                get { return _power; }
            }

            public StatusEffectInfoAttribute StatusEffectInfo
            {
                get { return _statusEffectInfo; }
            }

            public StatusEffectType StatusEffectType
            {
                get { return _statusEffectType; }
            }

            public void Draw(ISpriteBatch sb, Vector2 position, TickCount currentTime)
            {
                _grh.Update(currentTime);
                _grh.Draw(sb, position);
            }

            public int GetSecsLeft(TickCount currentTime)
            {
                var msRemaining = _disableTime - currentTime;
                var secsRemaining = (int)Math.Round(msRemaining / 1000f);
                return Math.Max(secsRemaining, 0);
            }
        }
    }
}