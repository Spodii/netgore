using System;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    /// <summary>
    /// <see cref="EventArgs"/> for handling events from the <see cref="Toolbar"/>.
    /// </summary>
    class ToolbarEventArgs : EventArgs
    {
        readonly ToolbarItemType _itemType;
        readonly Control _control;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolbarEventArgs"/> class.
        /// </summary>
        /// <param name="itemType"><see cref="ToolbarItemType"/> for the Toolbar item that raised the event.</param>
        /// <param name="control">The <see cref="Control"/> that raised the event.</param>
        public ToolbarEventArgs(ToolbarItemType itemType, Control control)
        {
            _itemType = itemType;
            _control = control;
        }

        /// <summary>
        /// Gets the <see cref="ToolbarItemType"/> for the Toolbar item that raised the event.
        /// </summary>
        public ToolbarItemType ItemType { get { return _itemType; } }

        /// <summary>
        /// Gets the <see cref="Control"/> that raised the event.
        /// </summary>
        public Control Control { get { return _control; } }
    }
}