using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Packets going out from the server / in to the client.
    /// </summary>
    static class ServerPacket
    {
        static readonly PacketWriterPool _writerPool = new PacketWriterPool();
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static PacketWriter CharAttack(MapEntityIndex mapEntityIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.CharAttack);
            pw.Write(mapEntityIndex);
            return pw;
        }

        public static PacketWriter CharDamage(MapEntityIndex mapEntityIndex, int damage)
        {
            PacketWriter pw = GetWriter(ServerPacketID.CharDamage);
            pw.Write(mapEntityIndex);
            pw.Write(damage);
            return pw;
        }

        public static PacketWriter Chat(string text)
        {
            PacketWriter pw = GetWriter(ServerPacketID.Chat);
            pw.Write(text, GameData.MaxServerSayLength);
            return pw;
        }

        public static PacketWriter ChatSay(string name, MapEntityIndex mapEntityIndex, string text)
        {
            PacketWriter pw = GetWriter(ServerPacketID.ChatSay);
            pw.Write(name, GameData.MaxServerSayNameLength);
            pw.Write(mapEntityIndex);
            pw.Write(text, GameData.MaxServerSayLength);
            return pw;
        }

        public static void CreateDynamicEntity(PacketWriter pw, DynamicEntity dynamicEntity)
        {
            pw.Write(ServerPacketID.CreateDynamicEntity);
            pw.Write(dynamicEntity.MapEntityIndex);
            DynamicEntityFactory.Write(pw, dynamicEntity);
        }

        public static PacketWriter CreateDynamicEntity(DynamicEntity dynamicEntity)
        {
            PacketWriter pw = GetWriter();
            CreateDynamicEntity(pw, dynamicEntity);
            return pw;
        }

        /// <summary>
        /// Gets a PacketWriter to use from the internal pool. It is important that this
        /// PacketWriter is disposed of properly when done.
        /// </summary>
        /// <returns>PacketWriter to use.</returns>
        public static PacketWriter GetWriter()
        {
            return _writerPool.Create();
        }

        /// <summary>
        /// Gets a PacketWriter to use from the internal pool. It is important that this
        /// PacketWriter is disposed of properly when done.
        /// </summary>
        /// <param name="id">ServerPacketID that this PacketWriter will be writing.</param>
        /// <returns>PacketWriter to use.</returns>
        static PacketWriter GetWriter(ServerPacketID id)
        {
            PacketWriter pw = _writerPool.Create();
            pw.Write(id);
            return pw;
        }

        /// <summary>
        /// Tells the user their login attempt was successful.
        /// </summary>
        public static PacketWriter LoginSuccessful()
        {
            return GetWriter(ServerPacketID.LoginSuccessful);
        }

        /// <summary>
        /// Tells the user their login attempt was unsuccessful.
        /// </summary>
        /// <param name="gameMessage">GameMessage for explaining why the login was unsuccessful.</param>
        public static PacketWriter LoginUnsuccessful(GameMessage gameMessage)
        {
            var pw = GetWriter(ServerPacketID.LoginUnsuccessful);
            pw.Write(gameMessage);
            return pw;
        }

        /// <summary>
        /// Tells the user their login attempt was unsuccessful.
        /// </summary>
        /// <param name="gameMessage">GameMessage for explaining why the login was unsuccessful.</param>
        /// <param name="p">Arguments for the GameMessage.</param>
        public static PacketWriter LoginUnsuccessful(GameMessage gameMessage, params object[] p)
        {
            var pw = GetWriter(ServerPacketID.LoginUnsuccessful);
            pw.Write(gameMessage, p);
            return pw;
        }

        public static PacketWriter NotifyExpCash(uint exp, uint cash)
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

        public static PacketWriter NotifyLevel(MapEntityIndex mapEntityIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.NotifyLevel);
            pw.Write(mapEntityIndex);
            return pw;
        }

        public static PacketWriter Ping()
        {
            return GetWriter(ServerPacketID.Ping);
        }

        public static PacketWriter RemoveDynamicEntity(DynamicEntity dynamicEntity)
        {
            PacketWriter pw = GetWriter(ServerPacketID.RemoveDynamicEntity);
            pw.Write(dynamicEntity.MapEntityIndex);
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
            pw.Write(item.BaseStats);
            pw.Write(item.ReqStats);
            return pw;
        }

        public static PacketWriter SendMessage(GameMessage gameMessage)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SendMessage);
            pw.Write(gameMessage);
            return pw;
        }

        public static PacketWriter SendMessage(GameMessage gameMessage, params object[] p)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SendMessage);
            pw.Write(gameMessage, p);
            return pw;
        }

        public static void SetInventorySlot(PacketWriter pw, InventorySlot slot, GrhIndex graphic, byte amount)
        {
            pw.Write(ServerPacketID.SetInventorySlot);
            pw.Write(slot);
            pw.Write(graphic);
            pw.Write(amount);
        }

        public static PacketWriter SetInventorySlot(InventorySlot slot, GrhIndex graphic, byte amount)
        {
            PacketWriter pw = GetWriter();
            SetInventorySlot(pw, slot, graphic, amount);
            return pw;
        }

        public static void SetMap(PacketWriter pw, MapIndex mapIndex)
        {
            pw.Write(ServerPacketID.SetMap);
            pw.Write(mapIndex);
        }

        public static PacketWriter SetMap(MapIndex mapIndex)
        {
            PacketWriter pw = GetWriter();
            SetMap(pw, mapIndex);
            return pw;
        }

        public static void SetUserChar(PacketWriter pw, MapEntityIndex mapEntityIndex)
        {
            pw.Write(ServerPacketID.SetUserChar);
            pw.Write(mapEntityIndex);
        }

        /// <summary>
        /// Sets which map character is the one controlled by the user
        /// </summary>
        /// <param name="mapEntityIndex">Map character index controlled by the user</param>
        public static PacketWriter SetUserChar(MapEntityIndex mapEntityIndex)
        {
            PacketWriter pw = GetWriter();
            SetUserChar(pw, mapEntityIndex);
            return pw;
        }

        public static void SynchronizeDynamicEntity(PacketWriter pw, DynamicEntity dynamicEntity)
        {
            pw.Write(ServerPacketID.CreateDynamicEntity);
            pw.Write(dynamicEntity.MapEntityIndex);
            dynamicEntity.Serialize(pw);
        }

        public static PacketWriter SynchronizeDynamicEntity(DynamicEntity dynamicEntity)
        {
            PacketWriter pw = GetWriter();
            SynchronizeDynamicEntity(pw, dynamicEntity);
            return pw;
        }

        public static PacketWriter UpdateEquipmentSlot(EquipmentSlot slot, GrhIndex? graphic)
        {
            PacketWriter pw = GetWriter(ServerPacketID.UpdateEquipmentSlot);
            pw.Write(slot);
            pw.Write(graphic.HasValue);

            if (graphic.HasValue)
                pw.Write(graphic.Value);

            return pw;
        }

        public static void UpdateStat(PacketWriter pw, IStat stat, StatCollectionType statCollectionType)
        {
            bool isBaseStat = (statCollectionType == StatCollectionType.Base);

            pw.Write(ServerPacketID.UpdateStat);
            pw.Write(isBaseStat);
            pw.Write(stat.StatType);
            stat.Write(pw);
        }

        public static PacketWriter UpdateStat(IStat stat, StatCollectionType statCollectionType)
        {
            PacketWriter pw = GetWriter();
            UpdateStat(pw, stat, statCollectionType);
            return pw;
        }

        public static void UpdateVelocityAndPosition(PacketWriter pw, DynamicEntity dynamicEntity, int currentTime)
        {
            pw.Write(ServerPacketID.UpdateVelocityAndPosition);
            pw.Write(dynamicEntity.MapEntityIndex);
            dynamicEntity.SerializePositionAndVelocity(pw, currentTime);
        }

        public static PacketWriter UpdateVelocityAndPosition(DynamicEntity dynamicEntity, int currentTime)
        {
            PacketWriter pw = GetWriter();
            UpdateVelocityAndPosition(pw, dynamicEntity, currentTime);
            return pw;
        }

        public static PacketWriter UseEntity(MapEntityIndex usedEntity, MapEntityIndex usedBy)
        {
            PacketWriter pw = GetWriter(ServerPacketID.UseEntity);
            pw.Write(usedEntity);
            pw.Write(usedBy);
            return pw;
        }
    }
}