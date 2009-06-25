using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.IO;

namespace DemoGame.Extensions
{
    /// <summary>
    /// Extensions for handling I/O of specialized types in the BitStream.
    /// </summary>
    public static class BitStreamExtensions
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Reads a StatType from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <returns>EquipmentSlot read from the BitStream.</returns>
        /// <exception cref="InvalidCastException">The EquipmentSlot was read from the BitStream, but the value is an invalid
        /// EquipmentSlot value.</exception>
        public static EquipmentSlot ReadEquipmentSlot(this BitStream bitStream)
        {
            byte value = bitStream.ReadByte();
            EquipmentSlot slot = (EquipmentSlot)value;

            // Ensure the value is a valid EquipmentSlot
            if (!slot.IsDefined())
            {
                const string errmsg = "Value `{0}` is not a valid EquipmentSlot.";
                Debug.Fail(string.Format(errmsg, value));
                throw new InvalidCastException(string.Format(errmsg, value));
            }

            return slot;
        }

        /// <summary>
        /// Reads a GameMessage from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <param name="gameMessages">Collection of GameMessages to use to grab the message.</param>
        /// <returns>String of the parsed GameMessage read from the BitStream.</returns>
        public static string ReadGameMessage(this BitStream bitStream, GameMessages gameMessages)
        {
            if (gameMessages == null)
                throw new ArgumentNullException("gameMessages");

            byte messageID = bitStream.ReadByte();
            byte paramCount = bitStream.ReadByte();

            // Parse the parameters
            string[] parameters = null;
            if (paramCount > 0)
            {
                parameters = new string[paramCount];
                for (int i = 0; i < paramCount; i++)
                {
                    parameters[i] = bitStream.ReadString(GameData.MaxServerMessageParameterLength);
                }
            }

            // Parse the message and return it
            GameMessage gameMessage = (GameMessage)messageID;
            string message = gameMessages.GetMessage(gameMessage, parameters);

            return message;
        }

        /// <summary>
        /// Reads a MapEntityIndex from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        public static MapEntityIndex ReadMapEntityIndex(this BitStream bitStream)
        {
            return MapEntityIndex.Read(bitStream);
        }

        /// <summary>
        /// Reads a MapIndex from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        public static MapIndex ReadMapIndex(this BitStream bitStream)
        {
            return MapIndex.Read(bitStream);
        }

        /// <summary>
        /// Reads an IStat from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <param name="statCollection">IStatCollection that the stat value will be loaded into. This IStatCollection
        /// must contain the StatType being read.</param>
        public static void ReadStat(this BitStream bitStream, IStatCollection statCollection)
        {
            StatType statType = bitStream.ReadStatType();
            IStat stat = statCollection.GetStat(statType);
            stat.Read(bitStream);
        }

        /// <summary>
        /// Reads a collection of stats. It is important to know all stats will be set to zero first, then be read.
        /// This is because only non-zero stats are sent.
        /// </summary>
        /// <param name="bitStream">BitStream to read from</param>
        /// <param name="statCollection">IStatCollection to read the stat values into. This IStatCollection
        /// must contain all of the StatTypes being read.</param>
        public static void ReadStatCollection(this BitStream bitStream, IStatCollection statCollection)
        {
            // Set all current stats to zero
            var stats = statCollection.Where(stat => stat.CanWrite);
            foreach (IStat stat in stats)
            {
                stat.Value = 0;
            }

            // Get the number of stats
            byte numStats = bitStream.ReadByte();

            // Read all of the stats
            for (int i = 0; i < numStats; i++)
            {
                ReadStat(bitStream, statCollection);
            }
        }

        /// <summary>
        /// Reads a StatType from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <returns>StatType read from the BitStream.</returns>
        /// <exception cref="InvalidCastException">The StatType was read from the BitStream, but the value is an invalid
        /// StatType value.</exception>
        public static StatType ReadStatType(this BitStream bitStream)
        {
            byte value = bitStream.ReadByte();
            StatType statType = (StatType)value;

            // Ensure the value is a valid StatType
            if (!statType.IsDefined())
            {
                const string errmsg = "Value `{0}` is not a valid StatType.";
                Debug.Fail(string.Format(errmsg, value));
                throw new InvalidCastException(string.Format(errmsg, value));
            }

            return statType;
        }

        /// <summary>
        /// Reads a Vector2 from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <returns>Vector2 read from the BitStream.</returns>
        public static Vector2 ReadVector2(this BitStream bitStream)
        {
            float x = bitStream.ReadFloat();
            float y = bitStream.ReadFloat();
            return new Vector2(x, y);
        }

        /// <summary>
        /// Writes a Vector2 to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="vector2">Vector2 to write.</param>
        public static void Write(this BitStream bitStream, Vector2 vector2)
        {
            bitStream.Write(vector2.X);
            bitStream.Write(vector2.Y);
        }

