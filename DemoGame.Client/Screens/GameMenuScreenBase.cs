using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// The base <see cref="GameScreen"/> to be used for all the menu <see cref="GameScreen"/>s.
    /// </summary>
    public class GameMenuScreenBase : GameScreen
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The color of the title text border.
        /// </summary>
        static readonly Color _titleBorderColor = Color.Black;

        /// <summary>
        /// The color of the title text.
        /// </summary>
        static readonly Color _titleColor = Color.White;

        Grh _background;
        string _title;
        Vector2 _titlePosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMenuScreenBase"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        /// <param name="name">Unique name of the screen that can be used to identify and
        /// call it from other screens</param>
        /// <param name="title">The text to display as the title.</param>
        /// <exception cref="ArgumentNullException"><paramref name="screenManager"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null or empty.</exception>
        public GameMenuScreenBase(IScreenManager screenManager, string name, string title) : base(screenManager, name)
        {
            Title = title;
            PlayMusic = true;
        }

        /// <summary>
        /// Gets or sets the title to display for this screen.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (StringComparer.Ordinal.Equals(_title, value))
                    return;

                _title = value;

                // Update the title size
                Vector2 titleSize;
                if (!string.IsNullOrEmpty(_title))
                {
                    // Measure the string
                    titleSize = GameScreenHelper.DefaultMenuTitleFont.MeasureString(_title);
                }
                else
                {
                    // Invalid string given, so just use 1x1
                    titleSize = Vector2.One;
                }

                // Update the position
                _titlePosition = new Vector2((ScreenManager.ScreenSize.X / 2f) - (titleSize.X / 2f), 10f);
            }
        }

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the
        /// active screen.
        /// When you override this method, call <see cref="GameMenuScreenBase.DrawBackground"/> with the
        /// <see cref="ISpriteBatch"/> for the GUI to draw the default menu background.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Draw(TickCount gameTime)
        {
            var spriteBatch = DrawingManager.BeginDrawGUI();
            if (spriteBatch == null)
                return;

            DrawBackground(spriteBatch);

            GUIManager.Draw(spriteBatch);
            DrawingManager.EndDrawGUI();
        }

        /// <summary>
        /// Performs the default menu background drawing.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected virtual void DrawBackground(ISpriteBatch spriteBatch)
        {
            // Draw the background
            if (_background != null)
            {
                var bgDest = new Rectangle(0, 0, ScreenManager.ScreenSize.X, ScreenManager.ScreenSize.Y);
                _background.Draw(spriteBatch, bgDest);
            }

            // Draw the title
            if (!string.IsNullOrEmpty(_title))
                spriteBatch.DrawStringShaded(GameScreenHelper.DefaultMenuTitleFont, Title, _titlePosition, _titleColor,
                    _titleBorderColor);
        }

        /// <summary>
        /// Gets the <see cref="Font"/> to use as the default font for the <see cref="IGUIManager"/> for this
        /// <see cref="GameScreen"/>.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> for this screen.</param>
        /// <returns>The <see cref="Font"/> to use for this <see cref="GameScreen"/>. If null, the
        /// <see cref="IScreenManager.DefaultFont"/> for this <see cref="GameScreen"/> will be used instead.</returns>
        protected override Font GetScreenManagerFont(IScreenManager screenManager)
        {
            return GameScreenHelper.DefaultScreenFont;
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            PlayMusic = false;

            // Create the background sprite
            _background = new Grh();
            var bgGrhData = GrhInfo.GetData("Background", "menu");
            if (bgGrhData != null)
                _background.SetGrh(bgGrhData);
            else
            {
                const string errmsg = "Failed to find menu background image for screen `{0}` - background will not be drawn.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
            }
        }
    }
}