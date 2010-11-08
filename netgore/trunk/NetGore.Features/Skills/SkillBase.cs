using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.Stats;

namespace NetGore.Features.Skills
{
    /// <summary>
    /// Base class for a general implementation of the ISkill.
    /// </summary>
    /// <typeparam name="TSkillType">The type of skill type enum.</typeparam>
    /// <typeparam name="TStatType">The type of stat type enum.</typeparam>
    /// <typeparam name="TCharacter">The type of character.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public abstract class SkillBase<TSkillType, TStatType, TCharacter> : ISkill<TSkillType, TStatType, TCharacter>
        where TSkillType : struct, IComparable, IConvertible, IFormattable
        where TStatType : struct, IComparable, IConvertible, IFormattable where TCharacter : class
    {
        readonly ushort _castingTime;
        readonly byte _cooldownGroup;
        readonly ushort _cooldownTime;
        readonly TSkillType _skillType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillBase{TSkillType, TStatType, TCharacter}"/> class.
        /// </summary>
        /// <param name="skillType">The type of skill that this object instance is for.</param>
        /// <param name="cooldownGroup">The cooldown group.</param>
        /// <param name="cooldownTime">The cooldown time.</param>
        /// <param name="castingTime">The casting time.</param>
        protected SkillBase(TSkillType skillType, byte cooldownGroup, ushort cooldownTime, ushort castingTime)
        {
            _skillType = skillType;
            _cooldownGroup = cooldownGroup;
            _cooldownTime = cooldownTime;
            _castingTime = castingTime;
        }

        /// <summary>
        /// Allows for additional processing for after the skill has been successfully used.
        /// </summary>
        /// <param name="user">User to make use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        protected virtual void AfterUseSkill(TCharacter user, TCharacter target)
        {
        }

        /// <summary>
        /// Does the CanUse checks to see if the given Characters are in a valid state.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the <paramref name="user"/> can use this Skill; otherwise false.</returns>
        protected virtual bool CheckValidCanUseCharacters(TCharacter user, TCharacter target)
        {
            return true;
        }

        /// <summary>
        /// When overridden in the derived class, makes the <paramref name="user"/> Character use this skill.
        /// </summary>
        /// <param name="user">The Character that used this skill. Will never be null.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the skill was successfully used; otherwise false.</returns>
        protected abstract bool HandleUse(TCharacter user, TCharacter target);

        #region ISkill<TSkillType,TStatType,TCharacter> Members

        /// <summary>
        /// Gets the amount of time in milliseconds that must elapse between the time the skill starts to be used and
        /// when the skill is actually used. A value of 0 means the skill will be used immediately.
        /// </summary>
        public ushort CastingTime
        {
            get { return _castingTime; }
        }

        /// <summary>
        /// Gets a byte that represents which group of skills this skill is part of when setting the skill usage
        /// cooldown.
        /// </summary>
        public byte CooldownGroup
        {
            get { return _cooldownGroup; }
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that must elapse before this skill, or any other skill in the same
        /// CooldownGroup, can be used again by the character who used the skill.
        /// </summary>
        public ushort CooldownTime
        {
            get { return _cooldownTime; }
        }

        /// <summary>
        /// Gets an IEnumerable of stats required by this ISkill. Can be null.
        /// </summary>
        public virtual IEnumerable<Stat<TStatType>> RequiredStats
        {
            get { return null; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if this Skill requires a target to be specified for the skill
        /// to be used. If this is true, the skill will never even attempt to be used unless there is a target.
        /// </summary>
        public abstract bool RequiresTarget { get; }

        /// <summary>
        /// Gets the type of skill that this skill object instance is for.
        /// </summary>
        public TSkillType SkillType
        {
            get { return _skillType; }
        }

        /// <summary>
        /// Checks if the given Character can use this Skill.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <returns>True if the <paramref name="user"/> can use this Skill; otherwise false.</returns>
        public bool CanUse(TCharacter user)
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
        public virtual bool CanUse(TCharacter user, TCharacter target)
        {
            // State checks
            if (!CheckValidCanUseCharacters(user, target))
                return false;

            // Check for a target
            if (RequiresTarget && target == null)
                return false;

            // Check for the required stats
            if (!HasRequiredStats(user))
                return false;

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, checks if the given <paramref name="character"/> has the required stats
        /// for using this Skill.
        /// </summary>
        /// <param name="character">The Character using the skill. Will not be null.</param>
        /// <returns>True if the <paramref name="character"/> has the required stats to use this skill; otherwise false.</returns>
        public abstract bool HasRequiredStats(TCharacter character);

        /// <summary>
        /// Uses this Skill without a target.
        /// </summary>
        /// <param name="user">User to make use this Skill.</param>
        /// <returns>True if the Skill was successfully used; otherwise false.</returns>
        public bool Use(TCharacter user)
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
        public bool Use(TCharacter user, TCharacter target)
        {
            // Ensure the skill can be used
            if (!CanUse(user, target))
                return false;

            // Use the skill
            var useSuccessful = HandleUse(user, target);
            if (!useSuccessful)
                return false;

            AfterUseSkill(user, target);

            return true;
        }

        #endregion
    }
}