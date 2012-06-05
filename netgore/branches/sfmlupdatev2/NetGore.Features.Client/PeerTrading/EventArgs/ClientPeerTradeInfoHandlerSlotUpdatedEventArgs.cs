using System;
using System.Linq;

namespace NetGore.Features.PeerTrading
{
    public class ClientPeerTradeInfoHandlerSlotUpdatedEventArgs : EventArgs
    {
        readonly bool _isSourceSide;
        readonly InventorySlot _slot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPeerTradeInfoHandlerSlotUpdatedEventArgs"/> class.
        /// </summary>
        /// <param name="slot">The slot that changed.</param>
        /// <param name="isSourceSide">If true, the changed slot was on the source character's side.
        /// If false, it was on the target character's side.</param>
        public ClientPeerTradeInfoHandlerSlotUpdatedEventArgs(InventorySlot slot, bool isSourceSide)
        {
            _slot = slot;
            _isSourceSide = isSourceSide;
        }

        /// <summary>
        /// Gets if the changed slot was on the source character's side.
        /// If false, it was on the target character's side.
        /// </summary>
        public bool IsSourceSide
        {
            get { return _isSourceSide; }
        }

        /// <summary>
        /// Gets the slot that changed.
        /// </summary>
        public InventorySlot Slot
        {
            get { return _slot; }
        }
    }
}