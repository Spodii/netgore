using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.World;
using SFML.Window;

namespace NetGore.Features.PeerTrading
{
    public class ItemSlotClickedEventArgs : EventArgs
    {
        readonly MouseButtonEventArgs _mouseButtonEventArgs;
        readonly bool _isSourceSide;
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
        /// Gets the <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.
        /// </summary>
        public MouseButtonEventArgs MouseButtonEventArgs { get { return _mouseButtonEventArgs; } }

        /// <summary>
        /// Gets if the item slot clicked is on the source side.
        /// </summary>
        public bool IsSourceSide { get { return _isSourceSide; } }

        /// <summary>
        /// Gets the slot that was clicked.
        /// </summary>
        public InventorySlot Slot { get { return _slot; } }
    }
}