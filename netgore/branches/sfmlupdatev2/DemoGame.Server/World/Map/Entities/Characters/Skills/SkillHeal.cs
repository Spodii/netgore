using System.Linq;

namespace DemoGame.Server
{
    public class SkillHeal : SkillBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkillHeal"/> class.
        /// </summary>
        SkillHeal() : base(DemoGame.SkillType.Heal, 2000, 0)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets if this Skill requires a target to be specified for the skill
        /// to be used. If this is false, the skill will never even attempt to be used unless there is a target.
        /// </summary>
        public override bool RequiresTarget
        {
            get { return false; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the MP cost of using this Skill.
        /// </summary>
        /// <param name="user">The Character using the skill. Will not be null.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>The MP cost of using this Skill.</returns>
        public override int GetMPCost(Character user, Character target)
        {
            return 2;
        }

        /// <summary>
        /// When overridden in the derived class, makes the <paramref name="user"/> Character use this skill.
        /// </summary>
        /// <param name="user">The Character that used this skill. Will never be null.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the skill was successfully used; otherwise false.</returns>
        protected override bool HandleUse(Character user, Character target)
        {
            if (target == null)
                target = user;

            var power = user.ModStats[StatType.Int] * 2 + 5;
            target.HP += power;

            return true;
        }
    }
}