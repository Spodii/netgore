using System;
using System.Linq;
using DemoGame.Server.Guilds;
using NetGore;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;

namespace DemoGame.Server
{
    /// <summary>
    /// The actual class that handles the Say commands.
    /// </summary>
    public partial class SayHandlerCommands : ISayCommands<User>
    {
        readonly Server _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="SayHandlerCommands"/> class.
        /// </summary>
        /// <param name="server">The Server that the commands will come from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="server" /> is <c>null</c>.</exception>
        public SayHandlerCommands(Server server)
        {
            if (server == null)
                throw new ArgumentNullException("server");

            _server = server;
        }

        public IGroupManager GroupManager
        {
            get { return Server.GroupManager; }
        }

        public static GuildManager GuildManager
        {
            get { return GuildManager.Instance; }
        }

        public static GuildSettings GuildSettings
        {
            get { return GuildSettings.Instance; }
        }

        /// <summary>
        /// Gets the Server that the commands are coming from.
        /// </summary>
        public Server Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Gets the World that the User belongs to.
        /// </summary>
        public World World
        {
            get { return Server.World; }
        }

        #region ISayCommands<User> Members

        /// <summary>
        /// Gets or sets the User that the current command came from.
        /// </summary>
        public User User { get; set; }

        #endregion
    }
}