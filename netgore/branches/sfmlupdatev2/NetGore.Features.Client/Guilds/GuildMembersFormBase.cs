using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace NetGore.Features.Guilds
{
    public abstract class GuildMembersFormBase : GuildInfoFormBase
    {
        /// <summary>
        /// How frequently the cached list updates due to elapsed time.
        /// </summary>
        const int _cacheUpdateRate = 2000;

        /// <summary>
        /// Cache list of the items to display.
        /// </summary>
        readonly List<GuildMemberNameRank> _listItemsCache = new List<GuildMemberNameRank>();

        readonly GuildMemberListControl _lstMembers;

        /// <summary>
        /// If true, the cache was built while in guild. If false, while not in a guild.
        /// </summary>
        bool _cacheStateInGuild;

        /// <summary>
        /// The time at which the cache will be updated.
        /// </summary>
        TickCount _cacheUpdateTime = TickCount.MinValue;

        TickCount _currentTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMembersFormBase"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        protected GuildMembersFormBase(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
            _lstMembers = new GuildMemberListControl(this, Vector2.Zero, ClientSize) { Items = _listItemsCache };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMembersFormBase"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        protected GuildMembersFormBase(IGUIManager guiManager, Vector2 position, Vector2 clientSize)
            : base(guiManager, position, clientSize)
        {
            _lstMembers = new GuildMemberListControl(this, Vector2.Zero, ClientSize) { Items = _listItemsCache };
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to display in the list.
        /// </summary>
        /// <param name="guildInfo">The guild info. Cannot be null.</param>
        /// <returns>The items to display in the list.</returns>
        protected abstract IEnumerable<GuildMemberNameRank> GetListItems(UserGuildInformation guildInfo);

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected override void OnResized()
        {
            base.OnResized();

            if (_lstMembers != null)
                _lstMembers.Size = ClientSize;
        }

        /// <summary>
        /// Forces the list cache to update.
        /// </summary>
        public void UpdateCache()
        {
            _cacheUpdateTime = _currentTime + _cacheUpdateRate;

            _listItemsCache.Clear();

            if (GuildInfo != null && GuildInfo.InGuild)
            {
                _cacheStateInGuild = true;
                _listItemsCache.AddRange(GetListItems(GuildInfo));
            }
            else
                _cacheStateInGuild = false;
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(TickCount currentTime)
        {
            _currentTime = currentTime;

            base.UpdateControl(currentTime);

            // Check to update the cache due to the elapsed time or guild state change
            var isInGuild = GuildInfo != null && GuildInfo.InGuild;
            if ((_cacheUpdateTime < currentTime) || (_cacheStateInGuild != isInGuild))
                UpdateCache();
        }
    }
}