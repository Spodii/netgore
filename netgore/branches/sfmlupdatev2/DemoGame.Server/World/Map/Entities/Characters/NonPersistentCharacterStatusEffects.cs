using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.Features.StatusEffects;

namespace DemoGame.Server
{
    /// <summary>
    /// A <see cref="CharacterStatusEffects"/> for a <see cref="Character"/> that does not persist to the database.
    /// Since the <see cref="Character"/> does not persist to the database, neither do their status effects.
    /// </summary>
    public class NonPersistentCharacterStatusEffects : CharacterStatusEffects
    {
        readonly List<ActiveStatusEffect> _statusEffects = new List<ActiveStatusEffect>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NonPersistentCharacterStatusEffects"/> class.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> that this collection belongs to.</param>
        public NonPersistentCharacterStatusEffects(Character character) : base(character)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets if this collection contains a given <see cref="StatusEffectType"/>.
        /// </summary>
        /// <param name="statusEffectType">The <see cref="StatusEffectType"/> to check for.</param>
        /// <returns>True if this collection contains the <paramref name="statusEffectType"/>; otherwise false.</returns>
        public override bool Contains(StatusEffectType statusEffectType)
        {
            return _statusEffects.Any(x => x.StatusEffect.StatusEffectType == statusEffectType);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<ActiveStatusEffect> GetEnumerator()
        {
            return _statusEffects.GetEnumerator();
        }

        /// <summary>
        /// When overridden in the derived class, handles removing an expired StatusEffect.
        /// </summary>
        /// <param name="activeStatusEffect">StatusEffect to be removed.</param>
        protected override void HandleExpired(ActiveStatusEffect activeStatusEffect)
        {
            // Remove the item from the list
            var wasRemoved = _statusEffects.Remove(activeStatusEffect);

            // Raise removal event (assuming it was properly removed like it should have been)
            if (wasRemoved)
                OnRemoved(activeStatusEffect);

            Debug.Assert(wasRemoved, "Couldn't find the activeStatusEffect in the collection. Where'd it go...?");
        }

        /// <summary>
        /// When overridden in the derived class, tries to add an <see cref="IStatusEffect{StatType, StatusEffectType}"/> to
        /// this collection.
        /// </summary>
        /// <param name="statusEffect">The status effect to add.</param>
        /// <param name="power">The power of the status effect.</param>
        /// <returns>True if the <paramref name="statusEffect"/> of the given <paramref name="power"/> was added
        /// to this collection; otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="statusEffect" /> is <c>null</c>.</exception>
        public override bool TryAdd(IStatusEffect<StatType, StatusEffectType> statusEffect, ushort power)
        {
            if (statusEffect == null)
                throw new ArgumentNullException("statusEffect");

            ActiveStatusEffect existingStatusEffect;
            var alreadyExists = TryGetStatusEffect(statusEffect.StatusEffectType, out existingStatusEffect);

            var time = GetTime();
            var disableTime = (TickCount)(time + statusEffect.GetEffectTime(power));

            if (alreadyExists)
            {
                var changed = existingStatusEffect.MergeWith(time, power, disableTime);
                if (changed)
                    RecalculateStatBonuses();
                return changed;
            }
            else
            {
                var ase = new ActiveStatusEffect(statusEffect, power, disableTime);
                _statusEffects.Add(ase);
                OnAdded(ase);
                return true;
            }
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
        public override bool TryGetStatusEffect(StatusEffectType statusEffectType, out ActiveStatusEffect statusEffect)
        {
            foreach (var activeStatusEffect in this)
            {
                if (activeStatusEffect.StatusEffect.StatusEffectType == statusEffectType)
                {
                    statusEffect = activeStatusEffect;
                    return true;
                }
            }

            statusEffect = null;
            return false;
        }
    }
}