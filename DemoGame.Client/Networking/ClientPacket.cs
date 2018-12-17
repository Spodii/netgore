using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Network;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Packets going out from the client / in to the server
    /// </summary>
    public static partial class ClientPacket
    {
        static readonly PacketWriterPool _writerPool = new PacketWriterPool();

        public static PacketWriter AcceptOrTurnInQuest(MapEntityIndex questProvider, QuestID questID)
        {
            var pw = GetWriter(ClientPacketID.AcceptOrTurnInQuest);
            pw.Write(questProvider);
            pw.Write(questID);
            return pw;
        }

        public static PacketWriter Attack(MapEntityIndex? target)
        {
            var pw = GetWriter(ClientPacketID.Attack);
            pw.Write(target.HasValue);
            if (target.HasValue)
                pw.Write(target.Value);
            return pw;
        }

        public static PacketWriter BuyFromShop(ShopItemIndex index, byte amount)
        {
            var pw = GetWriter(ClientPacketID.BuyFromShop);
            pw.Write(index);
            pw.Write(amount);
            return pw;
        }

        public static PacketWriter ClickWarp(Vector2 worldPos)
        {
            var pw = GetWriter(ClientPacketID.ClickWarp);
            pw.Write(worldPos);;
            return pw;
        }

        public static PacketWriter CreateNewAccount(string name, string password, string email)
        {
            var pw = GetWriter(ClientPacketID.CreateNewAccount);
            pw.Write(name);
            pw.Write(password);
            pw.Write(email);
            return pw;
        }

        public static PacketWriter CreateNewAccountCharacter(string name)
        {
            var pw = GetWriter(ClientPacketID.CreateNewAccountCharacter);
            pw.Write(name);
            return pw;
        }

        public static PacketWriter DeleteAccountCharacter(byte charSlot)
        {
            var pw = GetWriter(ClientPacketID.DeleteAccountCharacter);
            pw.Write(charSlot);
            return pw;
        }

        public static PacketWriter DropInventoryItem(InventorySlot slot, byte amount)
        {
            var pw = GetWriter(ClientPacketID.DropInventoryItem);
            pw.Write(slot);
            pw.Write(amount);
            return pw;
        }

        public static PacketWriter Emoticon(Emoticon emoticon)
        {
            var pw = GetWriter(ClientPacketID.Emoticon);
            pw.WriteEnum(emoticon);
            return pw;
        }

        public static PacketWriter EndNPCChatDialog()
        {
            return GetWriter(ClientPacketID.EndNPCChatDialog);
        }

        public static PacketWriter GetEquipmentItemInfo(EquipmentSlot slot)
        {
            var pw = GetWriter(ClientPacketID.GetEquipmentItemInfo);
            pw.WriteEnum(slot);
            return pw;
        }

        public static PacketWriter GetInventoryItemInfo(InventorySlot slot)
        {
            var pw = GetWriter(ClientPacketID.GetInventoryItemInfo);
            pw.Write(slot);
            return pw;
        }

        /// <summary>
        /// Gets a <see cref="PacketWriter"/> to use from the internal pool. It is important that this
        /// <see cref="PacketWriter"/> is disposed of properly when done.
        /// </summary>
        /// <returns>The <see cref="PacketWriter"/> to use.</returns>
        public static PacketWriter GetWriter()
        {
            return _writerPool.Acquire();
        }

        /// <summary>
        /// Gets a <see cref="PacketWriter"/> to use from the internal pool. It is important that this
        /// <see cref="PacketWriter"/> is disposed of properly when done.
        /// </summary>
        /// <param name="id">The <see cref="ClientPacketID"/> that this <see cref="PacketWriter"/> will be writing.</param>
        /// <returns>The <see cref="PacketWriter"/> to use.</returns>
        public static PacketWriter GetWriter(ClientPacketID id)
        {
            var pw = _writerPool.Acquire();
            Debug.Assert(pw.LengthBits == 0);
            pw.Write(id);
            return pw;
        }

        public static PacketWriter HasQuestFinishRequirements(QuestID questID)
        {
            var pw = GetWriter(ClientPacketID.HasQuestFinishRequirements);
            pw.Write(questID);
            return pw;
        }

        public static PacketWriter HasQuestStartRequirements(QuestID questID)
        {
            var pw = GetWriter(ClientPacketID.HasQuestStartRequirements);
            pw.Write(questID);
            return pw;
        }

        public static PacketWriter Login(string name, string password)
        {
            var pw = GetWriter(ClientPacketID.Login);
            pw.Write(name);
            pw.Write(password);
            return pw;
        }

        public static PacketWriter MoveLeft()
        {
            return GetWriter(ClientPacketID.MoveLeft);
        }

        public static PacketWriter MoveRight()
        {
            return GetWriter(ClientPacketID.MoveRight);
        }

        public static PacketWriter MoveStop()
        {
            return GetWriter(ClientPacketID.MoveStop);
        }

        public static PacketWriter PickupItem(MapEntityIndex mapEntityIndex)
        {
            var pw = GetWriter(ClientPacketID.PickupItem);
            pw.Write(mapEntityIndex);
            return pw;
        }

        public static PacketWriter RaiseStat(StatType statType)
        {
            var pw = GetWriter(ClientPacketID.RaiseStat);
            pw.WriteEnum(statType);
            return pw;
        }

        public static PacketWriter RequestDynamicEntity(MapEntityIndex index)
        {
            var pw = GetWriter(ClientPacketID.RequestDynamicEntity);
            pw.Write(index);
            return pw;
        }

        public static PacketWriter Say(string text)
        {
            var pw = GetWriter(ClientPacketID.Say);
            pw.Write(text, GameData.MaxClientSayLength);
            return pw;
        }

        public static PacketWriter SelectAccountCharacter(byte index)
        {
            var pw = GetWriter(ClientPacketID.SelectAccountCharacter);
            pw.Write(index);
            return pw;
        }

        public static PacketWriter SelectNPCChatDialogResponse(byte responseIndex)
        {
            var pw = GetWriter(ClientPacketID.SelectNPCChatDialogResponse);
            pw.Write(responseIndex);
            return pw;
        }

        public static PacketWriter SellInventoryToShop(InventorySlot slot, byte amount)
        {
            var pw = GetWriter(ClientPacketID.SellInventoryToShop);
            pw.Write(slot);
            pw.Write(amount);
            return pw;
        }

        public static PacketWriter StartNPCChatDialog(MapEntityIndex npcIndex, bool forceSkipQuestDialog)
        {
            var pw = GetWriter(ClientPacketID.StartNPCChatDialog);
            pw.Write(npcIndex);
            pw.Write(forceSkipQuestDialog);
            return pw;
        }

        public static PacketWriter StartShopping(MapEntityIndex entityIndex)
        {
            var pw = GetWriter(ClientPacketID.StartShopping);
            pw.Write(entityIndex);
            return pw;
        }

        public static PacketWriter SwapInventorySlots(InventorySlot a, InventorySlot b)
        {
            var pw = GetWriter(ClientPacketID.SwapInventorySlots);
            pw.Write(a);
            pw.Write(b);
            return pw;
        }

        public static PacketWriter SynchronizeGameTime()
        {
            var pw = GetWriter(ClientPacketID.SynchronizeGameTime);
            return pw;
        }

        public static PacketWriter UnequipItem(EquipmentSlot slot)
        {
            var pw = GetWriter(ClientPacketID.UnequipItem);
            pw.WriteEnum(slot);
            return pw;
        }

        public static PacketWriter UseInventoryItem(InventorySlot slot)
        {
            var pw = GetWriter(ClientPacketID.UseInventoryItem);
            pw.Write(slot);
            return pw;
        }

        public static PacketWriter UseSkill(SkillType skillType, MapEntityIndex? target)
        {
            var pw = GetWriter(ClientPacketID.UseSkill);
            pw.WriteEnum(skillType);
            pw.Write(target.HasValue);
            if (target.HasValue)
                pw.Write(target.Value);
            return pw;
        }

        public static PacketWriter UseWorld(MapEntityIndex useItemIndex)
        {
            var pw = GetWriter(ClientPacketID.UseWorld);
            pw.Write(useItemIndex);
            return pw;
        }

        public static PacketWriter GetFriends()
        {
            var pw = GetWriter(ClientPacketID.GetFriends);
            return pw;
        }

        public static PacketWriter SaveFriends(string friendsList)
        {
            var pw = GetWriter(ClientPacketID.SaveFriends);
            pw.Write(friendsList);
            return pw;
        }

        public static PacketWriter SendPrivateMessage(string targetName, string message)
        {
            var pw = GetWriter(ClientPacketID.SendPrivateMessage);
            pw.Write(targetName);
            pw.Write(message);
            return pw;
        }

        public static PacketWriter GetOnlineUsers()
        {
            var pw = GetWriter(ClientPacketID.GetOnlineUsers);
            return pw;
        }
    }
}