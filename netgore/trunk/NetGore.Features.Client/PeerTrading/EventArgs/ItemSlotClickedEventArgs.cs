using System;
using System.Linq;
using SFML.Window;

namespace NetGore.Features.PeerTrading
{
    public class ItemSlotClickedEventArgs : EventArgs
    {
        readonly bool _isSourceSide;
        readonly MouseButtonEventArgs _mouseButtonEventArgs;
        readonly InventorySlot _slot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSlotClickedEventArgs"/> class.
        /// </summary>
        /// <param name="mouseButtonEventArgs">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <param name="isSourceSide">If the item slot clicked is on the source side.</param>
        /// <param name="slot">The slot that was clicked.</param>
        public ItemSlotClickedEventArgs(MouseButtonEventArgs mouseButtonEventArgs, bool isSourceSide, InventorySlot slot)
        {
            _mouseButtonEventArgs = mouseButtonEventArgs;
            _isSourceSide = isSourceSide;
            _slot = slot;
        }

        /// <summary>
        /// Gets if the item slot clicked is on the source side.
        /// </summary>
        public bool IsSourceSide
        {
            get { return _isSourceSide; }
        }

        /// <summary>
        /// Gets the <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.
        /// </summary>
        public MouseButtonEventArgs MouseButtonEventArgs
        {
            get { return _mouseButtonEventArgs; }
        }

        /// <summary>
        /// Gets the slot that was clicked.
        /// </summary>
        public InventorySlot Slot
        {
            get { return _slot; }
        }
    }
}