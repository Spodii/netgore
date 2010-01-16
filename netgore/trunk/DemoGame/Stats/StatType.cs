using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Contains each different type of Stat.
    /// </summary>
    public enum StatType : byte
    {
        // TODO: Should make an error message appear if the StatType enum skips any numeric values, since quite a few things rely on it going from 0 to <length - 1>

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