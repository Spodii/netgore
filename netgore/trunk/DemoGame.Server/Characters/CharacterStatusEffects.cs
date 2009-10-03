using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Delegate for handling when an ActiveStatusEffect is added to or removed from a CharacterStatusEffects. 
    /// </summary>
    /// <param name="characterStatusEffects">CharacterStatusEffects the event took place on.</param>
    /// <param name="activeStatusEffect">The ActiveStatusEffect that was added or removed.</param>
    public delegate void CharacterStatusEffectsAddRemoveHandler(
        CharacterStatusEffects characterStatusEffects, ActiveStatusEffect activeStatusEffect);

    public abstract class CharacterStatusEffects : IEnumerable<ActiveStatusEffect>, IGetTime, IModStatContainer, IDisposable
    {
        /// <summary>
        /// How frequently, in milliseconds, the ActiveStatusEffect are checked for being expired.
        /// </summary>
        const int _updateFrequency = 200;

        readonly Character _character;

#if DEBUG
        static readonly Random _random = new Random();
#endif

        /// <summary>
        /// Contains the sum of the stat modifiers for each ActiveStatusEffect for each StatType.
        /// </summary>
        readonly FullStatCollection _modStats = new FullStatCollection(StatCollectionType.Modified);

        int _lastUpdateTime;

        public event CharacterStatusEffectsAddRemoveHandler OnAdd;

        public event CharacterStatusEffectsAddRemoveHandler OnRemove;

        public Character Character
        {
            get { return _character; }
        }

        protected CharacterStatusEffects(Character character)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            _character = character;
        }

        public abstract bool Contains(StatusEffectType statusEffectType);

        /// <summary>
        /// When overridden in the derived class, handles removing an expired StatusEffect.
        /// </summary>
        /// <param name="activeStatusEffect">StatusEffect to be removed.</param>
        protected abstract void HandleExpired(ActiveStatusEffect activeStatusEffect);

        protected virtual void NotifyAdded(ActiveStatusEffect statusEffect)
        {
            statusEffect.AddBonusesTo(_modStats);

            if (OnAdd != null)
                OnAdd(this, statusEffect);
        }

        protected virtual void NotifyRemoved(ActiveStatusEffect statusEffect)
        {
            statusEffect.SubtractBonusesFrom(_modStats);

            if (OnRemove != null)
                OnRemove(this, statusEffect);

#if DEBUG
            // Randomly test to see if the values are correct
            if (_random.Next(0, 5) == 0)
            {
                FullStatCollection oldValues = _modStats.DeepCopy();
                RecalculateStatBonuses();
                Debug.Assert(_modStats.AreValuesEqual(oldValues), "Somehow, at some point, the ModStats became out of sync!");
            }
#endif
        }

        /// <summary>
        /// Forces a recalculation of the stat bonuses. Ideally, this will never be needed, even though recalculation
        /// is not very expensive.
        /// </summary>
        protected void RecalculateStatBonuses()
        {
            _modStats.SetAll(0);

            foreach (ActiveStatusEffect item in this)
            {
                item.AddBonusesTo(_modStats);
            }
        }

        /// <summary>
        /// Finds all of the expired ActiveStatusEffects and uses HandleExpired to remove them.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected void RemoveExpired(int currentTime)
        {
            Stack<ActiveStatusEffect> removeStack = null;

            // Create a stack of the items to remove
            // We must create a temporary collection since HandleExpired will likely cause the collection to be modified
            // We also want to avoid creating the removeStack if possible
            foreach (ActiveStatusEffect item in this)
            {
                if (item.DisableTime < currentTime)
                {
                    if (removeStack == null)
                        removeStack = new Stack<ActiveStatusEffect>(2);

                    removeStack.Push(item);
                }
            }

            // Remove each of the items
            if (removeStack != null)
            {
                foreach (ActiveStatusEffect item in removeStack)
                {
                    HandleExpired(item);
                }
            }
        }

        public abstract bool TryAdd(StatusEffectBase statusEffect, ushort power);

        public bool TryAdd(StatusEffectType statusEffectType, ushort power)
        {
            StatusEffectBase statusEffect = StatusEffectManager.GetStatusEffect(statusEffectType);
            return TryAdd(statusEffect, power);
        }

        public abstract bool TryGetStatusEffect(StatusEffectType statusEffectType, out ActiveStatusEffect statusEffect);

        public virtual void Update()
        {
            int currentTime = GetTime();
            if (_lastUpdateTime + _updateFrequency < currentTime)
            {
                _lastUpdateTime = currentTime;
                RemoveExpired(currentTime);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public virtual void Dispose()
        {
        }

        #endregion

        #region IEnumerable<ActiveStatusEffect> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public abstract IEnumerator<ActiveStatusEffect> GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public int GetTime()
        {
            return _character.GetTime();
        }

        #endregion

        #region IModStatContainer Members

        public int GetStatModBonus(StatType statType)
        {
            return _modStats[statType];
        }

        #endregion
    }
}