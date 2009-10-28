using System.ComponentModel;
using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Unique IDs for the different ClientPackets (packets sent from the client to server).
    /// </summary>
    public enum ClientPacketID : byte
    {
        /// <summary>
        /// This value is reserved and must not be used!
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        RESERVED = 0,

        Attack,
        BuyFromShop,
        DropInventoryItem,
        EndNPCChatDialog,
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
        Say,
        SelectAccountCharacter,
        SelectNPCChatDialogResponse,
        SellInventoryToShop,
        SetUDPPort,
        StartNPCChatDialog,
        StartShopping,
        UnequipItem,
        UseInventoryItem,
        UseSkill,
        UseWorld
    }
}