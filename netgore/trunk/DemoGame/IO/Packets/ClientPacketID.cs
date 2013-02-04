using System.ComponentModel;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Unique IDs for the different ClientPackets (packets sent from the client to server).
    /// </summary>
    public enum ClientPacketID : ushort
    {
        /// <summary>
        /// This value is reserved and must not be used!
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        RESERVED = 0,

        AcceptOrTurnInQuest,
        Attack,
        BuyFromShop,
        ClickWarp,
        CreateNewAccount,
        CreateNewAccountCharacter,
        DeleteAccountCharacter,
        DropInventoryItem,
        Emoticon,
        EndNPCChatDialog,
        GetEquipmentItemInfo,
        GetFriends,
        GetOnlineUsers,
        GetInventoryItemInfo,
        HasQuestFinishRequirements,
        HasQuestStartRequirements,
        Login,
        MoveLeft,
        MoveRight,
        MoveStop,
        PeerTradeEvent,
        PickupItem,
        RaiseStat,
        RequestDynamicEntity,
        Say,
        SaveFriends,
        SendPrivateMessage,
        SelectAccountCharacter,
        SelectNPCChatDialogResponse,
        SellInventoryToShop,
        StartNPCChatDialog,
        StartShopping,
        SwapInventorySlots,
        SynchronizeGameTime,
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