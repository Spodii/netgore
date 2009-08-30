using System.Linq;
using NetGore;

namespace DemoGame
{
    public static class StatusEffectTypeExtensions
    {
        public static byte GetValue(this StatusEffectType statusEffectType)
        {
            return (byte)statusEffectType;
        }

        /// <summary>
        /// Checks if a specified StatusEffectType value is defined by the StatusEffectType enum.
        /// </summary>
        /// <param name="statusEffectType">StatusEffectType value to check.</param>
        /// <returns>True if the <paramref name="statusEffectType"/> is defined in the StatusEffectType enum, else false.</returns>
        public static bool IsDefined(this StatusEffectType statusEffectType)
        {
            return EnumHelper.IsDefined(statusEffectType);
        }
    }
}