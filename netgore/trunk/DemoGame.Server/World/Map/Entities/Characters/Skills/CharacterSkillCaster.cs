using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.Features.Skills;
using NetGore.Network;
using NetGore.World;
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
        /// If the character is currently casting a skill, then attempts to cancel the skill.
        /// </summary>
        /// <returns>True if there was a skill being casted and it was successfully stopped; false if there was
        /// no skill being casted or the skill could not be canceled.</returns>
        public bool TryCancelCastingSkill()
        {
            if (!IsCastingSkill)
                return false;

            Debug.Assert(_currentCastingSkill != null);

            // Clear the casting status variables
            _currentCastingSkill = null;
            _castingSkillTarget = null;

            // Tell the caster that they stopped casting
            if (_character is INetworkSender)
            {
                using (var pw = ServerPacket.SkillStopCasting_ToUser())
                {
                    ((INetworkSender)_character).Send(pw, ServerMessageType.GUIUserStatus);
                }
            }

            // Tell everyone that the casting stopped
            using (var pw = ServerPacket.SkillStopCasting_ToMap(_character.MapEntityIndex))
            {
                _character.Map.Send(pw, ServerMessageType.MapDynamicEntityProperty);
            }

            return true;
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

            // Check that the character knows the skill
            if (!_character.KnownSkills.Knows(skill.SkillType))
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

            if (skill.CastingTime == 0)
            {
                // The skill to use has no usage delay, so use it immediately
                UseSkill(skill, target, true);
            }
            else
            {
                // The skill does have a delay, so queue it for usage
                _currentCastingSkill = skill;
                _castingSkillTarget = target;
                var castingTime = skill.CastingTime;
                _castingSkillUsageTime = _character.GetTime() + castingTime;

                // Tell the character the details about the skill they are casting
                if (_character is INetworkSender)
                {
                    using (var pw = ServerPacket.SkillStartCasting_ToUser(skill.SkillType, castingTime))
                    {
                        ((INetworkSender)_character).Send(pw, ServerMessageType.GUIUserStatus);
                    }
                }

                // Tell the users on the map that the character is casting
                using (var pw = ServerPacket.SkillStartCasting_ToMap(_character.MapEntityIndex, skill.SkillType))
                {
                    _character.Map.Send(pw, ServerMessageType.MapDynamicEntityProperty);
                }
            }

            return true;
        }

        /// <summary>
        /// Updates the Character's skill casting progress.
        /// </summary>
        public void Update()
        {
            // Check if the character is currently casting a skill with a casting delay
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
            UseSkill(_currentCastingSkill, _castingSkillTarget, false);
        }

        /// <summary>
        /// Calls a skill to be used, and applies the respective cooldown values.
        /// </summary>
        /// <param name="skill">The skill to be used.</param>
        /// <param name="target">The optional character to use the skill on. Can be null.</param>
        /// <param name="castedImmediately">True if this skill was casted immediately (that is, there was no delay before casting);
        /// false if the skill had a delay before casting.</param>
        void UseSkill(ISkill<SkillType, StatType, Character> skill, Character target, bool castedImmediately)
        {
            Debug.Assert(skill != null);
            Debug.Assert(castedImmediately || _currentCastingSkill == skill);
            Debug.Assert(castedImmediately || _castingSkillTarget == target);

            // Clear the casting status variables
            _currentCastingSkill = null;
            _castingSkillTarget = null;

            // Tell the caster that they stopped casting
            if (_character is INetworkSender)
            {
                using (var pw = ServerPacket.SkillStopCasting_ToUser())
                {
                    ((INetworkSender)_character).Send(pw, ServerMessageType.GUIUserStatus);
                }
            }

            // If the skill had a casting time, tell everyone that the casting stopped
            if (!castedImmediately)
            {
                using (var pw = ServerPacket.SkillStopCasting_ToMap(_character.MapEntityIndex))
                {
                    _character.Map.Send(pw, ServerMessageType.MapDynamicEntityProperty);
                }
            }

            // Actually use the skill
            var skillSuccessfullyUsed = skill.Use(_character, target);

            // If the skill was not used for whatever reason, then return
            if (!skillSuccessfullyUsed)
                return;

            // Only set the cooldown if it was successfully used
            _cooldownManager.SetCooldown(skill.CooldownGroup, skill.CooldownTime, _character.GetTime());

            // Update the character's skill cooldown
            if (_character is INetworkSender)
            {
                using (var pw = ServerPacket.SkillSetGroupCooldown(skill.CooldownGroup, skill.CooldownTime))
                {
                    ((INetworkSender)_character).Send(pw, ServerMessageType.GUIUserStatus);
                }
            }

            // Notify the clients on the map that the character used the skill
            var targetEntityIndex = target != null ? (MapEntityIndex?)target.MapEntityIndex : null;
            using (var pw = ServerPacket.SkillUse(_character.MapEntityIndex, targetEntityIndex, skill.SkillType))
            {
                _character.Map.Send(pw, ServerMessageType.MapDynamicEntityProperty);
            }
        }
    }
}