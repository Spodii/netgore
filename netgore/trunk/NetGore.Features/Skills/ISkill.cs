using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.Stats;

namespace NetGore.Features.Skills
{
    /// <summary>
    /// Interface for an object that describes a skill used by Characters.
    /// </summary>
    /// <typeparam name="TSkillType">The type of skill type enum.</typeparam>
    /// <typeparam name="TStatType">The type of stat type enum.</typeparam>
    /// <typeparam name="TCharacter">The type of character.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public interface ISkill<out TSkillType, TStatType, in TCharacter>
        where TSkillType : struct, IComparable, IConvertible, IFormattable
        where TStatType : struct, IComparable, IConvertible, IFormattable where TCharacter : class
    {
        /// <summary>
        /// Gets the amount of time in milliseconds that must elapse between the time the skill starts to be used and
        /// when the skill is actually used. A value of 0 means the skill will be used immediately.
        /// </summary>
        ushort CastingTime { get; }

        /// <summary>
        /// Gets a byte that represents which group of skills this skill is part of when setting the skill usage
        /// cooldown.
        /// </summary>
        byte CooldownGroup { get; }

        /// <summary>
        /// Gets the amount of time in milliseconds that must elapse before this skill, or any other skill in the same
        /// CooldownGroup, can be used again by the character who used the skill.
        /// </summary>
        ushort CooldownTime { get; }

        /// <summary>
        /// Gets an IEnumerable of stats required by this ISkill. Can be null.
        /// </summary>
        IEnumerable<Stat<TStatType>> RequiredStats { get; }

        /// <summary>
        /// Gets if this skill requires a target to be specified for the skill to be used. If this is true,
        /// the skill will never even attempt to be used unless there is a target.
        /// </summary>
        bool RequiresTarget { get; }

        /// <summary>
        /// Gets the type of skill that this class is for.
        /// </summary>
        TSkillType SkillType { get; }

        /// <summary>
        /// Checks if the given Character can use this Skill.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <returns>True if the <paramref name="user"/> can use this Skill; otherwise false.</returns>
        bool CanUse(TCharacter user);

        /// <summary>
        /// Checks if the given Character can use this Skill.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the <paramref name="user"/> can use this Skill; otherwise false.</returns>
        bool CanUse(TCharacter user, TCharacter target);

        /// <summary>
        /// Checks if the given <paramref name="character"/> has the required stats for using this Skill.
        /// </summary>
        /// <param name="character">The Character using the skill. Will not be null.</param>
        /// <returns>True if the <paramref name="character"/> has the required stats to use this skill; otherwise false.</returns>
        bool HasRequiredStats(TCharacter character);

        /// <summary>
        /// Uses this Skill without a target.
        /// </summary>
        /// <param name="user">User to make use this Skill.</param>
        /// <returns>True if the Skill was successfully used; otherwise false.</returns>
        bool Use(TCharacter user);

        /// <summary>
        /// Uses this Skill.
        /// </summary>
        /// <param name="user">User to make use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the Skill was successfully used; otherwise false.</returns>
        bool Use(TCharacter user, TCharacter target);
    }
}