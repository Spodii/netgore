using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Platyform.Extensions;

namespace DemoGame
{
    /// <summary>
    /// Enum of all of the different types of Stats in the game. Not every stat is or needs to be
    /// used by every entity that uses stats, but every stat does have a unique value.
    /// </summary>
    public enum StatType : byte
    {
        Cash,
        Exp,
        ExpSpent,
        Level,

        HP,
        MP,
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
        WS,

        ModAcc,
        ModAgi,
        ModArmor,
        ModBra,
        ModDex,
        ModEvade,
        ModImm,
        ModInt,
        ModPerc,
        ModRecov,
        ModRegen,
        ModStr,
        ModTact,
        ModWS,

        ReqAcc,
        ReqAgi,
        ReqArmor,
        ReqBra,
        ReqDex,
        ReqEvade,
        ReqImm,
        ReqInt
    }
}