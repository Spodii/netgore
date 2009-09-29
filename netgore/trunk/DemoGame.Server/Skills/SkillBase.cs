using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.RPGComponents;

namespace DemoGame.Server
{
    /// <summary>
    /// The base class for skills that describes each individual skill.
    /// </summary>
    public abstract class SkillBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly SkillStatCollection _requiredStats = new SkillStatCollection();
        readonly SkillType _skillType;

        /// <summary>
        /// Gets the collection of stats required by this Skill.
        /// </summary>
        public IStatCollection RequiredStats
        {
            get { return _requiredStats; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if this Skill requires a target to be specified for the skill
        /// to be used. If this is false, the skill will never even attempt to be used unless there is a target.
        /// </summary>
        public abstract bool RequiresTarget { get; }

        /// <summary>
        /// Gets the type of skill that this class is for.
        /// </summary>
        public SkillType SkillType
        {
            get { return _skillType; }
        }

        /// <summary>
        /// SkillBase constructor.
        /// </summary>
        /// <param name="skillType">The type of skill that this class is for.</param>
        protected SkillBase(SkillType skillType)
        {
            _skillType = skillType;
        }

        /// <summary>
        /// Checks if the given Character can use this Skill.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <returns>True if the <paramref name="user"/> can use this Skill; otherwise false.</returns>
        public bool CanUse(Character user)
        {
            return CanUse(user, null);
        }

        /// <summary>
        /// Checks if the given Character can use this Skill.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the <paramref name="user"/> can use this Skill; otherwise false.</returns>
        public virtual bool CanUse(Character user, Character target)
        {
            // State checks
            if (!CheckValidCanUseCharacters(user, target))
                return false;

            // Check for a target
            if (RequiresTarget && target == null)
                return false;

            // Check for the required HP and MP
            int mpCost = GetMPCost(user, target);
            if (mpCost >= user.MP)
                return false;

            int hpCost = GetHPCost(user, target);
            if (hpCost >= user.HP)
                return false;

            // Check for the required stats
            if (!HasRequiredStats(user))
                return false;

            return true;
        }

        /// <summary>
        /// Does the CanUse checks to see if the given Characters are in a valid state.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the <paramref name="user"/> can use this Skill; otherwise false.</returns>
        protected bool CheckValidCanUseCharacters(Character user, Character target)
        {
            if (user.Map == null)
            {
                const string errmsg = "Character `{0}` attempted to use skill `{1}` while not on a map.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, user, SkillType);
                Debug.Fail(string.Format(errmsg, user, SkillType));
                return false;
            }

            if (!user.IsAlive)
            {
                const string errmsg = "Character `{0}` attempted to use skill `{1}` while dead.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, user, SkillType);
                Debug.Fail(string.Format(errmsg, user, SkillType));
                return false;
            }

            if (user.IsDisposed)
            {
                const string errmsg = "Character `{0}` attempted to use skill `{1}` while disposed.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, user, SkillType);
                Debug.Fail(string.Format(errmsg, user, SkillType));
                return false;
            }

            if (target != null)
            {
                if (target.Map == null)
                {
                    const string errmsg = "Character `{0}` attempted to use skill `{1}` on target `{2}` who is not on a map.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, user, SkillType, target);
                    Debug.Fail(string.Format(errmsg, user, SkillType, target));
                    return false;
                }

                if (!target.IsAlive)
                {
                    const string errmsg = "Character `{0}` attempted to use skill `{1}` on target `{2}` who is dead.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, user, SkillType, target);
                    Debug.Fail(string.Format(errmsg, user, SkillType, target));
                    return false;
                }

                if (target.IsDisposed)
                {
                    const string errmsg = "Character `{0}` attempted to use skill `{1}` on target `{2}` who is disposed.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, user, SkillType, target);
                    Debug.Fail(string.Format(errmsg, user, SkillType, target));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, gets the HP cost of using this Skill.
        /// </summary>
        /// <param name="user">The Character using the skill. Will not be null.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>The HP cost of using this Skill.</returns>
        public virtual SPValueType GetHPCost(Character user, Character target)
        {
            return 0;
        }

        /// <summary>
        /// When overridden in the derived class, gets the MP cost of using this Skill.
        /// </summary>
        /// <param name="user">The Character using the skill. Will not be null.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>The MP cost of using this Skill.</returns>
        public virtual SPValueType GetMPCost(Character user, Character target)
        {
            return 0;
        }

        /// <summary>
        /// When overridden in the derived class, makes the <paramref name="user"/> Character use this skill.
        /// </summary>
        /// <param name="user">The Character that used this skill. Will never be null.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the skill was successfully used; otherwise false.</returns>
        protected abstract bool HandleUse(Character user, Character target);

        /// <summary>
        /// Checks if the given <paramref name="character"/> has the required stats for using this Skill.
        /// </summary>
        /// <param name="character">The Character using the skill. Will not be null.</param>
        /// <returns>True if the <paramref name="character"/> has the required stats to use this skill; otherwise false.</returns>
        bool HasRequiredStats(Character character)
        {
            foreach (IStat reqStat in _requiredStats)
            {
                int characterStatValue;
                if (!character.ModStats.TryGetStatValue(reqStat.StatType, out characterStatValue))
                    return false;

                if (characterStatValue < reqStat.Value)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Uses this Skill without a target.
        /// </summary>
        /// <param name="user">User to make use this Skill.</param>
        /// <returns>True if the Skill was successfully used; otherwise false.</returns>
        public bool Use(Character user)
        {
            return Use(user, null);
        }

        /// <summary>
        /// Uses this Skill.
        /// </summary>
        /// <param name="user">User to make use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the Skill was successfully used; otherwise false.</returns>
        public bool Use(Character user, Character target)
        {
            // Ensure the skill can be used
            if (!CanUse(user, target))
                return false;

            // Use the skill
            bool useSuccessful = HandleUse(user, target);
            if (!useSuccessful)
                return false;

            // Apply the HP and MP costs
            SPValueType hpCost = GetHPCost(user, target);
            if (hpCost != 0)
                user.HP -= hpCost;

            SPValueType mpCost = GetMPCost(user, target);
            if (mpCost != 0)
                user.MP -= mpCost;

            return true;
        }

        /// <summary>
        /// The StatCollection used for the Skill's required stats.
        /// </summary>
        class SkillStatCollection : FixedStatCollection
        {
            /// <summary>
            /// SkillStatCollection constructor.
            /// </summary>
            public SkillStatCollection() : base(Enumerable.Empty<StatType>(), StatCollectionType.Requirement)
            {
                // TODO: !! The RequiredStats is never populated
            }
        }
    }
}