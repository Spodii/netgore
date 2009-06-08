using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using NetGore;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Packets going out from the server / in to the client
    /// </summary>
    static class ServerPacket
    {
        static readonly PacketWriterPool _writerPool = new PacketWriterPool();
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static PacketWriter CharAttack(ushort mapCharIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.CharAttack);
            pw.Write(mapCharIndex);
            return pw;
        }

        public static PacketWriter CharDamage(ushort mapCharIndex, int damage)
        {
            PacketWriter pw = GetWriter(ServerPacketID.CharDamage);
            pw.Write(mapCharIndex);
            pw.Write(damage);
            return pw;
        }

        public static PacketWriter Chat(string text)
        {
            PacketWriter pw = GetWriter(ServerPacketID.Chat);
            pw.Write(text, GameData.MaxServerSayLength);
            return pw;
        }

        public static PacketWriter ChatSay(string name, ushort mapCharIndex, string text)
        {
            PacketWriter pw = GetWriter(ServerPacketID.ChatSay);
            pw.Write(name, GameData.MaxServerSayNameLength);
            pw.Write(mapCharIndex);
            pw.Write(text, GameData.MaxServerSayLength);
            return pw;
        }

        public static PacketWriter CreateDynamicEntity(DynamicEntity dynamicEntity)
        {
            PacketWriter pw = GetWriter(ServerPacketID.CreateDynamicEntity);
            pw.Write((ushort)dynamicEntity.MapEntityIndex);
            DynamicEntityFactory.Write(pw, dynamicEntity);
            return pw;
        }

        /// <summary>
        /// Gets a PacketWriter to use.
        /// </summary>
        /// <param name="id">ServerPacketID that this PacketWriter will be writing.</param>
        /// <returns>PacketWriter to use.</returns>
        static PacketWriter GetWriter(ServerPacketID id)
        {
            PacketWriter pw = _writerPool.Create();
            pw.Write((byte)id);
            return pw;
        }

        /// <summary>
        /// Tells the user they entered invalid account information upon logging in
        /// </summary>
        public static PacketWriter InvalidAccount()
        {
            return GetWriter(ServerPacketID.InvalidAccount);
        }

        /// <summary>
        /// Tells the user they have successfully logged in
        /// </summary>
        public static PacketWriter Login()
        {
            return GetWriter(ServerPacketID.Login);
        }

        public static PacketWriter NotifyExpCash(int exp, int cash)
        {
            PacketWriter pw = GetWriter(ServerPacketID.NotifyExpCash);
            pw.Write(exp);
            pw.Write(cash);
            return pw;
        }

        public static PacketWriter NotifyGetItem(string name, byte amount)
        {
            PacketWriter pw = GetWriter(ServerPacketID.NotifyGetItem);
            pw.Write(name);
            pw.Write(amount);
            return pw;
        }

        public static PacketWriter NotifyLevel(ushort mapCharIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.NotifyLevel);
            pw.Write(mapCharIndex);
            return pw;
        }

        public static PacketWriter Ping()
        {
            return GetWriter(ServerPacketID.Ping);
        }

        public static PacketWriter RemoveDynamicEntity(DynamicEntity dynamicEntity)
        {
            PacketWriter pw = GetWriter(ServerPacketID.RemoveDynamicEntity);
            pw.Write((ushort)dynamicEntity.MapEntityIndex);
            return pw;
        }

        public static PacketWriter SendItemInfo(ItemEntity item)
        {
            if (item == null)
            {
                const string errmsg =
                    "item is null, so SendItemInfo will not send anything. Not a breaking error, but likely a design flaw.";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
            }

            PacketWriter pw = GetWriter(ServerPacketID.SendItemInfo);
            pw.Write(item.Name);
            pw.Write(item.Description);
            pw.Write(item.Value);
            pw.Write(item.Stats);
            return pw;
        }

        public static PacketWriter SendMessage(GameMessage message)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SendMessage);
            pw.Write((byte)message);
            pw.Write((byte)0);
            return pw;
        }

        public static PacketWriter SendMessage(GameMessage message, params object[] p)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SendMessage);
            pw.Write((byte)message);

            // Write the parameter count and all of the parameters
            if (p == null || p.Length < 1)
                pw.Write((byte)0);
            else
            {
                pw.Write((byte)p.Length);
                for (int i = 0; i < p.Length; i++)
                {
                    // Get the object
                    object obj = p[i];
                    if (obj == null)
                    {
                        const string errmsg = "Null object passed to SendMessage().";
                        Debug.Fail(errmsg);
                        if (log.IsErrorEnabled)
                            log.Error(errmsg);

                        // Write out an error string instead for the parameter
                        pw.Write("NULL_PARAMETER_ERROR", GameData.MaxServerMessageParameterLength);
                        continue;
                    }

                    // Convert to a string, and ensure the string is short enough (trimming if it is too long)
                    string str = obj.ToString();
                    if (str.Length > GameData.MaxServerMessageParameterLength)
                        str = str.Substring(0, GameData.MaxServerMessageParameterLength);

                    // Write the string
                    pw.Write(str, GameData.MaxServerMessageParameterLength);
                }
            }

            return pw;
        }

        public static PacketWriter SetInventorySlot(byte slot, ushort graphic, byte amount)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetInventorySlot);
            pw.Write(slot);
            pw.Write(graphic);
            pw.Write(amount);
            return pw;
        }

        public static PacketWriter SetMap(MapIndex mapIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetMap);
            pw.Write(mapIndex);
            return pw;
        }

        /// <summary>
        /// Sets which map character is the one controlled by the user
        /// </summary>
        /// <param name="mapCharIndex">Map character index controlled by the user</param>
        public static PacketWriter SetUserChar(ushort mapCharIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetUserChar);
            pw.Write(mapCharIndex);
            return pw;
        }

        public static PacketWriter SynchronizeDynamicEntity(DynamicEntity dynamicEntity)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SynchronizeDynamicEntity);
            pw.Write((ushort)dynamicEntity.MapEntityIndex);
            dynamicEntity.Serialize(pw);
            return pw;
        }

        public static PacketWriter UpdateEquipmentSlot(EquipmentSlot slot, ushort? graphic)
        {
            PacketWriter pw = GetWriter(ServerPacketID.UpdateEquipmentSlot);
            pw.Write(slot);
            pw.Write(graphic.HasValue);

            if (graphic.HasValue)
                pw.Write(graphic.Value);

            return pw;
        }

        public static PacketWriter UpdateStat(IStat stat)
        {
            PacketWriter pw = GetWriter(ServerPacketID.UpdateStat);
            pw.Write(stat.StatType);
            stat.Write(pw);
            return pw;
        }

        public static PacketWriter UpdateVelocityAndPosition(DynamicEntity dynamicEntity, int currentTime)
        {
            PacketWriter pw = GetWriter(ServerPacketID.UpdateVelocityAndPosition);
            pw.Write((ushort)dynamicEntity.MapEntityIndex);
            dynamicEntity.SerializePositionAndVelocity(pw, currentTime);
            return pw;
        }
    }
}