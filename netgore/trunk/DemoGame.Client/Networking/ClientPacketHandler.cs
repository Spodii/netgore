using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Client.NPCChat;
using DemoGame.DbObjs;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Audio;
using NetGore.Features.Emoticons;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Graphics.GUI;
using NetGore.IO;
using NetGore.Network;
using NetGore.NPCChat;

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

        SoundManager SoundManager
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

        static List<StyledText> CreateChatText(string name, string message)
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

            using (PacketWriter pw = ClientPacket.Ping())
            {
                _socketSender.Send(pw);
            }

            _pingWatch.Start();
        }

        [MessageHandler((byte)ServerPacketID.AcceptOrTurnInQuestReply)]
        void RecvAcceptOrTurnInQuestReply(IIPSocket conn, BitStream r)
        {
            QuestID questID = r.ReadQuestID();
            bool successful = r.ReadBool();
            bool accepted = r.ReadBool();

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
            StatusEffectType statusEffectType = r.ReadEnum<StatusEffectType>();
            ushort power = r.ReadUShort();
            ushort secsLeft = r.ReadUShort();

            GameplayScreen.StatusEffectsForm.AddStatusEffect(statusEffectType, power, secsLeft);
            GameplayScreen.AppendToChatOutput(string.Format("Added status effect {0} with power {1}.", statusEffectType, power));
        }

        [MessageHandler((byte)ServerPacketID.CharAttack)]
        void RecvCharAttack(IIPSocket conn, BitStream r)
        {
            MapEntityIndex mapCharIndex = r.ReadMapEntityIndex();

            Character chr = Map.GetDynamicEntity<Character>(mapCharIndex);
            if (chr == null)
                return;

            SoundManager.TryPlay("punch");
            chr.Attack();
        }

        [MessageHandler((byte)ServerPacketID.CharDamage)]
        void RecvCharDamage(IIPSocket conn, BitStream r)
        {
            MapEntityIndex mapCharIndex = r.ReadMapEntityIndex();
            int damage = r.ReadInt();

            Character chr = Map.GetDynamicEntity<Character>(mapCharIndex);
            if (chr == null)
                return;

            GameplayScreen.DamageTextPool.Create(damage, chr, GetTime());
        }

        [MessageHandler((byte)ServerPacketID.Chat)]
        void RecvChat(IIPSocket conn, BitStream r)
        {
            string text = r.ReadString(GameData.MaxServerSayLength);
            GameplayScreen.AppendToChatOutput(text);
        }

        [MessageHandler((byte)ServerPacketID.ChatSay)]
        void RecvChatSay(IIPSocket conn, BitStream r)
        {
            string name = r.ReadString(GameData.MaxServerSayNameLength);
            MapEntityIndex mapEntityIndex = r.ReadMapEntityIndex();
            string text = r.ReadString(GameData.MaxServerSayLength);

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
            bool successful = r.ReadBool();
            string errorMessage = successful ? string.Empty : r.ReadString();

            if (ReceivedCreateAccount != null)
                ReceivedCreateAccount(conn, successful, errorMessage);
        }

        [MessageHandler((byte)ServerPacketID.CreateAccountCharacter)]
        void RecvCreateAccountCharacter(IIPSocket conn, BitStream r)
        {
            bool successful = r.ReadBool();
            string errorMessage = successful ? string.Empty : r.ReadString();

            if (ReceivedCreateAccountCharacter != null)
                ReceivedCreateAccountCharacter(conn, successful, errorMessage);
        }

        [MessageHandler((byte)ServerPacketID.CreateDynamicEntity)]
        void RecvCreateDynamicEntity(IIPSocket conn, BitStream r)
        {
            MapEntityIndex mapEntityIndex = r.ReadMapEntityIndex();
            DynamicEntity dynamicEntity = _dynamicEntityFactory.Read(r);
            Map.AddDynamicEntity(dynamicEntity, mapEntityIndex);

            Character character = dynamicEntity as Character;
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
            QuestID questID = r.ReadQuestID();
            bool hasRequirements = r.ReadBool();

            UserInfo.HasFinishQuestRequirements.SetRequirementsStatus(questID, hasRequirements);
        }

        [MessageHandler((byte)ServerPacketID.HasQuestStartRequirementsReply)]
        void RecvHasQuestStartRequirementsReply(IIPSocket conn, BitStream r)
        {
            QuestID questID = r.ReadQuestID();
            bool hasRequirements = r.ReadBool();

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
            string message = r.ReadGameMessage(_gameMessages);

            if (ReceivedLoginUnsuccessful != null)
                ReceivedLoginUnsuccessful(this, conn, message);
        }

        [MessageHandler((byte)ServerPacketID.NotifyExpCash)]
        void RecvNotifyExpCash(IIPSocket conn, BitStream r)
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

        [MessageHandler((byte)ServerPacketID.NotifyGetItem)]
        void RecvNotifyGetItem(IIPSocket conn, BitStream r)
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

        [MessageHandler((byte)ServerPacketID.NotifyLevel)]
        void RecvNotifyLevel(IIPSocket conn, BitStream r)
        {
            MapEntityIndex mapCharIndex = r.ReadMapEntityIndex();

            Character chr = Map.GetDynamicEntity<Character>(mapCharIndex);
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
            SoundID soundID = r.ReadSoundID();

            if (!SoundManager.TryPlay(soundID))
                LogFailPlaySound(soundID);
        }

        [MessageHandler((byte)ServerPacketID.PlaySoundAt)]
        void RecvPlaySoundAt(IIPSocket conn, BitStream r)
        {
            SoundID soundID = r.ReadSoundID();
            Vector2 position = r.ReadVector2();

            if (!SoundManager.TryPlay(soundID, position))
                LogFailPlaySound(soundID);
        }

        [MessageHandler((byte)ServerPacketID.PlaySoundAtEntity)]
        void RecvPlaySoundAtEntity(IIPSocket conn, BitStream r)
        {
            SoundID soundID = r.ReadSoundID();
            MapEntityIndex index = r.ReadMapEntityIndex();

            var entity = Map.GetDynamicEntity(index);
            if (entity == null)
            {
                const string errmsg = "Failed to find DynamicEntity with MapEntityIndex `{0}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, index);
                Debug.Fail(string.Format(errmsg, index));
                return;
            }

            if (!SoundManager.TryPlay(soundID, entity))
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
            MapEntityIndex mapEntityIndex = r.ReadMapEntityIndex();
            DynamicEntity dynamicEntity = Map.GetDynamicEntity(mapEntityIndex);

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
            StatusEffectType statusEffectType = r.ReadEnum<StatusEffectType>();

            GameplayScreen.StatusEffectsForm.RemoveStatusEffect(statusEffectType);
            GameplayScreen.AppendToChatOutput(string.Format("Removed status effect {0}.", statusEffectType));
        }

        [MessageHandler((byte)ServerPacketID.SendAccountCharacters)]
        void RecvSendAccountCharacters(IIPSocket conn, BitStream r)
        {
            byte count = r.ReadByte();
            var charInfos = new AccountCharacterInfo[count];
            for (int i = 0; i < count; i++)
            {
                AccountCharacterInfo charInfo = r.ReadAccountCharacterInfo();
                charInfos[charInfo.Index] = charInfo;
            }

            _accountCharacterInfos.SetInfos(charInfos);
        }

        [MessageHandler((byte)ServerPacketID.SendEquipmentItemInfo)]
        void RecvSendEquipmentItemInfo(IIPSocket conn, BitStream r)
        {
            EquipmentSlot slot = r.ReadEnum<EquipmentSlot>();
            GameplayScreen.EquipmentInfoRequester.ReceiveInfo(slot, r);
        }

        [MessageHandler((byte)ServerPacketID.SendInventoryItemInfo)]
        void RecvSendInventoryItemInfo(IIPSocket conn, BitStream r)
        {
            InventorySlot slot = r.ReadInventorySlot();
            GameplayScreen.InventoryInfoRequester.ReceiveInfo(slot, r);
        }

        [MessageHandler((byte)ServerPacketID.SendMessage)]
        void RecvSendMessage(IIPSocket conn, BitStream r)
        {
            string message = r.ReadGameMessage(_gameMessages);

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
            int cash = r.ReadInt();
            UserInfo.Cash = cash;
        }

        [MessageHandler((byte)ServerPacketID.SetCharacterHPPercent)]
        void RecvSetCharacterHPPercent(IIPSocket conn, BitStream r)
        {
            MapEntityIndex mapEntityIndex = r.ReadMapEntityIndex();
            byte percent = r.ReadByte();

            Character character = Map.GetDynamicEntity<Character>(mapEntityIndex);
            if (character == null)
                return;

            character.HPPercent = percent;
        }

        [MessageHandler((byte)ServerPacketID.SetCharacterMPPercent)]
        void RecvSetCharacterMPPercent(IIPSocket conn, BitStream r)
        {
            MapEntityIndex mapEntityIndex = r.ReadMapEntityIndex();
            byte percent = r.ReadByte();

            Character character = Map.GetDynamicEntity<Character>(mapEntityIndex);
            if (character == null)
                return;

            character.MPPercent = percent;
        }

        [MessageHandler((byte)ServerPacketID.SetCharacterPaperDoll)]
        void RecvSetCharacterPaperDoll(IIPSocket conn, BitStream r)
        {
            MapEntityIndex mapEntityIndex = r.ReadMapEntityIndex();
            byte count = r.ReadByte();

            string[] layers = new string[count];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = r.ReadString();
            }

            Character character = Map.GetDynamicEntity<Character>(mapEntityIndex);
            if (character == null)
                return;

            character.CharacterSprite.SetPaperDollLayers(layers);
        }

        [MessageHandler((byte)ServerPacketID.SetChatDialogPage)]
        void RecvSetChatDialogPage(IIPSocket conn, BitStream r)
        {
            ushort pageIndex = r.ReadUShort();
            byte skipCount = r.ReadByte();

            var responsesToSkip = new byte[skipCount];
            for (int i = 0; i < skipCount; i++)
            {
                responsesToSkip[i] = r.ReadByte();
            }

            GameplayScreen.ChatDialogForm.SetPageIndex(pageIndex, responsesToSkip);
        }

        [MessageHandler((byte)ServerPacketID.SetExp)]
        void RecvSetExp(IIPSocket conn, BitStream r)
        {
            int exp = r.ReadInt();
            UserInfo.Exp = exp;
        }

        [MessageHandler((byte)ServerPacketID.SetHP)]
        void RecvSetHP(IIPSocket conn, BitStream r)
        {
            SPValueType value = r.ReadSPValueType();
            UserInfo.HP = value;

            if (User == null)
                return;

            User.HPPercent = UserInfo.HPPercent;
        }

        [MessageHandler((byte)ServerPacketID.SetInventorySlot)]
        void RecvSetInventorySlot(IIPSocket conn, BitStream r)
        {
            InventorySlot slot = r.ReadInventorySlot();
            bool hasGraphic = r.ReadBool();
            GrhIndex graphic = hasGraphic ? r.ReadGrhIndex() : GrhIndex.Invalid;
            byte amount = r.ReadByte();

            UserInfo.Inventory.Update(slot, graphic, amount, GetTime());
        }

        [MessageHandler((byte)ServerPacketID.SetLevel)]
        void RecvSetLevel(IIPSocket conn, BitStream r)
        {
            byte level = r.ReadByte();
            UserInfo.Level = level;
        }

        [MessageHandler((byte)ServerPacketID.SetMap)]
        void RecvSetMap(IIPSocket conn, BitStream r)
        {
            MapIndex mapIndex = r.ReadMapIndex();

            // Create the new map
            Map newMap = new Map(mapIndex, World.Camera, World, GameplayScreen.ScreenManager.GraphicsDevice);
            newMap.Load(ContentPaths.Build, false, _dynamicEntityFactory);

            // Clear quest requirements caches
            UserInfo.HasStartQuestRequirements.Clear();

            // Change maps
            World.Map = newMap;

            // Unload all map content from the previous map and from the new map loading
            GameplayScreen.ScreenManager.MapContent.Unload();

            // Change the screens, if needed
            GameplayScreen.ScreenManager.SetScreen(GameplayScreen.ScreenName);
        }

        [MessageHandler((byte)ServerPacketID.SetMP)]
        void RecvSetMP(IIPSocket conn, BitStream r)
        {
            SPValueType value = r.ReadSPValueType();
            UserInfo.MP = value;

            if (User == null)
                return;

            User.MPPercent = UserInfo.MPPercent;
        }

        [MessageHandler((byte)ServerPacketID.SetProvidedQuests)]
        void RecvSetProvidedQuests(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            byte count = r.ReadByte();
            QuestID[] questIDs = new QuestID[count];
            for (int i = 0; i < count; i++)
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
            byte skillGroup = r.ReadByte();
            ushort cooldownTime = r.ReadUShort();

            GameplayScreen.SkillCooldownManager.SetCooldown(skillGroup, cooldownTime, GetTime());
        }

        [MessageHandler((byte)ServerPacketID.SetStatPoints)]
        void RecvSetStatPoints(IIPSocket conn, BitStream r)
        {
            int statPoints = r.ReadInt();
            UserInfo.StatPoints = statPoints;
        }

        [MessageHandler((byte)ServerPacketID.SetUserChar)]
        void RecvSetUserChar(IIPSocket conn, BitStream r)
        {
            MapEntityIndex mapCharIndex = r.ReadMapEntityIndex();
            World.UserCharIndex = mapCharIndex;
        }

        [MessageHandler((byte)ServerPacketID.StartCastingSkill)]
        void RecvStartCastingSkill(IIPSocket conn, BitStream r)
        {
            var skillType = r.ReadEnum<SkillType>();
            ushort castTime = r.ReadUShort();

            GameplayScreen.SkillCastProgressBar.StartCasting(skillType, castTime);
        }

        [MessageHandler((byte)ServerPacketID.StartChatDialog)]
        void RecvStartChatDialog(IIPSocket conn, BitStream r)
        {
            MapEntityIndex npcIndex = r.ReadMapEntityIndex();
            ushort dialogIndex = r.ReadUShort();

            NPCChatDialogBase dialog = NPCChatManager.GetDialog(dialogIndex);
            GameplayScreen.ChatDialogForm.StartDialog(dialog);
        }

        [MessageHandler((byte)ServerPacketID.StartQuestChatDialog)]
        void RecvStartQuestChatDialog(IIPSocket conn, BitStream r)
        {
            MapEntityIndex npcIndex = r.ReadMapEntityIndex();

            // Available quests
            byte numAvailableQuests = r.ReadByte();
            QuestID[] availableQuests = new QuestID[numAvailableQuests];
            for (int i = 0; i < availableQuests.Length; i++)
            {
                availableQuests[i] = r.ReadQuestID();
            }

            // Quests that can be turned in
            byte numTurnInQuests = r.ReadByte();
            QuestID[] turnInQuests = new QuestID[numTurnInQuests];
            for (int i = 0; i < turnInQuests.Length; i++)
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
            var questDescriptions =
                availableQuests.Concat(turnInQuests).Distinct().Select(x => World.QuestDescriptions.GetOrDefault(x));

            // Display the form
            GameplayScreen.AvailableQuestsForm.Display(questDescriptions, npcIndex);
        }

        [MessageHandler((byte)ServerPacketID.StartShopping)]
        void RecvStartShopping(IIPSocket conn, BitStream r)
        {
            MapEntityIndex shopOwnerIndex = r.ReadMapEntityIndex();
            bool canBuy = r.ReadBool();
            string name = r.ReadString();
            byte itemCount = r.ReadByte();

            var items = new IItemTemplateTable[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                var value = new ItemTemplateTable();
                value.ReadState(r);
                items[i] = value;
            }

            DynamicEntity shopOwner = Map.GetDynamicEntity(shopOwnerIndex);
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
            EquipmentSlot slot = r.ReadEnum<EquipmentSlot>();
            bool hasValue = r.ReadBool();

            if (hasValue)
            {
                GrhIndex graphic = r.ReadGrhIndex();
                UserInfo.Equipped.SetSlot(slot, graphic);
            }
            else
                UserInfo.Equipped.ClearSlot(slot);
        }

        [MessageHandler((byte)ServerPacketID.UpdateStat)]
        void RecvUpdateStat(IIPSocket conn, BitStream r)
        {
            bool isBaseStat = r.ReadBool();
            CharacterStats statCollectionToUpdate = isBaseStat ? UserInfo.BaseStats : UserInfo.ModStats;
            r.ReadStat(statCollectionToUpdate);
        }

        [MessageHandler((byte)ServerPacketID.UpdateVelocityAndPosition)]
        void RecvUpdateVelocityAndPosition(IIPSocket conn, BitStream r)
        {
            MapEntityIndex mapEntityIndex = r.ReadMapEntityIndex();
            DynamicEntity dynamicEntity;

            // Grab the DynamicEntity
            try
            {
                dynamicEntity = Map.GetDynamicEntity<DynamicEntity>(mapEntityIndex);
            }
            catch (Exception)
            {
                // Ignore errors about finding the DynamicEntity
                dynamicEntity = null;
            }

            // Deserialize
            if (dynamicEntity != null)
            {
                // Read the value into the DynamicEntity
                dynamicEntity.DeserializePositionAndVelocity(r);
            }
            else
            {
                // Just flush the values from the reader
                DynamicEntity.FlushPositionAndVelocity(r);
            }
        }

        [MessageHandler((byte)ServerPacketID.UseEntity)]
        void RecvUseEntity(IIPSocket conn, BitStream r)
        {
            MapEntityIndex usedEntityIndex = r.ReadMapEntityIndex();
            MapEntityIndex usedByIndex = r.ReadMapEntityIndex();

            // Grab the used DynamicEntity
            DynamicEntity usedEntity = Map.GetDynamicEntity(usedEntityIndex);
            if (usedEntity == null)
            {
                const string errmsg = "UseEntity received but usedEntityIndex `{0}` is not a valid DynamicEntity.";
                Debug.Fail(string.Format(errmsg, usedEntityIndex));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, usedEntityIndex);
                return;
            }

            // Grab the one who used this DynamicEntity (we can still use it, we'll just pass null)
            DynamicEntity usedBy = Map.GetDynamicEntity(usedEntityIndex);
            if (usedBy == null)
            {
                const string errmsg = "UseEntity received but usedByIndex `{0}` is not a valid DynamicEntity.";
                Debug.Fail(string.Format(errmsg, usedEntityIndex));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, usedEntityIndex);
            }

            // Ensure the used DynamicEntity is even usable
            IUsableEntity asUsable = usedEntity as IUsableEntity;
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
            MapEntityIndex userID = r.ReadMapEntityIndex();
            bool hasTarget = r.ReadBool();
            MapEntityIndex? targetID = null;
            if (hasTarget)
                targetID = r.ReadMapEntityIndex();
            SkillType skillType = r.ReadEnum<SkillType>();

            CharacterEntity user = Map.GetDynamicEntity<CharacterEntity>(userID);
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

            // NOTE: Temporary output
            if (target != null)
                GameplayScreen.AppendToChatOutput(string.Format("{0} casted {1} on {2}.", user.Name, skillType, target.Name));
            else
                GameplayScreen.AppendToChatOutput(string.Format("{0} casted {1}.", user.Name, skillType));

            // TODO: Add skill effects

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