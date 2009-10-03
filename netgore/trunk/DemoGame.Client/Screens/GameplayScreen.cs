using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame;
using DemoGame.Client.Controls;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.Network;
using NetGore.NPCChat;

namespace DemoGame.Client
{
    /// <summary>
    /// Screen for the actual game
    /// </summary>
    class GameplayScreen : GameScreen, IDisposable, IGetTime
    {
        public const string ScreenName = "game";

        const string _latencyString = "Latency: {0} ms";

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Pool for the damage text
        /// </summary>
        readonly DamageTextPool _damageTextPool = new DamageTextPool();

        readonly GameplayScreenControls _gameControls;

        /// <summary>
        /// Skeleton templates that will remain in memory at all times
        /// </summary>
        readonly SkeletonManager _skelManager = new SkeletonManager(ContentPaths.Build.Skeletons);

        NPCChatDialogForm _chatDialogForm;

        ChatForm _chatForm;

        /// <summary>
        /// Current total time in milliseconds - used as the root of all timing
        /// in external classes through the GetTime method
        /// </summary>
        int _currentTime = 0;

        /// <summary>
        /// Font used for damage
        /// </summary>
        SpriteFont _damageFont;

        bool _disposed;
        EquipmentInfoRequester _equipmentInfoRequester;
        EquippedForm _equippedForm;

        /// <summary>
        /// GUI Manager
        /// </summary>
        GUIManager _gui;

        /// <summary>
        /// Font for the GUI
        /// </summary>
        SpriteFont _guiFont;

        GUISettings _guiSettings;

        /// <summary>
        /// Information box
        /// </summary>
        InfoBox _infoBox;

        InventoryForm _inventoryForm;
        InventoryInfoRequester _inventoryInfoRequester;

        /// <summary>
        /// Label used for displaying the latency.
        /// </summary>
        Label _latencyLabel;

        ShopForm _shopForm;

        SkillsForm _skillsForm;

        /// <summary>
        /// Client socket system used to handle the networking for the game
        /// </summary>
        ClientSockets _socket;

        /// <summary>
        /// SpriteBatch object used for the drawing (reference to ScreenManager.SpriteBatch)
        /// </summary>
        SpriteBatch _spriteBatch;

        /// <summary>
        /// Form used to display stats
        /// </summary>
        StatsForm _statsForm;

        StatusEffectsForm _statusEffectsForm;

        UserInfo _userInfo;

        /// <summary>
        /// Root world of the game
        /// </summary>
        World _world;

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

        public UserInfo UserInfo
        {
            get { return _userInfo; }
        }

