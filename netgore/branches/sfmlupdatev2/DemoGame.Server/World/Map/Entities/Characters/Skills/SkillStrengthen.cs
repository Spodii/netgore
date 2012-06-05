using System.Linq;

namespace DemoGame.Server
{
    public class SkillStrengthen : SkillBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkillStrengthen"/> class.
        /// </summary>
        SkillStrengthen() : base(DemoGame.SkillType.Strengthen, 10000, 3000)
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

            int power = user.ModStats[StatType.Int];
            var successful = target.StatusEffects.TryAdd(StatusEffectType.Strengthen, (ushort)power);

            return successful;
        }
    }
}