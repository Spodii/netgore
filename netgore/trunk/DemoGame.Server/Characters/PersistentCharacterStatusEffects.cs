using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;

// TODO: !! Apply the bonuses from active effects
// TODO: !! Delete expired effects
// TODO: !! Load effects from database
// TODO: !! Save effects when disposing character

namespace DemoGame.Server
{
    public class PersistentCharacterStatusEffects : CharacterStatusEffects
    {
        static readonly ActiveStatusEffectIDCreator _idCreator;
        static readonly ReplaceCharacterStatusEffectQuery _replaceQuery;
        static readonly DeleteCharacterStatusEffectQuery _deleteQuery;

        readonly List<ASEWithID> _statusEffects = new List<ASEWithID>();

        static PersistentCharacterStatusEffects()
        {
            var dbController = DBController.GetInstance();
            _idCreator = dbController.GetQuery<ActiveStatusEffectIDCreator>();
            _replaceQuery = dbController.GetQuery<ReplaceCharacterStatusEffectQuery>();
            _deleteQuery = dbController.GetQuery<DeleteCharacterStatusEffectQuery>();
        }

        public PersistentCharacterStatusEffects(Character character)
            : base(character)
        {
        }

        public override bool Contains(StatusEffectType statusEffectType)
        {
            return _statusEffects.Any(x => x.Value.StatusEffect.StatusEffectType == statusEffectType);
        }

        void DatabaseDelete(ActiveStatusEffectID id)
        {
            _deleteQuery.Execute(id);
        }

        void DatabaseUpdate(ASEWithID item)
        {
            int secsLeft = (int)Math.Round((item.Value.DisableTime - GetTime()) / 1000f);
            if (secsLeft < 1)
                secsLeft = 1;
          
            var values = new CharacterStatusEffectTable(Character.ID, item.ID, item.Value.Power, item.Value.StatusEffect.StatusEffectType, (ushort)secsLeft);
            _replaceQuery.Execute(values);
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
            foreach (ASEWithID item in _statusEffects)
            {
                yield return item.Value;
            }
        }

        public override bool TryAdd(StatusEffectBase statusEffect, ushort power)
        {
            if (statusEffect == null)
                throw new ArgumentNullException("statusEffect");

            ASEWithID existingStatusEffect;
            bool alreadyExists = TryGetStatusEffect(statusEffect.StatusEffectType, out existingStatusEffect);

            int time = GetTime();
            int disableTime = time + statusEffect.GetEffectTime(power);

            if (alreadyExists)
            {
                bool changed = existingStatusEffect.Value.MergeWith(time, power, disableTime);
                if (changed)
                    DatabaseUpdate(existingStatusEffect);
                return changed;
            }
            else
            {
                ActiveStatusEffect ase = new ActiveStatusEffect(statusEffect, power, disableTime);
                ActiveStatusEffectID id = (ActiveStatusEffectID)_idCreator.GetNext();

                ASEWithID aseWithID = new ASEWithID(id, ase);
                _statusEffects.Add(aseWithID);
                DatabaseUpdate(aseWithID);

                return true;
            }
        }

        bool TryGetStatusEffect(StatusEffectType statusEffectType, out ASEWithID statusEffect)
        {
            foreach (var item in _statusEffects)
            {
                if (item.Value.StatusEffect.StatusEffectType == statusEffectType)
                {
                    statusEffect = item;
                    return true;
                }
            }

            statusEffect = default(ASEWithID);
            return false;
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

        public struct ASEWithID
        {
            public readonly ActiveStatusEffectID ID;
            public readonly ActiveStatusEffect Value;

            public ASEWithID(ActiveStatusEffectID id, ActiveStatusEffect value)
            {
                ID = id;
                Value = value;
            }
        }
    }
}
