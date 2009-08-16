using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server
{
    public abstract class CharacterStatusEffects : IEnumerable<ActiveStatusEffect>, IGetTime
    {
        readonly Character _character;

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

        public abstract bool TryAdd(StatusEffectBase statusEffect, ushort power);

        public bool TryAdd(StatusEffectType statusEffectType, ushort power)
        {
            StatusEffectBase statusEffect = StatusEffectManager.GetStatusEffect(statusEffectType);
            return TryAdd(statusEffect, power);
        }

        public abstract bool TryGetStatusEffect(StatusEffectType statusEffectType, out ActiveStatusEffect statusEffect);

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
    }
}