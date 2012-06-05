using System.Linq;

namespace NetGore.Features.Skills
{
    /// <summary>
    /// Interface for an object that keeps track of which skill groups for a single character are currently cooling
    /// down and how long they have until the skill group can be used again.
    /// </summary>
    public interface ISkillCooldownManager
    {
        /// <summary>
        /// Gets the cooldown time remaining for the given group.
        /// </summary>
        /// <param name="group">The index of the skill group.</param>
        /// <returns>The cooldown time remaining for the given group.</returns>
        int GetCooldownTime(byte group);

        /// <summary>
        /// Gets if a skill group is currently cooling down.
        /// </summary>
        /// <param name="group">The index of the skill group.</param>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        /// <returns>True if the group is currently cooling down and cannot be used; false if the group is available
        /// for usage.</returns>
        bool IsCoolingDown(byte group, TickCount currentTime);

        /// <summary>
        /// Sets the cooldown time for a group of skills.
        /// </summary>
        /// <param name="group">The index of the skill group.</param>
        /// <param name="time">The cooldown time in milliseconds.</param>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        void SetCooldown(byte group, int time, TickCount currentTime);
    }
}