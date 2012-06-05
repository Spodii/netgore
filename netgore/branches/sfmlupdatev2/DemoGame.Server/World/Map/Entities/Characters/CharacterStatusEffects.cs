using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.Features.StatusEffects;
using NetGore.Stats;

namespace DemoGame.Server
{
    /// <summary>
    /// A collection of <see cref="ActiveStatusEffect"/>s that are currently applied to a single <see cref="Character"/>.
    /// </summary>
    public abstract class CharacterStatusEffects : IEnumerable<ActiveStatusEffect>, IGetTime, IModStatContainer<StatType>,
                                                   IDisposable
    {
        /// <summary>
        /// How frequently, in milliseconds, the ActiveStatusEffect are checked for being expired.
        /// </summary>
        const int _updateFrequency = 200;

        /// <summary>
        /// The <see cref="StatusEffectManager"/> instance.
        /// </summary>
        static readonly StatusEffectManager _statusEffectManager = StatusEffectManager.Instance;

        readonly Character _character;

        /// <summary>
        /// Contains the sum of the stat modifiers for each ActiveStatusEffect for each <see cref="StatType"/>.
        /// </summary>
        readonly StatCollection<StatType> _modStats = new StatCollection<StatType>(StatCollectionType.Modified);

        TickCount _lastUpdateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterStatusEffects"/> class.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> that this collection belongs to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="character" /> is <c>null</c>.</exception>
        protected CharacterStatusEffects(Character character)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            _character = character;
        }

        /// <summary>
        /// Notifies listeners when an <see cref="ActiveStatusEffect"/> is added to this collection.
        /// </summary>
        public event TypedEventHandler<CharacterStatusEffects, EventArgs<ActiveStatusEffect>> Added;

        /// <summary>
        /// Notifies listeners when an <see cref="ActiveStatusEffect"/> is removed from this collection.
        /// </summary>
        public event TypedEventHandler<CharacterStatusEffects, EventArgs<ActiveStatusEffect>> Removed;

        /// <summary>
        /// Gets the <see cref="Character"/> that this collection belongs to.
        /// </summary>
        public Character Character
        {
            get { return _character; }
        }

        /// <summary>
        /// Checks that the mod stat values are correct.
        /// Only occurs in debug builds.
        /// </summary>
        [Conditional("DEBUG")]
        void AssertModStatsAreCorrect()
        {
            var oldValues = _modStats.DeepCopy();
            RecalculateStatBonuses();
            if (!_modStats.HasSameValues(oldValues))
                Debug.Fail("Somehow, at some point, the ModStats became out of sync!");
        }

        /// <summary>
        /// When overridden in the derived class, gets if this collection contains a given <see cref="StatusEffectType"/>.
        /// </summary>
        /// <param name="statusEffectType">The <see cref="StatusEffectType"/> to check for.</param>
        /// <returns>True if this collection contains the <paramref name="statusEffectType"/>; otherwise false.</returns>
        public abstract bool Contains(StatusEffectType statusEffectType);

        /// <summary>
        /// When overridden in the derived class, handles removing an expired StatusEffect.
        /// </summary>
        /// <param name="activeStatusEffect">StatusEffect to be removed.</param>
        protected abstract void HandleExpired(ActiveStatusEffect activeStatusEffect);

        /// <summary>
        /// Occurs when the <see cref="CharacterStatusEffects.Added"/> event is raised.
        /// </summary>
        /// <param name="statusEffect">The <see cref="ActiveStatusEffect"/> that was added.</param>
        protected virtual void OnAdded(ActiveStatusEffect statusEffect)
        {
            statusEffect.AddBonusesTo(_modStats);

            if (Added != null)
                Added.Raise(this, EventArgsHelper.Create(statusEffect));

            AssertModStatsAreCorrect();
        }

        /// <summary>
        /// Occurs when the <see cref="CharacterStatusEffects.Removed"/> event is raised.
        /// </summary>
        /// <param name="statusEffect">The <see cref="ActiveStatusEffect"/> that was removed.</param>
        protected virtual void OnRemoved(ActiveStatusEffect statusEffect)
        {
            statusEffect.SubtractBonusesFrom(_modStats);

            if (Removed != null)
                Removed.Raise(this, EventArgsHelper.Create(statusEffect));

            AssertModStatsAreCorrect();
        }

        /// <summary>
        /// Forces a recalculation of the stat bonuses. Ideally, this will never be needed, even though recalculation
        /// is not very expensive.
        /// </summary>
        protected void RecalculateStatBonuses()
        {
            _modStats.SetAll(0);

            foreach (var item in this)
            {
                item.AddBonusesTo(_modStats);
            }
        }

        /// <summary>
        /// Finds all of the expired ActiveStatusEffects and uses HandleExpired to remove them.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected void RemoveExpired(TickCount currentTime)
        {
            Stack<ActiveStatusEffect> removeStack = null;

            // Create a stack of the items to remove
            // We must create a temporary collection since HandleExpired will likely cause the collection to be modified
            // We also want to avoid creating the removeStack if possible
            foreach (var item in this)
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
                foreach (var item in removeStack)
                {
                    HandleExpired(item);
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, tries to add an <see cref="IStatusEffect{StatType, StatusEffectType}"/> to
        /// this collection.
        /// </summary>
        /// <param name="statusEffect">The status effect to add.</param>
        /// <param name="power">The power of the status effect.</param>
        /// <returns>True if the <paramref name="statusEffect"/> of the given <paramref name="power"/> was added
        /// to this collection; otherwise false.</returns>
        public abstract bool TryAdd(IStatusEffect<StatType, StatusEffectType> statusEffect, ushort power);

        /// <summary>
        /// Tries to add a <see cref="StatusEffectType"/> of a given power to this collection.
        /// </summary>
        /// <param name="statusEffectType">The <see cref="StatusEffectType"/> to add.</param>
        /// <param name="power">The power of the <paramref name="statusEffectType"/>.</param>
        /// <returns>True if the <paramref name="statusEffectType"/> of the given <paramref name="power"/> was added
        /// to this collection; otherwise false.</returns>
        public bool TryAdd(StatusEffectType statusEffectType, ushort power)
        {
            var statusEffect = _statusEffectManager.Get(statusEffectType);
            return TryAdd(statusEffect, power);
        }

        /// <summary>
        /// When overridden in the derived class, tries to get the <see cref="ActiveStatusEffect"/> for a <see cref="StatusEffectType"/>
        /// in this collection.
        /// </summary>
        /// <param name="statusEffectType">The <see cref="StatusEffectType"/> to try to get the <see cref="ActiveStatusEffect"/> of.</param>
        /// <param name="statusEffect">When this method returns true, contains the <see cref="ActiveStatusEffect"/> instance from
        /// this collection for the given <paramref name="statusEffectType"/>.</param>
        /// <returns>True if the <see cref="ActiveStatusEffect"/> of the <paramref name="statusEffectType"/> was found in
        /// this collection; otherwise false.</returns>
        public abstract bool TryGetStatusEffect(StatusEffectType statusEffectType, out ActiveStatusEffect statusEffect);

        /// <summary>
        /// Updates the items in this collection.
        /// </summary>
        public virtual void Update()
        {
            var currentTime = GetTime();
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
        public abstract IEnumerator<ActiveStatusEffect> GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
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
        public TickCount GetTime()
        {
            return _character.GetTime();
        }

        #endregion

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
            return _modStats[statType];
        }

        #endregion
    }
}