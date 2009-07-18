namespace DemoGame
{
    /// <summary>
    /// Unique IDs for the different ServerPacket (packets sent from the server to client).
    /// </summary>
    public enum ServerPacketID : byte
    {
        CharAttack = 1,
        CharDamage,
        Chat,
        ChatSay,
        CreateDynamicEntity,
        LoginSuccessful,
        LoginUnsuccessful,
        NotifyExpCash,
        NotifyLevel,
        NotifyGetItem,
        Ping,
        RemoveDynamicEntity,
        SendItemInfo,
        SendMessage,
        SetCharacterHPPercent,
        SetCharacterMPPercent,
        SetHP,
        SetMP,
        SetInventorySlot,
        SetMap,
        SetUserChar,
        UpdateEquipmentSlot,
        UpdateStat,
        UpdateVelocityAndPosition,
        UseEntity
    }
}