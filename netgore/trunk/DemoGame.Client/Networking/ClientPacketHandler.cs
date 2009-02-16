using System;
using System.Bits;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.Network;

#pragma warning disable 168

namespace DemoGame.Client
{
    public class ClientPacketHandler : IMessageProcessor, IGetTime
    {
        readonly GameplayScreen _gameplayScreen;
        readonly Stopwatch _pingWatch = new Stopwatch();
        readonly MessageProcessorManager _ppManager;
        readonly ISocketSender _socketSender;

        /// <summary>
        /// Client has supplied invalid account information
        /// </summary>
        public event SocketEventHandler OnInvalidAccount;

        /// <summary>
        /// Client has been successfully logged in
        /// </summary>
        public event SocketEventHandler OnLogin;

        /// <summary>
        /// Gets the GameplayScreen the ClientSockets are a part of
        /// </summary>
        public GameplayScreen GameplayScreen
        {
            get { return _gameplayScreen; }
        }

        /// <summary>
        /// Gets the ItemInfoTooltip from the GameplayScreen
        /// </summary>
        public ItemInfoTooltip ItemInfoTooltip
        {
            get { return GameplayScreen.ItemInfoTooltip; }
        }

        /// <summary>
        /// Gets the map used by the world (Parent.World.Map)
        /// </summary>
        public Map Map
        {
            get { return GameplayScreen.World.Map; }
            set { GameplayScreen.World.Map = value; }
        }

        /// <summary>
        /// Gets the user's character
        /// </summary>
        public Character User
        {
            get { return GameplayScreen.UserChar; }
        }

        public UserEquipped UserEquipped
        {
            get { return GameplayScreen.UserEquipped; }
        }

        /// <summary>
        /// Gets the user's inventory
        /// </summary>
        public Inventory UserInventory
        {
            get { return GameplayScreen.Inventory; }
        }

        /// <summary>
        /// Gets the user's CharacterStats
        /// </summary>
        public CharacterStats UserStats
        {
            get { return GameplayScreen.UserStats; }
        }

        /// <summary>
        /// Gets the world used by the game (Parent.World)
        /// </summary>
        public World World
        {
            get { return GameplayScreen.World; }
        }

        public ClientPacketHandler(ISocketSender socketSender, GameplayScreen gameplayScreen)
        {
            _socketSender = socketSender;
            _gameplayScreen = gameplayScreen;
            _ppManager = new MessageProcessorManager(this);
        }

        static List<StyledText> CreateChatText(string name, string method, string message)
        {
            if (string.IsNullOrEmpty(method))
                return new List<StyledText> { new StyledText(name + ": ", Color.Green), new StyledText(message, Color.Black) };
            else
                return new List<StyledText>
                       { new StyledText(name + " " + method + ": ", Color.Green), new StyledText(message, Color.Black) };
        }

        public void Ping()
        {
            if (_pingWatch.IsRunning)
                return;

            _pingWatch.Reset();

            using (PacketWriter pw = ClientPacket.Ping())
            {
                _socketSender.Send(pw);
            }

            _pingWatch.Start();
        }

        [MessageHandler((byte)ServerPacketID.CharAttack)]
        void RecvCharAttack(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            chr.Attack();
        }

        [MessageHandler((byte)ServerPacketID.CharDamage)]
        void RecvCharDamage(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();
            int damage = r.ReadInt();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            GameplayScreen.DamageTextPool.Create(damage, chr, GetTime());
        }

        [MessageHandler((byte)ServerPacketID.Chat)]
        void RecvChat(TCPSocket conn, BitStream r)
        {
            string text = r.ReadString(GameData.MaxServerSayLength);
            GameplayScreen.AppendToChatOutput(text);
        }

        [MessageHandler((byte)ServerPacketID.ChatSay)]
        void RecvChatSay(TCPSocket conn, BitStream r)
        {
            string name = r.ReadString(GameData.MaxServerSayNameLength);
            ushort mapCharIndex = r.ReadUShort();
            string text = r.ReadString(GameData.MaxServerSayLength);

            // NOTE: Make use of the mapCharIndex for a chat bubble
            GameplayScreen.AppendToChatOutput(CreateChatText(name, "says", text));
        }

        [MessageHandler((byte)ServerPacketID.ChatShout)]
        void RecvChatShout(TCPSocket conn, BitStream r)
        {
            string name = r.ReadString(GameData.MaxServerSayNameLength);
            string text = r.ReadString(GameData.MaxServerSayLength);

            GameplayScreen.AppendToChatOutput(CreateChatText(name, "shouts", text));
        }

        [MessageHandler((byte)ServerPacketID.ChatTell)]
        void RecvChatTell(TCPSocket conn, BitStream r)
        {
            string name = r.ReadString(GameData.MaxServerSayNameLength);
            string text = r.ReadString(GameData.MaxServerSayLength);

            GameplayScreen.AppendToChatOutput(CreateChatText(name, "whispers", text));
        }

