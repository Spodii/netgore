using System.Linq;

namespace DemoGame
{
    public static class SkillTypeExtensions
    {
        public static byte GetValue(this SkillType skillType)
        {
            return (byte)skillType;
        }
    }
}