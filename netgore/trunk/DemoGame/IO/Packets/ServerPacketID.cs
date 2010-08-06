using System.ComponentModel;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Unique IDs for the different ServerPacket (packets sent from the server to client).
    /// </summary>
    public enum ServerPacketID : byte
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
        Ping,
        PlaySound,
        PlaySoundAt,
        PlaySoundAtEntity,
        QuestInfo,
        RemoveDynamicEntity,
        RemoveStatusEffect,
        RequestUDPConnection,
        StartCastingSkill,
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
        SetGameTime,
        SetExp,
        SetInventorySlot,
        SetLevel,
        SetProvidedQuests,
        SetSkillGroupCooldown,
        SetStatPoints,
        SetMap,
        SetUserChar,
        StartChatDialog,
        StartQuestChatDialog,
        StartShopping,
        StopShopping,
        UpdateEquipmentSlot,
        UpdateStat,
        UpdateVelocityAndPosition,
        UseEntity,
        UseSkill
    }
}