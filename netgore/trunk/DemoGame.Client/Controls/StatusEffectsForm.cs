using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    public class StatusEffectsForm : Form, IGetTime
    {
        static readonly Vector2 _iconSize = new Vector2(32, 32);
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly IGetTime _getTime;
        readonly StatusEffectCollection _statusEffects = new StatusEffectCollection();

        public StatusEffectsForm(Control parent, Vector2 position, IGetTime getTime)
            : base(parent.GUIManager, "Status Effects", position, _iconSize)
        {
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

        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            int currentTime = GetTime();

            Vector2 offset = new Vector2(-_iconSize.X, 0);

            foreach (StatusEffectCollectionItem dase in _statusEffects.ActiveStatusEffects)
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

        protected override void UpdateControl(int currentTime)
        {
            base.UpdateControl(currentTime);

            _statusEffects.Update(currentTime);
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public int GetTime()
        {
            return _getTime.GetTime();
        }

        #endregion

        sealed class StatusEffectCollection
        {
            const int _updateRate = 500;
            readonly List<StatusEffectCollectionItem> _statusEffects = new List<StatusEffectCollectionItem>();

            int _lastUpdateTime;

            public IEnumerable<StatusEffectCollectionItem> ActiveStatusEffects
            {
                get { return _statusEffects; }
            }

            public void Add(StatusEffectType statusEffectType, ushort power, ushort secsLeft, int currentTime)
            {
                Remove(statusEffectType);

                int msLeft = secsLeft * 1000;
                int life = msLeft + currentTime;

                StatusEffectCollectionItem ase = new StatusEffectCollectionItem(statusEffectType, power, life);
                _statusEffects.Add(ase);
            }

            public void Remove(StatusEffectType statusEffectType)
            {
                _statusEffects.RemoveAll(x => x.StatusEffectType == statusEffectType);
            }

            public void Update(int currentTime)
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
            readonly StatusEffectInfo _statusEffectInfo;
            readonly StatusEffectType _statusEffectType;

            public int DisableTime
            {
                get { return _disableTime; }
            }

            public ushort Power
            {
                get { return _power; }
            }

            public StatusEffectInfo StatusEffectInfo
            {
                get { return _statusEffectInfo; }
            }

            public StatusEffectType StatusEffectType
            {
                get { return _statusEffectType; }
            }

            /// <summary>
            /// StatusEffectCollectionItem constructor.
            /// </summary>
            /// <param name="statusEffectType">The StatusEffectType to use.</param>
            /// <param name="power">The power of the StatusEffect.</param>
            /// <param name="disableTime">The game time at which this ActiveStatusEffect will be disabled.</param>
            public StatusEffectCollectionItem(StatusEffectType statusEffectType, ushort power, int disableTime)
            {
                _statusEffectType = statusEffectType;
                _power = power;
                _disableTime = disableTime;

                _statusEffectInfo = StatusEffectInfo.GetStatusEffectInfo(statusEffectType);
                _grh = new Grh(_statusEffectInfo.Icon);
            }

            public void Draw(SpriteBatch sb, Vector2 position, int currentTime)
            {
                _grh.Update(currentTime);
                _grh.Draw(sb, position);
            }

            public int GetSecsLeft(int currentTime)
            {
                int msRemaining = _disableTime - currentTime;
                int secsRemaining = (int)Math.Round(msRemaining / 1000f);
                return Math.Max(secsRemaining, 0);
            }
        }
    }
}