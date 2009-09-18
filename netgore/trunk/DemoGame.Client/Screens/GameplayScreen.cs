using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Client.Controls;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        /// <summary>
        /// Minimum time the user must wait before performing a new attack
        /// </summary>
        const int _minAttackRate = 150;

        /// <summary>
        /// Minimum time the user must wait before performing a new movement after the last
        /// </summary>
        const int _minMoveRate = 150;

        /// <summary>
        /// Minimum time the user must wait before chatting to a NPC
        /// </summary>
        const int _minNPCChatRate = 150;


        const int _minShopRate = 150;

        /// <summary>
        /// Minimum time the user must wait before picking up something
        /// </summary>
        const int _minPickupRate = 150;

        /// <summary>
        /// Minimum time the user must wait before using something
        /// </summary>
        const int _minUseRate = 150;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Pool for the damage text
        /// </summary>
        readonly DamageTextPool _damageTextPool = new DamageTextPool();

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
        ItemInfoTooltip _itemInfoTooltip;
        ShopForm _shopForm;

        /// <summary>
        /// Time when the user last attacked
        /// </summary>
        int _lastAttackTime;

        /// <summary>
        /// Time when the user last jumped
        /// </summary>
        int _lastJumpTime;

        /// <summary>
        /// Time when the user last moved to the left
        /// </summary>
        int _lastMoveLeftTime;

        /// <summary>
        /// Time when the user last moved to the right
        /// </summary>
        int _lastMoveRightTime;

        /// <summary>
        /// Time when the user last stopped moving
        /// </summary>
        int _lastMoveStopTime;

        int _lastNPCChatTime;

        int _lastShopTime;

        /// <summary>
        /// Time when the user last picked up something
        /// </summary>
        int _lastPickupTime;

        /// <summary>
        /// Time when the user last used something
        /// </summary>
        int _lastUseTime;

        /// <summary>
        /// Label used for displaying the latency.
        /// </summary>
        Label _latencyLabel;

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

        /// <summary>
        /// Gets the InfoBox used by the screen
        /// </summary>
        public InfoBox InfoBox
        {
            get { return _infoBox; }
        }

        public ItemInfoTooltip ItemInfoTooltip
        {
            get { return _itemInfoTooltip; }
        }

        public Map Map
        {
            get { return World.Map; }
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
        /// GameplayScreen constructor
        /// </summary>
        public GameplayScreen() : base(ScreenName)
        {
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
            _itemInfoTooltip.Draw(_gui.CursorPosition, _spriteBatch, _guiFont);
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
            _itemInfoTooltip = new ItemInfoTooltip(Socket);

            // Create the GUI
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

            _inventoryForm = new InventoryForm(ItemInfoTooltip, new Vector2(250, 0), cScreen);
            _shopForm = new ShopForm(new Vector2(250, 0), cScreen);

            _skillsForm = new SkillsForm(new Vector2(100, 0), cScreen);
            _skillsForm.OnUseSkill += SkillsForm_OnUseSkill;

            _infoBox = new InfoBox(GameData.ScreenSize - new Vector2(5, 5), _guiFont);

            _equippedForm = new EquippedForm(ItemInfoTooltip, new Vector2(500, 0), cScreen);
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

            _inventoryForm.Inventory = UserInfo.Inventory;
            _equippedForm.UserEquipped = UserInfo.Equipped;

            // Update some other goodies
            World.Update();
            ItemInfoTooltip.Update();
            _gui.Update(_currentTime);
            _damageTextPool.Update(_currentTime);
            UpdateInput();
            _guiSettings.Update(_currentTime);

            if (_latencyLabel != null)
                _latencyLabel.Text = string.Format(_latencyString, _socket.Latency);
        }

        void UpdateInput()
        {
            if (UserChar == null)
                return;

            KeyboardState ks = _gui.KeyboardState;

            // Jump
            if (_currentTime - _lastMoveLeftTime > _minMoveRate && UserChar.CanJump && ks.IsKeyDown(Keys.Up))
            {
                if (GameData.AllowMovementWhileChattingToNPC || !ChatDialogForm.IsChatting)
                {
                    _lastMoveLeftTime = _currentTime;
                    using (PacketWriter pw = ClientPacket.Jump())
                    {
                        Socket.Send(pw);
                    }
                }
            }

            // Move left
            if (_currentTime - _lastMoveRightTime > _minMoveRate && ks.IsKeyDown(Keys.Left) && ks.IsKeyUp(Keys.Right) &&
                !UserChar.IsMovingLeft)
            {
                if (GameData.AllowMovementWhileChattingToNPC || !ChatDialogForm.IsChatting)
                {
                    _lastMoveRightTime = _currentTime;
                    using (PacketWriter pw = ClientPacket.MoveLeft())
                    {
                        Socket.Send(pw);
                    }
                }
            }

            // Move right
            if (_currentTime - _lastJumpTime > _minMoveRate && ks.IsKeyDown(Keys.Right) && ks.IsKeyUp(Keys.Left) &&
                !UserChar.IsMovingRight)
            {
                if (GameData.AllowMovementWhileChattingToNPC || !ChatDialogForm.IsChatting)
                {
                    _lastJumpTime = _currentTime;
                    using (PacketWriter pw = ClientPacket.MoveRight())
                    {
                        Socket.Send(pw);
                    }
                }
            }

            // Attack
            if (_currentTime - _lastAttackTime > _minAttackRate && ks.IsKeyDown(Keys.LeftControl))
            {
                _lastAttackTime = _currentTime;
                using (PacketWriter pw = ClientPacket.Attack())
                {
                    Socket.Send(pw);
                }
            }

            // Stop moving
            if (_currentTime - _lastMoveStopTime > _minMoveRate && ks.IsKeyUp(Keys.Left) && ks.IsKeyUp(Keys.Right) &&
                UserChar.IsMoving)
            {
                _lastMoveStopTime = _currentTime;
                using (PacketWriter pw = ClientPacket.MoveStop())
                {
                    Socket.Send(pw);
                }
            }

            // Use world entity
            if (_currentTime - _lastUseTime > _minUseRate && ks.IsKeyDown(Keys.LeftAlt))
            {
                _lastUseTime = _currentTime;
                DynamicEntity useEntity = Map.GetEntity<DynamicEntity>(UserChar.CB.ToRectangle(), UsableEntityFilter);
                if (useEntity != null)
                {
                    using (PacketWriter pw = ClientPacket.UseWorld(useEntity.MapEntityIndex))
                    {
                        Socket.Send(pw);
                    }
                }
            }

            // Shopping
            if (_currentTime - _lastShopTime > _minShopRate && ks.IsKeyDown(Keys.LeftAlt))
            {
                _lastShopTime = _currentTime;
                CharacterEntity npc = Map.GetEntity<CharacterEntity>(UserChar.CB.ToRectangle(), x => x.HasShop);
                if (npc != null)
                {
                    using (PacketWriter pw = ClientPacket.StartShopping(npc.MapEntityIndex))
                    {
                        Socket.Send(pw);
                    }
                }
            }

            // NPC chat
            if (_currentTime - _lastNPCChatTime > _minNPCChatRate && ks.IsKeyDown(Keys.LeftAlt))
            {
                _lastNPCChatTime = _currentTime;
                CharacterEntity npc = Map.GetEntity<CharacterEntity>(UserChar.CB.ToRectangle(), x => x.HasChatDialog);
                if (npc != null)
                {
                    using (PacketWriter pw = ClientPacket.StartNPCChatDialog(npc.MapEntityIndex))
                    {
                        Socket.Send(pw);
                    }
                }
            }

            // Pick up item
            if (_currentTime - _lastPickupTime > _minPickupRate && ks.IsKeyDown(Keys.Space))
            {
                _lastPickupTime = _currentTime;

                // Check if a pickupable item could be found
                Rectangle userRect = UserChar.CB.ToRectangle();
                ItemEntity pickupItem = Map.GetEntity<ItemEntity>(userRect);
                if (pickupItem != null)
                {
                    using (PacketWriter pw = ClientPacket.PickupItem(pickupItem.MapEntityIndex))
                    {
                        Socket.Send(pw);
                    }
                }
            }
        }

        /// <summary>
        /// Filter used to find a DynamicEntity for the User to use.
        /// </summary>
        /// <param name="dynamicEntity">The User's character.</param>
        /// <returns>True if it can be used by the <paramref name="dynamicEntity"/>, else false.</returns>
        static bool UsableEntityFilter(DynamicEntity dynamicEntity)
        {
            // Make sure it is usable
            IUsableEntity asUsable = dynamicEntity as IUsableEntity;
            if (asUsable == null)
                return false;

            // Check that this DynamicEntity can use it
            return asUsable.CanUse(dynamicEntity);
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