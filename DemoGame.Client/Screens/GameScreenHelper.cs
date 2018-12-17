using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Content;
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
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly Font _defaultChatFont;
        static readonly Font _defaultGameGUIFont;
        static readonly Font _defaultMenuButtonFont;
        static readonly Font _defaultMenuTitleFont;
        static readonly Font _defaultScreenFont;

        /// <summary>
        /// Initializes the <see cref="GameScreenHelper"/> class.
        /// </summary>
        static GameScreenHelper()
        {
            var content = ContentManager.Create();

            _defaultGameGUIFont = content.LoadFont("Font/varelaroundregular", 12, ContentLevel.Global);
            _defaultChatFont = content.LoadFont("Font/varelaroundregular", 12, ContentLevel.Global);
            _defaultScreenFont = content.LoadFont("Font/varelaroundregular", 24, ContentLevel.Global);
            _defaultMenuButtonFont = content.LoadFont("Font/journal", 42, ContentLevel.Global);
            _defaultMenuTitleFont = content.LoadFont("Font/journal", 72, ContentLevel.Global);
        }

        /// <summary>
        /// Gets the default <see cref="Font"/> for chat text.
        /// </summary>
        public static Font DefaultChatFont
        {
            get { return _defaultChatFont; }
        }

        /// <summary>
        /// Gets the default <see cref="Font"/> for the game GUI.
        /// </summary>
        public static Font DefaultGameGUIFont
        {
            get { return _defaultGameGUIFont; }
        }

        /// <summary>
        /// Gets the default menu button <see cref="Font"/>.
        /// </summary>
        public static Font DefaultMenuButtonFont
        {
            get { return _defaultMenuButtonFont; }
        }

        /// <summary>
        /// Gets the default <see cref="Font"/> for the title on the menu screens.
        /// </summary>
        public static Font DefaultMenuTitleFont
        {
            get { return _defaultMenuTitleFont; }
        }

        /// <summary>
        /// Gets the default font for a <see cref="GameScreen"/>.
        /// </summary>
        public static Font DefaultScreenFont
        {
            get { return _defaultScreenFont; }
        }

        /// <summary>
        /// Creates the <see cref="Control"/>s for the primary menu links.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> for the screen.</param>
        /// <param name="parent">The parent control to attach the buttons to.</param>
        /// <param name="names">The unique name of the buttons.</param>
        /// <returns>
        /// The menu <see cref="Control"/>s, where the unique name is the key and the <see cref="Control"/> instance
        /// is the value.
        /// </returns>
        public static IDictionary<string, Control> CreateMenuButtons(IScreenManager screenManager, Control parent,
                                                                     params string[] names)
        {
            // The offset from the side of the screen
            var baseOffset = new Vector2(25f);

            // The spacing between each button
            const float spacing = 10f;

            var clientSize = parent == null ? screenManager.ScreenSize : parent.ClientSize;
            var sumSize = 0f;
            var ret = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase);

            // Create each button in reverse (so the items at the end of the list end up at the bottom)
            for (var i = names.Length - 1; i >= 0; i--)
            {
                // Create the button
                var c = new MenuButton(parent, Vector2.One) { Text = names[i] };

                // Measure the size of the text
                var textSize = c.Font.MeasureString(c.Text);

                // Get the position
                var newPos = (clientSize - baseOffset - textSize);

                // Offset on the y-axis by the sum size
                newPos.Y -= sumSize;

                // Update the sumSize
                sumSize += textSize.Y + spacing;

                // Set the new position
                c.Position = newPos;

                // Add to the return collection
                ret.Add(names[i], c);

                if (log.IsDebugEnabled)
                    log.DebugFormat("Created menu button `{0}`.", c.Text);
            }

            return ret;
        }

        /// <summary>
        /// Creates a <see cref="Label"/> for labels in the menu.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="position">The position.</param>
        /// <param name="text">The initial text to display.</param>
        /// <returns>The <see cref="Label"/> instance.</returns>
        public static Label CreateMenuLabel(Control parent, Vector2 position, string text)
        {
            var ret = new MenuLabel(parent, position) { Text = text };

            if (log.IsDebugEnabled)
                log.DebugFormat("Created menu label `{0}`.", text);

            return ret;
        }

        /// <summary>
        /// A <see cref="Label"/> specifically for menu buttons.
        /// </summary>
        class MenuButton : Label
        {
            /// <summary>
            /// The color of the text's border.
            /// </summary>
            static readonly Color _textBorderColor = Color.Black;

            /// <summary>
            /// The color of the text when the mouse is not over it.
            /// </summary>
            static readonly Color _textColor = Color.White;

            /// <summary>
            /// The color of the text when the mouse is over it.
            /// </summary>
            static readonly Color _textMouseOverColor = Color.Green;

            /// <summary>
            /// Initializes a new instance of the <see cref="MenuButton"/> class.
            /// </summary>
            /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
            /// <param name="position">Position of the Control reletive to its parent.</param>
            /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
            public MenuButton(Control parent, Vector2 position) : base(parent, position)
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

                spriteBatch.DrawStringShaded(Font, Text, ScreenPosition + position, ForeColor, _textBorderColor);
            }

            /// <summary>
            /// Handles when the mouse has entered the area of the <see cref="Control"/>.
            /// This is called immediately before <see cref="Control.OnMouseEnter"/>.
            /// Override this method instead of using an event hook on <see cref="Control.MouseEnter"/> when possible.
            /// </summary>
            /// <param name="e">The event args.</param>
            protected override void OnMouseEnter(MouseMoveEventArgs e)
            {
                base.OnMouseEnter(e);

                ForeColor = _textMouseOverColor;
            }

            /// <summary>
            /// Handles when the mouse has left the area of the <see cref="Control"/>.
            /// This is called immediately before <see cref="Control.OnMouseLeave"/>.
            /// Override this method instead of using an event hook on <see cref="Control.MouseLeave"/> when possible.
            /// </summary>
            /// <param name="e">The event args.</param>
            protected override void OnMouseLeave(MouseMoveEventArgs e)
            {
                base.OnMouseLeave(e);

                ForeColor = _textColor;
            }

            /// <summary>
            /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
            /// base class's method to ensure that changes to settings are hierchical.
            /// </summary>
            protected override void SetDefaultValues()
            {
                base.SetDefaultValues();

                ForeColor = _textColor;
                Font = DefaultMenuButtonFont;
            }
        }

        /// <summary>
        /// A <see cref="Label"/> specifically for menu labels.
        /// </summary>
        class MenuLabel : Label
        {
            /// <summary>
            /// The color of the text's border.
            /// </summary>
            static readonly Color _textBorderColor = Color.Black;

            /// <summary>
            /// The color of the text when the mouse is not over it.
            /// </summary>
            static readonly Color _textColor = Color.White;

            /// <summary>
            /// Initializes a new instance of the <see cref="MenuLabel"/> class.
            /// </summary>
            /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
            /// <param name="position">Position of the Control reletive to its parent.</param>
            /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
            public MenuLabel(Control parent, Vector2 position) : base(parent, position)
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

                spriteBatch.DrawStringShaded(Font, Text, ScreenPosition + position, ForeColor, _textBorderColor);
            }

            /// <summary>
            /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
            /// base class's method to ensure that changes to settings are hierchical.
            /// </summary>
            protected override void SetDefaultValues()
            {
                base.SetDefaultValues();

                ForeColor = _textColor;
                Font = DefaultScreenFont;
            }
        }
    }
}