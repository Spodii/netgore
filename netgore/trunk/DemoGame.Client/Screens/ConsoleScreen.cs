using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    class ConsoleScreen : GameScreen
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string ScreenName = "console";
        static readonly Color _overlayColor = new Color(255, 255, 255, 200);

        readonly MemoryAppender _logger = new MemoryAppender();

        SpriteFont _consoleFont;
        Panel _cScreen;
        TextBox _txtOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="screenManager"/> is null.</exception>
        public ConsoleScreen(IScreenManager screenManager) : base(screenManager, ScreenName)
        {
        }

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the
        /// active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Draw(int gameTime)
        {
            var spriteBatch = DrawingManager.BeginDrawGUI();

            // Draw an overlay on top of the old screen
            XNARectangle.Draw(spriteBatch, _cScreen.GetScreenArea(), _overlayColor);

            // Draw the GUI
            GUIManager.Draw(spriteBatch);

            DrawingManager.EndDrawGUI();
        }

        /// <summary>
        /// Filters only the enabled log events.
        /// </summary>
        /// <param name="events">The unfiltered log events.</param>
        /// <returns>The enabled log events.</returns>
        LoggingEvent[] GetFilteredEvents(IEnumerable<LoggingEvent> events)
        {
            // TODO: !! Add support to filter the logs

            List<LoggingEvent> ret = new List<LoggingEvent>();

            foreach (var e in events)
            {
                ret.Add(e);
            }

            return ret.ToArray();
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            BasicConfigurator.Configure(_logger);

            _cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            _txtOutput = new TextBox(_cScreen, Vector2.Zero, _cScreen.Size)
            { Size = _cScreen.Size, Border = ControlBorder.Empty, MaxBufferSize = 200, BufferTruncateSize = 80 };
        }

        /// <summary>
        /// Handles the loading of game content. Any content that is loaded should be placed in here.
        /// This will be invoked once (right after Initialize()), along with an additional time for
        /// every time XNA notifies the ScreenManager that the game content needs to be reloaded.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            _consoleFont = ScreenManager.Content.Load<SpriteFont>("Font/Console");
            _txtOutput.Font = _consoleFont;
        }

        /// <summary>
        /// Updates the screen if it is currently the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(int gameTime)
        {
            base.Update(gameTime);

            // Get the latest events
            LoggingEvent[] events;
            try
            {
                events = _logger.GetEvents();
            }
            catch (ArgumentException)
            {
                // There is some bug in the _logger.GetEvents() that can throw this exception...
                return;
            }

            _logger.Clear();

            // Ensure there are events
            if (events != null && events.Length > 0)
            {
                events = GetFilteredEvents(events);
                if (events != null && events.Length > 0)
                {
                    foreach (var e in events)
                    {
                        var styledText = new StyledText(e.RenderedMessage, e.Level.GetColor());
                        _txtOutput.AppendLine(styledText);
                    }
                }
            }

            // Move to the last line in the log textbox
            _txtOutput.LineBufferOffset = Math.Max(0, _txtOutput.LineCount - _txtOutput.MaxVisibleLines);
            _txtOutput.Resized += delegate { UpdateConsoleBufferSize(); };
            _txtOutput.FontChanged += delegate { UpdateConsoleBufferSize(); };
        }

        /// <summary>
        /// Updates the output buffer to a reasonable value based on the textbox size.
        /// </summary>
        void UpdateConsoleBufferSize()
        {
            // Keep the buffer to show at least 2x more than the number of visible lines
            _txtOutput.BufferTruncateSize = _txtOutput.MaxVisibleLines * 2;

            // Truncate only when we hit 2x the buffer size (its cheaper to truncate a lot at once)
            _txtOutput.MaxBufferSize = _txtOutput.BufferTruncateSize * 2;
        }
    }
}