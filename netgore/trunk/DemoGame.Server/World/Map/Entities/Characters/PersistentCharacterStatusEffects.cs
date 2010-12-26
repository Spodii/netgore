using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Features.StatusEffects;

namespace DemoGame.Server
{
    /// <summary>
    /// A <see cref="CharacterStatusEffects"/> for a <see cref="Character"/> that persists to the database.
    /// Since the <see cref="Character"/> persists to the database, so must their status effects.
    /// </summary>
    public class PersistentCharacterStatusEffects : CharacterStatusEffects
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The minimum amount of time an ActiveStatusEffect must have remaining for it to be saved in the
        /// database when this object is disposed. Anything with less time remaining than this specified time
        /// will be deleted instead of updated.
        /// </summary>
        const int _minTimeForStatusEffectsOnDispose = 2000;

        static readonly IDbController _dbController;
        static readonly DeleteCharacterStatusEffectQuery _deleteQuery;
        static readonly InsertCharacterStatusEffectQuery _insertQuery;
        static readonly StatusEffectManager _statusEffectManager = StatusEffectManager.Instance;
        static readonly IEqualityComparer<StatusEffectType> _statusEffectTypeComparer = EnumComparer<StatusEffectType>.Instance;
        static readonly UpdateCharacterStatusEffectQuery _updateQuery;

        readonly List<ASEWithID> _statusEffects = new List<ASEWithID>();

