using System;
using NetGore.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;

namespace DemoGame.Extensions
{
    /// <summary>
    /// Extensions for handling I/O of specialized types in the BitStream.
    /// </summary>
    public static class BitStreamExtensions
    {
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
        /// Writes a StatType to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="statType">StatType to write.</param>
        public static void Write(this BitStream bitStream, StatType statType)
        {
            bitStream.Write((byte)statType);
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