using Microsoft.Xna.Framework;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Base of a game screen for the ScreenManager. Each screen should ideally be independent of
    /// one another (with the exception of screen change notifications).
    /// </summary>
    public abstract class GameScreen
    {
        readonly string _name;

        /// <summary>
        /// Gets the unique name of this screen, which is the same name that can be used in
        /// ScreenManager's GetScreen() and SetScreen() methods.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the ScreenManager used by this GameScreen
        /// </summary>
        public ScreenManager ScreenManager { get; internal set; }

        /// <summary>
        /// GameScreen constructor
        /// </summary>
        /// <param name="name">Unique name of the screen that can be used to identify and
        /// call it from other screens</param>
        protected GameScreen(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on Deactivate().
        /// </summary>
        public virtual void Activate()
        {
        }

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in Activate().
        /// </summary>
        public virtual void Deactivate()
        {
        }

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the 
        /// active screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Handles the loading of game content. Any content that is loaded should be placed in here.
        /// This will be invoked once (right after Initialize()), along with an additional time for
        /// every time XNA notifies the ScreenManager that the game content needs to be reloaded.
        /// </summary>
        public virtual void LoadContent()
        {
        }

        /// <summary>
        /// Handles the unloading of game content. This is raised whenever XNA notifies the ScreenManager
        /// that the content is to be unloaded.
        /// </summary>
        public virtual void UnloadContent()
        {
        }

        /// <summary>
        /// Handles updating of the screen. This will only be called while the screen is the active screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public abstract void Update(GameTime gameTime);
    }
}