        /// <summary>
        /// Initializes the <see cref="PersistentCharacterStatusEffects"/> class.
        /// </summary>
        static PersistentCharacterStatusEffects()
        {
            _dbController = DbControllerBase.GetInstance();
            _insertQuery = _dbController.GetQuery<InsertCharacterStatusEffectQuery>();
            _deleteQuery = _dbController.GetQuery<DeleteCharacterStatusEffectQuery>();
            _updateQuery = _dbController.GetQuery<UpdateCharacterStatusEffectQuery>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentCharacterStatusEffects"/> class.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> that this collection belongs to.</param>
        public PersistentCharacterStatusEffects(Character character) : base(character)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets if this collection contains a given <see cref="StatusEffectType"/>.
        /// </summary>
        /// <param name="statusEffectType">The <see cref="StatusEffectType"/> to check for.</param>
        /// <returns>True if this collection contains the <paramref name="statusEffectType"/>; otherwise false.</returns>
        public override bool Contains(StatusEffectType statusEffectType)
        {
            return
                _statusEffects.Any(x => _statusEffectTypeComparer.Equals(x.Value.StatusEffect.StatusEffectType, statusEffectType));
        }

        /// <summary>
        /// Deletes an <see cref="ActiveStatusEffect"/> from the database using the <see cref="ActiveStatusEffectID"/>.
        /// </summary>
        /// <param name="id">The <see cref="ActiveStatusEffectID"/> to delete form the database.</param>
        static void DeleteFromDatabase(ActiveStatusEffectID id)
        {
            _deleteQuery.Execute(id);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            var currentTime = GetTime();

            // Update or remove each ActiveStatusEffect
            foreach (var item in _statusEffects)
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
        public override IEnumerator<ActiveStatusEffect> GetEnumerator()
        {
            foreach (var item in _statusEffects)
            {
                yield return item.Value;
            }
        }

        /// <summary>
        /// Gets the number of seconds left for an <see cref="ActiveStatusEffect"/>.
        /// </summary>
        /// <param name="ase">The <see cref="ActiveStatusEffect"/>.</param>
        /// <returns>The number of seconds left for an <see cref="ActiveStatusEffect"/>.</returns>
        int GetSecsLeft(ActiveStatusEffect ase)
        {
            var secsLeft = (int)Math.Round((ase.DisableTime - GetTime()) / 1000f);
            if (secsLeft < 1)
                secsLeft = 1;

            return secsLeft;
        }

        /// <summary>
        /// When overridden in the derived class, handles removing an expired StatusEffect.
        /// </summary>
        /// <param name="activeStatusEffect">StatusEffect to be removed.</param>
        protected override void HandleExpired(ActiveStatusEffect activeStatusEffect)
        {
            // Loop through the _statusEffects list until we find 'activeStatusEffect'
            for (var i = 0; i < _statusEffects.Count; i++)
            {
                var item = _statusEffects[i];
                if (item.Value != activeStatusEffect)
                    continue;

                // Found it - now remove it from the list
                _statusEffects.RemoveAt(i);

                // Raise removal event
                OnRemoved(activeStatusEffect);

                // Remove from database
                DeleteFromDatabase(item.ID);

                return;
            }

            Debug.Fail("Couldn't find the activeStatusEffect in the collection. Where'd it go...?");
        }

        /// <summary>
        /// Inserts a new <see cref="ActiveStatusEffect"/> into the database.
        /// </summary>
        /// <param name="item">The <see cref="ActiveStatusEffect"/> to insert.</param>
        /// <returns>The <see cref="ActiveStatusEffectID"/> for the <paramref name="item"/>.</returns>
        ActiveStatusEffectID InsertInDatabase(ActiveStatusEffect item)
        {
            // Convert the time
            var secsLeft = GetSecsLeft(item);

            // Create the row
            var values = new CharacterStatusEffectTable
            {
                CharacterID = Character.ID,
                ID = new ActiveStatusEffectID(_dbController.ConnectionPool.AutoIncrementValue),
                Power = item.Power,
                TimeLeftSecs = (ushort)secsLeft,
                StatusEffect = item.StatusEffect.StatusEffectType
            };

            // Insert the data, and get the ID
            long id;
            _insertQuery.ExecuteWithResult(values, out id);

            Debug.Assert(id <= int.MaxValue);
            Debug.Assert(id >= int.MinValue);

            // Return the ID
            return new ActiveStatusEffectID((int)id);
        }

        /// <summary>
        /// Loads the status effects for the <see cref="CharacterStatusEffects.Character"/> that this collection belongs to from
        /// the database.
        /// </summary>
        public void Load()
        {
            Debug.Assert(_statusEffects.Count == 0, "Why is Load() being called while there are active effects here already?");

            var values = Character.DbController.GetQuery<SelectCharacterStatusEffectsQuery>().Execute(Character.ID);

            var currentTime = GetTime();

            // Load in the ActiveStatusEffects using the values read from the database
            foreach (var value in values)
            {
                var statusEffect = _statusEffectManager.Get(value.StatusEffect);
                if (statusEffect == null)
                {
                    const string errmsg = "Failed to get the StatusEffectBase for StatusEffectType `{0}` on Character `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, value.StatusEffect, Character);
                    Debug.Fail(string.Format(errmsg, value.StatusEffect, Character));
                    continue;
                }

                var ase = new ActiveStatusEffect(statusEffect, value.Power, (TickCount)(value.TimeLeftSecs * 1000 + currentTime));
                var aseWithID = new ASEWithID(value.ID, ase);
                _statusEffects.Add(aseWithID);
                OnAdded(ase);
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
        /// <exception cref="ArgumentNullException"><paramref name="statusEffect" /> is <c>null</c>.</exception>
        public override bool TryAdd(IStatusEffect<StatType, StatusEffectType> statusEffect, ushort power)
        {
            if (statusEffect == null)
                throw new ArgumentNullException("statusEffect");

            ASEWithID existingStatusEffect;
            var alreadyExists = TryGetStatusEffect(statusEffect.StatusEffectType, out existingStatusEffect);

            var time = GetTime();
            var disableTime = (TickCount)(time + statusEffect.GetEffectTime(power));

            if (alreadyExists)
            {
                // Status effect already exists - merge with it
                var changed = existingStatusEffect.Value.MergeWith(time, power, disableTime);
                if (changed)
                {
                    RecalculateStatBonuses();
                    UpdateInDatabase(existingStatusEffect);
                }
                return changed;
            }
            else
            {
                // Status effect doesn't exist - create new instance
                var ase = new ActiveStatusEffect(statusEffect, power, disableTime);

                var id = InsertInDatabase(ase);

                var aseWithID = new ASEWithID(id, ase);
                _statusEffects.Add(aseWithID);

                OnAdded(aseWithID.Value);

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

        /// <summary>
        /// Updates a <see cref="ActiveStatusEffect"/> in the database.
        /// </summary>
        /// <param name="item">The <see cref="ActiveStatusEffect"/> and <see cref="ActiveStatusEffectID"/> to update.</param>
        void UpdateInDatabase(ASEWithID item)
        {
            // Convert the time
            var secsLeft = GetSecsLeft(item.Value);

            // Create the row
            var values = new CharacterStatusEffectTable
            {
                CharacterID = Character.ID,
                ID = item.ID,
                Power = item.Value.Power,
                TimeLeftSecs = (ushort)secsLeft,
                StatusEffect = item.Value.StatusEffect.StatusEffectType
            };

            // Update the row
            _updateQuery.Execute(values);
        }

        /// <summary>
        /// A struct of a <see cref="ActiveStatusEffect"/> and corresponding <see cref="ActiveStatusEffectID"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public struct ASEWithID
        {
            readonly ActiveStatusEffectID _id;
            readonly ActiveStatusEffect _value;

            /// <summary>
            /// Initializes a new instance of the <see cref="ASEWithID"/> struct.
            /// </summary>
            /// <param name="id">The <see cref="ActiveStatusEffectID"/>.</param>
            /// <param name="value">The <see cref="ActiveStatusEffect"/>.</param>
            public ASEWithID(ActiveStatusEffectID id, ActiveStatusEffect value)
            {
                _id = id;
                _value = value;
            }

            /// <summary>
            /// Gets the <see cref="ActiveStatusEffectID"/>.
            /// </summary>
            public ActiveStatusEffectID ID
            {
                get { return _id; }
            }

            /// <summary>
            /// Gets the <see cref="ActiveStatusEffect"/>.
            /// </summary>
            public ActiveStatusEffect Value
            {
                get { return _value; }
            }
        }
    }
}