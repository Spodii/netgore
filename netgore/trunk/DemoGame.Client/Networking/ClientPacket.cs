using System.Linq;
using NetGore;
using NetGore.Features.Emoticons;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Packets going out from the client / in to the server
    /// </summary>
    public static class ClientPacket
    {
        static readonly PacketWriterPool _writerPool = new PacketWriterPool();

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
            PacketWriter pw = GetWriter(ClientPacketID.BuyFromShop);
            pw.Write(index);
            pw.Write(amount);
            return pw;
        }

        public static PacketWriter DropInventoryItem(InventorySlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.DropInventoryItem);
            pw.Write(slot);
            return pw;
        }

        public static PacketWriter Emoticon(Emoticon emoticon)
        {
            PacketWriter pw = GetWriter(ClientPacketID.Emoticon);
            pw.WriteEnum(emoticon);
            return pw;
        }

        public static PacketWriter EndNPCChatDialog()
        {
            return GetWriter(ClientPacketID.EndNPCChatDialog);
        }

        public static PacketWriter GetEquipmentItemInfo(EquipmentSlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.GetEquipmentItemInfo);
            pw.WriteEnum(slot);
            return pw;
        }

        public static PacketWriter GetInventoryItemInfo(InventorySlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.GetInventoryItemInfo);
            pw.Write(slot);
            return pw;
        }

        public static PacketWriter HasQuestStartRequirements(QuestID questID)
        {
            PacketWriter pw = GetWriter(ClientPacketID.HasQuestStartRequirements);
            pw.Write(questID);
            return pw;
        }

        static PacketWriter GetWriter(ClientPacketID id)
        {
            PacketWriter pw = _writerPool.Acquire();
            pw.Write(id);
            return pw;
        }

#if !TOPDOWN
        public static PacketWriter Jump()
        {
            return GetWriter(ClientPacketID.Jump);
        }
#endif

        public static PacketWriter Login(string name, string password)
        {
            PacketWriter pw = GetWriter(ClientPacketID.Login);
            pw.Write(name);
            pw.Write(password);
            return pw;
        }

        public static PacketWriter CreateNewAccountCharacter(string name)
        {
            PacketWriter pw = GetWriter(ClientPacketID.CreateNewAccountCharacter);
            pw.Write(name);
            return pw;
        }

        public static PacketWriter CreateNewAccount(string name, string password, string email)
        {
            PacketWriter pw = GetWriter(ClientPacketID.CreateNewAccount);
            pw.Write(name);
            pw.Write(password);
            pw.Write(email);
            return pw;
        }

#if TOPDOWN
        public static PacketWriter MoveDown()
        {
            return GetWriter(ClientPacketID.MoveDown);
        }
#endif

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

#if TOPDOWN
        public static PacketWriter MoveStopHorizontal()
        {
            return GetWriter(ClientPacketID.MoveStopHorizontal);
        }
#endif

#if TOPDOWN
        public static PacketWriter MoveStopVertical()
        {
            return GetWriter(ClientPacketID.MoveStopVertical);
        }
#endif

#if TOPDOWN
        public static PacketWriter MoveUp()
        {
            return GetWriter(ClientPacketID.MoveUp);
        }
#endif

        public static PacketWriter PickupItem(MapEntityIndex mapEntityIndex)
        {
            PacketWriter pw = GetWriter(ClientPacketID.PickupItem);
            pw.Write(mapEntityIndex);
            return pw;
        }

        public static PacketWriter Ping()
        {
            return GetWriter(ClientPacketID.Ping);
        }

        public static PacketWriter RaiseStat(StatType statType)
        {
            PacketWriter pw = GetWriter(ClientPacketID.RaiseStat);
            pw.WriteEnum(statType);
            return pw;
        }

        public static PacketWriter Say(string text)
        {
            PacketWriter pw = GetWriter(ClientPacketID.Say);
            pw.Write(text, GameData.MaxClientSayLength);
            return pw;
        }

        public static PacketWriter SelectAccountCharacter(byte index)
        {
            PacketWriter pw = GetWriter(ClientPacketID.SelectAccountCharacter);
            pw.Write(index);
            return pw;
        }

        public static PacketWriter SelectNPCChatDialogResponse(byte responseIndex)
        {
            PacketWriter pw = GetWriter(ClientPacketID.SelectNPCChatDialogResponse);
            pw.Write(responseIndex);
            return pw;
        }

        public static PacketWriter SellInventoryToShop(InventorySlot slot, byte amount)
        {
            PacketWriter pw = GetWriter(ClientPacketID.SellInventoryToShop);
            pw.Write(slot);
            pw.Write(amount);
            return pw;
        }

        public static PacketWriter SetUDPPort(int port)
        {
            PacketWriter pw = GetWriter(ClientPacketID.SetUDPPort);
            pw.Write((ushort)port);
            return pw;
        }

        public static PacketWriter StartNPCChatDialog(MapEntityIndex npcIndex)
        {
            PacketWriter pw = GetWriter(ClientPacketID.StartNPCChatDialog);
            pw.Write(npcIndex);
            return pw;
        }

        public static PacketWriter StartShopping(MapEntityIndex entityIndex)
        {
            PacketWriter pw = GetWriter(ClientPacketID.StartShopping);
            pw.Write(entityIndex);
            return pw;
        }

        public static PacketWriter UnequipItem(EquipmentSlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.UnequipItem);
            pw.WriteEnum(slot);
            return pw;
        }

        public static PacketWriter UseInventoryItem(InventorySlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.UseInventoryItem);
            pw.Write(slot);
            return pw;
        }

        public static PacketWriter UseSkill(SkillType skillType, MapEntityIndex? target)
        {
            PacketWriter pw = GetWriter(ClientPacketID.UseSkill);
            pw.WriteEnum(skillType);
            pw.Write(target.HasValue);
            if (target.HasValue)
                pw.Write(target.Value);
            return pw;
        }

        public static PacketWriter UseWorld(MapEntityIndex useItemIndex)
        {
            PacketWriter pw = GetWriter(ClientPacketID.UseWorld);
            pw.Write(useItemIndex);
            return pw;
        }
    }
}