using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public class NonPersistentCharacterStatusEffects : CharacterStatusEffects
    {
        readonly List<ActiveStatusEffect> _statusEffects = new List<ActiveStatusEffect>();

        protected override void HandleExpired(ActiveStatusEffect activeStatusEffect)
        {
            bool wasRemoved = _statusEffects.Remove(activeStatusEffect);

            Debug.Assert(wasRemoved, "Couldn't find the activeStatusEffect in the collection. Where'd it go...?");
        }

        public NonPersistentCharacterStatusEffects(Character character)
            : base(character)
        {
        }

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
        /// <filterpriority>1</filterpriority>
        public override IEnumerator<ActiveStatusEffect> GetEnumerator()
        {
            return _statusEffects.GetEnumerator();
        }

        public override bool TryAdd(StatusEffectBase statusEffect, ushort power)
        {
            if (statusEffect == null)
                throw new ArgumentNullException("statusEffect");

            ActiveStatusEffect existingStatusEffect;
            bool alreadyExists = TryGetStatusEffect(statusEffect.StatusEffectType, out existingStatusEffect);

            int time = GetTime();
            int disableTime = time + statusEffect.GetEffectTime(power);

            if (alreadyExists)
            {
                bool changed = existingStatusEffect.MergeWith(time, power, disableTime);
                return changed;
            }
            else
            {
                ActiveStatusEffect ase = new ActiveStatusEffect(statusEffect, power, disableTime);
                _statusEffects.Add(ase);
                NotifyAdded(ase);
                return true;
            }
        }

        public override bool TryGetStatusEffect(StatusEffectType statusEffectType, out ActiveStatusEffect statusEffect)
        {
            foreach (ActiveStatusEffect activeStatusEffect in this)
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
