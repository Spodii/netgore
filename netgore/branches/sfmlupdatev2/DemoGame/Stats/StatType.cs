using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Contains each different type of Stat.
    /// </summary>
    public enum StatType : byte
    {
        /* NOTE: For best results, DO NOT manually assign any values in the StatType enum!
         * In other words, do NOT do the following:
         *      Str = 0
         *      Agi = 1
         *      etc...
         * Let the compiler decide what value to assign to the stats.
         */

        MaxHP,
        MaxMP,

        Defence,
        MinHit,
        MaxHit,

        Agi,
        Int,
        Str
    }
}