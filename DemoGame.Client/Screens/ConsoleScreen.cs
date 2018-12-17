using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using NetGore;
using NetGore.Content;
using NetGore.Features.GameTime;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    class ConsoleScreen : GameScreen
    {
        public const string ScreenName = "console";
        static readonly Color _overlayColor = new Color(255, 255, 255, 200);

        readonly List<Level> _disabledLogLevels = new List<Level>();
        readonly List<CheckBox> _logLevelCheckBoxes = new List<CheckBox>();
        readonly MemoryAppender _logger = new MemoryAppender();

        Button _btnShowSettings;
        Panel _cScreen;
        Panel _cSettingsPanel;
        Font _consoleFont;
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
            var ret = new CheckBox(_cSettingsPanel, new Vector2(_cSettingsPanel.Size.X - 100, 5)) 
            { 
                Font = _consoleFont, 
                Text = level.Name, 
                Tag = level, 
                ForeColor = level.GetColor(), 
                Value = true 
            };

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
        public override void Draw(TickCount gameTime)
        {
            var spriteBatch = DrawingManager.BeginDrawGUI(false);
            if (spriteBatch == null)
                return;

            // Draw an overlay on top of the old screen
            RenderRectangle.Draw(spriteBatch, _cScreen.GetScreenArea(), _overlayColor);

            // Draw the GUI
            GUIManager.Draw(spriteBatch);

            // Draw some extras
            const int yOff = 150;
            spriteBatch.DrawStringShaded(_txtOutput.Font, "FPS: " + ScreenManager.FPS,
                new Vector2(_cScreen.ClientSize.X - 100, yOff), Color.White, Color.Black);
            spriteBatch.DrawStringShaded(_txtOutput.Font,
                string.Format("Game Time: {0}:{1:00}", GameDateTime.Now.Hour, GameDateTime.Now.Minute),
                new Vector2(_cScreen.ClientSize.X - 150, _txtOutput.Font.GetLineSpacing() + yOff), Color.White, Color.Black);

            DrawingManager.EndDrawGUI();
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            BasicConfigurator.Configure(_logger);

            ScreenManager.Updated -= ScreenManager_Updated;
            ScreenManager.Updated += ScreenManager_Updated;

            _consoleFont = ScreenManager.Content.LoadFont("Font/Courier New", 14, ContentLevel.GameScreen);

            _cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize) { Border = ControlBorder.Empty };

            // The main console output textbox
            _txtOutput = new TextBox(_cScreen, Vector2.Zero, _cScreen.Size)
            {
                Font = _consoleFont,
                Size = _cScreen.Size,
                Border = ControlBorder.Empty,
                MaxBufferSize = 200,
                BufferTruncateSize = 80,
                CanFocus = false,
                CanDrag = false,
                IsMultiLine = true,
                IsEnabled = false
            };
            _txtOutput.Resized += delegate { UpdateConsoleBufferSize(); };
            _txtOutput.FontChanged += delegate { UpdateConsoleBufferSize(); };

            // Create the panel that holds the settings options
            var settingsButtonSize = _consoleFont.MeasureString("Show Settings") + new Vector2(10, 4);
            _btnShowSettings = new Button(_cScreen, new Vector2(_cScreen.Size.X, 0), settingsButtonSize) { Font = _consoleFont, Text = "Hide Settings" };
            _btnShowSettings.Position += new Vector2(-4, 4);
            _btnShowSettings.Clicked += btnShowSettings_Clicked;

            var settingsPanelSize = new Vector2(400, 400);
            _cSettingsPanel = new Panel(_cScreen, new Vector2(_cScreen.Size.X - settingsPanelSize.X - 4, _btnShowSettings.Position.Y + _btnShowSettings.Size.Y + 8), settingsPanelSize) 
                { IsVisible = true, CanDrag = false };

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
        /// Handles when the value changes for a <see cref="CheckBox"/> for a <see cref="Level"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void LevelCheckBox_ValueChanged(Control sender, EventArgs e)
        {
            // Rebuild the list of disabled log levels
            _disabledLogLevels.Clear();
            _disabledLogLevels.AddRange(_logLevelCheckBoxes.Where(x => !x.Value).Select(x => (Level)x.Tag));

            // Also add filters to the logger to reject the disabled log levels
            _logger.ClearFilters();
            foreach (var disabledLevel in _disabledLogLevels)
            {
                _logger.AddFilter(new LevelMatchFilter { AcceptOnMatch = false, LevelToMatch = disabledLevel });
            }
        }

        /// <summary>
        /// Handles when the <see cref="ScreenManager"/> updates.
        /// </summary>
        /// <param name="screenManager">The screen manager.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void ScreenManager_Updated(IScreenManager screenManager, EventArgs e)
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
                foreach (var ev in events)
                {
                    var styledText = new StyledText(ev.RenderedMessage, ev.Level.GetColor());
                    _txtOutput.AppendLine(styledText);
                }
            }

            // Move to the last line in the log textbox
            _txtOutput.LineBufferOffset = Math.Max(0, _txtOutput.LineCount - _txtOutput.MaxVisibleLines);
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

        /// <summary>
        /// Handles the Clicked event of the btnShowSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void btnShowSettings_Clicked(object sender, MouseButtonEventArgs e)
        {
            _cSettingsPanel.IsVisible = !_cSettingsPanel.IsVisible;
            _btnShowSettings.Text = (_cSettingsPanel.IsVisible ? "Hide" : "Show") + " settings";
        }
    }
}