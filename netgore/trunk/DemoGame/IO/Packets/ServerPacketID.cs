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
        LoginSuccessful,
        LoginUnsuccessful,
        NotifyExpCash,
        NotifyLevel,
        NotifyGetItem,
        Ping,
        PlaySound,
        PlaySoundAt,
        PlaySoundAtEntity,
        RemoveDynamicEntity,
        RemoveStatusEffect,
        StartCastingSkill,
        SendAccountCharacters,
        SendEquipmentItemInfo,
        SendInventoryItemInfo,
        SendMessage,
        SetCharacterHPPercent,
        SetCharacterMPPercent,
        SetHP,
        SetMP,
        SetExp,
        SetCash,
        SetCharacterPaperDoll,
        SetChatDialogPage,
        SetInventorySlot,
        SetLevel,
        SetProvidedQuests,
        SetSkillGroupCooldown,
        SetStatPoints,
        SetMap,
        SetUserChar,
        StartChatDialog,
        StartShopping,
        StopShopping,
        UpdateEquipmentSlot,
        UpdateStat,
        UpdateVelocityAndPosition,
        UseEntity,
        UseSkill
    }
}