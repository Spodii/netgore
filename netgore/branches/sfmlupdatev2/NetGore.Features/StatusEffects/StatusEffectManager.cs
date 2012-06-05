using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.Features.StatusEffects
{
    /// <summary>
    /// Manages all of the individual <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>s for the corresponding
    /// <typeparamref name="TStatusEffectType"/>. <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>
    /// instances should be acquired through this manager instead of creating new instances of the class.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    /// <typeparam name="TStatusEffectType">The type of status effect.</typeparam>
    public class StatusEffectManager<TStatType, TStatusEffectType>
        where TStatType : struct, IComparable, IConvertible, IFormattable
        where TStatusEffectType : struct, IComparable, IConvertible, IFormattable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dictionary that allows for lookup of a <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>
        /// for the given <typeparamref name="TStatusEffectType"/>.
        /// </summary>
        readonly Dictionary<TStatusEffectType, IStatusEffect<TStatType, TStatusEffectType>> _statusEffects =
            new Dictionary<TStatusEffectType, IStatusEffect<TStatType, TStatusEffectType>>(
                EnumComparer<TStatusEffectType>.Instance);

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectManager&lt;TStatType, TStatusEffectType&gt;"/> class.
        /// </summary>
        public StatusEffectManager() : this(GetDefaultFilter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectManager&lt;TStatType, TStatusEffectType&gt;"/> class.
        /// </summary>
        /// <param name="filter">The filter used to get the <see cref="Type"/>s to try to instantiate.</param>
        public StatusEffectManager(Func<Type, bool> filter)
        {
            new TypeFactory(filter, HandleLoadType);
        }

        /// <summary>
        /// Gets the <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> for the given
        /// <paramref name="statusEffectType"/>.
        /// </summary>
        /// <param name="statusEffectType">The <typeparamref name="TStatusEffectType"/> of the
        /// <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> instance to get.</param>
        /// <returns>The <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> for the given
        /// <paramref name="statusEffectType"/>, or null if invalid.</returns>
        public IStatusEffect<TStatType, TStatusEffectType> Get(TStatusEffectType statusEffectType)
        {
            IStatusEffect<TStatType, TStatusEffectType> ret;
            if (!_statusEffects.TryGetValue(statusEffectType, out ret))
                return null;

            return ret;
        }

        /// <summary>
        /// Creates the default type filter.
        /// </summary>
        /// <returns>The default type filter.</returns>
        static Func<Type, bool> GetDefaultFilter()
        {
            var typeFilterCreator = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                ConstructorParameters = Type.EmptyTypes,
                RequireConstructor = true,
                Interfaces = new Type[] { typeof(IStatusEffect<TStatType, TStatusEffectType>) }
            };

            return typeFilterCreator.GetFilter();
        }

        /// <summary>
        /// Handles when a type is loaded into the <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="e">The <see cref="NetGore.Collections.TypeFactoryLoadedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="DuplicateKeyException">The loaded type in <paramref name="e"/> was already loaded.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "StatusEffectType")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "StatusEffects")]
        void HandleLoadType(TypeFactory factory, TypeFactoryLoadedEventArgs e)
        {
            // Create the instance
            var instance = (IStatusEffect<TStatType, TStatusEffectType>)TypeFactory.GetTypeInstance(e.LoadedType);

            // Ensure we don't already have the StatusEffectType being handled
            if (_statusEffects.ContainsKey(instance.StatusEffectType))
            {
                const string errmsg = "StatusEffects Dictionary already contains StatusEffectType `{0}`.";
                Debug.Fail(string.Format(errmsg, instance.StatusEffectType));
                if (log.IsFatalEnabled)
                    log.FatalFormat(errmsg, instance.StatusEffectType);
                throw new DuplicateKeyException(string.Format(errmsg, instance.StatusEffectType));
            }

            // Add the instance
            _statusEffects.Add(instance.StatusEffectType, instance);

            if (log.IsInfoEnabled)
                log.InfoFormat("Created status effect object `{0}` for StatusEffectType `{1}`.", instance,
                    instance.StatusEffectType);
        }
    }
}