using System.Linq;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    public sealed class SkillTypeHelper : EnumIOHelper<SkillType>
    {
        static readonly SkillTypeHelper _instance;

        /// <summary>
        /// Initializes the <see cref="SkillTypeHelper"/> class.
        /// </summary>
        static SkillTypeHelper()
        {
            _instance = new SkillTypeHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillTypeHelper"/> class.
        /// </summary>
        SkillTypeHelper()
        {
        }

        /// <summary>
        /// Gets the <see cref="SkillTypeHelper"/> instance.
        /// </summary>
        public static SkillTypeHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="SkillType"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="SkillType"/>.</returns>
        protected override SkillType FromInt(int value)
        {
            return (SkillType)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="SkillType"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected override int ToInt(SkillType value)
        {
            return (int)value;
        }
    }
}