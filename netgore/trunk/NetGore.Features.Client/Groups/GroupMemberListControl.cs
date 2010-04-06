using System;
using System.Linq;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace NetGore.Features.Groups
{
    public class GroupMemberListControl : ListBox<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupMemberListControl"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public GroupMemberListControl(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
            ShowPaging = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupMemberListControl"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public GroupMemberListControl(IGUIManager guiManager, Vector2 position, Vector2 clientSize)
            : base(guiManager, position, clientSize)
        {
            ShowPaging = true;
        }
    }
}