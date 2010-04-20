using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Provides helper methods for the <see cref="GameScreen"/>.
    /// </summary>
    public static class GameScreenHelper
    {
        /// <summary>
        /// Creates the buttons for the primary menu links.
        /// </summary>
        /// <param name="parent">The parent control to attach the buttons to.</param>
        /// <param name="names">The unique name of the buttons.</param>
        /// <returns>The menu buttons, where the name is the key.</returns>
        public static IDictionary<string, Button> CreateMenuButtons(Control parent, params string[] names)
        {
            const float spacing = 10;
            var buttonSize = new Vector2(225, 40);
            var bottomButtonPosition = GameData.ScreenSize - buttonSize - new Vector2(50, 50);

            var ret = new Dictionary<string, Button>(StringComparer.OrdinalIgnoreCase);

            var pos = bottomButtonPosition;
            for (var i = names.Length - 1; i >= 0; i--)
            {
                var button = new Button(parent, pos, buttonSize) { Text = names[i] };
                pos.Y -= spacing + button.Size.Y;

                ret.Add(names[i], button);
            }

            return ret;
        }
    }
}