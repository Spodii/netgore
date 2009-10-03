using System.ComponentModel;
using System.Linq;
using DemoGame;
using NetGore;

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
        CreateDynamicEntity,
        EndChatDialog,
        LoginSuccessful,
        LoginUnsuccessful,
        NotifyExpCash,
        NotifyLevel,
        NotifyGetItem,
        Ping,
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
        SetExp,
        SetCash,
        SetChatDialogPage,
        SetLevel,
        SetInventorySlot,
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