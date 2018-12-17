using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using log4net;
using NetGore;
using NetGore.Audio;
using NetGore.Content;
using NetGore.Features.ActionDisplays;
using NetGore.Features.GameTime;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;
using NetGore.Features.NPCChat;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Graphics.GUI;
using NetGore.IO;
using NetGore.Network;
using NetGore.Stats;
using NetGore.World;
using SFML.Graphics;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace DemoGame.Client
{
    /// <summary>
    /// Holds all the methods used to process received packets.
    /// </summary>
    public partial class ClientPacketHandler : IGetTime
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly IQuestDescriptionCollection _questDescriptions = QuestDescriptionCollection.Create(ContentPaths.Build);

        readonly AccountCharacterInfos _accountCharacterInfos = new AccountCharacterInfos();
        readonly IDynamicEntityFactory _dynamicEntityFactory;
        readonly INetworkSender _networkSender;
        readonly ObjGrabber _objGrabber;
        readonly ClientPeerTradeInfoHandler _peerTradeInfoHandler;
        readonly IScreenManager _screenManager;

        GameplayScreen _gameplayScreenCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPacketHandler"/> class.
        /// </summary>
        /// <param name="networkSender">The socket sender.</param>
        /// <param name="screenManager">The <see cref="IScreenManager"/>.</param>
        /// <param name="dynamicEntityFactory">The <see cref="IDynamicEntityFactory"/> used to serialize
        /// <see cref="DynamicEntity"/>s.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dynamicEntityFactory" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="screenManager" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="networkSender" /> is <c>null</c>.</exception>
        public ClientPacketHandler(INetworkSender networkSender, IScreenManager screenManager,
                                   IDynamicEntityFactory dynamicEntityFactory)
        {
            if (dynamicEntityFactory == null)
                throw new ArgumentNullException("dynamicEntityFactory");
            if (screenManager == null)
                throw new ArgumentNullException("screenManager");
            if (networkSender == null)
                throw new ArgumentNullException("networkSender");

            _networkSender = networkSender;
            _dynamicEntityFactory = dynamicEntityFactory;
            _screenManager = screenManager;

            _peerTradeInfoHandler = new ClientPeerTradeInfoHandler(networkSender);
            _peerTradeInfoHandler.GameMessageCallback += PeerTradeInfoHandler_GameMessageCallback;

            _objGrabber = new ObjGrabber(this);
        }

        /// <summary>
        /// Notifies listeners when a message has been received about creating an account.
        /// </summary>
        public event TypedEventHandler<IIPSocket, CreateAccountEventArgs> ReceivedCreateAccount;

        /// <summary>
        /// Notifies listeners when a message has been received about creating an account character.
        /// </summary>
        public event TypedEventHandler<IIPSocket, CreateAccountEventArgs> ReceivedCreateAccountCharacter;

        /// <summary>
        /// Notifies listeners when a successful login request has been made.
        /// </summary>
        public event TypedEventHandler<ClientPacketHandler, ClientPacketHandlerEventArgs> ReceivedLoginSuccessful;

        /// <summary>
        /// Notifies listeners when an unsuccessful login request has been made.
        /// </summary>
        public event TypedEventHandler<ClientPacketHandler, ClientPacketHandlerEventArgs<string>> ReceivedLoginUnsuccessful;

        public AccountCharacterInfos AccountCharacterInfos
        {
            get { return _accountCharacterInfos; }
        }

        /// <summary>
        /// Gets the <see cref="GameplayScreen"/>.
        /// </summary>
        public GameplayScreen GameplayScreen
        {
            get
            {
                if (_gameplayScreenCache == null)
                    _gameplayScreenCache = _screenManager.GetScreen<GameplayScreen>();

                return _gameplayScreenCache;
            }
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

        public INetworkSender NetworkSender
        {
            get { return _networkSender; }
        }

        /// <summary>
        /// Gets the <see cref="ClientPeerTradeInfoHandler"/> instance.
        /// </summary>
        public ClientPeerTradeInfoHandler PeerTradeInfoHandler
        {
            get { return _peerTradeInfoHandler; }
        }

        public UserQuestInformation QuestInfo
        {
            get { return GameplayScreen.UserInfo.QuestInfo; }
        }

        /// <summary>
        /// Gets the <see cref="IScreenManager"/>.
        /// </summary>
        public IScreenManager ScreenManager
        {
            get { return _screenManager; }
        }

        public ISoundManager SoundManager
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
            var left = new StyledText(name + ": ", Color.Red);
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

        /// <summary>
        /// Handles the <see cref="ClientPeerTradeInfoHandler.GameMessageCallback"/> event from the <see cref="ClientPeerTradeInfoHandler"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ClientPeerTradeInfoHandlerEventArgs"/> instance containing the event data.</param>
        void PeerTradeInfoHandler_GameMessageCallback(ClientPeerTradeInfoHandler sender, ClientPeerTradeInfoHandlerEventArgs e)
        {
            // Parse the GameMessage
            var msg = GameMessageCollection.CurrentLanguage.GetMessage(e.GameMessage, e.Args);

            // Display
            if (!string.IsNullOrEmpty(msg))
                GameplayScreen.AppendToChatOutput(msg);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "accepted")]
        [MessageHandler((uint)ServerPacketID.AcceptOrTurnInQuestReply)]
        void RecvAcceptOrTurnInQuestReply(IIPSocket conn, BitStream r)
        {
            var questID = r.ReadQuestID();
            var successful = r.ReadBool();
#pragma warning disable 168
            var accepted = r.ReadBool();
#pragma warning restore 168

            if (successful)
            {
                // Remove the quest from the available quests list
                var aqf = GameplayScreen.AvailableQuestsForm;
                if (aqf.IsVisible)
                {
                    aqf.AvailableQuests = aqf.AvailableQuests.Where(x => x.QuestID != questID).ToArray();
                }
            }
        }

        [MessageHandler((uint)ServerPacketID.AddStatusEffect)]
        void RecvAddStatusEffect(IIPSocket conn, BitStream r)
        {
            var statusEffectType = r.ReadEnum<StatusEffectType>();
            var power = r.ReadUShort();
            var secsLeft = r.ReadUShort();

            var param = new string[] {User.Name, statusEffectType.ToString(), power.ToString()};
            var message = GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.CombatStatusEffectGained, param);

            GameplayScreen.StatusEffectsForm.AddStatusEffect(statusEffectType, power, secsLeft);
            GameplayScreen.AppendToChatOutput(message);
        }

        [MessageHandler((uint)ServerPacketID.CharAttack)]
        void RecvCharAttack(IIPSocket conn, BitStream r)
        {
            // Read the values
            var attackerID = r.ReadMapEntityIndex();

            MapEntityIndex? attackedID;
            if (r.ReadBool())
                attackedID = r.ReadMapEntityIndex();
            else
                attackedID = null;

            ActionDisplayID? actionDisplayIDNullable;
            if (r.ReadBool())
                actionDisplayIDNullable = r.ReadActionDisplayID();
            else
                actionDisplayIDNullable = null;

            // Get the object references using the IDs provided
            var attacker = _objGrabber.GetDynamicEntity<Character>(attackerID);
            if (attacker == null)
                return;

            DynamicEntity attacked = attackedID.HasValue ? Map.GetDynamicEntity(attackedID.Value) : null;

            // Use the default ActionDisplayID if we were provided with a null value
            ActionDisplayID actionDisplayID = !actionDisplayIDNullable.HasValue ? GameData.DefaultActionDisplayID : actionDisplayIDNullable.Value;

            // Get the ActionDisplay to use and, if valid, execute it
            var actionDisplay = ActionDisplayScripts.ActionDisplays[actionDisplayID];
            if (actionDisplay != null)
                actionDisplay.Execute(Map, attacker, attacked);
        }

        [MessageHandler((uint)ServerPacketID.CharDamage)]
        void RecvCharDamage(IIPSocket conn, BitStream r)
        {
            var mapCharIndex = r.ReadMapEntityIndex();
            var damage = r.ReadInt();

            var chr = _objGrabber.GetDynamicEntity<Character>(mapCharIndex);
            if (chr == null)
                return;

            GameplayScreen.DamageTextPool.Create(damage, chr, GetTime());
        }

        [MessageHandler((uint)ServerPacketID.Chat)]
        void RecvChat(IIPSocket conn, BitStream r)
        {
            var text = r.ReadString(GameData.MaxServerSayLength);
            GameplayScreen.AppendToChatOutput(text);
        }

        [MessageHandler((uint)ServerPacketID.ChatSay)]
        void RecvChatSay(IIPSocket conn, BitStream r)
        {
            var name = r.ReadString(GameData.MaxServerSayNameLength);
            var mapEntityIndex = r.ReadMapEntityIndex();
            var text = r.ReadString(GameData.MaxServerSayLength);

            var chatText = CreateChatText(name, text);
            GameplayScreen.AppendToChatOutput(chatText);

            var entity = Map.GetDynamicEntity(mapEntityIndex);
            if (entity == null)
                return;

            GameplayScreen.AddChatBubble(entity, text);
        }

        [MessageHandler((uint)ServerPacketID.CreateAccount)]
        void RecvCreateAccount(IIPSocket conn, BitStream r)
        {
            var successful = r.ReadBool();
            var errorMessage = string.Empty;

            if (!successful)
            {
                var failureGameMessage = r.ReadEnum<GameMessage>();
                errorMessage = GameMessageCollection.CurrentLanguage.GetMessage(failureGameMessage);
            }

            if (ReceivedCreateAccount != null)
                ReceivedCreateAccount.Raise(conn, new CreateAccountEventArgs(successful, errorMessage));
        }

        [MessageHandler((uint)ServerPacketID.CreateAccountCharacter)]
        void RecvCreateAccountCharacter(IIPSocket conn, BitStream r)
        {
            var successful = r.ReadBool();
            var errorMessage = successful ? string.Empty : r.ReadString();

            if (ReceivedCreateAccountCharacter != null)
                ReceivedCreateAccountCharacter.Raise(conn, new CreateAccountEventArgs(successful, errorMessage));
        }

        [MessageHandler((uint)ServerPacketID.CreateDynamicEntity)]
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

        [MessageHandler((uint)ServerPacketID.SynchronizeDynamicEntity)]
        void RecvSynchronizeDynamicEntity(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            DynamicEntity e = Map.GetDynamicEntity(mapEntityIndex);
            e.Deserialize(r);
        }

        [MessageHandler((uint)ServerPacketID.Emote)]
        void RecvEmote(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var emoticon = r.ReadEnum<Emoticon>();

            var entity = Map.GetDynamicEntity(mapEntityIndex);
            if (entity == null)
                return;

            EmoticonDisplayManager.Instance.Add(entity, emoticon, GetTime());
        }

        [MessageHandler((uint)ServerPacketID.EndChatDialog)]
        void RecvEndChatDialog(IIPSocket conn, BitStream r)
        {
            GameplayScreen.ChatDialogForm.EndDialog();
        }

        [MessageHandler((uint)ServerPacketID.GroupInfo)]
        void RecvGroupInfo(IIPSocket conn, BitStream r)
        {
            GroupInfo.Read(r);
        }

        [MessageHandler((uint)ServerPacketID.GuildInfo)]
        void RecvGuildInfo(IIPSocket conn, BitStream r)
        {
            GuildInfo.Read(r);
        }

        [MessageHandler((uint)ServerPacketID.HasQuestFinishRequirementsReply)]
        void RecvHasQuestFinishRequirementsReply(IIPSocket conn, BitStream r)
        {
            var questID = r.ReadQuestID();
            var hasRequirements = r.ReadBool();

            UserInfo.HasFinishQuestRequirements.SetRequirementsStatus(questID, hasRequirements);
        }

        [MessageHandler((uint)ServerPacketID.HasQuestStartRequirementsReply)]
        void RecvHasQuestStartRequirementsReply(IIPSocket conn, BitStream r)
        {
            var questID = r.ReadQuestID();
            var hasRequirements = r.ReadBool();

            UserInfo.HasStartQuestRequirements.SetRequirementsStatus(questID, hasRequirements);
        }

        [MessageHandler((uint)ServerPacketID.LoginSuccessful)]
        void RecvLoginSuccessful(IIPSocket conn, BitStream r)
        {
            if (ReceivedLoginSuccessful != null)
                ReceivedLoginSuccessful.Raise(this, new ClientPacketHandlerEventArgs(conn));
        }

        [MessageHandler((uint)ServerPacketID.LoginUnsuccessful)]
        void RecvLoginUnsuccessful(IIPSocket conn, BitStream r)
        {
            var message = r.ReadGameMessage(GameMessageCollection.CurrentLanguage);

            if (ReceivedLoginUnsuccessful != null)
                ReceivedLoginUnsuccessful.Raise(this, new ClientPacketHandlerEventArgs<string>(conn, message));
        }

        [MessageHandler((uint)ServerPacketID.NotifyExpCash)]
        void RecvNotifyExpCash(IIPSocket conn, BitStream r)
        {
            var exp = r.ReadInt();
            var cash = r.ReadInt();

            var param = new string[] {exp.ToString(),cash.ToString()};
            var message = GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.CombatRecieveReward, param);

            GameplayScreen.InfoBox.Add(message);
        }

        [MessageHandler((uint)ServerPacketID.NotifyGetItem)]
        void RecvNotifyGetItem(IIPSocket conn, BitStream r)
        {
            var name = r.ReadString();
            var amount = r.ReadByte();

            string msg;
            if (amount > 1)
                msg = string.Format("You got {0} {1}s", amount, name);
            else
                msg = string.Format("You got a {0}", name);

            GameplayScreen.InfoBox.Add(msg);
        }

        [MessageHandler((uint)ServerPacketID.NotifyLevel)]
        void RecvNotifyLevel(IIPSocket conn, BitStream r)
        {
            var mapCharIndex = r.ReadMapEntityIndex();

            var chr = _objGrabber.GetDynamicEntity<Character>(mapCharIndex);
            if (chr == null)
                return;

            var message = GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.CombatSelfLevelUp, UserInfo.Level.ToString());

            if (chr == World.UserChar)
                GameplayScreen.InfoBox.Add(message);
        }

        [MessageHandler((uint)ServerPacketID.PeerTradeEvent)]
        void RecvPeerTradeEvent(IIPSocket conn, BitStream r)
        {
            PeerTradeInfoHandler.Read(r);
        }

        [MessageHandler((uint)ServerPacketID.PlaySound)]
        void RecvPlaySound(IIPSocket conn, BitStream r)
        {
            var soundID = r.ReadSoundID();

            if (!SoundManager.Play(soundID))
                LogFailPlaySound(soundID);
        }

        [MessageHandler((uint)ServerPacketID.PlaySoundAt)]
        void RecvPlaySoundAt(IIPSocket conn, BitStream r)
        {
            var soundID = r.ReadSoundID();
            var position = r.ReadVector2();

            if (!SoundManager.Play(soundID, position))
                LogFailPlaySound(soundID);
        }

        [MessageHandler((uint)ServerPacketID.PlaySoundAtEntity)]
        void RecvPlaySoundAtEntity(IIPSocket conn, BitStream r)
        {
            var soundID = r.ReadSoundID();
            var index = r.ReadMapEntityIndex();

            var entity = Map.GetDynamicEntity(index);
            if (entity == null)
                return;

            if (!SoundManager.Play(soundID, entity))
                LogFailPlaySound(soundID);
        }

        [MessageHandler((uint)ServerPacketID.QuestInfo)]
        void RecvQuestInfo(IIPSocket conn, BitStream r)
        {
            QuestInfo.Read(r);
        }

        [MessageHandler((uint)ServerPacketID.RemoveDynamicEntity)]
        void RecvRemoveDynamicEntity(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var dynamicEntity = Map.GetDynamicEntity(mapEntityIndex);

            if (dynamicEntity == null)
                return;

            Map.RemoveEntity(dynamicEntity);

            if (log.IsInfoEnabled)
                log.InfoFormat("Removed DynamicEntity with index `{0}`", mapEntityIndex);

            dynamicEntity.Dispose();
        }

        [MessageHandler((uint)ServerPacketID.RemoveStatusEffect)]
        void RecvRemoveStatusEffect(IIPSocket conn, BitStream r)
        {
            var statusEffectType = r.ReadEnum<StatusEffectType>();

            var message = GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.CombatStatusEffectWoreOff, statusEffectType.ToString());

            GameplayScreen.StatusEffectsForm.RemoveStatusEffect(statusEffectType);
            GameplayScreen.AppendToChatOutput(message);
        }

        [MessageHandler((uint)ServerPacketID.SendAccountCharacters)]
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

        [MessageHandler((uint)ServerPacketID.SendEquipmentItemInfo)]
        void RecvSendEquipmentItemInfo(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadEnum<EquipmentSlot>();
            GameplayScreen.EquipmentInfoRequester.ReceiveInfo(slot, r);
        }

        [MessageHandler((uint)ServerPacketID.SendInventoryItemInfo)]
        void RecvSendInventoryItemInfo(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();
            GameplayScreen.InventoryInfoRequester.ReceiveInfo(slot, r);
        }

        [MessageHandler((uint)ServerPacketID.SendMessage)]
        void RecvSendMessage(IIPSocket conn, BitStream r)
        {
            var message = r.ReadGameMessage(GameMessageCollection.CurrentLanguage);

            if (string.IsNullOrEmpty(message))
            {
                const string errmsg = "Received empty or null GameMessage.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return;
            }

            GameplayScreen.AppendToChatOutput(message, Color.Black);
        }

        [MessageHandler((uint)ServerPacketID.SetCash)]
        void RecvSetCash(IIPSocket conn, BitStream r)
        {
            var cash = r.ReadInt();
            UserInfo.Cash = cash;
        }

        [MessageHandler((uint)ServerPacketID.SetCharacterHPPercent)]
        void RecvSetCharacterHPPercent(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var percent = r.ReadByte();

            var character = _objGrabber.GetDynamicEntity<Character>(mapEntityIndex);
            if (character == null)
                return;

            character.HPPercent = percent;
        }

        [MessageHandler((uint)ServerPacketID.SetCharacterMPPercent)]
        void RecvSetCharacterMPPercent(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var percent = r.ReadByte();

            var character = _objGrabber.GetDynamicEntity<Character>(mapEntityIndex);
            if (character == null)
                return;

            character.MPPercent = percent;
        }

        [MessageHandler((uint)ServerPacketID.SetCharacterPaperDoll)]
        void RecvSetCharacterPaperDoll(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var count = r.ReadByte();

            var layers = new string[count];
            for (var i = 0; i < layers.Length; i++)
            {
                layers[i] = r.ReadString();
            }

            var character = _objGrabber.GetDynamicEntity<Character>(mapEntityIndex);
            if (character == null)
                return;

            character.CharacterSprite.SetPaperDollLayers(layers);
        }

        [MessageHandler((uint)ServerPacketID.SetChatDialogPage)]
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

        [MessageHandler((uint)ServerPacketID.SetClickWarpMode)]
        void RecvSetClickWarpMode(IIPSocket conn, BitStream r)
        {
            bool enabled = r.ReadBool();
            GameplayScreen.AppendToChatOutput("Click warp mode: " + (enabled ? "Enabled" : "Disabled"), Color.Green);
            GameplayScreen.ClickWarpMode = enabled;
        }

        [MessageHandler((uint)ServerPacketID.SetExp)]
        void RecvSetExp(IIPSocket conn, BitStream r)
        {
            var exp = r.ReadInt();
            UserInfo.Exp = exp;
        }

        [MessageHandler((uint)ServerPacketID.SetGameTime)]
        void RecvSetGameTime(IIPSocket conn, BitStream r)
        {
            var serverTimeBinary = r.ReadLong();
            var serverTime = DateTime.FromBinary(serverTimeBinary);

            GameDateTime.SetServerTimeOffset(serverTime);
        }

        [MessageHandler((uint)ServerPacketID.SetHP)]
        void RecvSetHP(IIPSocket conn, BitStream r)
        {
            var value = r.ReadSPValueType();
            UserInfo.HP = value;

            if (User == null)
                return;

            User.HPPercent = UserInfo.HPPercent;
        }

        [MessageHandler((uint)ServerPacketID.SetInventorySlot)]
        void RecvSetInventorySlot(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();
            var hasGraphic = r.ReadBool();
            var graphic = hasGraphic ? r.ReadGrhIndex() : GrhIndex.Invalid;
            var amount = r.ReadByte();

            UserInfo.Inventory.Update(slot, graphic, amount, GetTime());
        }

        [MessageHandler((uint)ServerPacketID.SetLevel)]
        void RecvSetLevel(IIPSocket conn, BitStream r)
        {
            var level = r.ReadShort();
            UserInfo.Level = level;
        }

        [MessageHandler((uint)ServerPacketID.SetMP)]
        void RecvSetMP(IIPSocket conn, BitStream r)
        {
            var value = r.ReadSPValueType();
            UserInfo.MP = value;

            if (User == null)
                return;

            User.MPPercent = UserInfo.MPPercent;
        }

        [MessageHandler((uint)ServerPacketID.SetMap)]
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
            GameplayScreen.ScreenManager.SetScreen<GameplayScreen>();
        }

        [MessageHandler((uint)ServerPacketID.SetProvidedQuests)]
        void RecvSetProvidedQuests(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            var count = r.ReadByte();
            var questIDs = new QuestID[count];
            for (var i = 0; i < count; i++)
            {
                questIDs[i] = r.ReadQuestID();
            }

            var character = _objGrabber.GetDynamicEntity<Character>(mapEntityIndex);
            if (character != null)
                character.ProvidedQuests = questIDs;
        }

        [MessageHandler((uint)ServerPacketID.SetStatPoints)]
        void RecvSetStatPoints(IIPSocket conn, BitStream r)
        {
            var statPoints = r.ReadInt();
            UserInfo.StatPoints = statPoints;
        }

        [MessageHandler((uint)ServerPacketID.SetUserChar)]
        void RecvSetUserChar(IIPSocket conn, BitStream r)
        {
            var mapCharIndex = r.ReadMapEntityIndex();
            World.UserCharIndex = mapCharIndex;
        }

        [MessageHandler((uint)ServerPacketID.SkillSetGroupCooldown)]
        void RecvSkillSetGroupCooldown(IIPSocket conn, BitStream r)
        {
            var skillGroup = r.ReadByte();
            var cooldownTime = r.ReadUShort();

            GameplayScreen.SkillCooldownManager.SetCooldown(skillGroup, cooldownTime, GetTime());
        }

        [MessageHandler((uint)ServerPacketID.SkillSetKnown)]
        void RecvSkillSetKnown(IIPSocket conn, BitStream r)
        {
            var skillType = r.ReadEnum<SkillType>();
            var isKnown = r.ReadBool();

            if (!EnumHelper<SkillType>.IsDefined(skillType))
            {
                const string errmsg = "Invalid SkillType received: `{0}`";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, skillType);
                Debug.Fail(string.Format(errmsg, skillType));
                return;
            }

            // Set the skill's known state
            UserInfo.KnownSkills.SetSkill(skillType, isKnown);
        }

        [MessageHandler((uint)ServerPacketID.SkillSetKnownAll)]
        void RecvSkillSetKnownAll(IIPSocket conn, BitStream r)
        {
            var count = r.ReadByte();
            var knownSkills = new List<SkillType>(count);

            // Read the known skills list
            for (var i = 0; i < count; i++)
            {
                var value = r.ReadEnum<SkillType>();
                knownSkills.Add(value);
            }

            Debug.Assert(knownSkills.Count == count);
            Debug.Assert(knownSkills.All(EnumHelper<SkillType>.IsDefined), "One or more known skills were unknown...");

            // Set the known skills
            UserInfo.KnownSkills.SetValues(knownSkills);
        }

        [MessageHandler((uint)ServerPacketID.CreateActionDisplayAtEntity)]
        void RecvCreateActionDisplayAtEntity(IIPSocket conn, BitStream r)
        {
            ActionDisplayID actionDisplayId = r.ReadActionDisplayID();
            MapEntityIndex sourceEntityIndex = r.ReadMapEntityIndex();
            bool hasTarget = r.ReadBool();
            MapEntityIndex? targetEntityIndex = hasTarget ? r.ReadMapEntityIndex() : (MapEntityIndex?)null;

            // Get the entities
            var sourceEntity = _objGrabber.GetDynamicEntity<Character>(sourceEntityIndex);
            if (sourceEntity == null)
                return;

            var targetEntity = targetEntityIndex.HasValue ? _objGrabber.GetDynamicEntity<Character>(targetEntityIndex.Value) : null;

            // Get the action display
            var ad = ActionDisplayScripts.ActionDisplays[actionDisplayId];
            if (ad == null)
                return;

            // Create
            ad.Execute(Map, sourceEntity, targetEntity);
        }

        [MessageHandler((uint)ServerPacketID.SkillStartCasting_ToMap)]
        void RecvSkillStartCasting_ToMap(IIPSocket conn, BitStream r)
        {
            var casterEntityIndex = r.ReadMapEntityIndex();
            var skillType = r.ReadEnum<SkillType>();

            // Get the SkillInfo for the skill being used
            var skillInfo = _objGrabber.GetSkillInfo(skillType);
            if (skillInfo == null)
                return;

            // Get the entity
            var casterEntity = _objGrabber.GetDynamicEntity<Character>(casterEntityIndex);
            if (casterEntity == null)
                return;

            // If an ActionDisplay is available for this skill, display it
            if (skillInfo.StartCastingActionDisplay.HasValue)
            {
                var ad = ActionDisplayScripts.ActionDisplays[skillInfo.StartCastingActionDisplay.Value];
                if (ad != null)
                {
                    casterEntity.IsCastingSkill = true;
                    ad.Execute(Map, casterEntity, null);
                }
            }
        }

        [MessageHandler((uint)ServerPacketID.SkillStartCasting_ToUser)]
        void RecvSkillStartCasting_ToUser(IIPSocket conn, BitStream r)
        {
            var skillType = r.ReadEnum<SkillType>();
            var castTime = r.ReadUShort();

            GameplayScreen.SkillCastProgressBar.StartCasting(skillType, castTime);
        }

        [MessageHandler((uint)ServerPacketID.SkillStopCasting_ToMap)]
        void RecvSkillStopCasting_ToMap(IIPSocket conn, BitStream r)
        {
            var casterEntityIndex = r.ReadMapEntityIndex();

            // Get the entity
            var casterEntity = _objGrabber.GetDynamicEntity<Character>(casterEntityIndex);
            if (casterEntity == null)
                return;

            // Set the entity as not casting
            casterEntity.IsCastingSkill = false;
        }

        [MessageHandler((uint)ServerPacketID.SkillStopCasting_ToUser)]
        void RecvSkillStopCasting_ToUser(IIPSocket conn, BitStream r)
        {
            GameplayScreen.SkillCastProgressBar.StopCasting();
        }

        [MessageHandler((uint)ServerPacketID.SkillUse)]
        void RecvSkillUse(IIPSocket conn, BitStream r)
        {
            var casterEntityIndex = r.ReadMapEntityIndex();
            var hasTarget = r.ReadBool();
            MapEntityIndex? targetEntityIndex = null;
            if (hasTarget)
                targetEntityIndex = r.ReadMapEntityIndex();
            var skillType = r.ReadEnum<SkillType>();

            var casterEntity = _objGrabber.GetDynamicEntity<CharacterEntity>(casterEntityIndex);
            CharacterEntity targetEntity = null;
            if (targetEntityIndex.HasValue)
                targetEntity = _objGrabber.GetDynamicEntity<CharacterEntity>(targetEntityIndex.Value);

            if (casterEntity == null)
                return;

            // Get the SkillInfo for the skill being used
            var skillInfo = _objGrabber.GetSkillInfo(skillType);
            if (skillInfo == null)
                return;

            // If an ActionDisplay is available for this skill, display it
            if (skillInfo.CastActionDisplay.HasValue)
            {
                var ad = ActionDisplayScripts.ActionDisplays[skillInfo.CastActionDisplay.Value];
                if (ad != null)
                    ad.Execute(Map, casterEntity, targetEntity);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "npcIndex")]
        [MessageHandler((uint)ServerPacketID.StartChatDialog)]
        void RecvStartChatDialog(IIPSocket conn, BitStream r)
        {
#pragma warning disable 168
            var npcIndex = r.ReadMapEntityIndex();
#pragma warning restore 168
            var dialogIndex = r.ReadNPCChatDialogID();

            var dialog = ClientNPCChatManager.Instance[dialogIndex];
            GameplayScreen.ChatDialogForm.StartDialog(dialog);
        }

        [MessageHandler((uint)ServerPacketID.StartQuestChatDialog)]
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
            var qds = availableQuests.Concat(turnInQuests).Distinct().Select(x => _questDescriptions.GetOrDefault(x)).ToArray();

            // Display the form
            GameplayScreen.AvailableQuestsForm.Display(qds, npcIndex);
        }

        [MessageHandler((uint)ServerPacketID.StartShopping)]
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

        [MessageHandler((uint)ServerPacketID.StopShopping)]
        void RecvStopShopping(IIPSocket conn, BitStream r)
        {
            GameplayScreen.ShopForm.HideShop();
        }

        [MessageHandler((uint)ServerPacketID.UpdateEquipmentSlot)]
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

        [MessageHandler((uint)ServerPacketID.UpdateStat)]
        void RecvUpdateStat(IIPSocket conn, BitStream r)
        {
            var isBaseStat = r.ReadBool();
            var stat = r.ReadStat<StatType>();

            var coll = isBaseStat ? UserInfo.BaseStats : UserInfo.ModStats;
            coll[stat.StatType] = stat.Value;
        }

        [MessageHandler((uint)ServerPacketID.UpdateVelocityAndPosition)]
        void RecvUpdateVelocityAndPosition(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();
            DynamicEntity dynamicEntity = null;

            // Grab the DynamicEntity
            // The map can be null if the spatial updates come very early (which isn't uncommon)
            if (Map != null)
                dynamicEntity = _objGrabber.GetDynamicEntity<DynamicEntity>(mapEntityIndex);

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

        [MessageHandler((uint)ServerPacketID.UseEntity)]
        void RecvUseEntity(IIPSocket conn, BitStream r)
        {
            var usedEntityIndex = r.ReadMapEntityIndex();
            var usedByIndex = r.ReadMapEntityIndex();

            // Grab the used DynamicEntity
            var usedEntity = _objGrabber.GetDynamicEntity<IUsableEntity>(usedEntityIndex);
            if (usedEntity == null)
                return;

            // Grab the one who used this DynamicEntity (we can still use it, we'll just pass null)
            var usedBy = Map.GetDynamicEntity(usedByIndex);
            if (usedBy == null)
                return;

            // Use it
            usedEntity.Use(usedBy);
        }

        [MessageHandler((uint)ServerPacketID.ReceiveFriends)]
        void RecvReceiveFriends(IIPSocket conn, BitStream r)
        {
            List<String> _onlineFriends = new List<String>();
            _onlineFriends = r.ReadString().Split(',').ToList<string>();
            string[] FriendsMap = r.ReadString().Split(',');
            string[] FriendsList = r.ReadString().Split(',');

            _onlineFriends.RemoveAll(x => x == "");
            int i = 0;
            FriendsForm._myFriends = new List<Friends>();

            foreach (string friend in _onlineFriends)
            {
                FriendsForm._myFriends.Add(new Friends
                {
                    Name = friend,
                    Map = FriendsMap[i],
                    Online = true
                });
            }

            foreach (string _friend in FriendsList)
            {
                FriendsForm._myFriends.Add(new Friends
                {
                    Name = _friend,
                    Online = false
                });

                i++;
            }

            FriendsForm._myFriends.RemoveDuplicates((x, y) => x.Name == y.Name);
            FriendsForm._myFriends.RemoveAll((x) => x.Name == "");
            FriendsForm.SortList();
        }

        [MessageHandler((uint)ServerPacketID.ReceivePrivateMessage)]
        void RecvReceivePrivateMessage(IIPSocket conn, BitStream r)
        {
            string Message = r.ReadString();
            string PrivateMessage = Message;

            // Display the private message
            GameplayScreen.AppendToChatOutput(Message, Color.BlueViolet);
        }

        [MessageHandler((uint)ServerPacketID.ReceiveOnlineUsers)]
        void RecvReceiveOnlineUsers(IIPSocket conn, BitStream r)
        {
            List<String> online = r.ReadString().Split(';').ToList();
            online.RemoveAll(x => x.IsEmpty());

            foreach (string user in online)
                OnlineUsersForm.Online.Add(user);

            OnlineUsersForm.Online.RemoveDuplicates((x, y) => x == y);
            OnlineUsersForm.Online.RemoveAll(x => x.IsEmpty());
            OnlineUsersForm.UpdateUsersList();
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public TickCount GetTime()
        {
            return GameplayScreen.GetTime();
        }

        #endregion
    }
}