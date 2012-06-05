using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="SkillType"/> enum.
    /// </summary>
    public static class SkillTypeExtensions
    {
        /// <summary>
        /// Gets the numeric value associated with a <see cref="SkillType"/>.
        /// </summary>
        /// <param name="skillType">The <see cref="SkillType"/>.</param>
        /// <returns>The numeric value of the <paramref name="skillType"/>.</returns>
        public static byte GetValue(this SkillType skillType)
        {
            return (byte)skillType;
        }
    }
}