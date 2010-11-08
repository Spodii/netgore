using System.Linq;
using NetGore.Features.Skills;

namespace DemoGame.Server
{
    /// <summary>
    /// Manages all of the individual SkillBases for the corresponding SkillType. Skill instances should be acquired
    /// through this manager instead of creating new instances of the class.
    /// </summary>
    public sealed class SkillManager : SkillManager<SkillType, StatType, Character>
    {
        static readonly SkillManager _instance;

        /// <summary>
        /// Initializes the <see cref="SkillManager"/> class.
        /// </summary>
        static SkillManager()
        {
            _instance = new SkillManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillManager"/> class.
        /// </summary>
        SkillManager()
        {
        }

        /// <summary>
        /// Gets the <see cref="SkillManager"/> instance.
        /// </summary>
        public static SkillManager Instance
        {
            get { return _instance; }
        }
    }
}