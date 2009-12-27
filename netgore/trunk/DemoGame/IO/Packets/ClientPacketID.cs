using System.ComponentModel;
using System.Linq;

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
#if !TOPDOWN
        Jump,
#endif
        Login,
#if TOPDOWN
        MoveDown,
#endif
        MoveLeft,
        MoveRight,
        MoveStop,
#if TOPDOWN
        MoveStopHorizontal,
#endif
#if TOPDOWN
        MoveStopVertical,
#endif
#if TOPDOWN
        MoveUp,
#endif
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