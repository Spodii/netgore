using System.Linq;
using NetGore;
using NetGore.Features.Skills;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// Provides all the handling for making a Character use skills.
    /// </summary>
    public class CharacterSkillCaster
    {
        readonly Character _character;
        readonly ISkillCooldownManager _cooldownManager;

        Character _castingSkillTarget;
        TickCount _castingSkillUsageTime;
        ISkill<SkillType, StatType, Character> _currentCastingSkill;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSkillCaster"/> class.
        /// </summary>
        /// <param name="character">The character.</param>
        public CharacterSkillCaster(Character character)
        {
            _character = character;
            _cooldownManager = new SkillCooldownManager();
        }

        /// <summary>
        /// Gets the skill that the user is currently casting.
        /// </summary>
        public ISkill<SkillType, StatType, Character> CurrentCastingSkill
        {
            get { return _currentCastingSkill; }
        }

        /// <summary>
        /// Gets if the Character is currently casting a skill.
        /// </summary>
        public bool IsCastingSkill
        {
            get { return CurrentCastingSkill != null; }
        }

        /// <summary>
        /// Tells the skill user to start using the given <paramref name="skill"/>. This is more of a suggestion than a
        /// request since the skill user does not actually have to start casting the skill. If this skill user decides
        /// to use the skill, the CurrentCastingSkill must be set to the <paramref name="skill"/>.
        /// </summary>
        /// <param name="skill">The skill to be used.</param>
        /// <param name="target">The optional character to use the skill on. Can be null.</param>
        /// <returns>True if the <paramref name="skill"/> started being casted; otherwise false. This does not indicate
        /// whether or not the skill was or will be successfully used, just that it was attempted to be used. Common
        /// times this will return false is if there is a skill already being casted, or if the skill that was
        /// attempted to be used still needs to cool down.</returns>
        public bool TryStartCastingSkill(ISkill<SkillType, StatType, Character> skill, Character target)
        {
            if (!_character.IsAlive)
                return false;

            // Don't interrupt a skill that the character is already casting
            if (IsCastingSkill)
                return false;

            // Check that the group is available for usage
            if (_cooldownManager.IsCoolingDown(skill.CooldownGroup, _character.GetTime()))
                return false;

            // Only allow immediate-usage skills when moving
            if (_character.Velocity != Vector2.Zero && skill.CastingTime > 0)
                return false;

            // If the skill to use has no usage delay, use it immediately
            if (skill.CastingTime == 0)
                UseSkill(skill, target);
            else
            {
                // The skill does have a delay, so queue it for usage
                _currentCastingSkill = skill;
                _castingSkillTarget = target;
                var castingTime = skill.CastingTime;
                _castingSkillUsageTime = _character.GetTime() + castingTime;

                if (_character is User)
                {
                    using (var pw = ServerPacket.StartCastingSkill(skill.SkillType, castingTime))
                    {
                        ((User)_character).Send(pw);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Updates the Character's skill casting progress.
        /// </summary>
        public void Update()
        {
            if (!IsCastingSkill)
                return;

            // Check if it is time to use the skill
            var currentTime = _character.GetTime();
            if (currentTime <= _castingSkillUsageTime)
            {
                // Not yet time to use
                return;
            }

            // Use the skill
            UseSkill(_currentCastingSkill, _castingSkillTarget);

            _currentCastingSkill = null;
            _castingSkillTarget = null;
        }

        /// <summary>
        /// Calls a skill to be used, and applies the respective cooldown values.
        /// </summary>
        /// <param name="skill">The skill to be used.</param>
        /// <param name="target">The optional character to use the skill on. Can be null.</param>
        void UseSkill(ISkill<SkillType, StatType, Character> skill, Character target)
        {
            var successful = skill.Use(_character, target);

            if (successful)
            {
                // Only set the cooldown if it was successfully used
                _cooldownManager.SetCooldown(skill.CooldownGroup, skill.CooldownTime, _character.GetTime());

                // Notify the user about the new cooldown
                if (_character is User)
                {
                    using (var pw = ServerPacket.SetSkillGroupCooldown(skill.CooldownGroup, skill.CooldownTime))
                    {
                        ((User)_character).Send(pw);
                    }
                }

                // Notify the clients in view that the character used a skill
                using (var pw = ServerPacket.UseSkill(_character.MapEntityIndex, null, skill.SkillType))
                {
                    _character.Map.SendToArea(_character, pw);
                }
            }
        }
    }
}