using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Features.Skills;

namespace DemoGame.Server
{
    /// <summary>
    /// The base class for skills that describes each individual skill.
    /// </summary>
    public abstract class SkillBase : SkillBase<SkillType, StatType, Character>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillBase"/> class.
        /// </summary>
        /// <param name="skillType">The type of skill that this object instance is for.</param>
        /// <param name="cooldownTime">The cooldown time.</param>
        /// <param name="castingTime">The casting time.</param>
        protected SkillBase(SkillType skillType, ushort cooldownTime, ushort castingTime)
            : base(skillType, GetCooldownGroup(skillType), cooldownTime, castingTime)
        {
        }

        /// <summary>
        /// Allows for additional processing for after the skill has been successfully used.
        /// </summary>
        /// <param name="user">User to make use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        protected override void AfterUseSkill(Character user, Character target)
        {
            // Apply the HP and MP costs
            var hpCost = GetHPCost(user, target);
            if (hpCost != 0)
                user.HP -= hpCost;

            var mpCost = GetMPCost(user, target);
            if (mpCost != 0)
                user.MP -= mpCost;

            base.AfterUseSkill(user, target);
        }

        /// <summary>
        /// Checks if the given Character can use this Skill.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>
        /// True if the <paramref name="user"/> can use this Skill; otherwise false.
        /// </returns>
        public override bool CanUse(Character user, Character target)
        {
            // Check for the required HP and MP
            var mpCost = GetMPCost(user, target);
            if (mpCost >= user.MP)
                return false;

            var hpCost = GetHPCost(user, target);
            if (hpCost >= user.HP)
                return false;

            return base.CanUse(user, target);
        }

        /// <summary>
        /// Does the CanUse checks to see if the given Characters are in a valid state.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>
        /// True if the <paramref name="user"/> can use this Skill; otherwise false.
        /// </returns>
        protected override bool CheckValidCanUseCharacters(Character user, Character target)
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

            return base.CheckValidCanUseCharacters(user, target);
        }

        /// <summary>
        /// Gets the cooldown group for the skill type.
        /// </summary>
        /// <param name="skillType">The skill type.</param>
        /// <returns>The cooldown group for the skill type.</returns>
        static byte GetCooldownGroup(SkillType skillType)
        {
            return SkillInfoManager.Instance[skillType].CooldownGroup;
        }

        /// <summary>
        /// When overridden in the derived class, gets the HP cost of using this Skill.
        /// </summary>
        /// <param name="user">The Character using the skill. Will not be null.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>The HP cost of using this Skill.</returns>
        public virtual int GetHPCost(Character user, Character target)
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
        public virtual int GetMPCost(Character user, Character target)
        {
            return 0;
        }

        /// <summary>
        /// When overridden in the derived class, checks if the given <paramref name="character"/> has the required stats
        /// for using this Skill.
        /// </summary>
        /// <param name="character">The Character using the skill. Will not be null.</param>
        /// <returns>
        /// True if the <paramref name="character"/> has the required stats to use this skill; otherwise false.
        /// </returns>
        public override bool HasRequiredStats(Character character)
        {
            var reqStats = RequiredStats;
            if (reqStats == null)
                return true;

            return character.ModStats.HasAllGreaterOrEqualValues(reqStats);
        }
    }
}