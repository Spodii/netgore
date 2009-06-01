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
        CreateNPC,
        CreateUser,
        InvalidAccount,
        Login,
        NotifyExpCash,
        NotifyLevel,
        NotifyGetItem,
        Ping,
        RemoveChar,
        RemoveDynamicEntity,
        SendItemInfo,
        SendMessage,
        SetCharHeadingRight,
        SetCharHeadingLeft,
        SetCharName,
        SetCharVelocity,
        SetCharVelocityX,
        SetCharVelocityY,
        SetCharVelocityZero,
        SetInventorySlot,
        SetMap,
        SetUserChar,
        SynchronizeDynamicEntity,
        TeleportChar,
        UpdateEquipmentSlot,
        UpdateStat
    }
}