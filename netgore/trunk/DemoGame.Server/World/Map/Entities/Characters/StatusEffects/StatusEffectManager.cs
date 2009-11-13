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
    /// Manages all of the individual StatusEffectBases for the corresponding StatusEffectType. StatusEffect instances
    /// should be acquired through this manager instead of creating new instances of the class.
    /// </summary>
    public static class StatusEffectManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dictionary that allows for lookup of a StatusEffectBase for the given StatusEffectType.
        /// </summary>
        static readonly Dictionary<StatusEffectType, StatusEffectBase> _statusEffects =
            new Dictionary<StatusEffectType, StatusEffectBase>(EnumComparer<StatusEffectType>.Instance);

        /// <summary>
        /// StatusEffectManager static constructor.
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
                StatusEffectBase instance = (StatusEffectBase)Activator.CreateInstance(type, true);

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
        /// Gets the StatusEffectBase for the given StatusEffectType.
        /// </summary>
        /// <param name="statusEffectType">The StatusEffectType to get the StatusEffectBase for.</param>
        /// <returns>The StatusEffectBase for the given <paramref name="statusEffectType"/>,
        /// or null if the <paramref name="statusEffectType"/> is invalid or contains no StatusEffectBase.</returns>
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