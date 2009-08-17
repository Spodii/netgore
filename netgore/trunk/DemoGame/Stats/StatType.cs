using System.Linq;

// TODO: Should make an error message appear if the StatType enum skips any numeric values, since quite a few things rely on it going from 0 to <length - 1>

namespace DemoGame
{
    /// <summary>
    /// Contains each different type of Stat.
    /// </summary>
    public enum StatType : byte
    {
        MaxHP,
        MaxMP,

        Defence,
        MinHit,
        MaxHit,

        Acc,
        Agi,
        Armor,
        Bra,
        Dex,
        Evade,
        Imm,
        Int,
        Perc,
        Recov,
        Regen,
        Str,
        Tact,
        WS
    }
}