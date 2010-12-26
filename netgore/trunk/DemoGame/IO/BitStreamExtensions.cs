using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Features.Shops;
using NetGore.IO;
using NetGore.Stats;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame
{
    /// <summary>
    /// Extensions for handling I/O of specialized types in the <see cref="BitStream"/>.
    /// </summary>
    public static class BitStreamExtensions
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly int _clientPacketIDBits;
        static readonly int _gameMessageIDBits;
        static readonly int _serverPacketIDBits;

        /// <summary>
        /// Initializes the <see cref="BitStreamExtensions"/> class.
        /// </summary>
        /// <exception cref="TypeLoadException">Invalid required bit amount on <see cref="_clientPacketIDBits"/>.</exception>
        /// <exception cref="TypeLoadException">Invalid required bit amount on <see cref="_serverPacketIDBits"/>.</exception>
        static BitStreamExtensions()
        {
            _clientPacketIDBits = EnumHelper<ClientPacketID>.BitsRequired;
            _serverPacketIDBits = EnumHelper<ServerPacketID>.BitsRequired;
            _gameMessageIDBits = EnumHelper<GameMessage>.BitsRequired;

            if (_clientPacketIDBits <= 0)
                throw new TypeLoadException("Invalid required bit amount on _clientPacketIDBits: " + _clientPacketIDBits);
            if (_serverPacketIDBits <= 0)
                throw new TypeLoadException("Invalid required bit amount on _serverPacketIDBits: " + _serverPacketIDBits);
        }

        /// <summary>
        /// Reads a GameMessage from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <param name="gameMessages">Collection of GameMessages to use to grab the message.</param>
        /// <returns>String of the parsed GameMessage read from the BitStream.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="gameMessages" /> is <c>null</c>.</exception>
        public static string ReadGameMessage(this BitStream bitStream, GameMessageCollection gameMessages)
        {
            if (gameMessages == null)
                throw new ArgumentNullException("gameMessages");

            var messageID = bitStream.ReadUInt(_gameMessageIDBits);
            var paramCount = bitStream.ReadByte();

            // Parse the parameters
            string[] parameters = null;
            if (paramCount > 0)
            {
                parameters = new string[paramCount];
                for (var i = 0; i < paramCount; i++)
                {
                    parameters[i] = bitStream.ReadString(GameData.MaxServerMessageParameterLength);
                }
            }

            // Parse the message and return it
            var gameMessage = (GameMessage)messageID;
            var message = gameMessages.GetMessage(gameMessage, parameters);

            return message;
        }

        /// <summary>
        /// Reads a MapEntityIndex from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static MapEntityIndex ReadMapEntityIndex(this BitStream bitStream)
        {
            return bitStream.ReadMapEntityIndex(null);
        }

        /// <summary>
        /// Reads a <see cref="MapID"/> from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static MapID ReadMapID(this BitStream bitStream)
        {
            return bitStream.ReadMapID(null);
        }

        /// <summary>
        /// Reads a ShopItemIndex from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        public static ShopItemIndex ReadShopItemIndex(this BitStream bitStream)
        {
            return new ShopItemIndex(bitStream.ReadByte());
        }

        /// <summary>
        /// Reads a Vector2 from the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <returns>Vector2 read from the BitStream.</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static Vector2 ReadVector2(this BitStream bitStream)
        {
            return bitStream.ReadVector2(null);
        }

        /// <summary>
        /// Writes a Vector2 to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="vector2">Vector2 to write.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void Write(this BitStream bitStream, Vector2 vector2)
        {
            bitStream.Write(null, vector2);
        }

        /// <summary>
        /// Writes a GameMessage to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="gameMessage">GameMessage to write.</param>
        public static void Write(this BitStream bitStream, GameMessage gameMessage)
        {
            bitStream.Write((uint)gameMessage, _gameMessageIDBits);
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
            bitStream.Write((uint)gameMessage, _gameMessageIDBits);

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
                for (var i = 0; i < args.Length; i++)
                {
                    // Make sure the object isn't null
                    var obj = args[i];
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
                    var str = obj.ToString();
                    if (str.Length > GameData.MaxServerMessageParameterLength)
                        str = str.Substring(0, GameData.MaxServerMessageParameterLength);

                    // Write the string
                    bitStream.Write(str, GameData.MaxServerMessageParameterLength);
                }
            }
        }

        /// <summary>
        /// Writes a ShopItemIndex to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="shopItemIndex">ShopItemIndex to write.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void Write(this BitStream bitStream, ShopItemIndex shopItemIndex)
        {
            ((IValueWriter)bitStream).Write(null, shopItemIndex);
        }

        /// <summary>
        /// Writes a MapEntityIndex to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="mapEntityIndex">MapEntityIndex to write.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void Write(this BitStream bitStream, MapEntityIndex mapEntityIndex)
        {
            bitStream.Write(null, mapEntityIndex);
        }

        /// <summary>
        /// Writes a <see cref="ServerPacketID"/> to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="serverPacketID">ServerPacketID to write.</param>
        public static void Write(this BitStream bitStream, ServerPacketID serverPacketID)
        {
            bitStream.Write((uint)serverPacketID, _serverPacketIDBits);
        }

        /// <summary>
        /// Writes a <see cref="ClientPacketID"/> to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="clientPacketID">ClientPacketID to write.</param>
        public static void Write(this BitStream bitStream, ClientPacketID clientPacketID)
        {
            bitStream.Write((uint)clientPacketID, _clientPacketIDBits);
        }

        /// <summary>
        /// Writes a <see cref="MapID"/> to the BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="mapID">MapID to write.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void Write(this BitStream bitStream, MapID mapID)
        {
            bitStream.Write(null, mapID);
        }

        /// <summary>
        /// Writes a collection of stats to the BitStream. Only non-zero value stats are written since since the reader
        /// will zero all stat values first.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="statCollection">IStatCollection containing the stat values to write.</param>
        public static void Write(this BitStream bitStream, IEnumerable<Stat<StatType>> statCollection)
        {
            // Get the IEnumerable of all the non-zero stats
            var nonZeroStats = statCollection.Where(stat => stat.Value != 0);

            // Get the number of stats
            var numStats = (byte)nonZeroStats.Count();
            Debug.Assert(numStats == nonZeroStats.Count(),
                "Too many stats in the collection - byte overflow! numStats may need to be raised to a ushort.");

            // Write the number of stats so the reader knows how many stats to read
            bitStream.Write(numStats);

            // Write each individual non-zero stat
            foreach (var stat in nonZeroStats)
            {
                bitStream.Write(stat);
            }
        }
    }
}