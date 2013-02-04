using System.ComponentModel;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Unique IDs for the different ServerPacket (packets sent from the server to client).
    /// </summary>
    public enum ServerPacketID : ushort
    {
        /// <summary>
        /// This value is reserved and must not be used!
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        RESERVED = 0,

        AcceptOrTurnInQuestReply,
        AddStatusEffect,
        CharAttack,
        CharDamage,
        Chat,
        ChatSay,
        CreateAccount,
        CreateAccountCharacter,
        CreateActionDisplayAtEntity,
        CreateDynamicEntity,
        Emote,
        EndChatDialog,
        GroupInfo,
        GuildInfo,
        HasQuestFinishRequirementsReply,
        HasQuestStartRequirementsReply,
        LoginSuccessful,
        LoginUnsuccessful,
        NotifyExpCash,
        NotifyLevel,
        NotifyGetItem,
        PeerTradeEvent,
        PlaySound,
        PlaySoundAt,
        PlaySoundAtEntity,
        QuestInfo,
        ReceiveFriends,
        ReceivePrivateMessage,
        ReceiveOnlineUsers,
        RemoveDynamicEntity,
        RemoveStatusEffect,
        SendAccountCharacters,
        SendEquipmentItemInfo,
        SendInventoryItemInfo,
        SendMessage,
        SetCharacterHPPercent,
        SetCharacterMPPercent,
        SetHP,
        SetMP,
        SetCash,
        SetCharacterPaperDoll,
        SetChatDialogPage,
        SetClickWarpMode,
        SetGameTime,
        SetExp,
        SetInventorySlot,
        SetLevel,
        SetProvidedQuests,
        SetStatPoints,
        SetMap,
        SetUserChar,
        SkillSetGroupCooldown,
        SkillSetKnown,
        SkillSetKnownAll,
        SkillStartCasting_ToUser,
        SkillStartCasting_ToMap,
        SkillStopCasting_ToUser,
        SkillStopCasting_ToMap,
        SkillUse,
        StartChatDialog,
        StartQuestChatDialog,
        StartShopping,
        StopShopping,
        SynchronizeDynamicEntity,
        UpdateEquipmentSlot,
        UpdateStat,
        UpdateVelocityAndPosition,
        UseEntity,
    }
}