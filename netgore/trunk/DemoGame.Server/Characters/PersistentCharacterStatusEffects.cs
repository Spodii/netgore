using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server
{
    public class PersistentCharacterStatusEffects : CharacterStatusEffects
    {
        /// <summary>
        /// The minimum amount of time an ActiveStatusEffect must have remaining for it to be saved in the
        /// database when this object is disposed. Anything with less time remaining than this specified time
        /// will be deleted instead of updated.
        /// </summary>
        const int _minTimeForStatusEffectsOnDispose = 2000;

        static readonly DeleteCharacterStatusEffectQuery _deleteQuery;

        static readonly ActiveStatusEffectIDCreator _idCreator;
        static readonly ReplaceCharacterStatusEffectQuery _replaceQuery;
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly List<ASEWithID> _statusEffects = new List<ASEWithID>();

        static PersistentCharacterStatusEffects()
        {
            IDbController dbController = DbControllerBase.GetInstance();
            _idCreator = dbController.GetQuery<ActiveStatusEffectIDCreator>();
            _replaceQuery = dbController.GetQuery<ReplaceCharacterStatusEffectQuery>();
            _deleteQuery = dbController.GetQuery<DeleteCharacterStatusEffectQuery>();
        }

        public PersistentCharacterStatusEffects(Character character) : base(character)
        {
        }

        public override bool Contains(StatusEffectType statusEffectType)
        {
            return _statusEffects.Any(x => x.Value.StatusEffect.StatusEffectType == statusEffectType);
        }

        static void DeleteFromDatabase(ActiveStatusEffectID id)
        {
            _deleteQuery.Execute(id);
            _idCreator.FreeID(id);
        }

        public override void Dispose()
        {
            base.Dispose();

            int currentTime = GetTime();

            // Update or remove each ActiveStatusEffect
            foreach (ASEWithID item in _statusEffects)
            {
                if (item.Value.GetTimeRemaining(currentTime) < _minTimeForStatusEffectsOnDispose)
                    DeleteFromDatabase(item.ID);
                else
                    UpdateInDatabase(item);
            }

            // Clear the list... just in case
            _statusEffects.Clear();
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

        protected override void HandleExpired(ActiveStatusEffect activeStatusEffect)
        {
            for (int i = 0; i < _statusEffects.Count; i++)
            {
                ASEWithID item = _statusEffects[i];
                if (item.Value != activeStatusEffect)
                    continue;

                _statusEffects.RemoveAt(i);
                NotifyRemoved(activeStatusEffect);
                DeleteFromDatabase(item.ID);
                return;
            }

            Debug.Fail("Couldn't find the activeStatusEffect in the collection. Where'd it go...?");
        }

        public void Load()
        {
            Debug.Assert(_statusEffects.Count == 0, "Why is Load() being called while there are active effects here already?");

            var values = Character.DbController.GetQuery<SelectCharacterStatusEffectsQuery>().Execute(Character.ID);

            int currentTime = GetTime();

            // Load in the ActiveStatusEffects using the values read from the database
            foreach (ICharacterStatusEffectTable value in values)
            {
                StatusEffectBase statusEffect = StatusEffectManager.GetStatusEffect(value.StatusEffect);
                if (statusEffect == null)
                {
                    const string errmsg = "Failed to get the StatusEffectBase for StatusEffectType `{0}` on Character `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, value.StatusEffect, Character);
                    Debug.Fail(string.Format(errmsg, value.StatusEffect, Character));
                    continue;
                }

                ActiveStatusEffect ase = new ActiveStatusEffect(statusEffect, value.Power, value.TimeLeftSecs * 1000 + currentTime);
                ASEWithID aseWithID = new ASEWithID(value.ID, ase);
                _statusEffects.Add(aseWithID);
                NotifyAdded(ase);
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
                    UpdateInDatabase(existingStatusEffect);
                return changed;
            }
            else
            {
                ActiveStatusEffect ase = new ActiveStatusEffect(statusEffect, power, disableTime);
                ActiveStatusEffectID id = _idCreator.GetNext();

                ASEWithID aseWithID = new ASEWithID(id, ase);
                _statusEffects.Add(aseWithID);
                NotifyAdded(aseWithID.Value);
                UpdateInDatabase(aseWithID);

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

        bool TryGetStatusEffect(StatusEffectType statusEffectType, out ASEWithID statusEffect)
        {
            foreach (ASEWithID item in _statusEffects)
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

        void UpdateInDatabase(ASEWithID item)
        {
            int secsLeft = (int)Math.Round((item.Value.DisableTime - GetTime()) / 1000f);
            if (secsLeft < 1)
                secsLeft = 1;

            CharacterStatusEffectTable values = new CharacterStatusEffectTable
            {
                CharacterID = Character.ID,
                ID = item.ID,
                Power = item.Value.Power,
                TimeLeftSecs = (ushort)secsLeft,
                StatusEffect = item.Value.StatusEffect.StatusEffectType
            };

            _replaceQuery.Execute(values);
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