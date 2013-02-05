using System;
using System.Linq;
using DemoGame.Client;
using NetGore;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;

namespace DemoGame.Client.Say
{
    /// <summary>
    /// The actual class that handles the Say commands.
    /// </summary>
    public partial class SayHandlerCommands : ISayCommands<Character>
    {
        private GameplayScreen _gameplayScreen;

        /// <summary>
        /// Initializes a new instance of the <see cref="SayHandlerCommands"/> class.
        /// </summary>
        /// <param name="server">The world that the commands will come from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="server" /> is <c>null</c>.</exception>
        public SayHandlerCommands(GameplayScreen gameplayScreen)
        {
            if (gameplayScreen == null)
                throw new ArgumentNullException("gameplayScreen");

          

            _gameplayScreen = gameplayScreen;
        }


        /// <summary>
        /// Gets the World that the User belongs to.
        /// </summary>
        public World World
        {
            get { return _gameplayScreen.World; }
        }

        public GameplayScreen GameplayScreen
        {
            get { return _gameplayScreen;  }
        }

        #region ISayCommands<User> Members

        /// <summary>
        /// Gets or sets the User that the current command came from.
        /// </summary>
        public Character User { get; set; }

        #endregion

    }
}