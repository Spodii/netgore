using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// A basic skill cooldown manager.
    /// </summary>
    public class SkillCooldownManager : ISkillCooldownManager
    {
        readonly Dictionary<byte, int> _cooldownGroups = new Dictionary<byte, int>();

        /// <summary>
        /// Sets the cooldown time for a group of skills.
        /// </summary>
        /// <param name="group">The index of the skill group.</param>
        /// <param name="time">The cooldown time in milliseconds.</param>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public void SetCooldown(byte group, int time, int currentTime)
        {
            if (time <= 0)
            {
                _cooldownGroups.Remove(group);
                return;
            }

            int endTime = time + currentTime;

            if (_cooldownGroups.ContainsKey(group))
                _cooldownGroups[group] = endTime;
            else
                _cooldownGroups.Add(group, endTime);
        }

        /// <summary>
        /// Gets if a skill group is currently cooling down.
        /// </summary>
        /// <param name="group">The index of the skill group.</param>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        /// <returns>True if the group is currently cooling down and cannot be used; false if the group is available
        /// for usage.</returns>
        public bool IsCoolingDown(byte group, int currentTime)
        {
            if (!_cooldownGroups.ContainsKey(group))
                return false;

            int groupTime = _cooldownGroups[group];
            if (groupTime > currentTime)
            {
                return true;
            }
            else
            {
                _cooldownGroups.Remove(group);
                return false;
            }
        }

        /// <summary>
        /// Gets the cooldown time remaining for the given group.
        /// </summary>
        /// <param name="group">The index of the skill group.</param>
        /// <returns>The cooldown time remaining for the given group.</returns>
        public int GetCooldownTime(byte group)
        {
            if (_cooldownGroups.ContainsKey(group))
                return _cooldownGroups[group];

            return 0;
        }
    }
}