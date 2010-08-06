using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Features.PeerTrading
{
    public abstract class PeerTradeFormBase<TChar, TItem, TItemInfo> : Form where TChar : Entity where TItem : Entity
                                                                                  where TItemInfo : class
    {
        readonly PeerTradeSidePanel _sourceSide;
        readonly PeerTradeSidePanel _targetSide;

        /// <summary>
        /// Gets the <see cref="PeerTradeSidePanel"/> for displaying the source character's side of the trade.
        /// </summary>
        public PeerTradeSidePanel SourceSide { get { return _sourceSide; } }

        /// <summary>
        /// Gets the <see cref="PeerTradeSidePanel"/> for displaying the target character's side of the trade.
        /// </summary>
        public PeerTradeSidePanel TargetSide { get { return _targetSide; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        protected PeerTradeFormBase(Control parent, Vector2 position)
            : base(parent, position, Vector2.One)
        {
            _sourceSide = new PeerTradeSidePanel(this, Vector2.Zero, true);
            _targetSide = new PeerTradeSidePanel(this, new Vector2(_sourceSide.Size.X, 0), false);
        }

        /// <summary>
        /// Delegate for handling the <see cref="ItemSlotClicked"/> event.
        /// </summary>
        /// <param name="sender">The control the event too place on.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <param name="isSourceSide">If the item slot clicked is on the source side.</param>
        /// <param name="slot">The slot that was clicked.</param>
        public delegate void ItemSlotClickedEventHandler(PeerTradeFormBase<TChar, TItem, TItemInfo> sender, SFML.Window.MouseButtonEventArgs e, bool isSourceSide, InventorySlot slot);

        /// <summary>
        /// Notifies listeners when an item slot was clicked.
        /// </summary>
        public event ItemSlotClickedEventHandler ItemSlotClicked;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        protected PeerTradeFormBase(IGUIManager guiManager, Vector2 position)
            : base(guiManager, position, Vector2.One)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of the <see cref="ItemSlotClicked"/> event.
        /// </summary>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <param name="isSourceSide">If the item slot clicked is on the source side.</param>
        /// <param name="slot">The slot that was clicked.</param>
        protected virtual void OnItemSlotClicked(SFML.Window.MouseButtonEventArgs e, bool isSourceSide, InventorySlot slot)
        {
        }

        /// <summary>
        /// The callback method for when an item slot is clicked.
        /// </summary>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <param name="isSourceSide">If the item slot clicked is on the source side.</param>
        /// <param name="slot">The slot that was clicked.</param>
        void ClickSlotCallback(SFML.Window.MouseButtonEventArgs e, bool isSourceSide, InventorySlot slot)
        {
            OnItemSlotClicked(e, isSourceSide, slot);
            if (ItemSlotClicked != null)
                ItemSlotClicked(this, e, isSourceSide, slot);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of setting up the control for an item slot.
        /// </summary>
        /// <param name="slot">The <see cref="PeerTradeSidePanel.PeerTradeItemsCollectionSlot"/> to set up.</param>
        protected virtual void SetupItemSlotControl(PeerTradeSidePanel.PeerTradeItemsCollectionSlot slot)
        {
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
        /// Gets or sets the trade information handler that will be used to display the current trade state. If null, an empty
        /// trade will be shown.
        /// </summary>
        public ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> PeerTradeInfoHandler {get; set;}

        /// <summary>
        /// When overridden in the derived class, initializes a <see cref="Grh"/> to display the information for an item.
        /// </summary>
        /// <param name="grh">The <see cref="Grh"/> to initialize (cannot be null).</param>
        /// <param name="itemInfo">The item information to be displayed. Can be null.</param>
        protected abstract void InitializeItemInfoSprite(Grh grh, TItemInfo itemInfo);

        public class PeerTradeSidePanel : Panel
        {
            readonly bool _isSourceSide;
            readonly PeerTradeFormBase<TChar, TItem, TItemInfo> _peerTradeForm;
            readonly Grh _grh = new Grh();

            /// <summary>
            /// Gets if this control handles the source character's peer trade information. If false, it handles the target character's side.
            /// </summary>
            public bool IsSourceSide { get { return _isSourceSide; } }

            /// <summary>
            /// Gets the <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}"/> that this control is on.
            /// </summary>
            public PeerTradeFormBase<TChar, TItem, TItemInfo> PeerTradeForm { get { return _peerTradeForm; } }

            /// <summary>
            /// Gets the <see cref="PeerTradeItemsCollectionSlot"/> controls in this control.
            /// </summary>
            /// <returns>The <see cref="PeerTradeItemsCollectionSlot"/> controls in this control.</returns>
            public IEnumerable<PeerTradeItemsCollectionSlot> GetItemSlotControls()
            {
                return Controls.OfType<PeerTradeItemsCollectionSlot>().ToImmutable();
            }

            /// <summary>
            /// Gets or sets the number of columns used for the item slots. Changes to not apply to existing object
            /// instances, so this value should be set early on. Default is 6.
            /// </summary>
            public static int ItemSlotColumns { get; set; }

            /// <summary>
            /// Gets or sets the size of the item slot's client area. Changes to not apply to existing object
            /// instances, so this value should be set early on. Default is {32, 32}.
            /// </summary>
            public static Vector2 ItemSlotClientSize { get; set; }

            /// <summary>
            /// Gets or sets the amount of padding between item slots. Changes to not apply to existing object
            /// instances, so this value should be set early on. Default is {2, 2}.
            /// </summary>
            public static Vector2 ItemSlotPadding { get; set; }

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
            public PeerTradeSidePanel(PeerTradeFormBase<TChar, TItem, TItemInfo> parent, Vector2 position,
                bool isSourceSide)
                : base(parent, position, Vector2.One)
            {
                _isSourceSide = isSourceSide;
                _peerTradeForm = parent;

                // Create the slots
                Vector2 slotSize = Vector2.Zero;
                Vector2 slotOffset = Vector2.Zero;

                for (int i = 0; i < PeerTradingSettings.Instance.MaxTradeSlots; i++)
                {
                    var slotControl = CreateItemSlotControl(slotOffset, ItemSlotClientSize, new InventorySlot(i));

                    // If this is the first control we created, use it to determine the size of the slot controls
                    if (slotSize == Vector2.Zero)
                        slotSize = slotControl.Size;

                    // Update the slot offset, resetting the row to the start
                    if (i > 0 && i % ItemSlotColumns == 0)
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
            /// Creates a <see cref="Control"/> for displaying a single item slot in this side of the peer trade table.
            /// </summary>
            /// <param name="position">The position of the control.</param>
            /// <param name="clientSize">The client size of the control.</param>
            /// <param name="slot">The slot that the created control will be handling.</param>
            /// <returns>A <see cref="Control"/> for displaying a single item slot in this side of the peer trade table.</returns>
            PeerTradeItemsCollectionSlot CreateItemSlotControl(Vector2 position, Vector2 clientSize, InventorySlot slot)
            {
                return new PeerTradeItemsCollectionSlot(this, position, clientSize, slot);
            }

            /// <summary>
            /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
            /// not visible.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(TickCount currentTime)
            {
                base.UpdateControl(currentTime);

                if (_grh.GrhData != null)
                    _grh.Update(currentTime);
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
            /// A control that handles a single item slot for a <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}.PeerTradeSidePanel"/>.
            /// </summary>
            public class PeerTradeItemsCollectionSlot : PictureBox
            {
                readonly PeerTradeSidePanel _itemsCollection;
                readonly Grh _sprite = new Grh();
                readonly InventorySlot _slot;

                TItemInfo _lastItemInfo;

                /// <summary>
                /// Gets the <see cref="PeerTradeSidePanel"/> that this control belongs to.
                /// </summary>
                public PeerTradeSidePanel ItemsCollection { get { return _itemsCollection; } }

                /// <summary>
                /// Gets the slot index that this control is for.
                /// </summary>
                public InventorySlot Slot { get { return _slot; } }

                /// <summary>
                /// Initializes a new instance of the
                /// <see cref="PeerTradeFormBase{TChar, TItem, TItemInfo}.PeerTradeSidePanel.PeerTradeItemsCollectionSlot"/> class.
                /// </summary>
                /// <param name="parent">The parent.</param>
                /// <param name="position">The position of the control.</param>
                /// <param name="clientSize">The client size of the control.</param>
                /// <param name="slot">The slot index that this control handles.</param>
                public PeerTradeItemsCollectionSlot(PeerTradeSidePanel parent, Vector2 position, Vector2 clientSize,
                    InventorySlot slot)
                    : base(parent, position, clientSize)
                {
                    _itemsCollection = parent;
                    _slot = slot;

                    ItemsCollection.PeerTradeForm.SetupItemSlotControl(this);
                }

                /// <summary>
                /// Handles when this <see cref="Control"/> was clicked.
                /// This is called immediately before <see cref="Control.OnClick"/>.
                /// Override this method instead of using an event hook on <see cref="Control.OnClick"/> when possible.
                /// </summary>
                /// <param name="e">The event args.</param>
                protected override void OnClick(SFML.Window.MouseButtonEventArgs e)
                {
                    base.OnClick(e);

                    // Forward the click event to the root peer trading form
                    ItemsCollection.PeerTradeForm.ClickSlotCallback(e, ItemsCollection.IsSourceSide, Slot);
                }

                /// <summary>
                /// Draws the <see cref="Control"/>.
                /// </summary>
                /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
                protected override void DrawControl(ISpriteBatch spriteBatch)
                {
                    base.DrawControl(spriteBatch);

                    // Draw the item
                    if (_sprite.GrhData != null)
                    {
                        var sp = ScreenPosition;
                        var cs = ClientSize;
                        _sprite.Draw(spriteBatch, new Rectangle((int)sp.X, (int)sp.Y, (int)cs.X, (int)cs.Y));
                    }
                }

                /// <summary>
                /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
                /// not visible.
                /// </summary>
                /// <param name="currentTime">The current time in milliseconds.</param>
                protected override void UpdateControl(TickCount currentTime)
                {
                    // Get the current item info
                    var currItemInfo = ItemsCollection.GetItemInfo(Slot);

                    // Check if the item info has changed and, if so, re-initialize the sprite
                    if (currItemInfo != _lastItemInfo)
                    {
                        ItemsCollection.PeerTradeForm.InitializeItemInfoSprite(_sprite, currItemInfo);
                        _lastItemInfo = currItemInfo;
                    }

                    base.UpdateControl(currentTime);

                    // Update the sprite
                    if (_sprite.GrhData != null)
                        _sprite.Update(currentTime);
                }
            }
        }
    }
}
