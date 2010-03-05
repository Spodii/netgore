using System;
using System.Collections.Generic;
using System.Linq;
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
        public const string ScreenName = "console";
        static readonly Color _overlayColor = new Color(255, 255, 255, 200);

        readonly List<Level> _disabledLogLevels = new List<Level>();
        readonly MemoryAppender _logger = new MemoryAppender();
        readonly List<CheckBox> _logLevelCheckBoxes = new List<CheckBox>();

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
        /// Creates a <see cref="CheckBox"/> for a <see cref="Level"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="index">The display index.</param>
        /// <returns>The created <see cref="CheckBox"/>.</returns>
        CheckBox CreateLogLevelCheckBox(Level level, int index)
        {
            var pos = new Vector2(_cScreen.Size.X - 100, 5);
            var ret = new CheckBox(_cScreen, pos) { Font = _consoleFont, Text = level.Name, Tag = level, ForeColor = level.GetColor(), Value = true };
            ret.Position += new Vector2(0, ret.Size.Y * index);
            ret.ValueChanged += LevelCheckBox_ValueChanged;
            return ret;
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
            List<LoggingEvent> ret = new List<LoggingEvent>();

            foreach (var e in events)
            {
                if (!_disabledLogLevels.Contains(e.Level))
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
            ScreenManager.Updated += ScreenManager_Updated;

            _consoleFont = ScreenManager.Content.Load<SpriteFont>("Font/Console");

            _cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            _txtOutput = new TextBox(_cScreen, Vector2.Zero, _cScreen.Size)
            {  Font = _consoleFont, Size = _cScreen.Size, Border = ControlBorder.Empty, MaxBufferSize = 200, BufferTruncateSize = 80, CanFocus = false, CanDrag = false, IsMultiLine = true, IsEnabled = false };

            // Create the logging level checkboxes
            _logLevelCheckBoxes.Add(CreateLogLevelCheckBox(Level.Fatal, 0));
            _logLevelCheckBoxes.Add(CreateLogLevelCheckBox(Level.Error, 1));
            _logLevelCheckBoxes.Add(CreateLogLevelCheckBox(Level.Warn, 2));
            _logLevelCheckBoxes.Add(CreateLogLevelCheckBox(Level.Info, 3));
            _logLevelCheckBoxes.Add(CreateLogLevelCheckBox(Level.Debug, 4));

            // Disable debug by default
            _logLevelCheckBoxes.First(x => (Level)x.Tag == Level.Debug).Value = false;
        }

        /// <summary>
        /// Handles when the <see cref="ScreenManager"/> updates.
        /// </summary>
        /// <param name="screenManager">The screen manager.</param>
        void ScreenManager_Updated(IScreenManager screenManager)
        {
            // The logger we use to grab log messages and output to the console will continue to queue messages
            // indefinitely until it is cleared. Because of this, we can't just flush the log by overriding
            // the screen's Update method. Instead, we have this event hook for when the ScreenManager
            // updates so we can clear out the log buffer every tick.

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
        /// Handles when the value changes for a <see cref="CheckBox"/> for a <see cref="Level"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void LevelCheckBox_ValueChanged(Control sender)
        {
            // Rebuild the list of disabled log levels
            _disabledLogLevels.Clear();
            _disabledLogLevels.AddRange(_logLevelCheckBoxes.Where(x => !x.Value).Select(x => (Level)x.Tag));
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