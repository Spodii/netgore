using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Client.NPCChat;
using DemoGame.DbObjs;
using log4net;
using NetGore;
using NetGore.Audio;
using NetGore.Content;
using NetGore.Features.Emoticons;
using NetGore.Features.GameTime;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Graphics.GUI;
using NetGore.IO;
using NetGore.Network;
using NetGore.NPCChat;
using NetGore.Stats;
using SFML.Graphics;

#pragma warning disable 168
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace DemoGame.Client
{
    class ClientPacketHandler : IMessageProcessor, IGetTime
    {
        /// <summary>
        /// Handles when a CreateAccount message is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="successful">If the account was successfully created.</param>
        /// <param name="errorMessage">If <paramref name="successful"/> is false, contains the error message from
        /// the server.</param>
        public delegate void CreateAccountEventHandler(IIPSocket sender, bool successful, string errorMessage);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly IQuestDescriptionCollection _questDescriptions = QuestDescriptionCollection.Create(ContentPaths.Build);

        readonly AccountCharacterInfos _accountCharacterInfos = new AccountCharacterInfos();
        readonly IDynamicEntityFactory _dynamicEntityFactory;
        readonly GameMessageCollection _gameMessages = GameMessageCollection.Create();
        readonly GameplayScreen _gameplayScreen;
        readonly Stopwatch _pingWatch = new Stopwatch();
        readonly MessageProcessorManager _ppManager;
        readonly ISocketSender _socketSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPacketHandler"/> class.
        /// </summary>
        /// <param name="socketSender">The socket sender.</param>
        /// <param name="gameplayScreen">The gameplay screen.</param>
        /// <param name="dynamicEntityFactory">The <see cref="IDynamicEntityFactory"/> used to serialize
        /// <see cref="DynamicEntity"/>s.</param>
        public ClientPacketHandler(ISocketSender socketSender, GameplayScreen gameplayScreen,
                                   IDynamicEntityFactory dynamicEntityFactory)
        {
            if (dynamicEntityFactory == null)
                throw new ArgumentNullException("dynamicEntityFactory");
            if (gameplayScreen == null)
                throw new ArgumentNullException("gameplayScreen");
            if (socketSender == null)
                throw new ArgumentNullException("socketSender");

            _dynamicEntityFactory = dynamicEntityFactory;
            _socketSender = socketSender;
            _gameplayScreen = gameplayScreen;
            _ppManager = new MessageProcessorManager(this, EnumHelper<ServerPacketID>.BitsRequired);
        }

        /// <summary>
        /// Notifies listeners when a message has been received about creating an account.
        /// </summary>
        public event CreateAccountEventHandler ReceivedCreateAccount;

        /// <summary>
        /// Notifies listeners when a message has been received about creating an account character.
        /// </summary>
        public event CreateAccountEventHandler ReceivedCreateAccountCharacter;

        /// <summary>
        /// Notifies listeners when a successful login request has been made.
        /// </summary>
        public event ClientPacketHandlerEventHandler ReceivedLoginSuccessful;

        /// <summary>
        /// Notifies listeners when an unsuccessful login request has been made.
        /// </summary>
        public event ClientPacketHandlerEventHandler<string> ReceivedLoginUnsuccessful;

        public AccountCharacterInfos AccountCharacterInfos
        {
            get { return _accountCharacterInfos; }
        }

        /// <summary>
        /// Gets the <see cref="GameplayScreen"/>.
        /// </summary>
        public GameplayScreen GameplayScreen
        {
            get { return _gameplayScreen; }
        }

        public UserGroupInformation GroupInfo
        {
            get { return GameplayScreen.UserInfo.GroupInfo; }
        }

        public UserGuildInformation GuildInfo
        {
            get { return GameplayScreen.UserInfo.GuildInfo; }
        }

        /// <summary>
        /// Gets the <see cref="Map"/> used by the <see cref="World"/>.
        /// </summary>
        public Map Map
        {
            get { return GameplayScreen.World.Map; }
        }

        public UserQuestInformation QuestInfo
        {
            get { return GameplayScreen.UserInfo.QuestInfo; }
        }

        ISoundManager SoundManager
        {
            get { return GameplayScreen.SoundManager; }
        }

        /// <summary>
        /// Gets the user's <see cref="Character"/>.
        /// </summary>
        public Character User
        {
            get { return GameplayScreen.UserChar; }
        }

        public UserInfo UserInfo
        {
            get { return GameplayScreen.UserInfo; }
        }

        /// <summary>
        /// Gets the world used by the game (Parent.World)
        /// </summary>
        public World World
        {
            get { return GameplayScreen.World; }
        }

        static IEnumerable<StyledText> CreateChatText(string name, string message)
        {
            var left = new StyledText(name + ": ", Color.Green);
            var right = new StyledText(message, Color.Black);
            return new List<StyledText> { left, right };
        }

        static void LogFailPlaySound(SoundID soundID)
        {
            const string errmsg = "Failed to play sound with ID `{0}`.";
            if (log.IsErrorEnabled)
                log.ErrorFormat(errmsg, soundID);

            Debug.Fail(string.Format(errmsg, soundID));
        }

        public void Ping()
        {
            if (_pingWatch.IsRunning)
                return;

            _pingWatch.Reset();

            using (var pw = ClientPacket.Ping())
            {
                _socketSender.Send(pw);
            }

            _pingWatch.Start();
        }

        [MessageHandler((byte)ServerPacketID.AcceptOrTurnInQuestReply)]
        void RecvAcceptOrTurnInQuestReply(IIPSocket conn, BitStream r)
        {
            var questID = r.ReadQuestID();
            var successful = r.ReadBool();
            var accepted = r.ReadBool();

            if (successful)
            {
                // Remove the quest from the available quests list
                var aqf = GameplayScreen.AvailableQuestsForm;
                if (aqf.IsVisible)
                    aqf.AvailableQuests = aqf.AvailableQuests.Where(x => x.QuestID != questID).ToImmutable();
            }
        }

        [MessageHandler((byte)ServerPacketID.AddStatusEffect)]
        void RecvAddStatusEffect(IIPSocket conn, BitStream r)
        {
            var statusEffectType = r.ReadEnum<StatusEffectType>();
            var power = r.ReadUShort();
            var secsLeft = r.ReadUShort();

            GameplayScreen.StatusEffectsForm.AddStatusEffect(statusEffectType, power, secsLeft);
            GameplayScreen.AppendToChatOutput(string.Format("Added status effect {0} with power {1}.", statusEffectType, power));
        }

        [MessageHandler((byte)ServerPacketID.CharAttack)]
        void RecvCharAttack(IIPSocket conn, BitStream r)
        {
            var mapCharIndex = r.ReadMapEntityIndex();

            var chr = Map.GetDynamicEntity<Character>(mapCharIndex);
            if (chr == null)
                return;

            SoundManager.Play("punch", chr);
            chr.Attack();
        }

        [MessageHandler((byte)ServerPacketID.CharDamage)]
        void RecvCharDamage(IIPSocket conn, BitStream r)
        {
            var mapCharIndex = r.ReadMapEntityIndex();
            var damage = r.ReadInt();

            var chr = Map.GetDynamicEntity<Character>(mapCharIndex);
            if (chr == null)
                return;

            GameplayScreen.DamageTextPool.Create(damage, chr, GetTime());
        }

        [MessageHandler((byte)ServerPacketID.Chat)]
        void RecvChat(IIPSocket conn, BitStream r)
        {
            var text = r.ReadString(GameData.MaxServerSayLength);
            GameplayScreen.AppendToChatOutput(text);
        }

        [MessageHandler((byte)ServerPacketID.ChatSay)]
        void RecvChatSay(IIPSocket conn, BitStream r)
        {
            var name = r.ReadString(GameData.MaxServerSayNameLength);
            var mapEntityIndex = r.ReadMapEntityIndex();
            var text = r.ReadString(GameData.MaxServerSayLength);

            var chatText = CreateChatText(name, text);
            GameplayScreen.AppendToChatOutput(chatText);

            DynamicEntity entity;
            if (!Map.TryGetDynamicEntity(mapEntityIndex, out entity))
            {
                const string errmsg = "Failed to get DynamicEntity `{0}` for creating a chat bubble with text `{1}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, mapEntityIndex, text);
            }
            else
                GameplayScreen.ChatBubbleManager.Add(entity, text, GetTime());
        }

        [MessageHandler((byte)ServerPacketID.CreateAccount)]
        void RecvCreateAccount(IIPSocket conn, BitStream r)
        {
            var successful = r.ReadBool();
            var errorMessage = successful ? string.Empty : r.ReadString();

            if (ReceivedCreateAccount != null)
                ReceivedCreateAccount(conn, successful, errorMessage);
        }

        [MessageHandler((byte)ServerPacketID.CreateAccountCharacter)]
        void RecvCreateAccountCharacter(IIPSocket conn, BitStream r)
        {
            var successful = r.ReadBool();
            var errorMessage = successful ? string.Empty : r.ReadString();

            if (ReceivedCreateAccountCharacter != null)
                ReceivedCreateAccountCharacter(conn, successful, errorMessage);
        }

        [MessageHandler((byte)ServerPacketID.CreateDynamicEntity)]
        void RecvCreateDynamicEntity(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var dynamicEntity = _dynamicEntityFactory.Read(r);
            Map.AddDynamicEntity(dynamicEntity, mapEntityIndex);

            var character = dynamicEntity as Character;
            if (character != null)
            {
                // HACK: Having to call this .Initialize() and pass these parameters seems hacky
                character.Initialize(Map, GameplayScreen.SkeletonManager);
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Created DynamicEntity with index `{0}` of type `{1}`", dynamicEntity.MapEntityIndex,
                               dynamicEntity.GetType());
        }

        [MessageHandler((byte)ServerPacketID.Emote)]
        void RecvEmote(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var emoticon = r.ReadEnum<Emoticon>();

            var entity = Map.GetDynamicEntity(mapEntityIndex);
            if (entity == null)
                return;

            EmoticonDisplayManager.Instance.Add(entity, emoticon, GetTime());
        }

        [MessageHandler((byte)ServerPacketID.EndChatDialog)]
        void RecvEndChatDialog(IIPSocket conn, BitStream r)
        {
            GameplayScreen.ChatDialogForm.EndDialog();
        }

        [MessageHandler((byte)ServerPacketID.GroupInfo)]
        void RecvGroupInfo(IIPSocket conn, BitStream r)
        {
            GroupInfo.Read(r);
        }

        [MessageHandler((byte)ServerPacketID.GuildInfo)]
        void RecvGuildInfo(IIPSocket conn, BitStream r)
        {
            GuildInfo.Read(r);
        }

        [MessageHandler((byte)ServerPacketID.HasQuestFinishRequirementsReply)]
        void RecvHasQuestFinishRequirementsReply(IIPSocket conn, BitStream r)
        {
            var questID = r.ReadQuestID();
            var hasRequirements = r.ReadBool();

            UserInfo.HasFinishQuestRequirements.SetRequirementsStatus(questID, hasRequirements);
        }

        [MessageHandler((byte)ServerPacketID.HasQuestStartRequirementsReply)]
        void RecvHasQuestStartRequirementsReply(IIPSocket conn, BitStream r)
        {
            var questID = r.ReadQuestID();
            var hasRequirements = r.ReadBool();

            UserInfo.HasStartQuestRequirements.SetRequirementsStatus(questID, hasRequirements);
        }

        [MessageHandler((byte)ServerPacketID.LoginSuccessful)]
        void RecvLoginSuccessful(IIPSocket conn, BitStream r)
        {
            if (ReceivedLoginSuccessful != null)
                ReceivedLoginSuccessful(this, conn);
        }

        [MessageHandler((byte)ServerPacketID.LoginUnsuccessful)]
        void RecvLoginUnsuccessful(IIPSocket conn, BitStream r)
        {
            var message = r.ReadGameMessage(_gameMessages);

            if (ReceivedLoginUnsuccessful != null)
                ReceivedLoginUnsuccessful(this, conn, message);
        }

        [MessageHandler((byte)ServerPacketID.NotifyExpCash)]
        void RecvNotifyExpCash(IIPSocket conn, BitStream r)
        {
            var exp = r.ReadInt();
            var cash = r.ReadInt();

            var userChar = World.UserChar;
            if (userChar == null)
            {
                Debug.Fail("UserChar is null.");
                return;
            }

            var msg = string.Format("Got {0} exp and {1} cash", exp, cash);
            _gameplayScreen.InfoBox.Add(msg);
        }

        [MessageHandler((byte)ServerPacketID.NotifyGetItem)]
        void RecvNotifyGetItem(IIPSocket conn, BitStream r)
        {
            var name = r.ReadString();
            var amount = r.ReadByte();

            string msg;
            if (amount > 1)
                msg = string.Format("You got {0} {1}s", amount, name);
            else
                msg = string.Format("You got a {0}", name);

            _gameplayScreen.InfoBox.Add(msg);
        }

        [MessageHandler((byte)ServerPacketID.NotifyLevel)]
        void RecvNotifyLevel(IIPSocket conn, BitStream r)
        {
            var mapCharIndex = r.ReadMapEntityIndex();

            var chr = Map.GetDynamicEntity<Character>(mapCharIndex);
            if (chr == null)
                return;

            if (chr == World.UserChar)
                _gameplayScreen.InfoBox.Add("You have leveled up!");
        }

        [MessageHandler((byte)ServerPacketID.Ping)]
        void RecvPing(IIPSocket conn, BitStream r)
        {
            _pingWatch.Stop();
        }

        [MessageHandler((byte)ServerPacketID.PlaySound)]
        void RecvPlaySound(IIPSocket conn, BitStream r)
        {
            var soundID = r.ReadSoundID();

            if (!SoundManager.Play(soundID))
                LogFailPlaySound(soundID);
        }

        [MessageHandler((byte)ServerPacketID.PlaySoundAt)]
        void RecvPlaySoundAt(IIPSocket conn, BitStream r)
        {
            var soundID = r.ReadSoundID();
            var position = r.ReadVector2();

            if (!SoundManager.Play(soundID, position))
                LogFailPlaySound(soundID);
        }

        [MessageHandler((byte)ServerPacketID.PlaySoundAtEntity)]
        void RecvPlaySoundAtEntity(IIPSocket conn, BitStream r)
        {
            var soundID = r.ReadSoundID();
            var index = r.ReadMapEntityIndex();

            var entity = Map.GetDynamicEntity(index);
            if (entity == null)
            {
                const string errmsg = "Failed to find DynamicEntity with MapEntityIndex `{0}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, index);
                Debug.Fail(string.Format(errmsg, index));
                return;
            }

            if (!SoundManager.Play(soundID, entity))
                LogFailPlaySound(soundID);
        }

        [MessageHandler((byte)ServerPacketID.QuestInfo)]
        void RecvQuestInfo(IIPSocket conn, BitStream r)
        {
            QuestInfo.Read(r);
        }

        [MessageHandler((byte)ServerPacketID.RemoveDynamicEntity)]
        void RecvRemoveDynamicEntity(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var dynamicEntity = Map.GetDynamicEntity(mapEntityIndex);

            if (dynamicEntity != null)
            {
                Map.RemoveEntity(dynamicEntity);
                if (log.IsInfoEnabled)
                    log.InfoFormat("Removed DynamicEntity with index `{0}`", mapEntityIndex);
                dynamicEntity.Dispose();
            }
            else
            {
                const string errmsg = "Could not remove DynamicEntity with index `{0}` - no DynamicEntity found.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, mapEntityIndex);
                Debug.Fail(string.Format(errmsg, mapEntityIndex));
            }
        }

        [MessageHandler((byte)ServerPacketID.RemoveStatusEffect)]
        void RecvRemoveStatusEffect(IIPSocket conn, BitStream r)
        {
            var statusEffectType = r.ReadEnum<StatusEffectType>();

            GameplayScreen.StatusEffectsForm.RemoveStatusEffect(statusEffectType);
            GameplayScreen.AppendToChatOutput(string.Format("Removed status effect {0}.", statusEffectType));
        }

        [MessageHandler((byte)ServerPacketID.RequestUDPConnection)]
        void RecvRequestUDPConnection(IIPSocket conn, BitStream r)
        {
            var challenge = r.ReadInt();

            ClientSockets.Instance.ConnectUDP(GameData.ServerIP, GameData.ServerUDPPort, challenge);
        }

        [MessageHandler((byte)ServerPacketID.SendAccountCharacters)]
        void RecvSendAccountCharacters(IIPSocket conn, BitStream r)
        {
            var count = r.ReadByte();
            var charInfos = new AccountCharacterInfo[count];
            for (var i = 0; i < count; i++)
            {
                var charInfo = r.ReadAccountCharacterInfo();
                charInfos[charInfo.Index] = charInfo;
            }

            _accountCharacterInfos.SetInfos(charInfos);
        }

        [MessageHandler((byte)ServerPacketID.SendEquipmentItemInfo)]
        void RecvSendEquipmentItemInfo(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadEnum<EquipmentSlot>();
            GameplayScreen.EquipmentInfoRequester.ReceiveInfo(slot, r);
        }

        [MessageHandler((byte)ServerPacketID.SendInventoryItemInfo)]
        void RecvSendInventoryItemInfo(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();
            GameplayScreen.InventoryInfoRequester.ReceiveInfo(slot, r);
        }

        [MessageHandler((byte)ServerPacketID.SendMessage)]
        void RecvSendMessage(IIPSocket conn, BitStream r)
        {
            var message = r.ReadGameMessage(_gameMessages);

            if (string.IsNullOrEmpty(message))
            {
                const string errmsg = "Received empty or null GameMessage.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return;
            }

            GameplayScreen.AppendToChatOutput(message, Color.Black);
        }

        [MessageHandler((byte)ServerPacketID.SetCash)]
        void RecvSetCash(IIPSocket conn, BitStream r)
        {
            var cash = r.ReadInt();
            UserInfo.Cash = cash;
        }

        [MessageHandler((byte)ServerPacketID.SetCharacterHPPercent)]
        void RecvSetCharacterHPPercent(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var percent = r.ReadByte();

            var character = Map.GetDynamicEntity<Character>(mapEntityIndex);
            if (character == null)
                return;

            character.HPPercent = percent;
        }

        [MessageHandler((byte)ServerPacketID.SetCharacterMPPercent)]
        void RecvSetCharacterMPPercent(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var percent = r.ReadByte();

            var character = Map.GetDynamicEntity<Character>(mapEntityIndex);
            if (character == null)
                return;

            character.MPPercent = percent;
        }

        [MessageHandler((byte)ServerPacketID.SetCharacterPaperDoll)]
        void RecvSetCharacterPaperDoll(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var count = r.ReadByte();

            var layers = new string[count];
            for (var i = 0; i < layers.Length; i++)
            {
                layers[i] = r.ReadString();
            }

            var character = Map.GetDynamicEntity<Character>(mapEntityIndex);
            if (character == null)
                return;

            character.CharacterSprite.SetPaperDollLayers(layers);
        }

        [MessageHandler((byte)ServerPacketID.SetChatDialogPage)]
        void RecvSetChatDialogPage(IIPSocket conn, BitStream r)
        {
            var pageID = r.ReadNPCChatDialogItemID();
            var skipCount = r.ReadByte();

            var responsesToSkip = new byte[skipCount];
            for (var i = 0; i < skipCount; i++)
            {
                responsesToSkip[i] = r.ReadByte();
            }

            GameplayScreen.ChatDialogForm.SetPageIndex(pageID, responsesToSkip);
        }

        [MessageHandler((byte)ServerPacketID.SetExp)]
        void RecvSetExp(IIPSocket conn, BitStream r)
        {
            var exp = r.ReadInt();
            UserInfo.Exp = exp;
        }

        [MessageHandler((byte)ServerPacketID.SetGameTime)]
        void RecvSetGameTime(IIPSocket conn, BitStream r)
        {
            var serverTimeBinary = r.ReadLong();
            var serverTime = DateTime.FromBinary(serverTimeBinary);

            GameDateTime.SetServerTimeOffset(serverTime);
        }

        [MessageHandler((byte)ServerPacketID.SetHP)]
        void RecvSetHP(IIPSocket conn, BitStream r)
        {
            var value = r.ReadSPValueType();
            UserInfo.HP = value;

            if (User == null)
                return;

            User.HPPercent = UserInfo.HPPercent;
        }

        [MessageHandler((byte)ServerPacketID.SetInventorySlot)]
        void RecvSetInventorySlot(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();
            var hasGraphic = r.ReadBool();
            var graphic = hasGraphic ? r.ReadGrhIndex() : GrhIndex.Invalid;
            var amount = r.ReadByte();

            UserInfo.Inventory.Update(slot, graphic, amount, GetTime());
        }

        [MessageHandler((byte)ServerPacketID.SetLevel)]
        void RecvSetLevel(IIPSocket conn, BitStream r)
        {
            var level = r.ReadByte();
            UserInfo.Level = level;
        }

        [MessageHandler((byte)ServerPacketID.SetMP)]
        void RecvSetMP(IIPSocket conn, BitStream r)
        {
            var value = r.ReadSPValueType();
            UserInfo.MP = value;

            if (User == null)
                return;

            User.MPPercent = UserInfo.MPPercent;
        }

        [MessageHandler((byte)ServerPacketID.SetMap)]
        void RecvSetMap(IIPSocket conn, BitStream r)
        {
            var mapID = r.ReadMapID();

            // Create the new map
            var newMap = new Map(mapID, World.Camera, World);
            newMap.Load(ContentPaths.Build, false, _dynamicEntityFactory);

            // Clear quest requirements caches
            UserInfo.HasStartQuestRequirements.Clear();

            // Change maps
            World.Map = newMap;

            // Unload all map content from the previous map and from the new map loading
            GameplayScreen.ScreenManager.Content.Unload(ContentLevel.Map);

            // Change the screens, if needed
            GameplayScreen.ScreenManager.SetScreen(GameplayScreen.ScreenName);
        }

        [MessageHandler((byte)ServerPacketID.SetProvidedQuests)]
        void RecvSetProvidedQuests(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var count = r.ReadByte();
            var questIDs = new QuestID[count];
            for (var i = 0; i < count; i++)
            {
                questIDs[i] = r.ReadQuestID();
            }

            var character = Map.GetDynamicEntity<Character>(mapEntityIndex);
            if (character != null)
                character.ProvidedQuests = questIDs;
        }

        [MessageHandler((byte)ServerPacketID.SetSkillGroupCooldown)]
        void RecvSetSkillGroupCooldown(IIPSocket conn, BitStream r)
        {
            var skillGroup = r.ReadByte();
            var cooldownTime = r.ReadUShort();

            GameplayScreen.SkillCooldownManager.SetCooldown(skillGroup, cooldownTime, GetTime());
        }

        [MessageHandler((byte)ServerPacketID.SetStatPoints)]
        void RecvSetStatPoints(IIPSocket conn, BitStream r)
        {
            var statPoints = r.ReadInt();
            UserInfo.StatPoints = statPoints;
        }

        [MessageHandler((byte)ServerPacketID.SetUserChar)]
        void RecvSetUserChar(IIPSocket conn, BitStream r)
        {
            var mapCharIndex = r.ReadMapEntityIndex();
            World.UserCharIndex = mapCharIndex;
        }

        [MessageHandler((byte)ServerPacketID.StartCastingSkill)]
        void RecvStartCastingSkill(IIPSocket conn, BitStream r)
        {
            var skillType = r.ReadEnum<SkillType>();
            var castTime = r.ReadUShort();

            GameplayScreen.SkillCastProgressBar.StartCasting(skillType, castTime);
        }

        [MessageHandler((byte)ServerPacketID.StartChatDialog)]
        void RecvStartChatDialog(IIPSocket conn, BitStream r)
        {
            var npcIndex = r.ReadMapEntityIndex();
            var dialogIndex = r.ReadNPCChatDialogID();

            var dialog = NPCChatManager.Instance[dialogIndex];
            GameplayScreen.ChatDialogForm.StartDialog(dialog);
        }

        [MessageHandler((byte)ServerPacketID.StartQuestChatDialog)]
        void RecvStartQuestChatDialog(IIPSocket conn, BitStream r)
        {
            var npcIndex = r.ReadMapEntityIndex();

            // Available quests
            var numAvailableQuests = r.ReadByte();
            var availableQuests = new QuestID[numAvailableQuests];
            for (var i = 0; i < availableQuests.Length; i++)
            {
                availableQuests[i] = r.ReadQuestID();
            }

            // Quests that can be turned in
            var numTurnInQuests = r.ReadByte();
            var turnInQuests = new QuestID[numTurnInQuests];
            for (var i = 0; i < turnInQuests.Length; i++)
            {
                turnInQuests[i] = r.ReadQuestID();
            }

            // For the quests that are available, make sure we set their status to not being able to be turned in (just in case)
            foreach (var id in availableQuests)
            {
                UserInfo.HasFinishQuestRequirements.SetRequirementsStatus(id, false);
            }

            // For the quests that were marked as being able to turn in, set their status to being able to be finished
            foreach (var id in turnInQuests)
            {
                UserInfo.HasFinishQuestRequirements.SetRequirementsStatus(id, true);
            }

            // Grab the descriptions for both the available quests and quests we can turn in
            var qds = availableQuests.Concat(turnInQuests).Distinct().Select(x => _questDescriptions.GetOrDefault(x));

            // Display the form
            GameplayScreen.AvailableQuestsForm.Display(qds, npcIndex);
        }

        [MessageHandler((byte)ServerPacketID.StartShopping)]
        void RecvStartShopping(IIPSocket conn, BitStream r)
        {
            var shopOwnerIndex = r.ReadMapEntityIndex();
            var canBuy = r.ReadBool();
            var name = r.ReadString();
            var itemCount = r.ReadByte();

            var items = new IItemTemplateTable[itemCount];
            for (var i = 0; i < itemCount; i++)
            {
                var value = new ItemTemplateTable();
                value.ReadState(r);
                items[i] = value;
            }

            var shopOwner = Map.GetDynamicEntity(shopOwnerIndex);
            var shopInfo = new ShopInfo<IItemTemplateTable>(shopOwner, name, canBuy, items);

            GameplayScreen.ShopForm.DisplayShop(shopInfo);
        }

        [MessageHandler((byte)ServerPacketID.StopShopping)]
        void RecvStopShopping(IIPSocket conn, BitStream r)
        {
            GameplayScreen.ShopForm.HideShop();
        }

        [MessageHandler((byte)ServerPacketID.UpdateEquipmentSlot)]
        void RecvUpdateEquipmentSlot(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadEnum<EquipmentSlot>();
            var hasValue = r.ReadBool();

            if (hasValue)
            {
                var graphic = r.ReadGrhIndex();
                UserInfo.Equipped.SetSlot(slot, graphic);
            }
            else
                UserInfo.Equipped.ClearSlot(slot);
        }

        [MessageHandler((byte)ServerPacketID.UpdateStat)]
        void RecvUpdateStat(IIPSocket conn, BitStream r)
        {
            var isBaseStat = r.ReadBool();
            var stat = r.ReadStat<StatType>();

            var coll = isBaseStat ? UserInfo.BaseStats : UserInfo.ModStats;
            coll[stat.StatType] = stat.Value;
        }

        [MessageHandler((byte)ServerPacketID.UpdateVelocityAndPosition)]
        void RecvUpdateVelocityAndPosition(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            DynamicEntity dynamicEntity = null;

            // Grab the DynamicEntity
            // The map can be null if the spatial updates come very early (which isn't uncommon)
            if (Map != null)
                dynamicEntity = Map.GetDynamicEntity<DynamicEntity>(mapEntityIndex);

            // Deserialize
            if (dynamicEntity != null)
            {
                // Read the value into the DynamicEntity
                dynamicEntity.DeserializePositionAndVelocity(r);
            }
            else
            {
                // DynamicEntity was null, so just flush the values from the reader
                DynamicEntity.FlushPositionAndVelocity(r);
            }
        }

        [MessageHandler((byte)ServerPacketID.UseEntity)]
        void RecvUseEntity(IIPSocket conn, BitStream r)
        {
            var usedEntityIndex = r.ReadMapEntityIndex();
            var usedByIndex = r.ReadMapEntityIndex();

            // Grab the used DynamicEntity
            var usedEntity = Map.GetDynamicEntity(usedEntityIndex);
            if (usedEntity == null)
            {
                const string errmsg = "UseEntity received but usedEntityIndex `{0}` is not a valid DynamicEntity.";
                Debug.Fail(string.Format(errmsg, usedEntityIndex));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, usedEntityIndex);
                return;
            }

            // Grab the one who used this DynamicEntity (we can still use it, we'll just pass null)
            var usedBy = Map.GetDynamicEntity(usedEntityIndex);
            if (usedBy == null)
            {
                const string errmsg = "UseEntity received but usedByIndex `{0}` is not a valid DynamicEntity.";
                Debug.Fail(string.Format(errmsg, usedEntityIndex));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, usedEntityIndex);
            }

            // Ensure the used DynamicEntity is even usable
            var asUsable = usedEntity as IUsableEntity;
            if (asUsable == null)
            {
                const string errmsg =
                    "UseEntity received but usedByIndex `{0}` refers to DynamicEntity `{1}` which does " +
                    "not implement IUsableEntity.";
                Debug.Fail(string.Format(errmsg, usedEntityIndex, usedEntity));
                if (log.IsErrorEnabled)
                    log.WarnFormat(errmsg, usedEntityIndex, usedEntity);
                return;
            }

            // Use it
            asUsable.Use(usedBy);
        }

        [MessageHandler((byte)ServerPacketID.UseSkill)]
        void RecvUseSkill(IIPSocket conn, BitStream r)
        {
            var userID = r.ReadMapEntityIndex();
            var hasTarget = r.ReadBool();
            MapEntityIndex? targetID = null;
            if (hasTarget)
                targetID = r.ReadMapEntityIndex();
            var skillType = r.ReadEnum<SkillType>();

            var user = Map.GetDynamicEntity<CharacterEntity>(userID);
            CharacterEntity target = null;
            if (targetID.HasValue)
                target = Map.GetDynamicEntity<CharacterEntity>(targetID.Value);

            if (user == null)
            {
                const string errmsg = "Read an invalid MapEntityIndex `{0}` in UseSkill for the skill user.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, userID);
                return;
            }

            if (target != null)
                GameplayScreen.AppendToChatOutput(string.Format("{0} casted {1} on {2}.", user.Name, skillType, target.Name));
            else
                GameplayScreen.AppendToChatOutput(string.Format("{0} casted {1}.", user.Name, skillType));

            // If the character that used the skill is our client's character, hide the skill cast progress bar
            if (user == User && skillType == GameplayScreen.SkillCastProgressBar.CurrentSkillType)
                GameplayScreen.SkillCastProgressBar.StopCasting();
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