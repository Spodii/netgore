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
        CreateMapItem,
        InvalidAccount,
        Login,
        NotifyExpCash,
        NotifyLevel,
        NotifyGetItem,
        NotifyFullInv,
        Ping,
        RemoveChar,
        RemoveMapItem,
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
        TeleportChar,
        UpdateEquipmentSlot,
        UpdateStat
    }
}