using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Unique IDs for the different ServerPacket (packets sent from the server to client).
    /// </summary>
    public enum ServerPacketID : byte
    {
        CharAttack = 1,
        CharDamage,
        Chat,
        ChatSay,
        CreateDynamicEntity,
        InvalidAccount,
        Login,
        NotifyExpCash,
        NotifyLevel,
        NotifyGetItem,
        Ping,
        RemoveDynamicEntity,
        SendItemInfo,
        SendMessage,
        SetInventorySlot,
        SetMap,
        SetUserChar,
        SynchronizeDynamicEntity,
        UpdateEquipmentSlot,
        UpdateStat
    }
}