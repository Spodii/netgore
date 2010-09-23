using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Content;
using NetGore.Features.GameTime;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Features.Skills;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;
using NetGore.NPCChat;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Screen for the actual game
    /// </summary>
    public class GameplayScreen : GameScreen, IGetTime
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string ScreenName = "game";
        const string _latencyString = "Latency: {0} ms";
        static readonly EmoticonDisplayManager _emoticonDisplayManager = EmoticonDisplayManager.Instance;

        readonly DamageTextPool _damageTextPool = new DamageTextPool();
        readonly SkeletonManager _skelManager = SkeletonManager.Create(ContentPaths.Build);
        readonly ISkillCooldownManager _skillCooldownManager = new SkillCooldownManager();

        AvailableQuestsForm _availableQuestsForm;
        Panel _cScreen;
        CharacterTargeter _characterTargeter;
        NPCChatDialogForm _chatDialogForm;
        ChatForm _chatForm;
        TickCount _currentTime = 0;
        Font _damageFont;
        DragDropHandler _dragDropHandler;
        EquipmentInfoRequester _equipmentInfoRequester;
        EquippedForm _equippedForm;
        GameplayScreenControls _gameControls;
        Font _guiFont;
        GUISettings _guiSettings;
        GuildForm _guildForm;
        InfoBox _infoBox;
        InventoryForm _inventoryForm;
        InventoryInfoRequester _inventoryInfoRequester;
        Label _latencyLabel;
        PeerTradeForm _peerTradeForm;
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
            PlayMusic = true;
        }

        public AvailableQuestsForm AvailableQuestsForm
        {
            get { return _availableQuestsForm; }
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

        public PeerTradeForm PeerTradeForm
        {
            get { return _peerTradeForm; }
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
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on <see cref="GameScreen.Deactivate"/>().
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            // Make sure to clear some stuff (text boxes, etc) that persists for the screen. Stuff that persists for the map only,
            // such as effects, should not need to be cleared here since it should be cleared when the map is set or changes.
            SoundManager.Stop3D();

            if (_chatForm != null)
            {
                _chatForm.ClearOutput();
                _chatForm.ClearInput();
            }

            // Set screen-specific globals
            ChatBubble.GetTopLeftCornerHandler = GetTopLeftDrawCorner;
            ControlBorder.AddGlobalColorTransformation(GlobalControlBorderTransformer);
        }

        public void AddChatBubble(Entity owner, string text)
        {
            ChatBubble.Create(_cScreen, owner, text);
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
        public void AppendToChatOutput(IEnumerable<StyledText> text)
        {
            _chatForm.AppendToOutput(text);
        }

        void ChatDialogForm_RequestEndDialog(NPCChatDialogForm sender)
        {
            using (var pw = ClientPacket.EndNPCChatDialog())
            {
                Socket.Send(pw, ClientMessageType.GUI);
            }
        }

        void ChatDialogForm_SelectResponse(NPCChatDialogForm sender, NPCChatResponseBase response)
        {
            using (var pw = ClientPacket.SelectNPCChatDialogResponse(response.Value))
            {
                Socket.Send(pw, ClientMessageType.GUI);
            }
        }

        void ChatForm_Say(ChatForm sender, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (text.Length > GameData.MaxClientSayLength)
                text = text.Substring(0, GameData.MaxClientSayLength);

            using (var pw = ClientPacket.Say(text))
            {
                Socket.Send(pw, ClientMessageType.Chat);
            }
        }

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in <see cref="GameScreen.Activate"/>().
        /// </summary>
        public override void Deactivate()
        {
            SoundManager.Stop3D();
            ControlBorder.RemoveGlobalColorTransformation(GlobalControlBorderTransformer);

            base.Deactivate();
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
                    Socket.Disconnect();
                }
                catch (Exception ex)
                {
                    // Ignore errors in disconnecting
                    const string errmsg = "Failed to disconnect client socket ({0}). Exception: {1}";
                    Debug.Fail(string.Format(errmsg, Socket, ex));
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, Socket, ex);
                }
            }
        }

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the
        /// active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Draw(TickCount gameTime)
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
            _damageTextPool.Draw(sb, _damageFont);
            DrawingManager.EndDrawWorld();

            // Draw the HUD layer
            sb = DrawingManager.BeginDrawGUI();
            _infoBox.Draw(sb);
            GUIManager.Draw(sb);
            sb.DrawString(_damageFont, "FPS: " + ScreenManager.FPS, Vector2.Zero, Color.White);
            sb.DrawString(_damageFont, string.Format("Game Time: {0}:{1:00}", GameDateTime.Now.Hour, GameDateTime.Now.Minute),
                          new Vector2(0, _damageFont.GetLineSpacing() + 1), Color.White);
            DrawingManager.EndDrawGUI();
        }

        public void EquippedForm_RequestUnequip(EquippedForm equippedForm, EquipmentSlot slot)
        {
            // Send unequip request
            using (var pw = ClientPacket.UnequipItem(slot))
            {
                Socket.Send(pw, ClientMessageType.GUIItems);
            }
        }

        /// <summary>
        /// Handles the ClickedQuit event of the gameMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void GameMenuClickedLogOut(object sender, EventArgs e)
        {
            // Change to the login screen
            ScreenManager.SetScreen(LoginScreen.ScreenName);

            // Disconnect the socket to close the connection
            Socket.Disconnect();
        }

        /// <summary>
        /// Gets the top-left corner to use for drawing for the given <paramref name="target"/>.
        /// </summary>
        /// <param name="target">The <see cref="ISpatial"/> to attach the bubble to.</param>
        /// <returns>The coordinate of the top-left corner of the <paramref name="target"/> to use for drawing.</returns>
        Vector2 GetTopLeftDrawCorner(ISpatial target)
        {
            Character asCharacter;

            // Make use of the Character's DrawPosition, otherwise it will look like the bubble is moving all over
            // the place since Characters like to interpolate all over the place
            if ((asCharacter = target as Character) != null)
                return asCharacter.LastScreenPosition;
            else
                return target.Position - World.Camera.Min;
        }

        /// <summary>
        /// Handles applying global transformations to all <see cref="ControlBorder"/>s drawn.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> the border being drawn is for.</param>
        /// <param name="c">The <see cref="Color"/> being used to draw the <see cref="ControlBorder"/>.</param>
        /// <returns>The <see cref="Color"/> to use to draw the <see cref="ControlBorder"/>.</returns>
        Color GlobalControlBorderTransformer(Control control, Color c)
        {
            // Only handle stuff for this screen
            if (control.GUIManager != GUIManager)
                return c;

            if (control is Form)
            {
                // Force forms to have an alpha value no greater than 150
                if (c.A > 150)
                    c.A = 150;
            }
            else if (!(control is Label))
            {
                // Every other control, except labels, must have an alpha <= 200
                if (c.A > 200)
                    c.A = 200;
            }

            return c;
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

            _socket = ClientSockets.Instance;

            _world = new World(this, new Camera2D(GameData.ScreenSize), new UserInfo(Socket));
            _world.MapChanged += World_MapChanged;

            // Create some misc goodies that require a reference to the Socket
            _equipmentInfoRequester = new EquipmentInfoRequester(UserInfo.Equipped, Socket);
            _inventoryInfoRequester = new InventoryInfoRequester(UserInfo.Inventory, Socket);

            // Other inits
            InitializeGUI();
            _characterTargeter = new CharacterTargeter(World);

            // NOTE: Test lighting
            _userLight = new Light { Size = new Vector2(512), IsEnabled = false };
            DrawingManager.LightManager.Add(_userLight);
        }

        /// <summary>
        /// Initializes the GUI components.
        /// </summary>
        void InitializeGUI()
        {
            // Set up the fonts
            _guiFont = ScreenManager.Content.LoadFont("Font/FullComp", 14, ContentLevel.Global);
            GUIManager.Font = _guiFont;
            GUIManager.Tooltip.Font = _guiFont;
            Character.NameFont = _guiFont;

            _cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize) { CanFocus = true };

            // Set up all the forms used by this screen
            _statsForm = new StatsForm(UserInfo, _cScreen);
            _statsForm.RequestRaiseStat += StatsForm_RequestRaiseStat;

            _inventoryForm = new InventoryForm(_dragDropHandler, x => x == UserInfo.Inventory, InventoryInfoRequester,
                                               new Vector2(250, 0), _cScreen);
            _inventoryForm.RequestDropItem += InventoryForm_RequestDropItem;
            _inventoryForm.RequestUseItem += InventoryForm_RequestUseItem;

            _shopForm = new ShopForm(_dragDropHandler, new Vector2(250, 0), _cScreen);
            _shopForm.RequestPurchase += ShopForm_RequestPurchase;

            _skillsForm = new SkillsForm(SkillCooldownManager, new Vector2(100, 0), _cScreen);
            _skillsForm.RequestUseSkill += SkillsForm_RequestUseSkill;

            _infoBox = new InfoBox(GameData.ScreenSize - new Vector2(5, 5), _guiFont);

            _equippedForm = new EquippedForm(_dragDropHandler, EquipmentInfoRequester, new Vector2(500, 0), _cScreen);
            _equippedForm.RequestUnequip += EquippedForm_RequestUnequip;

            _chatForm = new ChatForm(_cScreen, new Vector2(0, _cScreen.Size.Y));
            _chatForm.Say += ChatForm_Say;

            _chatDialogForm = new NPCChatDialogForm(new Vector2(50, 50), _cScreen);
            _chatDialogForm.SelectResponse += ChatDialogForm_SelectResponse;
            _chatDialogForm.RequestEndDialog += ChatDialogForm_RequestEndDialog;

            _statusEffectsForm = new StatusEffectsForm(_cScreen, new Vector2(_cScreen.Size.X, 0), this);

            _quickBarForm = new QuickBarForm(this, _cScreen, _cScreen.Position);

            _guildForm = new GuildForm(_cScreen, new Vector2(100, 100)) { GuildInfo = UserInfo.GuildInfo, IsVisible = false };
            new GroupForm(_cScreen, new Vector2(50, 350), new Vector2(150, 150)) { GroupInfo = UserInfo.GroupInfo };

            Func<QuestID, bool> questStartReqs = x => UserInfo.HasStartQuestRequirements.HasRequirements(x) ?? false;
            Func<QuestID, bool> questFinishReqs =
                x =>
                UserInfo.QuestInfo.ActiveQuests.Contains(x) && (UserInfo.HasFinishQuestRequirements.HasRequirements(x) ?? false);
            _availableQuestsForm = new AvailableQuestsForm(_cScreen, new Vector2(200), new Vector2(250, 350), questStartReqs,
                                                           questFinishReqs);
            _availableQuestsForm.QuestAccepted += availableQuestsForm_QuestAccepted;

            _latencyLabel = new Label(_cScreen, _cScreen.Size - new Vector2(75, 5)) { Text = string.Format(_latencyString, 0) };

            _skillCastProgressBar = new SkillCastProgressBar(_cScreen);

            var toolbar = new Toolbar(_cScreen, new Vector2(200, 200));
            toolbar.ItemClicked += Toolbar_ItemClicked;

            var gameMenu = new GameMenuForm(_cScreen);
            gameMenu.ClickedLogOut += GameMenuClickedLogOut;

            _peerTradeForm = new PeerTradeForm(_cScreen, new Vector2(200))
            { PeerTradeInfoHandler = Socket.PacketHandler.PeerTradeInfoHandler };

            // Add the forms to the GUI settings manager (which also restores any existing settings)
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
            _guiSettings.Add("PeerTradeForm", _peerTradeForm);

            // Set the focus to the screen container
            _cScreen.SetFocus();
        }

        /// <summary>
        /// Handles the <see cref="InventoryForm.RequestDropItem"/> event.
        /// </summary>
        /// <param name="inventoryForm">The sender.</param>
        /// <param name="slot">The slot containing the item requested to be dropped.</param>
        void InventoryForm_RequestDropItem(InventoryForm inventoryForm, InventorySlot slot)
        {
            // Make sure the event came from the user's inventory
            if (inventoryForm.Inventory != UserInfo.Inventory)
                return;

            // Depending on what screens are open, see if it makes sense to not just drop the item onto the ground
            if (PeerTradeForm.IsTradeActive)
            {
                // If we are doing a peer trade, add the item into the trade instead
                var ptih = PeerTradeForm.PeerTradeInfoHandler;
                if (ptih != null)
                    _peerTradeForm.AddToTrade(slot);
            }
            else if (ShopForm.IsVisible && ShopForm.ShopInfo != null)
            {
                // If we are currently shopping, try to sell the item instead
                if (ShopForm.ShopInfo.CanBuy)
                    UserInfo.Inventory.SellToShop(slot, GUIManager);
            }
            else
            {
                // Drop the item onto the ground
                UserInfo.Inventory.Drop(slot, GUIManager);
            }
        }

        /// <summary>
        /// Handles the <see cref="InventoryForm.RequestUseItem"/> event.
        /// </summary>
        /// <param name="inventoryForm">The sender.</param>
        /// <param name="slot">The slot containing the item requested to be used.</param>
        void InventoryForm_RequestUseItem(InventoryForm inventoryForm, InventorySlot slot)
        {
            // Make sure the event came from the user's inventory
            if (inventoryForm.Inventory != UserInfo.Inventory)
                return;

            // Send a request to use the item to the server
            UserInfo.Inventory.Use(slot);
        }

        /// <summary>
        /// Handles the loading of game content. Any content that is loaded should be placed in here.
        /// This will be invoked once (right after Initialize()), along with any time the content needs
        /// to be reloaded for whatever reason.
        /// </summary>
        public override void LoadContent()
        {
            _damageFont = ScreenManager.Content.LoadFont("Font/Arial", 14, ContentLevel.Global);
        }

        void ShopForm_RequestPurchase(ShopForm shopForm, ShopItemIndex slot)
        {
            using (var pw = ClientPacket.BuyFromShop(slot, 1))
            {
                Socket.Send(pw, ClientMessageType.GUI);
            }
        }

        public void SkillsForm_RequestUseSkill(SkillType skillType)
        {
            using (var pw = ClientPacket.UseSkill(skillType, _characterTargeter.TargetCharacterIndex))
            {
                Socket.Send(pw, ClientMessageType.CharacterAction);
            }
        }

        /// <summary>
        /// Handles when a stat is requested to be raised.
        /// </summary>
        /// <param name="statsForm">StatsForm that the event came from.</param>
        /// <param name="statType">StatType requested to be raised.</param>
        void StatsForm_RequestRaiseStat(StatsForm statsForm, StatType statType)
        {
            using (var pw = ClientPacket.RaiseStat(statType))
            {
                Socket.Send(pw, ClientMessageType.GUI);
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
        public override void Update(TickCount gameTime)
        {
            ThreadAsserts.IsMainThread();

            // Get the current time
            _currentTime = gameTime;

            if (UserChar == null)
            {
                _userLight.IsEnabled = false;
                return;
            }

            ScreenManager.AudioManager.ListenerPosition = UserChar.Center;

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
            _emoticonDisplayManager.Update(_currentTime);

            // Update targeting
            _characterTargeter.Update(GUIManager);
            _gameControls.TargetIndex = _characterTargeter.TargetCharacterIndex;

            // Update controls
            _gameControls.Update(GUIManager, _currentTime);

            var sock = _socket.RemoteSocket;
            if (_latencyLabel != null && sock != null && sock.IsConnected)
                _latencyLabel.Text = string.Format(_latencyString,
                                                   sock.AverageLatency < 1 ? "<1" : Math.Round(sock.AverageLatency).ToString());

            _userLight.IsEnabled = true;
            _userLight.Teleport(UserChar.Position);

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

            // Remove the refraction effects from the old map
            if (oldMap != null)
            {
                foreach (var fx in oldMap.RefractionEffects)
                {
                    DrawingManager.RefractionManager.Remove(fx);
                }
            }

            // Add the refraction effects for the new map
            foreach (var fx in newMap.RefractionEffects)
            {
                DrawingManager.RefractionManager.Add(fx);
            }
        }

        void availableQuestsForm_QuestAccepted(Control sender, IQuestDescription args)
        {
            using (var pw = ClientPacket.AcceptOrTurnInQuest(AvailableQuestsForm.QuestProviderIndex, args.QuestID))
            {
                Socket.Send(pw, ClientMessageType.GUI);
            }
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current game time where time 0 is when the application started
        /// </summary>
        /// <returns>Current game time in milliseconds</returns>
        public TickCount GetTime()
        {
            return _currentTime;
        }

        #endregion
    }
}