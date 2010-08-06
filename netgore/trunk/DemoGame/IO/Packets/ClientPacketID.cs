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

        AcceptOrTurnInQuest,
        Attack,
        BuyFromShop,
        CreateNewAccount,
        CreateNewAccountCharacter,
        DropInventoryItem,
        Emoticon,
        EndNPCChatDialog,
        GetEquipmentItemInfo,
        GetInventoryItemInfo,
        HasQuestFinishRequirements,
        HasQuestStartRequirements,
        Login,
        MoveLeft,
        MoveRight,
        MoveStop,
        PeerTradeEvent,
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
        SwapInventorySlots,
        UnequipItem,
        UseInventoryItem,
        UseSkill,
        UseWorld,

#if TOPDOWN
    // Top-down only
        MoveStopHorizontal,
        MoveStopVertical,
        MoveUp,
        MoveDown,
#else
        // Side-scroller only
        Jump,
#endif
    }
}