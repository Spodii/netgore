using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.World;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Features.PeerTrading
{
    /// <summary>
    /// The base class for a <see cref="Form"/> control that displays an active peer-to-peer trade session.
    /// </summary>
    /// <typeparam name="TChar">The type of character.</typeparam>
    /// <typeparam name="TItem">The type of item.</typeparam>
    /// <typeparam name="TItemInfo">The type describing item information.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public abstract class PeerTradeFormBase<TChar, TItem, TItemInfo> : Form
        where TChar : Entity where TItem : Entity where TItemInfo : class
    {
        readonly Button _acceptButton;

        readonly PeerTradeSidePanel _sourceSide;
        readonly PeerTradeSidePanel _targetSide;
        ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> _peerTradeInfoHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        protected PeerTradeFormBase(Control parent, Vector2 position) : base(parent, position, Vector2.One)
        {
            _acceptButton = new Button(this, Vector2.Zero, new Vector2(4, 4));
            _acceptButton.TextChanged += AcceptButton_TextChanged;
            _acceptButton.Text = "Accept";

            var pad = new Vector2(ResizeToChildrenPadding);
            _sourceSide = CreateTradeSidePanel(this, pad, true);
            _targetSide = CreateTradeSidePanel(this, pad + new Vector2(SourceSide.Size.X + SourceSide.Border.Width + 2, 0), false);

            AcceptButton.Position = ClientSize - new Vector2(AcceptButton.Size.X, 0);
            AcceptButton.Clicked += AcceptButton_Clicked;

            SetControlPositions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        protected PeerTradeFormBase(IGUIManager guiManager, Vector2 position) : base(guiManager, position, Vector2.One)
        {
        }

        /// <summary>
        /// Notifies listeners when an item slot was clicked.
        /// </summary>
        public event TypedEventHandler<PeerTradeFormBase<TChar, TItem, TItemInfo>, ItemSlotClickedEventArgs> ItemSlotClicked;

        /// <summary>
        /// Notifies listeners when the <see cref="PeerTradeInfoHandler"/> property has changed.
        /// </summary>
        public event
            TypedEventHandler
                <PeerTradeFormBase<TChar, TItem, TItemInfo>, PeerTradeInfoHandlerChangedEventArgs<TChar, TItem, TItemInfo>>
            PeerTradeInfoHandlerChanged;

        /// <summary>
        /// Gets the <see cref="Button"/> used to accept the trade.
        /// </summary>
        public Button AcceptButton
        {
            get { return _acceptButton; }
        }

        /// <summary>
        /// Gets or sets the trade information handler that will be used to display the current trade state. If null, an empty
        /// trade will be shown.
        /// </summary>
        public ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> PeerTradeInfoHandler
        {
            get { return _peerTradeInfoHandler; }
            set
            {
                if (_peerTradeInfoHandler == value)
                    return;

                var oldValue = _peerTradeInfoHandler;
                _peerTradeInfoHandler = value;

                // Raise the event
                OnPeerTradeInfoHandlerChanged(oldValue, value);
                if (PeerTradeInfoHandlerChanged != null)
                    PeerTradeInfoHandlerChanged.Raise(this, PeerTradeInfoHandlerChangedEventArgs.Create(oldValue, value));
            }
        }

        /// <summary>
        /// Gets the <see cref="PeerTradeSidePanel"/> for displaying the source character's side of the trade.
        /// </summary>
        public PeerTradeSidePanel SourceSide
        {
            get { return _sourceSide; }
        }

        /// <summary>
        /// Gets the <see cref="PeerTradeSidePanel"/> for displaying the target character's side of the trade.
        /// </summary>
        public PeerTradeSidePanel TargetSide
        {
            get { return _targetSide; }
        }

        /// <summary>
        /// Handles the Clicked event of the AcceptButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void AcceptButton_Clicked(object sender, MouseButtonEventArgs e)
        {
            var ptih = PeerTradeInfoHandler;
            if (ptih == null)
                return;

            if (!ptih.HasUserAccepted)
                ptih.WriteAccept();
        }

        /// <summary>
        /// Handles the TextChanged event of the AcceptButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void AcceptButton_TextChanged(Control sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            var measureText = string.IsNullOrEmpty(button.Text) ? " " : button.Text;
            button.ClientSize = button.Font.MeasureString(measureText);
        }

        /// <summary>
        /// The callback method for when an item slot is clicked.
        /// </summary>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <param name="isSourceSide">If the item slot clicked is on the source side.</param>
        /// <param name="slot">The slot that was clicked.</param>
        void ClickSlotCallback(MouseButtonEventArgs e, bool isSourceSide, InventorySlot slot)
        {
            OnItemSlotClicked(e, isSourceSide, slot);
            if (ItemSlotClicked != null)
                ItemSlotClicked.Raise(this, new ItemSlotClickedEventArgs(e, isSourceSide, slot));
        }

        /// <summary>
        /// Handles when the Close button on the form is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void CloseButtonClicked(object sender, MouseButtonEventArgs e)
        {
            // Instead of closing when clicked, make a request to the server to cancel the trade
            PeerTradeInfoHandler.WriteCancel();
        }

        /// <summary>
        /// Creates a <see cref="PeerTradeSidePanel"/> instance.
        /// </summary>
        /// <param name="parent">The parent control.</param>
        /// <param name="position">The position to place the control.</param>
        /// <param name="isSourceSide">If this panel is for the source trade character side.</param>
        /// <returns>The <see cref="PeerTradeSidePanel"/> instance.</returns>
        protected virtual PeerTradeSidePanel CreateTradeSidePanel(PeerTradeFormBase<TChar, TItem, TItemInfo> parent,
                                                                  Vector2 position, bool isSourceSide)
        {
            return new PeerTradeSidePanel(parent, position, isSourceSide);
        }

        /// <summary>
        /// When overridden in the derived class, gets the item quantity value from the item information.
        /// </summary>
        /// <param name="itemInfo">The item information to get the quantity value for.</param>
        /// <returns>The quantity value for the <paramref name="itemInfo"/>.</returns>
        protected abstract int GetItemAmount(TItemInfo itemInfo);

        /// <summary>
        /// When overridden in the derived class, initializes a <see cref="Grh"/> to display the information for an item.
        /// </summary>
        /// <param name="grh">The <see cref="Grh"/> to initialize (cannot be null).</param>
        /// <param name="itemInfo">The item information to be displayed. Can be null.</param>
        protected abstract void InitializeItemInfoSprite(Grh grh, TItemInfo itemInfo);

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public override void LoadSkin(ISkinManager skinManager)
        {
            base.LoadSkin(skinManager);

            if (SourceSide != null && TargetSide != null && AcceptButton != null)
                SetControlPositions();
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of the <see cref="ItemSlotClicked"/> event.
        /// </summary>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <param name="isSourceSide">If the item slot clicked is on the source side.</param>
        /// <param name="slot">The slot that was clicked.</param>
        protected virtual void OnItemSlotClicked(MouseButtonEventArgs e, bool isSourceSide, InventorySlot slot)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the PeerTradeInfoHandler property's value changes.
        /// </summary>
        /// <param name="oldHandler">The old (last) peer trade information handler. Can be null.</param>
        /// <param name="newHandler">The new (current) peer trade information handler. Can be null.</param>
        protected virtual void OnPeerTradeInfoHandlerChanged(ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> oldHandler,
                                                             ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> newHandler)
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected override void OnResized()
        {
            base.OnResized();

            if (SourceSide != null && TargetSide != null && AcceptButton != null)
                SetControlPositions();
        }

        /// <summary>
        /// When overridden in the derived class, sets the default child control positions.
        /// </summary>
        protected virtual void SetControlPositions()
        {
            var pad = new Vector2(ResizeToChildrenPadding);
            SourceSide.Position = pad;
            TargetSide.Position = pad + new Vector2(SourceSide.Size.X + pad.X + 2, 0);
            AcceptButton.Position = TargetSide.Position + TargetSide.Size - new Vector2(AcceptButton.Size.X, -pad.Y);
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            ResizeToChildren = true;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of setting up the control for an item slot.
        /// </summary>
        /// <param name="slot">The <see cref="PeerTradeSidePanel.PeerTradeItemsCollectionSlot"/> to set up.</param>
        protected virtual void SetupItemSlotControl(PeerTradeSidePanel.PeerTradeItemsCollectionSlot slot)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of setting up the control that holds all the
        /// controls for one side of a peer trade.
        /// </summary>
        /// <param name="tradePanel">The <see cref="PeerTradeSidePanel"/> to set up.</param>
        protected virtual void SetupTradePanelControl(PeerTradeSidePanel tradePanel)
        {
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(TickCount currentTime)
        {
            base.UpdateControl(currentTime);

            if (!IsVisible)
                return;

            var ptih = PeerTradeInfoHandler;
            if (ptih == null)
                return;

            AcceptButton.IsEnabled = !ptih.HasUserAccepted;
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public class PeerTradeSidePanel : Panel
        {
            readonly Label _acceptedLabel;
            readonly Grh _grh = new Grh();
            readonly bool _isSourceSide;
            readonly PeerTradeFormBase<TChar, TItem, TItemInfo> _peerTradeForm;
            readonly Label _title;

            bool _acceptLabelStatus = false;

            /// <summary>
            /// Initializes the <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}.PeerTradeSidePanel"/> class.
            /// </summary>
            static PeerTradeSidePanel()
            {
                ItemSlotColumns = 6;
                ItemSlotClientSize = new Vector2(32);
                ItemSlotPadding = new Vector2(2);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="PeerTradeSidePanel"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="position">The position.</param>
            /// <param name="isSourceSide">If this control handles the source character's side.</param>
            public PeerTradeSidePanel(PeerTradeFormBase<TChar, TItem, TItemInfo> parent, Vector2 position, bool isSourceSide)
                : base(parent, position, Vector2.One)
            {
                _isSourceSide = isSourceSide;
                _peerTradeForm = parent;

                ResizeToChildren = true;

                // Create the title label
                _title = new Label(this, Vector2.Zero) { Text = (isSourceSide ? "Source" : "Target"), AutoResize = false };
                _title.ClientSize = new Vector2(_title.ClientSize.X, _title.Font.DefaultSize);

                // Create the slots
                var slotSize = Vector2.Zero;
                var slotOffset = new Vector2(0, Title.Size.Y + 4);

                for (var i = 0; i < PeerTradingSettings.Instance.MaxTradeSlots; i++)
                {
                    var slotControl = CreateItemSlotControl(slotOffset, ItemSlotClientSize, new InventorySlot(i));

                    // If this is the first control we created, use it to determine the size of the slot controls
                    if (slotSize == Vector2.Zero)
                        slotSize = slotControl.Size;

                    // Update the slot offset, resetting the row to the start
                    if ((i > 0) && ((i + 1) % ItemSlotColumns == 0))
                    {
                        // Start new row below
                        slotOffset.X = 0;
                        slotOffset.Y += slotSize.Y + ItemSlotPadding.Y;
                    }
                    else
                    {
                        // Move to the right on the existing row
                        slotOffset.X += slotSize.X + ItemSlotPadding.X;
                    }
                }

                // Create the accept label
                _acceptedLabel = new Label(this, new Vector2(0, ClientSize.Y)) { Text = "Not Accepted", AutoResize = false };
                AcceptedLabel.ClientSize = new Vector2(AcceptedLabel.ClientSize.X, AcceptedLabel.Font.DefaultSize);
                AcceptedLabel.Position = new Vector2(ClientSize.X - AcceptedLabel.ClientSize.X, ClientSize.Y);

                // Create the cash label
                CreateCashLabel(new Vector2(0, AcceptedLabel.Position.Y));

                PeerTradeForm.SetupTradePanelControl(this);
            }

            /// <summary>
            /// Gets the <see cref="Label"/> used to display the trade acceptance status.
            /// </summary>
            public Label AcceptedLabel
            {
                get { return _acceptedLabel; }
            }

            /// <summary>
            /// Gets if this control handles the source character's peer trade information. If false, it handles the target character's side.
            /// </summary>
            public bool IsSourceSide
            {
                get { return _isSourceSide; }
            }

            /// <summary>
            /// Gets or sets the size of the item slot's client area. Changes to not apply to existing object
            /// instances, so this value should be set early on. Default is {32, 32}.
            /// </summary>
            [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
            public static Vector2 ItemSlotClientSize { get; set; }

            /// <summary>
            /// Gets or sets the number of columns used for the item slots. Changes to not apply to existing object
            /// instances, so this value should be set early on. Default is 6.
            /// </summary>
            [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
            public static int ItemSlotColumns { get; set; }

            /// <summary>
            /// Gets or sets the amount of padding between item slots. Changes to not apply to existing object
            /// instances, so this value should be set early on. Default is {2, 2}.
            /// </summary>
            [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
            public static Vector2 ItemSlotPadding { get; set; }

            /// <summary>
            /// Gets the <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}"/> that this control is on.
            /// </summary>
            public PeerTradeFormBase<TChar, TItem, TItemInfo> PeerTradeForm
            {
                get { return _peerTradeForm; }
            }

            /// <summary>
            /// Gets the <see cref="Label"/> control used to display the title.
            /// </summary>
            public Label Title
            {
                get { return _title; }
            }

            /// <summary>
            /// Changes the status displayed by the accept label.
            /// </summary>
            /// <param name="status">The trade acceptance status.</param>
            void ChangeAcceptLabelStatus(bool status)
            {
                if (status == _acceptLabelStatus)
                    return;

                _acceptLabelStatus = status;

                if (status)
                    _acceptedLabel.Text = "Accepted";
                else
                    _acceptedLabel.Text = "Not Accepted";
            }

            /// <summary>
            /// Creates a <see cref="CashLabel"/> for displaying the cash amount in this side of the peer trade table.
            /// </summary>
            /// <param name="position">The position of the control.</param>
            /// <returns>A <see cref="CashLabel"/> for displaying the cash amount in this side of the peer trade table.</returns>
            protected virtual CashLabel CreateCashLabel(Vector2 position)
            {
                var ret = new CashLabel(this, position);
                return ret;
            }

            /// <summary>
            /// Creates a <see cref="Control"/> for displaying a single item slot in this side of the peer trade table.
            /// </summary>
            /// <param name="position">The position of the control.</param>
            /// <param name="clientSize">The client size of the control.</param>
            /// <param name="slot">The slot that the created control will be handling.</param>
            /// <returns>A <see cref="Control"/> for displaying a single item slot in this side of the peer trade table.</returns>
            protected virtual PeerTradeItemsCollectionSlot CreateItemSlotControl(Vector2 position, Vector2 clientSize,
                                                                                 InventorySlot slot)
            {
                return new PeerTradeItemsCollectionSlot(this, position, clientSize, slot);
            }

            /// <summary>
            /// Gets the information for an item in the given slot.
            /// </summary>
            /// <param name="slot">The slot of the item to get the information for.</param>
            /// <returns>The item information for the item in the given slot, or null if the slot is empty or invalid.</returns>
            protected virtual TItemInfo GetItemInfo(InventorySlot slot)
            {
                var ptih = PeerTradeForm.PeerTradeInfoHandler;
                if (ptih == null)
                    return null;

                if (IsSourceSide)
                    return ptih.GetSourceItemInfo(slot);
                else
                    return ptih.GetTargetItemInfo(slot);
            }

            /// <summary>
            /// Gets the <see cref="PeerTradeItemsCollectionSlot"/> controls in this control.
            /// </summary>
            /// <returns>The <see cref="PeerTradeItemsCollectionSlot"/> controls in this control.</returns>
            public IEnumerable<PeerTradeItemsCollectionSlot> GetItemSlotControls()
            {
                return Controls.OfType<PeerTradeItemsCollectionSlot>().ToImmutable();
            }

            /// <summary>
            /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
            /// base class's method to ensure that changes to settings are hierchical.
            /// </summary>
            protected override void SetDefaultValues()
            {
                base.SetDefaultValues();

                ResizeToChildren = true;
            }

            /// <summary>
            /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
            /// not visible.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(TickCount currentTime)
            {
                base.UpdateControl(currentTime);

                if (!IsVisible)
                    return;

                if (_grh.GrhData != null)
                    _grh.Update(currentTime);

                var ptih = PeerTradeForm.PeerTradeInfoHandler;
                if (ptih != null)
                    ChangeAcceptLabelStatus(IsSourceSide ? ptih.HasSourceAccepted : ptih.HasTargetAccepted);
            }

            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public class CashLabel : Label
            {
                readonly PeerTradeSidePanel _itemsCollection;

                string _caption = "Cash";
                int _lastCash = 0;

                /// <summary>
                /// Initializes a new instance of the <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}.PeerTradeSidePanel.CashLabel"/> class.
                /// </summary>
                /// <param name="parent">The parent.</param>
                /// <param name="position">The position of the control.</param>
                public CashLabel(PeerTradeSidePanel parent, Vector2 position) : base(parent, position)
                {
                    _itemsCollection = parent;
                    Text = GetDisplayText(Caption, Cash);
                }

                /// <summary>
                /// Gets or sets the caption of the control, which is the text displayed next to the actual cash amount displayed.
                /// </summary>
                public string Caption
                {
                    get { return _caption; }
                    set
                    {
                        if (value == null)
                            value = string.Empty;

                        if (_caption == value)
                            return;

                        _caption = value;

                        Text = GetDisplayText(Caption, Cash);
                    }
                }

                /// <summary>
                /// Gets the current amount of cash for this side of the trade.
                /// </summary>
                int Cash
                {
                    get
                    {
                        var isSourceSide = ItemsCollection.IsSourceSide;
                        var ptih = ItemsCollection.PeerTradeForm.PeerTradeInfoHandler;
                        if (ptih == null)
                            return 0;

                        if (isSourceSide)
                            return ptih.SourceCash;
                        else
                            return ptih.TargetCash;
                    }
                }

                /// <summary>
                /// Gets the <see cref="PeerTradeSidePanel"/> that this control belongs to.
                /// </summary>
                public PeerTradeSidePanel ItemsCollection
                {
                    get { return _itemsCollection; }
                }

                /// <summary>
                /// Gets the text to display on the control.
                /// </summary>
                /// <param name="caption">The caption text.</param>
                /// <param name="cash">The amount of cash.</param>
                /// <returns>The text to display on the control.</returns>
                protected virtual string GetDisplayText(string caption, int cash)
                {
                    return caption + ": " + cash;
                }

                /// <summary>
                /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
                /// base class's method to ensure that changes to settings are hierchical.
                /// </summary>
                protected override void SetDefaultValues()
                {
                    base.SetDefaultValues();

                    Caption = "Cash";
                    IncludeInResizeToChildren = false;
                }

                /// <summary>
                /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
                /// not visible.
                /// </summary>
                /// <param name="currentTime">The current time in milliseconds.</param>
                protected override void UpdateControl(TickCount currentTime)
                {
                    base.UpdateControl(currentTime);

                    if (!IsVisible)
                        return;

                    // Update the text if the cash amount has changed
                    var newCash = Cash;
                    if (newCash == _lastCash)
                        return;

                    _lastCash = newCash;
                    Text = GetDisplayText(Caption, newCash);
                }
            }

            /// <summary>
            /// A control that handles a single item slot for a <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}.PeerTradeSidePanel"/>.
            /// </summary>
            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public class PeerTradeItemsCollectionSlot : PictureBox
            {
                readonly PeerTradeSidePanel _itemsCollection;
                readonly InventorySlot _slot;
                readonly Grh _sprite = new Grh();

                TItemInfo _lastItemInfo;

                /// <summary>
                /// Initializes a new instance of the
                /// <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}.PeerTradeSidePanel.PeerTradeItemsCollectionSlot"/> class.
                /// </summary>
                /// <param name="parent">The parent.</param>
                /// <param name="position">The position of the control.</param>
                /// <param name="clientSize">The client size of the control.</param>
                /// <param name="slot">The slot index that this control handles.</param>
                public PeerTradeItemsCollectionSlot(PeerTradeSidePanel parent, Vector2 position, Vector2 clientSize,
                                                    InventorySlot slot) : base(parent, position, clientSize)
                {
                    _itemsCollection = parent;
                    _slot = slot;

                    ItemsCollection.PeerTradeForm.SetupItemSlotControl(this);
                }

                /// <summary>
                /// Gets the <see cref="PeerTradeSidePanel"/> that this control belongs to.
                /// </summary>
                public PeerTradeSidePanel ItemsCollection
                {
                    get { return _itemsCollection; }
                }

                /// <summary>
                /// Gets the slot index that this control is for.
                /// </summary>
                public InventorySlot Slot
                {
                    get { return _slot; }
                }

                /// <summary>
                /// Draws the <see cref="Control"/>.
                /// </summary>
                /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
                protected override void DrawControl(ISpriteBatch spriteBatch)
                {
                    base.DrawControl(spriteBatch);
                    DrawItem(spriteBatch, _sprite);
                }

                /// <summary>
                /// Draws the item in this slot. This method is always called, even when the slot is empty.
                /// </summary>
                /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
                /// <param name="itemSprite">The <see cref="Grh"/> to draw. Will never be null, but can contain
                /// an invalid or empty sprite.</param>
                protected virtual void DrawItem(ISpriteBatch spriteBatch, Grh itemSprite)
                {
                    if (itemSprite.GrhData == null)
                        return;

                    // Grab the screen position and client size
                    var sp = ScreenPosition;
                    var cs = ClientSize;

                    // Get the size to use for drawing (never exceeding the size of the control)
                    var drawSize = Vector2.Min(cs, itemSprite.Size);

                    // Get the draw position (centering on the control)
                    var drawPos = sp + ((cs - drawSize) / 2f);

                    // Draw
                    var spriteDestRect = new Rectangle(drawPos.X, drawPos.Y, drawSize.X, drawSize.Y);
                    _sprite.Draw(spriteBatch, spriteDestRect);

                    // Draw the amount
                    var itemInfo = GetSlotItemInfo();
                    if (itemInfo == null)
                        return;

                    var amount = ItemsCollection.PeerTradeForm.GetItemAmount(itemInfo);
                    DrawItemAmount(spriteBatch, amount);
                }

                /// <summary>
                /// Draws the item amount string for this slot.
                /// </summary>
                /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
                /// <param name="amount">The amount value to draw.</param>
                protected virtual void DrawItemAmount(ISpriteBatch spriteBatch, int amount)
                {
                    if (amount <= 1)
                        return;

                    var font = GetItemAmountFont();
                    var foreColor = GetItemAmountFontForeColor();
                    var backColor = GetItemAmountFontShadowColor();
                    spriteBatch.DrawStringShaded(font, amount.ToString(), ScreenPosition, foreColor, backColor);
                }

                /// <summary>
                /// Gets the <see cref="Font"/> to use for drawing the item amount.
                /// </summary>
                /// <returns>The <see cref="Font"/> to use for drawing the item amount.</returns>
                protected virtual Font GetItemAmountFont()
                {
                    return ItemsCollection.PeerTradeForm.Font;
                }

                /// <summary>
                /// Gets the foreground color to use for drawing the item amount text.
                /// </summary>
                /// <returns>The foreground color to use for drawing the item amount text.</returns>
                protected virtual Color GetItemAmountFontForeColor()
                {
                    return Color.White;
                }

                /// <summary>
                /// Gets the shadow color to use for drawing the item amount text.
                /// </summary>
                /// <returns>The shadow color to use for drawing the item amount text.</returns>
                protected virtual Color GetItemAmountFontShadowColor()
                {
                    return Color.Black;
                }

                /// <summary>
                /// Gets the item information for the item in this slot.
                /// </summary>
                /// <returns>The item information for the item in this slot, or null if the slot is empty.</returns>
                protected TItemInfo GetSlotItemInfo()
                {
                    return ItemsCollection.GetItemInfo(Slot);
                }

                /// <summary>
                /// Handles when this <see cref="Control"/> was clicked.
                /// This is called immediately before <see cref="Control.OnClick"/>.
                /// Override this method instead of using an event hook on <see cref="Control.OnClick"/> when possible.
                /// </summary>
                /// <param name="e">The event args.</param>
                protected override void OnClick(MouseButtonEventArgs e)
                {
                    base.OnClick(e);

                    // Forward the click event to the root peer trading form
                    ItemsCollection.PeerTradeForm.ClickSlotCallback(e, ItemsCollection.IsSourceSide, Slot);
                }

                /// <summary>
                /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
                /// not visible.
                /// </summary>
                /// <param name="currentTime">The current time in milliseconds.</param>
                protected override void UpdateControl(TickCount currentTime)
                {
                    base.UpdateControl(currentTime);

                    if (!IsVisible)
                        return;

                    // Get the current item info
                    var currItemInfo = ItemsCollection.GetItemInfo(Slot);

                    // Check if the item info has changed and, if so, re-initialize the sprite
                    if (currItemInfo != _lastItemInfo)
                    {
                        ItemsCollection.PeerTradeForm.InitializeItemInfoSprite(_sprite, currItemInfo);
                        _lastItemInfo = currItemInfo;
                    }

                    // Update the sprite
                    if (_sprite.GrhData != null)
                        _sprite.Update(currentTime);
                }
            }
        }
    }
}