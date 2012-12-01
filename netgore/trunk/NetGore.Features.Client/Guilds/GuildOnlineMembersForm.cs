using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace NetGore.Features.Guilds
{
    public class GuildOnlineMembersForm : GuildMembersFormBase
    {
        readonly TypedEventHandler<UserGuildInformation, EventArgs<string>> _updateHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildOnlineMembersForm"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public GuildOnlineMembersForm(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
            _updateHandler = (x, y) => UpdateCache();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildOnlineMembersForm"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public GuildOnlineMembersForm(IGUIManager guiManager, Vector2 position, Vector2 clientSize)
            : base(guiManager, position, clientSize)
        {
            _updateHandler = (x, y) => UpdateCache();
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to display in the list.
        /// </summary>
        /// <returns>The items to display in the list.</returns>
        protected override IEnumerable<GuildMemberNameRank> GetListItems(UserGuildInformation guildInfo)
        {
            return guildInfo.OnlineMembers;
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="UserGuildInformation"/> changes.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        protected override void OnGuildInfoChanged(UserGuildInformation newValue, UserGuildInformation oldValue)
        {
            base.OnGuildInfoChanged(newValue, oldValue);

            if (oldValue != null)
            {
                oldValue.OnlineMemberAdded -= _updateHandler;
                oldValue.OnlineMemberRemoved -= _updateHandler;
            }

            if (newValue != null)
            {
                newValue.OnlineMemberAdded -= _updateHandler;
                newValue.OnlineMemberAdded += _updateHandler;

                newValue.OnlineMemberRemoved -= _updateHandler;
                newValue.OnlineMemberRemoved += _updateHandler;
            }
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Online Guild Members";
        }
    }
}