        /// <summary>
        /// Gets the root world of the game
        /// </summary>
        public World World
        {
            get { return _world; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameplayScreen"/> class.
        /// </summary>
        public GameplayScreen() : base(ScreenName)
        {
            _gameControls = new GameplayScreenControls(this);
        }

        void _inventoryForm_OnRequestDropItem(InventoryForm inventoryForm, InventorySlot slot)
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

        void _inventoryForm_OnRequestUseItem(InventoryForm inventoryForm, InventorySlot slot)
        {
            if (inventoryForm.Inventory != UserInfo.Inventory)
                return;

            UserInfo.Inventory.Use(slot);
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

        void ChatDialogForm_OnRequestEndDialog(NPCChatDialogForm sender)
        {
            using (PacketWriter pw = ClientPacket.EndNPCChatDialog())
            {
                Socket.Send(pw);
            }
        }

        void ChatDialogForm_OnSelectResponse(NPCChatDialogForm sender, NPCChatResponseBase response)
        {
            using (PacketWriter pw = ClientPacket.SelectNPCChatDialogResponse(response.Value))
            {
                Socket.Send(pw);
            }
        }

        void ChatForm_OnSay(ChatForm sender, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            using (PacketWriter pw = ClientPacket.Say(text))
            {
                Socket.Send(pw);
            }
        }

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the 
        /// active screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            // Don't draw if we don't know who our character is
            if (UserChar == null)
                return;

            DrawWorld();
            DrawHUD();
        }

        /// <summary>
        /// Draws the HUD, which is displayed above the world and not modified by the camera position.
        /// </summary>
        void DrawHUD()
        {
            _spriteBatch.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            _infoBox.Draw(_spriteBatch);
            _gui.Draw(_spriteBatch);
            _spriteBatch.DrawString(_damageFont, "FPS: " + ScreenManager.FPS, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }

        /// <summary>
        /// Draws the game world, which is modified by the camera position.
        /// </summary>
        void DrawWorld()
        {
            // Update the camera
            World.Camera.Min = World.UserChar.GetCameraPos();

            // Draw the world layer
            _spriteBatch.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None,
                                         World.Camera.Matrix);
            World.Draw(_spriteBatch);
            _damageTextPool.Draw(_spriteBatch, _damageFont);
            _spriteBatch.End();
        }

        void EquippedForm_OnRequestUnequip(EquippedForm equippedForm, EquipmentSlot slot)
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
            _world = new World(this, new Camera2D(GameData.ScreenSize));

            // Create the socket
            _socket = new ClientSockets(this);
            Socket.OnDisconnect += OnDisconnect;

            // Create some misc goodies that require a reference to the Socket
            _userInfo = new UserInfo(Socket);
            _equipmentInfoRequester = new EquipmentInfoRequester(_userInfo.Equipped, Socket);
            _inventoryInfoRequester = new InventoryInfoRequester(_userInfo.Inventory, Socket);

            // Other inits
            InitializeGUI();
        }

        /// <summary>
        /// Initializes the GUI components.
        /// </summary>
        void InitializeGUI()
        {
            _guiFont = ScreenManager.Content.Load<SpriteFont>("Font/Game");
            _gui = new GUIManager(_guiFont);
            Character.NameFont = _guiFont;

            Panel cScreen = new Panel(_gui, Vector2.Zero, ScreenManager.ScreenSize) { CanFocus = false };
            _statsForm = new StatsForm(UserInfo, cScreen);
            _statsForm.OnRaiseStat += StatsForm_OnRaiseStat;

            _inventoryForm = new InventoryForm(InventoryInfoRequester, new Vector2(250, 0), cScreen);
            _inventoryForm.OnRequestDropItem += _inventoryForm_OnRequestDropItem;
            _inventoryForm.OnRequestUseItem += _inventoryForm_OnRequestUseItem;

            _shopForm = new ShopForm(new Vector2(250, 0), cScreen);
            _shopForm.OnPurchase += ShopForm_OnPurchase;

            _skillsForm = new SkillsForm(new Vector2(100, 0), cScreen);
            _skillsForm.OnUseSkill += SkillsForm_OnUseSkill;

            _infoBox = new InfoBox(GameData.ScreenSize - new Vector2(5, 5), _guiFont);

            _equippedForm = new EquippedForm(EquipmentInfoRequester, new Vector2(500, 0), cScreen);
            _equippedForm.OnRequestUnequip += EquippedForm_OnRequestUnequip;

            _chatForm = new ChatForm(cScreen, new Vector2(0, cScreen.Size.Y));
            _chatForm.OnSay += ChatForm_OnSay;

            _chatDialogForm = new NPCChatDialogForm(new Vector2(50, 50), cScreen);
            _chatDialogForm.OnSelectResponse += ChatDialogForm_OnSelectResponse;
            _chatDialogForm.OnRequestEndDialog += ChatDialogForm_OnRequestEndDialog;

            _statusEffectsForm = new StatusEffectsForm(cScreen, new Vector2(cScreen.Size.X, 0), this);

            _latencyLabel = new Label(string.Format(_latencyString, 0), cScreen.Size - new Vector2(75, 5), cScreen);

            Toolbar toolbar = new Toolbar(cScreen, new Vector2(200, 200));
            toolbar.OnClickItem += Toolbar_OnClickItem;

            // Apply the settings
            _guiSettings = new GUISettings("Default"); // FUTURE: Allow changing of the profile
            _guiSettings.Add("InventoryForm", _inventoryForm);
            _guiSettings.Add("EquippedForm", _equippedForm);
            _guiSettings.Add("StatsForm", _statsForm);
            _guiSettings.Add("ChatForm", _chatForm);
            _guiSettings.Add("ToolbarForm", toolbar);
        }

        /// <summary>
        /// Handles the loading of game content. Any content that is loaded should be placed in here.
        /// This will be invoked once (right after Initialize()), along with an additional time for
        /// every time XNA notifies the ScreenManager that the game content needs to be reloaded.
        /// </summary>
        public override void LoadContent()
        {
            _spriteBatch = ScreenManager.SpriteBatch;
            _damageFont = ScreenManager.Content.Load<SpriteFont>("Font/Game");
        }

        void OnDisconnect(IIPSocket conn)
        {
            LoginScreen login = (LoginScreen)ScreenManager.GetScreen(LoginScreen.ScreenName);
            if (ScreenManager.ActiveScreen != login)
            {
                login.SetError("Connection to server lost.");
                ScreenManager.SetScreen(LoginScreen.ScreenName);
            }
        }

        void ShopForm_OnPurchase(ShopForm shopForm, ShopItemIndex slot)
        {
            using (PacketWriter pw = ClientPacket.BuyFromShop(slot, 1))
            {
                Socket.Send(pw);
            }
        }

        void SkillsForm_OnUseSkill(SkillType skillType)
        {
            using (PacketWriter pw = ClientPacket.UseSkill(skillType))
            {
                Socket.Send(pw);
            }
        }

        /// <summary>
        /// Handles when a stat is requested to be raised.
        /// </summary>
        /// <param name="statsForm">StatsForm that the event came from.</param>
        /// <param name="statType">StatType requested to be raised.</param>
        void StatsForm_OnRaiseStat(StatsForm statsForm, StatType statType)
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
        void Toolbar_OnClickItem(Toolbar toolbar, ToolbarItemType itemType, Control control)
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
            }
        }

        /// <summary>
        /// Handles updating of the screen. This will only be called while the screen is the active screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public override void Update(GameTime gameTime)
        {
            // Get the current time
            _currentTime = (int)gameTime.TotalRealTime.TotalMilliseconds;

            // Update the network
            Socket.Heartbeat();

            if (UserChar == null)
                return;

            // HACK: What a stupid way to make sure the correct inventory and equipped is used...
            _inventoryForm.Inventory = UserInfo.Inventory;
            _equippedForm.UserEquipped = UserInfo.Equipped;

            // Check to hide the shopping form from the user going out of range of the shop owner
            ShopInfo shopInfo = ShopForm.ShopInfo;
            if (shopInfo != null && shopInfo.ShopOwner != null && !GameData.IsValidDistanceToShop(UserChar, shopInfo.ShopOwner))
                ShopForm.HideShop();

            // Update some other goodies
            World.Update();
            _gui.Update(_currentTime);
            _damageTextPool.Update(_currentTime);
            _guiSettings.Update(_currentTime);

            if (UserChar != null)
                _gameControls.Update(_gui, _currentTime);

            if (_latencyLabel != null)
                _latencyLabel.Text = string.Format(_latencyString, _socket.Latency);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

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

        #endregion

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