        /// <summary>
        /// Writes a GameMessage to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="gameMessage">GameMessage to write.</param>
        public static void Write(this BitStream bitStream, GameMessage gameMessage)
        {
            bitStream.Write((byte)gameMessage);
            bitStream.Write((byte)0);
        }

        /// <summary>
        /// Writes a GameMessage to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="gameMessage">GameMessage to write.</param>
        /// <param name="args">Arguments for the message.</param>
        public static void Write(this BitStream bitStream, GameMessage gameMessage, params object[] args)
        {
            // Write the message ID
            bitStream.Write((byte)gameMessage);

            // Write the parameter count and all of the parameters
            if (args == null || args.Length < 1)
            {
                // No parameters makes our life easy
                bitStream.Write((byte)0);
            }
            else
            {
                // One or more parameters are present, so start by writing the count
                bitStream.Write((byte)args.Length);

                // Write each parameter
                for (int i = 0; i < args.Length; i++)
                {
                    // Make sure the object isn't null
                    object obj = args[i];
                    if (obj == null)
                    {
                        const string errmsg = "Null object argument found when writing GameMessage.";
                        Debug.Fail(errmsg);
                        if (log.IsErrorEnabled)
                            log.Error(errmsg);

                        // Write out an error string instead for the parameter
                        bitStream.Write("NULL_PARAMETER_ERROR", GameData.MaxServerMessageParameterLength);
                        continue;
                    }

                    // Convert to a string, and ensure the string is short enough (trimming if it is too long)
                    string str = obj.ToString();
                    if (str.Length > GameData.MaxServerMessageParameterLength)
                        str = str.Substring(0, GameData.MaxServerMessageParameterLength);

                    // Write the string
                    bitStream.Write(str, GameData.MaxServerMessageParameterLength);
                }
            }
        }

        /// <summary>
        /// Writes a StatType to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="statType">StatType to write.</param>
        public static void Write(this BitStream bitStream, StatType statType)
        {
            bitStream.Write((byte)statType);
        }

        /// <summary>
        /// Writes a MapEntityIndex to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="mapEntityIndex">MapEntityIndex to write.</param>
        public static void Write(this BitStream bitStream, MapEntityIndex mapEntityIndex)
        {
            mapEntityIndex.Write(bitStream);
        }

        /// <summary>
        /// Writes a ServerPacketID to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="serverPacketID">ServerPacketID to write.</param>
        public static void Write(this BitStream bitStream, ServerPacketID serverPacketID)
        {
            byte value = (byte)serverPacketID;
            bitStream.Write(value, GameData.ServerMessageIDBitLength);
        }

        /// <summary>
        /// Writes a ClientPacketID to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="clientPacketID">ClientPacketID to write.</param>
        public static void Write(this BitStream bitStream, ClientPacketID clientPacketID)
        {
            byte value = (byte)clientPacketID;
            bitStream.Write(value, GameData.ClientMessageIDBitLength);
        }

        /// <summary>
        /// Writes a MapIndex to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="mapIndex">MapIndex to write.</param>
        public static void Write(this BitStream bitStream, MapIndex mapIndex)
        {
            mapIndex.Write(bitStream);
        }

        /// <summary>
        /// Writes an IStat to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="stat">IStat to write.</param>
        public static void Write(this BitStream bitStream, IStat stat)
        {
            bitStream.Write(stat.StatType);
            stat.Write(bitStream);
        }

        /// <summary>
        /// Writes an EquipmentSlot to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="slot">EquipmentSlot to write.</param>
        public static void Write(this BitStream bitStream, EquipmentSlot slot)
        {
            byte index = slot.GetIndex();
            bitStream.Write(index);
        }

        /// <summary>
        /// Writes a collection of stats to the BitStream. Only non-zero value stats are written since since the reader
        /// will zero all stat values first. All stats will be sent, even if the IStat.CanWrite is false.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="statCollection">IStatCollection containing the stat values to write.</param>
        public static void Write(this BitStream bitStream, IStatCollection statCollection)
        {
            // Get the IEnumerable of all the non-zero stats
            var nonZeroStats = statCollection.Where(stat => stat.Value != 0);

            // Get the number of stats
            byte numStats = (byte)nonZeroStats.Count();
            Debug.Assert(numStats == nonZeroStats.Count(),
                         "Too many stats in the collection - byte overflow! numStats may need to be raised to a ushort.");

            // Write the number of stats so the reader knows how many stats to read
            bitStream.Write(numStats);

            // Write each individual non-zero stat
            foreach (IStat stat in nonZeroStats)
            {
                bitStream.Write(stat);
            }
        }
    }
}