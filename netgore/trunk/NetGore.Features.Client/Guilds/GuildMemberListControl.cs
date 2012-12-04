using System;
using System.Linq;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace NetGore.Features.Guilds
{
    public class GuildMemberListControl : ListBox<GuildMemberNameRank>
    {
        int _cachedSpacing = 5;
        Font _cachedSpacingFont = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberListControl"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public GuildMemberListControl(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
            ShowPaging = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberListControl"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public GuildMemberListControl(IGUIManager guiManager, Vector2 position, Vector2 clientSize)
            : base(guiManager, position, clientSize)
        {
            ShowPaging = true;
        }

        void DefaultDrawer(ISpriteBatch sb, Vector2 pos, GuildMemberNameRank item, int index)
        {
            if (Font != _cachedSpacingFont)
            {
                _cachedSpacingFont = Font;
                _cachedSpacing = (int)Font.MeasureString("W").X;
            }

            // Rank
            sb.DrawString(Font, item.Rank.ToString(), pos, Color.Green);

            // Name
            sb.DrawString(Font, item.Name, pos + new Vector2(_cachedSpacing + 2, 0), ForeColor);
        }

        /// <summary>
        /// Gets the default item drawer.
        /// </summary>
        /// <returns>The default item drawer.</returns>
        protected override Action<ISpriteBatch, Vector2, GuildMemberNameRank, int> GetDefaultItemDrawer()
        {
            return DefaultDrawer;
        }
    }
}