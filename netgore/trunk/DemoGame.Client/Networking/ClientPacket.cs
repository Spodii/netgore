using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Packets going out from the client / in to the server
    /// </summary>
    public static class ClientPacket
    {
        static readonly PacketWriterPool _writerPool = new PacketWriterPool();

        public static PacketWriter Attack()
        {
            return GetWriter(ClientPacketID.Attack);
        }

        public static PacketWriter DropInventoryItem(byte slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.DropInventoryItem);
            pw.Write(slot);
            return pw;
        }

        public static PacketWriter GetEquipmentItemInfo(EquipmentSlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.GetEquipmentItemInfo);
            pw.Write(slot);
            return pw;
        }

        public static PacketWriter GetInventoryItemInfo(byte slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.GetInventoryItemInfo);
            pw.Write(slot);
            return pw;
        }

        static PacketWriter GetWriter(ClientPacketID id)
        {
            PacketWriter pw = _writerPool.Create();
            pw.Write(id);
            return pw;
        }

        public static PacketWriter Jump()
        {
            return GetWriter(ClientPacketID.Jump);
        }

        public static PacketWriter Login(string name, string password)
        {
            PacketWriter pw = GetWriter(ClientPacketID.Login);
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
            pw.Write(statType);
            return pw;
        }

        public static PacketWriter Say(string text)
        {
            PacketWriter pw = GetWriter(ClientPacketID.Say);
            pw.Write(text, GameData.MaxClientSayLength);
            return pw;
        }

        public static PacketWriter SetUDPPort(int port)
        {
            PacketWriter pw = GetWriter(ClientPacketID.SetUDPPort);
            pw.Write((ushort)port);
            return pw;
        }

        public static PacketWriter UnequipItem(EquipmentSlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.UnequipItem);
            pw.Write(slot);
            return pw;
        }

        public static PacketWriter UseInventoryItem(byte slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.UseInventoryItem);
            pw.Write(slot);
            return pw;
        }

        public static PacketWriter UseWorld()
        {
            return GetWriter(ClientPacketID.UseWorld);
        }
    }
}