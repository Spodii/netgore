using System;
using System.Linq;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.GUITester
{
    sealed class Game1 : RenderWindow
    {
        readonly ScreenManager _screenManager;
        readonly ISkinManager _skinManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game1"/> class.
        /// </summary>
        public Game1() : base(new VideoMode(800, 600), "GUI test bench", Styles.Titlebar | Styles.Close)
        {
            UseVerticalSync(true);
            ShowMouseCursor(true);
            SetFramerateLimit(60);

            _skinManager = new SkinManager("Default");
            _screenManager = new ScreenManager(this, _skinManager, "Font/Arial", 14);
            GrhInfo.Load(ContentPaths.Build, _screenManager.Content);

            var ts = new TestScreen(_screenManager);
            _screenManager.ActiveScreen = ts;

            Closed += Game1_Closed;

            GameLoop();
        }

        void Game1_Closed(object sender, EventArgs e)
        {
            Close();
        }

        void GameLoop()
        {
            while (IsOpened())
            {
                // Update
                DispatchEvents();

                _screenManager.Update(Environment.TickCount);

                // Draw
                _screenManager.Draw(Environment.TickCount);

                // Present
                Display();
            }
        }
    }
}