        [MessageHandler((byte)ServerPacketID.CreateMapItem)]
        void RecvCreateMapItem(TCPSocket conn, BitStream r)
        {
            ushort mapIndex = r.ReadUShort();
            Vector2 pos = r.ReadVector2();
            byte width = r.ReadByte();
            byte height = r.ReadByte();
            ushort graphicIndex = r.ReadUShort();

            Vector2 size = new Vector2(width, height);
            ItemEntity item = new ItemEntity(mapIndex, pos, size, graphicIndex, GetTime());
            Map.AddEntity(item);
        }

        [MessageHandler((byte)ServerPacketID.CreateNPC)]
        void RecvCreateNPC(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();
            string name = r.ReadString();
            Vector2 pos = r.ReadVector2();
            ushort bodyIndex = r.ReadUShort();

            Character c = new Character(Map, pos, GameData.Body(bodyIndex), GameplayScreen.SkeletonManager)
                          { Name = name, MapCharIndex = mapCharIndex };
            Map.AddCharacter(c, mapCharIndex);
        }

        [MessageHandler((byte)ServerPacketID.CreateUser)]
        void RecvCreateUser(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();
            string name = r.ReadString();
            Vector2 pos = r.ReadVector2();
            ushort bodyIndex = r.ReadUShort();

            Character c = new Character(Map, pos, GameData.Body(bodyIndex), GameplayScreen.SkeletonManager)
                          { Name = name, MapCharIndex = mapCharIndex };
            Map.AddCharacter(c, mapCharIndex);
        }

        [MessageHandler((byte)ServerPacketID.InvalidAccount)]
        void RecvInvalidAccount(TCPSocket conn, BitStream r)
        {
            if (OnInvalidAccount != null)
                OnInvalidAccount(conn);
        }

        [MessageHandler((byte)ServerPacketID.Login)]
        void RecvLogin(TCPSocket conn, BitStream r)
        {
            if (OnLogin != null)
                OnLogin(conn);
        }

        [MessageHandler((byte)ServerPacketID.NotifyExpCash)]
        void RecvNotifyExpCash(TCPSocket conn, BitStream r)
        {
            int exp = r.ReadInt();
            int cash = r.ReadInt();

            Character userChar = World.UserChar;
            if (userChar == null)
            {
                Debug.Fail("UserChar is null.");
                return;
            }

            string msg = string.Format("Got {0} exp and {1} cash", exp, cash);
            _gameplayScreen.InfoBox.Add(msg);
        }

        [MessageHandler((byte)ServerPacketID.NotifyFullInv)]
        void RecvNotifyFullInv(TCPSocket conn, BitStream r)
        {
            _gameplayScreen.InfoBox.Add("Your inventory is full!");
        }

        [MessageHandler((byte)ServerPacketID.NotifyLevel)]
        void RecvNotifyLevel(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            if (chr == World.UserChar)
                _gameplayScreen.InfoBox.Add("You have leveled up!");
        }

        [MessageHandler((byte)ServerPacketID.NotifyGetItem)]
        void RecvNotifyPickup(TCPSocket conn, BitStream r)
        {
            string name = r.ReadString();
            byte amount = r.ReadByte();

            string msg;
            if (amount > 1)
                msg = string.Format("You got {0} {1}s", amount, name);
            else
                msg = string.Format("You got a {0}", name);

            _gameplayScreen.InfoBox.Add(msg);
        }

        [MessageHandler((byte)ServerPacketID.Ping)]
        void RecvPing(TCPSocket conn, BitStream r)
        {
            _pingWatch.Stop();
        }

        [MessageHandler((byte)ServerPacketID.RemoveChar)]
        void RecvRemoveChar(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            Map.RemoveEntity(chr);
        }

        [MessageHandler((byte)ServerPacketID.RemoveMapItem)]
        void RecvRemoveMapItem(TCPSocket conn, BitStream r)
        {
            ushort mapItemIndex = r.ReadUShort();

            ItemEntity item = Map.GetItem(mapItemIndex) as ItemEntity;
            if (item == null)
                return;

            Map.RemoveEntity(item);
        }

        [MessageHandler((byte)ServerPacketID.SendItemInfo)]
        void RecvSendItemInfo(TCPSocket conn, BitStream r)
        {
            string name = r.ReadString();
            string desc = r.ReadString();
            int value = r.ReadInt();

            ItemInfo itemInfo = ItemInfoTooltip.ItemInfo;

            itemInfo.SetItemInfo(name, desc, value);
            r.ReadStatCollection(itemInfo.Stats);

            itemInfo.SetAsUpdated();
        }

        [MessageHandler((byte)ServerPacketID.SendMessage)]
        void RecvSendMessage(TCPSocket conn, BitStream r)
        {
            byte messageID = r.ReadByte();
            byte paramCount = r.ReadByte();

            string[] parameters = null;
            if (paramCount > 0)
            {
                parameters = new string[paramCount];
                for (int i = 0; i < paramCount; i++)
                {
                    parameters[i] = r.ReadString(GameData.MaxServerMessageParameterLength);
                }
            }

            // NOTE: Add proper handling of messages
            GameplayScreen.AppendToChatOutput("I received a Server.SendMessage, but fuck if I know what to do with it! :o",
                                              Color.Red);
        }

