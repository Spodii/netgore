using System;
using System.Diagnostics;
using System.Reflection;
using log4net;
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

        Grh _background;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMenuScreenBase"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        /// <param name="name">Unique name of the screen that can be used to identify and
        /// call it from other screens</param>
        /// <exception cref="ArgumentNullException"><paramref name="screenManager"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null or empty.</exception>
        public GameMenuScreenBase(IScreenManager screenManager, string name) : base(screenManager, name)
        {
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

            _background = new Grh();
            var bgGrhData = GrhInfo.GetData("Background", "menu");
            if (bgGrhData != null)
            {
                _background.SetGrh(bgGrhData);
            }
            else
            {
                const string errmsg = "Failed to find menu background image for screen `{0}` - background will not be drawn.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
            }
        }

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the
        /// active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Draw(NetGore.TickCount gameTime)
        {
            var spriteBatch = DrawingManager.BeginDrawGUI();
            if (spriteBatch == null)
                return;

            // Draw the background
            if (_background != null)
            {
                var bgDest= new Rectangle(0, 0, (int)ScreenManager.ScreenSize.X, (int)ScreenManager.ScreenSize.Y);
                _background.Draw(spriteBatch, bgDest);
            }

            // Resume normal drawing
            GUIManager.Draw(spriteBatch);
            DrawingManager.EndDrawGUI();
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
    }
}