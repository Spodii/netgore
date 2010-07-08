using System.Linq;

namespace DemoGame
{
    // TODO: Define the skill information in files, not via SkillInfoAttribute

    /// <summary>
    /// Contains the unique name of each individual Skill.
    /// </summary>
    public enum SkillType : byte
    {
        [SkillInfo("Heal", "Me heal you long time!", 140, 0)]
        Heal,

        [SkillInfo("Strengthen", "Makes you stronger! Grr!!!", 142, 1)]
        Strengthen
    }
}