        [MessageHandler((byte)ServerPacketID.SetCharHeadingLeft)]
        void RecvSetCharHeadingLeft(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            chr.SetHeading(Direction.West);
        }

        [MessageHandler((byte)ServerPacketID.SetCharHeadingRight)]
        void RecvSetCharHeadingRight(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            chr.SetHeading(Direction.East);
        }

        [MessageHandler((byte)ServerPacketID.SetCharName)]
        void RecvSetCharName(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();
            string name = r.ReadString();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            chr.Name = name;
        }

        [MessageHandler((byte)ServerPacketID.SetCharVelocity)]
        void RecvSetCharVelocity(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();
            Vector2 pos = r.ReadVector2();
            Vector2 velocity = r.ReadVector2();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            chr.SetVelocity(velocity);
            chr.UpdatePosition(pos);
        }

        [MessageHandler((byte)ServerPacketID.SetCharVelocityX)]
        void RecvSetCharVelocityX(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();
            Vector2 pos = r.ReadVector2();
            float velocityX = r.ReadFloat();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            chr.SetVelocity(new Vector2(velocityX, 0));
            chr.UpdatePosition(pos);
        }

        [MessageHandler((byte)ServerPacketID.SetCharVelocityY)]
        void RecvSetCharVelocityY(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();
            Vector2 pos = r.ReadVector2();
            float velocityY = r.ReadFloat();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            chr.SetVelocity(new Vector2(0, velocityY));
            chr.UpdatePosition(pos);
        }

        [MessageHandler((byte)ServerPacketID.SetCharVelocityZero)]
        void RecvSetCharVelocityZero(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();
            Vector2 pos = r.ReadVector2();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            chr.SetVelocity(Vector2.Zero);
            chr.UpdatePosition(pos);
        }

        [MessageHandler((byte)ServerPacketID.SetInventorySlot)]
        void RecvSetInventorySlot(TCPSocket conn, BitStream r)
        {
            byte slot = r.ReadByte();
            ushort graphic = r.ReadUShort();
            byte amount = r.ReadByte();

            UserInventory.Update(slot, graphic, amount, GetTime());
        }

        [MessageHandler((byte)ServerPacketID.SetMap)]
        void RecvSetMap(TCPSocket conn, BitStream r)
        {
            ushort mapIndex = r.ReadUShort();

            // Create the new map
            Map newMap = new Map(mapIndex, World, GameplayScreen.ScreenManager.GraphicsDevice);
            newMap.Load(ContentPaths.Build);

            // Change maps
            World.SetMap(newMap);

            // Unload all map content from the previous map and from the new map loading
            GameplayScreen.ScreenManager.MapContent.Unload();
        }

        [MessageHandler((byte)ServerPacketID.SetUserChar)]
        void RecvSetUserChar(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();
            World.UserCharIndex = mapCharIndex;
        }

        [MessageHandler((byte)ServerPacketID.TeleportChar)]
        void RecvTeleportChar(TCPSocket conn, BitStream r)
        {
            ushort mapCharIndex = r.ReadUShort();
            Vector2 pos = r.ReadVector2();

            Character chr = Map.GetCharacter(mapCharIndex) as Character;
            if (chr == null)
                return;

            chr.Teleport(pos);
            chr.SetVelocity(Vector2.Zero);
        }

        [MessageHandler((byte)ServerPacketID.UpdateEquipmentSlot)]
        void RecvUpdateEquipmentSlot(TCPSocket conn, BitStream r)
        {
            EquipmentSlot slot = r.ReadEquipmentSlot();
            bool hasValue = r.ReadBool();

            if (hasValue)
            {
                ushort graphic = r.ReadUShort();
                UserEquipped.SetSlot(slot, graphic);
            }
            else
                UserEquipped.ClearSlot(slot);
        }

        [MessageHandler((byte)ServerPacketID.UpdateStat)]
        void RecvUpdateStat(TCPSocket conn, BitStream r)
        {
            r.ReadStat(UserStats);
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public int GetTime()
        {
            return GameplayScreen.GetTime();
        }

        #endregion

        #region IMessageProcessor Members

        /// <summary>
        /// Handles received data and forwards it to the corresponding MessageProcessors.
        /// </summary>
        /// <param name="rec">SocketReceiveData to process.</param>
        public void Process(SocketReceiveData rec)
        {
            _ppManager.Process(rec);
        }

        /// <summary>
        /// Handles received data and forwards it to the corresponding MessageProcessors.
        /// </summary>
        /// <param name="socket">Socket the data came from.</param>
        /// <param name="data">Data to process.</param>
        public void Process(TCPSocket socket, byte[] data)
        {
            _ppManager.Process(socket, data);
        }

        /// <summary>
        /// Handles a list of received data and forwards it to the corresponding MessageProcessors.
        /// </summary>
        /// <param name="recvData">List of SocketReceiveData to process.</param>
        public void Process(IEnumerable<SocketReceiveData> recvData)
        {
            _ppManager.Process(recvData);
        }

        #endregion
    }
}