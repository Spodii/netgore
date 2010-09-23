using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// Provides helper methods for the <see cref="GameScreen"/>.
    /// </summary>
    public static class GameScreenHelper
    {
        /// <summary>
        /// The menu button font name.
        /// </summary>
        const string _menuButtonFont = "Font/If";
        
        /// <summary>
        /// The menu button font size.
        /// </summary>
        const int _menuButtonFontSize = 42;

        static readonly MouseEventHandler _menuButtonMouseEnter;
        static readonly MouseEventHandler _menuButtonMouseLeave;

        /// <summary>
        /// The color of the menu text's border.
        /// </summary>
        static readonly Color _menuTextBorderColor = Color.Black;

        /// <summary>
        /// The color of the menu text when the mouse is not over it.
        /// </summary>
        static readonly Color _menuTextColor = Color.White;

        /// <summary>
        /// The color of the menu text when the mouse is over it.
        /// </summary>
        static readonly Color _menuTextMouseOverColor = Color.Green;

        /// <summary>
        /// Initializes the <see cref="GameScreenHelper"/> class.
        /// </summary>
        static GameScreenHelper()
        {
            _menuButtonMouseEnter = button_MouseEnter;
            _menuButtonMouseLeave = button_MouseLeave;
        }

        /// <summary>
        /// Gets the default font for a <see cref="GameScreen"/>.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> for the <see cref="GameScreen"/>.</param>
        /// <returns>The <see cref="Font"/>.</returns>
        public static Font GetScreenDefaultFont(IScreenManager screenManager)
        {
            return screenManager.Content.LoadFont("Font/If", 24, NetGore.Content.ContentLevel.Global);
        }

        /// <summary>
        /// Gets the menu button <see cref="Font"/>.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> for the screen the <see cref="Font"/> is for.</param>
        /// <returns>The <see cref="Font"/> to use.</returns>
        static Font GetMenuButtonFont(IScreenManager screenManager)
        {
            return screenManager.Content.LoadFont(_menuButtonFont, _menuButtonFontSize, NetGore.Content.ContentLevel.Global);
        }

        /// <summary>
        /// Creates the buttons for the primary menu links.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> for the screen.</param>
        /// <param name="parent">The parent control to attach the buttons to.</param>
        /// <param name="names">The unique name of the buttons.</param>
        /// <returns>
        /// The menu buttons, where the name is the key.
        /// </returns>
        public static IDictionary<string, Control> CreateMenuButtons(IScreenManager screenManager, Control parent, params string[] names)
        {
            const float spacing = 10;
            var buttonSize = new Vector2(225, 40);
            var bottomButtonPosition = GameData.ScreenSize - buttonSize - new Vector2(50, 50);

            var ret = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase);
            var font = GetMenuButtonFont(screenManager);

            var pos = bottomButtonPosition;
            for (var i = names.Length - 1; i >= 0; i--)
            {
                var c = new MenuButtonLabel(parent, pos)
                { Text = names[i], Border = null, ForeColor = _menuTextColor, Font = font };

                c.Position = new Vector2(parent.ClientSize.X - c.ClientSize.X - 50, c.Position.Y);

                var fontSize = c.Font.MeasureString(c.Text);
                c.ClientSize = fontSize;

                c.MouseEnter += _menuButtonMouseEnter;
                c.MouseLeave += _menuButtonMouseLeave;

                pos.Y -= spacing + c.Size.Y;

                ret.Add(names[i], c);
            }

            return ret;
        }

        /// <summary>
        /// Handles the MouseEnter event of the menu button controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseMoveEventArgs"/> instance containing the event data.</param>
        static void button_MouseEnter(object sender, MouseMoveEventArgs e)
        {
            var c = (Label)sender;
            c.ForeColor = _menuTextMouseOverColor;
        }

        /// <summary>
        /// Handles the MouseLeave event of the menu button controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseMoveEventArgs"/> instance containing the event data.</param>
        static void button_MouseLeave(object sender, MouseMoveEventArgs e)
        {
            var c = (Label)sender;
            c.ForeColor = _menuTextColor;
        }

        /// <summary>
        /// A <see cref="Label"/> specifically for menu buttons.
        /// </summary>
        class MenuButtonLabel : Label
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MenuButtonLabel"/> class.
            /// </summary>
            /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
            /// <param name="position">Position of the Control reletive to its parent.</param>
            /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
            public MenuButtonLabel(Control parent, Vector2 position) : base(parent, position)
            {
            }

            /// <summary>
            /// Draws the text for the control.
            /// </summary>
            /// <param name="spriteBatch"><see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="position">Position relative to the Control to draw the text.</param>
            protected override void DrawText(ISpriteBatch spriteBatch, Vector2 position)
            {
                if (string.IsNullOrEmpty(Text) || Font == null)
                    return;

                spriteBatch.DrawStringShaded(Font, Text, ScreenPosition + position, ForeColor, _menuTextBorderColor);
            }
        }
    }
}