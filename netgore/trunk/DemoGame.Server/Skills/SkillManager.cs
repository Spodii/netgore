using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame;
using log4net;
using NetGore;
using NetGore.Collections;

namespace DemoGame.Server
{
    /// <summary>
    /// Manages all of the individual SkillBases for the corresponding SkillType. Skill instances should be acquired
    /// through this manager instead of creating new instances of the class.
    /// </summary>
    public static class SkillManager
    {
        /// <summary>
        /// Dictionary that allows for lookup of a SkillBase for the given SkillType.
        /// </summary>
        static readonly Dictionary<SkillType, SkillBase> _skills =
            new Dictionary<SkillType, SkillBase>(EnumComparer<SkillType>.Instance);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SkillManager static constructor.
        /// </summary>
        static SkillManager()
        {
            // Get the Types for the classes that inherit SkillBase
            var types = TypeHelper.FindTypesThatInherit(typeof(SkillBase), Type.EmptyTypes, false);

            // Filter out the invalid derived Types
            types = types.Where(x => x.IsClass && !x.IsAbstract);

            // Create an instance of each of the valid derived classes
            foreach (Type type in types)
            {
                SkillBase instance = (SkillBase)Activator.CreateInstance(type, true);

                if (_skills.ContainsKey(instance.SkillType))
                {
                    const string errmsg = "Skills Dictionary already contains SkillType `{0}`.";
                    Debug.Fail(string.Format(errmsg, instance.SkillType));
                    if (log.IsFatalEnabled)
                        log.FatalFormat(errmsg, instance.SkillType);
                    throw new Exception(string.Format(errmsg, instance.SkillType));
                }

                _skills.Add(instance.SkillType, instance);

                if (log.IsInfoEnabled)
                    log.InfoFormat("Created skill object for SkillType `{0}`.", instance.SkillType);
            }
        }

        /// <summary>
        /// Gets the SkillBase for the given SkillType.
        /// </summary>
        /// <param name="skillType">The SkillType to get the SkillBase for.</param>
        /// <returns>The SkillBase for the given <paramref name="skillType"/>, or null if the <paramref name="skillType"/>
        /// is invalid or contains no SkillBase.</returns>
        public static SkillBase GetSkill(SkillType skillType)
        {
            SkillBase value;
            if (!_skills.TryGetValue(skillType, out value))
            {
                const string errmsg = "Failed to get the SkillBase for SkillType `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, skillType);
                Debug.Fail(string.Format(errmsg, skillType));
                return null;
            }

            return value;
        }
    }
}