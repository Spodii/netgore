using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// Describes an instance of an active StatusEffectBase on a Character.
    /// </summary>
    public class ActiveStatusEffect : IModStatContainer
    {
        readonly StatusEffectBase _statusEffect;
        int _disableTime;
        ushort _power;

        /// <summary>
        /// Gets the game time at which this ActiveStatusEffect will be disabled.
        /// </summary>
        public int DisableTime
        {
            get { return _disableTime; }
        }

        /// <summary>
        /// Gets the power of the StatusEffect. How the status effect's power is utilized depends completely
        /// on the StatusEffect used.
        /// </summary>
        public ushort Power
        {
            get { return _power; }
        }

        /// <summary>
        /// Gets the StatusEffectBase that this ActiveStatusEffect
        /// </summary>
        public StatusEffectBase StatusEffect
        {
            get { return _statusEffect; }
        }

        /// <summary>
        /// ActiveStatusEffect constructor.
        /// </summary>
        /// <param name="statusEffect">The StatusEffectBase to use.</param>
        /// <param name="power">The power of the StatusEffect.</param>
        /// <param name="disableTime">The game time at which this ActiveStatusEffect will be disabled.</param>
        public ActiveStatusEffect(StatusEffectBase statusEffect, ushort power, int disableTime)
        {
            _statusEffect = statusEffect;
            _power = power;
            _disableTime = disableTime;
        }

        public void AddBonusesTo(IStatCollection statCollection)
        {
            foreach (StatType statType in StatusEffect.ModifiedStats)
            {
                statCollection[statType] += GetStatModBonus(statType);
            }
        }

        public IEnumerable<KeyValuePair<StatType, int>> GetStatModBonuses()
        {
            var ret = new KeyValuePair<StatType, int>[StatusEffect.ModifiedStats.Count()];

            int i = 0;
            foreach (StatType statType in StatusEffect.ModifiedStats)
            {
                ret[i] = new KeyValuePair<StatType, int>(statType, GetStatModBonus(statType));
                i++;
            }

            return ret;
        }

        public static int GetTimeRemaining(int gameTime, int disableTime)
        {
            return disableTime - gameTime;
        }

        /// <summary>
        /// Gets the time remaining, in milliseconds, until this ActiveStatusEffect is disabled.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        /// <returns>The time remaining, in milliseconds, until this ActiveStatusEffect is disabled.</returns>
        public int GetTimeRemaining(int gameTime)
        {
            return DisableTime - gameTime;
        }

        /// <summary>
        /// Merges this ActiveStatusEffect with the values for another ActiveStatusEffect.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        /// <param name="power">The power of the ActiveStatusEffect that is merging with this one.</param>
        /// <param name="disableTime">The disable time of the ActiveStatusEffect that is merging with this one.</param>
        /// <returns>True if the merge resulted in this ActiveStatusEffect changing; otherwise false.</returns>
        public bool MergeWith(int currentTime, ushort power, int disableTime)
        {
            int oldPower = _power;
            int oldDisableTime = _disableTime;

            switch (StatusEffect.MergeType)
            {
                case StatusEffectMergeType.DiscardNewer:
                    break;

                case StatusEffectMergeType.DiscardOlder:
                    _power = power;
                    _disableTime = disableTime;
                    break;

                case StatusEffectMergeType.DiscardWeakest:
                    _power = Math.Min(power, _power);
                    _disableTime = Math.Min(disableTime, _disableTime);
                    break;

                case StatusEffectMergeType.DiscardWeakestUnlessTimeUnder30Secs:
                    if (power == power)
                        _disableTime = Math.Max(disableTime, _disableTime);
                    else if (_power > power)
                    {
                        if (GetTimeRemaining(currentTime, _disableTime) < 30000 && _disableTime <= disableTime)
                        {
                            _power = power;
                            _disableTime = disableTime;
                        }
                    }
                    else if (_power < power)
                    {
                        if (GetTimeRemaining(currentTime, _disableTime) >= 30000 &&
                            GetTimeRemaining(currentTime, disableTime) < 30000)
                        {
                            _power = power;
                            _disableTime = disableTime;
                        }
                    }
                    break;

                case StatusEffectMergeType.DiscardStrongest:
                    if (_power > power)
                    {
                        _power = power;
                        _disableTime = disableTime;
                    }
                    break;

                case StatusEffectMergeType.UseGreatestTimeAndPower:
                    _power = Math.Max(power, _power);
                    _disableTime = Math.Max(disableTime, _disableTime);
                    break;

                case StatusEffectMergeType.UseLeastTimeAndPower:
                    _power = Math.Min(power, _power);
                    _disableTime = Math.Min(disableTime, _disableTime);
                    break;

                case StatusEffectMergeType.CombineTimeOnGreaterPower:
                    _power = Math.Max(power, _power);
                    _disableTime = GetTimeRemaining(currentTime, disableTime) + GetTimeRemaining(currentTime, _disableTime);
                    break;

                case StatusEffectMergeType.CombineTimeOnWeakerPower:
                    _power = Math.Min(power, _power);
                    _disableTime = GetTimeRemaining(currentTime, disableTime) + GetTimeRemaining(currentTime, _disableTime);
                    break;

                case StatusEffectMergeType.CombinePowerOnGreaterTime:
                    _power = (ushort)(_power + power);
                    _disableTime = Math.Max(disableTime, _disableTime);
                    break;

                case StatusEffectMergeType.CombinePowerOnLeastTime:
                    _power = (ushort)(_power + power);
                    _disableTime = Math.Min(disableTime, _disableTime);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (oldPower != _power || oldDisableTime != _disableTime)
                return true;
            else
                return false;
        }

        public void SubtractBonusesFrom(IStatCollection statCollection)
        {
            foreach (StatType statType in StatusEffect.ModifiedStats)
            {
                statCollection[statType] -= GetStatModBonus(statType);
            }
        }

        #region IModStatContainer Members

        public int GetStatModBonus(StatType statType)
        {
            return StatusEffect.GetStatModifier(statType, Power);
        }

        #endregion
    }
}