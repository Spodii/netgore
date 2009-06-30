using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Unique IDs for the different ClientPackets (packets sent from the client to server).
    /// </summary>
    public enum ClientPacketID : byte
    {
        Attack = 1,
        Say,
        DropInventoryItem,
        GetEquipmentItemInfo,
        GetInventoryItemInfo,
        Jump,
        Login,
        MoveLeft,
        MoveRight,
        MoveStop,
        PickupItem,
        Ping,
        RaiseStat,
        SetUDPPort,
        UnequipItem,
        UseInventoryItem,
        UseWorld
    }
}