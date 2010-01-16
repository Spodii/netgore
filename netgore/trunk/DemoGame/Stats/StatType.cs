using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Contains each different type of Stat.
    /// </summary>
    public enum StatType : byte
    {
        // NOTE: For best results, DO NOT manually assign any values in the StatType enum!

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