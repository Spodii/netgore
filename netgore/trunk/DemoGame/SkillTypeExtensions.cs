using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame
{
    public static class SkillTypeExtensions
    {
        public static byte GetValue(this SkillType skillType)
        {
            return (byte)skillType;
        }

        /// <summary>
        /// Checks if a specified SkillType value is defined by the SkillType enum.
        /// </summary>
        /// <param name="skillType">SkillType value to check.</param>
        /// <returns>True if the <paramref name="skillType"/> is defined in the SkillType enum, else false.</returns>
        public static bool IsDefined(this SkillType skillType)
        {
            return Enum.IsDefined(typeof(SkillType), skillType);
        }
    }
}
