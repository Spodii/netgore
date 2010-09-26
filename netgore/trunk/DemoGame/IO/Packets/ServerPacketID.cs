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
        SetGameTime,
        SetExp,
        SetInventorySlot,
        SetLevel,
        SetProvidedQuests,
        SetStatPoints,
        SetMap,
        SetUserChar,
        SkillSetGroupCooldown,
        SkillStartCasting,
        SkillUse,
        StartChatDialog,
        StartQuestChatDialog,
        StartShopping,
        StopShopping,
        UpdateEquipmentSlot,
        UpdateStat,
        UpdateVelocityAndPosition,
        UseEntity,
    }
}