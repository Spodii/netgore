using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.Features.Skills
{
    /// <summary>
    /// A manager for a collection of ISkills.
    /// </summary>
    /// <typeparam name="TSkillType">The type of skill type enum.</typeparam>
    /// <typeparam name="TStatType">The type of stat type enum.</typeparam>
    /// <typeparam name="TCharacter">The type of character.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public class SkillManager<TSkillType, TStatType, TCharacter>
        where TSkillType : struct, IComparable, IConvertible, IFormattable
        where TStatType : struct, IComparable, IConvertible, IFormattable where TCharacter : class
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dictionary that allows for lookup of an ISkill for the given SkillType.
        /// </summary>
        readonly Dictionary<TSkillType, ISkill<TSkillType, TStatType, TCharacter>> _skills =
            new Dictionary<TSkillType, ISkill<TSkillType, TStatType, TCharacter>>(EnumComparer<TSkillType>.Instance);

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillManager{TSkillType, TStatType, TCharacter}"/> class.
        /// </summary>
        public SkillManager()
        {
            new TypeFactory(GetTypeFilter(), TypeFactoryLoadedHandler);
        }

        /// <summary>
        /// Gets the ISkill for the given skill type.
        /// </summary>
        /// <param name="skillType">The skill type to get the ISkill for.</param>
        /// <returns>The ISkill for the given <paramref name="skillType"/>, or null if the <paramref name="skillType"/>
        /// is invalid or contains no ISkill.</returns>
        public ISkill<TSkillType, TStatType, TCharacter> GetSkill(TSkillType skillType)
        {
            ISkill<TSkillType, TStatType, TCharacter> value;
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

        static Func<Type, bool> GetTypeFilter()
        {
            var filterCreator = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                Interfaces = new Type[] { typeof(ISkill<TSkillType, TStatType, TCharacter>) },
                ConstructorParameters = Type.EmptyTypes,
                RequireConstructor = true
            };

            return filterCreator.GetFilter();
        }

        /// <summary>
        /// Handles when a new type has been loaded into a <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="typeFactory">The type factory.</param>
        /// <param name="e">The <see cref="NetGore.Collections.TypeFactoryLoadedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="DuplicateKeyException">The loaded type in <paramref name="e"/> was already loaded.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "SkillType")]
        void TypeFactoryLoadedHandler(TypeFactory typeFactory, TypeFactoryLoadedEventArgs e)
        {
            var instance = (ISkill<TSkillType, TStatType, TCharacter>)TypeFactory.GetTypeInstance(e.LoadedType);

            if (_skills.ContainsKey(instance.SkillType))
            {
                const string errmsg = "Skills Dictionary already contains SkillType `{0}`.";
                Debug.Fail(string.Format(errmsg, instance.SkillType));
                if (log.IsFatalEnabled)
                    log.FatalFormat(errmsg, instance.SkillType);
                throw new DuplicateKeyException(string.Format(errmsg, instance.SkillType));
            }

            _skills.Add(instance.SkillType, instance);

            if (log.IsInfoEnabled)
                log.InfoFormat("Created skill object for SkillType `{0}`.", instance.SkillType);
        }
    }
}