using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using Microsoft.Xna.Framework;

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

        public static PacketWriter ChatShout(string name, string text)
        {
            PacketWriter pw = GetWriter(ServerPacketID.ChatShout);
            pw.Write(name, GameData.MaxServerSayNameLength);
            pw.Write(text, GameData.MaxServerSayLength);
            return pw;
        }

        public static PacketWriter ChatTell(string name, string text)
        {
            PacketWriter pw = GetWriter(ServerPacketID.ChatTell);
            pw.Write(name, GameData.MaxServerSayNameLength);
            pw.Write(text, GameData.MaxServerSayLength);
            return pw;
        }

        public static PacketWriter CreateMapItem(ushort mapItemIndex, Vector2 pos, Vector2 size, ushort graphic)
        {
            if (size.X > byte.MaxValue || size.Y > byte.MaxValue || size.X < byte.MinValue || size.Y < byte.MinValue)
                throw new ArgumentOutOfRangeException("size", "Cannot convert parameter size's values bytes.");

            PacketWriter pw = GetWriter(ServerPacketID.CreateMapItem);
            pw.Write(mapItemIndex);
            pw.Write(pos);
            pw.Write((byte)size.X);
            pw.Write((byte)size.Y);
            pw.Write(graphic);
            return pw;
        }

        public static PacketWriter CreateNPC(ushort mapCharIndex, string name, Vector2 position, ushort bodyIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.CreateNPC);
            pw.Write(mapCharIndex);
            pw.Write(name);
            pw.Write(position);
            pw.Write(bodyIndex);
            return pw;
        }

        /// <summary>
        /// Creates a user on the map
        /// </summary>
        /// <param name="mapCharIndex">Map character index of the user</param>
        /// <param name="name">Name of the user</param>
        /// <param name="pos">Position of the user</param>
        /// <param name="bodyIndex">Index of the body information</param>
        public static PacketWriter CreateUser(ushort mapCharIndex, string name, Vector2 pos, ushort bodyIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.CreateUser);
            pw.Write(mapCharIndex);
            pw.Write(name);
            pw.Write(pos);
            pw.Write(bodyIndex);
            return pw;
        }

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

        public static PacketWriter NotifyFullInv()
        {
            // TODO: Unused packet - remove completely
            return GetWriter(ServerPacketID.NotifyFullInv);
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

        /// <summary>
        /// Removes a character from the map
        /// </summary>
        /// <param name="mapCharIndex">Map index of the character to remove</param>
        public static PacketWriter RemoveChar(ushort mapCharIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.RemoveChar);
            pw.Write(mapCharIndex);
            return pw;
        }

        public static PacketWriter RemoveMapItem(ushort mapItemIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.RemoveMapItem);
            pw.Write(mapItemIndex);
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
            pw.Write((byte)p.Length);

            // Write all the parameters
            for (int i = 0; i < p.Length; i++)
            {
                // Convert to a string, and ensure the string is short enough (trimming if it is too long)
                string str = p[i].ToString();
                if (str.Length > GameData.MaxServerMessageParameterLength)
                    str = str.Substring(0, GameData.MaxServerMessageParameterLength);

                // Write the string
                pw.Write(str, GameData.MaxServerMessageParameterLength);
            }

            return pw;
        }

        public static PacketWriter SetCharHeadingLeft(ushort mapCharIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetCharHeadingLeft);
            pw.Write(mapCharIndex);
            return pw;
        }

        public static PacketWriter SetCharHeadingRight(ushort mapCharIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetCharHeadingRight);
            pw.Write(mapCharIndex);
            return pw;
        }

        public static PacketWriter SetCharName(ushort mapCharIndex, string name)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetCharName);
            pw.Write(mapCharIndex);
            pw.Write(name);
            return pw;
        }

        /// <summary>
        /// Sets the position and velocity of a character
        /// </summary>
        /// <param name="mapCharIndex">Map index of the character to update</param>
        /// <param name="pos">Character position</param>
        /// <param name="velocity">Character velocity</param>
        public static PacketWriter SetCharVelocity(ushort mapCharIndex, Vector2 pos, Vector2 velocity)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetCharVelocity);
            pw.Write(mapCharIndex);
            pw.Write(pos);
            pw.Write(velocity);
            return pw;
        }

        /// <summary>
        /// Sets the position and X velocity of a character, assuming the Y velocity is zero
        /// </summary>
        /// <param name="mapCharIndex">Map index of the character to update</param>
        /// <param name="pos">Character position</param>
        /// <param name="vX">X velocity of the character</param>
        public static PacketWriter SetCharVelocityX(ushort mapCharIndex, Vector2 pos, float vX)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetCharVelocityX);
            pw.Write(mapCharIndex);
            pw.Write(pos);
            pw.Write(vX);
            return pw;
        }

        /// <summary>
        /// Sets the position and Y velocity of a character, assuming the X velocity is zero
        /// </summary>
        /// <param name="mapCharIndex">Map index of the character to update</param>
        /// <param name="pos">Character position</param>
        /// <param name="vY">Y velocity of the character</param>
        public static PacketWriter SetCharVelocityY(ushort mapCharIndex, Vector2 pos, float vY)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetCharVelocityY);
            pw.Write(mapCharIndex);
            pw.Write(pos);
            pw.Write(vY);
            return pw;
        }

        /// <summary>
        /// Sends the position of the character and assumes the velocity is zero
        /// </summary>
        /// <param name="mapCharIndex">Map index of the character</param>
        /// <param name="pos">Character position</param>
        public static PacketWriter SetCharVelocityZero(ushort mapCharIndex, Vector2 pos)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetCharVelocityZero);
            pw.Write(mapCharIndex);
            pw.Write(pos);
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

        public static PacketWriter SetMap(ushort mapIndex)
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

        /// <summary>
        /// Teleports a character to a designated location (instead of smoothly translating
        /// the character) and assumes the character's velocity is zero
        /// </summary>
        /// <param name="mapCharIndex">Map index of the character</param>
        /// <param name="pos">Character position</param>
        public static PacketWriter TeleportChar(ushort mapCharIndex, Vector2 pos)
        {
            PacketWriter pw = GetWriter(ServerPacketID.TeleportChar);
            pw.Write(mapCharIndex);
            pw.Write(pos);
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
    }
}