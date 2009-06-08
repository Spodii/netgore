using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Screen for the actual game
    /// </summary>
    public class GameplayScreen : GameScreen, IDisposable, IGetTime
    {
        /// <summary>
        /// Minimum time the user must wait before performing a new attack
        /// </summary>
        const int _minAttackRate = 150;

        /// <summary>
        /// Minimum time the user must wait before performing a new movement after the last
        /// </summary>
        const int _minMoveRate = 150;

        /// <summary>
        /// Minimum time the user must wait before picking up something
        /// </summary>
        const int _minPickupRate = 150;

        /// <summary>
        /// Minimum time the user must wait before using something
        /// </summary>
        const int _minUseRate = 150;

        /// <summary>
        /// Camera used to manage the view area
        /// </summary>
        readonly Camera2D _camera = new Camera2D(GameData.ScreenSize);

        /// <summary>
        /// Pool for the damage text
        /// </summary>
        readonly DamageTextPool _damageTextPool = new DamageTextPool();

        /// <summary>
        /// Skeleton templates that will remain in memory at all times
        /// </summary>
        readonly SkeletonManager _skelManager = new SkeletonManager(ContentPaths.Build.Skeletons);

        readonly UserEquipped _userEquipped = new UserEquipped();

        readonly CharacterStats _userStats = new CharacterStats();
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

        Inventory _inventory;

        InventoryForm _inventoryForm;
        ItemInfoTooltip _itemInfoTooltip;

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

        /// <summary>
        /// Time when the user last picked up something
        /// </summary>
        int _lastPickupTime;

        /// <summary>
        /// Time when the user last used something
        /// </summary>
        int _lastUseTime;

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

        /// <summary>
        /// Root world of the game
        /// </summary>
        World _world;

        /// <summary>
        /// Gets the game camera
        /// </summary>
        public Camera2D Camera
        {
            get { return _camera; }
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

        /// <summary>
        /// Gets the User's Inventory
        /// </summary>
        public Inventory Inventory
        {
            get { return _inventory; }
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

        /// <summary>
        /// Gets the user's character
        /// </summary>
        public Character UserChar
        {
            get { return World.UserChar; }
        }

        public UserEquipped UserEquipped
        {
            get { return _userEquipped; }
        }

        public CharacterStats UserStats
        {
            get { return _userStats; }
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
        public GameplayScreen(string name) : base(name)
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

        void ChatForm_OnSay(ChatForm sender, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            using (PacketWriter pw = ClientPacket.Say(text))
            {
                Socket.Send(pw);
            }
        }

        protected virtual void Dispose(bool disposeManaged)
        {
            if (_disposed)
                return;

            _disposed = true;
            if (!disposeManaged)
                return;

            if (_guiSettings != null)
                _guiSettings.Dispose();

            if (Socket != null)
            {
                Socket.Disconnect();
                Socket.Dispose();
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime.</param>
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
            _spriteBatch.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, _camera.Matrix);
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

        public override void Initialize()
        {
            _world = new World(this, _camera);

            // Create the socket
            _socket = new ClientSockets(this);
            Socket.OnDisconnect += OnDisconnect;

            // Create some misc goodies that require a reference to the Socket
            _itemInfoTooltip = new ItemInfoTooltip(Socket);
            _inventory = new Inventory(Socket);

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

            Panel cScreen = new Panel(_gui, Vector2.Zero, ScreenManager.ScreenSize) { CanFocus = false };
            _statsForm = new StatsForm(_userStats, cScreen);
            _statsForm.OnRaiseStat += StatsForm_OnRaiseStat;

            _inventoryForm = new InventoryForm(ItemInfoTooltip, new Vector2(250, 0), cScreen);
            _infoBox = new InfoBox(GameData.ScreenSize - new Vector2(5, 5), _guiFont);

            _equippedForm = new EquippedForm(ItemInfoTooltip, new Vector2(500, 0), cScreen);
            _equippedForm.OnRequestUnequip += EquippedForm_OnRequestUnequip;

            _chatForm = new ChatForm(cScreen, new Vector2(0, cScreen.Size.Y));
            _chatForm.OnSay += ChatForm_OnSay;

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
        /// Load graphics content for the game
        /// </summary>
        public override void LoadContent()
        {
            _spriteBatch = ScreenManager.SpriteBatch;
            _damageFont = ScreenManager.Content.Load<SpriteFont>("Font/Game");
        }

        void OnDisconnect(IIPSocket conn)
        {
            ScreenManager.SetScreen("login");
            LoginScreen login = (LoginScreen)ScreenManager.GetScreen("login");
            login.SetError("Connection to server lost.");
            Dispose();
        }

        /// <summary>
        /// Handles when a stat is requested to be raised.
        /// </summary>
        /// <param name="statsForm">StatsForm that the event came from.</param>
        /// <param name="statType">StatType requested to be raised.</param>
        void StatsForm_OnRaiseStat(StatsForm statsForm, StatType statType)
        {
            Socket.Send(ClientPacket.RaiseStat(statType));
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
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Get the current time
            _currentTime = (int)gameTime.TotalRealTime.TotalMilliseconds;

            // Update the network
            Socket.Heartbeat();

            if (UserChar == null)
                return;

            _inventoryForm.Inventory = Inventory;
            _equippedForm.UserEquipped = UserEquipped;

            // Update some other goodies
            World.Update();
            ItemInfoTooltip.Update();
            _gui.Update(_currentTime);
            _damageTextPool.Update(_currentTime);
            UpdateInput();
            _guiSettings.Update(_currentTime);

            // Update the camera
            _camera.Min = UserChar.GetCameraPos();
            _camera.Map = Map;
        }

        void UpdateInput()
        {
            if (UserChar == null)
                return;

            KeyboardState ks = _gui.KeyboardState;

            // Jump
            if (_currentTime - _lastMoveLeftTime > _minMoveRate && UserChar.CanJump && ks.IsKeyDown(Keys.Up))
            {
                _lastMoveLeftTime = _currentTime;
                using (PacketWriter pw = ClientPacket.Jump())
                {
                    Socket.Send(pw);
                }
            }

            // Move left
            if (_currentTime - _lastMoveRightTime > _minMoveRate && ks.IsKeyDown(Keys.Left) && ks.IsKeyUp(Keys.Right) &&
                !UserChar.IsMovingLeft)
            {
                _lastMoveRightTime = _currentTime;
                using (PacketWriter pw = ClientPacket.MoveLeft())
                {
                    Socket.Send(pw);
                }
            }

            // Move right
            if (_currentTime - _lastJumpTime > _minMoveRate && ks.IsKeyDown(Keys.Right) && ks.IsKeyUp(Keys.Left) &&
                !UserChar.IsMovingRight)
            {
                _lastJumpTime = _currentTime;
                using (PacketWriter pw = ClientPacket.MoveRight())
                {
                    Socket.Send(pw);
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
                // Check if a useable entity could be found
                if (Map.GetUseable(UserChar.CB, UserChar) != null)
                {
                    using (PacketWriter pw = ClientPacket.UseWorld())
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
                ItemEntity pickupItem = Map.GetEntity<ItemEntity>(userRect, item => item.CanPickup(UserChar));

                if (pickupItem != null)
                {
                    using (PacketWriter pw = ClientPacket.PickupItem((ushort)pickupItem.MapIndex))
                    {
                        Socket.Send(pw);
                    }
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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