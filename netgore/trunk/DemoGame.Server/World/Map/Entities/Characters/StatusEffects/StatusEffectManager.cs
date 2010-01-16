using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Collections;

namespace DemoGame.Server
{
    /// <summary>
    /// Manages all of the individual <see cref="StatusEffectBase"/>s for the corresponding <see cref="StatusEffectType"/>.
    /// <see cref="StatusEffectBase"/> instances should be acquired through this manager instead of creating new
    /// instances of the class.
    /// </summary>
    public static class StatusEffectManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dictionary that allows for lookup of a <see cref="StatusEffectBase"/> for the given <see cref="StatusEffectType"/>.
        /// </summary>
        static readonly Dictionary<StatusEffectType, StatusEffectBase> _statusEffects =
            new Dictionary<StatusEffectType, StatusEffectBase>(EnumComparer<StatusEffectType>.Instance);

        /// <summary>
        /// Initializes the <see cref="StatusEffectManager"/> class.
        /// </summary>
        static StatusEffectManager()
        {
            // Get the Types for the classes that inherit StatusEffectBase
            var types = TypeHelper.FindTypesThatInherit(typeof(StatusEffectBase), Type.EmptyTypes, false);

            // Filter out the invalid derived Types
            types = types.Where(x => x.IsClass && !x.IsAbstract);

            // Create an instance of each of the valid derived classes
            foreach (Type type in types)
            {
                StatusEffectBase instance = (StatusEffectBase)TypeFactory.GetTypeInstance(type);

                if (_statusEffects.ContainsKey(instance.StatusEffectType))
                {
                    const string errmsg = "StatusEffects Dictionary already contains StatusEffectType `{0}`.";
                    Debug.Fail(string.Format(errmsg, instance.StatusEffectType));
                    if (log.IsFatalEnabled)
                        log.FatalFormat(errmsg, instance.StatusEffectType);
                    throw new Exception(string.Format(errmsg, instance.StatusEffectType));
                }

                _statusEffects.Add(instance.StatusEffectType, instance);

                if (log.IsInfoEnabled)
                    log.InfoFormat("Created status effect object for StatusEffectType `{0}`.", instance.StatusEffectType);
            }
        }

        /// <summary>
        /// Gets the <see cref="StatusEffectBase"/> for the given <see cref="StatusEffectType"/>.
        /// </summary>
        /// <param name="statusEffectType">The <see cref="StatusEffectType"/> to get the <see cref="StatusEffectBase"/>
        /// for.</param>
        /// <returns>The <see cref="StatusEffectBase"/> for the given <paramref name="statusEffectType"/>,
        /// or null if the <paramref name="statusEffectType"/> is invalid or contains no
        /// <see cref="StatusEffectBase"/>.</returns>
        public static StatusEffectBase GetStatusEffect(StatusEffectType statusEffectType)
        {
            StatusEffectBase value;
            if (!_statusEffects.TryGetValue(statusEffectType, out value))
            {
                const string errmsg = "Failed to get the StatusEffectBase for StatusEffectType `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, statusEffectType);
                Debug.Fail(string.Format(errmsg, statusEffectType));
                return null;
            }

            return value;
        }
    }
}