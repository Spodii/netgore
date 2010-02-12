using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics.GUI;

namespace NetGore.Features.Guilds
{
    public class GuildMemberListControl : PagedList<GuildMemberNameRank>
    {
        int _cachedSpacing = 5;
        SpriteFont _cachedSpacingFont = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberListControl"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public GuildMemberListControl(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
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
        }

        void DefaultDrawer(SpriteBatch sb, Vector2 pos, int v)
        {
            if (Font != _cachedSpacingFont)
            {
                _cachedSpacingFont = Font;
                _cachedSpacing = (int)Font.MeasureString("W").X;
            }

            // Rank
            sb.DrawString(Font, Items.ElementAt(v).Rank.ToString(), pos, Color.Green);

            // Name
            sb.DrawString(Font, Items.ElementAt(v).Name, pos + new Vector2(_cachedSpacing + 2, 0), ForeColor);
        }

        /// <summary>
        /// Gets the default item drawer.
        /// </summary>
        /// <returns>The default item drawer.</returns>
        protected override Action<SpriteBatch, Vector2, int> GetDefaultItemDrawer()
        {
            return DefaultDrawer;
        }
    }
}