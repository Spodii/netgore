using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Unique IDs for the different ServerPacket (packets sent from the server to client).
    /// </summary>
    public enum ServerPacketID : byte
    {
        // First index (0) is reserved, and must not be used

        AddStatusEffect = 1,
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
        SendItemInfo,
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
        UpdateEquipmentSlot,
        UpdateStat,
        UpdateVelocityAndPosition,
        UseEntity,
        UseSkill
    }
}