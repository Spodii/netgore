using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using NetGore.Features.Skills;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;
using NetGore.Network;
using NetGore.NPCChat;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Screen for the actual game
    /// </summary>
    class GameplayScreen : GameScreen, IGetTime
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string ScreenName = "game";
        const string _latencyString = "Latency: {0} ms";
        static readonly EmoticonDisplayManager _emoticonDisplayManager = EmoticonDisplayManager.Instance;

        readonly DamageTextPool _damageTextPool = new DamageTextPool();
        readonly SkeletonManager _skelManager = SkeletonManager.Create(ContentPaths.Build);
        readonly ISkillCooldownManager _skillCooldownManager = new SkillCooldownManager();

        AvailableQuestsForm _availableQuestsForm;
        CharacterTargeter _characterTargeter;
        ChatBubbleManager _chatBubbleManager;
        NPCChatDialogForm _chatDialogForm;
        ChatForm _chatForm;
        int _currentTime = 0;
        Font _damageFont;
        DragDropHandler _dragDropHandler;
        EquipmentInfoRequester _equipmentInfoRequester;
        EquippedForm _equippedForm;
        GameplayScreenControls _gameControls;
        Font _guiFont;
        GuildForm _guildForm;
        GUISettings _guiSettings;
        InfoBox _infoBox;
        InventoryForm _inventoryForm;
        InventoryInfoRequester _inventoryInfoRequester;
        Label _latencyLabel;
        QuickBarForm _quickBarForm;
        ShopForm _shopForm;
        SkillCastProgressBar _skillCastProgressBar;
        SkillsForm _skillsForm;
        ClientSockets _socket;
        StatsForm _statsForm;
        StatusEffectsForm _statusEffectsForm;
        ILight _userLight;
        World _world;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameplayScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public GameplayScreen(IScreenManager screenManager) : base(screenManager, ScreenName)
        {
        }

        public AvailableQuestsForm AvailableQuestsForm
        {
            get { return _availableQuestsForm; }
        }

        /// <summary>
        /// Gets the <see cref="ChatBubbleManagerBase"/>.
        /// </summary>
        public ChatBubbleManagerBase ChatBubbleManager
        {
            get { return _chatBubbleManager; }
        }

        public NPCChatDialogForm ChatDialogForm
        {
            get { return _chatDialogForm; }
        }

        /// <summary>
        /// Gets the DamageTextPool
        /// </summary>
        public DamageTextPool DamageTextPool
        {
            get { return _damageTextPool; }
        }

        public DragDropHandler DragDropHandler
        {
            get { return _dragDropHandler; }
        }

        public EquipmentInfoRequester EquipmentInfoRequester
        {
            get { return _equipmentInfoRequester; }
        }

        /// <summary>
        /// Gets the InfoBox used by the screen
        /// </summary>
        public InfoBox InfoBox
        {
            get { return _infoBox; }
        }

        public InventoryInfoRequester InventoryInfoRequester
        {
            get { return _inventoryInfoRequester; }
        }

        public Map Map
        {
            get { return World.Map; }
        }

        public QuickBarForm QuickBarForm
        {
            get { return _quickBarForm; }
        }

        public ShopForm ShopForm
        {
            get { return _shopForm; }
        }

        /// <summary>
        /// Gets the global SkeletonManager
        /// </summary>
        public SkeletonManager SkeletonManager
        {
            get { return _skelManager; }
        }

        public SkillCastProgressBar SkillCastProgressBar
        {
            get { return _skillCastProgressBar; }
        }

        public ISkillCooldownManager SkillCooldownManager
        {
            get { return _skillCooldownManager; }
        }

        /// <summary>
        /// Gets the client socket system used to handle the networking for the game
        /// </summary>
        public ClientSockets Socket
        {
            get { return _socket; }
        }

        public StatusEffectsForm StatusEffectsForm
        {
            get { return _statusEffectsForm; }
        }

        /// <summary>
        /// Gets the user's character
        /// </summary>
        public Character UserChar
        {
            get { return World.UserChar; }
        }

        /// <summary>
        /// Gets the <see cref="UserInfo"/>.
        /// </summary>
        public UserInfo UserInfo
        {
            get { return World.UserInfo; }
        }

        /// <summary>
        /// Gets the root world of the game
        /// </summary>
        public World World
        {
            get { return _world; }
        }

        /// <summary>
        /// Appends a set of styled text to the chat's output TextBox.
        /// </summary>
        /// <param name="text">Text to append to the chat's output TextBox.</param>
        public void AppendToChatOutput(string text)
        {
            _chatForm.AppendToOutput(text);
        }

        /// <summary>
        /// Appends a string of text to the chat's output TextBox.
        /// </summary>
        /// <param name="text">Text to append to the chat's output TextBox.</param>
        /// <param name="color">Color of the text to append.</param>
        public void AppendToChatOutput(string text, Color color)
        {
            _chatForm.AppendToOutput(text, color);
        }

        /// <summary>
        /// Appends a set of styled text to the chat's output TextBox.
        /// </summary>
        /// <param name="text">Text to append to the chat's output TextBox.</param>
        public void AppendToChatOutput(List<StyledText> text)
        {
            _chatForm.AppendToOutput(text);
        }

        void availableQuestsForm_QuestAccepted(Control sender, IQuestDescription args)
        {
            using (var pw = ClientPacket.AcceptOrTurnInQuest(AvailableQuestsForm.QuestProviderIndex, args.QuestID))
            {
                Socket.Send(pw);
            }
        }

        void ChatDialogForm_RequestEndDialog(NPCChatDialogForm sender)
        {
            using (PacketWriter pw = ClientPacket.EndNPCChatDialog())
            {
                Socket.Send(pw);
            }
        }

        void ChatDialogForm_SelectResponse(NPCChatDialogForm sender, NPCChatResponseBase response)
        {
            using (PacketWriter pw = ClientPacket.SelectNPCChatDialogResponse(response.Value))
            {
                Socket.Send(pw);
            }
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on <see cref="GameScreen.Deactivate"/>().
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            SoundManager.Stop3D();
        }

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in <see cref="GameScreen.Activate"/>().
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();

            SoundManager.Stop3D();
        }

        void ChatForm_Say(ChatForm sender, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (text.Length > GameData.MaxClientSayLength)
                text = text.Substring(0, GameData.MaxClientSayLength);

            using (PacketWriter pw = ClientPacket.Say(text))
            {
                Socket.Send(pw);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (_guiSettings != null)
                _guiSettings.Dispose();

            if (Socket != null)
            {
                try
                {
                    Socket.Dispose();
                }
                catch (Exception ex)
                {
                    // Ignore errors in disconnecting
                    Debug.Fail("Disconnect failed: " + ex);
                    if (log.IsErrorEnabled)
                        log.ErrorFormat("Failed to disconnect client socket ({0}). Exception: {1}", Socket, ex);
                }
            }
        }

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the
        /// active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Draw(int gameTime)
        {
            if (UserChar == null)
                return;

            // Update the ambient light based on the game time
            DrawingManager.LightManager.Ambient = Map.GetModifiedAmbientLight();

            // Update the camera
            World.Camera.Min = World.UserChar.GetCameraPos(World.Camera);

            // Since we only update entities in view, and the draw position is included in that update, make sure
            // that the camera focuses on the user properly if they teleport out of view of the camera.
            if (!World.Camera.InView(World.UserChar))
                World.Camera.CenterOn(World.UserChar);

            // Draw the world layer
            var sb = DrawingManager.BeginDrawWorld(World.Camera);
            if (sb == null)
                return;

            World.Draw(sb);
            _chatBubbleManager.Draw(sb);
            _damageTextPool.Draw(sb, _damageFont);
            DrawingManager.EndDrawWorld();

            // Draw the HUD layer
            sb = DrawingManager.BeginDrawGUI();
            _infoBox.Draw(sb);
            GUIManager.Draw(sb);
            sb.DrawString(_damageFont, "FPS: " + ScreenManager.FPS, Vector2.Zero, Color.White);
            sb.DrawString(_damageFont, string.Format("Game Time: {0}:{1:00}", GameDateTime.Now.Hour, GameDateTime.Now.Minute),
                          new Vector2(0, _damageFont.CharacterSize + 1), Color.White);
            DrawingManager.EndDrawGUI();
        }

        public void EquippedForm_RequestUnequip(EquippedForm equippedForm, EquipmentSlot slot)
        {
            // Send unequip request
            using (PacketWriter pw = ClientPacket.UnequipItem(slot))
            {
                Socket.Send(pw);
            }
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            _gameControls = new GameplayScreenControls(this);
            _dragDropHandler = new DragDropHandler(this);

            ClientSockets.Initialize(this);

            _socket = ClientSockets.Instance;

            _world = new World(this, new Camera2D(GameData.ScreenSize), new UserInfo(Socket));
            _world.MapChanged += World_MapChanged;

            // Create the socket
            Socket.Disconnected += OnDisconnect;

            // Create some misc goodies that require a reference to the Socket
            _equipmentInfoRequester = new EquipmentInfoRequester(UserInfo.Equipped, Socket);
            _inventoryInfoRequester = new InventoryInfoRequester(UserInfo.Inventory, Socket);

            // Other inits
            InitializeGUI();
            _characterTargeter = new CharacterTargeter(World);

            // NOTE: Test lighting
            _userLight = new Light { Size = new Vector2(512), IsEnabled = false };
            DrawingManager.LightManager.Add(_userLight);

            _chatBubbleManager = new ChatBubbleManager(GUIManager.SkinManager, _guiFont);
        }

        /// <summary>
        /// Initializes the GUI components.
        /// </summary>
        void InitializeGUI()
        {
            _guiFont = ScreenManager.Content.LoadFont("Font/Arial", 14, ContentLevel.Global);
            GUIManager.Font = _guiFont;
            GUIManager.Tooltip.Font = _guiFont;
            Character.NameFont = _guiFont;

            Panel cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize) { CanFocus = false };
            _statsForm = new StatsForm(UserInfo, cScreen);
            _statsForm.RequestRaiseStat += StatsForm_RequestRaiseStat;

            _inventoryForm = new InventoryForm(_dragDropHandler, x => x == UserInfo.Inventory, InventoryInfoRequester,
                                               new Vector2(250, 0), cScreen);
            _inventoryForm.RequestDropItem += InventoryForm_RequestDropItem;
            _inventoryForm.RequestUseItem += InventoryForm_RequestUseItem;

            _shopForm = new ShopForm(_dragDropHandler, new Vector2(250, 0), cScreen);
            _shopForm.RequestPurchase += ShopForm_RequestPurchase;

            _skillsForm = new SkillsForm(SkillCooldownManager, new Vector2(100, 0), cScreen);
            _skillsForm.RequestUseSkill += SkillsForm_RequestUseSkill;

            _infoBox = new InfoBox(GameData.ScreenSize - new Vector2(5, 5), _guiFont);

            _equippedForm = new EquippedForm(_dragDropHandler, EquipmentInfoRequester, new Vector2(500, 0), cScreen);
            _equippedForm.RequestUnequip += EquippedForm_RequestUnequip;

            _chatForm = new ChatForm(cScreen, new Vector2(0, cScreen.Size.Y));
            _chatForm.Say += ChatForm_Say;

            _chatDialogForm = new NPCChatDialogForm(new Vector2(50, 50), cScreen);
            _chatDialogForm.SelectResponse += ChatDialogForm_SelectResponse;
            _chatDialogForm.RequestEndDialog += ChatDialogForm_RequestEndDialog;

            _statusEffectsForm = new StatusEffectsForm(cScreen, new Vector2(cScreen.Size.X, 0), this);

            _quickBarForm = new QuickBarForm(this, cScreen, cScreen.Position);

            _guildForm = new GuildForm(cScreen, new Vector2(100, 100)) { GuildInfo = UserInfo.GuildInfo, IsVisible = false };
            new GroupForm(cScreen, new Vector2(50, 350), new Vector2(150, 150)) { GroupInfo = UserInfo.GroupInfo };

            Func<QuestID, bool> questStartReqs = x => UserInfo.HasStartQuestRequirements.HasRequirements(x) ?? false;
            Func<QuestID, bool> questFinishReqs =
                x =>
                UserInfo.QuestInfo.ActiveQuests.Contains(x) && (UserInfo.HasFinishQuestRequirements.HasRequirements(x) ?? false);
            _availableQuestsForm = new AvailableQuestsForm(cScreen, new Vector2(200), new Vector2(250, 350), questStartReqs,
                                                           questFinishReqs);
            _availableQuestsForm.QuestAccepted += availableQuestsForm_QuestAccepted;

            _latencyLabel = new Label(cScreen, cScreen.Size - new Vector2(75, 5)) { Text = string.Format(_latencyString, 0) };

            _skillCastProgressBar = new SkillCastProgressBar(cScreen);

            Toolbar toolbar = new Toolbar(cScreen, new Vector2(200, 200));
            toolbar.ItemClicked += Toolbar_ItemClicked;

            // Apply the settings
            _guiSettings = new GUISettings("Default"); // FUTURE: Allow changing of the profile
            _guiSettings.Add("InventoryForm", _inventoryForm);
            _guiSettings.Add("EquippedForm", _equippedForm);
            _guiSettings.Add("StatsForm", _statsForm);
            _guiSettings.Add("ChatForm", _chatForm);
            _guiSettings.Add("ToolbarForm", toolbar);
            _guiSettings.Add("GuildForm", _guildForm);
            _guiSettings.Add("StatusEffectsForm", _statusEffectsForm);
            _guiSettings.Add("SkillsForm", _skillsForm);
            _guiSettings.Add("QuickBarForm", _quickBarForm);
        }

        void InventoryForm_RequestDropItem(InventoryForm inventoryForm, InventorySlot slot)
        {
            if (inventoryForm.Inventory != UserInfo.Inventory)
                return;

            if (ShopForm.IsVisible && ShopForm.ShopInfo != null)
            {
                if (ShopForm.ShopInfo.CanBuy)
                {
                    using (PacketWriter pw = ClientPacket.SellInventoryToShop(slot, 1))
                    {
                        Socket.Send(pw);
                    }
                }
            }
            else
                UserInfo.Inventory.Drop(slot);
        }

        void InventoryForm_RequestUseItem(InventoryForm inventoryForm, InventorySlot slot)
        {
            if (inventoryForm.Inventory != UserInfo.Inventory)
                return;

            UserInfo.Inventory.Use(slot);
        }

        /// <summary>
        /// Handles the loading of game content. Any content that is loaded should be placed in here.
        /// This will be invoked once (right after Initialize()), along with an additional time for
        /// every time XNA notifies the ScreenManager that the game content needs to be reloaded.
        /// </summary>
        public override void LoadContent()
        {
            _damageFont = ScreenManager.Content.LoadFont("Font/Arial", 14, ContentLevel.Global);
        }

        void OnDisconnect(SocketManager sender, IIPSocket conn)
        {
            // We ony return want to the login screen if we were at this screen when the socket was disconnected
            if (ScreenManager.ActiveScreen != this)
                return;

            LoginScreen login = (LoginScreen)ScreenManager.GetScreen(LoginScreen.ScreenName);
            if (ScreenManager.ActiveScreen != login)
            {
                login.SetError("Connection to server lost.");
                ScreenManager.SetScreen(LoginScreen.ScreenName);
            }
        }

        void ShopForm_RequestPurchase(ShopForm shopForm, ShopItemIndex slot)
        {
            using (PacketWriter pw = ClientPacket.BuyFromShop(slot, 1))
            {
                Socket.Send(pw);
            }
        }

        public void SkillsForm_RequestUseSkill(SkillType skillType)
        {
            using (PacketWriter pw = ClientPacket.UseSkill(skillType, _characterTargeter.TargetCharacterIndex))
            {
                Socket.Send(pw);
            }
        }

        /// <summary>
        /// Handles when a stat is requested to be raised.
        /// </summary>
        /// <param name="statsForm">StatsForm that the event came from.</param>
        /// <param name="statType">StatType requested to be raised.</param>
        void StatsForm_RequestRaiseStat(StatsForm statsForm, StatType statType)
        {
            using (PacketWriter pw = ClientPacket.RaiseStat(statType))
            {
                Socket.Send(pw);
            }
        }

        /// <summary>
        /// Handles when a Toolbar item was clicked.
        /// </summary>
        /// <param name="toolbar">Toolbar that was clicked.</param>
        /// <param name="itemType">ToolbarItemType of the toolbar item clicked.</param>
        /// <param name="control">Control that was clicked.</param>
        void Toolbar_ItemClicked(Toolbar toolbar, ToolbarItemType itemType, Control control)
        {
            switch (itemType)
            {
                case ToolbarItemType.Equipped:
                    _equippedForm.IsVisible = !_equippedForm.IsVisible;
                    break;

                case ToolbarItemType.Inventory:
                    _inventoryForm.IsVisible = !_inventoryForm.IsVisible;
                    break;

                case ToolbarItemType.Stats:
                    _statsForm.IsVisible = !_statsForm.IsVisible;
                    break;

                case ToolbarItemType.Skills:
                    _skillsForm.IsVisible = !_skillsForm.IsVisible;
                    break;

                case ToolbarItemType.Guild:
                    _guildForm.IsVisible = !_guildForm.IsVisible;
                    break;
            }
        }

        /// <summary>
        /// Updates the screen if it is currently the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(int gameTime)
        {
            ThreadAsserts.IsMainThread();

            // Get the current time
            _currentTime = gameTime;

            if (UserChar == null)
            {
                _userLight.IsEnabled = false;
                return;
            }

            ScreenManager.AudioManager.ListenerPosition = UserChar.Position;

            _userLight.IsEnabled = true;
            _userLight.Teleport(UserChar.Position);

            // HACK: What a stupid way to make sure the correct inventory and equipped is used...
            _inventoryForm.Inventory = UserInfo.Inventory;
            _equippedForm.UserEquipped = UserInfo.Equipped;

            // Check to hide the shopping form from the user going out of range of the shop owner
            var shopInfo = ShopForm.ShopInfo;
            if (shopInfo != null && shopInfo.ShopOwner != null && !GameData.IsValidDistanceToShop(UserChar, shopInfo.ShopOwner))
                ShopForm.HideShop();

            // Update some other goodies
            World.Update();
            _damageTextPool.Update(_currentTime);
            _guiSettings.Update(_currentTime);
            _chatBubbleManager.Update(_currentTime);
            _emoticonDisplayManager.Update(_currentTime);

            // Update targeting
            _characterTargeter.Update(GUIManager);
            _gameControls.TargetIndex = _characterTargeter.TargetCharacterIndex;

            // Update controls
            if (UserChar != null)
                _gameControls.Update(GUIManager, _currentTime);

            if (_latencyLabel != null)
                _latencyLabel.Text = string.Format(_latencyString, _socket.Latency);

            base.Update(gameTime);
        }

        void World_MapChanged(World world, Map oldMap, Map newMap)
        {
            // Stop all sounds
            SoundManager.Stop();

            // Set the new music
            if (!newMap.MusicID.HasValue)
                ScreenMusic = null;
            else if (!MusicManager.Play(newMap.MusicID.Value))
            {
                var musicTrack = MusicManager.GetMusicInfo(newMap.MusicID.Value);
                if (musicTrack == null)
                {
                    const string errmsg = "Failed to play map music with ID `{0}`. No music with that ID could be found.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, newMap.MusicID);
                    Debug.Fail(string.Format(errmsg, newMap.MusicID));
                }

                ScreenMusic = musicTrack;
            }

            // Remove the lights from the old map
            if (oldMap != null)
            {
                foreach (var light in oldMap.Lights)
                {
                    DrawingManager.LightManager.Remove(light);
                }
            }

            // Add the lights for the new map
            foreach (var light in newMap.Lights)
            {
                DrawingManager.LightManager.Add(light);
            }
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current game time where time 0 is when the application started
        /// </summary>
        /// <returns>Current game time in milliseconds</returns>
        public int GetTime()
        {
            return _currentTime;
        }

        #endregion
    }
}