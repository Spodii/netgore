using System;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.Graphics.GUI;

namespace NetGore.Features.Guilds
{
    public abstract class GuildInfoFormBase : Form
    {
        public delegate void ChangeGuildInfoEventHandler(
            GuildInfoFormBase sender, UserGuildInformation newValue, UserGuildInformation oldValue);

        UserGuildInformation _guildInfo;

        /// <summary>
        /// Notifies listeners when the <see cref="GuildInfoFormBase.GuildInfo"/> value has changed.
        /// </summary>
        public event ChangeGuildInfoEventHandler OnChangeGuildInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildInfoFormBase"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        protected GuildInfoFormBase(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildInfoFormBase"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        protected GuildInfoFormBase(IGUIManager guiManager, Vector2 position, Vector2 clientSize)
            : base(guiManager, position, clientSize)
        {
        }

        /// <summary>
        /// Gets or sets the guild information source.
        /// </summary>
        public UserGuildInformation GuildInfo
        {
            get { return _guildInfo; }
            set
            {
                if (_guildInfo == value)
                    return;

                var old = _guildInfo;

                _guildInfo = value;

                HandleChangeGuild(_guildInfo, old);

                if (OnChangeGuildInfo != null)
                    OnChangeGuildInfo(this, _guildInfo, old);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="UserGuildInformation"/> changes.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        protected virtual void HandleChangeGuild(UserGuildInformation newValue, UserGuildInformation oldValue)
        {
        }
    }
}