using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server.Skills
{
    public class SkillHeal : SkillBase
    {
        /// <summary>
        /// When overridden in the derived class, gets the MP cost of using this Skill.
        /// </summary>
        /// <param name="user">The Character using the skill. Will not be null.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>The MP cost of using this Skill.</returns>
        public override SPValueType GetMPCost(Character user, Character target)
        {
            return 2;
        }

        /// <summary>
        /// When overridden in the derived class, gets the type of skill that this class is for.
        /// </summary>
        public override SkillType SkillType
        {
            get { return SkillType.Heal; }
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

            int power = user.ModStats[StatType.Int] * 2 + 5;
            target.HP += (SPValueType)power;

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, gets if this Skill requires a target to be specified for the skill
        /// to be used. If this is false, the skill will never even attempt to be used unless there is a target.
        /// </summary>
        public override bool RequiresTarget
        {
            get { return false; }
        }
    }
}
