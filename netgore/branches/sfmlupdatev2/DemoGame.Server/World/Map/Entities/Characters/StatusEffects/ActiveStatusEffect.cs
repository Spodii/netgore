using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Features.StatusEffects;
using NetGore.Stats;

namespace DemoGame.Server
{
    /// <summary>
    /// Describes an instance of an active <see cref="IStatusEffect{TStatType,TStatusEffectType}"/> on a Character.
    /// </summary>
    public class ActiveStatusEffect : IModStatContainer<StatType>
    {
        readonly IStatusEffect<StatType, StatusEffectType> _statusEffect;

        TickCount _disableTime;
        ushort _power;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveStatusEffect"/> class.
        /// </summary>
        /// <param name="statusEffect">The <see cref="IStatusEffect{TStatType, TSkillType}"/> to use.</param>
        /// <param name="power">The power of the StatusEffect.</param>
        /// <param name="disableTime">The game time at which this <see cref="ActiveStatusEffect"/> will be disabled.</param>
        public ActiveStatusEffect(IStatusEffect<StatType, StatusEffectType> statusEffect, ushort power, TickCount disableTime)
        {
            _statusEffect = statusEffect;
            _power = power;
            _disableTime = disableTime;
        }

        /// <summary>
        /// Gets the game time at which this <see cref="ActiveStatusEffect"/> will be disabled.
        /// </summary>
        public TickCount DisableTime
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
        /// Gets the <see cref="IStatusEffect{TStatType, TSkillType}"/> that this <see cref="ActiveStatusEffect"/>.
        /// </summary>
        public IStatusEffect<StatType, StatusEffectType> StatusEffect
        {
            get { return _statusEffect; }
        }

        public void AddBonusesTo(IStatCollection<StatType> statCollection)
        {
            foreach (var statType in StatusEffect.ModifiedStats)
            {
                statCollection[statType] += GetStatModBonus(statType);
            }
        }

        public IEnumerable<KeyValuePair<StatType, int>> GetStatModBonuses()
        {
            var ret = new KeyValuePair<StatType, int>[StatusEffect.ModifiedStats.Count()];

            var i = 0;
            foreach (var statType in StatusEffect.ModifiedStats)
            {
                ret[i] = new KeyValuePair<StatType, int>(statType, GetStatModBonus(statType));
                i++;
            }

            return ret;
        }

        public static TickCount GetTimeRemaining(TickCount gameTime, TickCount disableTime)
        {
            return disableTime - gameTime;
        }

        /// <summary>
        /// Gets the time remaining, in milliseconds, until this <see cref="ActiveStatusEffect"/> is disabled.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        /// <returns>The time remaining, in milliseconds, until this <see cref="ActiveStatusEffect"/> is disabled.</returns>
        public TickCount GetTimeRemaining(TickCount gameTime)
        {
            return DisableTime - gameTime;
        }

        /// <summary>
        /// Merges this <see cref="ActiveStatusEffect"/> with the values for another <see cref="ActiveStatusEffect"/>.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        /// <param name="power">The power of the <see cref="ActiveStatusEffect"/> that is merging with this one.</param>
        /// <param name="disableTime">The disable time of the <see cref="ActiveStatusEffect"/> that is merging with
        /// this one.</param>
        /// <returns>True if the merge resulted in this <see cref="ActiveStatusEffect"/> changing; otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="StatusEffectMergeType"/> is invalid.</exception>
        public bool MergeWith(TickCount currentTime, ushort power, TickCount disableTime)
        {
            int oldPower = _power;
            var oldDisableTime = _disableTime;

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
                    if (power == _power)
                    {
                        // Same power, so just use the longer time
                        _disableTime = Math.Max(disableTime, _disableTime);
                    }
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
                    const string errmsg = "Unknown StatusEffectMergeType `{0}`.";
                    throw new ArgumentOutOfRangeException(string.Format(errmsg, StatusEffect.StatusEffectType), (Exception)null);
            }

            if (oldPower != _power || oldDisableTime != _disableTime)
                return true;
            else
                return false;
        }

        public void SubtractBonusesFrom(IStatCollection<StatType> statCollection)
        {
            foreach (var statType in StatusEffect.ModifiedStats)
            {
                statCollection[statType] -= GetStatModBonus(statType);
            }
        }

        #region IModStatContainer<StatType> Members

        /// <summary>
        /// Gets the modifier value for the given <paramref name="statType"/>, where a positive value adds to the
        /// mod stat value, a negative value subtracts from the mod stat value, and a value of 0 does not modify
        /// the mod stat value.
        /// </summary>
        /// <param name="statType">The <see cref="StatType"/> to get the modifier value for.</param>
        /// <returns>The modifier value for the given <paramref name="statType"/>.</returns>
        public int GetStatModBonus(StatType statType)
        {
            return StatusEffect.GetStatModifier(statType, Power);
        }

        #endregion
    }
}