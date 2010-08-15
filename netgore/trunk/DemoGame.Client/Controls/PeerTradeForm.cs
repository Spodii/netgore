using System;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Features.PeerTrading;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    public class PeerTradeForm : PeerTradeFormBase<Character, ItemEntity, IItemTable>, IDragDropProvider
    {
        public PeerTradeForm(Control parent, Vector2 position) : base(parent, position)
        {
        }

        public PeerTradeForm(IGUIManager guiManager, Vector2 position) : base(guiManager, position)
        {
        }

        /// <summary>
        /// Gets if there is currently an active, valid trade going on right now.
        /// </summary>
        public bool IsTradeActive
        {
            get
            {
                var ptih = PeerTradeInfoHandler;
                if (ptih == null || !ptih.IsTradeOpen)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Creates a <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.PeerTradeSidePanel"/> instance.
        /// </summary>
        /// <param name="parent">The parent control.</param>
        /// <param name="position">The position to place the control.</param>
        /// <param name="isSourceSide">If this panel is for the source trade character side.</param>
        /// <returns>The <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.PeerTradeSidePanel"/> instance.</returns>
        protected override PeerTradeSidePanel CreateTradeSidePanel(PeerTradeFormBase<Character, ItemEntity, IItemTable> parent,
                                                                   Vector2 position, bool isSourceSide)
        {
            return new CustomSidePanel(parent, position, isSourceSide);
        }

        /// <summary>
        /// When overridden in the derived class, initializes a <see cref="Grh"/> to display the information for an item.
        /// </summary>
        /// <param name="grh">The <see cref="Grh"/> to initialize (cannot be null).</param>
        /// <param name="itemInfo">The item information to be displayed. Can be null.</param>
        protected override void InitializeItemInfoSprite(Grh grh, IItemTable itemInfo)
        {
            if (itemInfo == null)
            {
                // Clear the sprite
                grh.SetGrh(null);
            }
            else
            {
                // Set the new sprite
                var newGD = GrhInfo.GetData(itemInfo.Graphic);
                grh.SetGrh(newGD);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of the <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.ItemSlotClicked"/> event.
        /// </summary>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <param name="isSourceSide">If the item slot clicked is on the source side.</param>
        /// <param name="slot">The slot that was clicked.</param>
        protected override void OnItemSlotClicked(MouseButtonEventArgs e, bool isSourceSide, InventorySlot slot)
        {
            base.OnItemSlotClicked(e, isSourceSide, slot);

            var ptih = PeerTradeInfoHandler;
            if (ptih == null || !ptih.IsTradeOpen)
                return;

            // Check if the slot clicked was a slot on our side
            var clickWasOnOurSide = (isSourceSide && ptih.UserIsSource) || (!isSourceSide && !ptih.UserIsSource);

            // If the slot clicked was a slot on our side, and it contains an item, remove the item
            if (clickWasOnOurSide)
            {
                if (ptih.GetUserItemInfo(slot) != null)
                {
                    ptih.WriteRemoveItem(slot);
                    return;
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the item quantity value from the item information.
        /// </summary>
        /// <param name="itemInfo">The item information to get the quantity value for.</param>
        /// <returns>The quantity value for the <paramref name="itemInfo"/>.</returns>
        protected override int GetItemAmount(IItemTable itemInfo)
        {
            return itemInfo.Amount;
        }

        /// <summary>
        /// When overridden in the derived class, handles when the PeerTradeInfoHandler property's value changes.
        /// </summary>
        /// <param name="oldHandler">The old (last) peer trade information handler. Can be null.</param>
        /// <param name="newHandler">The new (current) peer trade information handler. Can be null.</param>
        protected override void OnPeerTradeInfoHandlerChanged(
            ClientPeerTradeInfoHandlerBase<Character, ItemEntity, IItemTable> oldHandler,
            ClientPeerTradeInfoHandlerBase<Character, ItemEntity, IItemTable> newHandler)
        {
            base.OnPeerTradeInfoHandlerChanged(oldHandler, newHandler);

            // Remove the event hooks from the old handler
            if (oldHandler != null)
                oldHandler.TradeOpened -= PeerTradeInfoHandler_TradeOpened;

            // Set the event hooks onto the new handler
            if (newHandler != null)
                newHandler.TradeOpened += PeerTradeInfoHandler_TradeOpened;
        }

        /// <summary>
        /// Handles the <see cref="ClientPeerTradeInfoHandlerBase{Character, ItemEntity, IItemTable}.TradeOpened"/> event on a
        /// <see cref="ClientPeerTradeInfoHandlerBase{Character, ItemEntity, IItemTable}"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void PeerTradeInfoHandler_TradeOpened(ClientPeerTradeInfoHandlerBase<Character, ItemEntity, IItemTable> sender)
        {
            // Figure out which side is for the client, and which side is for the other character
            PeerTradeSidePanel ourSide;
            PeerTradeSidePanel theirSide;

            if (sender.UserIsSource)
            {
                ourSide = SourceSide;
                theirSide = TargetSide;
            }
            else
            {
                ourSide = TargetSide;
                theirSide = SourceSide;
            }

            // Set up the display
            ourSide.Title.Text = "(You)";
            theirSide.Title.Text = sender.OtherCharName;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of setting up the control for an item slot.
        /// </summary>
        /// <param name="slot">The <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.PeerTradeSidePanel.PeerTradeItemsCollectionSlot"/> to set up.</param>
        protected override void SetupItemSlotControl(PeerTradeSidePanel.PeerTradeItemsCollectionSlot slot)
        {
            base.SetupItemSlotControl(slot);

            slot.Sprite = GUIManager.SkinManager.GetSprite("item_slot");
            slot.Tooltip = SlotTooltipCallback;
        }

        /// <summary>
        /// Handles the <see cref="TooltipHandler"/> callback for an item slot in the trade form.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="TooltipArgs"/>.</param>
        /// <returns>The text to display for the tooltip.</returns>
        StyledText[] SlotTooltipCallback(Control sender, TooltipArgs args)
        {
            // Cast the sender to the needed type
            var asItemSlot = sender as PeerTradeSidePanel.PeerTradeItemsCollectionSlot;
            if (asItemSlot == null)
                return null;
            
            // Ensure a valid trade state
            var ptih = PeerTradeInfoHandler;
            if (ptih == null || !ptih.IsTradeOpen)
                return null;

            // Get the item info for the slot
            IItemTable itemInfo;
            if (asItemSlot.ItemsCollection.IsSourceSide)
                itemInfo= ptih.GetSourceItemInfo(asItemSlot.Slot);
            else
                itemInfo = ptih.GetTargetItemInfo(asItemSlot.Slot);

            if (itemInfo == null)
                return null;

            // Get and return the tooltip text
            return ItemInfoHelper.GetStyledText(itemInfo);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of setting up the control that holds all the
        /// controls for one side of a peer trade.
        /// </summary>
        /// <param name="tradePanel">The <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.PeerTradeSidePanel"/> to set up.</param>
        protected override void SetupTradePanelControl(PeerTradeSidePanel tradePanel)
        {
            base.SetupTradePanelControl(tradePanel);

            tradePanel.Border = GUIManager.SkinManager.GetBorder("TextBox");
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(TickCount currentTime)
        {
            base.UpdateControl(currentTime);

            // Always show when a trade is active
            IsVisible = IsTradeActive;
        }

        /// <summary>
        /// A custom implementation of the <see cref="PeerTradeSidePanel"/>.
        /// </summary>
        class CustomSidePanel : PeerTradeSidePanel
        {
            public CustomSidePanel(PeerTradeFormBase<Character, ItemEntity, IItemTable> parent, Vector2 position,
                                   bool isSourceSide) : base(parent, position, isSourceSide)
            {
            }

            /// <summary>
            /// Creates a <see cref="Control"/> for displaying a single item slot in this side of the peer trade table.
            /// </summary>
            /// <param name="position">The position of the control.</param>
            /// <param name="clientSize">The client size of the control.</param>
            /// <param name="slot">The slot that the created control will be handling.</param>
            /// <returns>A <see cref="Control"/> for displaying a single item slot in this side of the peer trade table.</returns>
            protected override PeerTradeItemsCollectionSlot CreateItemSlotControl(Vector2 position, Vector2 clientSize,
                                                                                  InventorySlot slot)
            {
                return new CustomSidePanelSlot(this, position, clientSize, slot);
            }

            /// <summary>
            /// A custom implementation of the <see cref="PeerTradeItemsCollectionSlot"/>.
            /// </summary>
            class CustomSidePanelSlot : PeerTradeItemsCollectionSlot
            {
                public CustomSidePanelSlot(PeerTradeSidePanel parent, Vector2 position, Vector2 clientSize, InventorySlot slot)
                    : base(parent, position, clientSize, slot)
                {
                }

                /// <summary>
                /// Gets the <see cref="Font"/> to use for drawing the item amount.
                /// </summary>
                /// <returns>The <see cref="Font"/> to use for drawing the item amount.</returns>
                protected override Font GetItemAmountFont()
                {
                    return GUIManager.Font;
                }

                /// <summary>
                /// Gets the foreground color to use for drawing the item amount text.
                /// </summary>
                /// <returns>The foreground color to use for drawing the item amount text.</returns>
                protected override Color GetItemAmountFontForeColor()
                {
                    return InventoryForm.ItemAmountForeColor;
                }

                /// <summary>
                /// Gets the shadow color to use for drawing the item amount text.
                /// </summary>
                /// <returns>The shadow color to use for drawing the item amount text.</returns>
                protected override Color GetItemAmountFontShadowColor()
                {
                    return InventoryForm.ItemAmountBackColor;
                }
            }
        }

        #region Implementation of IDragDropProvider

        /// <summary>
        /// Gets if this <see cref="IDragDropProvider"/> can be dragged. In the case of something that only
        /// supports having items dropped on it but not dragging, this will always return false. For items that can be
        /// dragged, this will return false if there is currently nothing to drag (such as an empty inventory slot) or
        /// there is some other reason that this item cannot currently be dragged.
        /// </summary>
        bool IDragDropProvider.CanDragContents
        {
            get { return false; }
        }

        /// <summary>
        /// Gets if the specified <see cref="IDragDropProvider"/> can be dropped on this <see cref="IDragDropProvider"/>.
        /// </summary>
        /// <param name="source">The <see cref="IDragDropProvider"/> to check if can be dropped on this
        /// <see cref="IDragDropProvider"/>. This value will never be null.</param>
        /// <returns>True if the <paramref name="source"/> can be dropped on this <see cref="IDragDropProvider"/>;
        /// otherwise false.</returns>
        bool IDragDropProvider.CanDrop(IDragDropProvider source)
        {
            // Only allow drag-and-drop when trading
            if (!IsTradeActive)
                return false;

            // Only allow drag-and-drop from an inventory item control
            if (!(source is InventoryForm.InventoryItemPB))
                return false;

            return true;
        }

        /// <summary>
        /// Draws the item that this <see cref="IDragDropProvider"/> contains for when this item
        /// is being dragged.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="position">The position to draw the sprite at.</param>
        /// <param name="color">The color to use when drawing the item.</param>
        void IDragDropProvider.DrawDraggedItem(ISpriteBatch spriteBatch, Vector2 position, Color color)
        {
            // Nothing to implement since this control doesn't support dragging from
        }

        /// <summary>
        /// Draws a visual highlighting on this <see cref="IDragDropProvider"/> for when an item is being
        /// dragged onto it but not yet dropped.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        void IDragDropProvider.DrawDropHighlight(ISpriteBatch spriteBatch)
        {
        }

        /// <summary>
        /// Handles when the specified <see cref="IDragDropProvider"/> is dropped on this <see cref="IDragDropProvider"/>.
        /// </summary>
        /// <param name="source">The <see cref="IDragDropProvider"/> that is being dropped on this
        /// <see cref="IDragDropProvider"/>.</param>
        void IDragDropProvider.Drop(IDragDropProvider source)
        {
            var ptih = PeerTradeInfoHandler;
            if (ptih == null)
                return;

            // Handle an inventory item control
            var asInvItem = source as InventoryForm.InventoryItemPB;
            if (asInvItem != null)
            {
                ptih.WriteAddInventoryItem(asInvItem.Slot);
            }
        }

        #endregion
    }
}