using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore.Collections;

namespace DemoGame.Server
{
    public static class SkillManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly Dictionary<SkillType, SkillBase> _skills = new Dictionary<SkillType, SkillBase>(EnumComparer<SkillType>.Instance); 

        static SkillManager()
        {
            var types = NetGore.TypeHelper.FindTypesThatInherit(typeof(SkillBase), Type.EmptyTypes, false);
            foreach (var type in types)
            {
                var instance = (SkillBase)Activator.CreateInstance(type, true);

                if (_skills.ContainsKey(instance.SkillType))
                {
                    const string errmsg = "Skills Dictionary already contains SkillType `{0}`.";
                    Debug.Fail(string.Format(errmsg, instance.SkillType));
                    if (log.IsFatalEnabled)
                        log.FatalFormat(errmsg, instance.SkillType);
                    throw new Exception(string.Format(errmsg, instance.SkillType));
                }

                _skills.Add(instance.SkillType, instance);
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
                if (log.IsWarnEnabled)
                    log.WarnFormat("Failed to get the SkillBase for SkillType `{0}`.", skillType);
                return null;
            }

            return value;
        }
    }
}
